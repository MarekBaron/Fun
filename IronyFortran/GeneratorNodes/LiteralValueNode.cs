using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace IronyFortran.GeneratorNodes
{
    public class LiteralValueNode : BaseNode
    {
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            aSB.Append(Value);
        }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            Value = parseNode.Token.ValueString;
        }

        public String Value { get; protected set; }
    }
}
