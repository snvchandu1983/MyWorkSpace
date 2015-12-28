using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LAjitControls
{
    [ToolboxData(@"<{0}:LAjitTextBox runat=""server""></{0}:LAjitTextBox>")]
    [System.Drawing.ToolboxBitmap(typeof(LAjitTextBox), "LAjitTextBox.LAjitTextBox.ico")]
    public class LAjitTextBox : System.Web.UI.WebControls.TextBox
    {
        private string m_MapXML;
        private string m_MapBranchNode;
        private bool m_ShowIcon = false;
        private string m_IconPath;
        private string m_IconOnClick;
        private string m_IconAlternateText;
        private IconAlign m_IconAlign = IconAlign.Left;
        private bool m_MaintainWidth = false;

        /// <summary>
        /// Maintain the original width of the TextBox before adding the icon or not.
        /// </summary>
        public bool MaintainWidth
        {
            get { return m_MaintainWidth; }
            set { m_MaintainWidth = value; }
        }

        /// <summary>
        /// Displays the Icon either to the right or left of the TextBox.
        /// </summary>
        public IconAlign IconAlign
        {
            get { return m_IconAlign; }
            set { m_IconAlign = value; }
        }

        /// <summary>
        /// Alternate text for the Icon Image.
        /// </summary>
        public string IconAlternateText
        {
            get { return m_IconAlternateText; }
            set { m_IconAlternateText = value; }
        }

        /// <summary>
        /// The js function to be called upon click of the icon.
        /// </summary>
        public string IconOnClick
        {
            get { return m_IconOnClick; }
            set { m_IconOnClick = value; }
        }

        /// <summary>
        /// Whether to show an Icon inside the textbox(which is not actually inside though).
        /// </summary>
        /// <seealso cref="IconPath;IconOnClick"/>
        public bool ShowIcon
        {
            get { return m_ShowIcon; }
            set { m_ShowIcon = value; }
        }

        /// <summary>
        /// The path of the icon to be shown inside the textbox.
        /// </summary>
        public string IconPath
        {
            get { return m_IconPath; }
            set { m_IconPath = value; }
        }

        public string MapXML
        {
            get { return m_MapXML; }
            set { m_MapXML = value; }
        }

        public string MapBranchNode
        {
            get { return m_MapBranchNode; }
            set { m_MapBranchNode = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.CssClass.Length == 0)
            {
                this.CssClass = "LajitTextBox";
            }
            this.Attributes["autocomplete"] = "off";

            if (ShowIcon || this.Attributes["ShowIcon"] == "true")
            {
                if (this.Attributes["ShowIcon"] == "true")
                {
                    //Initialise the relavent properties.
                    IconAlternateText = this.Attributes["IconAlternateText"];
                    IconOnClick = this.Attributes["IconOnClick"];
                    IconPath = this.Attributes["IconPath"];
                    IconAlign = (this.Attributes["IconAlign"] == "Right") ? IconAlign.Right : IconAlign.Left;
                    MaintainWidth = Convert.ToBoolean(this.Attributes["MaintainWidth"]);
                }

                string tdDim = this.Height.ToString();
                string divClass = "txtIconLeft";//Default it to this.
                string txtStyle = "border-left";
                if (tdDim.Length == 0)
                {
                    tdDim = "18";
                }
                if (IconAlign == IconAlign.Right)
                {
                    divClass = "txtIconRight";//Default it to this.
                    txtStyle = "border-right";
                }

                if (MaintainWidth)
                {
                    int divWidth = Convert.ToInt32(tdDim);
                    this.Width = Unit.Pixel(Convert.ToInt32(this.Width.Value) - divWidth - 1);
                }

                HttpApplication ctx = (HttpApplication)HttpContext.Current.ApplicationInstance;
                string strImagesCDNPath = (String)ctx.Application["ImagesCDNPath"];

                writer.WriteLine("<table cellpadding='0' cellspacing='0'><tr>");
                if (IconAlign == IconAlign.Left)
                {
                    //Cell containing the Div
                    writer.WriteLine("<td>");
                    writer.WriteLine("<div class='" + divClass + "' style='width:" + tdDim + "px; height:" + tdDim + "px; '>");
                    //writer.WriteLine("<img src='../App_Themes/" + this.Page.Theme + "/Images/spacer.gif' alt='spacer' style='height:1px' />");
                    writer.WriteLine("<img src='" + strImagesCDNPath + "Images/spacer.gif' alt='spacer' style='height:1px' />");
                    writer.WriteLine("<img src='" + IconPath + "' onclick=" + IconOnClick);
                    if (!string.IsNullOrEmpty(IconAlternateText))
                    {
                        writer.Write(" title='" + IconAlternateText + "' alt='" + IconAlternateText + "'");
                    }
                    writer.WriteLine(" />");
                    writer.WriteLine("</div>");
                    writer.WriteLine("</td>");

                    //Cell containing the TextBox.
                    writer.WriteLine("<td>");
                    this.Style.Add(txtStyle, "none");
                    base.Render(writer);
                    writer.WriteLine("</td>");
                }
                else
                {
                    //Cell containing the TextBox.
                    writer.WriteLine("<td>");
                    this.Style.Add(txtStyle, "none");
                    base.Render(writer);
                    writer.WriteLine("</td>");

                    //Cell containing the Div
                    writer.WriteLine("<td>");
                    writer.WriteLine("<div class='" + divClass + "' style='width:" + tdDim + "px; height:" + tdDim + "px; '>");
                    writer.WriteLine("<img src='" + strImagesCDNPath + "Images/spacer.gif' alt='spacer' style='height:1px' />");
                    writer.WriteLine("<img src='" + IconPath + "' onclick=" + IconOnClick);
                    if (!string.IsNullOrEmpty(IconAlternateText))
                    {
                        writer.Write(" title='" + IconAlternateText + "' alt='" + IconAlternateText + "'");
                    }
                    writer.WriteLine(" />");
                    writer.WriteLine("</div>");
                    writer.WriteLine("</td>");
                }

                writer.WriteLine("</tr></table>");
            }
            else
            {
                base.Render(writer);
            }
        }
    }

    public enum IconAlign
    {
        Left, Right
    }
}
