using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace IronyFortran.GeneratorNodes
{
    public class ElseIfClauseListNode : BaseNode
    {
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            for(var i = 0; i<_clauses.Count; i++)
            {
                aSB.Append(Indent(anIndent));
                _clauses[i].Generate(aContext, anIndent, aSB);
                if(i != _clauses.Count - 1) //dla ostatnieg nie stawiamy entera
                    aSB.AppendLine();
            }
        }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            var nodes = parseNode.GetMappedChildNodes();
            _clauses = nodes
                .Select(n => n.AstNode)
                .Cast<BaseNode>()
                .ToList();
        }

        private List<BaseNode> _clauses;

        public IEnumerable<BaseNode> Clauses { get { return _clauses; } }
        public bool IsSingle { get { return _clauses.Count == 1; } }
        public bool IsEmpty { get { return _clauses.Count == 0; } }
    }
}
