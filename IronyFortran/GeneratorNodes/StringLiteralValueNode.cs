using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace IronyFortran.GeneratorNodes
{
    public class StringLiteralValueNode : LiteralValueNode
    {
        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            Value = PreprocesString(parseNode.Token.ValueString);
        }

        private string PreprocesString(string aValue)
        {
            return "@\"" + aValue.Replace("\"", "\"\"") + "\"";
        }
    }
}
