using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SiteMonitoring.Services.Tests
{
    /// <summary>
    /// Tests for <see cref="IdGenerator"/>
    /// </summary>
    public class IdGeneratorTests
    {
        private readonly IdGenerator.IdGenerator _target;

        public IdGeneratorTests()
        {
            _target = new IdGenerator.IdGenerator();
        }

        [Fact]
        public void Generate_Should_ReturnUniqueValues()
        {
            //Arrange
            var count = 1000;

            List<Guid> list = new List<Guid>(count);

            //Act
            for (int i = 0; i < count; i++)
            {
                list.Add(_target.Generate());
            }

            //Assert
            Assert.Equal(count, list.Distinct().Count());
        }
    }
}
