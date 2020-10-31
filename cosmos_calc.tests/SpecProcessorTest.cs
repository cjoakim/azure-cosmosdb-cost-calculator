using System;
using System.IO;
using Xunit;

using CJoakim.CosmosCalc;

// Xunit unit tests for class SpecReader.
// Chris Joakim, Microsoft, 2020/10/31

namespace cosmos_calc.tests
{
    public class SpecReaderTest
    {
        [Fact]
        public void Spec1Test()
        {
            string tempFilename = System.IO.GetTempFileName();
            Console.WriteLine(tempFilename);

            // string specFilename = "specification1.txt";
            // SpecReader sr = new SpecReader(specFilename);
            // sr.silent = false;
            // sr.process();
            // Console.WriteLine(sr.calculationResults);

            //Assert.True();

        }


        private string Spec1()
        {
return@"
SELECT foo, bar
FROM table
WHERE id = 42";
        }
    }
}
