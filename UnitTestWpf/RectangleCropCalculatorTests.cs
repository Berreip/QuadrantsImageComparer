using System;
using NUnit.Framework;
using QicRecVisualizer.Services.Helpers;

namespace UnitTestWpf
{
    [TestFixture]
    public class RectangleCropCalculatorTests
    {
        [Test]
        [TestCase(0, 0, 100, 100, 0)]
        [TestCase(10, 0, 100, 90, 0)]
        [TestCase(20, 0, 100, 80, 0)]
        [TestCase(20, 0, 300, 240, 0)]
        [TestCase(100, 0, 300, 0, 0)]
        [TestCase(50, 0, 200, 100, 0)]
        [TestCase(-10, 0, 100, 100, 0)] // negative value ignored
        [TestCase(200, 0, 100, 0, 0)] // higher than 100% cap
        [TestCase(10, 10, 100, 80, 10)]
        [TestCase(10, 20, 100, 70, 20)]
        [TestCase(50, 50, 100, 0, 50)]
        [TestCase(50, 60, 100, 0, 50)] // caped percentage
        public void GetNewRectangleHeightAndTopPosition_returns_expected_value(double percentageBottom, double percentageTop, int imageHeight, int expectedRectangleHeight, int expectedTopPosition)
        {
            //Arrange

            //Act
            var (topPosition, rectangleHeight) = RectangleCropCalculator.GetNewRectangleHeightAndTopPosition(percentageBottom, percentageTop, imageHeight);

            //Assert
            Assert.AreEqual(expectedRectangleHeight, rectangleHeight);
            Assert.AreEqual(expectedTopPosition, topPosition);
        }
        
        [Test]
        [TestCase(0, 0, 100, 100, 0)]
        [TestCase(10, 0, 100, 90, 0)]
        [TestCase(10, 20, 100, 70, 20)]
        [TestCase(30, 20, 200, 100, 40)]
        public void GetNewRectangleWidthAndLeftPosition_returns_expected_value(double percentageRight, double percentageLeft, int imageWidth, int expectedRectangleWidth, int expectedLeftPosition)
        {
            //Arrange

            //Act
            var (leftPosition, rectangleWidth) = RectangleCropCalculator.GetNewRectangleWidthAndLeftPosition(percentageRight, percentageLeft, imageWidth);

            //Assert
            Assert.AreEqual(expectedRectangleWidth, rectangleWidth);
            Assert.AreEqual(expectedLeftPosition, leftPosition);
        }

    }
}