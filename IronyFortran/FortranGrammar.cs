using Irony.Interpreter;
using Irony.Parsing;
using IronyFortran.GeneratorNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronyFortran
{
    [Language("VDIFortran")]
    public class FortranGrammar : InterpretedLanguageGrammar
    {
        public FortranGrammar() : base(caseSensitive: false)
        {            
            //terminals
            var identifier = new IdentifierTerminal("identifier");
            var number = new NumberLiteral("number");
            var intNumber = new NumberLiteral("intNumber", NumberOptions.IntOnly);

            //nonterminals            
            var builtinType = new NonTerminal("builtinType", typeof(NoGenerationNode));
            var stringType = new NonTerminal("string", typeof(NoGenerationNode));
            var paramList = new NonTerminal("paramList", typeof(NoGenerationNode));
            var functionHeader = new NonTerminal("functionHeader", typeof(FunctionHeaderNode));
            var functionFooter = new NonTerminal("functionFooter", typeof(NoGenerationNode));
            var variableDec = new NonTerminal("variableDec", typeof(NoGenerationNode));
            var variableDecList = new NonTerminal("variableDecList", typeof(NoGenerationNode));
            var variableDecListElem = new NonTerminal("variableDecListElem", typeof(NoGenerationNode));
            var function = new NonTerminal("function", typeof(FunctionNode));           
            var program = new NonTerminal("program", typeof(ProgramNode));

            this.MarkPunctuation(";", ",", "(", ")", "[", "]", ":");
            this.MarkTransient(builtinType);

            //rules
            stringType.Rule = ToTerm("CHARACTER") + "(" + intNumber + ")";
            builtinType.Rule = ToTerm("INTEGER") | "CHARACTER" | stringType;
            paramList.Rule = MakeStarRule(paramList, ToTerm(","), identifier);
            functionHeader.Rule = builtinType + ToTerm("FUNCTION") + identifier + "(" + paramList + ")" + ";";
            functionFooter.Rule = ToTerm("END") + ToTerm("FUNCTION") + identifier +";";

            variableDecListElem.Rule = identifier | identifier + "(" + intNumber + ")";            
            variableDecList.Rule = MakeStarRule(variableDecList, ToTerm(","), variableDecListElem);
            variableDec.Rule = builtinType + variableDecList + ";";

            function.Rule = functionHeader + variableDec + functionFooter;
            program.Rule = MakePlusRule(program, function);
            this.Root = program;

            this.LanguageFlags |= LanguageFlags.CreateAst;
            //this.
        }
    }
}
