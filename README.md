# MyLab.LogDsl 

For .NET Core 2.1+

Provides abilities to make log message by `Domain Specific Language` style. Uses buil-in `.NET Core` logging.

There are several points about it:
* there are three log message types which represent follows events: action, debug, error
* every log message have unique identifier (Guid)
* dsl composition should ends with `Write` method

## Beginning

When get build-in logger in constructor as dependency, just convert it into `DslLogger`

```C#
class Example
{
    private DslLogger _log;

    public Example(ILogger<Example> logger)
    {
        _log = logger.Dsl();
    }
}
```

Then you can write one of follows log messages:
* information about action  
* debug information
* occured error information

## Act

Let's write `act` log message:

```C#
_log.Act("I did it").Write();
```

In console we got
```
info: Demo.Example[0]
      I did it.
      Id: a8ba4eae-246c-4f35-84fb-6041d806aec7
```

## Debug

The primary difference between 'Debug' and 'Act' is a special marker for `Debug`. Let's write `debug` log message:

```C#
_log.Debug("I did debug").Write();
```

Console output:

```
dbug: Demo.Example[0]
      I did debug.
      Id: 108c3b58-22b8-4bb0-8b29-1f547cdfaead
      Markers: debug
```

## Error

To write simple error log message you can use the same way as with `act` and `debug`:

``` C#
 _log.Error("Something wrong").Write();
```

Console output:

```
fail: Demo.Example[0]
      Something wrong.
      Id: e1f68823-6fdd-4c48-904d-77a2864109af
      Markers: error
```
Please note the error-marker.

In error case you can also specify exception as reason of event:

```C#
Exception exception;
try
{
    throw new NullReferenceException();
}
catch (Exception e)
{
    exception = e;
}

_log.Error(exception).Write();
```
Console output:

```
fail: Demo.Example[0]
      Object reference not set to an instance of an object.
      Id: f90f336d-57a9-4ba3-abb0-eaaeb9f46408
      Markers: error

System.NullReferenceException: Object reference not set to an instance of an object.
   at Demo.Example.Example4_ErrorWithException() in D:\Projects\my\mylab-log-dsl\MyLab.LogDsl\Demo\Example.cs:line 36

```

## Facts

For each log message yoг can add several facts which will fall in log:

```C#
int debugParameter1 = 1;
int debugParameter2 = 10;

_log.Debug("I did debug")
    .AndFactIs("I'm tired")
    .AndFactIs("Debug password", "very secret password")
    .AndFactIs(() => debugParameter1 > 5)
    .AndFactIs(() => debugParameter2 > 5)
    .Write();
```

Console output:

```
dbug: Demo.Example[0]
      I did debug
      Id: 2b931e6d-9243-49e3-9eab-16ddea52ad29
      Markers: debug
      Conditions: I'm tired, 'debugParameter1 > 5' is False, 'debugParameter2 > 5' is True
      Debug password: very secret password
```

## Markers

For each log message yoг can add several markers which will fall in log:

```C#
_log.Act("I did it")
      .AndMarkAs("important")
      .Write();
```

Console output:

```
info: Demo.Example[0]
      I did it
      Id: 53ec21bd-5dfa-400f-993a-2f3b0c97c57f
      Markers: important
```

## PS

The max profit you can get when using [MyLab.LogYml](https://github.com/ozzy-ext/mylab-log-yml) together.

All examples above sored in `Demo` project.
