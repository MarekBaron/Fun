using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronyFortran.GeneratorNodes
{
    public class FunctionNode : BaseNode
    {
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            HeaderNode.Generate(aContext, anIndent, aSB);
            aSB.AppendLine(Indent(anIndent) + "{");
            aSB.AppendLine(String.Format("{0}{1} {2} = {3};", Indent(anIndent + 1), HeaderNode.ReturnType, Name, GetDefaultValue(HeaderNode.ReturnType)));
            StatementList.Generate(aContext, anIndent + 1, aSB);
            aSB.AppendLine(String.Format("{0}return {1};", Indent(anIndent + 1), Name));
            aSB.AppendLine(Indent(anIndent) + "}");
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            HeaderNode = (FunctionHeaderNode)treeNode.ChildNodes[0].AstNode;
            StatementList = (StatementListNode)treeNode.ChildNodes[1].AstNode;
        }

        public FunctionHeaderNode HeaderNode { get; private set; }
        public StatementListNode StatementList { get; private set; }
        public string Name { get { return HeaderNode.Name; } }
    }
}
