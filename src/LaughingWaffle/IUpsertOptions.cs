using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LaughingWaffle
{
    public interface IUpsertOptions<TType>
    {
        /// <summary>
        /// Set the name of the target table for this upsert. This method defaults the schema to "dbo".
        /// </summary>
        /// <param name="table">The name of the target table. </param>
        /// <returns>Instance of the parent class for method chaining.</returns>
        UpsertOptions<TType> SetTargetTable(string table);
        
        /// <summary>
        /// Set the name of the target table and schema for this upsert.
        /// </summary>
        /// <param name="schema">The schema name to use.</param>
        /// <param name="table">The name of the target table.</param>
        /// <returns>Instance of the parent class for method chaining.</returns>
        UpsertOptions<TType> SetTargetTableWithSchema(string schema, string table);
        
        /// <summary>
        /// Add a column to the MAP list. The columns that are updated or inserted into.
        /// </summary>
        /// <param name="func">The name of the column.</param>
        /// <returns>Instance of the parent class for method chaining.</returns>
        UpsertOptions<TType> AddMapColumn(Expression<Func<TType, object>> func);
        
        /// <summary>
        /// Add a column to the MATCH list. The columns used to determine equality of rows (for update vs. insert: Upsert).
        /// </summary>
        /// <param name="func">The name of the column.</param>
        /// <returns>Instance of the parent class for method chaining.</returns>
        UpsertOptions<TType> AddMatchColumn(Expression<Func<TType, object>> func);

        /// <summary>
        /// The columns to match on.
        /// </summary>
        IEnumerable<string> MatchColumns { get; }
        
        /// <summary>
        /// The columns to map (that is update, or insert data from/to)
        /// </summary>
        IEnumerable<string> MapColumns { get; }

        /// <summary>
        /// Name of the target Table for the Upsert.
        /// </summary>
        string TargetTableName { get; }
        
        /// <summary>
        /// Name of the target Schema for the Upsert.
        /// </summary>
        string TargetTableSchema { get; }
    }
}