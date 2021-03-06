﻿using Irony.Ast;
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
            number.AstConfig.NodeType = typeof(NumberValueNode);
            var intNumber = new NumberLiteral("intNumber", NumberOptions.IntOnly);
            intNumber.AstConfig.NodeType = typeof(LiteralValueNode);
            var stringValue = new StringLiteral("stringValue", "'");
            stringValue.AstConfig.NodeType = typeof(StringLiteralValueNode);
            //uwaga: eos oznacza End Of Statement, czyli średnik, po którym może występować opcjonalny komentarz
            //nie udało mi się tego uzyskać w bardziej elegancki sposób
            var eos = new CommentTerminal("comment", ";", "\n", "\r\n");
            eos.AstConfig.NodeType = typeof(NoGenerationNode);
            eos.Category = TokenCategory.Content;

            //nonterminals            
            var builtinType = new NonTerminal("builtinType");
            var stringType = new NonTerminal("stringType");
            var paramList = new NonTerminal("paramList");
            var functionHeader = new NonTerminal("functionHeader", typeof(FunctionHeaderNode));
            var functionFooter = new NonTerminal("functionFooter");
            var variableDec = new NonTerminal("variableDec", typeof(VariableDecNode));
            var variableDecList = new NonTerminal("variableDecList");
            var variableDecListElem = new NonTerminal("variableDecListElem");
            var binOp = new NonTerminal("binOp");
            var binExpr = new NonTerminal("binExp", typeof(BinExprNode));
            var unOp = new NonTerminal("unOp");
            var unExpr = new NonTerminal("unExpr", typeof(UnExprNode));
            var ifOneLineStatement = new NonTerminal("ifOneLineStatement", typeof(IfOneLineStatementNode));
            var ifStatement = new NonTerminal("ifStatement", typeof(IfStatementNode));
            var elseIfClause = new NonTerminal("elseIfClause", typeof(ElseIfClauseNode));
            var elseIfOneLineClause = new NonTerminal("elseIfOneLineClause", typeof(ElseIfOneLineClauseNode));
            var elseIfClauseList = new NonTerminal("elseIfClauseList", typeof(ElseIfClauseListNode));
            var elseIfClauseListElem = new NonTerminal("elseIfClauseListElem");
            var elseClause_opt = new NonTerminal("elseStatement_opt");
            var doWhileStatement = new NonTerminal("doWhile", typeof(DoWhileNode));
            var expression = new NonTerminal("expression");
            var parExpression = new NonTerminal("parExpression", typeof(ParExpressionNode));
            var expressionList = new NonTerminal("expressionList", typeof(ExpressionListNode));
            var assignment = new NonTerminal("assignment", typeof(AssignmentNode));
            var arrayAssignment = new NonTerminal("arrayAssignment", typeof(ArrayAssignmentNode));
            var arrayRangeAssignment = new NonTerminal("arrayRangeAssignment", typeof(ArrayRangeAssignmentNode));
            var functionCall = new NonTerminal("functionCall", typeof(FunctionCallNode));
            var statement = new NonTerminal("statement");
            var statementList = new NonTerminal("statementList", typeof(StatementListNode));
            var function = new NonTerminal("function", typeof(FunctionNode));
            var program = new NonTerminal("program", typeof(ProgramNode));
            var value = new NonTerminal("value");
            var boolValue = new NonTerminal("boolValue", typeof(BoolValueNode));

            //rules //NIE SKLEJAć STRINGÓW!!! np. ")" + ";"
            stringType.Rule = ToTerm("CHARACTER") + "(" + intNumber + ")";
            builtinType.Rule = ToTerm("INTEGER") | "CHARACTER" | stringType | "LOGICAL" | "REAL";
            boolValue.Rule = ToTerm(".true.") | ".false.";

            paramList.Rule = MakeStarRule(paramList, ToTerm(","), identifier);
            functionHeader.Rule = builtinType + ToTerm("FUNCTION") + identifier + "(" + paramList + ")" + eos;
            functionFooter.Rule = ToTerm("END") + ToTerm("FUNCTION") + identifier + eos;           

            variableDecListElem.Rule = identifier | identifier + "(" + intNumber + ( Empty | ":" + intNumber ) + ")";
            variableDecList.Rule = MakeStarRule(variableDecList, ToTerm(","), variableDecListElem);
            variableDec.Rule = builtinType + variableDecList + eos;

            functionCall.Rule = identifier + "(" + expressionList + ")";
            value.Rule = stringValue | intNumber | number | identifier | boolValue;
            binOp.Rule = ToTerm("+") | "-" | "*" | "/" | "**" | "==" | "<" | "<=" | ">" | ">=" | ".and." | ".or." ;
            binExpr.Rule = expression + binOp + expression;

            unOp.Rule = ToTerm(".not.") | "+" | "-";
            unExpr.Rule = unOp + expression;
                        
            ifStatement.Rule = ToTerm("if") + "(" + expression + ")" + "then" + eos + statementList + elseIfClauseList + elseClause_opt + "endif" + eos;
            ifOneLineStatement.Rule = ToTerm("if") + "(" + expression + ")" + statement;
            elseClause_opt.Rule = Empty | ToTerm("else") + ";" + statementList;
            elseIfClause.Rule = ToTerm("elseif") + "(" + expression + ")" + "then" + eos + statementList;
            elseIfOneLineClause.Rule = ToTerm("elseif") + "(" + expression + ")" + statement;
            elseIfClauseListElem.Rule = elseIfClause | elseIfOneLineClause;
            elseIfClauseList.Rule = MakeStarRule(elseIfClauseList, elseIfClauseListElem);

            doWhileStatement.Rule = ToTerm("do") + "while" + "(" + expression + ")" + eos + statementList + "enddo" + eos;

            parExpression.Rule = "(" + expression + ")";
            expressionList.Rule = MakePlusRule(expressionList, ToTerm(","), expression);
            expression.Rule = value | functionCall | binExpr | unExpr | parExpression;
            assignment.Rule = identifier + "=" + expression + eos;
            arrayAssignment.Rule = identifier + "(" + expression + ")" + "=" + expression + eos;            
            arrayRangeAssignment.Rule = identifier + "(" + expression + ":" + expression + ")" + "=" + "(/" + expressionList + "/)" + eos;

            statement.Rule = variableDec | assignment | arrayRangeAssignment | arrayAssignment | ifOneLineStatement | ifStatement | doWhileStatement | eos;
            statementList.Rule = MakeStarRule(statementList, statement);

            function.Rule = functionHeader + statementList + functionFooter;
            program.Rule = MakePlusRule(program, function);
            this.Root = program;

            this.RegisterOperators(10, "?", ".not.");
            this.RegisterOperators(15, ".and.", ".or.");
            this.RegisterOperators(20, "==", "<", "<=", ">", ">=");
            this.RegisterOperators(30, "+", "-");
            this.RegisterOperators(40, "*", "/");
            this.RegisterOperators(50, Associativity.Right, "**");

            this.MarkPunctuation(";", ",", "(", ")", "[", "]", ":", "=", "(/", "/)", "if", "then", "else", "elseif", "endif", "do", "while", "enddo");
            this.MarkTransient(builtinType, statement, value, expression, binOp, unOp, elseIfClauseListElem);
            this.MarkReservedWords("if", "then", "else", "elseif", "do", "while", "enddo");

            //NonGrammarTerminals.Add(eos);

            this.LanguageFlags |= LanguageFlags.CreateAst;

        }

        public override void BuildAst(LanguageData language, ParseTree parseTree)
        {
            //dzięki nadpisaniu tej metody, możemy ustawić DefaultNodeType
            if (!LanguageFlags.IsSet(LanguageFlags.CreateAst))
                return;
            var astContext = new AstContext(language);
            astContext.DefaultNodeType = typeof(NoGenerationNode);
            var astBuilder = new AstBuilder(astContext);
            astBuilder.BuildAst(parseTree);
        }
    }
}
