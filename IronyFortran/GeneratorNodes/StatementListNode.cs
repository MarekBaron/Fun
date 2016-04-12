using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace IronyFortran.GeneratorNodes
{
    public class StatementListNode : BaseNode
    {
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            foreach (var stmnt in _statements)
                stmnt.Generate(aContext, anIndent, aSB);
        }

        protected override void InitInternal(AstContext context, ParseTreeNode treeNode)
        {           
            var nodes = treeNode.GetMappedChildNodes();
            _statements = nodes
                .Select(n => n.AstNode)
                .Cast<StatementNode>()
                .ToList();
        }

        private List<StatementNode> _statements;

        public IEnumerable<StatementNode> Statements { get { return _statements; } }

    }
}
