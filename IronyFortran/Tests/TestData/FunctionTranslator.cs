using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using InstalSoft.Utils;
using System.Text.RegularExpressions;
using System.Reflection;
using InstalSoft.Utils.EnumerableExtensions;

namespace InstalSoft.CatalogModel.VDI.ScriptInterpretation
{
   /// <summary>
   /// Klasa wykorzystywana do tłumaczenia funkcji VDI na C#
   /// </summary>
   public class FunctionTranslator
   {
      /// <summary>
      /// 
      /// </summary>
      public FunctionTranslator()
      {
         var regexOptions = RegexOptions.None; // RegexOptions.Compiled;
         Tabs = String.Empty;
         _tabCount = 0;

         _typeConversion = _types.ToDictionary(t => t.RegExp.Substring(0, 4), StringComparer.InvariantCultureIgnoreCase);

         ws = @"\s+"; // obowiązkowa spacja lub wiecej
         ows = @"\s*"; // opcjonalna spacja lub wiecej
         type = @"(?<type>(" + _types.Select(t => t.RegExp).JoinStr("|") + "))" + ows;
         name = @"[A-Z][A-Z0-9_]*";
         arraydim = @"(?<arraydim>\(\d+(:\d+)?\))?";
         paramName = name + arraydim;
         paramList = @"(?<paramlist>" + paramName + @"(" + ows + "," + ows + paramName + ")*)";
         number = @"[+|-]?\d+(\.|\.\d+(e[+|-]?\d+)?)?";
         condition = @"\((?<condition>.+?)\)";

         //regex dopasowujący się do stringa zawierajacego prawidłowo sparowane nawiasy okrągłe
         //źródło: http://oreilly.com/catalog/regex2/chapter/index.html (chapter 9, Matching nested constructs)
         var expressionWithParentheses = @"([^():"",]+|\((?<DEPTH>)|\)(?<-DEPTH>))*(?(DEPTH)(?!))";
         //poniższe zapytania są wykonywane po translacji stringów i dostępu do tablicy, dlatego używaja już indeksera [...] i stringa w postci @"..."
         funcCallParam = "(" + name + @"(\[.+?\])?|@""(.*?)""|" + number + ")";
         funcCallParamWithParentheses = "(" + name + @"(\[.+?\])?|@""(.*?)""|" + number + @"|" + expressionWithParentheses + ")";
         //funcCallParamList = @"(?<paramlist>" + funcCallParam + @"(," + ows + funcCallParam + ")*)";
         funcCallParamList = @"(?<paramlist>" + funcCallParamWithParentheses + @"(," + ows + funcCallParam + ")*)";
         //funcCallParamList = @"(?<paramlist>(" + funcCallParam + ows + @",?" + ows + ")+)";

         R_function = new Regex(ows + type + ws + @"FUNCTION" + ws + @"(?<functionname>" + name + ")" + ows + @"\(" + paramList + @"\)" + ows + ";", regexOptions);
         R_varDeclaration = new Regex(ows + type + ws + paramList + ows + ";", regexOptions);
         R_quotedString = new Regex(@"'(.*?)'", regexOptions);
         R_arraySetRange = new Regex(ows + "(?<arrayname>" + name + @")\(((?<s>.+):)?(?<e>.+)\)" + ows + "=" + ows + @"\(/(?<values>.*?)/\)" + ows + ";", regexOptions);
         R_singleLineIf = new Regex(ows + @"IF" + ows + condition + @"(?!" + ows + "THEN)", regexOptions);         
         R_elseIfThen = new Regex(ows + @"(?<else>ELSE)?IF" + ows + condition + ows + "THEN" + ows + ";", regexOptions);
         R_endIfDo = new Regex(ows + "END" + ows + "(IF|DO)" + ows + ";", regexOptions);
         R_else = new Regex(ows + "ELSE" + ows + ";", regexOptions);
         R_arrayAccess = new Regex(@"(?<name>" + name + @")\((?<index>" + expressionWithParentheses + @")\)", regexOptions);         
         R_power = new Regex(@"(?<first>(" + number + @"|\([^\(]+?\)))" + ows + @"\*\*" + ows + "(?<second>" + number + ")", regexOptions);
         R_arrayDim = new Regex(arraydim, regexOptions);
         R_doWhile = new Regex(ows + "DO" + ws + "WHILE" + ows + condition + ows + ";", regexOptions);
         R_vdicwert = new Regex("VDICWERT", regexOptions);
         R_escapeRegex = new Regex(@"(?<!\\)\\(.)", regexOptions);
         R_assign = new Regex("(?<name>" + paramName + ")" + ows + "=" + ows + @"(?<expr>[^=\<\>].+)" + ows + ";", regexOptions);
         R_functionCall = new Regex(@"(?<!FUNCTION" + ws + @")(?<functionname>" + name + ")" + ows + @"\(" + funcCallParamList + @"\)", regexOptions);
         R_refParamNameReplacement = new Regex(@"[^A-Z0-9_]", regexOptions);
         R_paramIsConst = new Regex(@"(""(.*?)""|[\(+\-\*\/)]|\A" + number + @"\Z)", regexOptions);
         //R_paramIsConst = new Regex(@"[\[\]\(\)""\+\-\*\/]");
      }

      /// <summary>
      /// Translacja
      /// </summary>
      /// <param name="aLines">Linie z kodem (bez identyfikatora rekordu i indeksu)</param>
      /// <returns>Przetłumaczony kod</returns>
      public IEnumerable<string> Translate(IEnumerable<string> aLines)
      {
         List<string> output = new List<string>();
         Match functionHeaderMatch = null;
         int functionHeaderIndex = 0;
         string functionName = String.Empty;
         List<string> functionParams = null;

         List<VariableDefinition> variableDefinitions = new List<VariableDefinition>();
         Match m = null;         
         foreach (var iterLine in aLines)
         {                       
            string line = UpperCaseCode(iterLine);
            //usunięcie komentarza - wszystkiego po pierwszym ';'
            line = RemoveComment(line);
            //zamiana stringów z '..' na @".."            
            line = DoQuotedStringTranslation(line);
            //wykonanie prostych translacji
            line = DoSimpleTranslations(line);
            //specjalna obsługa metody VDIWERT
            line = DoVDICWERTTranslation(line);
            //rozbudowa przypisań o rzutowanie typu
            line = DoAssignTranslation(line, variableDefinitions);
            
            m = R_function.Match(line);
            if (m.Success)
            {
               functionHeaderMatch = m;
               functionParams = GetFunctionParams(m);
               //dopiero teraz znamy nazwę funkcji więc dopiero teraz możemy stworzyć Regex'y z niej korzystajace
               functionName = m.Groups["functionname"].Value;
               R_functionEnd = new Regex(ows + "END" + ws + "FUNCTION" + ws + functionName + ows + ";");
               //zapamiętujemy indeks linii w którym ma być nagłówek
               functionHeaderIndex = output.Count();
               //dodajemy puste linie - miejsce na przyszły nagłówek funkcji
               output.Add(String.Empty);
               output.Add(String.Empty);
               output.Add(String.Empty);
               IncraseTabs();
               continue;
            }
            m = R_varDeclaration.Match(line);
            if (m.Success)
            {
               TypeSpec paramsType = ConvertType(m.Groups["type"].Value);
               var newVariables = GetVariables(m.Groups["paramlist"].Value, paramsType);
               output.AddRange(InitializeValues(newVariables, functionParams).Select(v => Tabs + v + ";"));
               variableDefinitions.AddRange(newVariables);
               continue;
            }
            line = TranslateArrayAccess(line, variableDefinitions);
            m = R_doWhile.Match(line);
            if (m.Success)
            {
               output.Add(Tabs + "while(" + m.Groups["condition"] + ")");
               output.Add(Tabs + "{");
               IncraseTabs();
               continue;
            }
            //tłumaczenie operatora potęgowania **
            line = TranslatePowerOperator(line);
            //tłumaczenie złożonych ifów
            m = R_elseIfThen.Match(line);
            if (m.Success)
            {
               var optionalElse = m.Groups["else"].Success ? "else " : "";
               if (m.Groups["else"].Success)
               {
                  DecraseTabs();
                  output.Add(Tabs + "}");
                  output.Add(Tabs + "else if(" + m.Groups["condition"] + ")");

               }
               else
               {
                  output.Add(Tabs + optionalElse + "if(" + m.Groups["condition"] + ")");
               }
               output.Add(Tabs + "{");
               IncraseTabs();
               continue;
            }
            //tłumaczenie prostych ifów - uwaga: w powyższym 'ifie' obsługującym złożone wyrażenia 'if' mamy continue, dzieki czemu poniższy kod się już nie wykona
            line = DoSingleIfTranslation(line);
            m = R_endIfDo.Match(line);
            if (m.Success)
            {
               DecraseTabs();
               output.Add(Tabs + "}");
               continue;
            }
            m = R_else.Match(line);
            if (m.Success)
            {
               DecraseTabs();
               output.Add(Tabs + "}");
               output.Add(Tabs + "else");
               output.Add(Tabs + "{");
               IncraseTabs();
               continue;
            }
            m = R_arraySetRange.Match(line);
            if (m.Success)
            {
               var startIndex = m.Groups["s"].Success ? m.Groups["s"].Value : "1";               
               output.Add(Tabs + m.Groups["arrayname"] + ".SetRange(" + startIndex + ", " + m.Groups["e"] + ", " + m.Groups["values"] + ");");
               continue;
            }
            m = R_functionCall.Match(line);
            if (m.Success)
            {
               if (!_embeddedFunctionNames.Contains(m.Groups["functionname"].Value))
               {
                  output.AddRange(DoFunctionCallTranslation(m, line));
                  continue;
               }
            }
            if (functionHeaderMatch != null)
            {
               m = R_functionEnd.Match(line);
               if (m.Success)
               {
                  output.Add(Tabs + "return " + functionName + ";");
                  DecraseTabs();
                  output.Add(Tabs + "}");

                  var functionParamsString = CreateFunctionParamsString(functionParams, variableDefinitions);
                  var functionReturnType = ConvertType(functionHeaderMatch.Groups["type"].Value).Name;
                  var initValue = _types.First(ts => ts.Name == functionReturnType).DefaultValue;
                  output[functionHeaderIndex] = "public " + (functionName == "TGA_810" ? "override " : "") + functionReturnType + " " + functionName + "(" + functionParamsString + ")";
                  output[functionHeaderIndex + 1] = "{";
                  output[functionHeaderIndex + 2] = "   " + functionReturnType + " " + functionName + " = " + initValue + ";";

                  continue;
               }
            }
            //jesli doszliśmy tutaj (nie trafiliśmy na continue) to przepisujemy linię bez zmian
            output.Add(Tabs + line);
         }
         return output;
      }

      private string RemoveComment(string aLine)
      {
         var insideString = false;
         var semicolonIndex = -1;
         for (var i = 0; i < aLine.Length; i++)
         {
            if (aLine[i] == '\'')
               insideString = !insideString;
            if (aLine[i] == ';' && !insideString)
            {
               semicolonIndex = i;
               break;
            }
         }         
         if (semicolonIndex >= 0)
            return aLine.Substring(0, semicolonIndex + 1);
         return aLine;
      }

      private IEnumerable<string> DoFunctionCallTranslation(Match aMatch, string aLine)
      {
         List<string> output = new List<string>();

         var paramNames = GetFunctionCallParams(aMatch.Groups["paramlist"].Value);
         var paramList = paramNames.Select(n =>
            new
            {
               Name = n,
               RefName = GetRefParamName(n) + '_' + (_refParamCount++),
               IsConst = RefParamIsConst(n)
            }
         ).ToList();

         foreach (var p in paramList)
            output.Add(Tabs + "var " + p.RefName + "=" + p.Name + ";");

         output.Add(Tabs + aLine.Substring(0, aMatch.Index) //część przed wywołaniem funkcji
            + aMatch.Groups["functionname"].Value + "(" + paramList.Select(p => "ref " + p.RefName).JoinStr(",") + ")" //zmodyfikowane wywołanie funkcji
            + aLine.Substring(aMatch.Index + aMatch.Length)); //reszta linii za wywołaniem funkcji

         foreach (var p in paramList)
            output.Add(Tabs + (p.IsConst ? "//" : "") + p.Name + "=" + p.RefName + ";");

         return output;
      }

      private bool RefParamIsConst(string aParam)
      {
         //jest const gdy:
         // - jest wyrażeniem, np (iidx + 2)
         // - jest stringiem, np @"BASE"
         // - jest liczbą, np. 0
         return R_paramIsConst.IsMatch(aParam);
      }

      private string GetRefParamName(string aParam)
      {
         var name = R_refParamNameReplacement.Replace(aParam, "_");
         if (Char.IsDigit(name[0]))
            name = "_" + name;
         return name;
      }

      /// <summary>
      /// Rozbija string
      /// </summary>
      /// <param name="aParamsString"></param>
      /// <returns></returns>
      private IEnumerable<string> GetFunctionCallParams(string aParamsString)
      {
         List<string> paramList = new List<string>();
         bool insideString = false;
         bool previousWasEscape = false;
         StringBuilder sb = new StringBuilder();
         foreach (char c in aParamsString)
         {
            if (c == '"' && !previousWasEscape)
               insideString = !insideString;
            previousWasEscape = c == '\\' ? true : false;

            if (!insideString && c == ',')
            {
               paramList.Add(sb.ToString().Trim());
               sb.Clear();
            }
            else
            {
               sb.Append(c);
            }
         }
         paramList.Add(sb.ToString().Trim());

         return paramList;
      }

      private string DoAssignTranslation(string aLine, IEnumerable<VariableDefinition> aVariableDefinitions)
      {
         var m = R_assign.Match(aLine);
         if (m.Success)
         {
            var name = m.Groups["name"].Value;
            var variableDef = aVariableDefinitions.FirstOrDefault(vd => vd.Name == name);
            if (variableDef != null)
            {
               var expr = m.Groups["expr"];
               aLine = aLine.Substring(0, m.Index) + name + "=" + variableDef.TypeSpec.GetTypeConvertion(expr.ToString()) + "(" + expr + ");";
            }
         }
         return aLine;
      }

      private string DoQuotedStringTranslation(string aLine)
      {
         return R_quotedString.Replace(aLine, m =>
         {
            var quotedString = m.Groups[1].Value;
            //obsługujemy znaki '\' wykorzystywane do escapowania
            quotedString = R_escapeRegex.Replace(quotedString, esc => esc.Groups[1].Value);

            return "@\"" + quotedString.Replace("\"", "\"\"") + "\"";
         });
      }

      private string DoVDICWERTTranslation(string aLine)
      {
         //funkcjawbudowana VDICWERT posiada sygnaturę:
         // CHARACTER(256) FUNCTION VDICWERT (SATZARTNAME1,INDEX1,SATZARTNAME2,INDEX2,,,,SATZARTNAMEN,INDEXN,FELDNUMMER,LWERT)
         //Czyli:
         // - najpierw dowolna liczba par parametrów
         // - potem numer pola
         // - na końcu int, który jest parametrem WYJŚCIOWYM!!!
         //C# nie pozwala stworzyć metody z taką sygnaturą, musieliśmy ostatni parametr przenieść na początek listy i dodać mu 'out'
         //W związku z tym, musimy w czasie translacji zm,odyfikować jedno wywołanie na inne
         Match m = R_vdicwert.Match(aLine);
         if (m.Success)
         {
            int notClosedBracesCount = 0;
            string lastParameter = String.Empty;
            int lastCommaIndex = 0;
            int openBraceIndex = m.Index + m.Length;
            for (int i = openBraceIndex; i < aLine.Length; i++)
            {
               switch (aLine[i])
               {
                  case '(':
                     notClosedBracesCount++;
                     break;
                  case ')':
                     notClosedBracesCount--;
                     break;
                  case ',':
                     lastCommaIndex = i;
                     lastParameter = String.Empty;
                     break;
                  default:
                     lastParameter += aLine[i];
                     break;
               }
               if (notClosedBracesCount == 0)
               {
                  //osiągnęliśmy nawias zamykający wywołania funkcji VDICWERT
                  break;
               }
            }
            //usuwamy ostatni parametr
            aLine = aLine.Remove(lastCommaIndex, lastParameter.Length + 1);
            //dodajemy parametr jako pierwszy w wywołaniu
            aLine = aLine.Insert(openBraceIndex + 1, "out " + lastParameter.Trim() + ", ");
         }
         return aLine;
      }

      private string DoSingleIfTranslation(string line)
      {
         return R_singleLineIf.Replace(line, m => "if(" + m.Groups["condition"].Value + ")");
      }

      /// <summary>
      /// Zamienia wszystkie znaki w lini na duże - pomija teksty objęte znakami '' (czyli zawartość stringów)
      /// </summary>
      /// <param name="aLine"></param>
      /// <returns></returns>
      private string UpperCaseCode(string aLine)
      {
         StringBuilder sb = new StringBuilder(aLine.Length);
         bool insideString = false;
         bool previousWasEscape = false;
         foreach (char c in aLine)
         {
            if (c == '\'' && !previousWasEscape)
               insideString = !insideString;
            previousWasEscape = c == '\\' ? true : false;
            sb.Append(insideString ? c : Char.ToUpper(c));
         }
         return sb.ToString();
      }

      private string TranslateArrayAccess(string aLine, IEnumerable<VariableDefinition> aVariableDefinitions)
      {
         return R_arrayAccess.Replace(aLine, m =>
         {
            var translatedIndex = TranslateArrayAccess(m.Groups["index"].Value, aVariableDefinitions);
            var name = m.Groups["name"].Value;
            if (aVariableDefinitions.FirstOrDefault(vd => vd.Name == name) != null)
            {
               return name + "[" + translatedIndex + "]"; //to był dostęp do tablicy - zamieniamy nawiasy na kwadratowe
            }
            else
               return " " + name + "(" + translatedIndex + ")"; //wywołanie funkcji - pozostawiamy zwykłe nawiasy, ale wstawiamy do nich przetworzony indeks
         });
      }

      private string TranslatePowerOperator(string aLine)
      {
         return R_power.Replace(aLine, match =>
         {
            return "Math.Pow((double)" + match.Groups["first"] + ",(double)" + match.Groups["second"] + ")";
         });
      }

      private string DoSimpleTranslations(string aLine)
      {
         var line = aLine;
         foreach (var p in _simpleTranslations)
         {
            line = p.First.Replace(line, p.Second);
         }
         return line;
      }

      #region Tabs
      private string Tabs { get; set; }

      private void IncraseTabs()
      {
         _tabCount++;
         var sb = new StringBuilder();
         for (var i = 0; i < _tabCount; i++)
            sb.Append("   ");
         Tabs = sb.ToString();
      }

      private void DecraseTabs()
      {
         _tabCount--;
         var sb = new StringBuilder();
         for (var i = 0; i < _tabCount; i++)
            sb.Append("   ");
         Tabs = sb.ToString();
      }
      #endregion

      private string CreateFunctionParamsString(List<string> aFunctionParams, List<VariableDefinition> aVariableDefinitions)
      {
         return aFunctionParams.Select(fp =>
         {
            var vd = aVariableDefinitions.Find(v => v.Name == fp);
            return "ref " + (vd.IsArray ? "VDIArray<" + vd.TypeSpec.Name + ">" : vd.TypeSpec.Name) + " " + fp;
         }).JoinStr(", ");
      }

      private List<string> GetFunctionParams(Match aFunctionHeaderMatch)
      {
         return aFunctionHeaderMatch.Groups["paramlist"].Value
            .Split(',')
            .Select(p => p.Trim())
            .ToList();
      }

      private IEnumerable<VariableDefinition> GetVariables(string aParamsList, TypeSpec aParamsType)
      {
         return aParamsList.Split(',')
            .Select(n => n.Trim())
            .Select(n =>
            {
               if (n.Contains('('))
                  return new VariableDefinition(R_arrayDim.Replace(n, String.Empty), aParamsType, true);
               else
                  return new VariableDefinition(n, aParamsType, false);
            })
            .ToList();
      }

      private IEnumerable<string> InitializeValues(IEnumerable<VariableDefinition> aVariables, IEnumerable<string> aParamsToFilter)
      {
         return aVariables.Where(vd => !aParamsToFilter.Contains(vd.Name))
            .Select(vd =>
            {
               if (vd.IsArray)
                  return "VDIArray<" + vd.TypeSpec.Name + "> " + vd.Name + " = new VDIArray<" + vd.TypeSpec.Name + ">()";
               else
                  return vd.TypeSpec.Name + " " + vd.Name + " = " + vd.TypeSpec.DefaultValue;
            });
      }

      private TypeSpec ConvertType(string aVDIType)
      {
         //do porówniania bierzemy tylko 4 pierwsze znaki
         return _typeConversion[aVDIType.Substring(0, 4)];
      }

      private int _tabCount = 0;
      private int _refParamCount = 1;

      private string type;
      private string ows;
      private string ws;
      private string paramList;
      private string name;
      private string paramName;
      private string number;
      private string arraydim;
      private string condition;
      private string funcCallParam;
      private string funcCallParamWithParentheses;
      private string funcCallParamList;

      private Regex R_function;
      private Regex R_varDeclaration;
      private Regex R_quotedString;
      private Regex R_functionEnd;
      private Regex R_arraySetRange;
      private Regex R_arrayAccess;
      private Regex R_singleLineIf;
      private Regex R_elseIfThen;
      private Regex R_endIfDo;
      private Regex R_else;
      private Regex R_power;
      private Regex R_arrayDim;
      private Regex R_doWhile;
      private Regex R_vdicwert;
      private Regex R_escapeRegex;
      private Regex R_functionCall;
      private Regex R_refParamNameReplacement;
      private Regex R_paramIsConst;
      private Regex R_assign;

      private IEnumerable<TypeSpec> _types = new List<TypeSpec>()
      {
         new TypeSpec(@"REAL", "0", "double", true),
         new TypeSpec(@"LOGICAL", "false", "bool", true),
         new TypeSpec(@"INTEGER", "0", "int", true),
         new TypeSpec(@"CHARACTER\s*\(\d+\)", "String.Empty", "string", false),
         new TypeSpec(@"DOUBLE PRECISION", "0", "double", true),
      };

      private static Dictionary<string, string> _simpleTranslationsDefinition = new Dictionary<string, string>()
      {
         {@"\.AND\.", "&&"},
         {@"\.OR\.", "||"},
         {@"\.NOT\.", "!"},
         {@"\.EQV\.", "=="},
         {@"\.NEQV\.", "!="},
         {@"\.XOR\.", "^"},
         {@"/=", "!="},
         {@"\.TRUE\.", "true"},
         {@"\.FALSE\.", "false"}
      };

      private IEnumerable<Pair<Regex, string>> _simpleTranslations = _simpleTranslationsDefinition
         .Select(p => new Pair<Regex, string>(new Regex(p.Key), p.Value))
         .ToList();

      /// <summary>
      /// HashSet zawierajacy nazwy wszystkich metod protected z ImporterBase (czyli nazwy metod wbudowanych + kilka dodatkowych, nieistotnych)
      /// </summary>
      private HashSet<string> _embeddedFunctionNames = GetEmbeddedFunctionNames();
      private static HashSet<string> GetEmbeddedFunctionNames()
      {
         Type t = typeof(ImporterBase);

         var protectedMethodNames = t.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
            .Where(m => !m.IsPrivate)
            .Select(m => m.Name);

         return new HashSet<string>(protectedMethodNames);
      }


      private Dictionary<string, TypeSpec> _typeConversion;
   }

   class TypeSpec
   {
      public TypeSpec(string aRegExp, string aDefaultValue, string aName, bool aRequiresStringParsing)
      {
         Name = aName;
         RegExp = aRegExp;
         DefaultValue = aDefaultValue;
         RequiresStringParsing = aRequiresStringParsing;
      }

      public string Name { get; set; }
      public bool RequiresStringParsing { get; private set; }
      public string RegExp { get; set; }
      public string DefaultValue { get; set; }
      public string GetTypeConvertion(string aConvertedValue)
      {
         string customTypeConvertion = null;
         if (RequiresStringParsing && aConvertedValue.Trim().FirstOrDefault() == '@') // jesli string to musimy zrobic Parse. Jesli to string, to jako pierwszy znak ma @
            customTypeConvertion = Name + ".Parse";
         return customTypeConvertion ?? "(" + Name + ")";
      }
   }

   class VariableDefinition
   {
      public VariableDefinition(string aName, TypeSpec aTypeSpec, bool anIsArray)
      {
         Name = aName;
         TypeSpec = aTypeSpec;
         IsArray = anIsArray;
      }
      public string Name { get; private set; }
      public TypeSpec TypeSpec { get; private set; }
      public bool IsArray { get; private set; }
   }
}
