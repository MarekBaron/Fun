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
    public class FortranGrammar : Grammar
    {
        public FortranGrammar() : base(caseSensitive: false)
        {
            //terminals
            var identifier = new IdentifierTerminal("identifier");
            identifier.AstConfig.NodeType = typeof(IdentifierValueNode);
            var number = new NumberLiteral("number");
            number.AstConfig.NodeType = typeof(LiteralValueNode);
            var intNumber = new NumberLiteral("intNumber", NumberOptions.IntOnly);
            intNumber.AstConfig.NodeType = typeof(LiteralValueNode);
            var stringValue = new StringLiteral("stringValue", "'");
            stringValue.AstConfig.NodeType = typeof(StringLiteralValueNode);

            //nonterminals            
            var builtinType = new NonTerminal("builtinType", typeof(NoGenerationNode));
            var stringType = new NonTerminal("stringType", typeof(NoGenerationNode));
            var paramList = new NonTerminal("paramList", typeof(NoGenerationNode));
            var functionHeader = new NonTerminal("functionHeader", typeof(FunctionHeaderNode));
            var functionFooter = new NonTerminal("functionFooter", typeof(NoGenerationNode));
            var variableDec = new NonTerminal("variableDec", typeof(VariableDecNode));
            var variableDecList = new NonTerminal("variableDecList", typeof(NoGenerationNode));
            var variableDecListElem = new NonTerminal("variableDecListElem", typeof(NoGenerationNode));
            var binOp = new NonTerminal("binOp", typeof(NoGenerationNode));
            var binExpr = new NonTerminal("binExp", typeof(BinExprNode));            
            var ifOneLineStatement = new NonTerminal("ifOneLineStatement", typeof(IfOneLineStatementNode));
            var ifStatement = new NonTerminal("ifStatement", typeof(IfStatementNode));
            var elseIfClause = new NonTerminal("elseIfClause", typeof(ElseIfClauseNode));
            var elseIfOneLineClause = new NonTerminal("elseIfOneLineClause", typeof(ElseIfOneLineClauseNode));
            var elseIfClauseList = new NonTerminal("elseIfClauseList", typeof(ElseIfClauseListNode));
            var elseIfClauseListElem = new NonTerminal("elseIfClauseListElem", typeof(NoGenerationNode));
            var elseClause_opt = new NonTerminal("elseStatement_opt", typeof(NoGenerationNode));            
            var expression = new NonTerminal("expression", typeof(NoGenerationNode));
            var expressionList = new NonTerminal("expressionList", typeof(ExpressionListNode));
            var assignment = new NonTerminal("assignment", typeof(AssignmentNode));
            var arrayRangeAssignment = new NonTerminal("arrayRangeAssignment", typeof(ArrayRangeAssignmentNode));
            var functionCall = new NonTerminal("functionCall", typeof(FunctionCallNode));
            var statement = new NonTerminal("statement", typeof(NoGenerationNode));
            var statementList = new NonTerminal("statementList", typeof(StatementListNode));
            var function = new NonTerminal("function", typeof(FunctionNode));
            var program = new NonTerminal("program", typeof(ProgramNode));
            var value = new NonTerminal("value", typeof(NoGenerationNode));

            //rules //NIE SKLEJAć STRINGÓW!!! np. ")" + ";"
            stringType.Rule = ToTerm("CHARACTER") + "(" + intNumber + ")";
            builtinType.Rule = ToTerm("INTEGER") | "CHARACTER" | stringType | "LOGICAL";
            paramList.Rule = MakeStarRule(paramList, ToTerm(","), identifier);
            functionHeader.Rule = builtinType + ToTerm("FUNCTION") + identifier + "(" + paramList + ")" + ";";
            functionFooter.Rule = ToTerm("END") + ToTerm("FUNCTION") + identifier + ";";           

            variableDecListElem.Rule = identifier | identifier + "(" + intNumber + ")";
            variableDecList.Rule = MakeStarRule(variableDecList, ToTerm(","), variableDecListElem);
            variableDec.Rule = builtinType + variableDecList + ";";

            functionCall.Rule = identifier + "(" + expressionList + ")";
            value.Rule = stringValue | intNumber | number | identifier;
            binOp.Rule = ToTerm("+") | "-" | "*" | "/" | "**" | "==" | "<" | "<=" | ">" | ">=" | ".and." | ".or." ;
            binExpr.Rule = expression + binOp + expression;
            
            ifStatement.Rule = ToTerm("if") + "(" + expression + ")" + "then" + ";" + statementList + elseIfClauseList + elseClause_opt + "endif" + ";";
            ifOneLineStatement.Rule = ToTerm("if") + "(" + expression + ")" + statement;
            elseClause_opt.Rule = Empty | ToTerm("else") + ";" + statementList;
            elseIfClause.Rule = ToTerm("elseif") + "(" + expression + ")" + "then" + ";" + statementList;
            elseIfOneLineClause.Rule = ToTerm("elseif") + "(" + expression + ")" + statement;
            elseIfClauseListElem.Rule = elseIfClause | elseIfOneLineClause;
            elseIfClauseList.Rule = MakeStarRule(elseIfClauseList, elseIfClauseListElem);

            expressionList.Rule = MakePlusRule(expressionList, ToTerm(","), expression);
            expression.Rule = value | functionCall | binExpr;
            assignment.Rule = identifier + "=" + expression + ";";
            arrayRangeAssignment.Rule = identifier + "(" + intNumber + ":" + intNumber + ")" + "=" + "(/" + expressionList + "/)" + ";"; 

            statement.Rule = variableDec | assignment | arrayRangeAssignment | ifOneLineStatement | ifStatement;
            statementList.Rule = MakeStarRule(statementList, statement);

            function.Rule = functionHeader + statementList + functionFooter;
            program.Rule = MakePlusRule(program, function);
            this.Root = program;

            RegisterOperators(10, "?");
            RegisterOperators(15, ".and.", ".or.");
            RegisterOperators(20, "==", "<", "<=", ">", ">=");
            RegisterOperators(30, "+", "-");
            RegisterOperators(40, "*", "/");
            RegisterOperators(50, Associativity.Right, "**");

            this.MarkPunctuation(";", ",", "(", ")", "[", "]", ":", "=", "(/", "/)", "if", "then", "else", "elseif", "endif");
            this.MarkTransient(builtinType, statement, value, expression, binOp, elseIfClauseListElem);
            this.MarkReservedWords("if", "then", "else", "elseif");

            this.LanguageFlags |= LanguageFlags.CreateAst;

        }
    }
}
