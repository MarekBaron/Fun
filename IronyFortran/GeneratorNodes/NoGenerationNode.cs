using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronyFortran.GeneratorNodes
{
    public class NoGenerationNode : BaseNode
    {
        public override void Generate(int anIndent, StringBuilder aSB)
        {
            throw new InvalidOperationException("Wezeł nie przeznaczony do generacji");
        }

        public ParseTreeNode ParseTreeNode { get; private set; }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            ParseTreeNode = treeNode;
        }
    }
}
