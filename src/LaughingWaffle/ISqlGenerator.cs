namespace LaughingWaffle
{
    public interface ISqlGenerator
    {
        /// <summary>
        /// Given a class type, generate the Create Sql Statement for it. 
        /// Default Behavior is to create a #TempTable. Same as calling CreateTable<T>(true);
        /// </summary>
        /// <param name="tempTable">Boolean indicating if this table should be a #TempTable or a real table.</param>
        /// <returns>The DDL statements for the table based on the input paramaters.</returns>
        string CreateTable<TType>();

        /// <summary>
        /// Given a class type, generate the Create Sql Statement for it.
        /// </summary>
        /// <typeparam name="TType">The type to reflect against and pull public properties for table fields.</typeparam>
        /// <param name="tempTable">Boolean indicating if this table should be a #TempTable or a real table.</param>
        /// <returns>The DDL statements for the table based on the input paramaters.</returns>
        string CreateTable<TType>(bool tempTable);
    }
}