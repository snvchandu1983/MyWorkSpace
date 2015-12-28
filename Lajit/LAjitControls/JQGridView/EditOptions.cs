using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Xml;
using System.Text;

namespace LAjitControls.JQGridView
{
    public abstract class EditOptions
    {
        protected Hashtable m_htFieldList;

        //public bool ReadOnly
        //{
        //    get { return Convert.ToBoolean(m_htFieldList["readonly"]); }
        //    set { m_htFieldList["readonly"] = value; }
        //}

        public string Disable
        {
            get { return Convert.ToString(m_htFieldList["disabled"]); }
            set { m_htFieldList["disabled"] = value; }
        }

        public string DefaultValue
        {
            get { return Convert.ToString(m_htFieldList["defaultValue"]); }
            set { m_htFieldList["defaultValue"] = value; }
        }

        public EditOptions()
        {
            m_htFieldList = new Hashtable();
        }

        private EditType m_EditType;
        public EditType EditType
        {
            get { return m_EditType; }
            set { m_EditType = value; }
        }

        public override string ToString()
        {
            return "Danny";
        }
    }

    /// <summary>
    /// Initialises the EditOptions for a Checkbox.
    /// </summary>
    public class CheckBoxEditOptions : EditOptions
    {
        //private string m_FieldList;

        /// <summary>
        /// Whether the checkbox is selected or not.
        /// </summary>
        public string Value
        {
            get { return Convert.ToString(m_htFieldList["value"]); }
            set { m_htFieldList["value"] = value; }
        }
        //public CheckBoxEditOptions()
        //{
        //    m_FieldList = "";
        //}

        public override string ToString()
        {
            //string readonlyStr = base.ReadOnly ? string.Format("readonly:{0},", base.ReadOnly.ToString().ToLower()) : "";
            //return "{" + readonlyStr + "value:\"" + m_FieldList + "\"}";
            return GridHelper.ToCSV(m_htFieldList);
        }
    }

    /// <summary>
    /// Initialises the EditOptions for a Drop Down List.
    /// </summary>
    public class SelectEditOptions : EditOptions
    {
        private XmlNode m_NodeItems;
        private string m_NameLabel;
        private string m_ValueLabel;

        /// <summary>
        /// Parameterised Contructor.
        /// </summary>
        /// <param name="nodeItems">The XmlNode consisting of the Items for DDL.(The outer node with the RowList as child)</param>
        /// <param name="nameAttribute">The attribute from the Names have to be picked up.</param>
        /// <param name="valueAttribute">The attribute from the Values have to be picked up.</param>
        public SelectEditOptions(XmlNode nodeItems, string nameAttribute, string valueAttribute)
        {
            m_NodeItems = nodeItems;
            m_NameLabel = nameAttribute;
            m_ValueLabel = valueAttribute;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (m_NodeItems == null)
            {
                return "''";
            }
            string nodeName = m_NodeItems.LocalName;
            bool isReportTypeDDL = (m_NodeItems.FirstChild.LastChild != null
                        && m_NodeItems.FirstChild.LastChild.Attributes["ReportBPGID"] != null) ? true : false;

            for (int i = 0; i < m_NodeItems.FirstChild.ChildNodes.Count; i++)
            {
                XmlNode node = m_NodeItems.FirstChild.ChildNodes[i];
                //The value field is usually the TrxID and TrxType separated by a tilde.
                //Special Case : GenProcessEngine where the ReportRequestType contains the ReportBPGID to call(With a trailing Tilde).
                string valueField = (isReportTypeDDL && i != 0) ? node.Attributes["ReportBPGID"].Value + "~" :
                                        string.Format("{0}~{1}", node.Attributes["TrxID"].Value, node.Attributes["TrxType"].Value);
                string s = string.Format("{0}:{1};", valueField, node.Attributes[nodeName].Value.Replace("'", "\\'"));
                sb.Append(s);
            }
            //string readonlyStr = base.ReadOnly ? string.Format("readonly:{0},", base.ReadOnly.ToString().ToLower()) : "";
            string disableStr = (base.Disable == "disabled") ? string.Format("disabled:'{0}',", base.Disable.ToLower()) : "";
            string defaultValueStr = (base.DefaultValue.Length > 0) ? string.Format("defaultValue:\"{0}\",", base.DefaultValue) : "";
            //Remove the trailing semi-colon
            return "{" + defaultValueStr + disableStr + "value:'" + sb.Remove(sb.Length - 1, 1) + "'}";
            //return "{" + readonlyStr + "value:'" + sb.Remove(sb.Length - 1, 1) + "'}";
            // return "{}";
        }
    }

    /// <summary>
    /// Initialises the EditOptions for a TextBox.
    /// </summary>
    public class TextBoxEditOptions : EditOptions
    {
        //private Hashtable m_htFieldList;

        /// <summary>
        /// Size of the Textbox.
        /// </summary>
        public int Size
        {
            set { m_htFieldList["size"] = value; }
        }

        /// <summary>
        /// Accepts a Javascript function.If set this function is called only once when the element is created. To this function we pass the element object.
        /// dataInit: function(elem) {
        ///             do something }
        /// Also use this function to attach datepicker, time picker and etc. Example:
        /// dataInit : function (elem) {
        /// $(elem).datepicker();}
        /// </summary>
        public string DataInit
        {
            get { return Convert.ToString(m_htFieldList["dataInit"]); }
            set { m_htFieldList["dataInit"] = value; }
        }

        public int MaxLength
        {
            set { m_htFieldList["maxlength"] = value; }
        }

        //public TextBoxEditOptions()
        //{
        //    m_htFieldList = new Hashtable();

        //}
        public override string ToString()
        {
            //if (base.ReadOnly)
            //{
            //    m_htFieldList.Add("readonly", base.ReadOnly);
            //}
            return GridHelper.ToCSV(m_htFieldList);
        }
    }

    /// <summary>
    /// Initialises the EditOptions for a TextBox.
    /// </summary>
    public class TextAreaEditOptions : EditOptions
    {
        //private Hashtable m_htFieldList;

        /// <summary>
        /// Size of the Textbox.
        /// </summary>
        public int Rows
        {
            set { m_htFieldList["rows"] = value; }
        }

        public int Columns
        {
            set { m_htFieldList["cols"] = value; }
        }
        //public TextAreaEditOptions()
        //{
        //    m_htFieldList = new Hashtable();

        //}
        public override string ToString()
        {
            //if (base.ReadOnly)
            //{
            //    m_htFieldList.Add("readonly", base.ReadOnly);
            //}
            return GridHelper.ToCSV(m_htFieldList);
        }
    }
    /// <summary>
    /// Initialises the EditOptions for a TextBox.
    /// </summary>
    public class ImageEditOptions : EditOptions
    {
        /// <summary>
        /// Image Source.
        /// </summary>
        public string Source
        {
            set { m_htFieldList["src"] = value; }
        }

        public override string ToString()
        {
            return GridHelper.ToCSV(m_htFieldList);
        }
    }

}
