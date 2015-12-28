using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Web;
using LAjit_DAL;


namespace LAjit_BO
{
    public class CommonBO
    {
        private CommonDAL objDAL = new CommonDAL();

        //public string GetLoginUserInfo(string inputXML)
        //{
        //   //return objDAL.GetLoginInfo(inputXML);

        //  return objDAL.AddThisVendor(inputXML);

        //}

        public string GetLoginInfo(string inputXML)
        {
            //return objDAL.GetLoginInfo(inputXML);
            return objDAL.GetBPEOut(inputXML);
        }

        public string UpdateShortCuts(string inputXML)
        {
            //return objDAL.UpdateShortCuts(inputXML);
            return objDAL.GetBPEOut(inputXML);
        }

        public string GetVendorFormInfo(string inputXML)
        {
            //return objDAL.GetVendFormInfo(inputXML);
            return objDAL.GetBPEOut(inputXML);
        }

        public string GetModVendorFormInfo(string inputXML)
        {
            //return objDAL.GetModVendorFormInfo(inputXML);
            return objDAL.GetBPEOut(inputXML);
        }

        public string GetVendorDetails(string inputXML)
        {
            //return objDAL.GetVendorDetails(inputXML);
            return objDAL.GetBPEOut(inputXML);
        }

        public string AddThisVendor(string inputXML)
        {
            //return objDAL.AddThisVendor(inputXML);
            return objDAL.GetBPEOut(inputXML);
        }

        public string DeleteVendor(string inputXML)
        {
            //return objDAL.DeleteVendor(inputXML);
            return objDAL.GetBPEOut(inputXML);
        }

        public string ModifyVendor(string inputXML)
        {
            //return objDAL.ModifyVendor(inputXML);
            return objDAL.GetBPEOut(inputXML);
        }

        public string GetVendors(string inputXML)
        {
            //return objDAL.GetVendors(inputXML);
            return objDAL.GetBPEOut(inputXML);
        }

        public string FindThisVendor(string inputXML)
        {
            //return objDAL.FindThisVendor(inputXML);
            return objDAL.GetBPEOut(inputXML);
        }

        public string GetVendorHistory(string inputXML)
        {
            //return objDAL.GetVendorHistory(inputXML);
            return objDAL.GetBPEOut(inputXML);
        }

        public string GetDataForCPGV1(string inputXML)
        {

            //return objDAL.GetVendorHistory(inputXML);

            //Returns static data served by custom stored procedure.
            //return objDAL.GetDataForCPGV1(inputXML);

            //return objDAL.AddThisVendor(inputXML);
            return objDAL.GetBPEOut(inputXML);
            //return objDAL.GetDataAccountingLayout(inputXML);
        }

        public string GetAutoFillData(string inputXML, string StoredProcedureName)
        {
            //return objDAL.
            //GetBPEOutBySP
            return objDAL.RunStoredProcedure(inputXML, StoredProcedureName);
        }

        /// <summary>
        /// Gets the number of PO's existing for a given Vendor/Customer.
        /// </summary>
        /// <param name="inputXML">Input XML.</param>
        /// <param name="spName">The SP to run.</param>
        /// <returns>Out XML</returns>
        public string GetPOs(string inputXML, string spName)
        {
            return objDAL.RunStoredProcedure(inputXML, spName);
        }

        public string GetCOAFormInfo(string inputXML)
        {
            //return objDAL.GetCOAFormInfo(inputXML);
            return objDAL.GetBPEOut(inputXML);
        }

        /// <summary>
        /// Generated the xml to request data to be bound for the grid view.
        /// </summary>
        /// <param name="BPGID">BPGID of the grid view</param>
        /// <returns>XML Data.</returns>
        public string GenActionRequestXML(string BPEAction, string BPValue, string CntrlValues, string trxID, string GVDataXML, string BPE, bool gridExists, string pageSize, string pageNo, string BPInfo)
        {
            string ReqXMl = string.Empty;
            try
            {
                string m_GVTreeNodeName = string.Empty;
                XmlDocument xDocGV = new XmlDocument();
                XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
                if (BPEAction == "Save")
                {
                    nodeRoot.InnerXml = "<Autosave Process='1'></Autosave>";
                }
                nodeRoot.InnerXml += BPE;

                //if (BPEAction.ToUpper().Trim() == "NOROWS")
                //{
                //    XmlDocument xDoc = new XmlDocument();
                //    xDoc.LoadXml(BPValue);

                //    //Creating the INIT node
                //    XmlNode nodeINIT = xDoc.CreateNode(XmlNodeType.Element, "INIT", null);
                //    nodeINIT.InnerText = "1";

                //    //Appending INIT Node
                //    xDoc.SelectSingleNode("bpinfo").AppendChild(nodeINIT);

                //    BPValue = xDoc.SelectSingleNode("bpinfo").OuterXml;
                //    nodeRoot.InnerXml += BPValue.ToString();
                //}
                if (BPEAction.ToUpper().Trim() != "PAGELOAD" && BPEAction.ToUpper().Trim() != "PREVIOUSPAGELOAD")
                {
                    //Creating the bpinfo node
                    XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
                    nodeRoot.AppendChild(nodeBPInfo);

                    //Creating the BPGID node
                    XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
                    nodeBPGID.InnerText = BPValue;
                    nodeBPInfo.AppendChild(nodeBPGID);
                    if (BPEAction != "")
                    {
                        XmlDocument returnXML = new XmlDocument();
                        returnXML.LoadXml(GVDataXML);
                        m_GVTreeNodeName = returnXML.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                        XmlNode nodeAccountingLayout = xDocGV.CreateNode(XmlNodeType.Element, m_GVTreeNodeName, null);
                        nodeBPInfo.AppendChild(nodeAccountingLayout);
                        //Creating the Rowlist node
                        XmlNode nodeRowList = xDocGV.CreateNode(XmlNodeType.Element, "RowList", null);

                        if ((BPEAction.ToUpper().Trim() == "ADD") || (BPEAction.ToUpper().Trim() == "FIND"))
                        {
                            xDocGV.AppendChild(nodeRowList);
                            nodeRowList.InnerXml = CntrlValues;

                            XmlAttribute attrBPAction = xDocGV.CreateAttribute("BPAction");
                            attrBPAction.Value = BPEAction;
                            nodeRowList.SelectSingleNode("Rows").Attributes.Append(attrBPAction);
                        }
                        else if (BPEAction.ToUpper().Trim() == "SAVE")
                        {
                            xDocGV.AppendChild(nodeRowList);
                            nodeRowList.InnerXml = CntrlValues;
                            XmlAttribute attrBPAction = xDocGV.CreateAttribute("BPAction");
                            attrBPAction.Value = "Add";
                            nodeRowList.SelectSingleNode("Rows").Attributes.Append(attrBPAction);
                        }
                        else if ((BPEAction.ToUpper().Trim() == "MODIFY") || ((BPEAction.ToUpper().Trim() == "DELETE")))
                        {
                            XmlDocument xNewDoc = new XmlDocument();
                            xNewDoc.LoadXml(GVDataXML);
                            XmlNode nodeRowLst = xNewDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/RowList");

                            XmlNode nodeRow = nodeRowLst.SelectSingleNode("Rows[@TrxID='" + trxID + "']");
                            if (BPEAction.ToUpper().Trim() == "DELETE")
                            {
                                nodeRowList.InnerXml = nodeRow.InnerXml;
                            }
                            else
                            {
                                XmlDocument xDc = new XmlDocument();
                                xDc.LoadXml(CntrlValues);
                                XmlNode xNodeMod = xDc.SelectSingleNode("Rows");

                                for (int i = 0; i < xNodeMod.Attributes.Count; i++)
                                {
                                    if (nodeRow.Attributes[xNodeMod.Attributes[i].Name.ToString()] != null)
                                    {
                                        nodeRow.Attributes[xNodeMod.Attributes[i].Name.ToString()].Value = xNodeMod.Attributes[xNodeMod.Attributes[i].Name.ToString()].Value;
                                    }
                                    else
                                    {
                                        XmlAttribute attrNew = xNewDoc.CreateAttribute(xNodeMod.Attributes[i].Name.ToString());
                                        attrNew.Value = xNodeMod.Attributes[xNodeMod.Attributes[i].Name.ToString()].Value;
                                        nodeRow.Attributes.Append(attrNew);
                                    }
                                }
                            }
                            XmlAttribute attrBPAction = xNewDoc.CreateAttribute("BPAction");
                            attrBPAction.Value = BPEAction;
                            nodeRow.Attributes.Append(attrBPAction);

                            nodeRowList.InnerXml = nodeRow.OuterXml.ToString();
                        }
                        nodeAccountingLayout.AppendChild(nodeRowList);
                        if (BPInfo != null)
                        {
                            XmlDocument xdocBPInfo = new XmlDocument();
                            xdocBPInfo.LoadXml(BPInfo);
                            XmlNode nodecallingobj = xdocBPInfo.SelectSingleNode("bpinfo/CallingObject");
                            if (nodecallingobj != null)
                            {
                                //Creating the CallingObject node
                                XmlNode nodeCallingObject = xDocGV.CreateNode(XmlNodeType.Element, "CallingObject", null);
                                nodeCallingObject.InnerXml = nodecallingobj.InnerXml;
                                nodeBPInfo.AppendChild(nodeCallingObject);
                            }
                        }
                    }
                }
                else
                {
                    nodeRoot.InnerXml += BPValue.ToString();
                }
                ReqXMl = nodeRoot.OuterXml;


                if (gridExists)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(nodeRoot.OuterXml);
                    XmlNode nodeBPInfo = xDoc.SelectSingleNode("Root/bpinfo");
                    if (pageNo.Length == 0)
                    {
                        pageNo = "1";
                    }
                    XmlNode nodeGridView = nodeBPInfo.SelectSingleNode("//Gridview");
                    if (nodeGridView == null)
                    {
                        nodeBPInfo.InnerXml += @" <Gridview><Pagenumber>" + pageNo + "</Pagenumber><Pagesize>" + pageSize + "</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
                    }
                    //if (BPEAction.ToUpper().Trim() == "FIND")
                    //{
                    //    XmlNode treenode = xDoc.SelectSingleNode("Root/bpinfo/" + m_GVTreeNodeName);
                    //    treenode.InnerXml += @" <Gridview><Pagenumber>" + pageNo + "</Pagenumber><Pagesize>" + pageSize + "</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
                    //}
                    //else
                    //{
                    //    nodeBPInfo.InnerXml += @" <Gridview><Pagenumber>" + pageNo + "</Pagenumber><Pagesize>" + pageSize + "</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
                    //}//Above block commented by Danny(Improper BpInfo being created).
                    ReqXMl = xDoc.OuterXml;
                }
                if (BPEAction.ToUpper().Trim() == "PREVIOUSPAGELOAD")
                {
                    XmlDocument xBPinfo = new XmlDocument();
                    xBPinfo.LoadXml(ReqXMl);
                    XmlDocument returnXML = new XmlDocument();
                    returnXML.LoadXml(GVDataXML);
                    m_GVTreeNodeName = returnXML.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;
                    XmlNode nodeTreeNode = xBPinfo.CreateNode(XmlNodeType.Element, m_GVTreeNodeName, null);
                    XmlNode nodeBPInfo = xBPinfo.SelectSingleNode("Root/bpinfo");
                    XmlNode nodegridView = xBPinfo.SelectSingleNode("Root/bpinfo/Gridview");
                    string gridview = nodegridView.OuterXml;
                    nodeBPInfo.RemoveChild(nodegridView);
                    nodeTreeNode.InnerXml = gridview;
                    nodeBPInfo.AppendChild(nodeTreeNode);
                    ReqXMl = xBPinfo.OuterXml;
                }
            }
            catch (Exception ex)
            {
            }
            return ReqXMl;
        }


        /// <summary>
        /// Generated the xml to request data to be bound for the grid view.
        /// </summary>
        /// <param name="BPGID">BPGID of the grid view</param>
        /// <returns>XML Data.</returns>
        public string GenActionRequestXML(string BPEAction, string BPValue, string CntrlValues, string trxID, string GVDataXML, string BPE, bool gridExists, string pageSize, string pageNo, string BPInfo, string lstBxParams)
        {
            string ReqXMl = string.Empty;
            try
            {
                string m_GVTreeNodeName = string.Empty;
                XmlDocument xDocGV = new XmlDocument();
                XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
                if (BPEAction == "Save")
                {
                    nodeRoot.InnerXml = "<Autosave Process='1'></Autosave>";
                }
                nodeRoot.InnerXml += BPE;

                //if (BPEAction.ToUpper().Trim() == "NOROWS")
                //{
                //    XmlDocument xDoc = new XmlDocument();
                //    xDoc.LoadXml(BPValue);

                //    //Creating the INIT node
                //    XmlNode nodeINIT = xDoc.CreateNode(XmlNodeType.Element, "INIT", null);
                //    nodeINIT.InnerText = "1";

                //    //Appending INIT Node
                //    xDoc.SelectSingleNode("bpinfo").AppendChild(nodeINIT);

                //    BPValue = xDoc.SelectSingleNode("bpinfo").OuterXml;
                //    nodeRoot.InnerXml += BPValue.ToString();
                //}
                if (BPEAction.ToUpper().Trim() != "PAGELOAD" && BPEAction.ToUpper().Trim() != "PREVIOUSPAGELOAD")
                {
                    //Creating the bpinfo node
                    XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
                    nodeRoot.AppendChild(nodeBPInfo);

                    //Creating the BPGID node
                    XmlNode nodeBPGID = xDocGV.CreateNode(XmlNodeType.Element, "BPGID", null);
                    nodeBPGID.InnerText = BPValue;
                    nodeBPInfo.AppendChild(nodeBPGID);
                    if (BPEAction != "")
                    {
                        //Appending the List Box's Xml
                        if (lstBxParams != string.Empty)
                        {
                            nodeBPInfo.InnerXml += lstBxParams;
                        }

                        XmlDocument returnXML = new XmlDocument();
                        returnXML.LoadXml(GVDataXML);
                        m_GVTreeNodeName = returnXML.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

                        //XmlNode nodeColumns = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/GridHeading/Columns");

                        XmlNode nodeAccountingLayout = xDocGV.CreateNode(XmlNodeType.Element, m_GVTreeNodeName, null);
                        nodeBPInfo.AppendChild(nodeAccountingLayout);
                        //Creating the Rowlist node
                        XmlNode nodeRowList = xDocGV.CreateNode(XmlNodeType.Element, "RowList", null);

                        if ((BPEAction.ToUpper().Trim() == "ADD") || (BPEAction.ToUpper().Trim() == "FIND"))
                        {
                            xDocGV.AppendChild(nodeRowList);
                            nodeRowList.InnerXml = CntrlValues;

                            XmlAttribute attrBPAction = xDocGV.CreateAttribute("BPAction");
                            attrBPAction.Value = BPEAction;
                            nodeRowList.SelectSingleNode("Rows").Attributes.Append(attrBPAction);
                        }
                        else if (BPEAction.ToUpper().Trim() == "SAVE")
                        {
                            xDocGV.AppendChild(nodeRowList);
                            nodeRowList.InnerXml = CntrlValues;
                            XmlAttribute attrBPAction = xDocGV.CreateAttribute("BPAction");
                            attrBPAction.Value = "Add";
                            nodeRowList.SelectSingleNode("Rows").Attributes.Append(attrBPAction);
                        }
                        else if ((BPEAction.ToUpper().Trim() == "MODIFY") || ((BPEAction.ToUpper().Trim() == "DELETE")))
                        {
                            XmlDocument xNewDoc = new XmlDocument();
                            xNewDoc.LoadXml(GVDataXML);
                            XmlNode nodeRowLst = xNewDoc.SelectSingleNode("Root/bpeout/FormControls/" + m_GVTreeNodeName + "/RowList");

                            XmlNode nodeRow = nodeRowLst.SelectSingleNode("Rows[@TrxID='" + trxID + "']");
                            if (BPEAction.ToUpper().Trim() == "DELETE")
                            {
                                nodeRowList.InnerXml = nodeRow.InnerXml;
                            }
                            else
                            {
                                XmlDocument xDc = new XmlDocument();
                                xDc.LoadXml(CntrlValues);
                                XmlNode xNodeMod = xDc.SelectSingleNode("Rows");

                                //Iterating through all attributes in modified row and changing those values in GvDataXMl, if not present creating new one
                                for (int i = 0; i < xNodeMod.Attributes.Count; i++)
                                {
                                    if (nodeRow.Attributes[xNodeMod.Attributes[i].Name.ToString()] != null)
                                    {
                                        nodeRow.Attributes[xNodeMod.Attributes[i].Name.ToString()].Value = xNodeMod.Attributes[xNodeMod.Attributes[i].Name.ToString()].Value;
                                    }
                                    else
                                    {
                                        XmlAttribute attrNew = xNewDoc.CreateAttribute(xNodeMod.Attributes[i].Name.ToString());
                                        attrNew.Value = xNodeMod.Attributes[xNodeMod.Attributes[i].Name.ToString()].Value;
                                        nodeRow.Attributes.Append(attrNew);
                                    }
                                }
                                ////Iterating through all attributes in GvdataXMl row and deleting if it doesnt exist in Modified row
                                //for (int attCnt = 0; attCnt < nodeRow.Attributes.Count; attCnt++)
                                //{
                                //    foreach (DataColumn dCol in nodeColumns.ChildNodes)
                                //    { 
                                //        if(nodeRow.Attributes[attCnt].Name.ToString().Contains())
                                //    }
                                //}

                            }
                            XmlAttribute attrBPAction = xNewDoc.CreateAttribute("BPAction");
                            attrBPAction.Value = BPEAction;
                            nodeRow.Attributes.Append(attrBPAction);

                            nodeRowList.InnerXml = nodeRow.OuterXml.ToString();
                        }
                        nodeAccountingLayout.AppendChild(nodeRowList);
                        if (BPInfo != null)
                        {
                            XmlDocument xdocBPInfo = new XmlDocument();
                            xdocBPInfo.LoadXml(BPInfo);
                            XmlNode nodecallingobj = xdocBPInfo.SelectSingleNode("bpinfo/CallingObject");
                            if (nodecallingobj != null)
                            {
                                //Creating the CallingObject node
                                XmlNode nodeCallingObject = xDocGV.CreateNode(XmlNodeType.Element, "CallingObject", null);
                                nodeCallingObject.InnerXml = nodecallingobj.InnerXml;
                                nodeBPInfo.AppendChild(nodeCallingObject);
                            }
                        }
                    }
                }
                else
                {
                    nodeRoot.InnerXml += BPValue.ToString();
                }
                ReqXMl = nodeRoot.OuterXml;
                if (gridExists)
                {
                    XmlDocument xDocum = new XmlDocument();
                    xDocum.LoadXml(nodeRoot.OuterXml);
                    XmlNode bpInfoNode = xDocum.SelectSingleNode("Root/bpinfo");
                    if (pageNo == string.Empty)
                    {
                        pageNo = "1";
                    }
                    if (BPEAction.ToUpper().Trim() == "FIND")
                    {
                        pageNo = "1";
                        XmlNode treenode = xDocum.SelectSingleNode("Root/bpinfo/" + m_GVTreeNodeName);
                        treenode.InnerXml += @" <Gridview><Pagenumber>" + pageNo + "</Pagenumber><Pagesize>" + pageSize + "</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
                    }
                    else
                    {
                        bpInfoNode.InnerXml += @" <Gridview><Pagenumber>" + pageNo + "</Pagenumber><Pagesize>" + pageSize + "</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
                    }

                    ReqXMl = xDocum.OuterXml;
                }

                if (BPEAction.ToUpper().Trim() == "PREVIOUSPAGELOAD")
                {
                    XmlDocument xBPinfo = new XmlDocument();
                    xBPinfo.LoadXml(ReqXMl);
                    XmlNode nodeTreeNode = xBPinfo.CreateNode(XmlNodeType.Element, m_GVTreeNodeName, null);
                    XmlNode nodeBPInfo = xBPinfo.SelectSingleNode("Root/bpinfo");
                    XmlNode nodegridView = xBPinfo.SelectSingleNode("Root/bpinfo/Gridview");
                    string gridview = nodegridView.OuterXml;
                    nodeBPInfo.RemoveChild(nodegridView);
                    nodeTreeNode.InnerXml = gridview;
                    nodeBPInfo.AppendChild(nodeTreeNode);
                    ReqXMl = xBPinfo.OuterXml;
                }
            }
            catch (Exception ex)
            {

            }
            return ReqXMl;
        }
    }
}
