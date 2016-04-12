using Irony.Parsing;
using IronyFortran.GeneratorNodes;
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
            if(anErrors != String.Empty)
                return string.Empty;
            else
            {
                var sb = new StringBuilder();
                ((ProgramNode)parseTree.Root.AstNode).Generate(0, sb);
                return sb.ToString();
            }                        
        }

        private GenerationContext BuildGenerationContext(ProgramNode aProgramNode)
        {            
            var gc = new GenerationContext();
            foreach(var function in aProgramNode.Functions)
            {
                //todo funkcja powinna mieć statementsList - iterujemy przez nią, wybieramy variableDec i zapamiętujemy w GenerationContext
                //GenerationContext przekazujemy do BaseNode.Generate (będzie potrzebny w FunctionHeaderNode i VariableDecNode)
            }
            return gc;
        }
    }
}
