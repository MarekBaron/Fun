using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;
using System.Collections;

namespace IronyFortran.GeneratorNodes
{    
    public abstract class BaseNode : IAstNodeInit, IBrowsableAstNode
    {
        public abstract void Generate(GenerationContext aContext, int anIndent, StringBuilder aSB);

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
                indent = new string(' ', anIndentValue * 3);
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

        protected string GetDefaultValue(string aType)
        {
            var result = String.Empty;
            if (!_defaultValues.TryGetValue(aType, out result))
                throw new ArgumentException("Nieznana wartość domyślna dla typu: " + aType);
            return result;
        }

        public void Init(AstContext aContext, ParseTreeNode aParseNode)
        {
            Span = aParseNode.Span;
            _childNodes = aParseNode.GetMappedChildNodes()
                .Select(n => n.AstNode)
                .OfType<BaseNode>()
                .ToList();
            foreach (var child in _childNodes)
                child.Parent = this;
            InitInternal(aContext, aParseNode);
        }

        protected abstract void InitInternal(AstContext context, ParseTreeNode parseNode);

        public IEnumerable GetChildNodes()
        {
            return _childNodes;
        }

        private static Dictionary<string, string> _typeMapping = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"INTEGER", "int" },
            {"LOGICAL", "bool" }
        };

        private static Dictionary<string, string> _defaultValues = new Dictionary<string, string>()
        {
            {"int", "0" },
            {"string", "String.Empty" },
            {"bool", "false" }
        };

        public int Position
        {
            get
            {
                return Span.Location.Position;
            }
        }

        public SourceSpan Span { get; private set; }
        public BaseNode Parent { get; set; }
        private List<BaseNode> _childNodes;
    }
}
