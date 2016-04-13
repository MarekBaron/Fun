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
    public class ExpressionListNode : BaseNode
    {
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            for (int i = 0; i < _expressions.Count - 1; i++)
            {
                _expressions[i].Generate(aContext, anIndent, aSB);
                aSB.Append(", ");
            }
            _expressions.Last().Generate(aContext, anIndent, aSB);
        }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            _expressions = parseNode.ChildNodes
                .Select(n => n.AstNode)
                .Cast<ExpressionNode>()
                .ToList();
        }

        private List<ExpressionNode> _expressions;
        public IEnumerable<ExpressionNode> Expressions { get { return _expressions; } }
    }
}
