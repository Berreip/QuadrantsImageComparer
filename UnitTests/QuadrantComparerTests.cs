using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using NUnit.Framework;
using QuadrantsImageComparerLib;
using QuadrantsImageComparerLib.Dto;
using QuadrantsImageComparerLib.Models;
using UnitTests.RessourcesFiles;
using UnitTests.UnitTestHelpers;

namespace UnitTests
{
    [TestFixture]
    [SuppressMessage("Interoperability", "CA1416:Valider la compatibilité de la plateforme")]
    public sealed class QuadrantComparerTests
    {
        [Test]
        [TestCase(20, 12)]
        [TestCase(3, 22)]
        [TestCase(20, 10)]
        public void ComputeDelta_return_expected_matrix_size(int numberOfRows, int numberOfColumns)
        {
            //Arrange
            var file = ImgFileGetter.GetImageFile(ImgKey.img100x100_1);
            using var img1 = new Bitmap(file.FullName);
            using var img2 = new Bitmap(file.FullName);

            //Act
            var res = QuadrantComparer.ComputeDelta(img1, img2, new QuadrantConfig(numberOfRows, numberOfColumns));

            //Assert
            Assert.AreEqual(res.Red.Columns, numberOfColumns);
            Assert.AreEqual(res.Green.Columns, numberOfColumns);
            Assert.AreEqual(res.Blue.Columns, numberOfColumns);
            Assert.AreEqual(res.Red.Rows, numberOfRows);
            Assert.AreEqual(res.Green.Rows, numberOfRows);
            Assert.AreEqual(res.Blue.Rows, numberOfRows);
        }

        [Test]
        [TestCase(10, 15)]
        public void ComputeDelta_return_empty_matrix_for_same_image(int numberOfRows, int numberOfColumns)
        {
            //Arrange
            var file = ImgFileGetter.GetImageFile(ImgKey.img100x100_1);
            using var img1 = new Bitmap(file.FullName);
            using var img2 = new Bitmap(file.FullName);

            //Act
            var res = QuadrantComparer.ComputeDelta(img1, img2, new QuadrantConfig(numberOfRows, numberOfColumns));

            //Assert
            Assert.IsTrue(res.Red.GetValues().All(o => o == 0));
            Assert.IsTrue(res.Green.GetValues().All(o => o == 0));
            Assert.IsTrue(res.Blue.GetValues().All(o => o == 0));
        }

        [Test]
        public void ComputeDelta_return_warning_when_different_ratio()
        {
            //Arrange
            using var img1 = ImgFileGetter.GetImage(ImgKey.img100x100_blue_border);
            using var img2 = ImgFileGetter.GetImage(ImgKey.img150x60_violet);
          
            //Act
            var res = QuadrantComparer.ComputeDelta(img1, img2, new QuadrantConfig(2, 2));

            //Assert
            Assert.Contains(WarningKind.RatioDoNotMatch, res.Warnings.ToArray());
        }
        
        [Test]
        [TestCase(10, 15)]
        public void ComputeDelta_return_empty_matrix_when_cropping_to_Center(int numberOfRows, int numberOfColumns)
        {
            //Arrange
            using var img1 = ImgFileGetter.GetImage(ImgKey.img100x100_blue_border);
            using var img2 = ImgFileGetter.GetImage(ImgKey.img100x100_orange_border);
            
            // AOI = L: 35% | T: 12% | R: 12% | B: 0%
            var targetAoi = new ImageAoi
            {
                AoiLeftPercentage = 12,
                AoiTopPercentage = 12,
                AoiRightPercentage = 35,
                AoiBottomPercentage = 0
            };
            //Act
            var res = QuadrantComparer.ComputeDelta(img1, img2, new QuadrantConfig(numberOfRows, numberOfColumns)
            {
                Aoi = targetAoi
            });

            //Assert
            Assert.IsTrue(res.Red.GetValues().All(o => o == 0));
            Assert.IsTrue(res.Green.GetValues().All(o => o == 0));
            Assert.IsTrue(res.Blue.GetValues().All(o => o == 0));
        }
        
        [Test]
        public void ComputeDelta_return_expected_matrix_for_blue_without_aoi()
        {
            //Arrange
            using var img1 = ImgFileGetter.GetImage(ImgKey.img10x10_black); // black = 0 everywhere
            using var img2 = ImgFileGetter.GetImage(ImgKey.img10x10_blue);
            var quadrantConfig = new QuadrantConfig(1, 1); // one pixel result
            
            //Act
            var res = QuadrantComparer.ComputeDelta(img1, img2, quadrantConfig);

            //Assert
            Assert.AreEqual(0, res.Red.GetValues().Single());
            Assert.AreEqual(0, res.Green.GetValues().Single());
            Assert.AreEqual(-255, res.Blue.GetValues().Single());
        }
        
        [Test]
        public void ComputeDelta_return_expected_matrix_for_red_without_aoi()
        {
            //Arrange
            using var img1 = ImgFileGetter.GetImage(ImgKey.img10x10_black); // black = 0 everywhere
            using var img2 = ImgFileGetter.GetImage(ImgKey.img10x10_red);
            var quadrantConfig = new QuadrantConfig(1, 1); // one pixel result
            
            //Act
            var res = QuadrantComparer.ComputeDelta(img1, img2, quadrantConfig);

            //Assert
            Assert.AreEqual(-255, res.Red.GetValues().Single());
            Assert.AreEqual(0, res.Green.GetValues().Single());
            Assert.AreEqual(0, res.Blue.GetValues().Single());
        }

        [Test]
        public void ComputeDelta_return_expected_matrix_for_green_without_aoi()
        {
            //Arrange
            using var img1 = ImgFileGetter.GetImage(ImgKey.img10x10_black); // black = 0 everywhere
            using var img2 = ImgFileGetter.GetImage(ImgKey.img10x10_green);
            var quadrantConfig = new QuadrantConfig(1, 1); // one pixel result
            
            //Act
            var res = QuadrantComparer.ComputeDelta(img1, img2, quadrantConfig);

            //Assert
            Assert.AreEqual(0, res.Red.GetValues().Single());
            Assert.AreEqual(-255, res.Green.GetValues().Single());
            Assert.AreEqual(0, res.Blue.GetValues().Single());
        }
        
        [Test]
        public void ComputeDelta_Then_IsValideAgainst_returns_false()
        {
            //Arrange
            var quadrantInfo = ImgFileGetter.GetQuadrantInfoFile(QuadrantInfo.blue_orange_invalid_aoi).Deserialize<QuadrantDiffDto>();
            using var img1 = ImgFileGetter.GetImage(ImgKey.img100x100_blue_border);
            using var img2 = ImgFileGetter.GetImage(ImgKey.img100x100_orange_border);

            //Act
            var delta = QuadrantComparer.ComputeDelta(img1, img2, quadrantInfo.AoiInfo);
            var valid = delta.IsValideAgainst(quadrantInfo);
            
            //Assert
            Assert.IsFalse(valid);
        }  
        
        [Test]
        public void ComputeDelta_Then_IsValideAgainst_returns_true()
        {
            //Arrange
            var quadrantInfo = ImgFileGetter.GetQuadrantInfoFile(QuadrantInfo.blue_orange_valid_aoi).Deserialize<QuadrantDiffDto>();
            using var img1 = ImgFileGetter.GetImage(ImgKey.img100x100_blue_border);
            using var img2 = ImgFileGetter.GetImage(ImgKey.img100x100_orange_border);

            //Act
            var delta = QuadrantComparer.ComputeDelta(img1, img2, quadrantInfo.AoiInfo);
            var valid = delta.IsValideAgainst(quadrantInfo);
            
            //Assert
            Assert.IsTrue(valid);
        }
    }
}