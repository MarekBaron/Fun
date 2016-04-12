using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronyFortran
{
    public class GenerationContext
    {
        private readonly Dictionary<string, HashSet<string>> _parameterNames = new Dictionary<string, HashSet<string>>(StringComparer.InvariantCultureIgnoreCase);

        public void AddInputParameter(string aFunctionName, string aParameterName)
        {
            HashSet<string> hs = null;
            if(!_parameterNames.TryGetValue(aFunctionName, out hs))
            {
                hs = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
                _parameterNames.Add(aFunctionName, hs);
            }
        }

        public bool IsInputParameter(string aFunctionName, string aParameterName)
        {
            return _parameterNames[aFunctionName].Contains(aParameterName);
        }
    }
}
