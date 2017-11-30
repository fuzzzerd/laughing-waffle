using LaughingWaffle.SqlGeneration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace LaughingWaffle
{
    public static class Extensions
    {
        public static bool Upsert<TEntity>(
            this IDbConnection conn,
            IEnumerable<TEntity> entities)
        {
            // setup 
            var sqlGenerator = new TSqlGenerator<TEntity>();
            var createScript = sqlGenerator.CreateTable();

            // execute
            var createTableCmd = conn.CreateCommand();
            createTableCmd.CommandType = CommandType.Text;
            createTableCmd.CommandText = createScript;
            createTableCmd.ExecuteNonQuery();

            // ==> bulk insert to temp table
            using (var copy = new SqlBulkCopy((SqlConnection)conn))
            {
                // setup mapping

                // run the merge
            }
            // <== end bulk insert to temp table

            using (var trans = conn.BeginTransaction())
            {
                // ==> merge temp table to target table
                //var mergeTempAndRealCmd = conn.CreateCommand();
                //mergeTempAndRealCmd.CommandType = CommandType.Text;
                //mergeTempAndRealCmd.CommandText = mergeTempAndRealCmd;
                //mergeTempAndRealCmd.ExecuteNonQuery();
                // <== end merge temp table to target table
            }

            // cleanup

            // finalize
            return false; // maybe update this to indicate kinds of failure instead of simple true/false.
        }
    }
}