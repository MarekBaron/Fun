using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace IronyFortran.GeneratorNodes
{
    public class ArrayAssignmentNode : StatementNode
    {
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            VariableName.Generate(aContext, anIndent, aSB);
            aSB.Append("[");
            Index.Generate(aContext, anIndent, aSB);
            aSB.Append("] = ");
            Value.Generate(aContext, anIndent, aSB);
        }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            VariableName = (IdentifierValueNode)parseNode.ChildNodes[0].AstNode;
            Index = (BaseNode)parseNode.ChildNodes[1].AstNode;
            Value = (BaseNode)parseNode.ChildNodes[2].AstNode;
        }

        public BaseNode Value { get; private set; }
        public BaseNode Index { get; private set; }
        public IdentifierValueNode VariableName { get; private set; }
    }
}
