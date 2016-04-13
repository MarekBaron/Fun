using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace IronyFortran.GeneratorNodes
{
    public class ExpressionNode : BaseNode
    {
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            if (IsLiteralValue)
                aSB.Append(LiteralValue);
            else
                aSB.Append("<expressionValue");
        }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            if(parseNode.ChildNodes.Count == 1 && parseNode.ChildNodes[0].Term.Name == "value")
            {
                var valueNode = parseNode.ChildNodes[0].ChildNodes[0];
                var value = valueNode.Token.ValueString;
                if (valueNode.Term.Name == "stringValue")
                    value = PreprocesString(value);
                LiteralValue = value;
            }
        }

        private string PreprocesString(string aValue)
        {
            return "@\"" + aValue.Replace("\"", "\"\"") + "\"";
        }

        /// <summary>
        /// Jeśli true, to wyrażenie jest typu literalnego (np. string, liczba)
        /// Jeśli false, to wyrażenie jest typu złożonego
        /// </summary>
        public bool IsLiteralValue { get { return !String.IsNullOrEmpty(LiteralValue); } }

        public string LiteralValue { get; private set; }

        public override string ToString()
        {
            return String.Format("ExpressionNode: {0}", IsLiteralValue ? "Literal = <" + LiteralValue + ">" : "complex");
        }
    }
}
