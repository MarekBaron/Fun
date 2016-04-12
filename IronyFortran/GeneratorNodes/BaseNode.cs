using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Interpreter.Ast;

namespace IronyFortran.GeneratorNodes
{
    //todo zrezygnować z dziedziczenia z astnode
    public abstract class BaseNode : AstNode
    {
        public abstract void Generate(int anIndent, StringBuilder aSB);

        private static Dictionary<int, string> _indents = new Dictionary<int, string>();

        /// <summary>
        /// Zwraca string odpowiadający wcięciu o zadanej wartości
        /// </summary>
        /// <param name="anIndentValue"></param>
        /// <returns></returns>
        protected string Indent(int anIndentValue)
        {
            var indent = String.Empty;
            if(!_indents.TryGetValue(anIndentValue, out indent))
            {
                indent = new string('\t', anIndentValue);
                _indents.Add(anIndentValue, indent);
            }
            return indent;
        }

        protected string MapType(string anOriginalType)
        {
            var result = String.Empty;
            if (!_typeMapping.TryGetValue(anOriginalType, out result))
                throw new ArgumentException("Nieznany typ: " + anOriginalType);
            return result;
        }

        private static Dictionary<string, string> _typeMapping = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"INTEGER", "int" },
        };
    }
}
