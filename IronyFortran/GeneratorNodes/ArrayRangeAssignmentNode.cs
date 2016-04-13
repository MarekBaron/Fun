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
            aSB.AppendFormat("{0}{1}.SetRange({2}, {3}, ", Indent(anIndent), VariableName.Value, RangeFrom, RangeTo);
            ExpressionList.Generate(aContext, anIndent, aSB);
            aSB.AppendLine(");");
        }

        /// <summary/>
        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            VariableName = (IdentifierValueNode)parseNode.ChildNodes[0].AstNode;
            RangeFrom = parseNode.ChildNodes[1].Token.ValueString;
            RangeTo = parseNode.ChildNodes[2].Token.ValueString;
            ExpressionList = (ExpressionListNode)parseNode.ChildNodes[3].AstNode;
        }

        public IdentifierValueNode VariableName { get; private set; }
        public string RangeFrom { get; private set; }
        public string RangeTo { get; private set; }
        public ExpressionListNode ExpressionList { get; private set; }
        
    }
}
