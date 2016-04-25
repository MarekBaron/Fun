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
    public class DoWhileNode : StatementNode
    {
        /// <summary/>        
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            aSB.Append("while(");
            Condition.Generate(aContext, anIndent, aSB);
            aSB.AppendLine(")");
            aSB.Append(Indent(anIndent));
            aSB.AppendLine("{");
            Statements.Generate(aContext, anIndent + 1, aSB);
            aSB.AppendFormat("{0}}}", Indent(anIndent));
        }

        /// <summary/>
        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            Condition = (BaseNode)parseNode.ChildNodes[0].AstNode;
            Statements = (BaseNode)parseNode.ChildNodes[1].AstNode;
        }

        /// <summary/>
        public BaseNode Condition { get; private set; }
        /// <summary/>
        public BaseNode Statements { get; private set; }
    }
}
