using LaughingWaffle.SqlGeneration;
using WaffleTests.TestModels;
using Xunit;
using System;

namespace LaughingWaffle.Tests
{
    public class SqlGeneratorTests
    {
        [Fact(DisplayName = "Table Name Should Match Attribute")]
        public void TableNameShouldMatch()
        {
            // arrange
            var instance = new TSqlGenerator<StandardModel>();
            var expected = $@"StandardModels";

            // act
            var actual = instance.TableName(false);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Temp Table Name Should Match")]
        public void TempTableShouldMatch()
        {
            // arrange
            var instance = new TSqlGenerator<StandardModel>();
            var expected = $@"#StandardModels";

            // act
            var actual = instance.TableName(true);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Test Non Temp Table")]
        public void TestNonTempTableGeneration()
        {
            // arrange
            var instance = new TSqlGenerator<StandardModel>();
            var expected = $@"CREATE TABLE StandardModels ("
            + Environment.NewLine
            +"[PK] [int] NOT NULL,"
            + Environment.NewLine
            +"[FK] [int] NULL,"
            + Environment.NewLine
            +"[Name] [nvarchar](max) NOT NULL"
            + Environment.NewLine
            +")";

            // act
            var actual = instance.CreateTable(false);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Test Temp Table Implicit/Default")]
        public void TestTempTableGeneration()
        {
            // arrange
            var instance = new TSqlGenerator<StandardModel>();
            var expected = $@"CREATE TABLE #StandardModels ("
            + Environment.NewLine
            + "[PK] [int] NOT NULL,"
            + Environment.NewLine
            + "[FK] [int] NULL,"
            + Environment.NewLine
            + "[Name] [nvarchar](max) NOT NULL"
            + Environment.NewLine
            + ")";

            // act
            var actual = instance.CreateTable();

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Test Temp Table Explicitly")]
        public void TestTempTableGenerationExplicit()
        {
            // arrange
            var instance = new TSqlGenerator<StandardModel>();
            var expected = $@"CREATE TABLE #StandardModels ("
            + Environment.NewLine
            + "[PK] [int] NOT NULL,"
            + Environment.NewLine
            +"[FK] [int] NULL,"
            + Environment.NewLine
            +"[Name] [nvarchar](max) NOT NULL"
            + Environment.NewLine
            +")";

            // act
            var actual = instance.CreateTable(true);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Test Null and Not Null Ints")]
        public void TestNullAndNotNullInts()
        {
            // arrange
            var instance = new TSqlGenerator<TestNullAndNotNullInt>();
            var expected = $@"CREATE TABLE TestNullAndNotNullInts ("
            + Environment.NewLine
            + "[PK] [int] NOT NULL,"
            + Environment.NewLine
            + "[FK] [int] NULL"
            + Environment.NewLine
            + ")";

            // act
            var actual = instance.CreateTable(false);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Test All The Type Mappings")]
        public void TestEveryType()
        {
            // arrange
            var instance = new TSqlGenerator<EachTypeWeHave>();
            var expected = $@"CREATE TABLE EachTypeWeHave ("
            + Environment.NewLine
            +"[PK] [int] NOT NULL,"
            + Environment.NewLine
            +"[FK] [int] NULL,"
            + Environment.NewLine
            +"[theGuid] [uniqueidentifier] NOT NULL,"
            + Environment.NewLine
            +"[nullGuid] [uniqueidentifier] NULL,"
            + Environment.NewLine
            +"[Name] [nvarchar](max) NOT NULL,"
            + Environment.NewLine
            +"[Dt] [datetime] NOT NULL,"
            + Environment.NewLine
            +"[NullableDt] [datetime] NULL,"
            + Environment.NewLine
            +"[d] [real] NOT NULL,"
            + Environment.NewLine
            +"[dNull] [real] NULL,"
            + Environment.NewLine
            +"[longNumber] [bigint] NOT NULL,"
            + Environment.NewLine
            +"[i64Number] [bigint] NOT NULL,"
            + Environment.NewLine
            +"[er] [float] NOT NULL,"
            + Environment.NewLine
            +"[dec] [decimal] NOT NULL,"
            + Environment.NewLine
            +"[theBIT] [bit] NOT NULL,"
            + Environment.NewLine
            +"[theBOOLEAN] [bit] NOT NULL,"
            + Environment.NewLine
            +"[chewer] [tinyint] NOT NULL,"
            + Environment.NewLine
            +"[bigChewer] [tinyint] NOT NULL,"
            + Environment.NewLine
            +"[binary] [binary] NOT NULL,"
            + Environment.NewLine
            +"[offset] [datetimeoffset] NOT NULL"
            + Environment.NewLine
            +")";
            // act
            var actual = instance.CreateTable(false);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Generated Merge Statement Should Be Correct")]
        public void GeneratedMergeStatementShouldBeCorrect()
        {
            // arrange
            var instance = new TSqlGenerator<StandardModel>();
            var expected = $@"MERGE INTO dbo.StandardModels WITH (HOLDLOCK) AS target"
            + Environment.NewLine
            +"USING #StandardModels AS source"
            + Environment.NewLine
            +"ON target.PK = source.PK"
            + Environment.NewLine
            +"WHEN MATCHED THEN"
            + Environment.NewLine
            +"UPDATE SET target.Name = source.Name"
            + Environment.NewLine
            +"WHEN NOT MATCHED BY target THEN"
            + Environment.NewLine
            +"INSERT (Name)"
            + Environment.NewLine
            +"VALUES (source.Name)"
            + Environment.NewLine
            +";"; // final new line

            // act
            var actual = instance.Merge(new UpsertOptions<StandardModel>()
                .SetTargetTable("StandardModels")
                .AddMatchColumn(o => o.PK)
                .AddMapColumn(p => p.Name)
            );

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Generated Merge Should Show Mapped Columns")]
        public void GeneratedMergeShouldShowMappedColumns()
        {
            // arrange
            var instance = new TSqlGenerator<StandardModel>();
            var expected = $@"MERGE INTO dbo.StandardModels WITH (HOLDLOCK) AS target"
            + Environment.NewLine
            +"USING #StandardModels AS source"
            + Environment.NewLine
            +"ON target.PK = source.PK"
            + Environment.NewLine
            +"WHEN MATCHED THEN"
            + Environment.NewLine
            +"UPDATE SET target.PK = source.PK, target.FK = source.FK, target.Name = source.Name"
            + Environment.NewLine
            +"WHEN NOT MATCHED BY target THEN"
            + Environment.NewLine
            +"INSERT (PK, FK, Name)"
            + Environment.NewLine
            +"VALUES (source.PK, source.FK, source.Name)"
            + Environment.NewLine
            +";"; // final new line

            // act
            var actual = instance.Merge(new UpsertOptions<StandardModel>()
                .SetTargetTable("StandardModels")
                .AddMatchColumn(o => o.PK)
                .AddMapColumn(p => p.PK)
                .AddMapColumn(p => p.FK)
                .AddMapColumn(p => p.Name)
            );

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Generated Merge Should Show All Columns")]
        public void MergeWithAllMapped()
        {
            // arrange
            var instance = new TSqlGenerator<TestModel1>();
            var expected = $@"MERGE INTO dbo.TestModel1 WITH (HOLDLOCK) AS target"
            + Environment.NewLine
            +"USING #TestModel1 AS source"
            + Environment.NewLine
            +"ON target.TmId = source.TmId"
            + Environment.NewLine
            +"WHEN MATCHED THEN"
            + Environment.NewLine
            +"UPDATE SET target.TmId = source.TmId, target.TfkId = source.TfkId, target.ModifiedDate = source.ModifiedDate, target.PSI = source.PSI, target.ModifiedBy = source.ModifiedBy, target.CreatedBy = source.CreatedBy, target.Current = source.Current, target.CreatedDate = source.CreatedDate"
            + Environment.NewLine
            +"WHEN NOT MATCHED BY target THEN"
            + Environment.NewLine
            +"INSERT (TmId, TfkId, ModifiedDate, PSI, ModifiedBy, CreatedBy, Current, CreatedDate)"
            + Environment.NewLine
            +"VALUES (source.TmId, source.TfkId, source.ModifiedDate, source.PSI, source.ModifiedBy, source.CreatedBy, source.Current, source.CreatedDate)"
            + Environment.NewLine
            +";"; // final new line

            // act
            var actual = instance.Merge(new UpsertOptions<TestModel1>()
                .SetTargetTable("TestModel1")
                .AddMatchColumn(o => o.TmId)
                .MapAllColumns()
                );

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Multiple Match Columns Should Match")]
        public void MultipleMatchColumnsShouldMatch()
        {
            // arrange
            var instance = new TSqlGenerator<StandardModel>();
            var expected = $@"MERGE INTO dbo.StandardModels WITH (HOLDLOCK) AS target"
            + Environment.NewLine
            +"USING #StandardModels AS source"
            + Environment.NewLine
            +"ON target.PK = source.PK AND target.FK = source.FK"
            + Environment.NewLine
            +"WHEN MATCHED THEN"
            + Environment.NewLine
            +"UPDATE SET target.PK = source.PK, target.FK = source.FK, target.Name = source.Name"
            + Environment.NewLine
            +"WHEN NOT MATCHED BY target THEN"
            + Environment.NewLine
            +"INSERT (PK, FK, Name)"
            + Environment.NewLine
            +"VALUES (source.PK, source.FK, source.Name)"
            + Environment.NewLine
            +";"; // final new line

            // act
            var actual = instance.Merge(new UpsertOptions<StandardModel>()
                .SetTargetTable("StandardModels")
                .AddMatchColumn(o => o.PK)
                .AddMatchColumn(o => o.FK)
                .AddMapColumn(p => p.PK)
                .AddMapColumn(p => p.FK)
                .AddMapColumn(p => p.Name)
            );

            // assert
            Assert.Equal(expected, actual);
        }
    }
}