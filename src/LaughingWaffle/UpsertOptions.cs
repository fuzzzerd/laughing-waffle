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

        public UpsertOptions<TType> SetTargetTable(string table)
        {
            return SetTargetTableWithSchema("dbo", table);
        }

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
        /// <param name="func"></param>
        /// <returns></returns>
        public UpsertOptions<TType> AddMatchColumn(Expression<Func<TType, object>> func)
        {
            var name = GetMemberNameFromExpression(func);
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