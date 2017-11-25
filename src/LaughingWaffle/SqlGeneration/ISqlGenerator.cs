namespace LaughingWaffle.SqlGeneration
{
    public interface ISqlGenerator
    {
        /// <summary>
        /// Expose the table name of the underlying [TableAttribute] on the class.
        /// </summary>
        string TableName { get; }
        /// <summary>
        /// Expose the table schema of the underlying [TableAttribute] on the class.
        /// </summary>
        string TableSchema { get; }


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

    }
}