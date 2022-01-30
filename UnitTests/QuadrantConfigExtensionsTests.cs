using System.Drawing;
using NUnit.Framework;
using QuadrantsImageComparerLib;
using QuadrantsImageComparerLib.Helpers;
using QuadrantsImageComparerLib.Models;

namespace UnitTests
{
    [TestFixture]
    public sealed class QuadrantConfigExtensionsTests
    {
        [Test]
        [TestCase(0, 0, 0, 0, 100, 100, 0, 0, 100, 100)]
        [TestCase(0, 0, 0, 0, 200, 100, 0, 0, 200, 100)]
        [TestCase(10, 0, 0, 0, 200, 100, 20, 0, 180, 100)]
        [TestCase(10, 0, 10, 0, 200, 100, 20, 0, 160, 100)]
        [TestCase(0, 10, 0, 0, 200, 100, 0, 10, 200, 90)]
        [TestCase(0, 10, 0, 10, 200, 100, 0, 10, 200, 80)]
        [TestCase(0, 20, 0, 10, 200, 100, 0, 20, 200, 70)]
        public void ComputeAoi_returns_expected_aoi(
            int aoiLeftPercentage, int aoiTopPercentage, int aoiRightPercentage, int aoiBottomPercentage,
            int imgwidth, int imgHeight,
            int expectedRectX, int expectedRectY, int expectedRectWith, int expectedRectHeight)
        {
            //Arrange
            var expectedRect = new Rectangle(expectedRectX, expectedRectY, expectedRectWith, expectedRectHeight);
            var aoi = new ImageAoi
            {
                AoiLeftPercentage = aoiLeftPercentage,
                AoiTopPercentage = aoiTopPercentage,
                AoiRightPercentage = aoiRightPercentage,
                AoiBottomPercentage = aoiBottomPercentage
            };

            //Act
            var res = aoi.ComputeAoi(new Size(imgwidth, imgHeight));

            //Assert
            Assert.AreEqual(expectedRect, res);
        }

        [Test]
        [TestCase(0, 0, 1, 1)] // cap to on quadrant min
        [TestCase(-1, -1, 1, 1)] // cap to on quadrant min
        [TestCase(1, QuadrantLibConstants.MAXIMUM_ALLOWED_QUADRANTS + 1, QuadrantLibConstants.MAXIMUM_ALLOWED_QUADRANTS, 1)] // cap to on quadrant max width
        [TestCase(QuadrantLibConstants.MAXIMUM_ALLOWED_QUADRANTS + 1, 1, 1, QuadrantLibConstants.MAXIMUM_ALLOWED_QUADRANTS)] // cap to on quadrant max height
        [TestCase(10, 10, 10, 10)]
        [TestCase(10, 20, 20, 10)]
        [TestCase(99, 73, 73, 99)]
        public void ComputeSizeFromQuadrant_returns_expected_size_depending_on_quadrant_row_and_columns(
            int quadrantRows, int quadrantColumns,
            int expectedSizeWidth, int expectedSizeHeight)
        {
            //Arrange
            var expectedSize = new Size(expectedSizeWidth, expectedSizeHeight);

            //Act
            var res = QuadrantConfigCalculator.ComputeSizeFromQuadrant(quadrantRows, quadrantColumns);

            //Assert
            Assert.AreEqual(expectedSize, res);
        }
    }
}