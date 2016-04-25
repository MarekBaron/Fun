using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace IronyFortran.GeneratorNodes
{
    /// <summary>
    /// 
    /// </summary>
    public class BoolValueNode : BaseNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aContext"></param>
        /// <param name="anIndent"></param>
        /// <param name="aSB"></param>
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            aSB.Append(Value ? "true" : "false");
        }

        public bool Value { get; private set; }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            var originalValue = parseNode.ChildNodes[0].Token.ValueString;
            if (originalValue == ".true.")
                Value = true;
            else if (originalValue == ".false.")
                Value = false;
            else
                throw new InvalidOperationException("Nierozpoznana wartość bool: " + originalValue);
        }
    }
}
