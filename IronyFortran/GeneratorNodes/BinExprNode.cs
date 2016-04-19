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
    /// Węzeł opisujący wyrażenie binarne
    /// </summary>
    public class BinExprNode : BaseNode
    {
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            if (Operator == "**")
            {
                aSB.Append(" Math.Pow(");
                LeftExpr.Generate(aContext, anIndent, aSB);
                aSB.Append(", ");
                RightExpr.Generate(aContext, anIndent, aSB);
                aSB.Append(")");
            }
            else
            {
                LeftExpr.Generate(aContext, anIndent, aSB);
                aSB.AppendFormat(" {0} ", Operator);
                RightExpr.Generate(aContext, anIndent, aSB);
            }
        }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            LeftExpr = (BaseNode)parseNode.ChildNodes[0].AstNode;            
            RightExpr = (BaseNode)parseNode.ChildNodes[2].AstNode;
            Operator = parseNode.ChildNodes[1].Token.ValueString;
            if (Operator == ".and.")
                Operator = "&&";
            if (Operator == ".or.")
                Operator = "||";
        }

        /// <summary>
        /// Wyrażenie po lewej stronie operatora
        /// </summary>
        public BaseNode LeftExpr { get; private set; }
        /// <summary>
        /// Wyrażenie po prawej stronie operatora
        /// </summary>
        public BaseNode RightExpr { get; private set; }
        /// <summary>
        /// Operator
        /// </summary>
        public string Operator { get; private set; }
    }
}
