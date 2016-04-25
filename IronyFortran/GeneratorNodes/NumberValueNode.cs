using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace IronyFortran.GeneratorNodes
{
    public class NumberValueNode : LiteralValueNode
    {
        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            base.InitInternal(context, parseNode);
            Value = Value.Replace(",", ".");
        }
    }
}
