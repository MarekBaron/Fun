using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace IronyFortran.GeneratorNodes
{
    /// <summary/>    
    public class ExpressionListNode : BaseNode
    {
        /// <summary/>
        public override void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            GenerateInternal(aContext, anIndent, aSB, false);
        }

        private void GenerateInternal(GenerationContext aContext, int anIndent, StringBuilder aSB, bool aForVdicwert)
        {
            var exprsToGenerate = _expressions;
            if (aForVdicwert)
            {
                //przenosimy ostatni na początek listy                
                var last = exprsToGenerate.Last();
                exprsToGenerate.Remove(last);
                exprsToGenerate.Insert(0, last);
            }
            if (aForVdicwert)
                aSB.Append("out ");           
            //dziwna konstrukcja po to, żeby nie generować przecinka po ostatnim expression
            for (int i = 0; i < exprsToGenerate.Count - 1; i++)
            {
                exprsToGenerate[i].Generate(aContext, anIndent, aSB);
                aSB.Append(", ");
            }
            exprsToGenerate.Last().Generate(aContext, anIndent, aSB);            
        }

        /// <summary>
        /// Przd generacją przenosi pierwsze expression na koniec listy i generuje go jako out
        /// </summary>
        /// <param name="aContext"></param>
        /// <param name="anIndent"></param>
        /// <param name="aSB"></param>
        public void GenerateVdicwert(GenerationContext aContext, int anIndent, StringBuilder aSB)
        {
            GenerateInternal(aContext, anIndent, aSB, true);
        }    

        /// <summary/>
        protected override void InitInternal(AstContext context, ParseTreeNode parseNode)
        {
            _expressions = parseNode.ChildNodes
                .Select(n => n.AstNode)
                .Cast<BaseNode>()
                .ToList();
        }

        private List<BaseNode> _expressions;

        /// <summary/>
        public IEnumerable<BaseNode> Expressions { get { return _expressions; } }
    }
}
