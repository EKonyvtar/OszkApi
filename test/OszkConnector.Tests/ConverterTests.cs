using OszkConnector.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

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
            //TODO
        }
    }
}
