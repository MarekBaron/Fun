using System.Text;
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
            aSB.AppendFormat("{0}.SetRange({1}, {2}, ", VariableName.Value, RangeFrom, RangeTo);
            ExpressionList.Generate(aContext, anIndent, aSB);
            aSB.Append(")");
        }

        /// <summary/>
        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            VariableName = (IdentifierValueNode)parseNode.ChildNodes[0].AstNode;
            RangeFrom = parseNode.ChildNodes[1].Token.ValueString.ToUpper();
            RangeTo = parseNode.ChildNodes[2].Token.ValueString.ToUpper();
            ExpressionList = (ExpressionListNode)parseNode.ChildNodes[3].AstNode;
        }

        public IdentifierValueNode VariableName { get; private set; }
        public string RangeFrom { get; private set; }
        public string RangeTo { get; private set; }
        public ExpressionListNode ExpressionList { get; private set; }
        
    }
}
