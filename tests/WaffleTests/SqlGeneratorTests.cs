using LaughingWaffle.SqlGeneration;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Xunit;

namespace LaughingWaffle.Tests
{
    [Table("StandardModels", Schema = "dbo")]
    public class StandardModel
    {
        public int PK { get; set; }
        public int? FK { get; set; }
        public String Name { get; set; }
    }

    [Table("TestNullAndNotNullInts")]
    public class TestNullAndNotNullInt
    {
        public int PK { get; set; }
        public int? FK { get; set; }
    }

    [Table("EachTypeWeHave")]
    public class EachTypeWeHave
    {
        public int PK { get; set; }
        public int? FK { get; set; }

        public string Name { get; set; }
        public DateTime Dt { get; set; }
        public DateTime? NullableDt { get; set; }
        public double d { get; set; }
        public double? dNull { get; set; }
        public long longNumber { get; set; }
        public Int64 i64Number { get; set; }
        public float er { get; set; }
        public decimal dec { get; set; }
        public bool theBIT { get; set; }
        public Boolean theBOOLEAN { get; set; }
        public Byte chewer { get; set; }
        public byte bigChewer { get; set; }

        public byte[] binary { get; set; }
        public DateTimeOffset offset { get; set; }
    }

    public class SqlGeneratorTests
    {
        [Fact(DisplayName = "Table Name Should Match Attribute")]
        public void TableNameShouldMatch()
        {
            // arrange
            var instance = new TSqlGenerator<StandardModel>();
            var expected = $@"StandardModels";

            // act
            var actual = instance.TableName;

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Table Schema Should Match Attribute")]
        public void TableSchemaShouldMatchAttributeSchema()
        {
            // arrange
            var instance = new TSqlGenerator<StandardModel>();
            var expected = $@"dbo";

            // act
            var actual = instance.TableSchema;

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Test Non Temp Table")]
        public void TestNonTempTableGeneration()
        {
            // arrange
            var instance = new TSqlGenerator<StandardModel>();
            var expected = $@"CREATE TABLE StandardModels (
[PK] [int] NOT NULL
[FK] [int] NULL
[Name] [nvarchar(max)] NOT NULL
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
[PK] [int] NOT NULL
[FK] [int] NULL
[Name] [nvarchar(max)] NOT NULL
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
[PK] [int] NOT NULL
[FK] [int] NULL
[Name] [nvarchar(max)] NOT NULL
)";
            // act
            var actual = instance.CreateTable(true);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName ="Test Null and Not Null Ints")]
        public void TestNullAndNotNullInts()
        {
            // arrange
            var instance = new TSqlGenerator<TestNullAndNotNullInt>();
            var expected = $@"CREATE TABLE TestNullAndNotNullInts (
[PK] [int] NOT NULL
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
[PK] [int] NOT NULL
[FK] [int] NULL
[Name] [nvarchar(max)] NOT NULL
[Dt] [datetime] NOT NULL
[NullableDt] [datetime] NULL
[d] [real] NOT NULL
[dNull] [real] NULL
[longNumber] [bigint] NOT NULL
[i64Number] [bigint] NOT NULL
[er] [float] NOT NULL
[dec] [decimal] NOT NULL
[theBIT] [bit] NOT NULL
[theBOOLEAN] [bit] NOT NULL
[chewer] [tinyint] NOT NULL
[bigChewer] [tinyint] NOT NULL
[binary] [binary] NOT NULL
[offset] [datetimeoffset] NOT NULL
)";
            // act
            var actual = instance.CreateTable(false);

            // assert
            Assert.Equal(expected, actual);
        }
    }
}