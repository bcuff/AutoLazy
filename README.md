# AutoLazy

Post compile tool using [Fody](https://github.com/Fody/Fody) to implement the double check locking pattern.

### Works on
* static or instance members
* parameterless methods
* properties

### Example
Turns this
```c#
public class MyClass
{
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
			// thread-safe double check locking pattern generated here
			var result = _settings;
			if (result == null)
			{
				lock(_syncRoot)
				{
					if (_settings == null)
					{
						result = _settings = GetSettingsImpl();
					}
				}
			}
			return result;
		}
	}
	
	// actual implementation copied here
	private static Settings GetSettingsImpl()
	{
		using (var fs = File.Open("settings.xml", FileMode.Open))
		{
			var serializer = new XmlSerializer(typeof(Settings));
			return (Settings)serializer.Deserialize(fs);
		}
	}
}
```
