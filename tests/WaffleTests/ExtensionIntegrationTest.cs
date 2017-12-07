using LaughingWaffle;
using System;
using System.Collections.Generic;
using System.Text;
using WaffleTests.TestModels;
using Xunit;

namespace WaffleTests
{
    public class ExtensionIntegrationTest : IClassFixture<DatabaseSetupFixture>
    {
        DatabaseSetupFixture _fixture;

        public ExtensionIntegrationTest(DatabaseSetupFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName ="Integration Merge Updat Existing Rows")]
        public void TestUpsert()
        {
            var toInsert = new List<StandardModel>();
            toInsert.Add(new StandardModel { PK = 1, FK = 1000, Name = DateTime.Now.ToString("s") });
            toInsert.Add(new StandardModel { PK = 2, FK = 1002, Name = DateTime.Now.ToString("s") });
            toInsert.Add(new StandardModel { PK = 3, FK = 1003, Name = DateTime.Now.ToString("s") });
            toInsert.Add(new StandardModel { PK = 103, FK = 1004, Name = DateTime.Now.ToString("s") });

            var upsertOptions = new UpsertOptions<StandardModel>()
                .AddMatchColumn(p => p.PK)
                .AddMapColumn(p => p.FK)
                .AddMapColumn(p => p.Name)
                .SetTargetTableWithSchema("dbo", "StandardModels");

            _fixture.Connection.Upsert(toInsert, upsertOptions);

            int actual;
            using (var verifyCmd = _fixture.Connection.CreateCommand())
            {
                verifyCmd.CommandText = "SELECT COUNT(*) FROM dbo.StandardModels";
                actual = (int)verifyCmd.ExecuteScalar();
            }

            Assert.Equal(4, actual);
        }
    }
}
