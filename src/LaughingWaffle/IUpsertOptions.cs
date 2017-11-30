using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LaughingWaffle
{
    public interface IUpsertOptions<TType>
    {
        UpsertOptions<TType> SetTargetTable(string table);
        UpsertOptions<TType> SetTargetTableWithSchema(string schema, string table);
        UpsertOptions<TType> AddMapColumn(Expression<Func<TType, object>> func);
        UpsertOptions<TType> AddMatchColumn(Expression<Func<TType, object>> func);

        IEnumerable<string> MatchColumns { get; }
        IEnumerable<string> MapColumns { get; }

        string TargetTableName { get; }
        string TargetTableSchema { get; }
    }
}