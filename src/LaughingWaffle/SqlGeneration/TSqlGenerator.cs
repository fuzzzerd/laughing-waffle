using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaughingWaffle.SqlGeneration
{
    public class TSqlGenerator<TType> : GeneratorBase<TType>
    {
        /// <summary>
        /// Reference: https://stackoverflow.com/a/5873231/86860
        /// Reversed from the link
        /// </summary>
        protected override Dictionary<string, string> CsharpTypeToSqlType => new Dictionary<string, string>
        {
            { "Guid", "uniqueidentifier" },
            { "Int32", "int" },
            { "Byte", "tinyint" },
            { "Byte[]", "binary" },
            { "Int64", "bigint" },
            { "String", "nvarchar" },
            { "Boolean", "bit" },
            { "DateTime", "datetime" },
            { "DateTimeOffset", "datetimeoffset" },
            { "Single", "float" },
            { "Decimal", "decimal" },
            { "TimeSpan", "time" },
            { "Double", "real" }
        };

        /// <summary>
        /// Generates a TSQL Merge Statement for the provided options.
        /// </summary>
        /// <param name="options">Upsert Options that specify how the merge should be built.</param>
        /// <returns></returns>
        public override string Merge(IUpsertOptions<TType> options)
        {
            // reference: https://stackoverflow.com/a/14806962/86860

            var builder = new StringBuilder();

            builder.Append($"MERGE INTO {options.TargetTableSchema}.{options.TargetTableName}");
            builder.Append(" WITH (HOLDLOCK) AS target\r\n");

            builder.Append($"USING {TableName(true)} AS source\r\n");
            builder.Append("ON ");
            foreach (var match in options.MatchColumns)
            {
                builder.Append($"target.{match} = source.{match}");
                if (match == options.MatchColumns.Last())
                {
                    // on the last one, end the line
                    builder.Append("\r\n");
                }
                else
                {
                    // AND for the next parm
                    builder.Append(" AND ");
                }
            }
            builder.Append("WHEN MATCHED THEN\r\n");
            // updates
            builder.Append("UPDATE SET ");
            foreach (var map in options.MapColumns)
            {
                builder.Append($"target.{map} = source.{map}");
                if (map == options.MapColumns.Last())
                {
                    // on the last one, end the line
                    builder.AppendLine("");
                }
                else
                {
                    // AND for the next parm
                    builder.Append(", ");
                }
            }
            builder.Append("WHEN NOT MATCHED BY target THEN\r\n");
            //insert
            builder.Append($"INSERT ({string.Join(", ", options.MapColumns)})\r\n");
            builder.Append($"VALUES (source.{string.Join(", source.", options.MapColumns)})\r\n");

            //https://docs.microsoft.com/en-us/sql/t-sql/statements/merge-transact-sql Remarks
            builder.Append(";"); // final semicolon for MERGE statament
            return builder.ToString();
        }
    }
}