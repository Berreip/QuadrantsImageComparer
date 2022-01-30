using NUnit.Framework;
using QuadrantsImageComparerLib.Helpers;
using UnitTests.RessourcesFiles;

namespace UnitTests
{
    [TestFixture]
    public sealed class BitmapHelpersTests
    {
        [Test]
        public void GetRation_returns_expected_ratio_same_width_height()
        {
            //Arrange
            using var img1 = ImgFileGetter.GetImage(ImgKey.img100x100_blue_border);

            //Act
            var res = img1.GetRatio();

            //Assert
            Assert.AreEqual(1, res);
        }
        
        [Test]
        public void GetRation_returns_expected_ratio_different_width_height()
        {
            //Arrange
            using var img1 = ImgFileGetter.GetImage(ImgKey.img150x60_violet);

            //Act
            var res = img1.GetRatio();

            //Assert
            Assert.AreEqual(2.5, res);
        }

    }
}