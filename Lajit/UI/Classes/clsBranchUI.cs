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

namespace LAjitDev
{
    public class clsBranchUI
    {
        private CommonUI m_objCommonUI;

        public CommonUI ObjCommonUI
        {
            get { return m_objCommonUI; }
            set { m_objCommonUI = value; }
        }

        #region Branch Methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pnlEntryForm"></param>
        /// <param name="branchNodeName"></param>
        /// <param name="arrControls"></param>
        /// <returns></returns>
        public string GetBranchNewRow(Control pnlEntryForm, string branchNodeName, ArrayList arrControls, string branchNodeType, string controlName)
        {
            string newRow = string.Empty;
            StringBuilder sbNewRowXML = new StringBuilder();
           
            XmlDocument xDocForm = new XmlDocument();
            xDocForm.LoadXml(ObjCommonUI.ButtonsUserControl.GVDataXml.ToString());

            //To verify BPAction based on autofill default values are created.
            string BPAction = string.Empty;

            if (ObjCommonUI.ButtonsUserControl!=null)
            {
                HiddenField hdnCurrAction = (HiddenField)ObjCommonUI.ButtonsUserControl.FindControl("hdnCurrAction");
                if (hdnCurrAction != null)
                {
                    BPAction = hdnCurrAction.Value;
                }
            }           

            //ImageButton imgBtnSubmit = (ImageButton)pnlEntryForm.FindControl("imgbtnSubmit");
            //if (imgBtnSubmit != null)
            //{
            //     BPAction = imgBtnSubmit.AlternateText;
            //}

            //Branch nodes 
            XmlNode nodeBranchColumns = xDocForm.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");           

            for (int count = 0; count <= arrControls.Count - 1; count++)
            {
                Control control = (Control)pnlEntryForm.FindControl(arrControls[count].ToString());               
                if (control != null)
                {
                    if (control is LAjitTextBox)
                    {
                        LAjitTextBox txtCurrent = ((LAjitTextBox)control);                       

                        //Islink is One Start                   
                        string xPath = "//Columns/Col[@Label='" + (txtCurrent).MapXML.Trim() + "']";
                        string isLink = string.Empty;
                        XmlNode nodebranchcols = nodeBranchColumns.SelectSingleNode(xPath);
                        if (nodebranchcols != null)
                        {
                            if (nodebranchcols.Attributes["IsLink"] != null)
                            {
                                isLink = nodeBranchColumns.SelectSingleNode(xPath).Attributes["IsLink"].Value;
                            }
                        }
                        if (isLink == string.Empty)//TxtBx
                        {
                            //if (txtCurrent.Text != string.Empty)
                            //{
                                //sbNewRowXML.Append(txtCurrent.MapXML.Trim() + "=\"" + txtCurrent.Text + "\" ");
                                sbNewRowXML.Append(txtCurrent.MapXML.Trim() + "=\"" + ObjCommonUI.CharactersToHtmlCodes(txtCurrent.Text.TrimEnd().TrimStart().ToString()) + "\" ");
                            //}
                        }
                        else
                        {
                            if (isLink == "0")//TxtBx
                            {
                                //if (txtCurrent.Text != string.Empty)
                                //{
                                    //sbNewRowXML.Append(txtCurrent.MapXML.Trim() + "=\"" + txtCurrent.Text + "\" ");
                                    sbNewRowXML.Append(txtCurrent.MapXML.Trim() + "=\"" + ObjCommonUI.CharactersToHtmlCodes(txtCurrent.Text.TrimEnd().TrimStart().ToString()) + "\" ");
                                //}
                            }
                            else if (isLink == "1")//DDL
                            {
                                string cacheName = Classes.AutoFill.GetLoggedInCompanyID()+txtCurrent.MapXML.ToString();
                                string cacheTrxID = string.Empty;
                                string cacheTrxType = string.Empty;
                                string cacheCreatedRow = string.Empty;

                                if (CommonUI.IsAutoFillCache)
                                {
                                    if ((System.Web.HttpContext.Current.Cache[cacheName] != null) && (System.Web.HttpContext.Current.Cache[cacheName] != string.Empty) && (txtCurrent.Text != string.Empty))
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
                                                drfoundrows = dsAutoFill.Tables[0].Select(txtCurrent.MapXML.Trim() + " like '" + txtCurrent.Text.Trim().Replace("'", "''") + "%'");

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
                                            //Step 4 trxid and trxtype is empty send text only. 
                                            //cacheCreatedRow = txtCurrent.MapXML.Trim() + "=\"" + ObjCommonUI.CharactersToHtmlCodes(txtCurrent.Text) + "\"  ";
                                            // No need to create attribute  both trxid and trxtype are empty. we are removing these items at the end. on 04-11-03
                                            // ADD operation sending both trxid and trxtype as empty and MODIFY case trxtype and trxtype are removed.
                                            cacheCreatedRow = txtCurrent.MapXML.Trim() + "_TrxID=\"\" "
                                                        + txtCurrent.MapXML.Trim() + "_TrxType=\"\"  "
                                                        + txtCurrent.MapXML.Trim() + "=\"\"  ";

                                        }

                                        if (cacheCreatedRow != string.Empty)
                                        {
                                            sbNewRowXML.Append(cacheCreatedRow);
                                        }
                                    }
                                }
                                else
                                {
                                    //NO CACHE
                                    string[] autofillTrxIDS = txtCurrent.Text.ToString().Split('~');
                                    if (autofillTrxIDS.Length == 2)
                                    {
                                        cacheCreatedRow = txtCurrent.MapXML.Trim() + "_TrxID=\"" + autofillTrxIDS[0].ToString() + "\"  "
                                                     + txtCurrent.MapXML.Trim() + "_TrxType=\"" + autofillTrxIDS[1].ToString() + "\"  ";
                                        sbNewRowXML.Append(cacheCreatedRow);
                                    }
                                }
                                //If the data is empty and isrequired take values from cache.
                                if(cacheCreatedRow==string.Empty)
                                {
                                    if ((nodeBranchColumns.SelectSingleNode(xPath).Attributes["IsRequired"].Value == "1")&& (BPAction!="Find"))
                                    {
                                        //Preselect default  value 
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
                                                    txtCurrent.Text = drfoundrows[0][txtCurrent.MapXML.Trim()].ToString();
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
                                            sbNewRowXML.Append(cacheCreatedRow);
                                        }
                                    } 
                                }
                                //Modify Case and txtCurrent is empty
                                if ((BPAction.ToUpper() == "MODIFY") && (txtCurrent.Text == string.Empty))
                                {
                                    cacheCreatedRow = txtCurrent.MapXML.Trim() + "_TrxID=\""  + "\"  "
                                                           + txtCurrent.MapXML.Trim() + "_TrxType=\""   + "\"  ";
                                    sbNewRowXML.Append(cacheCreatedRow);
                                }
                            }

                        }
                        //Islink is One End
                    }
                    else if (control is LAjitDropDownList)
                    {
                        LAjitDropDownList ddlCurrent = (LAjitDropDownList)control;
                        string newTrxIDType = string.Empty;
                        string ddlRow = string.Empty;
                        bool setvalue=false;
                        if (ddlCurrent.Attributes["MapPreviousSelItem"] != null && ddlCurrent.Attributes["MapPreviousSelItem"] != string.Empty && ddlCurrent.Attributes["MapPreviousSelItem"] != ddlCurrent.Items[0].Text)
                        {
                            if (controlName != string.Empty)
                            {
                                if (ddlCurrent.ID.Trim().Contains(controlName))
                                {
                                    setvalue = true;
                                }
                            }
                        }

                        if(setvalue)   
                        {
                            newTrxIDType = ((LAjitDropDownList)control).Items.FindByText(ddlCurrent.Attributes["MapPreviousSelItem"]).Value;
                        }
                        else
                        {
                           newTrxIDType = ddlCurrent.SelectedValue.Trim(); 
                        }

                        if (newTrxIDType.Length == 0)
                        {
                            continue;
                        }
                        string[] strarr = newTrxIDType.Split('~');
                        string trxID = strarr[0].ToString();
                        string trxType = strarr[1].ToString();

                        if (trxID != string.Empty && trxType != string.Empty)
                        {
                            if (trxID == "-1")
                            {
                                ddlRow = ddlCurrent.MapXML.Trim() + "_TrxID=\"" + string.Empty + "\"  "
                                        + ddlCurrent.MapXML.Trim() + "_TrxType=\"" + string.Empty + "\"  ";
                                // + ddlCurrent.MapXML.Trim() + "=\"" + string.Empty + "\"  ";
                            }
                            else
                            {
                                ddlRow = ddlCurrent.MapXML.Trim() + "_TrxID=\"" + trxID + "\"  "
                                        + ddlCurrent.MapXML.Trim() + "_TrxType=\"" + trxType + "\"  ";
                                //  + ddlCurrent.MapXML.Trim() + "=\"" + ddlCurrent.Attributes["MapPreviousSelItem"] + "\"  ";
                            }
                        }
                        else
                        {
                            //Appending only text when TrxId and TrxType are empty
                            if (trxID == "-1")
                            {
                                ddlRow = ddlCurrent.MapXML.Trim() + "=\"" + string.Empty + "\"  ";
                            }
                            else
                            {
                                if(setvalue)   
                                {
                                    ddlRow = ddlCurrent.MapXML.Trim() + "=\"" + ObjCommonUI.CharactersToHtmlCodes(ddlCurrent.Attributes["MapPreviousSelItem"].TrimEnd().TrimStart().ToString()) + "\" "; //ddlCurrent.Attributes["MapPreviousSelItem"] + "\"  ";
                                }
                                else
                                {
                                    ddlRow = ddlCurrent.MapXML.Trim() + "=\"" + ObjCommonUI.CharactersToHtmlCodes(ddlCurrent.SelectedItem.Text.TrimEnd().TrimStart().ToString()) + "\" ";
                                }
                            }
                        }
                        if (ddlRow != string.Empty)
                        {
                            sbNewRowXML.Append(ddlRow);
                        }
                    }                    
                    else if (control is LAjitCheckBox)
                    {
                        LAjitCheckBox chkCurrent = (LAjitCheckBox)control;
                        if (chkCurrent.Checked)
                        {
                            //True
                            sbNewRowXML.Append(chkCurrent.MapXML.Trim() + "=\"1\" ");
                        }
                        else
                        {   //False
                            sbNewRowXML.Append(chkCurrent.MapXML.Trim() + "=\"0\" ");
                        }
                    }
                }
            }
            return sbNewRowXML.ToString();
        }

        public void ClearBranchControls(Control pnlEntryForm, string branchNodeName, ArrayList arrControls)
        {
            for (int count = 0; count <= arrControls.Count - 1; count++)
            {
                Control ctrl = (Control)pnlEntryForm.FindControl(arrControls[count].ToString());
                if (ctrl != null)
                {

                    if (ctrl is LAjitTextBox)
                    {
                        ((LAjitTextBox)ctrl).Text = string.Empty;
                    }
                    else if (ctrl is LAjitDropDownList)
                    {
                        if (((LAjitDropDownList)ctrl).Items.Count > 0)
                        {
                            ((LAjitDropDownList)ctrl).SelectedIndex = -1;
                        }
                    }

                    else if (ctrl is LAjitCheckBox)
                    {
                        if (((LAjitCheckBox)ctrl).Checked)
                        {
                            ((LAjitCheckBox)ctrl).Checked = false;
                        }
                    }
                    else if (ctrl is HtmlTableRow)
                    {
                        foreach (Control contrl in ctrl.Controls)
                        {
                            ClearBranchControls(contrl,branchNodeName, arrControls);
                        }
                    }
                    //else if (ctrl is LAjitListBox)
                    //{
                    //    ((LAjitListBox)ctrl).ClearSelection();
                    //}
                    //else if (ctrl is GridView)
                    //{
                    //    GridView grdVwCurrent = (GridView)ctrl;
                    //    DataSet ds = (DataSet)grdVwCurrent.DataSource;
                    //    ds.Tables[0].Rows.Clear();
                    //    int blockSize = Convert.ToInt32(ConfigurationManager.AppSettings["AddBlockSize"]);
                    //    for (int i = 0; i < blockSize; i++)
                    //    {
                    //        ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                    //    }
                    //    grdVwCurrent.DataSource = ds;
                    //    grdVwCurrent.DataBind();
                    //}
                }
            }

        }

        /// </summary>
        /// <param name="Status"></param>
        /// <param name="Control"></param>
        /// <returns></returns>
        public void FillBranchUI(Control pnlEntryForm, XmlNode xNode, string branchNodeName, ArrayList arrControls)
        {
            for (int count = 0; count <= arrControls.Count - 1; count++)
            {
                Control ctrl = (Control)pnlEntryForm.FindControl(arrControls[count].ToString());

                if (ctrl != null)
                {
                    if (ctrl is LAjitTextBox)
                    {
                        if (xNode.Attributes[((LAjitTextBox)ctrl).MapXML] != null)
                        {
                            ((LAjitTextBox)ctrl).Text = xNode.Attributes[((LAjitTextBox)ctrl).MapXML].Value;
                        }
                    }
                    else if (ctrl is LAjitDropDownList)
                    {
                        string dataValueField = string.Empty;
                        if ((xNode.Attributes[((LAjitDropDownList)ctrl).MapXML.Trim() + "_TrxID"] != null) && (xNode.Attributes[((LAjitDropDownList)ctrl).MapXML.Trim() + "_TrxType"] != null))
                        {
                            dataValueField = xNode.Attributes[((LAjitDropDownList)ctrl).MapXML.Trim() + "_TrxID"].Value + '~' + xNode.Attributes[((LAjitDropDownList)ctrl).MapXML.Trim() + "_TrxType"].Value;
                            if ((xNode.Attributes[((LAjitDropDownList)ctrl).MapXML.Trim() + "_TrxID"].Value != string.Empty) && (xNode.Attributes[((LAjitDropDownList)ctrl).MapXML.Trim() + "_TrxType"].Value != string.Empty))
                            {
                                ((LAjitDropDownList)ctrl).SelectedIndex = ((LAjitDropDownList)ctrl).Items.IndexOf(((LAjitDropDownList)ctrl).Items.FindByValue(dataValueField));
                            }
                        }
                    }
                    else if (ctrl is LAjitLabel)
                    {
                        if (((LAjitLabel)ctrl).ID.Contains("_Value"))
                        {
                            if (xNode.Attributes[((LAjitLabel)ctrl).MapXML] != null)
                            {
                                ((LAjitLabel)ctrl).Text = xNode.Attributes[((LAjitLabel)ctrl).MapXML].Value;
                            }

                            ////Label visable based on IsApproved status
                            //if (xNode.Attributes["IsApproved"] != null)
                            //{
                            //    if (xNode.Attributes["IsApproved"].Value == "0")
                            //    {
                            //        ((LAjitLabel)ctrl).Visible = true;
                            //    }
                            //    else
                            //    {
                            //        ((LAjitLabel)ctrl).Visible = false;
                            //    }
                            //}

                        }
                    }
                    //else if (ctrl is HtmlTableRow)
                    //{
                    //    foreach (Control contrl in ctrl.Controls)
                    //    {
                    //        EnableDisableAndFillUI(status, contrl, xNode, treeNodeName, nodeColumns);
                    //    }
                    //}
                    //else if (ctrl is LAjitRequiredFieldValidator)
                    //{
                    //    if (nodeColumns != null)
                    //    {
                    //        string xPath = "//Columns/Col[@Label='" + ((LAjitRequiredFieldValidator)ctrl).MapXML + "']";
                    //        string isRequired = nodeColumns.SelectSingleNode(xPath).Attributes["IsRequired"].Value;
                    //        if (isRequired != "0")
                    //        {
                    //            ((LAjitRequiredFieldValidator)ctrl).Enabled = true;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        //do nothing
                    //    }
                    //}
                    //else if (ctrl is LAjitRegularExpressionValidator)
                    //{
                    //    if (nodeColumns != null)
                    //    {
                    //        string xPath = "//Columns/Col[@Label='" + ((LAjitRegularExpressionValidator)ctrl).MapXML + "']";
                    //        string isNumeric = nodeColumns.SelectSingleNode(xPath).Attributes["IsNumeric"].Value;
                    //        if (isNumeric != "0")
                    //        {
                    //            ((LAjitRegularExpressionValidator)ctrl).Enabled = true;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        //do nothing
                    //    }
                    //}
                    else if (ctrl is LAjitCheckBox)
                    {
                        if (xNode.Attributes[((LAjitCheckBox)ctrl).MapXML] != null)
                        {
                            if (xNode.Attributes[((LAjitCheckBox)ctrl).MapXML].Value == "1")
                            {
                                ((LAjitCheckBox)ctrl).Checked = true;
                            }
                            else
                            {
                                ((LAjitCheckBox)ctrl).Checked = false;
                            }
                        }
                    }
                    else if (ctrl is LAjitLinkButton)
                    {
                        if (xNode.Attributes[((LAjitLinkButton)ctrl).MapXML] != null)
                        {
                            ((LAjitLinkButton)ctrl).ToolTip = xNode.Attributes[((LAjitLinkButton)ctrl).MapXML].Value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Common Update method for Add, Modify, Delete, Find to update branch ViewStateXML.
        /// </summary>
        /// <param name="Action">Current Action.</param>
        /// <param name="ReqXMLTrxID">TrxID For Delete operation.</param>
        /// <param name="bpout">OutXML from the respective Action.</param>
        public void UpdateBranchViewStateXml(String Action, string ReqXMLTrxID, string bpout)
        {
            //Modifying the Viewstate RetXML which is used for GV Binding 
            XmlDocument returnXML = new XmlDocument();
            returnXML.LoadXml(bpout);

            ////success messge
            XmlNode nodeMsg = returnXML.SelectSingleNode("Root/bpeout/MessageInfo/Message");
            XmlNode nodeMsgStatus = returnXML.SelectSingleNode("Root/bpeout/MessageInfo/Status");

            if (nodeMsgStatus.InnerText == "Success")
            {
                XmlDocument Xdoc = new XmlDocument();
                Xdoc.LoadXml(ObjCommonUI.ButtonsUserControl.GVDataXml.ToString());

                //Branch nodes 
                XmlNode nodeBranches = Xdoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");

                if (nodeBranches != null)
                {

                    XmlNode nodeResParent = returnXML.SelectSingleNode("//" + ObjCommonUI.ButtonsUserControl.TreeNode + "/RowList/Rows[1]");
                    string parentTrxID = string.Empty;
                    //string parentTrxType=string.Empty;
                    if (nodeResParent != null)
                    {
                        parentTrxID = nodeResParent.Attributes["TrxID"].Value;
                        //parentTrxType = nodeResParent.Attributes["TrxType"].Value;
                    }
                    else
                    {
                        parentTrxID = ReqXMLTrxID;
                    }

                    foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                    {
                        if (nodeBranch.Attributes["ControlType"] == null)
                        {
                            string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;

                            //Creating branch node if not present in GVDataXMl
                            if (Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/RowList") == null)
                            {

                                //Creating the Row List node
                                XmlNode nodeBranchRowList = Xdoc.CreateNode(XmlNodeType.Element, "RowList", null);

                                //Appending Row List Node
                                Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName).AppendChild(nodeBranchRowList);
                            }
                            if (Action.ToUpper().Trim() == "FIND" || Action.ToUpper().Trim() == "PREVIOUSPAGELOAD")
                            {
                                XmlNode nodeRowList = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/RowList");
                                nodeRowList.RemoveAll();
                                XmlNode nodeResRowList = returnXML.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/RowList");
                                if (nodeResRowList != null)
                                {
                                    nodeRowList.InnerXml += nodeResRowList.InnerXml;
                                }

                                //Updating the Total Page Size
                                XmlNode nodeNewRowList = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/RowList");
                                if (nodeNewRowList != null)
                                {
                                    XmlNode nodeGridResults = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/Gridresults");
                                    nodeGridResults.RemoveAll();
                                    XmlNode nodeResGridResults = returnXML.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/Gridresults");
                                    nodeGridResults.InnerXml += nodeResGridResults.InnerXml;
                                }
                                ObjCommonUI.ButtonsUserControl.GVDataXml = Xdoc.OuterXml;
                            }
                            else
                            {
                                XmlNodeList nodeResRows = returnXML.SelectNodes("Root/bpeout/FormControls/" + branchNodeName + "/RowList/Rows[@" + ObjCommonUI.ButtonsUserControl.TreeNode + "_TrxID = '" + parentTrxID + "']");
                                if (nodeResRows != null)
                                {
                                    foreach (XmlNode nodeResRow in nodeResRows)
                                    {
                                        //TrxID of the Updated record
                                        string trxID = string.Empty;
                                        if (Action.ToUpper().Trim() == "DELETE")
                                        {
                                            trxID = ReqXMLTrxID;
                                        }
                                        else if (Action.ToUpper().Trim() == "SAVE")
                                        {
                                            //Do nothing
                                        }
                                        else
                                        {
                                            trxID = nodeResRow.Attributes["TrxID"].Value;
                                        }

                                        if (trxID != string.Empty)
                                        {
                                            XmlNode nodeRowList = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/RowList");
                                            if ((Action.ToUpper().Trim() == "DELETE") || (Action.ToUpper().Trim() == "MODIFY") || (Action.ToUpper().Trim() == "SOXAPPROVAL"))
                                            {
                                                string xPath = "Rows[@TrxID='" + trxID + "' and @" + ObjCommonUI.ButtonsUserControl.TreeNode + "_TrxID='" + parentTrxID + "']";
                                                XmlNode nodeRow = nodeRowList.SelectSingleNode(xPath);
                                                if (nodeRow != null)
                                                {
                                                    nodeRowList.RemoveChild(nodeRow);
                                                }

                                                if (Action.ToUpper().Trim() == "MODIFY" || Action.ToUpper().Trim() == "SOXAPPROVAL")
                                                {
                                                    nodeRowList.InnerXml += nodeResRow.OuterXml;
                                                }
                                                else if (Action.ToUpper().Trim() == "DELETE")
                                                {
                                                    //Updating total Page Size
                                                    XmlNode nodeGridResults = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/Gridresults");
                                                    int newPageSize = Convert.ToInt32(nodeGridResults.SelectSingleNode("Totalpage").Attributes["Pagesize"].Value) - 1;
                                                    nodeGridResults.SelectSingleNode("Totalpage").Attributes["Pagesize"].Value = Convert.ToString(newPageSize);
                                                    int newCurrentPageSize = Convert.ToInt32(nodeGridResults.SelectSingleNode("Currentpage").Attributes["Pagesize"].Value) - 1;
                                                    nodeGridResults.SelectSingleNode("Currentpage").Attributes["Pagesize"].Value = Convert.ToString(newCurrentPageSize);
                                                }
                                            }
                                            else if (Action.ToUpper().Trim() == "ADD")
                                            {
                                                //Creating the AddedRow node
                                                nodeRowList.InnerXml += nodeResRow.OuterXml;
                                                ////Updating total Page Size
                                                XmlNode nodeGridResults = Xdoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/Gridresults");
                                                int newPageSize = Convert.ToInt32(nodeGridResults.SelectSingleNode("Totalpage").Attributes["Pagesize"].Value) + 1;
                                                nodeGridResults.SelectSingleNode("Totalpage").Attributes["Pagesize"].Value = Convert.ToString(newPageSize);
                                                int newCurrentPageSize = Convert.ToInt32(nodeGridResults.SelectSingleNode("Currentpage").Attributes["Pagesize"].Value) + 1;
                                                nodeGridResults.SelectSingleNode("Currentpage").Attributes["Pagesize"].Value = Convert.ToString(newCurrentPageSize);
                                            }
                                        }
                                    }
                                    //Updating the GV XML
                                    ObjCommonUI.ButtonsUserControl.GVDataXml = Xdoc.OuterXml;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetBranchXML(string GVDataXml, string parentTrxID)
        {
            if (GVDataXml != string.Empty)
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(GVDataXml.ToString());

                bool branchExists = false;
                //Branch nodes 
                XmlNode nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");

                if (nodeBranches != null)
                {
                    XmlDocument branchDoc = new XmlDocument();
                    XmlNode nodeRoot = branchDoc.CreateNode(XmlNodeType.Element, "Root", null);

                    foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                    {
                        if (nodeBranch.Attributes["ControlType"] == null)
                        {
                            branchExists = true;
                            string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;

                            //Checking if this branch exists in Result XML
                            XmlNode nodeBranchRowlist = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/RowList");
                            if (nodeBranchRowlist != null)
                            {
                                XmlNodeList nodeResRows = nodeBranchRowlist.SelectNodes("Rows[@" + ObjCommonUI.ButtonsUserControl.TreeNode + "_TrxID = '" + parentTrxID + "']");
                                if (nodeResRows != null)
                                {
                                    XmlNode nodeNewBranch = branchDoc.CreateNode(XmlNodeType.Element, branchNodeName, null);
                                    nodeRoot.AppendChild(nodeNewBranch);
                                    XmlNode nodeNewBranchRowlist = branchDoc.CreateNode(XmlNodeType.Element, "RowList", null);
                                    nodeNewBranch.AppendChild(nodeNewBranchRowlist);
                                    foreach (XmlNode nodeResRow in nodeResRows)
                                    {
                                        nodeNewBranchRowlist.InnerXml += nodeResRow.OuterXml;
                                    }
                                }
                            }
                        }
                    }
                    if (branchExists)
                    {
                        ObjCommonUI.ButtonsUserControl.BranchXML = nodeRoot.OuterXml;
                        //Setting BranchXML in hdnBranchXML
                        HiddenField hdnBranchXML = (HiddenField)ObjCommonUI.ButtonsUserControl.FindControl("hdnBranchXML");
                        if (hdnBranchXML != null)
                        {
                            hdnBranchXML.Value = nodeRoot.OuterXml;
                        }

                        //Setting BranchColumns in hdnBranchColsXML
                        //setHiddenBranchXML(xDoc);
                    }
                }
            }
        }

        //Setting BranchColumns in hdnBranchColsXML
        public void setHiddenBranchXML(XmlDocument xDoc)
        {
            if (xDoc != null)
            {
                //Branch nodes 
                XmlNode nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");

                if (nodeBranches != null)
                {
                    //Setting BranchColumns in hdnBranchColsXML
                    XmlDocument branchColDoc = new XmlDocument();
                    XmlNode nodeColRoot = branchColDoc.CreateNode(XmlNodeType.Element, "Root", null);
                    foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                    {
                        if (nodeBranch.Attributes["ControlType"] == null)
                        {
                            string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                            //XmlNode nodebranchNode = branchColDoc.CreateNode(XmlNodeType.Element, branchNodeName, null);
                            //Checking if this branch exists in Result XML
                            XmlNode nodeBranchCols = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                            if (nodeBranchCols != null)
                            {                               
                                XmlNodeList nodeResCols = nodeBranchCols.SelectNodes("Col");
                                if (nodeResCols != null)//If Cols exists 
                                {
                                    XmlNode nodeNewBranch = branchColDoc.CreateNode(XmlNodeType.Element, branchNodeName, null);
                                    nodeColRoot.AppendChild(nodeNewBranch);
                                    XmlNode nodeNewBranchColumnslist = branchColDoc.CreateNode(XmlNodeType.Element, "Columns", null);
                                    nodeNewBranch.AppendChild(nodeNewBranchColumnslist);
                                    //Saving only Label, Default and ControlType attribues in the cols
                                    foreach (XmlNode nodeResCol in nodeResCols)
                                    {
                                       // nodeNewBranchColumnslist.InnerXml += nodeResCol.OuterXml;
                                        XmlNode nodeNewBranchCol = branchColDoc.CreateNode(XmlNodeType.Element, "Col", null);
                                       
                                        //Label Attribute
                                        if (nodeResCol.Attributes["Label"] != null)
                                        {
                                            XmlAttribute attLabel = branchColDoc.CreateAttribute("Label");
                                            attLabel.Value = nodeResCol.Attributes["Label"].Value;
                                            nodeNewBranchCol.Attributes.Append(attLabel);
                                        }
                                        //Default Attribute
                                        if (nodeResCol.Attributes["Default"] != null)
                                        {
                                            XmlAttribute attDefault = branchColDoc.CreateAttribute("Default");
                                            attDefault.Value = nodeResCol.Attributes["Default"].Value;
                                            nodeNewBranchCol.Attributes.Append(attDefault);
                                        }
                                        //ControlType Attribute
                                        if (nodeResCol.Attributes["ControlType"] != null)
                                        {
                                            XmlAttribute attControlType = branchColDoc.CreateAttribute("ControlType");
                                            attControlType.Value = nodeResCol.Attributes["ControlType"].Value;
                                            nodeNewBranchCol.Attributes.Append(attControlType);
                                        }
                                        nodeNewBranchColumnslist.AppendChild(nodeNewBranchCol);
                                    }
                                }                                
                            }
                        }
                    }
                    HiddenField hdnBranchColsXML = (HiddenField)ObjCommonUI.ButtonsUserControl.FindControl("hdnBranchColsXML");
                    if (hdnBranchColsXML != null)
                    {
                        hdnBranchColsXML.Value = nodeColRoot.OuterXml;
                    }
                }
            }
        }

        /// <summary>
        /// Generates the branch Req XML for branch controls with DDL case
        /// </summary>
        /// <param name="branchNodeName">branchNodeName.</param>
        /// <param name="controlName">DDL control ID.</param>
        /// <param name="dataEntered">Is Data entered in branch controls</param>
        /// <param name="pnlEntryForm">panel containing the controls</param>
        /// 
        public void saveBranch(string branchNodeName, string controlName, bool dataEntered, Panel pnlEntryForm, string action)
        {

            //Retrieving list of branch controls
            ArrayList arrCtrls = GetBranchControls(branchNodeName);

            string control = "ddl" + controlName + "_" + branchNodeName;
            LAjitDropDownList ddlBranchDropdown = (LAjitDropDownList)pnlEntryForm.FindControl(control);
            if (ddlBranchDropdown == null)
            {
                //No dropdown found

            }
            else
            {
                XmlDocument xdoc = new XmlDocument();
                if (ObjCommonUI.ButtonsUserControl.BranchXML != string.Empty)
                {
                    xdoc.LoadXml(ObjCommonUI.ButtonsUserControl.BranchXML.ToString());
                }
                else
                {
                    xdoc = null;
                }

                string previousSelValue = string.Empty;
                bool save = true;
                if (dataEntered)
                {
                    if (xdoc != null)
                    {
                        XmlNode nodeBranchRowList = xdoc.SelectSingleNode("Root/" + branchNodeName + "/RowList");
                       
                        if (nodeBranchRowList != null)
                        {
                            string value= ddlBranchDropdown.Items.FindByText(ddlBranchDropdown.Attributes["MapPreviousSelItem"]).Value.ToString();

                            //dataValueField = nodeSelectedRow.Attributes[((LAjitDropDownList)ctrl).MapXML.Trim() + "_TrxID"].Value + '~' + nodeSelectedRow.Attributes[((LAjitDropDownList)ctrl).MapXML.Trim() + "_TrxType"].Value;
                            string[] strarr = value.Split('~');
                            string xPath = "Rows[@" + controlName + "_TrxID ='" + strarr[0].ToString() + "' and @" + controlName + "_TrxType ='" + strarr[1].ToString() + "']";
                            XmlNode nodeRow = nodeBranchRowList.SelectSingleNode(xPath);                                                                                

                            if (nodeRow != null)
                            {
                                save = false;
                                //Modifying the values if exists
                                string newRow = "<Rows " + GetBranchNewRow(pnlEntryForm, branchNodeName, arrCtrls, ddlBranchDropdown.Attributes["MapPreviousSelItem"], controlName) + " />";
                                XmlDocument xDc = new XmlDocument();
                                xDc.LoadXml(newRow);
                                XmlNode xNodeMod = xDc.SelectSingleNode("Rows");

                                //Check for IsPrimary
                                if (xNodeMod.Attributes["IsPrimary"] != null)
                                {
                                    if (xNodeMod.Attributes["IsPrimary"].Value == "1")
                                    {
                                        XmlNode nodePrimaryRow = nodeBranchRowList.SelectSingleNode("Rows[@IsPrimary='1']");
                                        if (nodePrimaryRow != null)
                                        {
                                            nodePrimaryRow.Attributes["IsPrimary"].Value = "0";
                                            ObjCommonUI.ButtonsUserControl.BranchXML = xdoc.OuterXml;
                                        }
                                    }
                                }

                                //

                                for (int i = 0; i < xNodeMod.Attributes.Count; i++)
                                {
                                    if (nodeRow.Attributes[xNodeMod.Attributes[i].Name.ToString()] != null)
                                    {
                                        nodeRow.Attributes[xNodeMod.Attributes[i].Name.ToString()].Value = xNodeMod.Attributes[xNodeMod.Attributes[i].Name.ToString()].Value;
                                    }
                                    else
                                    {
                                        XmlAttribute attrNew = xdoc.CreateAttribute(xNodeMod.Attributes[i].Name.ToString());
                                        attrNew.Value = xNodeMod.Attributes[xNodeMod.Attributes[i].Name.ToString()].Value;
                                        nodeRow.Attributes.Append(attrNew);
                                    }
                                }
                                ObjCommonUI.ButtonsUserControl.BranchXML = xdoc.OuterXml;
                            }
                        }
                    }

                    if (save)
                    {
                        string newRow = "<Rows " + GetBranchNewRow(pnlEntryForm, branchNodeName, arrCtrls, ddlBranchDropdown.Attributes["MapPreviousSelItem"], controlName) + " />";
                        if (xdoc == null)
                        {
                            ObjCommonUI.ButtonsUserControl.BranchXML = "<Root><" + branchNodeName + "><RowList>" + newRow + "</RowList></" + branchNodeName + "></Root>";
                        }
                        else
                        {
                            if (xdoc.SelectSingleNode("Root/" + branchNodeName + "/RowList") == null)
                            {
                                xdoc.SelectSingleNode("Root").InnerXml += "<" + branchNodeName + "><RowList>" + newRow + "</RowList></" + branchNodeName + ">";
                                ObjCommonUI.ButtonsUserControl.BranchXML = xdoc.OuterXml;
                            }
                            else
                            {
                                //Check for IsPrimary
                                XmlDocument xDc = new XmlDocument();
                                xDc.LoadXml(newRow);
                                XmlNode xNodeMod = xDc.SelectSingleNode("Rows");
                                if (xNodeMod.Attributes["IsPrimary"] != null)
                                {
                                    if (xNodeMod.Attributes["IsPrimary"].Value == "1")
                                    {
                                        XmlNode nodePrimaryRow = xdoc.SelectSingleNode("Root/" + branchNodeName + "/RowList").SelectSingleNode("Rows[@IsPrimary='1']");
                                        if (nodePrimaryRow != null)
                                        {
                                            nodePrimaryRow.Attributes["IsPrimary"].Value = "0";
                                            ObjCommonUI.ButtonsUserControl.BranchXML = xdoc.OuterXml;
                                        }
                                    }
                                }
                                //

                                xdoc.SelectSingleNode("Root/" + branchNodeName + "/RowList").InnerXml += newRow;
                                ObjCommonUI.ButtonsUserControl.BranchXML = xdoc.OuterXml;
                            }
                        }
                        string mapPreviousSelitem = ddlBranchDropdown.Attributes["MapPreviousSelItem"].ToString().Trim();
                        int index = ddlBranchDropdown.Items.IndexOf(ddlBranchDropdown.Items.FindByText(mapPreviousSelitem));

                        previousSelValue = ddlBranchDropdown.SelectedItem.Text.ToString();
                        ddlBranchDropdown.Attributes["MapPreviousSelItem"] = previousSelValue;
                        arrCtrls.Remove(control);
                        if (index!=0 && dataEntered == true && action==string.Empty)
                        {
                            ClearBranchControls(pnlEntryForm, branchNodeName, arrCtrls);
                        }
                    }
                    else
                    {
                        string mapPreviousSelitem = ddlBranchDropdown.Attributes["MapPreviousSelItem"].ToString().Trim();
                        int index = ddlBranchDropdown.Items.IndexOf(ddlBranchDropdown.Items.FindByText(mapPreviousSelitem));

                        previousSelValue = ddlBranchDropdown.SelectedItem.Text.ToString();
                        ddlBranchDropdown.Attributes["MapPreviousSelItem"] = previousSelValue;
                        arrCtrls.Remove(control);
                        if (index != 0 && dataEntered == true && action == string.Empty)
                        {
                            ClearBranchControls(pnlEntryForm, branchNodeName, arrCtrls);
                        }
                    }
                }
                else
                {
                    if (xdoc != null)
                    {
                        XmlNode nodeBranchRowList = xdoc.SelectSingleNode("//" + branchNodeName + "/RowList");
                        if (nodeBranchRowList != null)
                        {
                            //XmlNode nodeRow = nodeBranchRowList.SelectSingleNode("Rows[@" + controlName + "='" + ddlBranchDropdown.Attributes["MapPreviousSelItem"] + "']");
                            string value = ddlBranchDropdown.Items.FindByText(ddlBranchDropdown.Attributes["MapPreviousSelItem"]).Value.ToString();

                            //dataValueField = nodeSelectedRow.Attributes[((LAjitDropDownList)ctrl).MapXML.Trim() + "_TrxID"].Value + '~' + nodeSelectedRow.Attributes[((LAjitDropDownList)ctrl).MapXML.Trim() + "_TrxType"].Value;
                            string[] strarr = value.Split('~');
                            string xPath = "Rows[@" + controlName + "_TrxID ='" + strarr[0].ToString() + "' and @" + controlName + "_TrxType ='" + strarr[1].ToString() + "']";
                            XmlNode nodeRow = nodeBranchRowList.SelectSingleNode(xPath); 

                            if (nodeRow != null)
                            {
                                nodeBranchRowList.RemoveChild(nodeRow);
                                ObjCommonUI.ButtonsUserControl.BranchXML = xdoc.OuterXml;
                                previousSelValue = ddlBranchDropdown.SelectedItem.Text.ToString();
                                ddlBranchDropdown.Attributes["MapPreviousSelItem"] = previousSelValue;
                            }
                            else
                            {
                                previousSelValue = ddlBranchDropdown.SelectedItem.Text.ToString();
                                ddlBranchDropdown.Attributes["MapPreviousSelItem"] = previousSelValue;
                            }
                        }
                        else
                        {
                            previousSelValue = ddlBranchDropdown.SelectedItem.Text.ToString();
                            ddlBranchDropdown.Attributes["MapPreviousSelItem"] = previousSelValue;
                        }
                    }
                    else
                    {
                        previousSelValue = ddlBranchDropdown.SelectedItem.Text.ToString();
                        ddlBranchDropdown.Attributes["MapPreviousSelItem"] = previousSelValue;
                    }
                }

                ////Filling BranchUI
                //if (ObjCommonUI.ButtonsUserControl.BranchXML != string.Empty)
                //{
                //    XmlDocument xbranchdoc = new XmlDocument();
                //    xbranchdoc.LoadXml(ObjCommonUI.ButtonsUserControl.BranchXML.ToString());
                //    XmlNode nodeBranchRowList = xbranchdoc.SelectSingleNode("Root/" + branchNodeName + "/RowList");
                //    if (nodeBranchRowList != null)
                //    {
                //        string newTrxIDType = string.Empty;
                //        newTrxIDType = ddlBranchDropdown.SelectedValue.Trim();
                //        if (newTrxIDType.Length != 0)
                //        {
                //            string[] strarr = newTrxIDType.Split('~');
                //            string trxID = strarr[0].ToString();
                //            string trxType = strarr[1].ToString();
                //            XmlNode nodeRow = nodeBranchRowList.SelectSingleNode("Rows[@" + controlName + "_TrxID='" + trxID + "' and @" + controlName + "_TrxType='" + trxType + "']");
                //            //nodeBranchRowList.SelectSingleNode("Rows[@" + controlName + "='" + ddlBranchDropdown.SelectedItem.ToString() + "']");                                                   
                //            if (nodeRow != null)
                //            {
                //                FillBranchUI(pnlEntryForm, nodeRow, branchNodeName, arrCtrls);
                //                previousSelValue = ddlBranchDropdown.SelectedItem.Text.ToString();
                //                ddlBranchDropdown.Attributes["MapPreviousSelItem"] = previousSelValue;
                //            }

                //        }
                //    }
                //}                
            }
        }

        //public void ClearBranchControls(Control pnlEntryForm, string branchNodeName, XmlDocument xDocBtnsGVDataXML)
        //{
        //    XmlNode nodeColumns = xDocBtnsGVDataXML.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");

        //    foreach (Control ctrl in pnlEntryForm.Controls)
        //    {
        //        //Control ctrl = (Control)pnlEntryForm.FindControl(arrControls[count].ToString());

        //        if (ctrl is LAjitTextBox)
        //        {
        //            if (((LAjitTextBox)ctrl).MapBranchNode != null)
        //            {
        //                if (((LAjitTextBox)ctrl).MapBranchNode.Trim() == branchNodeName)
        //                {
        //                    ((LAjitTextBox)ctrl).Text = string.Empty;
        //                }
        //            }
        //        }
        //        else if (ctrl is LAjitDropDownList)
        //        {
        //            if (((LAjitDropDownList)ctrl).Items.Count > 0)
        //            {
        //                if (((LAjitDropDownList)ctrl).MapBranchNode != null)
        //                {
        //                    if (((LAjitDropDownList)ctrl).MapBranchNode.Trim() == branchNodeName)
        //                    {
        //                        //((LAjitDropDownList)ctrl).SelectedIndex = -1;
        //                        //if (((LAjitDropDownList)ctrl).Items.Count > 0)
        //                        {
        //                            //((LAjitDropDownList)ctrl).SelectedIndex = 0;
        //                            XmlNode nodeDdl = nodeColumns.SelectSingleNode("Col [@Label='" + ((LAjitDropDownList)ctrl).MapXML.ToString() + "']");
        //                            if (nodeDdl != null)
        //                            {
        //                                LAjitDropDownList ddlCurrent = ((LAjitDropDownList)ctrl);
        //                                if (nodeDdl.Attributes["Default"] != null)
        //                                {
        //                                    //if (((LAjitDropDownList)ctrl).Items.Count > 0)
        //                                    {
        //                                        string dataValueField = string.Empty;
        //                                        string[] strarr = ((LAjitDropDownList)ctrl).Items[0].Value.Split('~');
        //                                        //Getting TrxType
        //                                        if (strarr[1].ToString() != string.Empty)
        //                                        {
        //                                            dataValueField = nodeDdl.Attributes["Default"].Value.Trim().ToString() + "~" + strarr[1].ToString();
        //                                        }
        //                                        if (dataValueField != string.Empty)
        //                                        {
        //                                            ((LAjitDropDownList)ctrl).SelectedIndex = ((LAjitDropDownList)ctrl).Items.IndexOf(((LAjitDropDownList)ctrl).Items.FindByValue(dataValueField));
        //                                            ddlCurrent.Attributes["MapPreviousSelItem"] = ddlCurrent.SelectedItem.Text;
        //                                        }

        //                                    }
        //                                }
        //                                else
        //                                {
                                           
        //                                    ((LAjitDropDownList)ctrl).SelectedIndex = 0;
        //                                    ddlCurrent.Attributes["MapPreviousSelItem"] = ddlCurrent.Items[0].Text;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        else if (ctrl is LAjitCheckBox)
        //        {
        //            if (((LAjitCheckBox)ctrl).Checked)
        //            {
        //                if (((LAjitCheckBox)ctrl).MapBranchNode != null)
        //                {
        //                    if (((LAjitCheckBox)ctrl).MapBranchNode.Trim() == branchNodeName)
        //                    {
        //                        ((LAjitCheckBox)ctrl).Checked = false;
        //                    }
        //                }
        //            }
        //        }
        //        else if (ctrl is HtmlTableRow)
        //        {
        //            foreach (Control contrl in ctrl.Controls)
        //            {
        //                ClearBranchControls(contrl, branchNodeName,xDocBtnsGVDataXML);
        //            }
        //        }
        //        //else if (ctrl is LAjitListBox)
        //        //{
        //        //    ((LAjitListBox)ctrl).ClearSelection();
        //        //}
        //        //else if (ctrl is GridView)
        //        //{
        //        //    GridView grdVwCurrent = (GridView)ctrl;
        //        //    DataSet ds = (DataSet)grdVwCurrent.DataSource;
        //        //    ds.Tables[0].Rows.Clear();
        //        //    int blockSize = Convert.ToInt32(ConfigurationManager.AppSettings["AddBlockSize"]);
        //        //    for (int i = 0; i < blockSize; i++)
        //        //    {
        //        //        ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
        //        //    }
        //        //    grdVwCurrent.DataSource = ds;
        //        //    grdVwCurrent.DataBind();
        //        //}
        //    }
        //}

        public ArrayList GetBranchControls(string branchNodeName)
        {
            //Loading GVdata XML
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(ObjCommonUI.ButtonsUserControl.GVDataXml.ToString());
            XmlNode nodeBranchColumns = xdoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");

            ArrayList arrCtrls = new ArrayList();
            if (nodeBranchColumns != null)
            {
                foreach (XmlNode nodeBranchcol in nodeBranchColumns.ChildNodes)
                {
                    string label = nodeBranchcol.Attributes["Label"].Value;
                    string controlType = nodeBranchcol.Attributes["ControlType"].Value;
                    string controlID = string.Empty;

                    switch (controlType.ToUpper().Trim())
                    {
                        case "TBOX":
                            {
                                controlID = "txt" + label + "_" + branchNodeName;
                                break;
                            }
                        case "DDL":
                            {
                                controlID = "ddl" + label + "_" + branchNodeName;
                                break;
                            }
                        case "CHECK":
                            {
                                controlID = "chk" + label + "_" + branchNodeName;
                                break;
                            }
                        case "CAL":
                            {
                                controlID = "txt" + label + "_" + branchNodeName;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                    arrCtrls.Add(controlID);
                }
            }
            return arrCtrls;
        }

        /// <summary>
        /// Generates the branch Req XML having only branch controls 
        /// </summary>
        /// <param name="branchNodeName">branchNodeName.</param>
        /// <param name="dataEntered">Is Data entered in branch controls</param>
        /// <param name="pnlEntryForm">panel containing the controls</param>
        /// 
        public void saveBranchUI(string branchNodeName, bool dataEntered, Panel pnlEntryForm)
        {

            //Retrieving list of branch controls
            ArrayList arrCtrls = GetBranchControls(branchNodeName);
            XmlDocument xdoc = new XmlDocument();
            if (ObjCommonUI.ButtonsUserControl.BranchXML != string.Empty)
            {
                xdoc.LoadXml(ObjCommonUI.ButtonsUserControl.BranchXML.ToString());
            }
            else
            {
                //xdoc = null;
                xdoc.LoadXml("<Root></Root>");
            }

            bool save = true;
            if (dataEntered)
            {
                if (xdoc != null)
                {
                    XmlNode nodeBranchRowList = xdoc.SelectSingleNode("Root/" + branchNodeName + "/RowList");
                   
                    if (nodeBranchRowList != null)
                    {
                        XmlNode nodeRow = nodeBranchRowList.SelectSingleNode("Rows");
                        if (nodeRow != null)
                        {
                            save = false;
                            //Modifying the values if exists
                            string newRow = "<Rows " + GetBranchNewRow(pnlEntryForm, branchNodeName, arrCtrls, string.Empty, string.Empty) + " />";
                            XmlDocument xDc = new XmlDocument();
                            xDc.LoadXml(newRow);
                            XmlNode xNodeMod = xDc.SelectSingleNode("Rows");

                            for (int i = 0; i < xNodeMod.Attributes.Count; i++)
                            {
                                if (nodeRow.Attributes[xNodeMod.Attributes[i].Name.ToString()] != null)
                                {
                                    nodeRow.Attributes[xNodeMod.Attributes[i].Name.ToString()].Value = xNodeMod.Attributes[xNodeMod.Attributes[i].Name.ToString()].Value;
                                }
                                else
                                {
                                    XmlAttribute attrNew = xdoc.CreateAttribute(xNodeMod.Attributes[i].Name.ToString());
                                    attrNew.Value = xNodeMod.Attributes[xNodeMod.Attributes[i].Name.ToString()].Value;
                                    nodeRow.Attributes.Append(attrNew);
                                }
                            }
                            //ObjCommonUI.ButtonsUserControl.BranchXML = xdoc.OuterXml;
                        }
                    }
                }

                if (save)
                {
                    string newRow = "<Rows " + GetBranchNewRow(pnlEntryForm, branchNodeName, arrCtrls, string.Empty, string.Empty) + " />";
                    if (xdoc == null)
                    {
                        ObjCommonUI.ButtonsUserControl.BranchXML = "<Root><" + branchNodeName + "><RowList>" + newRow + "</RowList></" + branchNodeName + "></Root>";
                    }
                    else
                    {
                        if (xdoc.SelectSingleNode("Root/" + branchNodeName + "/RowList") == null)
                        {
                            xdoc.SelectSingleNode("Root").InnerXml += "<" + branchNodeName + "><RowList>" + newRow + "</RowList></" + branchNodeName + ">";                            
                        }
                        else
                        {
                            xdoc.SelectSingleNode("Root/" + branchNodeName + "/RowList").InnerXml += newRow;                            
                        }
                        //ObjCommonUI.ButtonsUserControl.BranchXML = xdoc.OuterXml;
                    }
                    //ClearBranchControls(pnlEntryForm, branchNodeName, arrCtrls);
                }
                else
                {
                    //ClearBranchControls(pnlEntryForm, branchNodeName, arrCtrls);
                }
            }

            
            //Setting BPAction
            XmlNode nodeRoot = xdoc.SelectSingleNode("Root");
            XmlNodeList listBranches = nodeRoot.SelectNodes("//Rows");            
            if (listBranches != null)
            {
                //Updated on 12/02/08 by shanti.
                string curAction = string.Empty;
                if (ObjCommonUI.ButtonsUserControl != null)
                {
                    HiddenField hdnCurrAction = (HiddenField)ObjCommonUI.ButtonsUserControl.FindControl("hdnCurrAction");
                    if (hdnCurrAction != null)
                    {
                        curAction = hdnCurrAction.Value;
                    }
                } 

                ImageButton imgbtnSubmit = (ImageButton)pnlEntryForm.FindControl("imgbtnSubmit");
                foreach (XmlNode nodeRow in listBranches)
                {                    
                    XmlAttribute attrBPAction = xdoc.CreateAttribute("BPAction");
                    if (imgbtnSubmit.Attributes["attrSave"].ToUpper().Trim() == "SAVE")
                    {
                        attrBPAction.Value = "Add";
                    }
                    else if (curAction.Trim().ToUpper().ToString() != "SAVE")
                    {
                        if (nodeRow.Attributes["TrxID"] == null)
                        {
                            if (curAction.Trim().ToUpper().ToString() == "FIND")
                            {
                                attrBPAction.Value = curAction.Trim();
                            }
                            else
                            {
                                attrBPAction.Value = "Add";
                            }
                        }
                        else
                        {
                            attrBPAction.Value = curAction.Trim();
                        }
                    }
                    nodeRow.Attributes.Append(attrBPAction);

                    //For modify action, checking if "-1" is sent for ddl and removing those attributes
                    if (nodeRow.Attributes["BPAction"].Value.Trim() == "Modify")
                    {
                        ArrayList attToBeRemoved = new ArrayList();
                        foreach (XmlAttribute att in nodeRow.Attributes)
                        {
                            if (att.Value == "-1" || att.Value == string.Empty)
                            {
                                string[] strarr = att.Name.Split('_');
                                if (!attToBeRemoved.Contains(strarr[0].ToString()))
                                {
                                    attToBeRemoved.Add(strarr[0].ToString());
                                }
                            }
                        }
                        if (attToBeRemoved.Count > 0)
                        {
                            foreach (string attRemove in attToBeRemoved)
                            {
                                for (int i = 0; i < nodeRow.Attributes.Count; i++)
                                {
                                    if (nodeRow.Attributes[i].Name.Contains(attRemove))
                                    {
                                        nodeRow.Attributes.Remove(nodeRow.Attributes[i]);
                                        i--;
                                    }
                                }
                            }

                        }
                    }
                }                
            }

            ObjCommonUI.ButtonsUserControl.BranchXML = xdoc.OuterXml;

            ////Filling BranchUI
            //if (ObjCommonUI.ButtonsUserControl.BranchXML != string.Empty)
            //{
            //    XmlDocument xbranchdoc = new XmlDocument();
            //    xbranchdoc.LoadXml(ObjCommonUI.ButtonsUserControl.BranchXML.ToString());
            //    XmlNode nodeBranchRowList = xbranchdoc.SelectSingleNode("Root/" + branchNodeName + "/RowList");
            //    if (nodeBranchRowList != null)
            //    {
            //        XmlNode nodeRow = nodeBranchRowList.SelectSingleNode("Rows");
            //        if (nodeRow != null)
            //        {
            //            FillBranchUI(pnlEntryForm, nodeRow, branchNodeName, arrCtrls);
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Fills the Branch controls
        /// </summary>
        /// <param name="branchNodeName">branchNodeName.</param>
        /// <param name="controlName">DDL control ID.</param>
        /// <param name="dataEntered">Is Data entered in branch controls</param>
        /// <param name="pnlEntryForm">panel containing the controls</param>
        /// 
        public void FillBranch(string branchNodeName, string controlName, bool dataEntered, Panel pnlEntryForm)
        {
            string control = "ddl" + controlName + "_" + branchNodeName;
            LAjitDropDownList ddlBranchDropdown = (LAjitDropDownList)pnlEntryForm.FindControl(control);
            if (ddlBranchDropdown != null)
            {
                //Retrieving list of branch controls
                ArrayList arrCtrls = GetBranchControls(branchNodeName);

                //Filling BranchUI
                if (ObjCommonUI.ButtonsUserControl.BranchXML != string.Empty)
                {
                    XmlDocument xbranchdoc = new XmlDocument();
                    xbranchdoc.LoadXml(ObjCommonUI.ButtonsUserControl.BranchXML.ToString());
                    XmlNode nodeBranchRowList = xbranchdoc.SelectSingleNode("Root/" + branchNodeName + "/RowList");
                    if (nodeBranchRowList != null)
                    {
                        string newTrxIDType = string.Empty;
                        newTrxIDType = ddlBranchDropdown.SelectedValue.Trim();
                        if (newTrxIDType.Length != 0)
                        {
                            string[] strarr = newTrxIDType.Split('~');
                            string trxID = strarr[0].ToString();
                            string trxType = strarr[1].ToString();
                            XmlNode nodeRow = nodeBranchRowList.SelectSingleNode("Rows[@" + controlName + "_TrxID='" + trxID + "' and @" + controlName + "_TrxType='" + trxType + "']");                                                                            
                            if (nodeRow != null)
                            {
                                arrCtrls.Remove(control);
                                FillBranchUI(pnlEntryForm, nodeRow, branchNodeName, arrCtrls);
                                string previousSelValue = ddlBranchDropdown.SelectedItem.Text.ToString();
                                ddlBranchDropdown.Attributes["MapPreviousSelItem"] = previousSelValue;
                            }

                        }
                    }
                }
            }


        }

        #endregion

    }
}
