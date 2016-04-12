using IronyFortran.GeneratorNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronyFortran
{
    public class GenerationContext
    {
        private readonly Dictionary<string, Dictionary<string, Variable>> _variables = new Dictionary<string, Dictionary<string, Variable>>(StringComparer.InvariantCultureIgnoreCase);

        public void AddVariable(string aFunctionName, Variable aVariable)
        {
            Dictionary<string, Variable> functionVariables = null;
            if(!_variables.TryGetValue(aFunctionName, out functionVariables))
            {
                functionVariables = new Dictionary<string, Variable>(StringComparer.InvariantCultureIgnoreCase);
                _variables.Add(aFunctionName, functionVariables);               
            }
            functionVariables.Add(aVariable.Name, aVariable);
        }

        public bool IsInputParameter(string aParameterName)
        {
            return GetVariable(aParameterName).IsFunctionParameter;
        }

        public Variable GetVariable(string aParameterName, string aFunctionName = null)
        {
            var functionName = aFunctionName != null ? aFunctionName : CurrentFunctionName;
            return _variables[functionName][aParameterName];
        }
        
        public void MarkAsInputParameter(string aFunctionName, string aParameterName)
        {            
            GetVariable(aParameterName, aFunctionName).IsFunctionParameter = true;
        }

        public string CurrentFunctionName { get; set; }
    }
}
