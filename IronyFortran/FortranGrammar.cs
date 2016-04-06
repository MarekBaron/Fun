using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronyFortran
{
    [Language("VDIFortran")]
    public class FortranGrammar : Grammar
    {
        public FortranGrammar() : base(caseSensitive: false)
        {            
            //terminals
            var identifier = new IdentifierTerminal("identifier");
            var number = new NumberLiteral("number");            

            //nonterminals            
            var builtinType = new NonTerminal("builtinType");
            var stringType = new NonTerminal("string");
            var paramList = new NonTerminal("paramList");
            var functionHeader = new NonTerminal("functionHeader");
            var functionFooter = new NonTerminal("functionFooter");
            var variableDec = new NonTerminal("variableDec");
            var variableDecList = new NonTerminal("variableDecList");
            var variableDecListElem = new NonTerminal("variableDecList");
            var program = new NonTerminal("program");

            this.MarkPunctuation(";", ",", "(", ")", "[", "]", ":");
            this.MarkTransient(builtinType);

            //rules
            stringType.Rule = ToTerm("CHARACTER") + "(" + number + ")";
            builtinType.Rule = ToTerm("INTEGER") | "CHARACTER" | stringType;
            paramList.Rule = MakeStarRule(paramList, ToTerm(","), identifier);
            functionHeader.Rule = builtinType + ToTerm("FUNCTION") + identifier + "(" + paramList + ")" + ";";
            functionFooter.Rule = ToTerm("END") + ToTerm("FUNCTION") + identifier +";";

            variableDecListElem.Rule = identifier | identifier + "(" + number +")";
            variableDecList.Rule = MakePlusRule(variableDecList, ToTerm(","), variableDecListElem);
            variableDec.Rule = builtinType + variableDecList + ";";

            program.Rule = functionHeader + variableDec + functionFooter;
            this.Root = program;
        }
    }
}
