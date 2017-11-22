using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dapper.Upsert.Tests
{
    [TestClass]
    public class SqlGeneratorTests
    {
        [Table("TestNullAndNotNullInts")]
        public class TestNullAndNotNullInt
        {
            public int PK { get; set; }
            public int? FK { get; set; }
        }

        [TestMethod]
        public void TestNullAndNotNullInts()
        {
            // arrange
            var instance = new SqlGenerator();
            var expected = $@"CREATE TABLE TestNullAndNotNullInts (
[PK] [int] NOT NULL
[FK] [int] NULL
)";
            // act
            var actual = instance.CreateTable<TestNullAndNotNullInt>();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [Table("EachTypeWeHave")]
        public class EachTypeWeHave
        {
            public int PK { get; set; }
            public int? FK { get; set; }

            public string Name { get; set; }
            public DateTime Dt { get; set; }
            public DateTime? NullableDt { get; set; }
        }

        [TestMethod]
        public void TestEveryType()
        {
            // arrange
            var instance = new SqlGenerator();
            var expected = $@"CREATE TABLE EachTypeWeHave (
[PK] [int] NOT NULL
[FK] [int] NULL
[Name] [nvarchar(max)] NOT NULL
[Dt] [datetime] NOT NULL
[NullableDt] [datetime] NULL
)";
            // act
            var actual = instance.CreateTable<EachTypeWeHave>();

            // assert
            Assert.AreEqual(expected, actual);
        }
    }
}