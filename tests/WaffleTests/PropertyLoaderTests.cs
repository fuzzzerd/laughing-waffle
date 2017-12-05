using LaughingWaffle.PropertyLoaders;
using System.Collections.Generic;
using WaffleTests.TestModels;
using Xunit;
using System.Linq;
using System;
using System.Reflection;

namespace LaughingWaffle.Tests
{
    public class PropertyLoaderTests
    {
        [Fact(DisplayName = "Standard Model Has PK, FK, Name Properties")]
        public void StandardModelHasThreeProperties()
        {
            // arrange
            IPropertyLoader instance = new PropertyLoader();
            var expected = new List<string> { "PK", "FK", "Name" };

            // act
            var actual = instance
                .GetProperties(typeof(StandardModel))
                .Select(p => p.Name);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact(DisplayName = "Do Not Include ReadOnly Properties")]
        public void DoNotIncludeReadOnlyProperties()
        {
            // arrange
            IPropertyLoader instance = new PropertyLoader();
            var expected = new List<string> { "PK", "FK" };

            // act
            var actual = instance
                .GetProperties(typeof(ModelWithAGetter))
                .Select(p => p.Name);

            // assert
            Assert.Equal(expected, actual);
        }

        [Theory(DisplayName = "Get Properties Returns IEnumerable PropertyInfos")]
        [InlineData(typeof(StandardModel))]
        [InlineData(typeof(ModelWithAGetter))]
        [InlineData(typeof(TestNullAndNotNullInt))]
        public void GetPropertiesReturnsIEnumerablePropInfos(Type t)
        {
            // arrange
            IPropertyLoader instance = new PropertyLoader();
            var expected = typeof(IEnumerable<PropertyInfo>);

            // act
            var actual = instance.GetProperties(t);

            // assert that we have a derived class from IEnumerable<PropertyInfo>
            Assert.IsAssignableFrom<IEnumerable<PropertyInfo>>(actual);
        }

        [Fact(DisplayName = "Standard Model Should Show Three Properties")]
        public void StandardModelHasThreeValueAndStrings()
        {
            // arrange
            IPropertyLoader instance = new PropertyLoader();
            var expected = 3;

            // act
            var actual = instance.GetProperties(typeof(StandardModel));

            // assert that we have a derived class from IEnumerable<PropertyInfo>
            Assert.Equal(expected, actual.Count());
        }

        [Fact(DisplayName = "Custom Model Should Show All Properties")]
        public void CustommodelShouldShowAllProperties()
        {
            // arrange
            IPropertyLoader instance = new PropertyLoader();
            var expected = 8;

            // act
            var actual = instance.GetProperties(typeof(TestModel1));

            // assert that we have a derived class from IEnumerable<PropertyInfo>
            Assert.Equal(expected, actual.Count());
        }
    }
}