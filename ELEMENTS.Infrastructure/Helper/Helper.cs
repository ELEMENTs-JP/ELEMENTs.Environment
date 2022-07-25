using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ELEMENTS.Infrastructure
{
    public static partial class Helper
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
       (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        // https://docs.microsoft.com/de-de/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-core-3-1 
        public static object MapPropertiesByReflection(object uiObject, object dbObject)
        {
            // check Objects 
            if (uiObject == null)
                return null;

            if (dbObject == null)
                return null;

            // Check Type 
            Type _uiType = uiObject.GetType();
            Type _dbType = dbObject.GetType();
            if (_dbType.FullName != _uiType.FullName)
                return null;

            // Iterate over Properties 
            foreach (PropertyInfo pi in _uiType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                //check 
                if (pi == null)
                    continue;

                // not mapping GUID 
                string property = pi.Name;
                if (property == "GUID")
                    continue;

                if (property == "ItemGUID")
                    continue;

                if (property == "PrincipalGUID")
                    continue;

                if (property == "MasterGUID")
                    continue;

                // Check for NotMapped 
                NotMappedAttribute notM = (NotMappedAttribute)Attribute.GetCustomAttribute(pi, typeof(NotMappedAttribute));
                if (notM != null)
                {
                    continue;
                }

                // get old value 
                object uiValue = null;
                try
                {
                    uiValue = pi.GetValue(uiObject);
                }
                catch (Exception ex)
                {

                }

                try
                {
                    // SAVE VALUE: set new Value 
                    dbObject.GetType().GetProperty(property).SetValue(dbObject, uiValue);
                }
                catch (Exception ex)
                {

                }
            }

            return dbObject;
        }
        public static int ExtractNumber(string original, int fail = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(original))
                    return 0;

                char[] numbers = original.Where(c => Char.IsNumber(c)).ToArray();
                char[] digits = original.Where(c => Char.IsDigit(c)).ToArray();

                string number = new string(numbers);
                string digit = new string(digits);

                if (!string.IsNullOrEmpty(number))
                { return int.Parse(number); }

                if (!string.IsNullOrEmpty(number))
                { return int.Parse(digit); }

                return fail;
            }
            catch
            {
                return fail;
            }
        }
        public static string ToSecureSqlValue(this object obj, string comparison = "=", bool dateWithTime = false)
        {
            string returnText = string.Empty;
            try
            {
                if (obj == null)
                    return string.Empty;

                Type typ = obj.GetType();

                // Boolean 
                if (typ == typeof(bool))
                {
                    bool bol = Convert.ToBoolean(obj);
                    returnText = ((bol == true) ? "'true'" : "'false'");
                }
                // string 
                if (typ == typeof(string) ||
                    typ == typeof(String))
                {
                    returnText = "'" + obj.ToSecureString() + "'";
                }
                // Integer 
                if (typ == typeof(int))
                {
                    returnText = "'" + obj.ToSecureString() + "'";
                }
                // Decimal // Double // Float  
                if (typ == typeof(decimal) ||
                    typ == typeof(double) ||
                    typ == typeof(float))
                {
                    returnText = "'" + obj.ToSecureString().ToString(CultureInfo.InvariantCulture) + "'";
                }
                // Date 
                if (typ == typeof(DateTime))
                {
                    DateTime dt = Convert.ToDateTime(obj);

                    if (comparison == "=")
                    {
                        returnText = " between '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "' and '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " 23:59:59.999' ";
                    }
                    else
                    {
                        if (dateWithTime == true)
                        {
                            returnText = " '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second + "' ";
                        }
                        else
                        {
                            returnText = " '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "' ";
                        }
                    }
                }
                // GUID 
                if (typ == typeof(Guid))
                {
                    returnText = "'" + obj.ToSecureString() + "'";
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

            return returnText;
        }
        public static string ToJsonString(ItemMetadata theObject)
        {
            string json = string.Empty;
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                };
                json = JsonSerializer.Serialize(theObject, options);
            }
            catch (Exception ex)
            {

            }
            return json;
        }
        public static ItemMetadata ToObjectFromJson(string json)
        {
            ItemMetadata metadata = new ItemMetadata();

            if (string.IsNullOrEmpty(json))
            {
                return metadata;
            }

            try
            {
                metadata = JsonSerializer.Deserialize<ItemMetadata>(json);
            }
            catch (Exception ex)
            {

            }
            return metadata;
        }
        public static string SplitGetLast(string text, string separator = ".")
        {
            try
            {
                string[] arr = text.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                int length = arr.Length;
                return arr[length - 1];
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToSecureString(this object text)
        {
            try
            {
                if (text == null)
                    return string.Empty;

                return text.ToString();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static int ToSecureInt(this object text)
        {
            int value = 0;

            try
            {
                if (text == null)
                    return 0;

                value = Convert.ToInt32(text);
            }
            catch (Exception ex)
            {
                try
                {
                    value = Int32.Parse(text.ToSecureString());
                }
                catch (Exception exInner)
                {
                    System.Diagnostics.Debug.WriteLine("Fail Conversion: " + exInner.Message);
                }
            }

            return value;
        }
        public static decimal ToSecureDecimal(this object text)
        {
            try
            {
                if (text == null)
                    return 0m;

                return Convert.ToDecimal(text.ToString());
            }
            catch (Exception ex)
            {
                return 0m;
            }
        }
        public static bool ToSecureBool(this object text)
        {
            try
            {
                if (text == null)
                    return false;

                return Convert.ToBoolean(text.ToString());
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static DateTime ToSecureDateTime(this object text)
        {
            try
            {
                if (text == null)
                    return DateTime.Now;

                return Convert.ToDateTime(text.ToString());
            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }
        }


        public static string SplitGetFirst(this string text, string separator = "-")
        {
            List<char> cs = new List<char>();
            if (separator.Length > 1)
            {
                foreach (char c in separator)
                {
                    cs.Add(c);
                }
                return text.Split(cs.ToArray(), StringSplitOptions.RemoveEmptyEntries)[0];
            }
            return text.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        public static string[] SplitByDefault(this string text, string[] separators = null)
        {
            separators = separators ?? new string[] { ",", ".", ";", ";", "#", "-", "_", "+", "*", "?", "!", "@", "&", "%", " " };

            try
            {
                if (text == null)
                {
                    return new string[] { };
                }

                return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                return new string[] { };
            }
        }
        public static string[] SplitByLineBreak(this string text)
        {
            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            string _1 = "\r\n";
            string _2 = "\n";
            string _3 = "\r";

            string[] splits = text.Split(new string[] { lineSeparator, paragraphSeparator, _1, _2, _3 }, StringSplitOptions.RemoveEmptyEntries);
            return splits;
        }
        public static string RemoveAtStartEnd(this string text, string start = "", string end = "")
        {

            try
            {
                if (text.StartsWith(start))
                {
                    text = text.Substring(start.Length, text.Length - start.Length - 1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
            }

            try
            {
                if (text.EndsWith(end))
                {
                    text = text.Substring(text.Length - end.Length - 1, end.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("FAIL: " + ex.Message);
            }

            return text;
        }
    }


    public static partial class Helper
    {
        // Logging 
        public static void LogMessage(this string msg)
        {
            try
            {
                string message = "Message: " + msg;

                Thread thr = new Thread(new ParameterizedThreadStart(Helper.Log));
                thr.Start(message);
            }
            catch (Exception exOuter)
            {
                Console.WriteLine("Fehler: " + exOuter.Message);
            }
        }
        public static void LogException(this Exception ex)
        {
            try
            {
                string msg = (ex.StackTrace != null) ? ex.Message + " - " + ex.StackTrace.ToString() : ex.Message;

                try
                {
                    if (ex.InnerException != null)
                    {
                        msg += (ex.InnerException.StackTrace != null) ? " - " + ex.InnerException.StackTrace : "";
                    }
                }
                catch (Exception exOuter)
                {
                    Console.WriteLine("Fehler: " + exOuter.Message);
                }

                Thread thr = new Thread(new ParameterizedThreadStart(Helper.Log));
                thr.Start(msg);
            }
            catch (Exception exOuter)
            {
                Console.WriteLine("Fehler: " + exOuter.Message);
            }
        }
        public static void LogAsync(string text)
        {
            try
            {
                Thread thr = new Thread(new ParameterizedThreadStart(Helper.Log));
                thr.Start(text);
            }
            catch (Exception exOuter)
            {
                Console.WriteLine("Fehler: " + exOuter.Message);
            }
        }
        private static void Log(object obj)
        {
            try
            {
                // Text 
                string errorMsg = obj.ToSecureString();

                string date = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();
                errorMsg = date + " - " + errorMsg;

                Console.WriteLine("ERROR --- " + errorMsg);

                // Path 
                #region Path
                string basePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                if (!basePath.EndsWith("\\"))
                {
                    basePath += "\\";
                }

                string errorPath = "Error_Logging_Files";
                if (!basePath.ToLower().Contains(errorPath))
                {
                    basePath += "\\" + errorPath + "\\";
                }

                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);
                #endregion

                // FileName 
                #region FileName
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                string day = DateTime.Now.Day.ToString();

                basePath += "Logging_" + year + "_" + month + "_" + day + ".txt";
                #endregion

                // File 
                if (!File.Exists(basePath))
                {
                    using (StreamWriter sw = File.CreateText(basePath))
                    {
                        sw.WriteLine(errorMsg);
                        sw.Close();
                        sw.Dispose();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(basePath))
                    {
                        sw.WriteLine(errorMsg);
                        sw.Close();
                        sw.Dispose();
                    }
                }
            }
            catch (Exception exOuter)
            {
                Console.WriteLine("Fehler: " + exOuter.Message);
            }
        }

        public static List<FileInfo> GetLogFiles()
        {
            List<FileInfo> files = new List<FileInfo>();

            // Path 
            #region Path
            string basePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            if (!basePath.EndsWith("\\"))
            {
                basePath += "\\";
            }

            string errorPath = "Error_Logging_Files";
            if (!basePath.ToLower().Contains(errorPath))
            {
                basePath += "\\" + errorPath + "\\";
            }

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);
            #endregion

            foreach (string file in Directory.GetFiles(basePath))
            {
                FileInfo fi = new FileInfo(file);
                files.Add(fi);
            }

            return files;
        }
    }
}
