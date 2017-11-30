using LaughingWaffle.PropertyLoaders;
using System.Collections.Generic;
using WaffleTests.TestModels;
using Xunit;
using System.Linq;
using System;
using System.Reflection;

namespace LaughingWaffle.Tests
{
    public class UpsertOptionsTests
    {
        [Fact(DisplayName = "Add Column To Options Adds, To Internal Mapping String")]
        public void AddColumnToInternalMappingList()
        {
            // arrange
            var instance = new UpsertOptions<StandardModel>();
            var expected = "PK";

            // act
            instance.AddMapColumn(p => p.PK);
            var actual = instance._mapColumns.First();

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Add Two Columns To Options Adds, To Internal Mapping String")]
        public void AddTwoColumnToInternalMappingList()
        {
            // arrange
            var instance = new UpsertOptions<StandardModel>();
            var expected = "PK,Name";

            // act
            instance.AddMapColumn(p => p.PK).AddMapColumn(p => p.Name);
            var actual = string.Join(",",instance._mapColumns);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Add Column To Options Adds, To Internal Match String")]
        public void AddColumnToInternalMatchList()
        {
            // arrange
            var instance = new UpsertOptions<StandardModel>();
            var expected = "PK";

            // act
            instance.AddMatchColumn(p => p.PK);
            var actual = instance._matchColumns.First();

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Add Two Columns To Options, Adds To Internal Match String")]
        public void AddTwoColumnToInternalMatchList()
        {
            // arrange
            var instance = new UpsertOptions<StandardModel>();
            var expected = "PK,Name";

            // act
            instance.AddMatchColumn(p => p.PK).AddMatchColumn(p => p.Name);
            var actual = string.Join(",", instance._matchColumns);

            // assert
            Assert.Equal(expected, actual);
        }
    }
}