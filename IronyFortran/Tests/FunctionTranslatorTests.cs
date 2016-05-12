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
        [TestCase("varDec")]
        [TestCase("arrayAssign")]                
        [TestCase("ifelseelseif")]
        [TestCase("comments")]
        [TestCase("nestedArrayAccess")]
        [TestCase("PowerOperator")]
        [TestCase("nonPredefinedFunctionCall")]
        [TestCase("translation_1")]
        [TestCase("translation_2")]        
        [TestCase("translation_4")]
        [TestCase("translation_6")]
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

            FileAssert.Exists(expectedFilename);
            FileAssert.AreEqual(expectedFilename, outputFilename);            
        }        
    }
}
