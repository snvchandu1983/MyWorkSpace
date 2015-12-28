using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace LAjitControls.JQGridView
{
    public class GridHelper
    {
        internal static string ToCSV(List<ColModel> cmItems, bool insertNewLine)
        {
            StringBuilder sb = new StringBuilder();
            string format = insertNewLine ? "{0},\n" : "{0},";
            int trailCount = insertNewLine ? 2 : 1;
            foreach (ColModel cm in cmItems)
            {
                sb.AppendFormat(format, cm.ToString());
            }
            return sb.Remove(sb.Length - trailCount, trailCount).ToString();
        }

        internal static string ToCSV(List<ColModel> cmItems)
        {
            return ToCSV(cmItems, false);
        }

        internal static string ToCSV(Hashtable ht)
        {
            return ToCSV(ht, false);
        }

        internal static string ToCSV(Hashtable ht, bool insertNewLine)
        {
            if (ht == null || ht.Count == 0)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("{");//Opening brace.
            IEnumerator pntr = ht.GetEnumerator();
            while (pntr.MoveNext())
            {
                DictionaryEntry de = (DictionaryEntry)pntr.Current;
                string valueString = de.Value.ToString();//Call the object's ToString method.
                if (valueString != null)
                {
                    switch (de.Value.GetType().Name)
                    {
                        case "String":
                        case "FormatterType":
                            {
                                if (de.Key.ToString() == "dataInit" || de.Key.ToString() == "custom_func")
                                {
                                    sb.Append(de.Key.ToString() + ":" + valueString + "");
                                }
                                else
                                {
                                    sb.Append(de.Key.ToString() + ":'" + valueString + "'");
                                }
                                break;
                            }
                        case "Boolean":
                        case "Int32":
                            {
                                sb.Append(de.Key.ToString() + ":" + valueString.ToLower());//ToLower to convert True to true and False to false.
                                break;
                            }
                        case "String[]"://Used in case of SearchOptions which is of the type String[]
                            {
                                sb.Append(de.Key.ToString() + ":" + ToJSONString(de.Value));
                                break;
                            }
                        default:
                            {
                                sb.Append(de.Key.ToString() + ":" + valueString);
                                break;
                            }
                    }

                    if (insertNewLine)
                    {
                        sb.Append(",\n");
                    }
                    else
                    {
                        sb.Append(",");
                    }
                }
            }

            int trailCount = insertNewLine ? 2 : 1;
            sb.Remove(sb.Length - trailCount, trailCount);
            sb.Append("}");
            return sb.ToString();
        }

        internal static string ToColNameCSV(List<ColModel> cmItems)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ColModel cm in cmItems)
            {
                sb.AppendFormat("'{0}',", cm.ColumnName.Replace("'", "\\'"));
            }
            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        /// <summary>
        /// Converts an Object to JSON.
        /// </summary>
        /// <param name="arr">Object to be converted.</param>
        /// <returns>JSON String.</returns>
        internal static string ToJSONString(Object arr)
        {
            System.Web.Script.Serialization.JavaScriptSerializer objSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return objSerializer.Serialize(arr);
        }
    }
}
