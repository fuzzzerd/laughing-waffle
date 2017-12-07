using LaughingWaffle.SqlGeneration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using WaffleTests.TestModels;

namespace WaffleTests
{
    public class DatabaseSetupFixture : IDisposable
    {

        private const string DbName = "laughing_waffle_test";
        public IDbConnection Connection { get; private set; }

        public DatabaseSetupFixture()
        {
            var connString = "Server=(localdb)\\mssqllocaldb;Initial Catalog=master;Integrated Security=True";

            Connection = new SqlConnection(connString);
            Connection.Open();

            InitDb();
        }

        public void Dispose()
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = $"USE master; DROP DATABASE {DbName}";
                cmd.ExecuteNonQuery();
            }
            Connection.Dispose();
        }

        private void InitDb()
        {
            using (var cmdCreate = Connection.CreateCommand())
            {
                cmdCreate.CommandText = $"IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = '{DbName}') CREATE DATABASE [{DbName}];";
                cmdCreate.ExecuteNonQuery();
            }

            void DropTable(string schema, string name)
            {
                using (var cmdDelete = Connection.CreateCommand())
                {
                    cmdDelete.CommandText = $@"IF OBJECT_ID('{schema}.{name}', 'U') IS NOT NULL DROP TABLE [{schema}].[{name}]; ";
                    cmdDelete.ExecuteNonQuery();
                }
            }

            DropTable("dbo", "StandardModels");


            using (var createStandardModelCMD = Connection.CreateCommand())
            {
                var createStandardModel = @"CREATE TABLE StandardModels (PK int IDENTITY(1,1) not null, FK int null, Name varchar(256) NOT NULL, PRIMARY KEY (PK))";
                createStandardModelCMD.CommandText = createStandardModel;
                createStandardModelCMD.ExecuteNonQuery();
            }

            // INSERT SOME Seed data for 'upsert'
            var inserts = @"INSERT INTO StandardModels (FK, Name) VALUES (200, getdate());
INSERT INTO StandardModels (FK, Name) VALUES (200, getdate());
INSERT INTO StandardModels (FK, Name) VALUES (200, getdate());";

            using (var insertStandardModels = Connection.CreateCommand())
            {
                insertStandardModels.CommandText = inserts;
                insertStandardModels.ExecuteNonQuery();
            }
        }
    }
}