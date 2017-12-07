using LaughingWaffle.SqlGeneration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Linq;

namespace LaughingWaffle
{
    public static class Extensions
    {
        public static bool Upsert<TEntity>(
            this IDbConnection conn,
            IEnumerable<TEntity> entities,
            IUpsertOptions<TEntity> options)
        {
            // setup 
            var sqlGenerator = new TSqlGenerator<TEntity>();
            var createScript = sqlGenerator.CreateTable();
            var mergeScript = sqlGenerator.Merge(options);

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

            // finalize
            return false; // maybe update this to indicate kinds of failure instead of simple true/false.
        }
    }
}