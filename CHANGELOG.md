# Changelog

All notable changes to this project will be documented in this file

Log format based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## [3.4.3] - 2021-11-11

### Added

* A log context object can be added into log builder with `AddDslCtx` method

## [3.3.3] - 2021-11-09

### Fix

* `ILogger.Dsl()` previously threw an error

## [3.3.2] - 2021-11-01

### Changed

* Move log context injection from app services into logging builder 
* Change log contexts lifetime from scope to singleton

## [3.2.2] - 2021-10-20

### Added

* DI injection
* Support context appliers

### Changed

* `ILogger.Dsl()` has become obsolete

## [3.1.2] - 2021-07-17

### Changed

* Remove exception logging because it writes to console addition to log content

## [3.1.1] - 2021-06-22

### Changed

* Remove old `MyLab.Logging` reference

## [3.0.1] - 2021-05-12

### Added

- This changelog

### Changed

* Renaming `MyLab.LogDsl` -> `MyLab.Log.Dsl`
* Full refactoring