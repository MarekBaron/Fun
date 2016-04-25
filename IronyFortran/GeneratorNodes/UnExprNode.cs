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
    public class UnExprNode : BaseNode
    {
        /// <summary>
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            aSB.Append(Operator);
            Expression.Generate(aContext, anIndent, aSB);
        }

        /// <summary>
        protected override void InitInternal(AstContext aContext, ParseTreeNode aParseNode)
        {
            var opAsString = aParseNode.ChildNodes[0].Token.ValueString;
            if (opAsString == ".not.")
                Operator = "!";
            else
                Operator = opAsString;
            Expression = (BaseNode)aParseNode.ChildNodes[1].AstNode;
        }

        /// <summary>
        public string Operator { get; private set; }
        /// <summary>
        public BaseNode Expression { get; private set; }
    }
}
