using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronyFortran.GeneratorNodes
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class StatementNode : BaseNode
    {
        /// <summary>
        /// Węzeł powinien zwrócic true, jeśli z jakiegoś powodu nie generuje żadnej linii kodu
        /// </summary>
        public virtual bool IsEmpty(GenerationContext aContext) { return false; }
    }
}
