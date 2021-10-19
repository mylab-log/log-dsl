# MyLab.Log.Dsl 

For .NET Core 3.1+

[![NuGet](https://img.shields.io/nuget/v/MyLab.Log.Dsl.svg)](https://www.nuget.org/packages/MyLab.Log.Dsl/)

Provides abilities to make log message by `Domain Specific Language` style. Uses built-in `.NET Core` logging. Based on [MyLab.Log](https://github.com/mylab-log/log).

There are several points about it:
* there are several log message levels which represent follows events: 
  * debug
  * action
  * warning
  * error 
* `dsl` expression should ends with `Write` method

## Review

The typically points is:

* add `DSL` into logging system

*  get `IDslLogger` with `DI`
* write `dsl` expression with special methods
  * begin from log-level method to define log-level specific parameters
  * set reason exception 
  * add several facts
  * add several labels
* call `Write()` method to write log.

The following example shows how to use `dsl` logger:

```C#
services.AddLogging(l => l.AddDsl());
```

```C#
class Example
{
    private IDslLogger _log;

    public Example(IDslLogger<Example> logger)
    {
        _log = logger.Dsl();
    }
    
    public void DoSomething()
    {
        //...
        
        _log.Action("I did it!")
            .AndFactIs("The day is rainy")
            .AndFactIs(DateTime.Now > dt)
            .AndLabel("important")
            .Write();
    }
}
```

## Log level methods

All following methods has affect on built-in logging message level.

### Action

Describes common case event.

```c#
_log.Act("I did it").Write();
```

Output:

```yaml
Message: I did it!
Time: 2021-02-24T18:35:24.142
```

### Debug

Describes debug message;

```c#
_log.Debug("Trigger was triggered").Write();
```

Output:

```yaml
Message: Trigger was triggered
Time: 2021-02-24T18:35:24.242
Labels:
  log_level: debug
```

### Warning

Describes message about some important things which can have dangerous consequences. Client request format error for example.

```c#
_log.Warning("Invalid client request").Write();
```

Output:

```yaml
Message: Invalid client request
Time: 2021-02-24T18:35:24.241
Labels:
  log_level: warning
```

### Error

Describes a server's fault which interrupts all or part of request processing.   

```C#
_log.Error("Backend server connection error").Write();
```

In console we got
```
Message: Backend server connection error
Time: 2021-02-24T18:35:24.240
Labels:
  log_level: error
```

## Facts

For each log message yoг can add several facts which will be logged. 

There are two fact types:

* named facts - has string names and any type value. 

* conditions - text value, which describes some current condition. Combines in named fact `conditions`:
  * based on text
  * based on `Expression`

Example:

```C#
int debugParameter= 1;

_log.Action("Something done")
    .AndFactIs("foo", "bar")
    .AndFactIs("The day is rainy")
    .AndFactIs(() => debugParameter > 5)
    .Write();
```

Output:

```yaml
Message: Something done
Time: 2021-02-24T20:26:27.263
Facts:
  foo: bar
  conditions:
  - The day is rainy
  - "'debugParameter > 5' is False"
```

## Labels

For each log message yoг can add several labels which will be logged.

There are two label types:

* regular named label - has name and string value
* label-flag - has name and `true` string value

Example:

```C#
_log.Action("Something done")
    .AndLabel("priority", "high")
    .AndLabel("good_message")
    .Write();
```

Output:

```yaml
Message: Something done
Time: 2021-02-24T21:02:30.467
Labels:
  priority: high
  good_message: true
```
