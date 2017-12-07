using LaughingWaffle.SqlGeneration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Linq;

namespace LaughingWaffle
{
    /// <summary>
    /// Extension methods that hang off the Connection class.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Upsert (insert if not matched, update if matched) a generic collection of objects.
        /// </summary>
        /// <typeparam name="TEntity">The type of entities to be updated.</typeparam>
        /// <param name="conn">The <see cref="IDbConnection"/> instance we're hanging off.</param>
        /// <param name="entities">An IEnumerable of objects to be upserted into the connection.</param>
        /// <param name="options">Bulk Upsert Options that describe how this upsert should work.</param>
        /// <returns></returns>
        public static bool Upsert<TEntity>(
            this IDbConnection conn,
            IEnumerable<TEntity> entities,
            IUpsertOptions<TEntity> options)
        {
            // setup 
            var sqlGenerator = new TSqlGenerator<TEntity>();
            var createScript = sqlGenerator.CreateTable();
            var mergeScript = sqlGenerator.Merge(options);
            var cleanUpScript = $"DROP TABLE {sqlGenerator.TableName(true)};";

            // execute
            using (var createTableCmd = conn.CreateCommand())
            {
                createTableCmd.CommandType = CommandType.Text;
                createTableCmd.CommandText = createScript;
                createTableCmd.ExecuteNonQuery();
            }

            // ==> bulk insert to temp table
            using (var copy = new SqlBulkCopy((SqlConnection)conn))
            using(var reader = FastMember.ObjectReader.Create(entities, sqlGenerator.GetProperties().ToArray()))
            {
                // setup mapping
                foreach (var prop in sqlGenerator.GetProperties())
                {
                    // force one<=>one mapping
                    copy.ColumnMappings.Add(prop, prop);
                }
                // set destination table (insert into TEMP table)
                copy.DestinationTableName = sqlGenerator.TableName(true);
                // write data from reader to server
                copy.WriteToServer(reader);
                // run the bulk insert to temp table
                copy.Close();
            }
            // <== end bulk insert to temp table

            // ==> merge upsert
            using (var trans = conn.BeginTransaction())
            using (var mergeTempAndRealCmd = conn.CreateCommand())
            {
                // ==> merge temp table to target table
                mergeTempAndRealCmd.Transaction = trans;
                mergeTempAndRealCmd.CommandType = CommandType.Text;
                mergeTempAndRealCmd.CommandText = mergeScript;
                mergeTempAndRealCmd.ExecuteNonQuery();
                // <== end merge temp table to target table
                trans.Commit();
            }
            // <== end merge upsert

            // cleanup
            using (var dropTableCmd = conn.CreateCommand())
            {
                dropTableCmd.CommandText = cleanUpScript;
                dropTableCmd.ExecuteNonQuery();
            }
            // finalize
            return true; // maybe update this to indicate kinds of failure instead of simple true/false.
        }
    }
}