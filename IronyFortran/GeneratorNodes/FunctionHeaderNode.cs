using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronyFortran.GeneratorNodes
{
    public class FunctionHeaderNode : BaseNode
    {
        public string Name { get; private set; }
        public string ReturnType { get; private set; }
                
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            aSB.AppendLine(String.Format("{0}public {1}{2} {3}({4})",
                Indent(anIndent),
                Name == "TGA_810" ? "override " : String.Empty,
                ReturnType, 
                Name, 
                String.Join(", ", ParamNames.Select(n => "ref " + GetParamSignature(aContext, n)))));
            aContext.RegisterFunction(this);
        }

        protected override void InitInternal(AstContext context, ParseTreeNode treeNode)
        {
            ReturnType = MapType(treeNode.ChildNodes[0]);
            Name = treeNode.ChildNodes[2].Token.ValueString.ToUpper();
            ParamNames = treeNode.ChildNodes[3].ChildNodes.Select(n => n.Token.ValueString).ToList();
        }

        public IEnumerable<string> ParamNames { get; private set; }

        private string GetParamSignature(GenerationContext aContext, string aParamName)
        {
            var variable = aContext.GetVariable(aParamName);
            if (variable.IsArray)
                return String.Format("VDIArray<{0}> {1}", variable.Type, variable.Name);
            else
                return String.Format("{0} {1}", variable.Type, variable.Name);
        }
    }
}
