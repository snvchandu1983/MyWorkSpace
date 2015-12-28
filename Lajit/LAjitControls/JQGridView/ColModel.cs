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
using System.Text;

namespace LAjitControls.JQGridView
{
    public class ColModel
    {
        private Hashtable m_htFieldList;

        private string m_ColumnName;

        public string ColumnName
        {
            get { return m_ColumnName; }
            set { m_ColumnName = value; }
        }

        public string Align
        {
            set { m_htFieldList["align"] = value; }
        }

        public string DateFormat
        {
            set { m_htFieldList["datefmt"] = value; }
        }

        public bool Editable
        {
            set { m_htFieldList["editable"] = value; }
        }

        public EditOptions EditOptions
        {
            set
            {
                if (value != null)
                {
                    m_htFieldList["editoptions"] = value;
                }
            }
            get { return (EditOptions)m_htFieldList["editoptions"]; }

        }

        public EditRules EditRules
        {
            get { return (EditRules)m_htFieldList["editrules"]; }
            set
            {
                if (value != null)
                {
                    m_htFieldList["editrules"] = value;
                }
            }
        }

        public EditType EditType
        {
            set
            {
                m_htFieldList["edittype"] = value.ToString();
            }
        }

        public FormOptions FormOptions
        {
            get { return (FormOptions)m_htFieldList["formoptions"]; }
            set { m_htFieldList["formoptions"] = value; }
        }

        public FormatterType Formatter
        {
            get { return (FormatterType)m_htFieldList["formatter"]; }
            set
            {
                m_htFieldList["formatter"] = value;
                ////Intialise the FormatOptions for the given Formatter Type.
                //switch (value)
                //{
                //    case FormatterType.integer:
                //        {
                //            FormatOptions = new IntegerFormatter();
                //            break;
                //        }
                //    default:
                //        {
                //            throw new Exception("TODO:Not implemented.Implement all the cases for FormatterType.");
                //            break;
                //        }
                //}
            }
        }

        public Formatter FormatOptions
        {
            get
            {
                if (m_htFieldList["formatoptions"] != null)
                {
                    return (Formatter)m_htFieldList["formatoptions"];
                }
                else
                {
                    throw new Exception("Intialise the Formatter first.");
                }
            }
            set { m_htFieldList["formatoptions"] = value; }
        }

        public bool HideDLG
        {
            set { m_htFieldList["hidedlg"] = value; }
        }

        public bool Hidden
        {
            set { m_htFieldList["hidden"] = value; }
        }

        public string Index
        {
            set { m_htFieldList["index"] = value; }
        }

        public string JSONMap
        {
            set { m_htFieldList["jsonmap"] = value; }
        }

        public bool Key
        {
            set { m_htFieldList["key"] = value; }
        }

        public string Label
        {
            set { m_htFieldList["label"] = value; }
        }

        public string Name
        {
            set { m_htFieldList["name"] = value; }
        }

        public bool Resizable
        {
            set { m_htFieldList["resizable"] = value; }
        }

        public bool Search
        {
            set
            {
                m_htFieldList["search"] = value;
                //    // ***Test code

                //    // Instantiate the SearchOptions once this field is set to true.
                //    if (value)
                //    {
                //        SearchOptions = new SearchOptions();
                //    }

                //    //*** End Test Code
            }
        }

        public SearchOptions SearchOptions
        {
            set { m_htFieldList["searchoptions"] = value; }
        }

        /// <summary>
        /// Defines is column can be sorted. Default is true.
        /// </summary>
        public bool Sortable
        {
            set { m_htFieldList["sortable"] = value; }
        }

        public string SortType
        {
            set { m_htFieldList["sorttype"] = value; }
        }

        public string SearchType
        {
            set { m_htFieldList["stype"] = value; }
        }

        public int Width
        {
            set { m_htFieldList["width"] = value; }
            get { return Convert.ToInt32(m_htFieldList["width"]); }
        }

        public string XmlMap
        {
            set { m_htFieldList["xmlmap"] = value; }
        }

        public string ControlType
        {
            set { m_htFieldList["cType"] = value; }
        }

        public ColModel()
        {
            m_htFieldList = new Hashtable();
            EditRules = new EditRules();
            FormOptions = new FormOptions();
        }

        /// <summary>
        /// The ToString Override which will return the intialised fiels in the custom jqGrid friendly format.
        /// </summary>
        /// <returns>String.</returns>
        public override string ToString()
        {
            return GridHelper.ToCSV(m_htFieldList);
        }

        /// <summary>
        /// Adds a custom key/value pair to the colModel object.
        /// </summary>
        /// <param name="name">Property Name.</param>
        /// <param name="value">Property Value.</param>
        public void AddColModelProperty(string name, object value)
        {
            m_htFieldList.Add(name, value);
        }
    }
}
