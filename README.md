# Laughing Waffle

Laughing Waffle is a helper library for doing bulk insert and upate (read upsert) work with SQL Server. Specifically providing help and code generation around the [MERGE Statement](https://docs.microsoft.com/en-us/sql/t-sql/statements/merge-transact-sql).

It is targeting .NET Standard v1.3 for wide compatibility across the .NET ecosystem.

| Build Status | Activity | MyGet Downloads | NuGet Downloads | License |
|---|---|---|---|---|
| [![LaughingWaffle Duild Status](https://www.myget.org/BuildSource/Badge/laughing-waffle?identifier=cfe06860-e514-4595-aeff-6bb1f7a2e974)](https://www.myget.org/feed/Packages/laughing-waffle) | [![GitHub last commit](https://img.shields.io/github/last-commit/fuzzzerd/laughing-waffle.svg?style=flat-square)]() | [![MyGet Downloads](https://img.shields.io/myget/laughing-waffle/dt/LaughingWaffle.svg?style=flat-square)](https://www.myget.org/feed/laughing-waffle/package/nuget/LaughingWaffle) | coming soon | [![license](https://img.shields.io/github/license/fuzzzerd/laughing-waffle.svg?style=flat-square)](https://github.com/fuzzzerd/laughing-waffle/blob/master/LICENSE) |

## What is Laughing Waffle

This library is intended to make Extract, Transform, and Load jobs easier. Other tools make both extracting the data easy as well as transforming it from one format to another, but there was no package for helping insert/update on the target side.

## Basic Usage

```csharp
// your collection that needs updated
var toUpsert = new List<YourModel>();

// upsert options
var upsertOptions = new UpsertOptions<YourModel>()
    .AddMatchColumn(p => p.PrimaryKey)
    .AddMapColumn(p => p.Column1)
    .AddMapColumn(p => p.Column2)
    .SetTargetTable("YourModelTable");

// use of extension methods on connections
_connection.Upsert(toUpsert, upsertOptions);
```

### Repository Statistics
[![GitHub commit activity the past week, 4 weeks, year](https://img.shields.io/github/commit-activity/y/fuzzzerd/laughing-waffle.svg?style=flat-square)]()

[![GitHub issues](https://img.shields.io/github/issues/fuzzzerd/laughing-waffle.svg?style=flat-square)]()

[![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/fuzzzerd/laughing-waffle.svg?style=flat-square)]()

[![GitHub language count](https://img.shields.io/github/languages/count/fuzzzerd/laughing-waffle.svg?style=flat-square)]()