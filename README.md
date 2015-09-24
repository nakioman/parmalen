Parmalen Engine
===================

Parmalen is a speech to commands engine written in **c#** that uses [Wit.ai](http://wit.ai) for natural language understanding. 

It uses [MEF](https://msdn.microsoft.com/en-us/library/dd460648%28VS.100%29.aspx) and [Autofac](http://autofac.org/) 
to load plugins on demand in execution time.

Requisites
-------------
* Visual Studio 2015


Plugins
-------
To create new plugins the interface **IIntent** from the assembly **Parmalen.Contracts** must be implemented, the plugin should look like:
```c#
[Export(typeof(IIntent))]
[Name("example")] //The "example" name is the Intent name from Wit
public class ExampleIntent : IIntent
{
}
```

The following plugins are already included.

1. **Weather** (currently only works in Spanish):
    * Gets info from [OpenWeatherMap](http://openweathermap.org/)
    * Gets current location using [telize](http://www.telize.com/) Geo IP API
    * [eSpeak](http://espeak.sourceforge.net/) for text to speech the weather to the user

Input streams
------------
The Engine uses [Sox](http://sox.sourceforge.net/) as an Input Stream, the input stream is customizable implementing the interface **IStreamRecord**
from the assembly **Parmalen.Contracts**, as an example the input stream should look like:
```c#
[Export(tyepof(IStreamRecord))]
[Name("example")] //The "example" name is the name of the stream created
public class ExampleStream
{
}
```
Then the **App.config** of the Engine must be change to use the new stream Input.
```xml
<parmalen maxRecordTime="25" witAccessToken="CHANGEME" streamRecordType="example" />
```