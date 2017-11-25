using LaughingWaffle.SqlGeneration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
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
            ISqlGenerator sqlGenerator = new TSqlGenerator<TEntity>();
            var createScript = sqlGenerator.CreateTable();

            // execute
            conn.Execute(createScript); // create temp table

            // ==> bulk insert to temp table
            using (var copy = new SqlBulkCopy((SqlConnection)conn))
            {
                // setup mapping

                // run the merge
            }
            // <== end bulk insert to temp table

            // ==> merge temp table to target table
            // TODO: implement
            // <== end merge temp table to target table

            // cleanup

            // finalize
            return false; // maybe update this to indicate kinds of failure instead of simple true/false.
        }
    }
}