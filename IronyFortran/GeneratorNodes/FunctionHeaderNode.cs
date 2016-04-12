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
        class ParamDefinition
        {
            public ParamDefinition(string aName, string aType)
            {
                Name = aName;
                Type = aType;
            }

            public string Name { get; private set; }
            public string Type { get; private set; }

            public override string ToString()
            {
                return String.Format("{0} {1}", Type, Name);
            }
        }

        private string _name;
        private string _returnType;
        private readonly List<ParamDefinition> _paramDefinitions = new List<ParamDefinition>();

        public override void Generate(int anIndent, StringBuilder aSB)
        {
            aSB.AppendLine(String.Format("{0}{1} {2}({3})", 
                Indent(anIndent), 
                _returnType, 
                _name, 
                String.Join(",", _paramDefinitions.Select(pd => pd.ToString()))));            
        }

        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            _returnType = MapType(treeNode.ChildNodes[0].Token.ValueString);
            _name = treeNode.ChildNodes[2].Token.ValueString;
            foreach (var prm in treeNode.ChildNodes[3].ChildNodes)
            {
                _paramDefinitions.Add(new ParamDefinition(prm.Token.ValueString, "--todo--"));
            }
        }
    }
}
