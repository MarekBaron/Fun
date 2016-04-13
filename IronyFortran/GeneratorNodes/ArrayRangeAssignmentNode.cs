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
    /// 
    /// </summary>
    public class ArrayRangeAssignmentNode : StatementNode
    {
        /// <summary/>        
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            aSB.AppendFormat("{0}{1}.SetRange({2}, {3}, ", Indent(anIndent), VariableName, RangeFrom, RangeTo);
            for(int i = 0; i < _expressions.Count - 1; i++)
            {
                _expressions[i].Generate(aContext, anIndent, aSB);
                aSB.Append(", ");
            }
            _expressions.Last().Generate(aContext, anIndent, aSB);
            aSB.AppendLine(");");

        }

        /// <summary/>
        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            VariableName = parseNode.ChildNodes[0].Token.ValueString.ToUpper();
            RangeFrom = parseNode.ChildNodes[1].Token.ValueString;
            RangeTo = parseNode.ChildNodes[2].Token.ValueString;
            _expressions = parseNode.ChildNodes[3].ChildNodes
                .Select(n => n.AstNode)
                .Cast<ExpressionNode>()
                .ToList();
        }

        public string VariableName { get; private set; }
        public string RangeFrom { get; private set; }
        public string RangeTo { get; private set; }
        private List<ExpressionNode> _expressions;
        public IEnumerable<ExpressionNode> Expressions { get { return _expressions; } }
    }
}
