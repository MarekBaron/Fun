using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronyFortran.GeneratorNodes
{
    public class VariableDecNode : StatementNode
    {
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            foreach (var variable in Variables.Where(v => !aContext.IsInputParameter(v.Name)))
            {
                aSB.Append(Indent(anIndent));
                aSB.Append(variable.IsArray ? "VDIArray<" + variable.Type + ">" : variable.Type);
                aSB.AppendFormat(" {0} = {1};", variable.Name, variable.IsArray ? "new VDIArray<" + variable.Type + ">()" : GetDefaultValue(variable.Type));
                aSB.AppendLine();
            }
        }

        protected override void InitInternal(AstContext context, ParseTreeNode treeNode)
        {           
            var nodes = treeNode.GetMappedChildNodes();
            Type = nodes[0].Token == null ? "string" : MapType(nodes[0].Token.ValueString);
            _variables = nodes[1].ChildNodes.Select(n => new Variable(Type, n)).ToList();
        }

        public string Type { get; private set; }

        private List<Variable> _variables;
        public IEnumerable<Variable> Variables { get { return _variables; } }
    }
}
