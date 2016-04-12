using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronyFortran.GeneratorNodes
{
    public class Variable
    {
        public Variable(string aType, ParseTreeNode aTreeNode)
        {
            Type = aType;
            var firstChildNode = aTreeNode.ChildNodes.First();
            IsArray = aTreeNode.ChildNodes.Count > 1;
            Name = firstChildNode.Token.ValueString.ToUpper();  
        }

        public string Type { get; private set; }
        public string Name { get; private set; }
        public bool IsArray { get; private set; }
        public bool IsFunctionParameter { get; set; }
    }
}
