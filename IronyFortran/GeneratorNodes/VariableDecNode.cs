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
            var variablesToGenerate = _variables.Where(v => !aContext.IsInputParameter(v.Name)).ToList();
            if (!variablesToGenerate.Any())
                return;
            var lastVariable = variablesToGenerate.Last();

            foreach (var variable in variablesToGenerate)
            {
                if(variable != variablesToGenerate[0])
                    aSB.Append(Indent(anIndent));
                aSB.Append(variable.IsArray ? "VDIArray<" + variable.Type + ">" : variable.Type);
                aSB.AppendFormat(" {0} = {1}", variable.Name, variable.IsArray ? "new VDIArray<" + variable.Type + ">()" : GetDefaultValue(variable.Type));
                if(variable != lastVariable)
                    aSB.AppendLine(";");
            }
        }

        protected override void InitInternal(AstContext context, ParseTreeNode treeNode)
        {           
            var nodes = treeNode.GetMappedChildNodes();
            Type = MapType(nodes[0]);
            _variables = nodes[1].ChildNodes.Select(n => new Variable(Type, n)).ToList();
        }

        public string Type { get; private set; }

        private List<Variable> _variables;
        public IEnumerable<Variable> Variables { get { return _variables; } }
    }
}
