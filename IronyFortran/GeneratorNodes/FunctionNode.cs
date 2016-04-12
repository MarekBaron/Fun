using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronyFortran.GeneratorNodes
{
    public class FunctionNode : BaseNode
    {
        public override void Generate(int anIndent, StringBuilder aSB)
        {
            HeaderNode.Generate(anIndent, aSB);
            aSB.AppendLine(Indent(anIndent) + "{");
            //_statementsNode.Generate(anIndent + 1, aSB);
            aSB.AppendLine(Indent(anIndent) + "}");
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            HeaderNode = (FunctionHeaderNode)treeNode.ChildNodes[0].AstNode;
        }

        public FunctionHeaderNode HeaderNode { get; private set; }
    }
}
