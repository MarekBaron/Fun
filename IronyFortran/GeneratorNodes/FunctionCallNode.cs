using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace IronyFortran.GeneratorNodes
{
    /// <summary>
    /// Uwaga: tutaj także wpada odwołanie do tablicy!!!
    /// </summary>
    public class FunctionCallNode : BaseNode
    {
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            if (aContext.IsArray(FunctionOrArrayName.Value))
            {
                FunctionOrArrayName.Generate(aContext, anIndent, aSB);
                aSB.Append("[");
                Parameters.Generate(aContext, anIndent, aSB);
                aSB.Append("]");
            }
            else
            {
                FunctionOrArrayName.Generate(aContext, anIndent, aSB);
                aSB.Append("(");
                Parameters.Generate(aContext, anIndent, aSB);
                aSB.Append(")");
            }
        }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            FunctionOrArrayName = (IdentifierValueNode)parseNode.ChildNodes[0].AstNode;
            Parameters = (ExpressionListNode)parseNode.ChildNodes[1].AstNode;
        }

        public IdentifierValueNode FunctionOrArrayName { get; private set; }
        public ExpressionListNode Parameters { get; private set; }
    }
}
