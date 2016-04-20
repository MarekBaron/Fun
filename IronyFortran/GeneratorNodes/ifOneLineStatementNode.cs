using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace IronyFortran.GeneratorNodes
{
    public class IfOneLineStatementNode : StatementNode
    {
        public BaseNode Condition { get; private set; }
        public BaseNode TrueStatement { get; private set; }

        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            GenerateInternal("if", aContext, anIndent, aSB);
        }

        protected void GenerateInternal(string aKeyword, GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            aSB.AppendFormat("{0}(", aKeyword);
            Condition.Generate(aContext, anIndent, aSB);
            aSB.Append(") ");
            TrueStatement.Generate(aContext, anIndent, aSB);
        }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            Condition = (BaseNode)parseNode.ChildNodes[0].AstNode;
            TrueStatement = (BaseNode)parseNode.ChildNodes[1].AstNode;
        }
    }
}
