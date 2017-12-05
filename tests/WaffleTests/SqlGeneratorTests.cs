using LaughingWaffle.SqlGeneration;
using WaffleTests.TestModels;
using Xunit;

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
            var expected = $@"CREATE TABLE StandardModels (
[PK] [int] NOT NULL,
[FK] [int] NULL,
[Name] [nvarchar](max) NOT NULL
)";
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
            var expected = $@"CREATE TABLE #StandardModels (
[PK] [int] NOT NULL,
[FK] [int] NULL,
[Name] [nvarchar](max) NOT NULL
)";
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
            var expected = $@"CREATE TABLE #StandardModels (
[PK] [int] NOT NULL,
[FK] [int] NULL,
[Name] [nvarchar](max) NOT NULL
)";
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
            var expected = $@"CREATE TABLE TestNullAndNotNullInts (
[PK] [int] NOT NULL,
[FK] [int] NULL
)";
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
            var expected = $@"CREATE TABLE EachTypeWeHave (
[PK] [int] NOT NULL,
[FK] [int] NULL,
[theGuid] [uniqueidentifier] NOT NULL,
[nullGuid] [uniqueidentifier] NULL,
[Name] [nvarchar](max) NOT NULL,
[Dt] [datetime] NOT NULL,
[NullableDt] [datetime] NULL,
[d] [real] NOT NULL,
[dNull] [real] NULL,
[longNumber] [bigint] NOT NULL,
[i64Number] [bigint] NOT NULL,
[er] [float] NOT NULL,
[dec] [decimal] NOT NULL,
[theBIT] [bit] NOT NULL,
[theBOOLEAN] [bit] NOT NULL,
[chewer] [tinyint] NOT NULL,
[bigChewer] [tinyint] NOT NULL,
[binary] [binary] NOT NULL,
[offset] [datetimeoffset] NOT NULL
)";
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
            var expected = $@"MERGE INTO dbo.StandardModels WITH (HOLDLOCK) AS target
USING #StandardModels AS source
ON target.PK = source.PK
WHEN MATCHED THEN
UPDATE SET target.Name = source.Name
WHEN NOT MATCHED BY target THEN
INSERT (Name)
VALUES (source.Name)
;"; // final new line

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
            var expected = $@"MERGE INTO dbo.StandardModels WITH (HOLDLOCK) AS target
USING #StandardModels AS source
ON target.PK = source.PK
WHEN MATCHED THEN
UPDATE SET target.PK = source.PK, target.FK = source.FK, target.Name = source.Name
WHEN NOT MATCHED BY target THEN
INSERT (PK, FK, Name)
VALUES (source.PK, source.FK, source.Name)
;"; // final new line

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
            var expected = $@"MERGE INTO dbo.TestModel1 WITH (HOLDLOCK) AS target
USING #TestModel1 AS source
ON target.TmId = source.TmId
WHEN MATCHED THEN
UPDATE SET target.TmId = source.TmId, target.TfkId = source.TfkId, target.ModifiedDate = source.ModifiedDate, target.PSI = source.PSI, target.ModifiedBy = source.ModifiedBy, target.CreatedBy = source.CreatedBy, target.Current = source.Current, target.CreatedDate = source.CreatedDate
WHEN NOT MATCHED BY target THEN
INSERT (TmId, TfkId, ModifiedDate, PSI, ModifiedBy, CreatedBy, Current, CreatedDate)
VALUES (source.TmId, source.TfkId, source.ModifiedDate, source.PSI, source.ModifiedBy, source.CreatedBy, source.Current, source.CreatedDate)
;"; // final new line

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
            var expected = $@"MERGE INTO dbo.StandardModels WITH (HOLDLOCK) AS target
USING #StandardModels AS source
ON target.PK = source.PK AND target.FK = source.FK
WHEN MATCHED THEN
UPDATE SET target.PK = source.PK, target.FK = source.FK, target.Name = source.Name
WHEN NOT MATCHED BY target THEN
INSERT (PK, FK, Name)
VALUES (source.PK, source.FK, source.Name)
;"; // final new line

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