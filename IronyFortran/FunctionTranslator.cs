using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronyFortran
{
    public class FunctionTranslator
    {
        public string Translate(string anInput, out string anErrors)
        {
            var grammar = new FortranGrammar();

            LanguageData language = new LanguageData(grammar);

            Parser parser = new Parser(language);

            ParseTree parseTree = parser.Parse(anInput);

            anErrors = parseTree
                .ParserMessages.Any() ? 
                    parseTree
                    .ParserMessages
                    .Select(m => $"{m.Level.ToString()} {m.Location} {m.Message}")
                    .Aggregate((a, s) => a + Environment.NewLine + s)
                : String.Empty;
            return string.Empty;
        }
    }
}
