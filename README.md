# MyLab.LogDsl 

For .NET Standard 2.0+, .NET Framework 4.6.1+, .NET Core 2.0+

[![NuGet](https://img.shields.io/nuget/v/MyLab.LogDsl.svg)](https://www.nuget.org/packages/MyLab.LogDsl/)

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
      I did it
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
      I did debug
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
      Something wrong
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
      Id: 862ce7cd-f37c-43b0-89a0-136f443a31ac
      Markers: debug
      Debug password: very secret password
      Conditions: I'm tired, 'debugParameter1 > 5' is False, 'debugParameter2 > 5' is True
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

## Rich Exception

At middle place where exception pass through, you can add conditions and markers for it. And those additional parameters will fall in log:

```C#
TopLevelActon();

void BottomLevelAction()
{
    throw new Exception("SQL server error")
        .AndMarkAs("db-error");
}

void MiddleLevelAction()
{
    try
    {
        BottomLevelAction();
    }
    catch (Exception e)
    {
        e.AndFactIs("userId", 100)
            .AndMarkAs("vip-client");
        throw;
    }
}

void TopLevelActon()
{
    try
    {
        MiddleLevelAction();
    }
    catch (Exception e)
    {
        _log.Error(e)
            .AndFactIs("ip", "90.109.220.01")
            .Write();
    }
}
```

Console output:

```
fail: Demo.Example[0]
      SQL server error
      Id: 5c34b1b9-49c9-4e5b-ab0f-4c5e33cc89b2
      Markers: error, db-error, vip-client
      userId: 100
      ip: 90.109.220.01

System.Exception: SQL server error
   at Demo.Example.<Example7_WithExceptionParameters>g__BottomLevelAction|8_0() in D:\Projects\my\mylab-log-dsl\MyLab.LogDsl\Demo\Example.cs:line 73
   at Demo.Example.<Example7_WithExceptionParameters>g__MiddleLevelAction|8_1() in D:\Projects\my\mylab-log-dsl\MyLab.LogDsl\Demo\Example.cs:line 81
   at Demo.Example.<Example7_WithExceptionParameters>g__TopLevelActon|8_2() in D:\Projects\my\mylab-log-dsl\MyLab.LogDsl\Demo\Example.cs:line 95
```



## PS

The max profit you can get when using [MyLab.LogYml](https://github.com/ozzy-ext/mylab-log-yml) together.

All examples above you can find in `Demo` project.
