using Irony.Ast;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronyFortran.GeneratorNodes
{
    public class IdentifierValueNode : LiteralValueNode
    {
        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            //wszystkie identyfikatory zmieniamy na uppercase - w VDI są one case insensitive
            Value = parseNode.Token.ValueString.ToUpper();
        }
    }
}
