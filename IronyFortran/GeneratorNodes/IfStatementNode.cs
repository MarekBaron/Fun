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
    public class IfStatementNode : StatementNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aContext"></param>
        /// <param name="anIndent"></param>
        /// <param name="aSB"></param>
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {            
            aSB.Append("if(");
            Condition.Generate(aContext, anIndent, aSB);
            aSB.AppendLine(")");
            aSB.Append(Indent(anIndent));
            aSB.AppendLine("{");
            TrueStatements.Generate(aContext, anIndent + 1, aSB);
            aSB.AppendFormat("{0}}}", Indent(anIndent));
            if (!ElseIfClauseList.IsEmpty)
            {
                aSB.AppendLine();
                ElseIfClauseList.Generate(aContext, anIndent, aSB);                
            }
            if(ElseStatements != null)
            {
                aSB.AppendLine();
                aSB.AppendFormat("{0}else", Indent(anIndent));
                aSB.AppendLine();
                aSB.AppendFormat("{0}{{", Indent(anIndent));
                aSB.AppendLine();
                ElseStatements.Generate(aContext, anIndent + 1, aSB);
                aSB.AppendFormat("{0}}}", Indent(anIndent));                
            };         
        }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {            
            Condition = (BaseNode)parseNode.ChildNodes[0].AstNode;
            TrueStatements = (BaseNode)parseNode.ChildNodes[1].AstNode;
            ElseIfClauseList = (ElseIfClauseListNode)parseNode.ChildNodes[2].AstNode;
            if(parseNode.ChildNodes[3].ChildNodes.Any())
                ElseStatements = (BaseNode)parseNode.ChildNodes[3].ChildNodes[0].AstNode;

        }

        public BaseNode Condition { get; private set; }
        public BaseNode TrueStatements { get; private set; }
        public ElseIfClauseListNode ElseIfClauseList { get; private set; }
        public BaseNode ElseStatements { get; private set; }
        
    }
}
