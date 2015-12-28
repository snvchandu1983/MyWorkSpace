using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LAjitDev.UserControls;
using System.Xml;
using System.Web.SessionState;
using System.Collections;
using System.Drawing;
using System.Text;
using LAjitControls;
using LAjit_BO;
using System.IO;
using System.Collections.Generic;
using NLog;


namespace LAjitDev
{
    /// <summary>
    /// Partial Class of CommonUI
    /// Created On : 19-06-2008
    /// </summary>
    public partial class CommonUI
    {
        # region Private Member Variables
        
        private int m_RecursionLevel = -1;
        private ArrayList m_arrBranchObjects = new ArrayList();
        //The form XMl for the current page.
        private XmlDocument m_xDocFormXML;
        XmlDocument XDocUserInfo = new XmlDocument();

        //The current selected row in the gridview.
        private XmlNode m_nodeGVSelectedRow;
        private string m_primaryKeyFieldName;
        private string m_OnGridLoadJSCall = string.Empty;

        private bool m_IsChildGridEnabled = false;

        private bool m_SelectColVisible = false;

        private bool m_IsFindMode = false;

        private ArrayList m_arrXMLDS = new ArrayList();

        /// <summary>
        /// To differentiate whether controls have already been added during multiple method calls in the same postback event.
        /// </summary>
        private bool m_AddAmountLabels = true;

        /// <summary>
        /// Holds the indices of the Drop Down Lists in the Data Source of the Grid View
        /// </summary>
        private Hashtable m_htDDLIndices = new Hashtable();

        /// <summary>
        /// Holds the indices of the CheckBoxes in the Data Source of the Grid View
        /// </summary>
        private Hashtable m_htCheckBoxIndices = new Hashtable();

        /// <summary>
        /// Holds the indices of the Label in the Data Source of the Grid View
        /// </summary>
        private Hashtable m_htLabelIndices = new Hashtable();

        /// <summary>
        /// Stores the Label attributes of each of the columns
        /// </summary>
        private ArrayList m_arrColLabels = new ArrayList();

        /// <summary>
        /// Contains the list of all the IsRequired Columns
        /// </summary>
        private ArrayList m_arrIsRequiredCols = new ArrayList();

        /// <summary>
        /// Contains the list of indices of all the IsDisplayOnly Columns
        /// </summary>
        private ArrayList m_arrIsDisplayOnlyCols = new ArrayList();

        /// <summary>
        /// Holds the indices of the TextBoxes in the Data Source of the Grid View
        /// </summary>
        private Hashtable m_htTextBoxIndices = new Hashtable();

        /// <summary>
        /// Holds the indices of the TextBoxes in the Data Source of the Grid View
        /// </summary>
        private Hashtable m_htGVColWidths = new Hashtable();

        /// <summary>
        /// Holds the indices of the TextBox Calcualator in the Data Source of the Grid View
        /// </summary>
        private Hashtable m_htTextBoxCalcIndices = new Hashtable();

        /// <summary>
        /// Holds the indices of the TextBoxes in the Data Source of the Grid View with auto complete feature
        /// </summary>
        private Hashtable m_htTextBoxIsLink = new Hashtable();

        /// <summary>
        /// Holds the indices of the Links(ControlType=Link) in the Data Source of the Grid View with auto complete feature
        /// </summary>
        private Hashtable m_htLinkIndices = new Hashtable();

        /// <summary>
        /// Holds the indices of the Calendar in the Data Source of the Grid View
        /// </summary>
        private Hashtable m_htCalendarIndices = new Hashtable();

        /// <summary>
        /// Keeps record of the indices of columns with ControlType="Amount"
        /// </summary>
        private ArrayList m_arrAmountCols = new ArrayList();


        private bool m_ShowOpLinks = true;

        private int m_RowCount = 10;

        #endregion

        #region Properties

        /// <summary>
        /// Show or hide the operation links beneath the Child Grid View.
        /// </summary>
        public bool ShowOpLinks
        {
            get { return m_ShowOpLinks; }
            set { m_ShowOpLinks = value; }
        }

        /// <summary>
        /// Rows to be diplayed in the Child Grid View.
        /// </summary>
        public int RowCount
        {
            //get { return m_RowCount; }
            //set { m_RowCount = value; }

            get
            {
                int returnValue;
                object objRCSession = HttpContext.Current.Session["CGVRowCount"];
                if (Int32.TryParse(Convert.ToString(objRCSession), out  returnValue))
                {
                    return returnValue;
                }
                else
                {
                    return 10;
                }
            }
            set { HttpContext.Current.Session["CGVRowCount"] = value; }
        }

        /// <summary>
        /// Contains the Row XML of the currently selected row in the grid view.
        /// </summary>
        public XmlNode GVSelectedRow
        {
            get { return m_nodeGVSelectedRow; }
            set { m_nodeGVSelectedRow = value; }
        }

        /// <summary>
        /// Contains the form XML.
        /// </summary>
        public XmlDocument XDocFormXML
        {
            get { return m_xDocFormXML; }
            set { m_xDocFormXML = value; }
        }

        #endregion

        /// <summary>
        /// Removes the customised datetime stamp inserted while uploading.
        /// </summary>
        /// <param name="p">String containing the timestamp.</param>
        /// <returns>String without the timestamp</returns>
        public string TrimTimeStamp(string p)
        {
            //Remove the file extension
            if (p.Length > 0 && p.Contains("_"))
            {
                int indexOfDot = p.LastIndexOf('.');
                if (indexOfDot == -1)
                {
                    indexOfDot = p.Length;
                }
                string file = p.Substring(0, indexOfDot);
                string extsn = p.Substring(indexOfDot, p.Length - indexOfDot);
                int cntr = file.Length - 1;
                int delimCntr = 0;
                while (delimCntr < 6)
                {
                    if (file[cntr--] == '_')
                    {
                        delimCntr++;
                    }
                    if (cntr == -1)
                    {
                        cntr = file.Length - 1;
                        break;
                    }
                }
                return file.Substring(0, cntr + 1) + extsn;
            }
            else
            {
                return p;
            }
        }

        ///// <summary>
        ///// Emits client-side script into the response which will automatically redirct the user to a session-expired page
        ///// as soon as the user's session has expired on the server.
        ///// </summary>
        ///// <seealso cref="Common.js->ShowSessionExpireAlert()"/>
        //public void InjectSessionExpireScript(Page objPage)
        //{
        //    string isPopUp = objPage.Request.QueryString["PopUp"];
        //    int timeOut = objPage.Session.Timeout * 60000;
        //    int alertBefore = 50000; //50secs before expiry
        //    int timeOutForAlert = timeOut - alertBefore;
        //    string sessionExpirePage = "../Common/SessionExpire.aspx";
        //    string script = "function ExpireSession(){" + Environment.NewLine
        //        + "g_temp=false;top.g_TimeOut=" + timeOut + ";" + Environment.NewLine //Override all validations for page unload.
        //        //+ "window.location='" + sessionExpirePage + "';}" + Environment.NewLine
        //        + "top.location='" + sessionExpirePage + "';}" + Environment.NewLine
        //        + "ClearSessionExpireTimeOuts();" + Environment.NewLine
        //        + "top.g_TimerIDSessionExpire = top.setTimeout('top.ExpireSession()', " + timeOut + " );" + Environment.NewLine
        //        + "top.seconds=" + (alertBefore / 1000) + ";" + Environment.NewLine //Initialise the global variable for the countDown timer.
        //        + "top.g_TimerIDSessionAlert = top.setTimeout('top.ShowSessionExpireAlert()', " + timeOutForAlert + " );";
        //    ScriptManager.RegisterStartupScript(objPage.Page, objPage.GetType(), "SessionExpireScript", script, true);
        //}

        /// <summary>
        /// Emits client-side script into the response which will automatically redirct the user to a session-expired page
        /// as soon as the user's session has expired on the server.
        /// </summary>
        /// <seealso cref="Common.js->ShowSessionExpireAlert()"/>
        public void InjectSessionExpireScript(Page objPage)
        {
            string isPopUp = objPage.Request.QueryString["PopUp"];
            int timeOut = objPage.Session.Timeout * 60000;
            int alertBefore = 50000; //50secs before expiry
            int timeOutForAlert = timeOut - alertBefore;
            string sessionExpirePage = "../Common/SessionExpire.aspx";
            string script = "top.g_TimeOut=" + timeOut + ";" + Environment.NewLine
                + "top.seconds=" + (alertBefore / 1000) + ";" + Environment.NewLine
                + "DoSessionExpire();" + Environment.NewLine;
            ScriptManager.RegisterStartupScript(objPage.Page, objPage.GetType(), "SessionExpireScript", script, true);
        }

        /// <summary>
        /// Binds the data to the controls and enables or disables them accordingly.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="c">Container Control.</param>
        /// <param name="xDocOut">The return document for the current page.</param>
        /// <param name="nodeSelectedRow">The node containing the selected row's bpinfo</param>
        /// Added By Danny 17/06/2008
        public void EnableDisableAndFillUI(bool status, Control c, XmlDocument xDocOut, XmlNode nodeSelectedRow, string mode, bool changeColor)
        {
            #region NLog
            logger.Info("Binds the data to the controls and enables or disables them accordingly."); 
            #endregion

            //Parent Details
            string parentTreeNode = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            XmlNode nodeColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + parentTreeNode + "/GridHeading/Columns");
            
            //Keeping backups
            XmlNode parentCols = nodeColumns;
            XmlNode nodeResRow = nodeSelectedRow;

            //Parent TrxID necessary to get branch rows
            string parentTrxID = string.Empty;
            if (nodeResRow != null)
            {
                if (nodeResRow.Attributes["TrxID"] != null)
                {
                    parentTrxID = nodeResRow.Attributes["TrxID"].Value;
                }
            }

            //old code
            bool gridExists = false;
            string pageNo = string.Empty;
            if (c is Panel)
            {
                HiddenField hdnCurrPgNo;
                Control GVUC = c.FindControl("GVUC");
                Control PCGVUC = c.FindControl("PCGVUC");
                if (GVUC != null)
                {
                    hdnCurrPgNo = (HiddenField)c.FindControl("GVUC").FindControl("hdnCurrPageNo");
                    pageNo = hdnCurrPgNo.Value;
                }
                else if (PCGVUC != null)
                {
                    hdnCurrPgNo = (HiddenField)c.FindControl("PCGVUC").FindControl("hdnCurrPageNo");
                    pageNo = hdnCurrPgNo.Value;
                }
                if (c.FindControl("pnlGVContent") != null)
                {
                    gridExists = c.FindControl("pnlGVContent").Visible;
                }
            }

            //Setting color as empty by default
            System.Drawing.Color setColor = System.Drawing.Color.Empty;
            ////if color should be changed, then setting the color. 
            //if (changeColor)
            //{
            //    setColor = System.Drawing.Color.White;
            //    if (mode.ToUpper().Trim() == "DISABLEMODE")
            //    {
            //        setColor = System.Drawing.Color.LightGray;
            //    }
            //    else if (mode.ToUpper().Trim() == "FIND")
            //    {
            //        setColor = System.Drawing.Color.LightGoldenrodYellow;
            //    }
            //}

            //Looping through all page controls to change Color,Clear,Enable and Default
            if (m_PageControls == null)
            {
                m_PageControls = GetPageControls(xDocOut, (Panel)c);
            }

            //Looping through all the page controls to Fill, Change Color and Disable.
            foreach (KeyValuePair<string, Control> kvPair in m_PageControls)
            {
                string strCntrl = kvPair.Key.Substring(0, Convert.ToInt32(kvPair.Key.Length) - 3);

                string[] arr = kvPair.Value.ID.Split('_');
                if (arr.Length > 1)
                {
                    //Getting the RowList for corresponding branch node.
                    XmlNode brnchcols = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + arr[1] + "/GridHeading/Columns");
                    nodeColumns = brnchcols;

                    XmlNodeList nodeResRows = xDocOut.SelectNodes("//" + arr[1] + "/RowList/Rows[@" + parentTreeNode + "_TrxID = '" + parentTrxID + "']");
                    if (nodeResRows != null)
                    {
                        //Getting the First Row need to set for corresponding branch node.
                        nodeResRow = nodeResRows.Item(0);

                        //Getting the Row whose 'IsPrimary=1' need to set for corresponding branch node.
                        for (int nodeRowCnt = 0; nodeRowCnt < nodeResRows.Count; nodeRowCnt++)
                        {
                            nodeResRow = nodeResRows.Item(nodeRowCnt);
                            if (nodeResRow.Attributes["IsPrimary"] != null)
                            {
                                if (nodeResRow.Attributes["IsPrimary"].Value == "1")
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    nodeColumns = parentCols;
                    nodeResRow = nodeSelectedRow;
                }
                switch (strCntrl)
                {
                    case "TextBox":
                        {
                            LAjitTextBox txtCurrent = (LAjitTextBox)kvPair.Value;
                            txtCurrent.Text = string.Empty;//Clear the previous text if any.
                            string ctrlMapXML = txtCurrent.MapXML;
                            if (setColor != Color.Empty)
                            {
                                txtCurrent.BackColor = setColor;
                            }

                            if (nodeResRow != null)
                            {
                                if (nodeResRow.Attributes[ctrlMapXML] != null)
                                {
                                    //Control type is Amount or Calc format the amount added on 06-11-08.
                                    XmlNode nodeFormat = nodeColumns.SelectSingleNode("Col [@Label='" + ctrlMapXML + "']");
                                    if (nodeFormat.Attributes["ControlType"] != null)
                                    {
                                        string ctrlType = nodeFormat.Attributes["ControlType"].Value;
                                        string ctrlValue = nodeResRow.Attributes[ctrlMapXML].Value;
                                        if (IsHelpAuthPage && nodeFormat.Attributes["Label"].Value == "HelpFile")
                                        {
                                            //Trim the Time Stamp.
                                            txtCurrent.Text = TrimTimeStamp(ctrlValue);
                                            txtCurrent.Attributes["File"] = ctrlValue;
                                        }
                                        else
                                        {
                                            switch (ctrlType)
                                            {
                                                case "Amount":
                                                case "Calc":
                                                    {
                                                        decimal amount;
                                                        if (Decimal.TryParse(ctrlValue, out amount))
                                                        {
                                                            txtCurrent.Text = string.Format("{0:N}", amount);
                                                        }
                                                        break;
                                                    }
                                                case "Cal":
                                                    {
                                                        if (IsDate(ctrlValue))
                                                        {
                                                            DateTime date;
                                                            //if the value is IsDate then change format MM/DD/YYYY
                                                            DateTime.TryParse(ctrlValue, out date);
                                                            txtCurrent.Text = date.ToString("MM/dd/yy");
                                                        }
                                                        break;
                                                    }
                                                case "TBox":
                                                    {
                                                        XmlAttribute attIsLink = nodeFormat.Attributes["IsLink"];
                                                        if (attIsLink != null && attIsLink.Value == "1")
                                                        {
                                                            txtCurrent.Attributes["AFText"] = ctrlValue;
                                                            txtCurrent.Text = nodeResRow.Attributes[ctrlMapXML + "_TrxID"].Value + "~"
                                                                        + nodeResRow.Attributes[ctrlMapXML + "_TrxType"].Value;
                                                        }
                                                        else
                                                        {
                                                            txtCurrent.Text = ctrlValue;
                                                        }
                                                        break;
                                                    }
                                                case "Passwd":
                                                    {
                                                        txtCurrent.Attributes.Add("Value",ctrlValue);
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        txtCurrent.Text = ctrlValue;
                                                        break;
                                                    }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        txtCurrent.Text = nodeResRow.Attributes[ctrlMapXML].Value;
                                    }
                                }
                            }
                            break;
                        }
                    case "DDL":
                        {
                            LAjitDropDownList ddlCurrent = (LAjitDropDownList)kvPair.Value;
                            string mapXML = ddlCurrent.MapXML.Trim();
                            if (setColor != Color.Empty)
                            {
                                ddlCurrent.BackColor = setColor;
                            }
                            if (nodeResRow != null)
                            {
                                string dataValueField = string.Empty;
                                XmlAttribute attMapXMLTrxID = nodeResRow.Attributes[mapXML + "_TrxID"];
                                XmlAttribute attMapXMLTrxType = nodeResRow.Attributes[mapXML + "_TrxType"];
                                if (attMapXMLTrxID != null && attMapXMLTrxID.Value != string.Empty
                                      && attMapXMLTrxType != null && attMapXMLTrxType.Value != string.Empty)
                                {
                                    dataValueField = nodeResRow.Attributes[mapXML + "_TrxID"].Value.Trim() + '~' + nodeResRow.Attributes[mapXML + "_TrxType"].Value.Trim();
                                    if ((nodeResRow.Attributes[mapXML + "_TrxID"].Value.Trim() != string.Empty) && (nodeResRow.Attributes[mapXML + "_TrxType"].Value.Trim() != string.Empty))
                                    {
                                        ddlCurrent.SelectedIndex = ddlCurrent.Items.IndexOf(ddlCurrent.Items.FindByValue(dataValueField));
                                        if (ddlCurrent.Attributes["MapPreviousSelItem"] != null)
                                        {
                                            ddlCurrent.Attributes["MapPreviousSelItem"] = ddlCurrent.SelectedItem.Text;
                                        }
                                    }
                                }
                                else //if no value is available in row, we need to set it default else clear
                                {
                                    if (ddlCurrent.Items.Count > 0)
                                    {
                                        XmlNode nodeDdl = nodeColumns.SelectSingleNode("Col [@Label='" + mapXML + "']");
                                        if (nodeDdl != null)
                                        {
                                            if (nodeDdl.Attributes["Default"] != null)
                                            {
                                                dataValueField = string.Empty;
                                                string[] strarr = ddlCurrent.Items[0].Value.Split('~');
                                                //Getting TrxType
                                                if (strarr[1].ToString() != string.Empty)
                                                {
                                                    dataValueField = nodeDdl.Attributes["Default"].Value.Trim().ToString() + "~" + strarr[1].ToString();
                                                }
                                                if (dataValueField != string.Empty)
                                                {
                                                    ddlCurrent.SelectedIndex = ddlCurrent.Items.IndexOf(ddlCurrent.Items.FindByValue(dataValueField));
                                                    ddlCurrent.Attributes["MapPreviousSelItem"] = ddlCurrent.SelectedItem.Text.ToString();
                                                }
                                            }
                                            else
                                            {
                                                ddlCurrent.SelectedIndex = 0;
                                                ddlCurrent.Attributes["MapPreviousSelItem"] = ddlCurrent.Items[0].Text;
                                            }
                                        }
                                    }
                                }
                            }
                            else //we need to set it default if exist else clear
                            {
                                if (ddlCurrent.Items.Count > 0)
                                {
                                    XmlNode nodeDdl = nodeColumns.SelectSingleNode("Col [@Label='" + mapXML + "']");
                                    if (nodeDdl != null)
                                    {
                                        if (nodeDdl.Attributes["Default"] != null)
                                        {
                                            string dataValueField = string.Empty;
                                            string[] strarr = ddlCurrent.Items[0].Value.Split('~');
                                            //Getting TrxType
                                            if (strarr[1].ToString() != string.Empty)
                                            {
                                                dataValueField = nodeDdl.Attributes["Default"].Value.Trim().ToString() + "~" + strarr[1].ToString();
                                            }
                                            if (dataValueField != string.Empty)
                                            {
                                                ddlCurrent.SelectedIndex = ddlCurrent.Items.IndexOf(ddlCurrent.Items.FindByValue(dataValueField));
                                            }
                                        }
                                        else
                                        {
                                            ddlCurrent.SelectedIndex = 0;
                                            ddlCurrent.Attributes["MapPreviousSelItem"] = ddlCurrent.Items[0].Text;
                                        }
                                    }
                                }
                            }

                            //ddlCurrent.Enabled = status;
                            break;
                        }
                    case "CheckBox":
                        {
                            LAjitCheckBox chkbxCurrent = (LAjitCheckBox)kvPair.Value;
                            string mapXML = chkbxCurrent.MapXML;
                            if (!string.IsNullOrEmpty(mapXML))
                            {
                                if (setColor != Color.Empty)//No need of setting any color to a checkbox.
                                {
                                    //chkbxCurrent.BackColor = setColor;
                                }
                                if (nodeResRow != null)
                                {
                                    if (nodeResRow.Attributes[mapXML] != null)
                                    {
                                        if (nodeResRow.Attributes[mapXML].Value == "1")
                                        {
                                            chkbxCurrent.Checked = true;
                                        }
                                        else
                                        {
                                            chkbxCurrent.Checked = false;
                                        }
                                    }
                                }
                                else
                                {
                                    chkbxCurrent.Checked = false;
                                }
                            }
                            //chkbxCurrent.Enabled = status;
                            break;
                        }
                    case "Label":
                        {
                            LAjitLabel lblCurrent = (LAjitLabel)kvPair.Value;
                            if (setColor != Color.Empty)
                            {
                                // lblCurrent.BackColor = setColor;
                                lblCurrent.BackColor = Color.Transparent;
                            }

                            if (lblCurrent.ID.Contains("_Value"))
                            {
                                if (nodeResRow != null)
                                {
                                    if (nodeResRow.Attributes[lblCurrent.MapXML] != null)
                                    {
                                        lblCurrent.Text = nodeResRow.Attributes[lblCurrent.MapXML].Value;
                                    }
                                    else
                                    {
                                        lblCurrent.Text = string.Empty;
                                    }

                                    //Label visible based on IsApproved status
                                    if (nodeResRow.Attributes["IsApproved"] != null)
                                    {
                                        if (nodeResRow.Attributes["IsApproved"].Value == "0")
                                        {
                                            lblCurrent.Visible = true;
                                        }
                                        else
                                        {
                                            lblCurrent.Visible = false;
                                        }
                                    }
                                }
                                else
                                {
                                    lblCurrent.Text = string.Empty;
                                }

                            }
                            break;
                        }
                    case "LinkButton":
                        {
                            LAjitLinkButton lnkBtnCurrent = (LAjitLinkButton)kvPair.Value;
                            if (nodeResRow != null)
                            {
                                if (nodeResRow.Attributes[lnkBtnCurrent.MapXML] != null)
                                {
                                    lnkBtnCurrent.ToolTip = nodeResRow.Attributes[lnkBtnCurrent.MapXML].Value;
                                }
                                //SOX Approval
                                if (nodeResRow.Attributes["SoxApprovedStatus"] != null)
                                {
                                    lnkBtnCurrent.ToolTip = nodeResRow.Attributes["SoxApprovedStatus"].Value;
                                }
                            }

                            break;
                        }
                    case "ListBox":
                        {
                            LAjitListBox lstBxCurrent = (LAjitListBox)kvPair.Value;
                            if (setColor != Color.Empty)
                            {
                                lstBxCurrent.BackColor = setColor;
                            }
                            //lstBxCurrent.Enabled = status;
                            if (!m_arrBranchObjects.Contains(lstBxCurrent))
                            {
                                m_arrBranchObjects.Add(lstBxCurrent);
                            }
                            break;
                        }
                    case "GridView":
                        {
                            m_arrBranchObjects.Add(kvPair.Value);
                            break;
                        }
                    case "reqFldValidator":
                        {
                            LAjitRequiredFieldValidator reqCurrent = (LAjitRequiredFieldValidator)kvPair.Value;
                            XmlNode nodeCol = nodeColumns.SelectSingleNode("Col [@Label='" + reqCurrent.MapXML + "']");

                            if (nodeCol != null)
                            {
                                XmlAttribute attrIsRequired = nodeCol.Attributes["IsRequired"];
                                XmlAttribute attrIsHidden = nodeCol.Attributes["IsHidden"];
                                if (attrIsRequired != null && attrIsHidden != null)
                                {
                                    if (attrIsRequired.Value != "0" && attrIsHidden.Value != "1")
                                    {
                                        //Except for 'Find' action, validation should be there for all other actions. Done by Shanti.
                                        reqCurrent.Enabled = true;
                                        if (nodeCol.Attributes["Caption"] != null)
                                        {
                                            reqCurrent.ErrorMessage = nodeCol.Attributes["Caption"].Value;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case "regExpValidator":
                        {
                            LAjitRegularExpressionValidator regCurrent = (LAjitRegularExpressionValidator)kvPair.Value;
                            XmlNode nodeCol = nodeColumns.SelectSingleNode("Col [@Label='" + regCurrent.MapXML + "']");

                            if (nodeCol != null)
                            {
                                XmlAttribute attrIsNumeric = nodeCol.Attributes["IsNumeric"];
                                XmlAttribute attrIsHidden = nodeCol.Attributes["IsHidden"];
                                if (attrIsNumeric != null && attrIsHidden != null)
                                {
                                    if (attrIsNumeric.Value != "0" && attrIsHidden.Value != "1")
                                    {
                                        //Except for 'Find' action, validation should be there for all other actions. Done by Shanti.
                                        regCurrent.Enabled = true;
                                        if (nodeCol.Attributes["Caption"] != null)
                                        {
                                            regCurrent.ErrorMessage = nodeCol.Attributes["Caption"].Value;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }

            BindBranchObjects(c, gridExists, pageNo, m_arrBranchObjects, xDocOut);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="gridExists"></param>
        /// <param name="pageNo"></param>
        /// <param name="objectsToInit"></param>
        /// <param name="xDocFormLoad"></param>
        public void BindBranchObjects(Control c, bool gridExists, string pageNo, ArrayList objectsToInit, XmlDocument xDocFormLoad)
        {
            #region NLog
            logger.Info("This method is used in binding the Branch Objects.");
            #endregion

            XDocUserInfo = loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);

            foreach (Object obj in objectsToInit)
            {
                if (obj is LAjitListBox)
                {
                    LAjitListBox lstBxCurrent = (LAjitListBox)obj;
                    switch (lstBxCurrent.XMLType)
                    {
                        case XMLType.ParentOnly:
                            {//DefaultGridSize
                                string pageSize = GetUserPreferenceValue("59");
                                string BPGID = GetBPCBPGID("Find", XDocFormXML);
                                if (BPGID.Length == 0)
                                {
                                    //BPGID = GetFormBPGID(GridViewUserControl.GridViewInitData);
                                    BPGID = GetFormBPGID(BtnsUC.GVDataXml);
                                }
                                //Generate the new XML.
                                string requestXML = objBO.GenActionRequestXML("Find", BPGID, GVSelectedRow.OuterXml,
                                     "", XDocFormXML.OuterXml, strBPE,
                                     gridExists, pageSize, pageNo, HttpContext.Current.Session["BPINFO"].ToString());

                                //Hit the DB
                                string responseXML = objBO.GetDataForCPGV1(requestXML);
                                XmlDocument xDocRowOut = new XmlDocument();
                                xDocRowOut.LoadXml(responseXML);
                                XDocFormXML = xDocRowOut;
                                GridViewUserControl.FormTempData = responseXML;
                                InitialiseBranchObjects(xDocRowOut, c);
                                break;
                            }
                        case XMLType.ParentChild:
                            {
                                InitialiseBranchObjects(xDocFormLoad, c);
                                break;
                            }
                        case XMLType.HybridParent:
                            {
                                break;
                            }
                        default:
                            {
                                break;
                            }

                    }
                }
                else if (obj is GridView)
                {
                    InitialiseBranchObjects(xDocFormLoad, c);
                }
            }
        }

        /// <summary>
        /// Gets  the Form's master BPGID.
        /// </summary>
        /// <param name="formXML">The Form XML.</param>
        /// <returns>String BPGID.</returns>
        private string GetFormBPGID(string formXML)
        {
            #region NLog
            logger.Info("Getting  the Form's master BPGID.");
            #endregion

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(formXML);
            return xDoc.SelectSingleNode("Root/bpeout/FormInfo/BPGID").InnerText;
        }

        /// <summary>
        /// Initialises the objects present under the Tree node in the GridLayout node.
        /// </summary>
        /// <param name="xDocOut">The return document for the current page.</param>
        /// <param name="pnlEntryForm">The parent control which contains all the UI I/O elements.</param>
        public void InitialiseBranchObjects(XmlDocument xDocOut, Control pnlEntryForm)
        {
            #region NLog
            logger.Info("Initialises the objects present under the Tree node in the GridLayout node.");
            #endregion

            string treeNodeName = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            XmlNode nodeBranches = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
            if (nodeBranches == null)
            {
                return;
            }
            foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
            {
                if (nodeBranch.Attributes["ControlType"] != null)
                {
                    string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                    XmlNode nodeBranchColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");

                    //Check for ListBox or GridView
                    if (nodeBranch.Attributes["ControlType"].Value == "GView")
                    {
                        //Check for the presence of GridView in the Page if absent go for ListBox
                        GridView grdVwBranch = (GridView)pnlEntryForm.FindControl("grdVw" + branchNodeName);
                        if (grdVwBranch != null)
                        {
                            //GridView
                            BindGrid(xDocOut, branchNodeName, nodeBranchColumns, grdVwBranch, pnlEntryForm, treeNodeName);
                        }
                    }
                    else if (nodeBranch.Attributes["ControlType"].Value == "LBox")
                    {
                        //Search for ListBox
                        //ListBox lstBxBranch = (ListBox)pnlEntryForm.FindControl("lstBx" + branchNodeName);
                        //if (lstBxBranch != null)
                        {
                            BindListBox(xDocOut, pnlEntryForm, treeNodeName, branchNodeName, nodeBranchColumns);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the control which caused the postback.
        /// </summary>
        /// <param name="page">The page in which the control is present.</param>
        /// <returns>Control.</returns>
        public static Control GetPostBackControl(Page page)
        {
            Control control = null;
            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if (ctrlname != null && ctrlname != string.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    if (ctl == null)
                    {
                        //A Case where Enter was hit to cause the postback
                        Control ctrlEnter = new Control();
                        ctrlEnter.ID = "btnEnter";
                        return ctrlEnter;

                        //Control ctrlPostBack = page.Master.FindControl("cphPageContents").FindControl("imgbtnSubmit");
                        //return ctrlPostBack;
                    }
                    string controlToFind = string.Empty;
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        controlToFind = ctl.Substring(0, ctl.Length - 2);
                    }
                    else
                    {
                        controlToFind = ctl;
                    }
                    Control c = page.FindControl(controlToFind);
                    if (c is LAjitImageButton || c is ImageButton)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;
        }

        /// <summary>
        /// Gets the control's ID which caused the postback.
        /// </summary>
        /// <param name="page">The page in which the control is present.</param>
        /// <returns>Control ID.</returns>
        public static string GetPostBackControlID(Page page)
        {
            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if (!string.IsNullOrEmpty(ctrlname))
            {
                string[] split = ctrlname.Split('$');
                return split[split.Length - 1];
            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    if (ctl == null)
                    {
                        //A Case where Enter was hit to cause the postback
                        return "btnEnter";
                    }

                    if (ctl.ToUpper().Contains("IMGBTN"))
                    {
                        string[] split = ctl.Split('$');
                        ctrlname = split[split.Length - 1];
                        if (ctrlname.EndsWith(".x") || ctrlname.EndsWith(".y"))
                        {
                            ctrlname = ctrlname.Substring(0, ctrlname.Length - 2);
                        }
                        break;
                    }
                }
            }
            return ctrlname;
        }

        #region GridView Methods

        /// <summary>
        /// Performs all the grid view initialisation operations.
        /// </summary>
        /// <param name="xDocOut">The current page's document.</param>
        /// <param name="branchNodeName">The Branch Node Name.</param>
        /// <param name="nodeBranchColumns">The Col Node</param>
        /// <param name="grdVwBranch">Gridview Object</param>
        private void BindGrid(XmlDocument xDocOut, string branchNodeName, XmlNode nodeBranchColumns, GridView grdVwBranch, Control pnlEntryForm, string treeNodeName)
        {
            #region NLog
            logger.Info("Performs all the grid view initialisation operations.");
            #endregion

            if (pnlEntryForm.Page.ToString().Contains("checkhistory"))
            {
                RowCount = 5;
                ShowOpLinks = false;
            }
            if (pnlEntryForm.Page.ToString().Contains("manualcheck"))
            {
                RowCount = 5;
            }


            //WahlinControlToolkit.GridViewHeaderExtender gheChildGrid = new WahlinControlToolkit.GridViewHeaderExtender();
            //gheChildGrid.WrapperDivCssClass = "WrapperDiv";
            //gheChildGrid.TargetControlID = grdVwBranch.ID;
            //pnlEntryForm.Controls.Add(gheChildGrid);


            DataSet dsRowList = new DataSet();
            //Get the child node rows only belonging to the selected parent
            string parentTrxID = string.Empty;
            string parentTrxType = string.Empty;
            string GVTreeNodeName = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            string gridMode = string.Empty;
            if (BtnsUC != null)
            {
                HiddenField hdnCurrAction = (HiddenField)BtnsUC.FindControl("hdnCurrAction");
                gridMode = hdnCurrAction.Value.ToUpper();
            }

            Control postBackCtrl = GetPostBackControl(pnlEntryForm.Page);
            if (postBackCtrl != null && (postBackCtrl.ID.Contains("imgBtnTTnNavigate") || postBackCtrl.ID.Contains("imgbtnAdd") || postBackCtrl.ID.Contains("imgbtnSubmit")))
            {
                InitCheckBoxSelection(grdVwBranch, true);
            }
            else
            {
                InitCheckBoxSelection(grdVwBranch, false);
            }

            //Calcuting the enabled/disabled/colour status of the child gridview.
            if (postBackCtrl != null && !postBackCtrl.ID.Contains("Cancel")
                && (postBackCtrl.ID.Contains("Add") || postBackCtrl.ID.Contains("Modify") || postBackCtrl.ID.Contains("Find") || postBackCtrl.ID.Contains("timer")))
            {
                m_IsChildGridEnabled = true;
                if (postBackCtrl.ID.Contains("Find") || gridMode == "FIND")
                {
                    m_IsFindMode = true;
                }
            }
            else if (postBackCtrl == null || postBackCtrl.ID == "imgbtnCancel")//Implies page load(mostly) or Cancel Click.If it is cancel click then show the user the view similar to that of page load.
            {
                //Find the parent node to know the number of rows. No rows implies that page is in ADD mode
                XmlNode nodeParentRows = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + GVTreeNodeName + "/RowList");
                if (nodeParentRows == null || nodeParentRows.ChildNodes.Count == 0)
                {
                    //Case of no parent records present, so lands in the page in "ADD" mode
                    m_IsChildGridEnabled = true;
                }
                else if (nodeParentRows.ChildNodes.Count == 1)
                {
                    if (xDocOut.SelectSingleNode("Root/bpeout/FormInfo/PageInfo").InnerText.Contains("SelectRequest"))
                    {
                        m_IsChildGridEnabled = true;
                        m_IsFindMode = true;
                    }
                    else
                    {
                        m_IsChildGridEnabled = false;
                    }
                }
            }
            else if (postBackCtrl.ID == "imgbtnSubmit" && gridMode == "FIND")
            {
                //After find criteria submitted remain in the FIND mode so that upon error it will be already in FIND mode.
                m_IsChildGridEnabled = true;
                m_IsFindMode = true;
            }
            else if ((gridMode == "MODIFY" || gridMode == "ADD"))//Changed from if to Else If on NOV 3rd
            {
                //19-12-2008-Added the below 3 conditions for Note,Attachment and secure.
                if (postBackCtrl != null && (postBackCtrl.ID.Contains("Submit") || postBackCtrl.ID.Contains("Note") || postBackCtrl.ID.Contains("Secure") || postBackCtrl.ID.Contains("Attachment")))
                {
                    m_IsChildGridEnabled = false;//Success Process Complete
                }
                else
                {
                    if (!postBackCtrl.ID.Contains("Cancel"))//25-11-08 - For DisabledModifyLoad case, cancel click
                    {
                        m_IsChildGridEnabled = true;
                    }
                }
            }

            //Retreiving the TrxID and TrxType of the Parent row.
            if (BtnsUC == null || BtnsUC.RwToBeModified == null || BtnsUC.RwToBeModified.Length == 0)
            {
                if (GVSelectedRow != null)
                {
                    parentTrxID = GVSelectedRow.Attributes["TrxID"].Value;
                    parentTrxType = GVSelectedRow.Attributes["TrxType"].Value;
                }
                else if (pnlEntryForm.Page.ToString().Contains("selectrequest_aspx"))
                {
                    XmlNode nodeParentRows = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + GVTreeNodeName + "/RowList");//Only one parent node.
                    if (nodeParentRows != null)
                    {
                        parentTrxID = nodeParentRows.ChildNodes[0].Attributes["TrxID"].Value;
                        parentTrxType = nodeParentRows.ChildNodes[0].Attributes["TrxType"].Value;
                    }
                }
            }
            else
            {
                XmlDocument xDocSelectedRow = new XmlDocument();
                xDocSelectedRow.LoadXml(ButtonsUserControl.RwToBeModified);
                parentTrxID = xDocSelectedRow.SelectSingleNode("Rows").Attributes["TrxID"].Value;
                parentTrxType = xDocSelectedRow.SelectSingleNode("Rows").Attributes["TrxType"].Value;
            }

            string xPath = "Root/bpeout/FormControls/" + branchNodeName + "/RowList/Rows[@" + treeNodeName + "_TrxID='" + parentTrxID + "' and @" + treeNodeName + "_TrxType='" + parentTrxType + "']";
            XmlNodeList nodeBranchRows = xDocOut.SelectNodes(xPath);

            //Create a XML node object for the dataset to read the XML.
            XmlNode nodeRows = xDocOut.CreateElement("RowList");
            foreach (XmlNode nodeChildRow in nodeBranchRows)
            {
                nodeRows.AppendChild(nodeChildRow);
            }

            if (postBackCtrl != null && (postBackCtrl.ID.ToUpper() == "IMGBTNADD" || m_IsFindMode || postBackCtrl.ID == "imgbtnContinueAdd") && !postBackCtrl.ID.Contains("AddNew"))
            {
                dsRowList = GetNewTableSchema(xDocOut, branchNodeName, grdVwBranch, dsRowList);
            }
            else
            {
                bool executeReader = true;
                if (gridMode == "ADD" && postBackCtrl != null && postBackCtrl.ID.Contains("lnkBtnAutoFillVendor"))
                {
                    executeReader = false;
                }

                //Added a condition to prevent the grid binding with the previously selected parent row's data upon SelectRequest operation on 17/10/08
                if (nodeRows != null && nodeRows.ChildNodes.Count > 0 && executeReader)
                {
                    XmlNodeReader read = new XmlNodeReader(nodeRows);
                    dsRowList.ReadXml(read);
                }
            }
            if (dsRowList.Tables.Count == 0)
            {
                dsRowList = GetNewTableSchema(xDocOut, branchNodeName, grdVwBranch, dsRowList);
            }

            //Persist the number of rows originally present in the gridview
            //int gvRowCount = grdVwBranch.Rows.Count;
            //if (gvRowCount < 10)
            //{
            //    gvRowCount = 10;
            //}

            while (dsRowList.Tables[0].Rows.Count < RowCount)
            {
                dsRowList.Tables[0].Rows.Add(dsRowList.Tables[0].NewRow());
            }

            //Initialising the Bound fields.
            bool isOkToAddFields = false;
            if (grdVwBranch.Columns.Count == 1)
            {
                isOkToAddFields = true;
            }

            //Set the Grid Heading
            string headerTitle = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Title").InnerText;
            //Find the HeaderTitle TD in the Page
            HtmlTableCell htcHeader = (HtmlTableCell)pnlEntryForm.FindControl("htcHeader" + branchNodeName);
            if (htcHeader != null)
                htcHeader.InnerText = headerTitle;

            ////Set the style of the header so that it remains fixed while scrolling.
            //grdVwBranch.HeaderStyle.CssClass = "GvHeaderFreeze";

            //Reset necessary fieds.
            int colCntr = 0;
            int processColCntr = 0;//Tracks the number of columns added due to BusinessProcesses.
            m_htDDLIndices.Clear();
            m_htLabelIndices.Clear();
            m_htCheckBoxIndices.Clear();
            m_htCalendarIndices.Clear();
            m_htTextBoxIndices.Clear();
            m_htTextBoxCalcIndices.Clear();
            m_htTextBoxIsLink.Clear();
            m_arrAmountCols.Clear();

            Table tblOperations = (Table)pnlEntryForm.FindControl("tblOperationLinks");
            TableRow tr = null;
            if (tblOperations != null)
            {
                tr = tblOperations.Rows[0];//Only one biigg row.
            }
            //Loop the columns.
            foreach (XmlNode colNode in nodeBranchColumns.ChildNodes)
            {
                //Add the column only if FullViewLength is not equal to zero
                int colFullViewLength = Convert.ToInt32(colNode.Attributes["FullViewLength"].Value);
                if (colFullViewLength != 0)
                {
                    string currentLabel = colNode.Attributes["Label"].Value.Trim();

                    //Adding the current column to the dataset if not present.
                    if (!dsRowList.Tables[0].Columns.Contains(currentLabel))
                    {
                        DataColumn dcNew = new DataColumn(currentLabel, typeof(string));
                        dcNew.AllowDBNull = true;
                        dsRowList.Tables[0].Columns.Add(dcNew);
                    }

                    BoundField newField = new BoundField();
                    newField.DataField = currentLabel;
                    newField.HeaderText = colNode.Attributes["Caption"].Value.Trim();
                    newField.HeaderStyle.Wrap = true;

                    //Check for IsDisplayOnly
                    bool isDisplayOnly = false;
                    if (colNode.Attributes["IsDisplayOnly"] != null
                                && colNode.Attributes["IsDisplayOnly"].Value.Trim() == "1")
                    {
                        m_arrIsDisplayOnlyCols.Add(colCntr);
                    }

                    ////Check for IsNumeric
                    //if (colNode.Attributes["IsNumeric"] != null && colNode.Attributes["IsNumeric"].Value == "1")
                    //{
                    //    m_arrIsNumericCols.Add(colCntr);
                    //}

                    if (colNode.Attributes["ControlType"] != null)
                    {
                        switch (colNode.Attributes["ControlType"].Value)
                        {
                            case "DDL":
                                {
                                    //DropDownList
                                    m_htDDLIndices.Add(colCntr, currentLabel);
                                    break;
                                }
                            case "Label":
                                {
                                    //Label
                                    m_htLabelIndices.Add(colCntr, currentLabel);
                                    break;
                                }
                            case "Cal":
                                {
                                    //Calendar
                                    //Apply formatting to the DateTime fields in the DataSet.
                                    foreach (DataRow dr in dsRowList.Tables[0].Rows)
                                    {
                                        //If the value is IsDate then change format MM/DD/YY
                                        DateTime date;
                                        if (DateTime.TryParse(dr[currentLabel].ToString(), out date))
                                        {
                                            dr[currentLabel] = date.ToString("MM/dd/yy");
                                        }
                                    }
                                    m_htCalendarIndices.Add(colCntr, currentLabel);
                                    break;
                                }
                            case "Check":
                                {
                                    //CheckBox
                                    m_htCheckBoxIndices.Add(colCntr, currentLabel);
                                    break;
                                }
                            case "TBox":
                                {
                                    m_htTextBoxIndices.Add(colCntr, currentLabel);
                                    //If textbox contains islink attribute
                                    if ((colNode.Attributes["IsLink"] != null) && (colNode.Attributes["IsLink"].Value == "1"))
                                    {
                                        //if (currentLabel == "EntryJob")
                                        //{
                                        //    HttpContext.Current.Session["EntryJob"] = GetDropDownData("EntryJob");
                                        //}
                                        m_htTextBoxIsLink.Add(colCntr, currentLabel);
                                    }
                                    break;
                                }
                            case "Amount":
                                {
                                    //Presumptions for this Control Type
                                    //It is a textbox
                                    m_htTextBoxIndices.Add(colCntr, currentLabel);
                                    m_arrAmountCols.Add(colCntr);
                                    break;
                                }
                            case "Calc":
                                {
                                    m_htTextBoxIndices.Add(colCntr, currentLabel);
                                    m_arrAmountCols.Add(colCntr);
                                    m_htTextBoxCalcIndices.Add(colCntr, currentLabel);
                                    //If textbox contains islink attribute
                                    if ((colNode.Attributes["IsLink"] != null) && (colNode.Attributes["IsLink"].Value == "1"))
                                    {
                                        m_htTextBoxIsLink.Add(colCntr, currentLabel);
                                    }
                                    break;
                                }
                            case "Link":
                                {
                                    m_htLinkIndices.Add(colCntr, currentLabel);
                                    //string linkHTML = "<a oncontextmenu='return false;' Title='######'" + " href=javascript:OnColumnLinkClick('" + currentBPGID + "','" + pageInfo + "','" + hdnCurrentRow.ClientID + "','" + TrxID + "','" + TrxType + "','" + hdnFormInfo.ClientID + "','" + drvCurrent.Row.ItemArray[currentBPCIndex].ToString().Replace(" ", "~::~") + "','" + isPopUp + "')>"
                                    //    + drvCurrent.Row.ItemArray[currentBPCIndex].ToString() + "</a>";
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }
                    //Set the sort expression as inidicated in the xml.
                    if (colNode.Attributes["IsSortable"].Value == "1")
                    {
                        newField.SortExpression = currentLabel;
                    }

                    if (colNode.Attributes["IsRequired"] != null && colNode.Attributes["IsRequired"].Value == "1")
                    {
                        m_arrIsRequiredCols.Add(colCntr);
                    }

                    //Check if the current column node has any processes assigned
                    if (tblOperations != null && colNode.Attributes["BPControl"] != null && colNode.Attributes["BPControl"].Value.Trim().Length > 0)
                    {
                        string procName = colNode.Attributes["BPControl"].Value;
                        XmlNode nodeProc = GetBPCNode(procName, xDocOut);
                        string currentBPGID = nodeProc.Attributes["BPGID"].Value;
                        string pageInfo = nodeProc.Attributes["PageInfo"].Value;

                        TableCell tdLinkButton = new TableCell();
                        LAjitLinkButton lnkBtnBPC = new LAjitLinkButton();
                        lnkBtnBPC.Enabled = !m_IsChildGridEnabled;
                        lnkBtnBPC.ID = "lnkBtnBPC" + processColCntr;
                        lnkBtnBPC.Text = nodeProc.Attributes["Label"].Value;
                        lnkBtnBPC.OnClientClick = "javascript:OnChildBPCLinkClick(this,'" + currentBPGID + "','" + pageInfo + "','"
                                    + grdVwBranch.ClientID + "','" + nodeProc.Attributes["ID"].Value + "','" + nodeProc.Attributes["IsPopup"].Value + "');return false";
                        // lnkBtnBPC.Click += new EventHandler(lnkBtnBPC_Click);
                        tdLinkButton.Controls.Add(lnkBtnBPC);

                        TableCell tdSeparator = new TableCell();
                        tdSeparator.Width = Unit.Pixel(6);
                        tdSeparator.Text = "|";
                        Control ctrlBPC = tblOperations.FindControl(lnkBtnBPC.ID);
                        if (ctrlBPC == null)
                        {
                            tr.Controls.AddAt(0, tdLinkButton);
                            tr.Controls.AddAt(1, tdSeparator);
                        }
                        else
                        {
                            ((LAjitLinkButton)ctrlBPC).Enabled = !m_IsChildGridEnabled;
                        }
                        processColCntr++;
                    }

                    //Add the field now.
                    if (isOkToAddFields)
                    {
                        grdVwBranch.Columns.Add(newField);
                    }
                    //Set the widhts of the controls in the column as per the FullView Length
                    grdVwBranch.Columns[colCntr + 1].ControlStyle.Width = Unit.Pixel(colFullViewLength * 7);//Assuming one character to be around 7px in width
                    grdVwBranch.Columns[colCntr + 1].ItemStyle.Width = Unit.Pixel(colFullViewLength * 7);


                    m_arrColLabels.Add(currentLabel);
                    colCntr++;
                }
            }





            if (tblOperations != null)
            {
                int cellCount = 9;//No of cells containing the operationlinks, their separators.
                if (m_ShowOpLinks)
                {
                    if (tr.FindControl("lnkBtnCopy") == null || tr.FindControl("lnkBtnPaste") == null)
                    {
                        TableCell tdCopy = new TableCell();
                        LAjitLinkButton lnkBtnCopy = new LAjitLinkButton();
                        lnkBtnCopy.ID = "lnkBtnCopy";
                        lnkBtnCopy.Text = "Copy";
                        lnkBtnCopy.OnClientClick = "CopyGridRows('" + grdVwBranch.ClientID + "',this);return false;";
                        tdCopy.Controls.Add(lnkBtnCopy);

                        TableCell tdSeparatorC = new TableCell();
                        tdSeparatorC.Width = Unit.Pixel(6);
                        tdSeparatorC.Text = "|";

                        TableCell tdPaste = new TableCell();
                        LAjitLinkButton lnkBtnPaste = new LAjitLinkButton();
                        lnkBtnPaste.ID = "lnkBtnPaste";
                        lnkBtnPaste.Text = "Paste";
                        lnkBtnPaste.OnClientClick = "PasteGridRows('" + grdVwBranch.ClientID + "',this);return false;";
                        tdPaste.Controls.Add(lnkBtnPaste);

                        TableCell tdSeparatorP = new TableCell();
                        tdSeparatorP.Width = Unit.Pixel(6);
                        tdSeparatorP.Text = "|";

                        tr.Controls.Add(tdSeparatorC);
                        tr.Controls.Add(tdCopy);
                        tr.Controls.Add(tdSeparatorP);
                        tr.Controls.Add(tdPaste);
                    }
                }
                else
                {
                    //Hide the static cells(excluding the BPC Cells) that were previouly added and reset the cellCount varaible.
                    for (int index = 0; index < tr.Controls.Count; index++)
                    {
                        ControlCollection ccCell = tr.Controls[index].Controls;
                        if (ccCell.Count > 0)
                        {
                            if (ccCell[0].ID != null && !ccCell[0].ID.Contains("lnkBtnBPC"))
                            {
                                tr.Controls.Remove(tr.Controls[index--]);
                                if (index < 0)
                                {
                                    continue;
                                }
                                tr.Controls.Remove(tr.Controls[index--]);
                            }
                        }

                    }
                    cellCount = 0;
                }

                m_SelectColVisible = grdVwBranch.Columns[0].Visible;

                StringBuilder sbAmountCols = new StringBuilder(m_arrAmountCols.Count * 2);
                foreach (int amtColIndex in m_arrAmountCols)
                {

                    sbAmountCols.Append(amtColIndex + "*");
                    string amtColName = ((BoundField)grdVwBranch.Columns[amtColIndex + 1]).DataField;//+1 override the static column.
                    //Calculate the column's initial sum.
                    decimal totalAmount = 0;
                    decimal rowAmount;

                    //Commented code's functionality is being acheived through javascript.
                    if (!grdVwBranch.Page.IsPostBack)
                    {
                        if (!pnlEntryForm.Page.ToString().Contains("selectinvoice"))
                        {
                            foreach (DataRow dr in dsRowList.Tables[0].Rows)
                            {
                                if (decimal.TryParse(Convert.ToString(dr[amtColName]), out rowAmount))
                                {
                                    totalAmount += rowAmount;
                                }
                            }
                        }
                    }

                    if (m_AddAmountLabels)
                    {
                        TableCell tdSeparatorAmt = new TableCell();
                        tdSeparatorAmt.Width = Unit.Pixel(6);
                        tdSeparatorAmt.Text = "|";

                        TableCell tdAmountSum = new TableCell();
                        tdAmountSum.ID = "tdAmount" + amtColIndex;
                        tdAmountSum.CssClass = "mbodyb";
                        tdAmountSum.Text = grdVwBranch.Columns[amtColIndex + 1].HeaderText + " : <span id='tdAmt" + amtColIndex + "'>" + string.Format("{0:N}", totalAmount) + "</span>";

                        if (tr.FindControl("tdAmount" + amtColIndex) == null)
                        {
                            //IF current col is the first column being added and there no other BPC links then...
                            if ((amtColIndex == Convert.ToInt32(m_arrAmountCols[0])) && tr.Cells.Count == 0)
                            {
                                tr.Controls.Add(tdAmountSum);
                                cellCount = cellCount + 1;
                            }
                            else
                            {
                                tr.Controls.Add(tdSeparatorAmt);
                                tr.Controls.Add(tdAmountSum);
                                //If Amount Label is present increment the cellCount by 2
                                cellCount = cellCount + 2;
                            }
                        }

                    }
                    else
                    {
                        TableCell tdAmountSum = (TableCell)tr.FindControl("tdAmount" + amtColIndex);
                        tdAmountSum.Text = grdVwBranch.Columns[amtColIndex + 1].HeaderText + " : <span id='tdAmt" + amtColIndex + "'>" + totalAmount + "</span>";
                    }
                }
                m_AddAmountLabels = false;

                //Disable the operations link buttons explicitly
                //for (int cntr = tr.Cells.Count - 1; cntr >= tr.Cells.Count - cellCount; cntr--)
                //{
                //    tr.Cells[cntr].Enabled = m_IsChildGridEnabled;
                //}
                if (m_IsChildGridEnabled == true)
                {
                    tr.Attributes.Add("style", "DISPLAY: Block;");
                }
                else
                {
                    tr.Attributes.Add("style", "DISPLAY: none;");
                }

                //Check for SelectInvoice.aspx-Display only sum of selected rows.
                string addSelected = "false";
                if (pnlEntryForm.Page.ToString().Contains("selectinvoice"))
                {
                    addSelected = "true";
                }

                m_OnGridLoadJSCall = "if(typeof DisplayOnGridLoadSum== 'function')DisplayOnGridLoadSum('" + grdVwBranch.ClientID + "', '" + sbAmountCols.ToString() + "');var g_AddSelected=" + addSelected + "; ";
                ScriptManager.RegisterClientScriptBlock(grdVwBranch.Page, grdVwBranch.Page.GetType(), "ShowOnGridLoadSum", m_OnGridLoadJSCall, true);
            }


            //If the data source consists of more than specified records implement vertical scrolling.
            Panel pnlGVContainer = (Panel)pnlEntryForm.FindControl("pnlGVBranch");
            //Implement vertical scrolling by default
            pnlGVContainer.Height = Unit.Pixel(230);
            pnlGVContainer.ScrollBars = ScrollBars.Both;
            int maxRowSize = Convert.ToInt32(ConfigurationManager.AppSettings["BranchGVMaxRows"]);
            if (dsRowList.Tables[0].Rows.Count < maxRowSize)
            {
                //Find the immediate parent container panel of the grid view 

                pnlGVContainer.Style.Add("height", "auto");
                pnlGVContainer.ScrollBars = ScrollBars.Horizontal;
            }

            m_primaryKeyFieldName = dsRowList.Tables[0].Columns[0].ColumnName;
            grdVwBranch.DataSource = dsRowList;
            grdVwBranch.RowDataBound += new GridViewRowEventHandler(OnRowDataBound);
            grdVwBranch.DataBind();


            //Add validation for Cancel(Ex Changes submitted to server or not..)
            ImageButton imgbtnCancel = (ImageButton)pnlEntryForm.FindControl("imgbtnCancel");
            imgbtnCancel.OnClientClick = "return ValidateChangesSubmitted('" + grdVwBranch.ClientID + "')";

            //if (postBackCtrl != null && postBackCtrl.ID.Contains("addclone"))
            //{
            //    ReEnableChildObject((Panel)pnlEntryForm, "ADDCLONE");
            //}


            ////Finally update the changes to the client
            //UpdatePanel updatePnlChildGV = (UpdatePanel)pnlGVContainer.FindControl("updatePnlChildGV");
            //if (updatePnlChildGV != null)
            //    updatePnlChildGV.Update();

            ////Set the hidden field with the current row count of the grid(Will be used in js Added rows functionality)
            //HiddenField hdnRowsToDelete = (HiddenField)pnlEntryForm.Parent.FindControl("hdnRowCount" + branchNodeName);
            //hdnRowsToDelete.Value = dsRowList.Tables[0].Rows.Count.ToString();
            ////Re-initialise the checkbox selection.
            //string[] checkBoxSelection = grdVwBranch.Attributes["CheckBoxSel"].Split(',');
            //foreach (GridViewRow gvrCheck in grdVwBranch.Rows)
            //{
            //    CheckBox checkBox = (CheckBox)gvrCheck.FindControl("chkBxSelectRow");
            //    for (int i = 0; i < checkBoxSelection.Length; i++)
            //    {
            //        if (checkBoxSelection[i] == gvrCheck.RowIndex.ToString())
            //        {
            //            checkBox.Checked = true;
            //            break;
            //        }
            //    }
            //}

            //Client-side functionality done Shanti requires no disabling or changing of colour be done on the server-side
            m_IsChildGridEnabled = true;
            m_IsFindMode = false;

        }

        void lnkBtnBPC_Click(object sender, EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Gets the Business Process Control node based on the passed process ID.
        /// </summary>
        /// <param name="processName">The Process ID.</param>
        /// <param name="xDocForm">The XML Document containing the BPC node.</param>
        /// <returns>The BPC node.</returns>
        private XmlNode GetBPCNode(string processName, XmlDocument xDocForm)
        {
            #region NLog
            logger.Info("Getting the Business Process Control node based on the passed process ID.");
            #endregion

            return xDocForm.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess[@ID='" + processName + "']");
        }

        /// <summary>
        /// Initialises the an attribute of the grid view conisting of the checkbox selection.
        /// Purpose: To maintain checkbox selection across postbacks.
        /// </summary>
        /// <param name="grdVwBranch">GridView Object.</param>
        /// <param name="reset">Resets all the checkboxes to unchecked state.</param>
        private void InitCheckBoxSelection(GridView grdVwBranch, bool reset)
        {
            StringBuilder sbSelection = new StringBuilder(grdVwBranch.Rows.Count);
            foreach (GridViewRow row in grdVwBranch.Rows)
            {
                CheckBox chkBxSelectRow = (CheckBox)row.Cells[0].FindControl("chkBxSelectRow");
                if (chkBxSelectRow == null)
                {
                    chkBxSelectRow = new CheckBox();
                    row.Cells[0].Controls.Add(chkBxSelectRow);
                    continue;
                }
                if (chkBxSelectRow.Checked)
                {
                    if (!reset)
                    {
                        sbSelection.Append(row.RowIndex.ToString() + ",");
                    }
                }
                else
                {
                    chkBxSelectRow.Checked = false;
                }
            }
            grdVwBranch.Attributes["CheckBoxSel"] = sbSelection.ToString();
        }

        /// <summary>
        /// Gets the table schema from the XML and creates a table with a default number of blank rows.
        /// </summary>
        /// <param name="xDocOut">The form XML.</param>
        /// <param name="branchNodeName">The name of the branch.</param>
        /// <param name="grdVwBranch">Gridview object.</param>
        /// <param name="dsRowList">The parent dataset to bind to the gridview.</param>
        private DataSet GetNewTableSchema(XmlDocument xDocOut, string branchNodeName, GridView grdVwBranch, DataSet dsRowList)
        {
            #region NLog
            logger.Info("Getting the table schema from the XML and creates a table with a default number of blank rows.");
            #endregion

            XmlNode nodeColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
            DataTable dtAddNew = new DataTable("Rows");
            //Create the schema for the table.
            foreach (XmlNode nodeCol in nodeColumns.ChildNodes)
            {
                DataColumn dcNew = new DataColumn(nodeCol.Attributes["Label"].Value, typeof(string));
                dtAddNew.Columns.Add(dcNew);
            }
            //For fullview page(Reuires single row only).
            if (grdVwBranch.Page.ToString().Contains("fullview"))
            {
                dtAddNew.Rows.Add(dtAddNew.NewRow());
            }
            else
            {
                //Add some rows now.
                int blockSize = Convert.ToInt32(ConfigurationManager.AppSettings["AddBlockSize"]);
                for (int i = 0; i < blockSize; i++)
                {
                    dtAddNew.Rows.Add(dtAddNew.NewRow());
                }
            }
            //Add the table to a dataset.
            if (dsRowList.Tables.Count == 0)
            {
                dsRowList.Tables.Add(dtAddNew);
                dsRowList.AcceptChanges();
            }
            return dsRowList;
        }

        /// <summary>
        /// Row Data bound event for the grid view object
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Arguments.</param>
        public void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            string[] checkBoxSelection = ((GridView)e.Row.Parent.Parent).Attributes["CheckBoxSel"].Split(',');
            //bool gridEnabled = true;
            //Control cphThisPage = e.Row.NamingContainer.NamingContainer;
            //ImageButton imgbtnSubmit = (ImageButton)cphThisPage.FindControl("imgbtnSubmit");
            //string gridMode = imgbtnSubmit.AlternateText;



            string gridClientID = ((GridView)sender).ClientID;
            DataRowView drvCurrent = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Set an ID to row(To be used in the client-side fading effects)
                e.Row.ID = "GVRow" + e.Row.RowIndex;
                int cellCntr = 0;
                //Row data binding for BPC's
                //int procCntr = 0;
                //foreach (XmlNode nodeProc in m_arrChildProcs)
                //{
                //    ImageButton imgBtnProc = new ImageButton();
                //    imgBtnProc.ImageUrl = "~/App_Themes/" + HttpContext.Current.Session["MyTheme"].ToString() + "/Images/GridViewSelect.png";
                //    imgBtnProc.ID = gridClientID + "_" + e.Row.RowIndex + "_" + procCntr;
                //    imgBtnProc.Click += new ImageClickEventHandler(imgBtnProc_Click);
                //    e.Row.Cells[procCntr + 1].Controls.Clear();
                //    e.Row.Cells[procCntr + 1].Controls.Add(imgBtnProc);
                //    procCntr++;
                //}
                //First field is always a primary key!!
                string primaryKeyValue = drvCurrent.Row.ItemArray[0].ToString();
                //Create the row xml to append to the checkbox as attribute
                string rowXML = GetRowXml(drvCurrent.DataView.Table, primaryKeyValue);
                foreach (TableCell tc in e.Row.Cells)
                {
                    int currentCellIndex = e.Row.Cells.GetCellIndex(tc);
                    string cellText = tc.Text.Trim();
                    if (cellText == "&nbsp;")
                    {
                        cellText = string.Empty;
                    }
                    //Re-Init the checkbox
                    if (currentCellIndex == 0)//First cell consists of the checkbox.
                    {
                        CheckBox chkBxSelect = (CheckBox)tc.FindControl("chkBxSelectRow");



                        chkBxSelect.Attributes["RowInfo"] = rowXML;
                        chkBxSelect.Attributes["RowIndex"] = e.Row.RowIndex.ToString();
                        for (int i = 0; i < checkBoxSelection.Length; i++)
                        {
                            if (checkBoxSelection[i] == e.Row.RowIndex.ToString())
                            {
                                chkBxSelect.Checked = true;
                                break;
                            }
                        }
                        continue;
                    }
                    tc.Controls.Clear();
                    tc.Enabled = m_IsChildGridEnabled;
                    if (m_htDDLIndices.ContainsKey(cellCntr))
                    {
                        //Implies this cell should contain a Drop Down List
                        string labelColumn = m_htDDLIndices[cellCntr].ToString();
                        string ddlTrxID = labelColumn + "_TrxID";
                        string ddlTrxType = labelColumn + "_TrxType";
                        string selectedValue = string.Empty;
                        string cellContent = drvCurrent.Row[labelColumn].ToString();
                        if (drvCurrent.DataView.Table.Columns.Contains(ddlTrxID) && drvCurrent.DataView.Table.Columns.Contains(ddlTrxType))
                        {
                            selectedValue = drvCurrent.Row[ddlTrxID].ToString() + "~" + drvCurrent.Row[ddlTrxType].ToString();
                        }
                        else if (cellContent.Length > 0)
                        {
                            //Check for the existence of a selected value string in the column
                            //For rebinding the data which has been entered in the UI before submission to the server.
                            if (cellContent.Contains("~"))//Validate for correct selected value format(TrxId~TrxType)
                            {
                                selectedValue = cellContent;
                            }
                        }

                        string rowType = string.Empty;
                        if (drvCurrent.DataView.Table.Columns.Contains("RowType"))
                        {
                            rowType = drvCurrent.Row["RowType"].ToString();
                        }

                        LAjitDropDownList ddlCurrent = CreateDropDownList(labelColumn, selectedValue, rowType, e.Row.ClientID.ToString());
                        if (m_arrIsRequiredCols.Contains(cellCntr) && !m_IsFindMode)
                        {
                            ddlCurrent.Attributes.Add("onblur", "javascript:ValidateChildControl(this)");
                        }

                        if (m_arrIsDisplayOnlyCols.Contains(cellCntr))//Show the Control in its actual form but disable it.
                        {
                            ddlCurrent.Enabled = false;
                            ddlCurrent.BackColor = Color.LightGray;
                        }
                        else
                        {
                            if (!m_IsChildGridEnabled)
                            {
                                ddlCurrent.BackColor = Color.LightGray;
                            }
                            else if (m_IsFindMode)
                            {
                                ddlCurrent.BackColor = Color.LightGoldenrodYellow;
                            }
                        }
                        tc.Controls.Add(ddlCurrent);
                    }
                    else if (m_htCalendarIndices.ContainsKey(cellCntr))
                    {
                        //Calendar Control Textbox with cal image
                        if (cellCntr > m_arrColLabels.Count)
                        {
                            continue;
                        }
                        string currentControlName = Convert.ToString(m_arrColLabels[cellCntr]);
                        LAjitTextBox txtCurrentCell = new LAjitTextBox();
                        txtCurrentCell.ID = "txt" + currentControlName;
                        txtCurrentCell.Text = cellText;
                        txtCurrentCell.MapXML = currentControlName;

                        //alCalendarTBoxIDS.Add(txtCurrentCell.ClientID);

                        if (m_arrIsDisplayOnlyCols.Contains(cellCntr))//Show the Control in its actual form but disable it.
                        {
                            txtCurrentCell.Enabled = false;
                            txtCurrentCell.BackColor = Color.LightGray;
                            tc.Controls.Add(txtCurrentCell);
                            cellCntr++;
                            continue;
                        }

                        ////Ajax Controls
                        //AjaxControlToolkit.CalendarExtender ceCurrent = new AjaxControlToolkit.CalendarExtender();
                        //ceCurrent.ID = "ce" + currentControlName;
                        ////ceCurrent.BehaviorID = "bce" + currentControlName;
                        //ceCurrent.TargetControlID = "txt" + currentControlName;
                        //ceCurrent.Format = "MM/dd/yy";


                        //AjaxControlToolkit.MaskedEditExtender meeCurrent = new AjaxControlToolkit.MaskedEditExtender();
                        //meeCurrent.ID = "mee" + currentControlName;
                        //meeCurrent.TargetControlID = "txt" + currentControlName;
                        //meeCurrent.MessageValidatorTip = true;
                        //meeCurrent.Mask = "99/99/99";
                        //meeCurrent.MaskType = AjaxControlToolkit.MaskedEditType.Date;
                        //meeCurrent.PromptCharacter = "_";
                        ////meeCurrent.OnFocusCssClass = "MaskedEditFocus";
                        //meeCurrent.OnInvalidCssClass = "MaskedEditError";
                        ////meeCurrent.MessageValidatorTip = true;
                        //meeCurrent.ClearTextOnInvalid = true;

                        //Add client-side script to validate the date selected/entered.
                        txtCurrentCell.Attributes.Add("onchange", "javascript:CheckDate(this);");

                        if (m_arrIsRequiredCols.Contains(cellCntr) && !m_IsFindMode)
                        {
                            txtCurrentCell.Attributes.Add("onblur", "ValidateChildControl(this);");
                        }

                        if (!m_IsChildGridEnabled)
                        {
                            txtCurrentCell.BackColor = Color.LightGray;
                        }
                        else if (m_IsFindMode)
                        {
                            txtCurrentCell.BackColor = Color.LightGoldenrodYellow;
                        }
                        tc.Controls.Add(txtCurrentCell);
                        //tc.Controls.Add(ceCurrent);
                        //tc.Controls.Add(meeCurrent);
                    }
                    else if (m_htTextBoxIndices.ContainsKey(cellCntr))
                    {
                        if (cellCntr > m_arrColLabels.Count)
                        {
                            continue;
                        }
                        LAjitTextBox txtCurrentCell = new LAjitTextBox();
                        tc.Controls.Add(txtCurrentCell);//Make sure the first control to be added to the cell is the Texbox rather than any extender objects.

                        if (m_htTextBoxIndices.ContainsKey(cellCntr) != m_htTextBoxCalcIndices.ContainsKey(cellCntr))
                        {
                            //txtCurrentCell.Width = Unit.Pixel(60);
                            txtCurrentCell.MapXML = Convert.ToString(m_arrColLabels[cellCntr]);
                            txtCurrentCell.ID = "txt" + Convert.ToString(m_arrColLabels[cellCntr]);
                            txtCurrentCell.Text = HtmlCodesToCharacters(cellText);
                            txtCurrentCell.Attributes.Add("onfocus", "javascript:Hidden(this)");
                            txtCurrentCell.Attributes.Add("onclick", "javascript:Hidden(this)");
                        }
                        else
                        {
                            if (m_htTextBoxCalcIndices.ContainsKey(cellCntr))
                            {
                                txtCurrentCell.MapXML = Convert.ToString(m_arrColLabels[cellCntr]);
                                txtCurrentCell.ID = "txt" + Convert.ToString(m_arrColLabels[cellCntr]);
                                decimal amount;
                                if (Decimal.TryParse(cellText, out amount))
                                {
                                    txtCurrentCell.Text = string.Format("{0:N}", amount);
                                }
                                txtCurrentCell.Attributes.Add("onfocus", "javascript:Hidden(this)");
                                txtCurrentCell.Attributes.Add("ondblclick", "javascript:ShowCalc(this)");
                                //on lost focus change amount to decimals 
                                if (!m_IsFindMode)
                                {
                                    txtCurrentCell.Attributes.Add("onblur", "javascript:FilterAmount(this)");
                                }
                            }
                        }
                        if (m_arrIsRequiredCols.Contains(cellCntr) && !m_IsFindMode)
                        {
                            txtCurrentCell.Attributes.Add("onblur", "javascript:ValidateChildControl(this)");
                        }

                        if (m_arrIsDisplayOnlyCols.Contains(cellCntr))//Show the Control in its actual form but disable it.
                        {
                            txtCurrentCell.Enabled = false;
                            txtCurrentCell.BackColor = Color.LightGray;
                        }
                        else
                        {
                            if (!m_IsChildGridEnabled)
                            {
                                txtCurrentCell.BackColor = Color.LightGray;
                            }
                            else if (m_IsFindMode)
                            {
                                txtCurrentCell.BackColor = Color.LightGoldenrodYellow;
                            }
                        }
                        //tc.Controls.Add(txtCurrentCell);//Commented By Danny on 10 Oct 08
                        //adding onchange event attribute for textbox to return column sum 
                        //int gvColIndex = cellCntr + 1;
                        //string jsFunctionCall = "ColumnSum('" + gridClientID + "','" + gvColIndex + "')";
                        //txtCurrentCell.Attributes.Add("onchange", jsFunctionCall);
                        if (m_htTextBoxIsLink.ContainsKey(cellCntr))
                        {
                            //Ajax
                            //txtCurrentCell.AutoCompleteType = "off";
                            /*AjaxControlToolkit.AutoCompleteExtender aceCurrent = new AjaxControlToolkit.AutoCompleteExtender();
                            aceCurrent.ID = "ace" + Convert.ToString(m_arrColLabels[cellCntr]);
                            aceCurrent.TargetControlID = "txt" + Convert.ToString(m_arrColLabels[cellCntr]);
                            // aceCurrent.ServicePath = "./WebService/AutoComplete.asmx";
                            aceCurrent.ServicePath = "../WebService/AutoComplete.asmx";
                            aceCurrent.ServiceMethod = "GetChildCompletionList";
                            aceCurrent.MinimumPrefixLength = 1;
                            aceCurrent.CompletionInterval = 1000;
                            aceCurrent.EnableCaching = true;
                            aceCurrent.CompletionSetCount = 12;
                            aceCurrent.UseContextKey = true;
                            aceCurrent.ContextKey = Convert.ToString(m_arrColLabels[cellCntr]);
                            aceCurrent.CompletionListCssClass = "autocomplete_completionListElement";
                            aceCurrent.CompletionListItemCssClass = "autocomplete_listItem";
                            aceCurrent.CompletionListHighlightedItemCssClass = "autocomplete_highlightedListItem";
                            aceCurrent.FirstRowSelected = true;*/

                            //Anand
                            //txtCurrentCell.Width = Unit.Pixel(100);
                            //AutoCompleteType=Disabled
                            /* txtCurrentCell.AutoCompleteType = AutoCompleteType.Disabled;
                             txtCurrentCell.Attributes.Add("autocomplete", "off");
                             //txtCurrentCell.Attributes.Add("onchange", "javascript:alert('HI');");
                             //tc.Controls.Add(txtCurrentCell);//Commented By Danny on 10 Oct 08
                             tc.Controls.Add(aceCurrent);*/
                        }

                        //Normal TextBox with Amount formatting.
                        //Check for Amount;Right-Justify and apply formatting.
                        //if (((DataControlFieldCell)tc).ContainingField.HeaderText == "Amount")
                        if (m_arrAmountCols.Contains(cellCntr))
                        {
                            decimal amount;
                            if (Decimal.TryParse(txtCurrentCell.Text, out amount))
                            {
                                txtCurrentCell.Text = string.Format("{0:N}", amount);
                            }
                            txtCurrentCell.Attributes["IsAmount"] = cellCntr.ToString();// "1";-Changed by Danny on 31/12/2008
                            txtCurrentCell.Style.Add("TEXT-ALIGN", "right");
                            txtCurrentCell.Attributes.Add("onkeyup", "javascript:ShowInsantaneousSum('" + gridClientID + "','" + cellCntr + "')");
                            txtCurrentCell.Attributes.Add("onpaste", "javascript:ShowInsantaneousSum('" + gridClientID + "','" + cellCntr + "')");
                        }
                    }
                    else if (m_htCheckBoxIndices.ContainsKey(cellCntr))
                    {
                        if (cellCntr > m_arrColLabels.Count)
                        {
                            continue;
                        }
                        LAjitCheckBox chkCurrentCell = new LAjitCheckBox();
                        chkCurrentCell.MapXML = Convert.ToString(m_arrColLabels[cellCntr]);
                        chkCurrentCell.ID = "chkBx" + Convert.ToString(m_arrColLabels[cellCntr]);
                        if (cellText == "1")
                        {
                            chkCurrentCell.Checked = true;
                        }
                        if (m_arrIsDisplayOnlyCols.Contains(cellCntr))//Show the Control in its actual form but disable it.
                        {
                            chkCurrentCell.Enabled = false;
                        }
                        //else
                        //{
                        //    if (!m_IsChildGridEnabled)
                        //    {
                        //        chkCurrentCell.BackColor = Color.LightGray;
                        //    }
                        //    else if (m_IsFindMode)
                        //    {
                        //        chkCurrentCell.BackColor = Color.LightGoldenrodYellow;
                        //    }
                        //}


                        //Add the OnGridLoad summation for SelectInvoice page.
                        if (((GridView)sender).Page.ToString().Contains("selectinvoice"))
                        {
                            chkCurrentCell.Attributes.Add("onclick", m_OnGridLoadJSCall.Replace("var", ""));
                        }

                        tc.Controls.Add(chkCurrentCell);
                    }
                    else if (m_htLinkIndices.ContainsKey(cellCntr))
                    {
                        tc.Text = "";
                    }
                    else
                    {
                        //This is a Label
                        LAjitLabel lblCellText = new LAjitLabel();
                        lblCellText.MapXML = Convert.ToString(m_arrColLabels[cellCntr]);

                        lblCellText.BorderStyle = BorderStyle.None;
                        lblCellText.Style.Add("background-color", "Transparent");
                        lblCellText.Style.Add("font-size", "11px");
                        lblCellText.Style.Add("font-family", "Arial, Helvetica, sans-serif");

                        //Check for IsNumeric-apply formatting.
                        if (m_arrAmountCols.Contains(cellCntr))
                        {
                            decimal amount;
                            if (Decimal.TryParse(cellText, out amount))
                            {
                                lblCellText.Text = string.Format("{0:N}", amount);
                                lblCellText.ToolTip = lblCellText.Text;
                            }
                            lblCellText.Attributes["IsAmount"] = "1";
                        }
                        tc.Controls.Add(lblCellText);
                    }

                    ////Setting the js to be called if the control is marked "IsRequired"
                    //if (m_arrIsRequiredCols.Contains(cellCntr))
                    //{
                    //    foreach(Control ctrl in tc.Controls)

                    //    ctrl.Attributes.Add("onblur", "alert('Hi');");
                    //}

                    cellCntr++;
                }
            }

            //JQUERY CALENDAR base page unload



        }

        void imgBtnProc_Click(object sender, ImageClickEventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Creates the xml structure of the given row.
        /// </summary>
        /// <param name="dtGridData">The table which contains the row.</param>
        /// <param name="primaryKey">The unique key to identify the row.</param>
        /// <returns>xml string.</returns>
        private string GetRowXml(DataTable dtGridData, string primaryKey)
        {
            #region NLog
            logger.Info("Creating the xml structure of the given row with primary key as : "+primaryKey);
            #endregion

            if (!dtGridData.Columns.Contains(m_primaryKeyFieldName))
            {
                return string.Empty;
            }
            DataRow[] drFoundRows = dtGridData.Select(m_primaryKeyFieldName + "='" + primaryKey + "'");
            if (drFoundRows.Length == 0)
            {
                return string.Empty;
            }
            XmlDocument xDocRow = new XmlDocument();
            XmlNode nodeRow = xDocRow.CreateNode(XmlNodeType.Element, dtGridData.TableName, null);
            for (int i = 0; i < dtGridData.Columns.Count; i++)
            {
                XmlAttribute attCurrentCol = xDocRow.CreateAttribute(dtGridData.Columns[i].ColumnName.Trim());
                attCurrentCol.Value = drFoundRows[0][i].ToString();
                nodeRow.Attributes.Append(attCurrentCol);
            }
            return nodeRow.OuterXml;
        }

        /// <summary>
        /// Creates a new DDL object to be added into the grid view cell.
        /// </summary>
        /// <param name="cellText">Specifies the node in the XML from the Items data is found</param>
        /// <param name="rowType">If rowType is specified get only those rows in the DDL Items node whose TrxType matches the 
        ///     specified rowType, else fill all the items.</param>
        private LAjitDropDownList CreateDropDownList(string cellText, string selectedValue, string rowType, string GridRowIndex)
        {
            #region NLog
            logger.Info("Creating a new DDL object to be added into the grid view cell text as : "+cellText);
            #endregion

            LAjitDropDownList ddlGridItem = new LAjitDropDownList();
            //ddlGridItem.Width = Unit.Pixel(140);
            ddlGridItem.MapXML = cellText;
            ddlGridItem.Enabled = true;
            XmlDataSource xDS = new XmlDataSource();
            xDS.EnableCaching = false;

            if (GridViewUserControl != null && GridViewUserControl.GridViewInitData != null)
            {
                xDS.Data = GridViewUserControl.GridViewInitData;
            }
            else if (ParentChildGVUC != null && ParentChildGVUC.GridViewInitData != null)
            {
                xDS.Data = ParentChildGVUC.GridViewInitData;
            }
            else
            {
                if (ButtonsUserControl != null)
                    xDS.Data = ButtonsUserControl.GVDataXml;
            }

            //Add only the items which match the TrxType/RowType
            string xPathFilter = string.Empty;
            if (rowType.Length != 0)
            {
                xPathFilter = "Root/bpeout/FormControls/" + cellText + "/RowList/Rows[@TrxType='" + rowType + "']";
            }
            else
            {
                xPathFilter = "Root/bpeout/FormControls/" + cellText + "/RowList/Rows";
            }

            xDS.XPath = xPathFilter;
            ddlGridItem.DataValueField = "DataValueField";
            ddlGridItem.DataTextField = cellText;
            ddlGridItem.ID = "ddl" + cellText;
            ddlGridItem.DataSource = xDS;
            if (xDS.Data.Trim().Length > 0)
            {
                ddlGridItem.DataBind();
                ddlGridItem.SelectedValue = selectedValue;
            }

            //Adding onchage event exclusive for the column EntryJob
            if (cellText == "EntryJob")
            {
                if (ddlGridItem != null)
                {
                    ddlGridItem.Attributes.Add("onchange", "javascript:FillCascadingDropDowns('" + GridRowIndex + "');");
                }
            }

            return ddlGridItem;
        }

        /// <summary>
        /// Adds a new row to the grid with current assigned data source's schmema
        /// </summary>
        /// <param name="gridView">The Grid view in which the row is to be added.</param>
        public void AddNewRowToGrid(GridView gridView)
        {
            //To balance serverside and clientside
            //To enable/Change clor/ResetActionButtons/ShowHideFormButtons depending on current action.
            InvokeOnButtonClick("Clone", this.BtnsUC.Page);

            //Enable validation for confirm page exit
            ((HiddenField)gridView.Parent.NamingContainer.FindControl("BtnsUC").FindControl("hdnNeedToConfirmExit")).Value = "True";

            DataSet ds = (DataSet)gridView.DataSource;
            DataTable dtCurrent = ds.Tables[0];
            //Match the gridview's row data to it's data source
            //Data source may be different from that of the gridview's in the case where the user has
            //entered some information and clicked AddMoreRows
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                GridViewRow gvrRow = gridView.Rows[i];
                for (int j = 1; j < gvrRow.Cells.Count; j++)
                {
                    string dataField = ((BoundField)gridView.Columns[j]).DataField;
                    foreach (Control ctrl in gvrRow.Cells[j].Controls)
                    {
                        if (ctrl is LAjitTextBox)
                        {
                            LAjitTextBox txtCurrent = (LAjitTextBox)ctrl;
                            dtCurrent.Rows[i][dataField] = txtCurrent.Text;
                        }
                        else if (ctrl is LAjitDropDownList)
                        {
                            LAjitDropDownList ddlCurrent = (LAjitDropDownList)ctrl;
                            string mapXML = ddlCurrent.MapXML;
                            dtCurrent.Rows[i][mapXML] = ddlCurrent.SelectedValue;
                        }
                        else if (ctrl is LAjitCheckBox)
                        {
                            LAjitCheckBox chkBxCurrent = (LAjitCheckBox)ctrl;
                            string mapXML = chkBxCurrent.MapXML;
                            if (chkBxCurrent.Checked)
                            {
                                dtCurrent.Rows[i][mapXML] = "1";
                            }
                            else
                            {
                                dtCurrent.Rows[i][mapXML] = "0";
                            }
                        }
                    }
                    //dtCurrent.Rows[i].ItemArray[dtCurrent.Columns[dataField].Ordinal] = ;
                }
            }
            dtCurrent.AcceptChanges();

            //int blockSize = Convert.ToInt32(ConfigurationManager.AppSettings["AddBlockSize"]);
            //for (int i = 0; i < blockSize; i++)
            //{
            //    GridViewRow gvrNew = new GridViewRow(gridView.Rows.Count, -1, DataControlRowType.DataRow, DataControlRowState.Normal);
            //    gvrNew.Cells.AddRange(CreateTableCells(gridView));
            //    Table tblChild = gridView.Controls[0] as Table;
            //    tblChild.Rows.Add(gvrNew);
            //}

            int blockSize = Convert.ToInt32(ConfigurationManager.AppSettings["AddBlockSize"]);
            for (int i = 0; i < blockSize; i++)
            {
                DataRow drNewRow = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(drNewRow);
            }
            ds.AcceptChanges();
            gridView.DataSource = ds;
            gridView.DataBind();

            //Set the RowCount variable accordingly
            RowCount = ds.Tables[0].Rows.Count;

            //Get the hidden field with the info about rows to delete
            string branchNodeName = gridView.ClientID.Replace("ctl00_cphPageContents_grdVw", "");
            HiddenField hdnRowsToDelete = (HiddenField)gridView.Parent.NamingContainer.FindControl("hdnRowsToDelete" + branchNodeName);
            string[] arrDeletions = hdnRowsToDelete.Value.Split(',');
            foreach (string strRowIndex in arrDeletions)
            {
                if (strRowIndex.Trim().Length == 0)
                {
                    continue;
                }
                int rowIndex = Convert.ToInt32(strRowIndex.Trim());
                gridView.Rows[rowIndex].Visible = false;
            }

            //If the data source consists of more than specified records implement vertical scrolling.
            int maxRowSize = Convert.ToInt32(ConfigurationManager.AppSettings["BranchGVMaxRows"]);
            if (ds.Tables[0].Rows.Count - (arrDeletions.Length - 1) > maxRowSize)
            {
                //Find the immediate parent container panel of the grid view 
                Panel pnlGVContainer = (Panel)gridView.NamingContainer.FindControl("pnlGVBranch");
                if (!pnlGVContainer.Page.ToString().Contains("commercial"))
                {
                    pnlGVContainer.Style.Remove("height");
                    pnlGVContainer.Height = Unit.Pixel(230);
                    pnlGVContainer.ScrollBars = ScrollBars.Both;
                }
            }
        }

        private TableCell[] CreateTableCells(GridView gridView)
        {
            TableCell[] rowCells = new TableCell[gridView.Columns.Count];
            for (int i = 0; i < gridView.Columns.Count; i++)
            {
                rowCells[i] = new TableCell();
                foreach (Control ctrl in gridView.Rows[0].Cells[i].Controls)
                {
                    if (ctrl is LAjitTextBox)
                    {
                        LAjitTextBox txtReference = (LAjitTextBox)ctrl;
                        LAjitTextBox txtCurrent = new LAjitTextBox();
                        txtCurrent.ID = txtReference.ID;
                        txtCurrent.Width = txtReference.Width;
                        rowCells[i].Controls.Add(txtCurrent);
                    }
                    else if (ctrl is CheckBox)
                    {
                        CheckBox chkBxReference = (CheckBox)ctrl;
                        CheckBox chkBxCurrent = new CheckBox();
                        chkBxCurrent.ID = chkBxReference.ID;
                        rowCells[i].Controls.Add(chkBxCurrent);
                    }
                }
            }
            return rowCells;
        }

        /// <summary>
        /// Delete rows event for the child grid view in the page.
        /// </summary>
        /// <param name="grdVwGroupItem"></param>
        internal void DeleteRowsInGrid(GridView grdVwGroupItem)
        {
            throw new Exception("The method or operation is not implemented.");

        }

        /// <summary>
        /// Builds the XML for the Grid View contents.
        /// </summary>
        /// <param name="pnlContainer">The Grid View Container.</param>
        /// <param name="formXML">The form XML for the current page.</param>
        /// <returns>XML string.</returns>
        public string GetGridViewXML(Panel pnlContainer, string formXML, string parentTrxID, string parentTrxType, string BPAction)
        {
            #region NLog
            logger.Info("Builds the XML for the Grid View contents with parent txx id as : "+parentTrxID+ " and parent trx type as : "+parentTrxType+ " and BP Action : "+BPAction);
            #endregion

            XmlDocument xDocForm = new XmlDocument();
            xDocForm.LoadXml(formXML);
            //Get the root tree name.
            string treeNodeName = xDocForm.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            //Get the branch nodes of the above tree.
            XmlNode nodeBranches = xDocForm.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
            //If there are no branches nothing to do just return.
            if (nodeBranches == null)
            {
                return string.Empty;
            }
            //Trim as required
            parentTrxID = parentTrxID.Trim();
            parentTrxType = parentTrxType.Trim();
            string parentIDString = string.Empty;

            //Build the XML string
            StringBuilder sbGridViewXML = new StringBuilder();
            foreach (XmlNode nodeBranch in nodeBranches.ChildNodes) //Each branch corresponds to a child object
            {
                string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                XmlNode nodeBranchColumns = xDocForm.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                GridView gridView = (GridView)pnlContainer.FindControl("grdVw" + branchNodeName);
                if (gridView == null)
                {
                    continue;
                }
                DataSet dsCurrent = (DataSet)gridView.DataSource;
                sbGridViewXML.Append("<" + branchNodeName + "><RowList>");

                //Get the hidden field with the info about rows to delete
                HiddenField hdnRowsToDelete = (HiddenField)pnlContainer.Parent.Parent.FindControl("hdnRowsToDelete" + branchNodeName);
                string[] arrDeletions = hdnRowsToDelete.Value.Split(',');

                int rowCntr = 0;
                foreach (GridViewRow row in gridView.Rows)
                {
                    //Get the TrxID and TrxType of the current row from the DataSource.
                    string trxID = dsCurrent.Tables[0].Rows[rowCntr].ItemArray[0].ToString().Trim();
                    string trxType = dsCurrent.Tables[0].Rows[rowCntr].ItemArray[1].ToString().Trim();
                    int rVerIndex = GetColumnIndex("RVer", dsCurrent.Tables[0]);
                    parentIDString = treeNodeName + "_TrxID=\"" + parentTrxID + "\" " + treeNodeName + "_TrxType=\"" + parentTrxType + "\" ";
                    string rowVersion = string.Empty;
                    bool isRowDeleted = false;//Indicates that row is meant for deletion.

                    if (rVerIndex != -1)
                    {
                        rowVersion = dsCurrent.Tables[0].Rows[rowCntr].ItemArray[rVerIndex].ToString();
                    }

                    //Check whether current row is in RowsToDelete List
                    foreach (string strRowIndex in arrDeletions)
                    {
                        if (strRowIndex == row.RowIndex.ToString())
                        {
                            isRowDeleted = true;
                            break;
                        }
                    }

                    //Set the BPAction according to the operation being performed
                    string actionLocal = BPAction;
                    if (trxID.Length == 0 && trxType.Length == 0)
                    {
                        if (isRowDeleted)
                        {
                            continue;//User has entered a new row but deleted it without submitting it to the server.
                        }

                        if (!m_IsFindMode)
                        {
                            actionLocal = "Add";
                        }

                        if (parentTrxID.Length != 0)
                        {
                            //Parent already exists and a new child is being added.
                            sbGridViewXML.Append("<Rows " + treeNodeName + "_TrxID=\"" + parentTrxID + "\" " + treeNodeName + "_TrxType=\"" + parentTrxType + "\"  ");
                        }
                        else
                        {
                            //Both parent and child are being added
                            sbGridViewXML.Append("<Rows ");
                        }
                    }
                    else
                    {
                        sbGridViewXML.Append("<Rows " + treeNodeName + "_TrxID=\"" + parentTrxID + "\" " + treeNodeName + "_TrxType=\"" + parentTrxType + "\"  TrxID=\"" + trxID + "\" TrxType=\"" + trxType + "\" RVer=\"" + rowVersion + "\" ");
                    }

                    if (isRowDeleted)
                    {
                        actionLocal = "Delete";
                    }

                    bool isRowOk = false;//Describes that the row contains some data rather than being empty.
                    foreach (TableCell cell in row.Cells)
                    {
                        if (cell.Controls.Count == 0)
                        {
                            //Implies the cell is IsDisplayOnly, no modifications made, so
                            continue;
                        }
                        Control currentCellCtrl = cell.Controls[0];
                        if (currentCellCtrl is LAjitTextBox)
                        {
                            LAjitTextBox txtCurrent = ((LAjitTextBox)currentCellCtrl);

                            //if (txtCurrent.Text.Trim().Length == 0)
                            //{
                            //    continue;
                            //}

                            string xPath = "//Columns/Col[@Label='" + (txtCurrent).MapXML.Trim() + "']";
                            string isLink = string.Empty;
                            XmlNode nodebranchcols = nodeBranchColumns.SelectSingleNode(xPath);

                            if (nodebranchcols != null)
                            {
                                if (nodebranchcols.Attributes["IsLink"] != null)
                                {
                                    isLink = nodeBranchColumns.SelectSingleNode(xPath).Attributes["IsLink"].Value.Trim();
                                }
                            }

                            if ((txtCurrent.Text.Trim().Length == 0) && (isLink == "0"))
                            {
                                continue;
                            }

                            //Check for Amount.If it is so remove the formatting such as commas etc.,
                            if (txtCurrent.Attributes["IsAmount"] != null)// && txtCurrent.Attributes["IsAmount"] == "1")
                            {
                                //Check it commas are present.
                                if (txtCurrent.Text.Contains(","))
                                {
                                    txtCurrent.Text = txtCurrent.Text.Replace(",", "");
                                }
                            }

                            //if (isLink == string.Empty)
                            //{
                            //    //sbGridViewXML.Append(txtCurrent.MapXML.Trim() + "=\"" + txtCurrent.Text + "\" ");
                            //    sbGridViewXML.Append(txtCurrent.MapXML.Trim() + "=\"" + CharactersToHtmlCodes(txtCurrent.Text.TrimEnd().TrimStart().ToString()) + "\" ");
                            //    isRowOk = true;
                            //}
                            //else
                            //{
                            if (isLink.Length == 0 || isLink == "0")
                            {
                                if (txtCurrent.Text != string.Empty)
                                {
                                    //sbGridViewXML.Append(txtCurrent.MapXML.Trim() + "=\"" + txtCurrent.Text + "\" ");

                                    //Check Entry To Upper key
                                    //EnterAllUpperCase
                                    string UpperCaseValue = GetPreferenceValue("16");
                                    if (UpperCaseValue == "1")
                                    {
                                        sbGridViewXML.Append(txtCurrent.MapXML.Trim() + "=\"" + CharactersToHtmlCodes(txtCurrent.Text.ToUpper().Trim()) + "\" ");
                                        isRowOk = true;
                                    }
                                    else
                                    {
                                        sbGridViewXML.Append(txtCurrent.MapXML.Trim() + "=\"" + CharactersToHtmlCodes(txtCurrent.Text.Trim()) + "\" ");
                                        isRowOk = true;
                                    }
                                }
                            }
                            else if (isLink == "1")//DDL
                            {
                                //Reading from cache
                                string cacheName = Classes.AutoFill.GetLoggedInCompanyID() + txtCurrent.MapXML.ToString();
                                string cacheTrxID = string.Empty;
                                string cacheTrxType = string.Empty;
                                string cacheCreatedRow = string.Empty;

                                if ((System.Web.HttpContext.Current.Cache[cacheName] != null) && (txtCurrent.Text.Trim() != ""))
                                {
                                    DataSet dsAutoFill = (DataSet)System.Web.HttpContext.Current.Cache[cacheName];
                                    if (dsAutoFill.Tables[txtCurrent.MapXML.ToString()].Rows.Count > 0)
                                    {
                                        //Step 1 look Exact match word
                                        if (dsAutoFill.Tables[txtCurrent.MapXML.ToString()].Rows.Count > 0)
                                        {
                                            DataRow[] drfoundrows;
                                            drfoundrows = dsAutoFill.Tables[0].Select(txtCurrent.MapXML.Trim() + "='" + txtCurrent.Text.Trim().Replace("'", "''") + "'");

                                            if (drfoundrows.Length > 0)
                                            {
                                                cacheTrxID = drfoundrows[0][txtCurrent.MapXML.Trim() + "_TrxID"].ToString();
                                                cacheTrxType = drfoundrows[0][txtCurrent.MapXML.Trim() + "_TrxType"].ToString();
                                            }
                                        }

                                        // Step 2  Exact match not found take like first record
                                        if ((cacheTrxID == string.Empty) && (cacheTrxType == string.Empty))
                                        {
                                            DataRow[] drfoundrows;
                                            drfoundrows = dsAutoFill.Tables[0].Select(txtCurrent.MapXML.Trim() + " like '%" + txtCurrent.Text.Trim().Replace("'", "''") + "%'");

                                            if (drfoundrows.Length > 0)
                                            {
                                                cacheTrxID = drfoundrows[0][txtCurrent.MapXML.Trim() + "_TrxID"].ToString();
                                                cacheTrxType = drfoundrows[0][txtCurrent.MapXML.Trim() + "_TrxType"].ToString();
                                            }
                                        }
                                    }
                                    // Step 3  Genearate row
                                    if ((cacheTrxID != string.Empty) && (cacheTrxType != string.Empty))
                                    {
                                        if (cacheTrxID == "-1")
                                        {
                                            cacheCreatedRow = txtCurrent.MapXML.Trim() + "_TrxID=\"" + string.Empty + "\"  "
                                                    + txtCurrent.MapXML.Trim() + "_TrxType=\"" + string.Empty + "\"  ";
                                            // + txtCurrent.MapXML.Trim() + "=\"" + string.Empty + "\"  ";
                                        }
                                        else
                                        {
                                            cacheCreatedRow = txtCurrent.MapXML.Trim() + "_TrxID=\"" + cacheTrxID + "\"  "
                                                    + txtCurrent.MapXML.Trim() + "_TrxType=\"" + cacheTrxType + "\"  ";
                                            //  + txtCurrent.MapXML.Trim() + "=\"" + txtCurrent.Text + "\"  ";
                                        }
                                    }
                                    else
                                    {
                                        //trxid and trxtype is empty send text only
                                        // cacheCreatedRow = txtCurrent.MapXML.Trim() + "=\"" + txtCurrent.Text + "\"  ";
                                        // No need to create attribute  both trxid and trxtype are empty. we are removing these items at the end. on 04-11-03
                                        // ADD operation sending both trxid and trxtype as empty and MODIFY case trxtype and trxtype are removed.
                                        cacheCreatedRow = txtCurrent.MapXML.Trim() + "_TrxID=\"\" "
                                                    + txtCurrent.MapXML.Trim() + "_TrxType=\"\"  "
                                                    + txtCurrent.MapXML.Trim() + "=\"\"  ";
                                    }

                                    if (cacheCreatedRow != string.Empty)
                                    {
                                        sbGridViewXML.Append(cacheCreatedRow);
                                        isRowOk = true;
                                    }
                                    //Reading from cache
                                }
                                else
                                {
                                    if ((nodeBranchColumns.SelectSingleNode(xPath).Attributes["IsRequired"].Value == "1") && (BPAction != "Find"))
                                    {
                                        //Get Default value 

                                        if ((System.Web.HttpContext.Current.Cache[cacheName] != null))
                                        {
                                            DataSet dsAutoFill = (DataSet)System.Web.HttpContext.Current.Cache[cacheName];

                                            if (dsAutoFill.Tables[txtCurrent.MapXML.ToString()].Rows.Count > 0)
                                            {
                                                DataRow[] drfoundrows;
                                                drfoundrows = dsAutoFill.Tables[0].Select(txtCurrent.MapXML.Trim() + "_TrxID " + "<>'-1'");

                                                if (drfoundrows.Length > 0)
                                                {
                                                    cacheTrxID = drfoundrows[0][txtCurrent.MapXML.Trim() + "_TrxID"].ToString();
                                                    cacheTrxType = drfoundrows[0][txtCurrent.MapXML.Trim() + "_TrxType"].ToString();
                                                    //txtCurrent.Text = drfoundrows[0][txtCurrent.MapXML.Trim()].ToString();
                                                }
                                            }
                                        }
                                        //Row Create default
                                        if ((cacheTrxID != string.Empty) && (cacheTrxType != string.Empty))
                                        {
                                            if (cacheTrxID == "-1")
                                            {
                                                cacheCreatedRow = txtCurrent.MapXML.Trim() + "_TrxID=\"" + string.Empty + "\"  "
                                                        + txtCurrent.MapXML.Trim() + "_TrxType=\"" + string.Empty + "\"  ";
                                                // + txtCurrent.MapXML.Trim() + "=\"" + string.Empty + "\"  ";
                                            }
                                            else
                                            {
                                                cacheCreatedRow = txtCurrent.MapXML.Trim() + "_TrxID=\"" + cacheTrxID + "\"  "
                                                        + txtCurrent.MapXML.Trim() + "_TrxType=\"" + cacheTrxType + "\"  ";
                                                //  + txtCurrent.MapXML.Trim() + "=\"" + txtCurrent.Text + "\"  ";
                                            }
                                        }
                                        //Row Create
                                        if (cacheCreatedRow != string.Empty)
                                        {
                                            sbGridViewXML.Append(cacheCreatedRow);
                                            //isRowOk = false;
                                        }
                                    }
                                }
                            }
                        }
                        else if (currentCellCtrl is LAjitDropDownList)
                        {
                            LAjitDropDownList ddlCurrent = (LAjitDropDownList)currentCellCtrl;
                            string newTrxIDType = ddlCurrent.SelectedValue.Trim();
                            string ddlRow = string.Empty;
                            if (newTrxIDType.Length == 0)
                            {
                                continue;
                            }
                            string[] strarr = newTrxIDType.Split('~');
                            string ddlTrxID = strarr[0].ToString();
                            string ddlTrxType = strarr[1].ToString();
                            //if (actionLocal.ToUpper() != "MODIFY")
                            {
                                if (ddlTrxID != string.Empty && ddlTrxType != string.Empty)
                                {
                                    //TrxID should not -1
                                    if (ddlTrxID != "-1")
                                    {
                                        ddlRow = ddlCurrent.MapXML.Trim() + "_TrxID=\"" + ddlTrxID + "\"  "
                                                + ddlCurrent.MapXML.Trim() + "_TrxType=\"" + ddlTrxType + "\"  ";
                                        //+ ddlCurrent.MapXML.Trim() + "=\"" + ddlCurrent.SelectedItem + "\"  ";
                                    }
                                }
                                else
                                {
                                    ddlRow = ddlCurrent.MapXML.Trim() + "=\"" + CharactersToHtmlCodes(ddlCurrent.SelectedItem.Text.TrimEnd().TrimStart().ToString()) + "\" ";
                                }
                            }
                            if (ddlRow != string.Empty)
                            {
                                sbGridViewXML.Append(ddlRow);
                                isRowOk = true;
                            }
                        }
                        else if (currentCellCtrl is LAjitCheckBox)
                        {
                            LAjitCheckBox chkCurrent = (LAjitCheckBox)currentCellCtrl;
                            if (chkCurrent.Checked)
                            {
                                //True
                                sbGridViewXML.Append(chkCurrent.MapXML.Trim() + "=\"1\" ");
                                isRowOk = true;
                            }
                            else
                            {   //False
                                sbGridViewXML.Append(chkCurrent.MapXML.Trim() + "=\"0\" ");
                            }
                        }
                        //else if (currentCellCtrl is LAjitLabel)
                        //{
                        //    LAjitLabel lblCurrent = (LAjitLabel)currentCellCtrl;
                        //    if (lblCurrent.Text.Length == 0)
                        //    {
                        //        continue;
                        //    }
                        //    sbGridViewXML.Append(lblCurrent.MapXML.Trim() + "=\"" + CharactersToHtmlCodes(lblCurrent.Text.Trim()) + "\" ");
                        //    isRowOk = true;
                        //}
                    }
                    if (!isRowOk)
                    {
                        int strLength = sbGridViewXML.Length;
                        int startIndex = sbGridViewXML.ToString().LastIndexOf("<");
                        sbGridViewXML.Remove(startIndex, strLength - startIndex);
                        rowCntr++;
                        continue;
                    }
                    sbGridViewXML.Append("BPAction=\"" + actionLocal + "\" />");
                    rowCntr++;
                }
                //string strAddRowsXML = GetAddedRowsXML(parentIDString, branchNodeName, gridView.Rows[0]);
                sbGridViewXML.Append("</RowList></" + branchNodeName + ">");
                //dsCurrent.AcceptChanges();
                //gridView.DataSource = dsCurrent;
                //gridView.DataBind();
            }

            return sbGridViewXML.ToString();
        }

        private string GetAddedRowsXML(string parentIDString, string branchNodeName, GridViewRow gridViewRow)
        {
            #region NLog
            logger.Info("Builds the XML for the Added Rows XML with parent id as : "+parentIDString+" and brance node name as : "+branchNodeName);
            #endregion

            //Get the hidden field with the info about rows to delete
            HiddenField hdnRowsToAdd = (HiddenField)gridViewRow.Parent.NamingContainer.NamingContainer.FindControl("hdnAddedRows" + branchNodeName);
            string[] arrDelimiters = { "||" };
            string[] arrRowAdditions = hdnRowsToAdd.Value.Split(arrDelimiters, StringSplitOptions.RemoveEmptyEntries);
            //Infer the schema
            ArrayList arrSchema = new ArrayList();
            foreach (TableCell tc in gridViewRow.Cells)
            {
                int x = tc.Controls.Count;
                Control currentCellCtrl = tc.Controls[0];
                if (currentCellCtrl is LAjitTextBox)
                {
                    LAjitTextBox txtCurrent = ((LAjitTextBox)currentCellCtrl);
                    arrSchema.Add(txtCurrent.MapXML);
                }
                else if (currentCellCtrl is LAjitDropDownList)
                {
                    LAjitDropDownList ddlCurrent = (LAjitDropDownList)currentCellCtrl;
                    arrSchema.Add(ddlCurrent.MapXML);
                }
                else if (currentCellCtrl is LAjitCheckBox)
                {
                    LAjitCheckBox chkCurrent = (LAjitCheckBox)currentCellCtrl;
                    arrSchema.Add(chkCurrent.MapXML);
                }
            }
            StringBuilder sbAddedRows = new StringBuilder();
            foreach (string newRow in arrRowAdditions)
            {
                sbAddedRows.Append("<Rows " + parentIDString);
                string[] rowDelimiter = { "~::~" };
                string[] cellContents = newRow.Split(rowDelimiter, StringSplitOptions.RemoveEmptyEntries);
                foreach (string cellItem in cellContents)
                {
                    string[] cellDelimiter = { "*#*" };
                    string[] cellValues = cellItem.Split(cellDelimiter, StringSplitOptions.RemoveEmptyEntries);
                    string currentCellXML = arrSchema[Convert.ToInt32(cellValues[0])].ToString() + "=\"" + cellValues[1] + "\" ";
                    sbAddedRows.Append(currentCellXML);
                }
                sbAddedRows.Append("BPAction=\"Add\"/>");
            }

            return sbAddedRows.ToString();
        }

        /// <summary>
        /// Updates the local copy of branch grid view's XML
        /// </summary>
        /// <param name="strOutXml">The new XML from the DB just after an operation has been performed.</param>
        /// <param name="treeNodeName">TreeNode in the current form XML.</param>
        /// <param name="arrBranches">Array of all the branches.</param>
        private void UpdateGVBranchViewStateXML(string strOutXml, string treeNodeName, ArrayList arrBranches, Panel pnlContainer)
        {
            #region NLog
            logger.Info("Updates the local copy of branch grid view's XML.");
            #endregion

            string GVDataXML = ButtonsUserControl.GVDataXml;
            //if (GVDataXML.Length == 0)
            //{
            //    GVDataXML = ButtonsUserControl.GVDataXml;
            //}
            XmlDocument xDocLocalCopy = new XmlDocument();
            xDocLocalCopy.LoadXml(GVDataXML);
            XmlDocument xDocReturn = new XmlDocument();
            xDocReturn.LoadXml(strOutXml);
            foreach (string branchNode in arrBranches)
            {
                //updating default columns in branchxml newly added on 26-08-08

                XmlNode nodeColumnsList = xDocReturn.SelectSingleNode("Root/bpeout/FormControls/" + branchNode + "/GridHeading/Columns");

                if (nodeColumnsList != null)
                {
                    //Method 1 Replace the columns default value

                    XmlNodeList nodeDefaultlist = nodeColumnsList.SelectNodes("Col[@Default]");

                    if (nodeDefaultlist != null)
                    {
                        foreach (XmlNode nodeDefault in nodeDefaultlist)
                        {
                            if (nodeDefault != null)
                            {
                                XmlNode nodeColumn = xDocLocalCopy.SelectSingleNode("//" + branchNode + "/GridHeading/Columns//Col[@Label='" + nodeDefault.Attributes["Label"].Value + "']");
                                XmlNode nodeColParent = xDocLocalCopy.SelectSingleNode("//" + branchNode + "/GridHeading/Columns");
                                if (nodeColumn != null)
                                {
                                    nodeColParent.RemoveChild(nodeColumn);
                                    nodeColParent.InnerXml += nodeDefault.OuterXml;
                                }
                            }
                        }
                    }

                    /*  XmlNode nodeDefault = nodeColumnsList.SelectSingleNode("Col[@Default]");
                      if (nodeDefault != null)
                      {
                          XmlNode nodeColumn = xDocLocalCopy.SelectSingleNode("//" + branchNode + "/GridHeading/Columns//Col[@Label='" + nodeDefault.Attributes["Label"].Value + "']");
                          XmlNode nodeColParent = xDocLocalCopy.SelectSingleNode("//" + branchNode + "/GridHeading/Columns");
                          if (nodeColumn != null)
                          {
                              nodeColParent.RemoveChild(nodeColumn);
                              nodeColParent.InnerXml += nodeDefault.OuterXml;
                          }
                      }*/



                    //Method 2 Replace the columns default value
                    //foreach (XmlNode nodecol in nodeColumnsList.ChildNodes)
                    //{
                    //    if (nodecol.Attributes["Default"] != null)
                    //    {
                    //        XmlNode nodeColumn = xDocLocalCopy.SelectSingleNode("//" + branchNode + "/GridHeading/Columns//Col[@Label='" + nodecol.Attributes["Label"].Value + "']");
                    //        nodeColumn.InnerXml += nodecol.InnerXml;
                    //    }
                    //}

                    //xDocLocalCopy.SelectSingleNode("//" + branchNode + "/GridHeading/Columns/Col")
                }

                XmlNode nodeRowList = xDocReturn.SelectSingleNode("Root/bpeout/FormControls/" + branchNode + "/RowList");

                if (nodeRowList != null)
                {
                    //Loop to update added/modified rows.
                    foreach (XmlNode nodeRow in nodeRowList.ChildNodes)
                    {
                        //Get the row's ID
                        string returnRowTrxID = nodeRow.Attributes["TrxID"].Value;
                        string returnRowTrxType = nodeRow.Attributes["TrxType"].Value;

                        //Get the corresponding row in the local copy XML
                        XmlNode nodeLocal = xDocLocalCopy.SelectSingleNode("//" + branchNode + "/RowList/Rows[@TrxID='" + returnRowTrxID + "' and @TrxType='" + returnRowTrxType + "']");
                        if (nodeLocal == null)
                        {
                            //No node exists implies it has been added so add it to the local copy's RowList
                            XmlNode nodeChildRowList = xDocLocalCopy.SelectSingleNode("//" + branchNode + "/RowList");
                            if (nodeChildRowList == null)
                            {
                                //Brand new addition of both the parent and the child, so create the rowlist node before adding rows.
                                nodeChildRowList = xDocLocalCopy.CreateElement("RowList");
                                nodeChildRowList.InnerXml += nodeRow.OuterXml;
                                xDocLocalCopy.SelectSingleNode("//" + branchNode).AppendChild(nodeChildRowList);
                            }
                            else
                            {
                                nodeChildRowList.InnerXml += nodeRow.OuterXml;
                            }
                        }
                        else
                        {
                            //Replace the node
                            xDocLocalCopy.SelectSingleNode("//" + branchNode + "/RowList").RemoveChild(nodeLocal);
                            xDocLocalCopy.SelectSingleNode("//" + branchNode + "/RowList").InnerXml += nodeRow.OuterXml;
                        }
                    }
                }


                //A case of delete.
                GridView gridView = (GridView)pnlContainer.FindControl("grdVw" + branchNode);
                if (gridView == null)
                {
                    continue;
                }
                DataSet dsCurrent = (DataSet)gridView.DataSource;

                //Get the hidden field with the info about rows to delete
                HiddenField hdnRowsToDelete = (HiddenField)pnlContainer.Parent.Parent.FindControl("hdnRowsToDelete" + branchNode);
                string[] arrDeletions = hdnRowsToDelete.Value.Split(',');
                //Delete the rows in the gridview/ViewState as indicated in the above hidden field.
                int deleteCntr = 0;
                foreach (string strRowIndex in arrDeletions)
                {
                    if (strRowIndex.Length > 0)
                    {
                        DataRow drToDelete = dsCurrent.Tables[0].Rows[Convert.ToInt32(strRowIndex) - deleteCntr];
                        string trxIDToDelete = string.Empty;
                        string trxTypeToDelete = string.Empty;
                        try
                        {
                            trxIDToDelete = drToDelete["TrxID"].ToString();
                            trxTypeToDelete = drToDelete["TrxType"].ToString();
                        }
                        catch (ArgumentException)
                        {
                            //Current row is present at the client-side only(not in DB)
                            continue;
                        }
                        if (trxIDToDelete.Length == 0 && trxTypeToDelete.Length == 0)
                        {
                            //No row to delete or empty row case
                            continue;
                        }
                        //Delete the child rows in the GVDataXML as per the TrxID and TrxType
                        XmlNode nodeBranchRow = xDocLocalCopy.SelectSingleNode("Root/bpeout/FormControls/" + branchNode + "/RowList/Rows[@TrxID='" + trxIDToDelete + "' and @TrxType='" + trxTypeToDelete + "']");
                        XmlNode nodeParent = xDocLocalCopy.SelectSingleNode("Root/bpeout/FormControls/" + branchNode + "/RowList");
                        nodeParent.RemoveChild(nodeBranchRow);
                        //Also update the datasource for the child grid view.
                        dsCurrent.Tables[0].Rows.Remove(drToDelete);
                        deleteCntr++;

                        //Also update the checkbox selection attribute of the gridview.
                        gridView.Attributes["CheckBoxSel"] = gridView.Attributes["CheckBoxSel"].Replace(strRowIndex + ",", "");
                        //Clear the checkboxes in the grid
                        ClearGridViewSelection(gridView);
                    }
                }
                //Clear the Hidden Field once the deletions are performed.
                hdnRowsToDelete.Value = string.Empty;
            }
            ButtonsUserControl.GVDataXml = xDocLocalCopy.OuterXml;
            GridViewUserControl.FormTempData = xDocLocalCopy.OuterXml;

            //Re-Bind the branch objects once the changes have been persisted to DB
            if (GVUC != null)
            {
                InitialiseBranchObjects(xDocLocalCopy, GVUC.Parent);
            }
            else if (PCGVUC != null)
            {
                InitialiseBranchObjects(xDocLocalCopy, PCGVUC.Parent);
            }
        }

        /// <summary>
        /// Clears the checkbox selections in the grid view.
        /// </summary>
        /// <param name="gridView"></param>
        private static void ClearGridViewSelection(GridView gridView)
        {
            foreach (GridViewRow row in gridView.Rows)
            {
                //foreach (TableCell cell in row.Cells)
                //{
                //    if (cell.Controls.Count == 0)
                //    {
                //        continue;
                //    }
                //    Control currentCellCtrl = cell.Controls[0];
                //    if (currentCellCtrl is LiteralControl)
                //    {
                //        foreach (Control cellChildCtrl in cell.Controls)
                //        {
                //            if (cellChildCtrl is CheckBox)
                //            {
                //                if (((CheckBox)cellChildCtrl).Checked)
                //                {
                //                    ((CheckBox)cellChildCtrl).Checked = false;
                //                }
                //            }
                //        }
                //    }
                //}
                CheckBox chkBxSelectRow = (CheckBox)row.Cells[0].FindControl("chkBxSelectRow");
                if (chkBxSelectRow.Checked)
                {
                    chkBxSelectRow.Checked = false;
                }
            }
        }
        #endregion

        #region ListBox Methods

        /// <summary>
        /// Binds the ListBox Branch Objects in the Page.
        /// </summary>
        /// <param name="xDocOut"></param>
        /// <param name="pnlEntryForm"></param>
        /// <param name="treeNodeName"></param>
        /// <param name="branchNodeName"></param>
        /// <param name="nodeBranchColumns"></param>
        private void BindListBox(XmlDocument xDocOut, Control pnlEntryForm, string treeNodeName, string branchNodeName, XmlNode nodeBranchColumns)
        {
            bool isOkToExec = false;
            Control ctrlPostBack = CommonUI.GetPostBackControl(pnlEntryForm.Page);
            if (ctrlPostBack != null && ctrlPostBack.ID.Contains("imgBtnTTnNavigate"))//Grid Row Selected Change Event
            {
                //Execute
                isOkToExec = true;
            }
            else if (ctrlPostBack == null)//Page Load
            {
                //Execute
                isOkToExec = true;
            }
            if (isOkToExec)
            {
                foreach (XmlNode nodeBrCol in nodeBranchColumns.ChildNodes)
                {
                    if (nodeBrCol.Attributes["IsParentLink"] == null ||
                        nodeBrCol.Attributes["IsParentLink"].Value == "0")
                    //If the above condition is satisfies the control is either a ListBox or a GridView
                    {
                        //try to find grdVw" + listItemsNode if null go for listbox.
                        string listItemsNode = nodeBrCol.Attributes["Label"].Value.Trim();
                        LAjitListBox lstBxCurrent = (LAjitListBox)pnlEntryForm.FindControl("lstBx" + listItemsNode);

                        if (lstBxCurrent == null)
                        {
                            return;
                        }

                        lstBxCurrent.Attributes.Add("onchange", "FillListValues(this);");
                        lstBxCurrent.Attributes.Add("onMouseDown", "GetCurrentListValues(this);");


                        if (lstBxCurrent.Items.Count == 0)
                        {
                            //Add the List Box items if previously not added.
                            XmlNode nodeBranchData = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + listItemsNode + "/RowList");
                            if (nodeBranchData != null)
                            {
                                foreach (XmlNode nodeRow in nodeBranchData.ChildNodes)
                                {
                                    lstBxCurrent.Items.Add(new ListItem(nodeRow.Attributes[listItemsNode].Value, nodeRow.Attributes["DataValueField"].Value));
                                }
                            }
                        }
                        //Left side label column name

                        //Initialising the corresponding label for the current column.
                        string control = "lbl" + nodeBrCol.Attributes["Label"].Value.ToString();
                        Label lblCurrent = (Label)pnlEntryForm.FindControl(control);
                        if (lblCurrent != null)
                        {
                            lblCurrent.Text = nodeBrCol.Attributes["Caption"].Value;
                        }

                        //Pick up nodes only which are relevant
                        string parentTrxID = string.Empty;
                        string parentTrxType = string.Empty;
                        if (GVSelectedRow != null)
                        {
                            parentTrxID = GVSelectedRow.Attributes["TrxID"].Value;
                            parentTrxType = GVSelectedRow.Attributes["TrxType"].Value;
                        }
                        //Display pre-selected rows in the as indicated in the XML.
                        //string xPath = "Root/bpeout/FormControls/" + branchNodeName + "/RowList/Rows[@" + treeNodeName + "_TrxID='" + parentTrxID + "']|/RowList/Rows[@" + treeNodeName + "_TrxType='" + parentTrxType + "']";
                        string xPath = "Root/bpeout/FormControls/" + branchNodeName + "/RowList/Rows[@" + treeNodeName + "_TrxID='" + parentTrxID + "' and @" + treeNodeName + "_TrxType='" + parentTrxType + "']";
                        XmlNodeList nodeBranchRows = xDocOut.SelectNodes(xPath);
                        lstBxCurrent.ClearSelection();
                        if (nodeBranchRows != null)
                        {
                            foreach (XmlNode nodeRow in nodeBranchRows)
                            {
                                string selectedValue = nodeRow.Attributes[listItemsNode + "_TrxID"].Value + "~" +
                                    nodeRow.Attributes[listItemsNode + "_TrxType"].Value;
                                int selectedIndex = GetListItemIndex(selectedValue, lstBxCurrent);
                                if (selectedIndex == -1)
                                {
                                    continue;
                                }
                                lstBxCurrent.Items[selectedIndex].Selected = true;
                                //lstBxCurrent.Items[selectedIndex].Attributes.Add("RowInfo", nodeRow.OuterXml);
                            }
                        }
                        int[] test = lstBxCurrent.GetSelectedIndices();
                        if (test.Length > 0)
                        {
                            string selectedIndices = string.Empty;
                            foreach (int index in test)
                            {
                                selectedIndices += index.ToString() + ",";
                            }
                            selectedIndices = selectedIndices.Remove(selectedIndices.Length - 1, 1);
                            lstBxCurrent.Attributes["OriginalSelection"] = selectedIndices;
                        }
                        else
                        {
                            //No initial selections are there so set the attribute to null
                            lstBxCurrent.Attributes["OriginalSelection"] = null;
                        }

                        if (lstBxCurrent.Items.Count > 0)
                        {
                            //Required field validator added on 08-07-08 

                            LAjitRequiredFieldValidator reqCntrlId = (LAjitRequiredFieldValidator)pnlEntryForm.FindControl("req" + listItemsNode + "_" + branchNodeName);
                            if (reqCntrlId != null)
                            {
                                reqCntrlId.InitialValue = lstBxCurrent.Items[0].Value;
                                string isRequired = nodeBrCol.Attributes["IsRequired"].Value.Trim();
                                if (isRequired != "0")
                                {
                                    reqCntrlId.Enabled = true;
                                }
                            }
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Gets the position of the selected value.
        /// </summary>
        /// <param name="selectedValue">Selected Value present in the ListBox.</param>
        /// <param name="listBoxObj">ListBox to search in.</param>
        /// <returns>Integer position, -1 if not found.</returns>
        private int GetListItemIndex(string selectedValue, LAjitListBox listBoxObj)
        {
            int position = 0;
            foreach (ListItem li in listBoxObj.Items)
            {
                if (li.Value == selectedValue)
                {
                    return position;
                }
                position++;
            }
            return -1;
        }

        /// <summary>
        /// Builds an XML string for the selection in the listBox object.
        /// </summary>
        /// <param name="orginalXML">The current page return xml.</param>
        /// <returns>XML string.</returns>
        private string GetListBoxSelectionXML(string orginalXML, Panel container, string operation
            , string trxID, string trxType, string rowVersion, out string treeNodeName, out ArrayList arrBranches)
        {
            if (orginalXML == null || orginalXML == string.Empty)
            {
                orginalXML = ButtonsUserControl.GVDataXml;
            }
            XmlDocument xDocOut = new XmlDocument();
            xDocOut.LoadXml(orginalXML);
            treeNodeName = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
            XmlNode nodeBranches = xDocOut.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
            arrBranches = new ArrayList();
            if (nodeBranches == null)
            {
                return string.Empty;
            }
            StringBuilder sbEntireListBoxXML = new StringBuilder();
            foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
            {
                string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                arrBranches.Add(branchNodeName);
                XmlNode nodeBranchColumns = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");

                //Fill the list box with children.
                foreach (XmlNode nodeBrCol in nodeBranchColumns.ChildNodes)
                {
                    if (nodeBrCol.Attributes["IsParentLink"] == null ||
                        nodeBrCol.Attributes["IsParentLink"].Value == "0")
                    //If the above condition is satisfies the control is either a ListBox or a GridView
                    {
                        string listItemsNode = nodeBrCol.Attributes["Label"].Value.Trim();
                        LAjitListBox lstBxCurrent = (LAjitListBox)container.FindControl("lstBx" + listItemsNode);
                        if (lstBxCurrent == null)
                        {
                            //No listbox found
                            continue;
                        }
                        //Get the  original selection as indicated in the XML.
                        XmlNode nodeBranchRows = xDocOut.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/RowList");
                        StringBuilder sbListBoxXML = new StringBuilder();
                        if (operation.Trim().ToUpper() == "ADD" || operation.Trim().ToUpper() == "FIND")
                        {
                            //Add the entire selection of the list box.
                            foreach (ListItem liRow in lstBxCurrent.Items)
                            {
                                //Ignore the first item(selection required)
                                if (liRow.Value.Contains("-1"))
                                {
                                    continue;
                                }
                                if (liRow.Selected)
                                {
                                    string[] trxIDnType = liRow.Value.Split('~');
                                    sbListBoxXML.Append("<Rows " + listItemsNode + "_TrxID=\"" + trxIDnType[0] + "\" " + listItemsNode + "_TrxType=\"" + trxIDnType[1] + "\" BPAction=\"" + operation + "\"></Rows>");
                                }
                            }
                        }
                        else if (operation.Trim().ToUpper() == "MODIFY")
                        {
                            ArrayList arrSelections = new ArrayList();
                            if (lstBxCurrent.Attributes["OriginalSelection"] != null)
                            {
                                string[] arrSplitSelections = lstBxCurrent.Attributes["OriginalSelection"].Split(',');
                                foreach (string index in arrSplitSelections)
                                {
                                    arrSelections.Add(index);
                                }
                            }
                            int itemIndexCntr = 0;
                            foreach (ListItem liRow in lstBxCurrent.Items)
                            {
                                //Ignore the first item(selection required)
                                if (liRow.Value.Contains("-1"))
                                {
                                    itemIndexCntr++;
                                    continue;
                                }
                                if (liRow.Selected && !arrSelections.Contains(itemIndexCntr.ToString()))
                                {
                                    //Add
                                    string[] trxIDnType = liRow.Value.Split('~');
                                    //TrxID=\"" + trxID + "\" TrxType=\"" + trxType + "\" RVer=\"" + rowVersion + "\" 
                                    sbListBoxXML.Append("<Rows " + listItemsNode + "_TrxID=\"" + trxIDnType[0]
                                        + "\" " + listItemsNode + "_TrxType=\"" + trxIDnType[1]
                                        + "\" BPAction=\"Add\"></Rows>");
                                }
                                else if (arrSelections.Contains(itemIndexCntr.ToString()) && !liRow.Selected)
                                {
                                    //Delete
                                    string[] trxIDnType = liRow.Value.Split('~');
                                    //string xPath = "//Rows[@" + listItemsNode + "_TrxID='" + trxIDnType[0] + "']";
                                    //XmlNode testrowToDelete = nodeBranchRows.SelectSingleNode(xPath);
                                    XmlNodeList nodeListFilter1 = nodeBranchRows.SelectNodes("Rows[@" + treeNodeName + "_TrxID='" + trxID + "']");
                                    XmlNodeList nodeListFilter2 = nodeBranchRows.SelectNodes("Rows[@" + treeNodeName + "_TrxType='" + trxType + "']");

                                    foreach (XmlNode nodeToDelete in nodeListFilter1)
                                    {
                                        if (nodeToDelete.Attributes[listItemsNode + "_TrxID"].Value == trxIDnType[0] &&
                                            nodeToDelete.Attributes[listItemsNode + "_TrxType"].Value == trxIDnType[1])
                                        {
                                            XmlAttribute attBPAction = xDocOut.CreateAttribute("BPAction");
                                            attBPAction.Value = "Delete";
                                            nodeToDelete.Attributes.Append(attBPAction);
                                            sbListBoxXML.Append(nodeToDelete.OuterXml);
                                            break;
                                        }
                                    }
                                    //XmlNode rowToDelete = nodeBranchRows.SelectSingleNode("/Rows[@" + listItemsNode + "_TrxID='" + trxIDnType[0] + "']");//|RowList/Rows[@" + listItemsNode + "_TrxType='" + trxIDnType[1] + "']");
                                    //XmlAttribute attBPAction = xDocOut.CreateAttribute("BPAction");
                                    //attBPAction.Value = "Delete";
                                    //rowToDelete.Attributes.Append(attBPAction);
                                    //sbListBoxXML.Append(rowToDelete.OuterXml);
                                    //sbListBoxXML.Append("<Rows TrxID=\"" + trxID + "\" TrxType=\"" + trxType + "\" RVer=\"" + rowVersion + "\" "
                                    //    + listItemsNode + "_TrxID=\"" + trxIDnType[0]
                                    //    + "\" " + listItemsNode + "_TrxType=\"" + trxIDnType[1]
                                    //    + "\" BPAction=\"Delete\"></Rows>");
                                }
                                itemIndexCntr++;
                            }
                        }
                        XmlNode nodeBranchNew = xDocOut.CreateElement(branchNodeName);
                        XmlNode nodeBranchRowList = xDocOut.CreateElement("RowList");
                        nodeBranchRowList.InnerXml = sbListBoxXML.ToString();
                        nodeBranchNew.AppendChild(nodeBranchRowList);
                        sbEntireListBoxXML.Append(nodeBranchNew.OuterXml);
                    }
                }
            }
            return sbEntireListBoxXML.ToString();
        }

        /// <summary>
        /// Gets the BPGID of the Businees Process Control.
        /// </summary>
        /// <param name="processName">The name of the process.</param>
        /// <param name="bpcXml">The form XML</param>
        /// <returns>string BPGID</returns>
        private string GetBPCBPGID(string processName, XmlDocument xDocForm)
        {
            XmlNode nodeBPC = xDocForm.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess[@ID='" + processName + "']");
            if (nodeBPC != null)
            {
                return nodeBPC.Attributes["BPGID"].Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Updates the Branch Object's data with the latest version.
        /// </summary>
        /// <param name="strOutXml">The return XML resulting from an operation(Modify).</param>
        private void UpdateBranchViewStateXML(string strOutXml, string treeNodeName, ArrayList arrBranches)
        {
            string GVDataXML = ButtonsUserControl.GVDataXml;
            // string GVDataXML = GridViewUserControl.FormTempData;
            if (GridViewUserControl.FormTempData.Length == 0)
            {
                GVDataXML = ButtonsUserControl.GVDataXml;
            }
            XmlDocument xDocLocalCopy = new XmlDocument();
            xDocLocalCopy.LoadXml(GVDataXML);
            XmlDocument xDocReturn = new XmlDocument();
            xDocReturn.LoadXml(strOutXml);

            foreach (string branchNode in arrBranches)
            {
                //updating default columns in branchxml newly added on 26-08-08

                XmlNode nodeColumnsList = xDocReturn.SelectSingleNode("Root/bpeout/FormControls/" + branchNode + "/GridHeading/Columns");

                if (nodeColumnsList != null)
                {
                    //Method 1 Replace the columns default value

                    XmlNode nodeDefault = nodeColumnsList.SelectSingleNode("//Col[@Default]");
                    if (nodeDefault != null)
                    {
                        XmlNode nodeColumn = xDocLocalCopy.SelectSingleNode("//" + branchNode + "/GridHeading/Columns//Col[@Label='" + nodeDefault.Attributes["Label"].Value + "']");
                        XmlNode nodeColParent = xDocLocalCopy.SelectSingleNode("//" + branchNode + "/GridHeading/Columns");
                        if (nodeColumn != null)
                        {
                            nodeColParent.RemoveChild(nodeColumn);
                            nodeColParent.InnerXml += nodeDefault.OuterXml;
                        }
                    }
                }
                //Get the Parent TrxID and TrxType values from the return XML
                XmlNode nodeParent = xDocReturn.SelectSingleNode("//" + treeNodeName + "/RowList/Rows[1]");
                if (nodeParent == null)
                {
                    return;
                }
                string parentTrxID = nodeParent.Attributes["TrxID"].Value;
                string parentTrxType = nodeParent.Attributes["TrxType"].Value;

                //Delete all the child nodes for the current parent.
                XmlNodeList nodeListToModify = xDocLocalCopy.SelectNodes("//" + branchNode + "/RowList/Rows[@" + treeNodeName + "_TrxID = '" + parentTrxID + "']");
                foreach (XmlNode nodeToDelete in nodeListToModify)
                {
                    xDocLocalCopy.SelectSingleNode("//" + branchNode + "/RowList").RemoveChild(nodeToDelete);
                }

                //Update the children with the version from the return XML.
                XmlNode nodeLocalChild = xDocLocalCopy.SelectSingleNode("//" + branchNode + "/RowList");
                XmlNode nodeReturnChild = xDocReturn.SelectSingleNode("//" + branchNode + "/RowList");
                //Update the local copy only nodeReturnChild is not null.This implies there were no selections to add.
                if (nodeReturnChild != null)
                {
                    if (nodeLocalChild == null)
                    {
                        //Case where there were no selections(rows) in the child
                        nodeLocalChild = xDocLocalCopy.SelectSingleNode("//" + branchNode);
                        nodeLocalChild.InnerXml += "<RowList>" + nodeReturnChild.InnerXml + "</RowList>";
                    }
                    else
                    {
                        nodeLocalChild.InnerXml += nodeReturnChild.InnerXml;
                    }
                }
            }
            ButtonsUserControl.GVDataXml = xDocLocalCopy.OuterXml;
            GridViewUserControl.FormTempData = xDocLocalCopy.OuterXml;
        }
        #endregion

        /// <summary>
        /// Gets the table consisting of form level business process links.
        /// </summary>
        /// <param name="pageXML">The rendering xml for the form(BtnsUC.GVDataXML would do)</param>
        /// <returns>Asp Table consisting of the links.</returns>
        public Table GetBusinessProcessLinksTable(string pageXML)
        {
            #region NLog
            logger.Info("Getting the table consisting of form level business process links.");
            #endregion

            try
            {
                //Check for pageXML to be null.
                if (pageXML == null)
                {
                    return new Table();
                }
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(pageXML);
                XmlNode nodeBPC = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
                if (nodeBPC == null)
                {
                    //No links to add so return an empty table.
                    return new Table();
                }

                //Store all the processes assigned to columns and the Buttons User Control
                ArrayList arrColumnProcs = new ArrayList();
                arrColumnProcs.Add("Add");
                arrColumnProcs.Add("Modify");
                arrColumnProcs.Add("Delete");
                arrColumnProcs.Add("Find");
                arrColumnProcs.Add("AutoSave");
                arrColumnProcs.Add("SOXApproval");
                arrColumnProcs.Add("VendorSelect");
                arrColumnProcs.Add("CustomerSelect");
                arrColumnProcs.Add("AutofillBPGID1");
                arrColumnProcs.Add("AutofillBPGID2");

                //Get the relevant branch node.For BPCIcon functionality
                string BPCBranchName = string.Empty;

                XmlNode nodeGridLayOut = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                foreach (XmlNode nodeTree in nodeGridLayOut.ChildNodes)//Loop all the trees
                {
                    string treeNodeName = nodeTree.SelectSingleNode("Node").InnerText;
                    //Go the tree node and pick up the columns node.
                    XmlNode nodeTreeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                    foreach (XmlNode nodeCol in nodeTreeColumns.ChildNodes)
                    {
                        XmlAttribute attBPControl = nodeCol.Attributes["BPControl"];
                        //Get all the processes list assigned to the columns except Process BPGID
                        if (attBPControl != null && attBPControl.Value.Trim() != string.Empty
                                            && !attBPControl.Value.Contains("BPGID"))
                        {
                            arrColumnProcs.Add(attBPControl.Value.Trim());
                        }
                    }

                    XmlNode nodeBranches = nodeTree.SelectSingleNode("Branches");
                    if (nodeBranches != null)
                    {
                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)//Loop all the branches of the current tree.
                        {
                            string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                            XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                            foreach (XmlNode nodeCol in nodeColumns.ChildNodes)
                            {
                                XmlAttribute attBPControl = nodeCol.Attributes["BPControl"];
                                if (attBPControl != null && attBPControl.Value.Trim() != string.Empty
                                            && !attBPControl.Value.Contains("BPGID"))
                                {
                                    arrColumnProcs.Add(attBPControl.Value.Trim());
                                }
                            }

                            if (BPCBranchName.Length == 0)
                            {
                                if (nodeBranch.Attributes["ControlType"] == null)
                                {
                                    BPCBranchName = branchNodeName;
                                }
                            }
                        }
                    }
                }

                ArrayList arrFormLevelProcs = new ArrayList();
                foreach (XmlNode nodeProcess in nodeBPC.ChildNodes)
                {
                    string processName = nodeProcess.Attributes["ID"].Value.Trim();
                    if (!arrColumnProcs.Contains(processName))
                    {
                        arrFormLevelProcs.Add(processName);
                    }
                }
                //Create the Table..
                return CreateProcessLinksTable(xDoc, arrFormLevelProcs);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                Table tblErrorContainer = new Table();
                TableRow tr = new TableRow();
                TableCell td = new TableCell();
                td.Text = ex.Message;
                td.ForeColor = Color.Red;
                tr.Cells.Add(td);
                tblErrorContainer.Rows.Add(tr);
                return tblErrorContainer;
            }
        }

        /// <summary>
        /// Initialises the BPC Icons if any in the page       
        /// </summary>
        public void InitBPCIcon(XmlDocument xDoc, Panel pnlEntryForm)
        {
            //string uniqueName = string.Empty;
            //Added on 22/09/2008
            //BPC Icon requirement - CustomerSelect & VendorSelect cases.
            XmlNode nodeBPC = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls");
            if (nodeBPC == null)
            {
                return;
            }

            XmlNode nodeSelect = nodeBPC.SelectSingleNode("BusinessProcess[@ID='VendorSelect']");//VendorSelect or CustomerSelect
            if (nodeSelect == null)
            {
                //Look for CustomerSelect
                nodeSelect = nodeBPC.SelectSingleNode("BusinessProcess[@ID='CustomerSelect']");
            }

            if (nodeSelect != null)//Check if a process has been assigned.
            {
                //string operationMode = ((ImageButton)pnlEntryForm.FindControl("imgbtnSubmit")).AlternateText;
                string BPCBranchName = string.Empty;
                XmlNode nodeGridLayOut = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
                foreach (XmlNode nodeTree in nodeGridLayOut.ChildNodes)//Loop all the trees
                {
                    string treeNodeName = nodeTree.SelectSingleNode("Node").InnerText;
                    XmlNode nodeBranches = nodeTree.SelectSingleNode("Branches");
                    if (nodeBranches != null)
                    {
                        foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)//Loop all the branches of the current tree.
                        {
                            string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                            if (BPCBranchName.Length == 0)
                            {
                                if (nodeBranch.Attributes["ControlType"] == null)
                                {
                                    BPCBranchName = branchNodeName;
                                    break;
                                }
                            }
                        }
                    }
                }

                string currentBPGID = nodeSelect.Attributes["BPGID"].Value;
                string navigatePage = nodeSelect.Attributes["PageInfo"].Value;
                string formBPGID = xDoc.SelectSingleNode("Root/bpeout/FormInfo/BPGID").InnerText;

                //Get the name of the first control in the UI.
                XmlNode nodeTopControl = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + BPCBranchName + "/GridHeading/Columns/Col[@SortOrder='1']");
                string autoFillLabel = nodeTopControl.Attributes["Label"].Value;
                if (nodeTopControl != null && nodeTopControl.Attributes["ControlType"].Value == "TBox")//Make sure the control is a textbox
                {
                    //Find that control
                    string uniqueName = autoFillLabel + "_" + BPCBranchName;
                    LAjitTextBox txtTopOne = (LAjitTextBox)pnlEntryForm.FindControl("txt" + uniqueName);
                    string callingObjXML = "<bpinfo><BPGID>" + currentBPGID + "</BPGID><CallingObject><BPGID>" + formBPGID
                        + "</BPGID><TrxID></TrxID><TrxType></TrxType><PageInfo>" + navigatePage
                        + "</PageInfo><Caption></Caption></CallingObject></bpinfo>";

                    BtnsUC = (LAjitDev.UserControls.ButtonsUserControl)pnlEntryForm.Parent.FindControl("BtnsUC");
                    if (BtnsUC == null)
                    {
                        return;
                    }
                    string autoFillTrxID = string.Empty;
                    string autoFillTrxType = string.Empty;
                    if (!string.IsNullOrEmpty(BtnsUC.RwToBeModified))
                    {
                        //Get the AutoFill TrxID and TrxType from the Branch Row.
                        XmlDocument xDocSelectedRow = new XmlDocument();
                        xDocSelectedRow.LoadXml(BtnsUC.RwToBeModified);
                        string parentTrxID = xDocSelectedRow.FirstChild.Attributes["TrxID"].Value;
                        string parentTrxType = xDocSelectedRow.FirstChild.Attributes["TrxType"].Value;

                        XmlNode nodeBranchRow = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + BPCBranchName + "/RowList//Rows[@" + BtnsUC.TreeNode + "_TrxID='" + parentTrxID + "' and @" + BtnsUC.TreeNode + "_TrxType='" + parentTrxType + "']");
                        if (nodeBranchRow != null)
                        {
                            if (nodeBranchRow.Attributes[autoFillLabel + "_TrxID"] != null)
                            {
                                autoFillTrxID = nodeBranchRow.Attributes[autoFillLabel + "_TrxID"].Value;
                            }
                            if (nodeBranchRow.Attributes[autoFillLabel + "_TrxType"] != null)
                            {
                                autoFillTrxType = nodeBranchRow.Attributes[autoFillLabel + "_TrxType"].Value;
                            }
                        }
                    }

                    LAjitLinkButton lnkBtnBPCIcon = (LAjitLinkButton)pnlEntryForm.FindControl("lnkBtn" + uniqueName);
                    if (lnkBtnBPCIcon == null)
                    {
                        return;
                    }
                    lnkBtnBPCIcon.MapXML = txtTopOne.MapXML;
                    lnkBtnBPCIcon.CommandArgument = callingObjXML;
                    lnkBtnBPCIcon.Text = nodeSelect.Attributes["Label"].Value;

                    //Add all the parameters as attributes to the Link Button so that they can be re-organised 
                    //in the client-click event..
                    txtTopOne.Attributes["AutoFillID"] = autoFillTrxID + "~" + autoFillTrxType;
                    txtTopOne.Attributes["CallerType"] = txtTopOne.MapXML.Replace("AutoFill", "");
                    txtTopOne.Attributes["POLinkID"] = lnkBtnBPCIcon.ClientID;
                    lnkBtnBPCIcon.Attributes["BPGID"] = currentBPGID;
                    lnkBtnBPCIcon.Attributes["PageInfo"] = navigatePage;
                    lnkBtnBPCIcon.Attributes["FORMBPGID"] = formBPGID;
                    lnkBtnBPCIcon.Attributes["IsPopUp"] = nodeSelect.Attributes["IsPopup"].Value;
                    lnkBtnBPCIcon.Attributes["ProcName"] = nodeSelect.Attributes["ID"].Value;
                    lnkBtnBPCIcon.OnClientClick = "javascript:return ShowPOs(this,'" + txtTopOne.ClientID + "');";

                    //txtTopOne.Attributes.Add("onblur", "javascript:alert('Focus Out!!');CheckForPOs(this,'" + txtTopOne.MapXML.Replace("AutoFill", "") + "','" + lnkBtnBPCIcon.ClientID + "');");
                    txtTopOne.Attributes.Add("onkeyup", "javascript:HidePOLink(this)");
                    lnkBtnBPCIcon.Visible = true;
                    lnkBtnBPCIcon.Style.Add("display", "none");//Will be changed in client-side depending on the availability of PO's                    
                }
            }
            //return uniqueName;
        }

        //Image Button
        [Obsolete("Functionality is being achieved in the client-side")]
        /*public void imgBPCIcon_Click(object sender, ImageClickEventArgs e)
        {
            LAjitImageButton imgBtnThis = (LAjitImageButton)sender;
            //Get the associated textbox of the BPC icon.
            LAjitTextBox txtReference = (LAjitTextBox)imgBtnThis.Parent.Parent.FindControl(imgBtnThis.ID.Replace("imgBtn", "txt"));
            //if (txtReference.Text.Trim().Length == 0)
            //{
            //    string alertString = "alert('Please select an item first!');";
            //    ScriptManager.RegisterClientScriptBlock(BtnsUC.Page, BtnsUC.Page.GetType(), "EmptyText",alertString, true);
            //      return;
            //}
            XmlDocument xDocBPEINFO = new XmlDocument();
            string autoFillTrxID = string.Empty;
            string autoFillTrxType = string.Empty;
            string POFound = string.Empty;
            string BPEINFO = imgBtnThis.CommandArgument;
            xDocBPEINFO.LoadXml(BPEINFO);
            string controlText = txtReference.Text;
            string pageInfo = xDocBPEINFO.SelectSingleNode("bpinfo/CallingObject/PageInfo").InnerText;
            string BPGID = xDocBPEINFO.SelectSingleNode("bpinfo/BPGID").InnerText;//The Process's BPGID
            string cacheName = Classes.AutoFill.GetLoggedInCompanyID() + imgBtnThis.MapXML;
            string IsPopUp = imgBtnThis.Attributes["IsPopUp"];
            string procName = imgBtnThis.Attributes["ProcName"];
            if (System.Web.HttpContext.Current.Cache[cacheName] != null)
            {
                DataSet dsAutoFill = (DataSet)System.Web.HttpContext.Current.Cache[cacheName];
                if (dsAutoFill.Tables[imgBtnThis.MapXML.ToString()].Rows.Count > 0)
                {
                    //Step 1 look Exact match word
                    if (dsAutoFill.Tables[imgBtnThis.MapXML.ToString()].Rows.Count > 0)
                    {
                        DataRow[] drfoundrows;
                        drfoundrows = dsAutoFill.Tables[0].Select(imgBtnThis.MapXML.Trim() + "='" + controlText.Trim().Replace("'", "''") + "'");

                        if (drfoundrows.Length > 0)
                        {
                            autoFillTrxID = drfoundrows[0][imgBtnThis.MapXML.Trim() + "_TrxID"].ToString();
                            autoFillTrxType = drfoundrows[0][imgBtnThis.MapXML.Trim() + "_TrxType"].ToString();
                            POFound = drfoundrows[0]["POFound"].ToString();
                        }
                    }

                    // Step 2  Exact match not found take like first record
                    if ((autoFillTrxID == string.Empty) && (autoFillTrxType == string.Empty))
                    {
                        DataRow[] drfoundrows;
                        drfoundrows = dsAutoFill.Tables[0].Select(imgBtnThis.MapXML.Trim() + " like '" + controlText.Trim().Replace("'", "''") + "%'");

                        if (drfoundrows.Length > 0)
                        {
                            autoFillTrxID = drfoundrows[0][imgBtnThis.MapXML.Trim() + "_TrxID"].ToString();
                            autoFillTrxType = drfoundrows[0][imgBtnThis.MapXML.Trim() + "_TrxType"].ToString();
                            POFound = drfoundrows[0]["POFound"].ToString();
                        }
                    }
                }
            }

            if (POFound.Length == 0 || POFound == "0")
            {
                string noPOAlert = "alert('No Items found for this selection')";
                ScriptManager.RegisterClientScriptBlock(BtnsUC.Page, BtnsUC.Page.GetType(), "ShowNoPOAlert", noPOAlert, true);
            }
            else
            {
                xDocBPEINFO.SelectSingleNode("bpinfo/CallingObject/TrxID").InnerText = autoFillTrxID;
                xDocBPEINFO.SelectSingleNode("bpinfo/CallingObject/TrxType").InnerText = autoFillTrxType;
                xDocBPEINFO.SelectSingleNode("bpinfo/CallingObject/Caption").InnerText = controlText;

                HiddenField hdnBPIconBPEINFO = (HiddenField)BtnsUC.FindControl("hdnBPIconBPEINFO");
                if (hdnBPIconBPEINFO != null)
                {
                    hdnBPIconBPEINFO.Value = xDocBPEINFO.SelectSingleNode("bpinfo/CallingObject").OuterXml;
                    string BPCIconScript = "OnFormBPCIconClick('" + BPGID + "','" + pageInfo.Replace(" ", "") + "','"
                                        + hdnBPIconBPEINFO.ClientID + "','" + procName + "','" + IsPopUp + "')";
                    //string BPCIconScript = "alert($find('mpePagePopUpBehaviourID'))";
                    //Find the page's script manager in the page and set LoadScriptsBeforeUI to false
                    //Purpose : The ModalPopUpExtender object not getting found in js code.
                    //((ScriptManager)BtnsUC.Page.Master.FindControl("topLeftScriptMngr")).LoadScriptsBeforeUI = true;
                    //Commented in V3
                    //ScriptManager.RegisterClientScriptBlock(BtnsUC.Page, BtnsUC.Page.GetType(), "ShowPagePopUp", BPCIconScript, true);
                    //if (IsPopUp == "1")
                    //{
                    //    AjaxControlToolkit.ModalPopupExtender mpePagePopUp = (AjaxControlToolkit.ModalPopupExtender)BtnsUC.FindControl("mpePagePopUp");
                    //    mpePagePopUp.Show();
                    //}
                }
            }

            XmlDocument xDocOut = new XmlDocument();
            xDocOut.LoadXml(BtnsUC.GVDataXml);
            Panel pnlEntryForm = (Panel)BtnsUC.Page.Master.FindControl("cphPageContents").FindControl("pnlEntryForm");
            InitialiseBranchObjects(xDocOut, pnlEntryForm);
        }
        //Hyperlink
        [Obsolete("Functionality is being achieved in the client-side")]
        public void BPCIconLinkClick(object sender, EventArgs e)
        {
            LAjitLinkButton imgBtnThis = (LAjitLinkButton)sender;
            //Get the associated textbox of the BPC icon.
            LAjitTextBox txtReference = (LAjitTextBox)imgBtnThis.Parent.Parent.FindControl(imgBtnThis.ID.Replace("lnkBtn", "txt"));
            //if (txtReference.Text.Trim().Length == 0)
            //{
            //    string alertString = "alert('Please select an item first!');";
            //    ScriptManager.RegisterClientScriptBlock(BtnsUC.Page, BtnsUC.Page.GetType(), "EmptyText",alertString, true);
            //      return;
            //}
            string str = txtReference.Attributes["autofillid"];
            XmlDocument xDocBPEINFO = new XmlDocument();
            string autoFillTrxID = string.Empty;
            string autoFillTrxType = string.Empty;
            string POFound = string.Empty;
            string BPEINFO = imgBtnThis.CommandArgument;
            xDocBPEINFO.LoadXml(BPEINFO);
            string controlText = txtReference.Text;
            string pageInfo = xDocBPEINFO.SelectSingleNode("bpinfo/CallingObject/PageInfo").InnerText;
            string BPGID = xDocBPEINFO.SelectSingleNode("bpinfo/BPGID").InnerText;//The Process's BPGID
            string cacheName = Classes.AutoFill.GetLoggedInCompanyID() + imgBtnThis.MapXML;
            string IsPopUp = imgBtnThis.Attributes["IsPopUp"];
            string procName = imgBtnThis.Attributes["ProcName"];
            if (System.Web.HttpContext.Current.Cache[cacheName] != null)
            {
                DataSet dsAutoFill = (DataSet)System.Web.HttpContext.Current.Cache[cacheName];
                if (dsAutoFill.Tables[imgBtnThis.MapXML.ToString()].Rows.Count > 0)
                {
                    //Step 1 look Exact match word
                    if (dsAutoFill.Tables[imgBtnThis.MapXML.ToString()].Rows.Count > 0)
                    {
                        DataRow[] drfoundrows;
                        drfoundrows = dsAutoFill.Tables[0].Select(imgBtnThis.MapXML.Trim() + "='" + controlText.Trim().Replace("'", "''") + "'");

                        if (drfoundrows.Length > 0)
                        {
                            autoFillTrxID = drfoundrows[0][imgBtnThis.MapXML.Trim() + "_TrxID"].ToString();
                            autoFillTrxType = drfoundrows[0][imgBtnThis.MapXML.Trim() + "_TrxType"].ToString();
                            POFound = drfoundrows[0]["POFound"].ToString();
                        }
                    }

                    // Step 2  Exact match not found take like first record
                    if ((autoFillTrxID == string.Empty) && (autoFillTrxType == string.Empty))
                    {
                        DataRow[] drfoundrows;
                        drfoundrows = dsAutoFill.Tables[0].Select(imgBtnThis.MapXML.Trim() + " like '" + controlText.Trim().Replace("'", "''") + "%'");

                        if (drfoundrows.Length > 0)
                        {
                            autoFillTrxID = drfoundrows[0][imgBtnThis.MapXML.Trim() + "_TrxID"].ToString();
                            autoFillTrxType = drfoundrows[0][imgBtnThis.MapXML.Trim() + "_TrxType"].ToString();
                            POFound = drfoundrows[0]["POFound"].ToString();
                        }
                    }
                }
            }

            if (POFound.Length == 0 || POFound == "0")
            {
                string noPOAlert = "alert('No Items found for this selection')";
                ScriptManager.RegisterClientScriptBlock(BtnsUC.Page, BtnsUC.Page.GetType(), "ShowNoPOAlert", noPOAlert, true);
            }
            else
            {
                xDocBPEINFO.SelectSingleNode("bpinfo/CallingObject/TrxID").InnerText = autoFillTrxID;
                xDocBPEINFO.SelectSingleNode("bpinfo/CallingObject/TrxType").InnerText = autoFillTrxType;
                xDocBPEINFO.SelectSingleNode("bpinfo/CallingObject/Caption").InnerText = controlText;

                HiddenField hdnBPIconBPEINFO = (HiddenField)BtnsUC.FindControl("hdnBPIconBPEINFO");
                if (hdnBPIconBPEINFO != null)
                {
                    hdnBPIconBPEINFO.Value = xDocBPEINFO.SelectSingleNode("bpinfo/CallingObject").OuterXml;
                    string BPCIconScript = "OnFormBPCIconClick('" + BPGID + "','" + pageInfo.Replace(" ", "") + "','"
                                        + hdnBPIconBPEINFO.ClientID + "','" + procName + "','" + IsPopUp + "')";
                    //string BPCIconScript = "alert($find('mpePagePopUpBehaviourID'))";
                    //Find the page's script manager in the page and set LoadScriptsBeforeUI to false
                    //Purpose : The ModalPopUpExtender object not getting found in js code.
                    //((ScriptManager)BtnsUC.Page.Master.FindControl("topLeftScriptMngr")).LoadScriptsBeforeUI = true;
                    //ScriptManager.RegisterClientScriptBlock(BtnsUC.Page, BtnsUC.Page.GetType(), "ShowPagePopUp", BPCIconScript, true);
                    //if (IsPopUp == "1")
                    //{
                    //    AjaxControlToolkit.ModalPopupExtender mpePagePopUp = (AjaxControlToolkit.ModalPopupExtender)BtnsUC.FindControl("mpePagePopUp");
                    //    mpePagePopUp.Show();
                    //}
                }
            }

            XmlDocument xDocOut = new XmlDocument();
            xDocOut.LoadXml(BtnsUC.GVDataXml);
            Panel pnlEntryForm = (Panel)BtnsUC.Page.Master.FindControl("cphPageContents").FindControl("pnlEntryForm");
            InitialiseBranchObjects(xDocOut, pnlEntryForm);
        }*/

        /// <summary>
        /// Creats an Asp Table with the required styling as per the indicated processes.
        /// </summary>
        /// <param name="xDoc">The Page Document</param>
        /// <param name="arrFormLevelProcs">The list of processes in the table.</param>
        /// <returns>Asp Table.</returns>
        private Table CreateProcessLinksTable(XmlDocument xDoc, ArrayList arrFormLevelProcs)
        {
            #region NLog
            logger.Info("Creating an Asp Table with the required styling as per the indicated processes.");
            #endregion

            Table tblProcessLinks = new Table();
            //Add a new row to the table.
            TableRow tr = new TableRow();
            tr.ID = "trDynamicProcessLinks";
            tblProcessLinks.Rows.Add(tr);
            int rowCntr = 0;
            for (int procCntr = 0; procCntr < arrFormLevelProcs.Count; procCntr++)
            {
                string process = Convert.ToString(arrFormLevelProcs[procCntr]);
                XmlNode nodeProc = xDoc.SelectSingleNode("Root/bpeout/FormControls/BusinessProcessControls/BusinessProcess[@ID='" + process + "']");
                string currentBPGID = nodeProc.Attributes["BPGID"].Value;
                string pageInfo = nodeProc.Attributes["PageInfo"].Value;
                string confirmMessage = string.Empty;//Confirm message to be shown before running the BP.
                string hdnVarName = string.Empty;
                string trxID = string.Empty;
                string trxType = string.Empty;
                string ParentNode = string.Empty;

                ParentNode = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                if (GVUC != null)
                {
                    hdnVarName = GVUC.ClientID + "_hdnGVBPEINFO";//The place where the COXML can found in the client-side
                }
                else if (PCGVUC != null)
                {
                    hdnVarName = PCGVUC.ClientID + "_hdnGVBPEINFO";//The place where the COXML can found in the client-side
                }

                //Get the attribute which the text to be displayed while confirming the execution of the proc.
                XmlAttribute attConfirm = nodeProc.Attributes["MsgPrompt"];
                if (attConfirm != null)
                {
                    confirmMessage = attConfirm.Value;
                    confirmMessage = confirmMessage.Replace("'", "\\'").Replace(" ", "~::~"); ;
                    confirmMessage = this.HtmlEncode(confirmMessage);
                }


                Label lblCurrent = new Label();
                TableCell tdLinkText = new TableCell();
                tdLinkText.Wrap = false;
                //Check if the current process is a Refresh Process and there is no valid page info.
                if (IsProcessRefresh(process) && !IsValidURL(pageInfo))
                {
                    LAjitLinkButton lnkBtnProcRefresh = new LAjitLinkButton();
                    lnkBtnProcRefresh.ID = "lnkBtnRefresh" + nodeProc.Attributes["ID"].Value;
                    lnkBtnProcRefresh.Text = nodeProc.Attributes["Label"].Value;
                    lnkBtnProcRefresh.CommandArgument = currentBPGID;
                    lnkBtnProcRefresh.Click += new EventHandler(lnkBtnRefresh_Click);
                    lnkBtnProcRefresh.OnClientClick = "javascript: var proceed=confirm('" + confirmMessage + "');if(proceed)return true;else return false; ";
                    tdLinkText.Controls.Add(lnkBtnProcRefresh);
                }
                else
                {
                    if (xDoc.SelectSingleNode("//bpeout/FormInfo/PageInfo").InnerText.Trim().Contains("SelectInvoice"))
                    {
                        lblCurrent.Text = "<a oncontextmenu='return false;' Title='" + nodeProc.Attributes["Label"].Value
                        + "'" + " href=#>"
                        + nodeProc.Attributes["Label"].Value + "</a>";
                        lblCurrent.Attributes.Add("onclick", "javascript:OnSelectFormBPCLinkClick('" + currentBPGID + "','" + pageInfo.Replace(" ", "") + "','"
                        + hdnVarName + "','" + nodeProc.Attributes["ID"].Value + "','" + nodeProc.Attributes["IsPopup"].Value + "','" + confirmMessage + "');");
                    }
                    else
                    {
                        if (pageInfo.ToUpper().Contains("MAILPDF"))
                        {
                            lblCurrent.Text = "<a onmouseover=PreInitEmailHover('" + currentBPGID + "','" + hdnVarName + "',this)  id='aProcessMenu' style='cursor: pointer' oncontextmenu='return false;' Title='" + nodeProc.Attributes["Label"].Value
                         + "'" + " href=javascript:OnFormBPCLinkClick('" + currentBPGID + "','" + pageInfo.Replace(" ", "") + "','"
                         + hdnVarName + "','" + nodeProc.Attributes["ID"].Value + "','" + nodeProc.Attributes["IsPopup"].Value + "','" + confirmMessage + "')>"
                         + nodeProc.Attributes["Label"].Value + "</a>";
                        }
                        else
                        {
                            lblCurrent.Text = "<a oncontextmenu='return false;' Title='" + nodeProc.Attributes["Label"].Value
                            + "'" + " href=javascript:OnFormBPCLinkClick('" + currentBPGID + "','" + pageInfo.Replace(" ", "") + "','"
                            + hdnVarName + "','" + nodeProc.Attributes["ID"].Value + "','" + nodeProc.Attributes["IsPopup"].Value + "','" + confirmMessage + "')>"
                            + nodeProc.Attributes["Label"].Value + "</a>";
                        }
                    }
                    tdLinkText.Controls.Add(lblCurrent);
                }
                tr.Cells.Add(tdLinkText);
                //TD for the separator
                TableCell tdLinkSeparator = new TableCell();
                tdLinkSeparator.Width = Unit.Pixel(5);
                tdLinkSeparator.Text = "|";
                tr.Cells.Add(tdLinkSeparator);
                //Accomodates 7(the number/2) links in each row.
                //Second Condition:If the last process has already been added then don't add a new row.
                if (tr.Cells.Count >= 14 && procCntr != arrFormLevelProcs.Count - 1)
                {
                    tr = new TableRow();
                    tblProcessLinks.Rows.Add(tr);
                    rowCntr++;
                }
            }
            //Remove the trailing separator for each row.
            foreach (TableRow trToRemove in tblProcessLinks.Rows)
            {
                if (trToRemove.Cells.Count > 0)
                {
                    trToRemove.Cells.Remove(trToRemove.Cells[trToRemove.Cells.Count - 1]);
                }
            }
            return tblProcessLinks;
        }

        /// <summary>
        /// Checks to see if the process is a Refresh Process.Format is Process(n)Refresh.
        /// </summary>
        /// <param name="process">Proc Name.</param>
        /// <returns>True if Refresh Proc else false.</returns>
        private bool IsProcessRefresh(string process)
        {
            return process.Contains("Refresh");
        }

        void lnkBtnRefresh_Click(object sender, EventArgs e)
        {
            LAjitLinkButton lnkBtnSender = (LAjitLinkButton)sender;
            string BPGID = lnkBtnSender.CommandArgument;
            HiddenField hdnGVBPEINFO = (HiddenField)GVUC.FindControl("hdnGVBPEINFO");
            GridView grdVwContents = (GridView)GVUC.FindControl("grdVwContents");
            string currentPageNo = ((HiddenField)GVUC.FindControl("hdnCurrPageNo")).Value;
            string gvBPInfo = "<bpinfo>" + hdnGVBPEINFO.Value + "</bpinfo>"; ;

            XmlDocument xDocBPInfo = new XmlDocument();
            xDocBPInfo.LoadXml(gvBPInfo);
            string trxID = xDocBPInfo.SelectSingleNode("//Rows").Attributes["TrxID"].Value;
            string trxType = xDocBPInfo.SelectSingleNode("//Rows").Attributes["TrxType"].Value;
            string defaultPageSize = grdVwContents.PageSize.ToString();

            XmlNode nodeTree = xDocBPInfo.SelectSingleNode("//" + BtnsUC.TreeNode);
            if (nodeTree == null)
            {
                nodeTree = xDocBPInfo.CreateElement(BtnsUC.TreeNode);
                nodeTree.InnerXml = @"<Gridview><Pagenumber>" + currentPageNo + "</Pagenumber><Pagesize>"
                    + defaultPageSize + "</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";

                //Remove the Rows node form the parent node bpinfo and add it to nodeTree
                XmlNode nodeRow = xDocBPInfo.SelectSingleNode("bpinfo/Rows");
                nodeRow.ParentNode.RemoveChild(nodeRow);
                nodeTree.PrependChild(nodeRow);
                //Append the newly created nodeTree to the bpInfo node
                xDocBPInfo.DocumentElement.AppendChild(nodeTree);
            }
            else
            {
                nodeTree.InnerXml += @"<Gridview><Pagenumber>" + currentPageNo + "</Pagenumber><Pagesize>"
                    + defaultPageSize + "</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
            }

            //Create and append the BPGID node
            XmlNode nodeBPGID = xDocBPInfo.CreateElement("BPGID");
            nodeBPGID.InnerText = BPGID;
            xDocBPInfo.DocumentElement.PrependChild(nodeBPGID);


            if (xDocBPInfo.SelectSingleNode("bpinfo/CallingObject") == null)
            {
                //Create and append the Calling Object node.
                //string callingObjXML = "<CallingObject><BPGID>" + BPGID + "</BPGID><TrxID>" + trxID + "</TrxID><TrxType>" + trxType + "</TrxType><PageInfo>" + HttpContext.Current.Request.Url.AbsolutePath + "</PageInfo><Caption>" + lnkBtnSender.Text + "</Caption></CallingObject>";
                //xDocBPInfo.InnerXml += callingObjXML;
                XmlNode nodeCO = xDocBPInfo.CreateElement("CallingObject");
                nodeCO.InnerXml = "<BPGID>" + BPGID + "</BPGID><TrxID>" + trxID + "</TrxID><TrxType>" + trxType + "</TrxType><PageInfo>" + HttpContext.Current.Request.Url.AbsolutePath + "</PageInfo><Caption>" + lnkBtnSender.Text + "</Caption>";
                xDocBPInfo.DocumentElement.AppendChild(nodeCO);
            }


            string BPINFO = xDocBPInfo.OuterXml;
            CommonBO objBO = new CommonBO();
            XDocUserInfo = loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            //Send the Request to the DB to do the corresponding opertion.
            string strReqXml = objBO.GenActionRequestXML("PAGELOAD", BPINFO, "", "", "", strBPE, false, "1", "1", null);
            string strOutXml = objBO.GetDataForCPGV1(strReqXml);
            //Check if NextPage is being sent.
            XmlDocument xDocOut = new XmlDocument();
            //xDocOut.Load(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\nextBPschema.xml");
            xDocOut.LoadXml(strOutXml);
            XmlNode nodeNextPage = xDocOut.SelectSingleNode("Root/NextPage");
            if (nodeNextPage != null)
            {
                XmlNode nodeBpInfo = nodeNextPage.SelectSingleNode("bpinfo");
                XmlNode nodeFormInfo = nodeBpInfo.SelectSingleNode("FormInfo");//Remove this node embedded in NextPage node.
                nodeBpInfo.RemoveChild(nodeFormInfo);
                XmlNode nodeNavBPGID = nodeFormInfo.SelectSingleNode("BPGID");
                //Remove nodeBPGID from nodeFormInfo and append it to nodeBpInfo
                nodeFormInfo.RemoveChild(nodeNavBPGID);
                nodeBpInfo.PrependChild(nodeNavBPGID);
                //Create the CallingObject and append it to nodeBpInfo.
                XmlNode nodeCO = xDocOut.CreateElement("CallingObject");
                nodeCO.InnerXml = "<BPGID>" + BPGID + "</BPGID><TrxID>" + trxID + "</TrxID><TrxType>" + trxType + "</TrxType><PageInfo>" + HttpContext.Current.Request.Url.AbsolutePath + "</PageInfo><Caption>" + lnkBtnSender.Text + "</Caption>";
                nodeBpInfo.AppendChild(nodeCO);

                string redirectPage = "../" + nodeFormInfo.SelectSingleNode("PageInfo").InnerText;

                //Store the passed bpinfo in the Session and redirect to the passed page.
                HttpContext.Current.Session["BPINFO"] = nodeBpInfo.OuterXml;
                HttpContext.Current.Response.Redirect(redirectPage);
            }
            else
            {
                //Refresh the page.
                ReloadPage(trxID, trxType, strBPE, (Control)sender);
            }
        }

        /// <summary>
        /// Gets the BPOut from DB again and binds it to the UI.
        /// </summary>
        /// <param name="trxID">TrxID of the selected row.</param>
        /// <param name="trxType">TrxType of the selected row.</param>
        /// <param name="strBPE">Session BPE string</param>
        public void ReloadPage(string trxID, string trxType, string strBPE, Control ctrlPostback)
        {
            #region NLog
            logger.Info("Getting the BPOut from DB again and binds it to the UI with trx id as : " + trxID + " and trx type as : " + trxType + " and BPE : " + strBPE); ;
            #endregion

            string bpInfo = GVUC.GridViewBPInfo;
            //If BPInfo is that of CreatePayments then consider the BPGID present in the CallingObject.
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(bpInfo);
            XmlNode nodeBPGID = xDoc.SelectSingleNode("bpinfo/BPGID");
            string BPGID = string.Empty;
            if (nodeBPGID != null)
            {
                BPGID = nodeBPGID.InnerText;
            }
            if (BPGID.Length > 0)
            {
                if (BPGID == "1174" || BPGID == "531")
                {
                    XmlNode nodeCO = xDoc.SelectSingleNode("bpinfo/CallingObject");
                    if (nodeCO != null)
                    {
                        XmlNode nodeCOBPGID = nodeCO.SelectSingleNode("BPGID");
                        if (nodeCOBPGID != null)
                        {
                            bpInfo = "<bpinfo>" + nodeCOBPGID.OuterXml + "</bpinfo>";
                        }
                    }
                }
            }

            //Set the BPINFO Session Variable.
            string qspDepth = ctrlPostback.Page.Request.QueryString["depth"];
            int popUpDepth = 0;
            if (!string.IsNullOrEmpty(qspDepth))
            {
                popUpDepth = Convert.ToInt32(qspDepth);
            }
            if (popUpDepth >= 1)
            {
                SessionLinkBPInfo = bpInfo;
            }
            else
            {
                SessionBPInfo = bpInfo;
            }


            //Reload the entire page.
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.UrlReferrer.ToString());

            CommonBO objBO = new CommonBO();
            //Get the BPINFO of the current page.
            string refreshReqXML = "<Root>" + strBPE + bpInfo + "</Root>";
            string refreshResponseXML = objBO.GetDataForCPGV1(refreshReqXML);
            XmlDocument xDocRefresh = new XmlDocument();
            xDocRefresh.LoadXml(refreshResponseXML);
            XmlNode nodeMsgStatus = xDocRefresh.SelectSingleNode("Root/bpeout/MessageInfo/Status");
            XmlNode nodeMsg = xDocRefresh.SelectSingleNode("Root/bpeout/MessageInfo/Message");
            Control ctrlEntryForm = this.UpdatePanelContent.FindControl("pnlEntryForm");

            if (nodeMsgStatus.InnerText == "Success")
            {
                Label lblMessage = (Label)ctrlEntryForm.FindControl("lblMsg");
                //Update the local variables with the new XML.
                BtnsUC.GVDataXml = xDocRefresh.OuterXml;
                //Get the current row from the new XML and update it in the RwToBeModifed
                XmlNode nodeRowToBeMod = xDocRefresh.SelectSingleNode("Root/bpeout/FormControls/" + BtnsUC.TreeNode + "/RowList/Rows[@TrxID='" + trxID + "' and @TrxType='" + trxType + "']");

                if (nodeRowToBeMod != null)
                {
                    BtnsUC.RwToBeModified = nodeRowToBeMod.OuterXml;
                }
                else
                {
                    nodeRowToBeMod = xDocRefresh.CreateNode(XmlNodeType.Element, "RowsList", "");
                    nodeRowToBeMod.InnerXml = BtnsUC.RwToBeModified;
                    nodeRowToBeMod = nodeRowToBeMod.FirstChild;
                }
                //BindPage(GVUC.GridViewBPInfo, ctrlEntryForm);//8th June 09
                bool gridExists = false;
                if (!string.IsNullOrEmpty(GVUC.GridViewBPInfo))
                {
                    gridExists = this.ButtonsUserControl.SetGVData(GVUC.GridViewBPInfo);
                }
                //paging
                //DefaultGridSize
                string pageSize = GetUserPreferenceValue("59");
                HiddenField hdnCurrPgNo;
                string pageNo = "1";
                if (GVUC != null)
                {
                    hdnCurrPgNo = (HiddenField)GVUC.FindControl("hdnCurrPageNo");
                    pageNo = hdnCurrPgNo.Value;
                }
                else if (PCGVUC != null)
                {
                    hdnCurrPgNo = (HiddenField)PCGVUC.FindControl("hdnCurrPageNo");
                    pageNo = hdnCurrPgNo.Value;
                }


                BindOutXML((Control)this.UpdatePanelContent, gridExists, refreshReqXML, refreshResponseXML);

                //InvokeOnButtonClick("PageLoad", this.BtnsUC.Page);

                string js = "javascript:setTimeout('ExpandGrid()',50);";
                ScriptManager.RegisterStartupScript(this.BtnsUC.Page, this.BtnsUC.Page.GetType(), "ForceExpand", js, true);
                ChildGridView CGVUC = (ChildGridView)ctrlEntryForm.FindControl("CGVUC");
                if (CGVUC != null)
                {
                    CGVUC.InitialiseBranchGrid(xDocRefresh, ctrlPostback);
                }
                lblMessage.Text = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("Label").InnerText;
                lblMessage.Visible = true;
                if (this.UpdatePanelContent.UpdateMode == UpdatePanelUpdateMode.Conditional)
                {
                    this.UpdatePanelContent.Update();
                }
            }
            else
            {
                Label lblError = (Label)ctrlEntryForm.FindControl("lblError");
                if (nodeMsg != null)
                {
                    if (nodeMsg.SelectSingleNode("OtherInfo") != null)
                    {
                        if (nodeMsg.SelectSingleNode("OtherInfo").InnerText != null && nodeMsg.SelectSingleNode("OtherInfo").InnerText != string.Empty)
                        {
                            lblError.Text = nodeMsgStatus.InnerText + " - " + nodeMsg.SelectSingleNode("OtherInfo").InnerText;
                        }
                        else
                        {
                            lblError.Text = nodeMsgStatus.InnerText + "-" + nodeMsg.SelectSingleNode("Label").InnerText;
                        }
                    }
                    else
                    {
                        lblError.Text = nodeMsgStatus.InnerText + "-" + nodeMsg.SelectSingleNode("Label").InnerText;
                    }
                }
            }
            //ImageButton imgbtnSubmit = (ImageButton)ctrlEntryForm.FindControl("imgbtnSubmit");
            //imgbtnSubmit.Style["display"] = "none";
            //ImageButton imgbtnCancel = (ImageButton)ctrlEntryForm.FindControl("imgbtnCancel");
            //imgbtnCancel.Style["display"] = "none";
            //((Panel)ctrlEntryForm).Style["display"] = "block";
        }

        void lnkBtnSOX_Click(object sender, EventArgs e)
        {
            Control currentPage = ((LAjitLinkButton)sender).NamingContainer;
            SubmitEntries("SOXAPPROVAL", currentPage);
            //BtnsUC.ResetUI("ISRDBTNCLICK");
        }

        /// <summary>
        /// Checks if the URL sent from the DB is in the format "FolderName/PageName.aspx"
        /// </summary>
        /// <param name="url">URL from DB</param>
        /// <returns>True if valid, else false.</returns>
        public bool IsValidURL(string url)
        {
            //^[\w-]+\\([\w-][\w-\s]*[\w-])+\.(aspx)$
            if (url.Contains(".aspx") && url.Contains("/"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the position of the column in the given data table.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="dtSearch">Data table to search in.</param>
        /// <returns>Integer column position, zero being the first column.</returns>
        /// <returns>Returns -1 if column is not found.</returns>
        private int GetColumnIndex(string columnName, DataTable dtSearch)
        {
            int colCntr = 0;
            foreach (DataColumn dc in dtSearch.Columns)
            {
                if (dc.ColumnName.ToUpper() == columnName.ToUpper())
                {
                    return colCntr;
                }
                colCntr++;
            }
            return -1;
            //throw new Exception("Specified column" + columnName + " not found in the data source!!");
        }

        #region DropDownListData In String
        /// <summary>
        /// Getting dropdowndata in string
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <param name="RelationAttribute"></param>
        /// <returns></returns>
        public string GetDropDownData(string ColumnName, string RelationAttribute)
        {
            XmlDocument xDoc = new XmlDocument();
            string ReturnXML = "";
            string FinalResult = string.Empty;

            if (BtnsUC.GVDataXml != null)
            {
                ReturnXML = BtnsUC.GVDataXml.ToString();
                //ReturnXML = hdnReturnXML.Value.ToString();
                xDoc.LoadXml(ReturnXML);


                //Filtering 

                FinalResult = string.Empty;
                //SubFinalResult = string.Empty;
                XmlNode xnodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + ColumnName + "/RowList");
                foreach (XmlNode Nodes in xnodeRows.ChildNodes)
                {
                    if (Nodes.Attributes[RelationAttribute] != null)
                    {
                        if (FinalResult == string.Empty)
                        {
                            FinalResult = "value=" + Nodes.Attributes["DataValueField"].Value + ",text=" + Nodes.Attributes[ColumnName].Value + "," + RelationAttribute + "=" + Nodes.Attributes[RelationAttribute].Value;
                        }
                        else
                        {
                            FinalResult = FinalResult + "; value=" + Nodes.Attributes["DataValueField"].Value + ",text=" + Nodes.Attributes[ColumnName].Value + "," + RelationAttribute + "=" + Nodes.Attributes[RelationAttribute].Value;
                        }
                    }
                }
            }
            return FinalResult;
        }

        public string GetDropDownData(string ColumnName)
        {
            XmlDocument xDoc = new XmlDocument();
            string ReturnXML = "";
            string FinalResult = string.Empty;

            if (BtnsUC.GVDataXml != null)
            {
                ReturnXML = BtnsUC.GVDataXml.ToString();
                //ReturnXML = hdnReturnXML.Value.ToString();

                xDoc.LoadXml(ReturnXML);

                //Filtering 

                FinalResult = string.Empty;
                //SubFinalResult = string.Empty;
                XmlNode xnodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + ColumnName + "/RowList");

                if (xnodeRows != null)
                {
                    foreach (XmlNode Nodes in xnodeRows.ChildNodes)
                    {
                        // Only comma separator values
                        if (FinalResult == string.Empty)
                        {
                            FinalResult = Nodes.Attributes[ColumnName].Value.Trim();
                        }
                        else
                        {
                            FinalResult = FinalResult + "~" + Nodes.Attributes[ColumnName].Value.Trim();
                        }
                    }
                }
            }
            return FinalResult;
        }

        public string GetDropDownDataRelation(string ColumnName, string RelationNode, string RelationAttribute)
        {
            //  subaccount1, EntryJobSubAccount,SubAccountID
            XmlDocument xDoc = new XmlDocument();
            string ReturnXML = "";
            string FinalResult = string.Empty;
            string SubFinalResult = string.Empty;
            if (BtnsUC.GVDataXml != null)
            {
                ReturnXML = BtnsUC.GVDataXml.ToString();
                //ReturnXML = hdnReturnXML.Value.ToString();
            }
            xDoc.LoadXml(ReturnXML);

            //Filtering 

            FinalResult = string.Empty;
            //SubFinalResult = string.Empty;
            XmlNode xnodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + ColumnName + "/RowList");
            foreach (XmlNode Nodes in xnodeRows.ChildNodes)
            {
                if (Nodes.Attributes["TrxID"].Value != "-1")
                {
                    //SubFinalResult = SubFinalResult + "value=" + Nodes.Attributes["DataValueField"].Value + ",text=" + Nodes.Attributes[ColumnName].Value;
                    if (FinalResult != string.Empty)
                    {
                        FinalResult = FinalResult + ";";
                    }

                    SubFinalResult = Nodes.Attributes[ColumnName].Value + ":";

                    foreach (XmlNode xmlNodes in xDoc.SelectNodes("Root/bpeout/FormControls/" + RelationNode + "/RowList/Rows[@" + RelationAttribute + "='" + Nodes.Attributes[RelationAttribute].Value + "']"))
                    {
                        if (xmlNodes != null)
                        {
                            if (FinalResult == string.Empty)
                            {
                                // FinalResult = SubFinalResult + ",JobID=" + xmlNodes.Attributes["JobID"].Value;
                                FinalResult = SubFinalResult + xmlNodes.Attributes[RelationNode].Value;
                            }
                            else
                            {
                                FinalResult = FinalResult + "," + xmlNodes.Attributes[RelationNode].Value;
                                //  FinalResult = FinalResult + ";" + SubFinalResult + ",JobID=" + xmlNodes.Attributes["JobID"].Value;
                            }
                        }
                    }
                }
            }

            return FinalResult;
        }

        /// <summary>
        /// Getting dropdown data in string
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <param name="RelationNode"></param>
        /// <param name="RelationAttribute"></param>
        /// <returns></returns>
        public string GetDropDownSubAccounts(string ColumnName, string RelationNode, string RelationAttribute)
        {
            //  subaccount1, EntryJobSubAccount,SubAccountID
            XmlDocument xDoc = new XmlDocument();
            string ReturnXML = "";
            string FinalResult = string.Empty;
            string SubFinalResult = string.Empty;
            if (BtnsUC.GVDataXml != null)
            {
                ReturnXML = BtnsUC.GVDataXml.ToString();
                //ReturnXML = hdnReturnXML.Value.ToString();

                xDoc.LoadXml(ReturnXML);

                //Filtering 

                FinalResult = string.Empty;
                //SubFinalResult = string.Empty;
                XmlNode xnodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + ColumnName + "/RowList");
                foreach (XmlNode Nodes in xnodeRows.ChildNodes)
                {
                    if (Nodes.Attributes["TrxID"].Value != "-1")
                    {
                        SubFinalResult = SubFinalResult + "value=" + Nodes.Attributes["DataValueField"].Value + ",text=" + Nodes.Attributes[ColumnName].Value;
                        foreach (XmlNode xmlNodes in xDoc.SelectNodes("Root/bpeout/FormControls/" + RelationNode + "/RowList/Rows[@" + RelationAttribute + "='" + Nodes.Attributes["TrxID"].Value + "']"))
                        {
                            if (xmlNodes != null)
                            {
                                if (FinalResult == string.Empty)
                                {
                                    FinalResult = SubFinalResult + ",JobID=" + xmlNodes.Attributes["JobID"].Value;
                                }
                                else
                                {
                                    FinalResult = FinalResult + ";" + SubFinalResult + ",JobID=" + xmlNodes.Attributes["JobID"].Value;
                                }
                            }
                        }
                    }
                }
            }
            return FinalResult;
        }
        #endregion

        #region Ajax Methods

        /// <summary>
        /// Updates the shortcuts selection to DB(Will be called from javascript..)
        /// </summary>
        /// <param name="arrUpdates">Array consisting of the selection criterion.</param>
        /// <returns>Success string if success, else error string.</returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public string UpdateShortCuts(string[] arrUpdates)
        {
            XDocUserInfo = loadXmlFile(Convert.ToString(HttpContext.Current.Session["USERINFOXML"]));
            string strBPE = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpe").OuterXml);
            string menuXML = Convert.ToString(XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls").OuterXml);
            XmlDocument xDocShrtCuts = new XmlDocument();
            xDocShrtCuts.LoadXml(menuXML);
            string updtShrtCutsBPGID =
                xDocShrtCuts.SelectSingleNode("GlobalBusinessProcessControls/BusinessProcessControls/BusinessProcess[translate(@ID, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') ='shortcuts']").Attributes["BPGID"].Value;

            //Get the selected panel id(The first parameter)from any of the contents of the array "updates"
            string clickedPanelID = arrUpdates[0].Split('_')[0];
            XmlNode nodeSelMenuPnl = xDocShrtCuts.SelectSingleNode("GlobalBusinessProcessControls/menupanel/Panel[@ID='" + clickedPanelID + "']");

            XmlDocument xDocUpdtShrtCuts = new XmlDocument();
            XmlNode nodeShortCuts = xDocUpdtShrtCuts.CreateNode(XmlNodeType.Element, "ShortCuts", null);

            foreach (XmlNode nodeItem in nodeSelMenuPnl.ChildNodes)
            {
                string currItemID = nodeItem.Attributes["ID"].Value;
                foreach (XmlNode nodeProcess in nodeItem.ChildNodes)
                {
                    string BPGID = nodeProcess.Attributes["BPGID"].Value;
                    string thisNodeUID = clickedPanelID + "_" + currItemID + "_" + BPGID;
                    for (int index = 0; index < arrUpdates.Length; index++)
                    {
                        string uniqueID = arrUpdates[index].Split(',')[0];
                        string isChecked = arrUpdates[index].Split(',')[1];
                        if (thisNodeUID == uniqueID)
                        {
                            //Make IsShortCut="True"
                            nodeProcess.Attributes["IsShortcut"].Value = isChecked;
                            nodeShortCuts.InnerXml += "<Process BPGID=\"" + BPGID + "\"  IsShortcut=\"" + isChecked + "\"/>";
                        }
                    }
                }
            }

            XmlNode nodeRoot = xDocUpdtShrtCuts.CreateNode(XmlNodeType.Element, "Root", null);
            nodeRoot.InnerXml = strBPE;

            //Creating the bpinfo node
            XmlNode nodeBPInfo = xDocUpdtShrtCuts.CreateNode(XmlNodeType.Element, "bpinfo", null);
            nodeRoot.AppendChild(nodeBPInfo);

            //Creating the BPGID node
            XmlNode nodeBPGID = xDocUpdtShrtCuts.CreateNode(XmlNodeType.Element, "BPGID", null);
            nodeBPGID.InnerText = updtShrtCutsBPGID;
            nodeBPInfo.AppendChild(nodeBPGID);

            nodeBPInfo.AppendChild(nodeShortCuts);

            string returnXML = objBO.UpdateShortCuts(nodeRoot.OuterXml);
            xDocUpdtShrtCuts.LoadXml(returnXML);
            string updateStatus = xDocUpdtShrtCuts.SelectSingleNode("//Status").InnerText;
            string updateLabel = xDocUpdtShrtCuts.SelectSingleNode("//Label").InnerText;
            if (updateStatus == "Success")
            {

                XmlNode nodeResRow = XDocUserInfo.SelectSingleNode("Root/bpeout/GlobalBusinessProcessControls");
                nodeResRow.RemoveAll();
                XmlNode nodeResGridResults = xDocShrtCuts.SelectSingleNode("GlobalBusinessProcessControls");
                nodeResRow.InnerXml += nodeResGridResults.InnerXml;

                //XDocUserInfo = commonObjUI.loadXmlFile(Convert.ToString(Session["USERINFOXML"]));
                //Update the Session["GBPC"] to the current settings.
                // HttpContext.Current.Session["GBPC"] = xDocShrtCuts.OuterXml;
                //Return message is in the format - Status~::~MessageLabel~::~UpdatedXml
                updateStatus += "~::~" + updateLabel + "~::~" + xDocShrtCuts.SelectSingleNode("GlobalBusinessProcessControls/menupanel").OuterXml;
                XDocUserInfo.Save(HttpContext.Current.Session["USERINFOXML"].ToString());
            }
            return updateStatus;
        }

        /// <summary>
        /// Returns the string width in pixels.
        /// </summary>
        /// <param name="title">The string to measure.</param>
        /// <returns>Integer width in pixels.</returns>
        [Ajax.AjaxMethod()]
        public int GetStringWidth(string title)
        {
            Font f = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);
            // Need a bitmap to call the MeasureString method
            Bitmap objBitmap = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics objGraphics = Graphics.FromImage(objBitmap);
            int intScrollLength = (int)objGraphics.MeasureString(title, f).Width;
            //Padding 
            intScrollLength = intScrollLength + 20;
            return intScrollLength;
        }

        /// <summary>
        /// To Set the Session "BPinfo". This method will be called from javascript...
        /// </summary>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void SetBPINFOSession(string BPInfo)
        {
            if (BPInfo != null)
            {
                System.Web.HttpContext.Current.Session["BPINFO"] = BPInfo;
            }
        }

        #endregion

        public string GetPageColumns(XmlDocument xDocForm)
        {

            #region NLog
            logger.Info("Getting the page columns from the xDocForm");
            #endregion

            string strPageControls = string.Empty;
            XmlNode nodeGridLayOut = xDocForm.SelectSingleNode("Root/bpeout/FormControls/GridLayout");
            foreach (XmlNode nodeTree in nodeGridLayOut.ChildNodes)//Loop all the trees
            {
                string treeNodeName = xDocForm.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                //Go the tree node and pick up the columns node.
                XmlNode nodeTreeColumns = xDocForm.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
                strPageControls = FindPageColumns(nodeTreeColumns, null, string.Empty);

                //XmlNode nodeBranches = nodeTree.SelectSingleNode("Branches");
                XmlNode nodeBranches = xDocForm.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
                if (nodeBranches != null)
                {
                    //int grdVwCntr = 0;
                    foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)//Loop all the branches of the current tree.
                    {
                        string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                        XmlAttribute attControlType = nodeBranch.Attributes["ControlType"];
                        if (attControlType != null && attControlType.Value == "GView")
                        {
                            //Add the GridView exclusively.
                            //string controlType = attControlType.Value;
                            //GridView grdVw = (GridView)container.FindControl("grdVw" + branchNodeName);
                            //if (grdVw != null)
                            //{
                            XmlNode nodeColumns = xDocForm.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                            string strCurrentBranchControls = FindPageColumns(nodeColumns, branchNodeName, attControlType.Value);
                            strPageControls = strPageControls + ">" + strCurrentBranchControls;
                            //}
                        }
                        else
                        {
                            //Rest of the Branch Controls.
                            XmlNode nodeColumns = xDocForm.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                            string strCurrentBranchControls = FindPageColumns(nodeColumns, branchNodeName, string.Empty);
                            //Add the above Branch controls to the Master Page Controls Container.
                            strPageControls = strPageControls + ">" + strCurrentBranchControls;
                        }
                    }
                }
            }
            return strPageControls;
        }

        internal static string GetGridViewNodeXML(string pageNo, string sortColumn, string sortOrder, string defaultPageSize)
        {
            return @"<Pagenumber>" + pageNo + "</Pagenumber><Pagesize>"
                + defaultPageSize + "</Pagesize><Sortcolumn>" + sortColumn
                + "</Sortcolumn><Sortorder>" + sortOrder + "</Sortorder>";
        }


        #region Convert Number To Decimal
        public string SpellDecimal(decimal number)
        {
            string[] digit = { "", "one", "two", "three", "four", "five", "six",
            "seven", "eight", "nine", "ten", "eleven", "twelve",
            "thirteen", "fourteen", "fifteen", "sixteen",
            "seventeen", "eighteen", "nineteen"  };

            string[] baseten ={
            "", "", "twenty", "thirty", "fourty", "fifty",
            "sixty", "seventy", "eighty", "ninety"  };

            string[] expo ={
            "", "thousand", "million", "billion", "trillion",
            "quadrillion", "quintillion"  };

            if (number == Decimal.Zero)
                return "zero";

            decimal n = Decimal.Truncate(number);

            decimal cents = Decimal.Truncate((number - n) * 100);

            StringBuilder sb = new StringBuilder();

            int thousands = 0;

            decimal power = 1;

            if (n < 0)
            {
                sb.Append("minus ");
                n = -n;
            }

            for (decimal i = n; i >= 1000; i /= 1000)
            {
                power *= 1000;
                thousands++;
            }

            bool sep = false;

            for (decimal i = n; thousands >= 0; i %= power, thousands--, power /= 1000)
            {
                int j = (int)(i / power);
                int k = j % 100;
                int hundreds = j / 100;
                int tens = j % 100 / 10;
                int ones = j % 10;

                if (j == 0)
                    continue;
                if (hundreds > 0)
                {
                    if (sep)
                        sb.Append(", ");
                    sb.Append(digit[hundreds]);
                    sb.Append(" hundred");
                    sep = true;
                }
                if (k != 0)
                {
                    if (sep)
                    {
                        sb.Append(" and ");
                        sep = false;
                    }
                    if (k < 20)
                        sb.Append(digit[k]);
                    else
                    {
                        sb.Append(baseten[tens]);
                        if (ones > 0)
                        {
                            sb.Append("-");
                            sb.Append(digit[ones]);
                        }
                    }
                }
                if (thousands > 0)
                {
                    sb.Append(" ");
                    sb.Append(expo[thousands]);
                    sep = true;
                }
            }
            /*sb.Append(" and ");
            if (cents < 10) sb.Append("0");
            sb.Append(cents);
            sb.Append("/100");*/
            return sb.ToString();
        }
        #endregion
    }
}