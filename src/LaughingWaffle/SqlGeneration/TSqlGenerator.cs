using System.Collections.Generic;

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
    }
}