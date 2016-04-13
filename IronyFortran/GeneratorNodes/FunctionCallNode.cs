using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace IronyFortran.GeneratorNodes
{
    public class FunctionCallNode : BaseNode
    {
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            aSB.AppendFormat("{0}(", FunctionName);
            Parameters.Generate(aContext, anIndent, aSB);
            aSB.AppendFormat(")");
        }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            FunctionName = parseNode.ChildNodes[0].Token.ValueString;
            Parameters = (ExpressionListNode)parseNode.ChildNodes[1].AstNode;
        }

        public string FunctionName { get; private set; }
        public ExpressionListNode Parameters { get; private set; }
    }
}
