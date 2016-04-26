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
                var gc = BuildGenerationContext((ProgramNode)parseTree.Root.AstNode);
                var sb = new StringBuilder();
                ((ProgramNode)parseTree.Root.AstNode).Generate(gc, 0, sb);

                GenerateFunctionWrappers(gc, sb);
                return sb.ToString();
            }                        
        }

        private void GenerateFunctionWrappers(GenerationContext aGenerationContext, StringBuilder aSB)
        {
            foreach(var fwd in aGenerationContext.FunctionWrappers.Values)
            {
                fwd.Generate(aSB);
            }
        }

        private GenerationContext BuildGenerationContext(ProgramNode aProgramNode)
        {            
            var gc = new GenerationContext();
            foreach(var function in aProgramNode.Functions)
            {
                gc.AddVariable(function.Name, new Variable(function.HeaderNode.ReturnType, function.Name));//nazwa funkcji może w jej ciele występować jak zmienna
                foreach(var varDecNode in function.StatementList.Statements.OfType<VariableDecNode>())
                {
                    foreach (var variable in varDecNode.Variables)
                        gc.AddVariable(function.Name, variable);
                }
                foreach (var paramName in function.HeaderNode.ParamNames)
                    gc.MarkAsInputParameter(function.Name, paramName);
            }
            return gc;
        }
    }
}
