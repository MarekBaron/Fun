using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronyFortran.GeneratorNodes
{
    public class ProgramNode : BaseNode
    {
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            foreach(var function in Functions)
            {
                aContext.CurrentFunctionName = function.Name;
                function.Generate(aContext, anIndent, aSB);
                aSB.AppendLine();
            }
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {            
            base.Init(context, treeNode);
            var nodes = treeNode.GetMappedChildNodes();
            _functions = nodes
                .Select(n => n.AstNode)
                .Cast<FunctionNode>()
                .ToList();
        }

        private List<FunctionNode> _functions;

        public IEnumerable<FunctionNode> Functions { get { return _functions; } }
        
    }
}
