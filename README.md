# AutoLazy

Post compile tool using [Fody](https://github.com/Fody/Fody) to implement the double-checked locking pattern.

### The nuget package  [![NuGet Status](http://img.shields.io/nuget/v/AutoLazy.Fody.svg?style=flat)](https://www.nuget.org/packages/AutoLazy.Fody/)

https://nuget.org/packages/AutoLazy.Fody/

    PM> Install-Package AutoLazy.Fody

### Works on
* static or instance members
* parameterless methods
* properties
* methods with a single parameter *(new)*

### Contributing

Please read the [guidelines](./CONTRIBUTING.md) before submitting any changes. Thank you!

### Example
Turns this
```c#
public class MyClass
{
	// This would work as a method, e.g. GetSettings(), as well.
	[Lazy]
	public static Settings Settings
	{
		get
		{
			using (var fs = File.Open("settings.xml", FileMode.Open))
			{
				var serializer = new XmlSerializer(typeof(Settings));
				return (Settings)serializer.Deserialize(fs);
			}
		}
	}

	[Lazy]
	public static Settings GetSettingsFile(string fileName)
	{
		using (var fs = File.Open(fileName, FileMode.Open))
		{
			var serializer = new XmlSerializer(typeof(Settings));
			return (Settings)serializer.Deserialize(fs);
		}
	}
}
```

Into something equivalent to this
```c#
public class MyClass
{
	// begin - fields added by the post-compile step
	private static readonly object _syncRoot = new object();
	private static volatile Settings _settings;
	// end
	
	[Lazy]
	public static Settings Settings
	{
		get
		{
			// thread-safe double-checked locking pattern generated here
			var result = _settings;
			if (result == null)
			{
				lock(_syncRoot)
				{
					result = _settings;
					if (result == null)
					{
						using (var fs = File.Open("settings.xml", FileMode.Open))
						{
							var serializer = new XmlSerializer(typeof(Settings));
							result = (Settings)serializer.Deserialize(fs);
							_settings = result;
						}
					}
				}
			}
			return result;
		}
	}

	// begin - fields added by post-compile step
	private static readonly object _getSettingsFileSyncRoot = new object();
	private static volatile Dictionary<string, Settings> _getSettingsFileCache
		= new Dictionary<string, Settings>();
	// end

	[Lazy]
	public static Settings GetSettingsFile(string fileName)
	{
		Settings result;
		if (!_getSettingsFileCache.TryGetValue(fileName, out result)) // volatile read
		{
			lock (_getSettingsFileSyncRoot)
			{
				if (!_getSettingsFileCache.TryGetValue(fileName, out result))
				{
					using (var fs = File.Open(fileName, FileMode.Open))
					{
						var serializer = new XmlSerializer(typeof(Settings));
						result = (Settings)serializer.Deserialize(fs);
					}
					// note - we can't mutate the dictionary after
					// exposing it to reads (possibly from other threads)
					// therefore, we have to copy the dictionary into a new one
					// then expose it by setting _getSettingsFileCache
					var newCache = new Dictionary<string, Settings>(_getSettingsFileCache.Count + 1);
					newCache.Add(fileName, result);
					foreach (var pair in _getSettingsFileCache)
					{
						newCache.Add(pair.Key, pair.Value);
					}
					_getSettingsFileCache = newCache; // volatile write
				}
			}
		}
		return result;
	}

}
```
