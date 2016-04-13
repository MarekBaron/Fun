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
        /// <summary/>
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            //dziwna konstrukcja po to, żeby nie generować przecinka po ostatnim expression
            for (int i = 0; i < _expressions.Count - 1; i++)
            {
                _expressions[i].Generate(aContext, anIndent, aSB);
                aSB.Append(", ");
            }
            _expressions.Last().Generate(aContext, anIndent, aSB);            
        }

        /// <summary/>
        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            _expressions = parseNode.ChildNodes
                .Select(n => n.AstNode)
                .Cast<BaseNode>()
                .ToList();
        }

        private List<BaseNode> _expressions;

        /// <summary/>
        public IEnumerable<BaseNode> Expressions { get { return _expressions; } }
    }
}
