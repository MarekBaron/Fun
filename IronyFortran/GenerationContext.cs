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

        public bool IsArray(string aVariableName)
        {
            return IsVariable(aVariableName) && GetVariable(aVariableName).IsArray;
        }

        public bool IsVariable(string aVariableName)
        {
            return _variables[CurrentFunctionName].ContainsKey(aVariableName);
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

        public readonly Dictionary<string, FunctionWrapperData> FunctionWrappers = new Dictionary<string, FunctionWrapperData>(StringComparer.OrdinalIgnoreCase);

        public string RegisterFunctionWrapper(FunctionHeaderNode aFunctionHeader, IEnumerable<bool> anIsRefParam)
        {
            FunctionWrapperData fwd;
            if(!FunctionWrappers.TryGetValue(aFunctionHeader.Name, out fwd))
            {
                fwd = new FunctionWrapperData(aFunctionHeader);
                FunctionWrappers[fwd.FunctionName] = fwd;
            }
            return fwd.RegisterVersion(anIsRefParam);            
        }

        private readonly Dictionary<string, FunctionHeaderNode> _functions = new Dictionary<string, FunctionHeaderNode>(StringComparer.OrdinalIgnoreCase);
        public void RegisterFunction(FunctionHeaderNode aFunctionHeader)
        {
            _functions[aFunctionHeader.Name] = aFunctionHeader;
        }

        public FunctionHeaderNode GetFunctionHeaderNode(string aFunctionName)
        {
            return _functions[aFunctionName];
        }
    }
}
