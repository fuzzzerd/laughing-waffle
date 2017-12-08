# Laughing Waffle

Laughing Waffle is a helper library for doing bulk insert and upate (read upsert) work with SQL Server. Specifically providing help and code generation around the [MERGE Statement](https://docs.microsoft.com/en-us/sql/t-sql/statements/merge-transact-sql).

It is targeting .NET Standard v1.3 for wide compatibility across the .NET ecosystem.

| Build Status | MyGet Downloads | NuGet Downloads |
|---|---|---|
| [![LaughingWaffle Duild Status](https://www.myget.org/BuildSource/Badge/laughing-waffle?identifier=cfe06860-e514-4595-aeff-6bb1f7a2e974)](https://www.myget.org/feed/Packages/laughing-waffle) | [![MyGet Downloads](https://img.shields.io/myget/laughing-waffle/dt/LaughingWaffle.svg?style=flat-square)](https://www.myget.org/feed/laughing-waffle/package/nuget/LaughingWaffle) | coming soon |

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