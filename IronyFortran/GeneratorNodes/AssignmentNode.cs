using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace IronyFortran.GeneratorNodes
{
    public class AssignmentNode : StatementNode
    {
        public BaseNode Value { get; private set; }
        public IdentifierValueNode VariableName { get; private set; }

        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            aSB.AppendFormat("{0}{1} = ({2})(", Indent(anIndent), VariableName.Value, aContext.VariableType(VariableName.Value));
            Value.Generate(aContext, anIndent, aSB);
            aSB.AppendLine(");");
        }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {            
            VariableName = (IdentifierValueNode)parseNode.ChildNodes[0].AstNode;
            Value = (BaseNode)parseNode.ChildNodes[1].AstNode;
        }
    }
}
