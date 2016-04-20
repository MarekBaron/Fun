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
            var variableType = aContext.VariableType(VariableName.Value);
            aSB.AppendFormat("{0} = ({1})", VariableName.Value, variableType);            
            if (Value is StringLiteralValueNode && (variableType != "string"))
            {
                aSB.AppendFormat("{0}.Parse", aContext.VariableType(VariableName.Value));
            }
            aSB.Append("(");            
            Value.Generate(aContext, anIndent, aSB);
            aSB.Append(")");
        }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {            
            VariableName = (IdentifierValueNode)parseNode.ChildNodes[0].AstNode;
            Value = (BaseNode)parseNode.ChildNodes[1].AstNode;
        }
    }
}
