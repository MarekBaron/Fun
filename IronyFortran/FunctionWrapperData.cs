using IronyFortran.GeneratorNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronyFortran
{
    public class FunctionWrapperData
    {
        public FunctionWrapperData(FunctionHeaderNode aHeaderNode)
        {
            FunctionHeaderNode = aHeaderNode;
            FunctionName = FunctionHeaderNode.Name;
            _versions = new HashSet<IEnumerable<bool>>();
        }

        public string FunctionName { get; private set; }
        FunctionHeaderNode FunctionHeaderNode { get; set; }
        HashSet<IEnumerable<bool>> _versions;
        IEnumerable<IEnumerable<bool>> Versions { get { return _versions; } }

        public string RegisterVersion(IEnumerable<bool> anIsRefParam)
        {
            _versions.Add(anIsRefParam);
            return WrappedFunctionName(anIsRefParam);
        }

        private string WrappedFunctionName(IEnumerable<bool> anIsRefParamData)
        {
            return FunctionName +"_" + String.Join(String.Empty, anIsRefParamData.Select(i => i ? "r" : "v"));
        }

        public void Generate(GenerationContext aContext, StringBuilder aSB)
        {
            aSB.Append("#region function wrappers for ");
            aSB.AppendLine(FunctionName);
            foreach(var isRefParamData in Versions)
            {
                aSB.AppendLine();
                aSB.AppendFormat("public {0} {1}(", FunctionHeaderNode.ReturnType, WrappedFunctionName(isRefParamData));                
                aSB.Append(FunctionHeaderNode.ParametersLine(aContext, isRefParamData));
                aSB.AppendLine(")");
                aSB.AppendLine("{");
                var paramNames = FunctionHeaderNode.ParamNames.ToArray();
                for (var i =0; i < paramNames.Length; i++)
                {
                    if(!isRefParamData.ElementAt(i))
                    {                        
                        aSB.AppendFormat("   {0} {1}_wrappedLocal = {1};", aContext.GetVariable(paramNames[i], FunctionHeaderNode.Name).Type, paramNames[i]);
                        aSB.AppendLine();
                    }
                }
                aSB.AppendFormat("   return {0}(", FunctionHeaderNode.Name);
                for (var i = 0; i < paramNames.Length; i++)
                {                    
                    aSB.AppendFormat("ref {0}", paramNames[i]);
                    if (!isRefParamData.ElementAt(i))
                        aSB.Append("_wrappedLocal");
                    if (i != paramNames.Length - 1)
                        aSB.Append(", ");                    
                }
                aSB.AppendLine(");");
                aSB.AppendLine("}");
            }

            aSB.AppendLine();
            aSB.AppendLine("#endregion");
        }
    }
}
