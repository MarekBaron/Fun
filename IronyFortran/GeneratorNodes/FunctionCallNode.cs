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
    public class FunctionCallNode : StatementNode
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
                    FunctionOrArrayName.Generate(aContext, anIndent, aSB);
                    aSB.Append("(");
                    Parameters.GenerateVdicwert(aContext, anIndent, aSB);
                    aSB.Append(")");
                }
                else if(!_embeddedFunctionNames.Contains(FunctionOrArrayName.Value))
                {
                    //funkcja predefiniowana
                    GenerateFunctionCallWithRefParams(aContext, FunctionOrArrayName.Value, Parameters, aSB);                    
                }
                else
                {
                    FunctionOrArrayName.Generate(aContext, anIndent, aSB);
                    aSB.Append("(");
                    Parameters.Generate(aContext, anIndent, aSB);
                    aSB.Append(")");
                }                
            }
        }

        private void GenerateFunctionCallWithRefParams(GenerationContext aContext, string aFunctionName, ExpressionListNode aParameters, StringBuilder aSB)
        {
            //w Fortranie, o zgrozo, parametry do funcji są standardowo przekazywane jako referencje.
            //Najprostrzym rozwiązaniem byłoby generowanie po stronie C# metody z samymi ref'ami, ale, to zadziała tylko wtedy, 
            //gdy parametrami wywołania będą tylko zmienne. Jeśli pojawi się tam wyrażenie lub stała, to kod po stronie C# się nie skompiluje
            //bo przez ref można przekazać tylko 'assignable variable'
            //Rozwiązanie: docelowa funkcja ma wszystkie parametry z ref. My analizujemy jej miejsca wywołania i generujemy funkcje wrappery, które mają
            //jako ref opisane te paramtery, które rzeczywiście w tym miejscu wywołania są refami. W funkcji wrappera dodajemy zmienne lokalne na te parametry
            //które są przekazywane w danym wywołaniu przez value, przypisujemy do tych zmiennych przekazane wartości i przekazujemy te zmienne do oryginalnej funkcji
            var refParamPositions = aParameters.Expressions.Select(exp => CanBePassedByReference(aContext, exp));
            var functionWrapperName = aContext.RegisterFunctionWrapper(aContext.GetFunctionHeaderNode(aFunctionName), refParamPositions);
            aSB.Append(functionWrapperName);
            aSB.Append("(");
            Parameters.GenerateRefVal(aContext, refParamPositions, aSB);
            aSB.Append(")");
        }

        private bool CanBePassedByReference(GenerationContext aContext, BaseNode anExpression)
        {
            if (anExpression is IdentifierValueNode)
                return true;
            var fcn = anExpression as FunctionCallNode;
            if (fcn != null && aContext.IsArray(fcn.FunctionOrArrayName.Value))
                return true;
            return false;
        }

        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            FunctionOrArrayName = (IdentifierValueNode)parseNode.ChildNodes[0].AstNode;
            Parameters = (ExpressionListNode)parseNode.ChildNodes[1].AstNode;

        }

        public IdentifierValueNode FunctionOrArrayName { get; private set; }
        public ExpressionListNode Parameters { get; private set; }

        /// <summary>
        /// HashSet zawierajacy nazwy wszystkich metod protected z ImporterBase (czyli nazwy metod wbudowanych + kilka dodatkowych, nieistotnych)
        /// </summary>
        private static HashSet<string> _embeddedFunctionNames = GetEmbeddedFunctionNames();
        private static HashSet<string> GetEmbeddedFunctionNames()
        {
            //poniższy kod odpowiada temu, co jest w oryginalnym FunctionTranslatorze (i ta metoda jest dobra)
            //nie mogę jej jednak zastosować, bo nie mam ImporterBase
            //Type t = typeof(ImporterBase);

            //var protectedMethodNames = t.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
            //   .Where(m => !m.IsPrivate)
            //   .Select(m => m.Name);

            //return new HashSet<string>(protectedMethodNames);

            //zastępczo, tworzę hashset z palucha
            return new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
            {
                "VDICSTRING",
                "VDICWERT",
                "VDIERSTER",
                "VDIFOLGE",
                "VdiIndex",                
                "VDIISTRING",
                "VdiIWert",
                "VdiLetzter",
                "VdiRWert",
                "VDISTRING",                
            };
        }
    }
}
