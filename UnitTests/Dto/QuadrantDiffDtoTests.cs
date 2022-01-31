using NUnit.Framework;
using QuadrantsImageComparerLib.Dto;
using UnitTests.UnitTestHelpers;

namespace UnitTests.Dto
{
    [TestFixture]
    public class QuadrantDiffDtoTests
    {
        [Test]
        public void SerializeToJsonCycle_empty()
        {
            //Arrange
            var dto = new QuadrantDiffDto();

            //Act
            var res = dto.SerializeThenDeserialize();

            //Assert
            Assert.IsNotNull(res);
        }
        
        [Test]
        public void SerializeToJsonCycle_with_Threshold()
        {
            //Arrange
            var dto = new QuadrantDiffDto
            {
                Threshold = 9
            };

            //Act
            var res = dto.SerializeThenDeserialize();

            //Assert
            Assert.IsNotNull(res);
            Assert.AreEqual(dto.Threshold, res.Threshold);
        }
        
        [Test]
        public void SerializeToJsonCycle_with_RGB()
        {
            //Arrange
            var dto = new QuadrantDiffDto
            {
                Red = new[,]
                {
                    {1, 2, 3},
                    {4, 5, 6}
                }
            };

            //Act
            var res = dto.SerializeThenDeserialize();

            //Assert
            Assert.IsNotNull(res);
            Assert.AreEqual(dto.Red, res.Red);
        }
  
        [Test]
        public void SerializeToJsonCycle_with_AoiInfoDto()
        {
            //Arrange
            var dto = new QuadrantDiffDto
            {
                AoiInfo = new AoiInfoDto
                {
                    QuadrantRows = 90,
                    QuadrantColumns =52,
                    AoiLeftPercentage = 1,
                    AoiTopPercentage = 2,
                    AoiRightPercentage = 5,
                    AoiBottomPercentage = 99,
                }
            };

            //Act
            var res = dto.SerializeThenDeserialize();

            //Assert
            Assert.IsNotNull(res);
            Assert.AreEqual(dto.AoiInfo.QuadrantRows, res.AoiInfo.QuadrantRows);
            Assert.AreEqual(dto.AoiInfo.QuadrantColumns, res.AoiInfo.QuadrantColumns);
            Assert.AreEqual(dto.AoiInfo.AoiLeftPercentage, res.AoiInfo.AoiLeftPercentage);
            Assert.AreEqual(dto.AoiInfo.AoiTopPercentage, res.AoiInfo.AoiTopPercentage);
            Assert.AreEqual(dto.AoiInfo.AoiRightPercentage, res.AoiInfo.AoiRightPercentage);
            Assert.AreEqual(dto.AoiInfo.AoiBottomPercentage, res.AoiInfo.AoiBottomPercentage);
        }

        
        [Test]
        public void SerializeToJsonCycle_Full()
        {
            //Arrange
            var dto = new QuadrantDiffDto
            {
                Threshold = 9,
                Red = new[,]
                {
                    {1, 2, 3},
                    {4, 5, 6}
                },
                Green = new[,]
                {
                    {6, 2, 3},
                    {4, 5, 6}
                },
                Blue = new[,]
                {
                    {9, 2, 3},
                    {4, 5, 6}
                }
            };

            //Act
            var res = dto.SerializeThenDeserialize();

            //Assert
            Assert.IsNotNull(res);
            Assert.AreEqual(dto.Threshold, res.Threshold);
            Assert.AreEqual(dto.Red, res.Red);
            Assert.AreEqual(dto.Green, res.Green);
            Assert.AreEqual(dto.Blue, res.Blue);
        }
    }
}