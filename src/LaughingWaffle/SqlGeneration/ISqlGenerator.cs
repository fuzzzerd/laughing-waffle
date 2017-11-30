using System.Collections.Generic;

namespace LaughingWaffle.SqlGeneration
{
    public interface ISqlGenerator<TType>
    {
        /// <summary>
        /// Expose the table name of the underlying [TableAttribute] on the class.
        /// </summary>
        string TableName(bool tempTable);

        /// <summary>
        /// Generate (and return) the DDL statement for an arbitrary type.
        /// </summary>
        /// <returns>The DDL statements for the table based on the input paramaters.</returns>
        string CreateTable();

        /// <summary>
        /// Generate (and return) the DDL statement for an arbitrary type.
        /// </summary>
        /// <param name="tempTable">Boolean to indicate if the table should be in the 'temporary' form or not. (Table or #Table).</param>
        /// <returns>The DDL statements for the table based on the input paramaters.</returns>
        string CreateTable(bool tempTable);

        /// <summary>
        /// Get the properties for this instance type.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetProperties();

        string Merge(IUpsertOptions<TType> options);
    }
}