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
    public class ElseIfClauseNode : StatementNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aContext"></param>
        /// <param name="anIndent"></param>
        /// <param name="aSB"></param>
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            if (IsEmpty)
                return;
            aSB.Append("else if(");
            Condition.Generate(aContext, anIndent, aSB);
            aSB.AppendLine(")");
            aSB.Append(Indent(anIndent));
            aSB.AppendLine("{");
            Statements.Generate(aContext, anIndent + 1, aSB);
            aSB.AppendFormat("{0}}}", Indent(anIndent));
        }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            IsEmpty = !parseNode.ChildNodes.Any();
            if (IsEmpty)
                return;
            Condition = (BaseNode)parseNode.ChildNodes[0].AstNode;
            Statements = (BaseNode)parseNode.ChildNodes[1].AstNode;
        }

        public bool IsEmpty { get; private set; }
        public BaseNode Condition { get; private set; }
        public BaseNode Statements { get; private set; }

    }
}
