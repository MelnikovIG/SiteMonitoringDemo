using System;
using Xunit;

namespace SiteMonitoring.Domain.Tests
{
    /// <summary>
    /// Tests for <see  cref="RefreshPeriod"/>
    /// </summary>
    public class RefreshPeriodTests
    {
        [Fact]
        public void FromSeconds_ShouldFail_IfValueIsZero()
        {
            //Act //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => RefreshPeriod.FromSeconds(0));
        }

        [Fact]
        public void SecondsProperty_Should_ReturnCreatedValue()
        {
            //Arrange
            var value = 5u;
            var refreshPeriod = RefreshPeriod.FromSeconds(value);

            //Act
            var result = refreshPeriod.Seconds;

            //Assert
            Assert.Equal(value, result);
        }
    }
}
