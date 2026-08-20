using OszkConnector.Models;
using System.Text;

namespace Tests
{
    public class ConverterTests
    {

        [Fact]
        public void MultilineTrim()
        {
            var sample = "\n\n\rFirstLine\r\n\tSecondLine\n  ThirdLine  \n FourthLine    \nFifth Line\n";
            var expected = "FirstLine\nSecondLine\nThirdLine\nFourthLine\nFifth Line";
            var trimmed = MekConvert.Trim(sample);

            Assert.Equal(expected, trimmed);
        }

        [Fact]
        public void BookTitle()
        {
            var content = Encoding.UTF8.GetBytes("Gárdonyi Géza");

            Assert.Equal("Gárdonyi Géza", MekConvert.ToIso88592(content));
        }
    }
}
