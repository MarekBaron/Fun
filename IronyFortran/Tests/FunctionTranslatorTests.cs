using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NUnit.Framework;
using System.Reflection;

namespace IronyFortran.Tests
{
    [TestFixture]
    public class FunctionTranslatorTests
    {
        [TestCase("test1")]
        //[TestCase("ifelseelseif")]
        public void TestTranslation(string aFilename)
        {
            var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\Tests\TestData"));
            var inputFilename = Path.Combine(path, aFilename) + ".input.txt";            
            var expectedFilename = Path.Combine(path, aFilename) + ".expected.cs";
            var outputFilename = Path.Combine(path, aFilename) + ".output.cs";
            FileAssert.Exists(inputFilename);            

            var trns = new FunctionTranslator();
            string errors;
            var output = trns.Translate(File.ReadAllText(inputFilename), out errors);
            File.WriteAllText(outputFilename, output);

            Assert.That(errors, Is.Empty);
            FileAssert.AreEqual(expectedFilename, outputFilename);            
        }        
    }
}
