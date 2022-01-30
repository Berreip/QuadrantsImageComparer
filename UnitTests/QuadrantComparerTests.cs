using System.Drawing;
using System.Linq;
using NUnit.Framework;
using QuadrantsImageComparerLib;
using QuadrantsImageComparerLib.Models;
using UnitTests.RessourcesFiles;

namespace UnitTests
{
    [TestFixture]
    public sealed class QuadrantComparerTests
    {
        [Test]
        [TestCase(20, 12)]
        [TestCase(3, 22)]
        [TestCase(20, 10)]
        public void ComputeDelta_return_expected_matrix_size(int numberOfRows, int numberOfColumns)
        {
            //Arrange
            var file = ImgFileGetter.GetImage("img100x100_1.png");
            using var img1 = new Bitmap(file.FullName);
            using var img2 = new Bitmap(file.FullName);
            
            //Act
            var res = QuadrantComparer.ComputeDelta(img1, img2, new QuadrantConfig {NumberOfQuadrantColumns = numberOfRows, NumberOfQuadrantRows = numberOfRows,});

            //Assert
            Assert.AreEqual(res.Red.Columns, numberOfRows);
            Assert.AreEqual(res.Green.Columns, numberOfRows);
            Assert.AreEqual(res.Blue.Columns, numberOfRows);
            Assert.AreEqual(res.Red.Rows, numberOfRows);
            Assert.AreEqual(res.Green.Rows, numberOfRows);
            Assert.AreEqual(res.Blue.Rows, numberOfRows);
        }
        
        [Test]
        [TestCase(10, 15)]
        public void ComputeDelta_return_empty_matrix_for_same_image(int numberOfRows, int numberOfColumns)
        {
            //Arrange
            var file = ImgFileGetter.GetImage("img100x100_1.png");
            using var img1 = new Bitmap(file.FullName);
            using var img2 = new Bitmap(file.FullName);
            
            //Act
            var res = QuadrantComparer.ComputeDelta(img1, img2, new QuadrantConfig
            {
                NumberOfQuadrantColumns = numberOfColumns,
                NumberOfQuadrantRows = numberOfRows,
            });
            
            //Assert
            Assert.IsTrue(res.Red.GetValues().All(o => o == 0));
        }

    }
}