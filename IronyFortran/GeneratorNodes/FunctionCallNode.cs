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
    /// Uwaga: tutaj także wpada odwołanie do tablicy!!!
    /// </summary>
    public class FunctionCallNode : BaseNode
    {
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            if (aContext.IsArray(FunctionOrArrayName.Value))
            {
                FunctionOrArrayName.Generate(aContext, anIndent, aSB);
                aSB.Append("[");
                Parameters.Generate(aContext, anIndent, aSB);
                aSB.Append("]");
            }
            else
            {
                FunctionOrArrayName.Generate(aContext, anIndent, aSB);
                aSB.Append("(");
                if (FunctionOrArrayName.Value.ToUpper() == "VDICWERT")
                {
                    //funkcjawbudowana VDICWERT posiada sygnaturę:
                    // CHARACTER(256) FUNCTION VDICWERT (SATZARTNAME1,INDEX1,SATZARTNAME2,INDEX2,,,,SATZARTNAMEN,INDEXN,FELDNUMMER,LWERT)
                    //Czyli:
                    // - najpierw dowolna liczba par parametrów
                    // - potem numer pola
                    // - na końcu int, który jest parametrem WYJŚCIOWYM!!!
                    //C# nie pozwala stworzyć metody z taką sygnaturą, musieliśmy ostatni parametr przenieść na początek listy i dodać mu 'out'
                    //W związku z tym, musimy w czasie translacji zm,odyfikować jedno wywołanie na inne
                    Parameters.GenerateVdicwert(aContext, anIndent, aSB);
                }
                else
                {
                    Parameters.Generate(aContext, anIndent, aSB);
                }
                aSB.Append(")");
            }
        }

       

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            FunctionOrArrayName = (IdentifierValueNode)parseNode.ChildNodes[0].AstNode;
            Parameters = (ExpressionListNode)parseNode.ChildNodes[1].AstNode;
        }

        public IdentifierValueNode FunctionOrArrayName { get; private set; }
        public ExpressionListNode Parameters { get; private set; }
    }
}
