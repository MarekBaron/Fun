using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace IronyFortran.GeneratorNodes
{
    /// <summary/>    
    public class ParExpressionNode : BaseNode
    {
        /// <summary/>    
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            aSB.Append("(");
            Expression.Generate(aContext, anIndent, aSB);
            aSB.Append(")");
        }

        /// <summary/>    
        protected override void InitInternal(AstContext aContext, ParseTreeNode aParseNode)
        {
            Expression = (BaseNode)aParseNode.ChildNodes[0].AstNode;
        }

        /// <summary/>
        public BaseNode Expression { get; private set; }
    }
}
