using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WaffleTests")]

namespace LaughingWaffle
{
    public class UpsertOptions<TType> : IUpsertOptions<TType>
    {
        internal List<string> _mapColumns;
        internal List<string> _matchColumns;

        public IEnumerable<string> MatchColumns => _matchColumns;
        public IEnumerable<string> MapColumns => _mapColumns;

        /// <summary>
        /// Set the name of the target table for this upsert. This method defaults the schema to "dbo".
        /// </summary>
        /// <param name="table">The name of the target table. </param>
        /// <returns>Instance of the parent class for method chaining.</returns>
        public UpsertOptions<TType> SetTargetTable(string table)
        {
            return SetTargetTableWithSchema("dbo", table);
        }

        /// <summary>
        /// Set the name of the target table and schema for this upsert.
        /// </summary>
        /// <param name="schema">The schema name to use.</param>
        /// <param name="table">The name of the target table.</param>
        /// <returns>Instance of the parent class for method chaining.</returns>
        public UpsertOptions<TType> SetTargetTableWithSchema(string schema, string table)
        {
            TargetTableName = table;
            TargetTableSchema = schema;
            return this;
        }

        public string TargetTableName { get; private set; }

        public string TargetTableSchema { get; private set; }

        public UpsertOptions()
        {
            _mapColumns = new List<string>();
            _matchColumns = new List<string>();
        }

        /// <summary>
        /// Adds a column to the list of columns to match on in the merge statement. Typically the primary key, but an arbitrary number are supported for composite relationships.
        /// </summary>
        /// <param name="func">Name of the property or field to be matched on.</param>
        /// <returns></returns>
        public UpsertOptions<TType> AddMatchColumn(Expression<Func<TType, object>> func)
        {
            var name = GetMemberNameFromExpression(func);
            return AddMatchColumn(name);
        }

        /// <summary>
        /// Adds a column to the list of columns to match on in the merge statement. Typically the primary key, but an arbitrary number are supported for composite relationships.
        /// </summary>
        /// <param name="name">Name of the property or field to be matched on.</param>
        /// <returns></returns>
        public UpsertOptions<TType> AddMatchColumn(string name)
        {
            _matchColumns.Add(name);
            return this;
        }

        /// <summary>
        /// Adds a column the internal list of columns to be mapped in the eventual merge statement.
        /// </summary>
        /// <param name="func">The name of the property/field to be merged.</param>
        /// <returns>Current instance of the <see cref="UpsertOptions"/> for fluent call chaining.</returns>
        public UpsertOptions<TType> AddMapColumn(Expression<Func<TType, object>> func)
        {
            var name = GetMemberNameFromExpression(func);
            return AddMapColumn(name);
        }

        /// <summary>
        /// Adds a column the internal list of columns to be mapped in the eventual merge statement.
        /// </summary>
        /// <param name="name">The name of the property/field to be merged.</param>
        /// <returns>Current instance of the <see cref="UpsertOptions"/> for fluent call chaining.</returns>
        public UpsertOptions<TType> AddMapColumn(string name)
        {
            _mapColumns.Add(name);
            return this;
        }

        private string GetMemberNameFromExpression(Expression<Func<TType, object>> func)
        {
            // found at https://stackoverflow.com/a/2916344/86860
            MemberExpression body = func.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)func.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }
    }
}