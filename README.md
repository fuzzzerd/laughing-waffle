# Laughing Waffle

Laughing Waffle is SQL Merge statement helper library targeting .NET Standard v1.3 for wide compatibility across the .NET ecosystem.

[![laughing-waffle MyGet Build Status](https://www.myget.org/BuildSource/Badge/laughing-waffle?identifier=cfe06860-e514-4595-aeff-6bb1f7a2e974)](https://www.myget.org/feed/Packages/laughing-waffle)

## What is Laughing Waffle

This library is intended to make Extract, Transform, and Load workloads easier. This package specifically focuses on the Load portion. When you're trying to **load** data from somehwere *into* your system using the SQL Merge statement.

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