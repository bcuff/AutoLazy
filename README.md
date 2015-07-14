# AutoLazy

Post compile tool using [Fody](https://github.com/Fody/Fody) to implement the double-checked locking pattern.

### The nuget package  [![NuGet Status](http://img.shields.io/nuget/v/AutoLazy.Fody.svg?style=flat)](https://www.nuget.org/packages/AutoLazy.Fody/)

https://nuget.org/packages/AutoLazy.Fody/

    PM> Install-Package AutoLazy.Fody

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
			// thread-safe double-checked locking pattern generated here
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
