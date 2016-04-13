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

        public bool IsInputParameter(string aVariableName)
        {
            return GetVariable(aVariableName).IsFunctionParameter;
        }

        public Variable GetVariable(string aVariableName, string aFunctionName = null)
        {
            var functionName = aFunctionName != null ? aFunctionName : CurrentFunctionName;
            return _variables[functionName][aVariableName];
        }
        
        public void MarkAsInputParameter(string aFunctionName, string aParameterName)
        {            
            GetVariable(aParameterName, aFunctionName).IsFunctionParameter = true;
        }

        /// <summary>
        /// Zwraca typ zmiennej w kontekście aktualnej funkcji
        /// </summary>
        /// <param name="aVariableName"></param>
        /// <returns></returns>
        public string VariableType(string aVariableName)
        {
            return GetVariable(aVariableName).Type;
        }

        public string CurrentFunctionName { get; set; }
    }
}
