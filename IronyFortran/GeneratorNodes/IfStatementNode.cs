using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace IronyFortran.GeneratorNodes
{
    public class IfStatementNode : StatementNode
    {
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            aSB.Append("if(");
            Condition.Generate(aContext, anIndent, aSB);
            aSB.AppendLine(")");
            aSB.Append(Indent(anIndent));
            aSB.AppendLine("{");
            TrueStatements.Generate(aContext, anIndent + 1, aSB);
            aSB.AppendFormat("{0}}}", Indent(anIndent));
            aSB.AppendLine();
            if(ElseStatements != null)
            {
                aSB.Append(Indent(anIndent));
                aSB.AppendLine("{");
                ElseStatements.Generate(aContext, anIndent + 1, aSB);
                aSB.AppendFormat("{0}}}", Indent(anIndent));
                aSB.AppendLine();
            }
            
        }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            Condition = (BaseNode)parseNode.ChildNodes[0].AstNode;
            TrueStatements = (BaseNode)parseNode.ChildNodes[1].AstNode;
            if(parseNode.ChildNodes[2].ChildNodes.Any())
                ElseStatements = (BaseNode)parseNode.ChildNodes[2].ChildNodes[0].AstNode;

        }

        public BaseNode Condition { get; private set; }
        public BaseNode TrueStatements { get; private set; }
        public BaseNode ElseStatements { get; private set; }
    }
}
