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
            {
                aSB.Append(Indent(anIndent));
                stmnt.Generate(aContext, anIndent, aSB);
                aSB.AppendLine(";");
            }
        }

        protected override void InitInternal(AstContext context, ParseTreeNode treeNode)
        {           
            var nodes = treeNode.GetMappedChildNodes();
            _statements = nodes
                .Select(n => n.AstNode)
                .Cast<BaseNode>()
                .ToList();
        }

        private List<BaseNode> _statements;

        public IEnumerable<BaseNode> Statements { get { return _statements; } }
        
        public bool IsSingle { get { return _statements.Count == 1; } }

    }
}
