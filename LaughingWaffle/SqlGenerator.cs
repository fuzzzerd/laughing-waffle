using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Dapper.Upsert
{
    public class SqlGenerator
    {
        public SqlGenerator() { }

        public string CreateTable<TType>()
        {
            var builder = new System.Text.StringBuilder();

            var entityType = typeof(TType);
            var entityTypeInfo = entityType.GetTypeInfo();

            var tableAttribute = entityTypeInfo.GetCustomAttribute<TableAttribute>();

            var tableName = tableAttribute != null ? tableAttribute.Name : entityTypeInfo.Name;
            var tableSchema = tableAttribute != null ? tableAttribute.Schema : string.Empty;

            var allProperties = entityType.GetProperties().Where(q => q.CanWrite);

            builder.AppendLine($@"CREATE TABLE {tableName} (");

            foreach(var prop in allProperties)
            {
                var name = prop.Name;
                var csharpType = GetCoreType(prop.PropertyType).Name;
                var tsqlType = CsharpToTsql[csharpType];
                var nulle = Nullable.GetUnderlyingType(prop.PropertyType) != null ? "NULL" : "NOT NULL";
                builder.AppendLine($"[{name}] [{tsqlType}] {nulle}");                
            }

            builder.Append(")");

            return builder.ToString();
        }

        public static Type GetCoreType(Type type)
        {
            if (type.GetTypeInfo().IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return Nullable.GetUnderlyingType(type);
            else
                return type;
        }

        /// <summary>
        /// Reference: https://stackoverflow.com/a/5873231/86860
        /// Reversed from the link
        /// </summary>
        private Dictionary<string, string> CsharpToTsql => new Dictionary<string, string> {
            { "Guid", "uniqueidentifer" },
            { "int", "int" },
            { "Int32", "int" },
            { "byte", "tinyint" },
            { "long", "bigint" },
            { "Int64", "bigint" },
            { "String", "nvarchar(max)" },
            { "bool", "bit" },
            { "byte[]", "binary" },
            { "DateTime", "datetime" },
            { "DateTimeOffset", "datetimeoffset" },
            { "float", "float" },
            { "decimal", "decimal" },
            { "TimeSpan", "time" },
            { "Double", "real" }
        };
    }
}