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
        public string VariableName { get; private set; }
        public ExpressionNode Expression { get; private set; }

        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            aSB.AppendFormat("{0}{1} = ({2})(", Indent(anIndent), VariableName, aContext.VariableType(VariableName));
            Expression.Generate(aContext, anIndent, aSB);
            aSB.AppendLine(");");
        }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            VariableName = parseNode.ChildNodes[0].Token.ValueString.ToUpper();
            Expression = (ExpressionNode)parseNode.ChildNodes[1].AstNode;
        }
    }
}
