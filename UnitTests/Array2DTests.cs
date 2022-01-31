using NUnit.Framework;
using QuadrantsImageComparerLib.Models;

namespace UnitTests
{
    [TestFixture]
    public class Array2DTests
    {
        [Test]
        [TestCase(10, 10)]
        [TestCase(10, 20)]
        [TestCase(100, 30)]
        public void Array2D_ctor_return_expected_size(int rowSize, int columnsSize)
        {
            //Arrange
            var array = new int[rowSize, columnsSize];

            //Act
            var res = new Array2D(array);

            //Assert
            Assert.AreEqual(rowSize, res.Rows);
            Assert.AreEqual(columnsSize, res.Columns);
            Assert.AreEqual(columnsSize*rowSize, res.GetValues().Length);
        }
        
        [Test]
        public void Array2D_GetValue_returns_expected_value()
        {
            //Arrange
            var array = new[, ]
            {
                { 1, 5},
                { 198, 0},
                { 12, -5},
            };

            //Act
            var res = new Array2D(array);

            //Assert
            Assert.AreEqual(array[0,0], res.GetValue(0,0));
            Assert.AreEqual(array[0,1], res.GetValue(0,1));
            Assert.AreEqual(array[1,0], res.GetValue(1,0));
            Assert.AreEqual(array[1,1], res.GetValue(1,1));
            Assert.AreEqual(array[2,0], res.GetValue(2,0));
            Assert.AreEqual(array[2,1], res.GetValue(2,1));
        }
        
        [Test]
        public void Array2D_GetValues_returns_expected_1DArray()
        {
            //Arrange
            var array = new[, ]
            {
                { 1, 5},
                { 198, 0},
                { 12, -5},
            };
            
            var array1D = new[] {1, 5, 198, 0,  12, -5};

            //Act
            var res = new Array2D(array);

            //Assert
            Assert.AreEqual(array1D, res.GetValues());
        }

        [Test]
        public void EqualsArray_returns_false_for_not_equals_array_when_null()
        {
            //Arrange
            var array = new[, ]
            {
                { 1, 5},
                { 198, 0},
                { 12, -5},
            };
            var array2D = new Array2D(array);

            //Act
            var res = array2D.EqualsArray(null);

            //Assert
            Assert.IsFalse(res);
        }
        
        [Test]
        public void EqualsArray_returns_false_for_not_equals_array_when_empty()
        {
            //Arrange
            var array = new[, ]
            {
                { 1, 5},
                { 198, 0},
                { 12, -5},
            };
            var array2D = new Array2D(array);

            //Act
            var res = array2D.EqualsArray(new int[, ]{});

            //Assert
            Assert.IsFalse(res);
        }
        
        [Test]
        public void EqualsArray_returns_false_for_not_equals_array_when_not_same_format()
        {
            //Arrange
            var array = new[, ]
            {
                { 1, 5},
                { 198, 0},
                { 12, -5},
            };
            var array2D = new Array2D(array);

            //Act
            var res = array2D.EqualsArray(new[, ]
            {
                { 1, 5, 198},
                {0, 12, -5}
            });

            //Assert
            Assert.IsFalse(res);
        }
        
        [Test]
        public void EqualsArray_returns_false_for_not_equals_array_when_value_differs()
        {
            //Arrange
            var array = new[, ]
            {
                { 1, 5},
                { 198, 0},
                { 12, -5},
            };
            var array2D = new Array2D(array);

            //Act
            var res = array2D.EqualsArray(new[, ]
            {
                { 1, 5},
                { 0, 0},
                { 12, -5},
            });

            //Assert
            Assert.IsFalse(res);
        } 
        
        [Test]
        public void EqualsArray_returns_true_when_equals_()
        {
            //Arrange
            var array = new[, ]
            {
                { 1, 5},
                { 198, 0},
                { 12, -5},
            };
            var array2D = new Array2D(array);

            //Act
            var res = array2D.EqualsArray(new[, ]
            {
                { 1, 5},
                { 198, 0},
                { 12, -5},
            });

            //Assert
            Assert.IsTrue(res);
        }

    }
}