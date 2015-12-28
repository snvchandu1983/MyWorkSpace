using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace LAjitDev
{
    class ITextXMLStore
    {
        #region Structs & Enums
        //public struct FileResult
        //{
        //    public bool Success;
        //    public string Msg;

        //    public FileResult(bool bDefault, string sMsg)
        //    {
        //        this.Success = bDefault;
        //        this.Msg = "";
        //    }
        //}
        public enum DataType
        {
            String,     //default
        }

        public struct Field
        {
            public string label;
            public string caption;
            public string fullViewLength;
            public string controlType;
            public DataType type;

            public Field(string sName, string sTitle, DataType enType)
            {
                label = sName;
                caption = sTitle;
                type = enType;
                controlType = "";
                fullViewLength = "";     //Relative fullViewLength; can be used for display, print, PDF, ...
            }
        }
        #endregion

        #region Member Variables
        private string msFile = "";
        private bool mbIsReadOnly = false;
        private string msDataNodePath = "";
        private Field[] mFields = null;
        private string msIdColumnName = "ID";
        private bool mbIdColumnAdded = false;
        private int mnIdColumn = -1;

        private XmlDocument mxDoc = null;
        private XmlNode mxnodeSchema = null;
        private XmlNodeList mxParentNodeListData = null;
        public XmlNode xStoreRowList = null;
        public XmlNodeList mxChildNodeListData = null;
        public XmlNodeList mxTrxNodeListColData = null;
        private bool isParent = false;
        private bool isChild = false;

        private string reportStyle;
        private int subChdColHdrCount = 0;
        private string parentTreeNodeName = string.Empty;
        private string treeNodeName = string.Empty;

        #endregion

        #region Properties
        private bool mbThrowExceptions = false;

        public bool IsReadOnly
        {
            get { return mbIsReadOnly; }
            set { mbIsReadOnly = value; }
        }

        public int ColumnUID
        {
            get { return mnIdColumn; }
            set { mnIdColumn = value; }
        }

        public string NameOfFieldWithUniqueID
        {
            get { return msIdColumnName; }
            set { msIdColumnName = value; }
        }

        public Field[] Fields
        {
            get { return mFields; }
            set { mFields = value; }
        }

        public string File
        {
            get { return msFile; }
            set { msFile = value; }
        }

        public string ParentTreeNodeName
        {
            get { return parentTreeNodeName; }
            set { parentTreeNodeName = value; }
        }

        public string TreeNodeName
        {
            get { return treeNodeName; }
            set { treeNodeName = value; }
        }

        public string RecordNodeName
        {
            get { return msDataNodePath; }
            set { msDataNodePath = value; }
        }

        public bool ThrowExceptions
        {
            get { return mbThrowExceptions; }
            set { mbThrowExceptions = value; }
        }

        public bool IsParent
        {
            get { return isParent; }
            set { isParent = value; }
        }

        public bool IsChild
        {
            get { return isChild; }
            set { isChild = value; }
        }

        public string ReportStyle
        {
            get { return reportStyle; }
            set { reportStyle = value; }
        }

        #region Commented Code
        //public static bool IsXMLReadOnly(string filelabel)
        //{
        //    bool bIsReadOnly = false;
        //    try
        //    {
        //        FileInfo fi = new FileInfo(filelabel);
        //        if (fi.Exists)
        //            bIsReadOnly = fi.IsReadOnly;
        //    }
        //    catch (Exception ex)
        //    {
        //        FileResult frRet = new FileResult(true, "");
        //        frRet.Success = false;
        //        frRet.Msg = ex.ToString();
        //    }

        //    return bIsReadOnly;
        //}
        #endregion
        #endregion

        #region Constructors
        private ITextXMLStore()  //mainly to disable generic constructor
        {
        }

        //For Loading Child XML
        public ITextXMLStore(XmlDocument m_xDoc, string parentTreeNodeName, string childTreeNodeName, string subChildTreeNodeName, string trxTreeNodeName, string trxSecondTreeNodeName, string trxThirdTreeNodeName, string reportStyle, long Link1)
        {
            this.reportStyle = reportStyle;
            this.parentTreeNodeName = parentTreeNodeName;
            ChildConstructor(m_xDoc, childTreeNodeName, subChildTreeNodeName, trxTreeNodeName, trxSecondTreeNodeName, trxThirdTreeNodeName, Link1);
        }

        //For Loading SubChild XML
        public ITextXMLStore(XmlDocument m_xDoc, string subChildTreeNodeName, string trxTreeNodeName, string trxSecondTreeNodeName, string trxThirdTreeNodeName, string reportStyle, int Link2, bool trxTreeNode)
        {
            this.reportStyle = reportStyle;
            SubChildConstructor(m_xDoc, subChildTreeNodeName, trxTreeNodeName, trxSecondTreeNodeName, trxThirdTreeNodeName, Link2);
        }

        //For Loading 400 XML
        public ITextXMLStore(XmlDocument m_xDoc, string treeNodeName, int trxID)
        {
            this.parentTreeNodeName = treeNodeName;

            XmlNodeList xnodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + parentTreeNodeName + "//GridHeading//Columns/Col");

            XmlDocument xChildDoc = new XmlDocument();

            XmlNode nodeRoot = xChildDoc.CreateNode(XmlNodeType.Element, "Root", null);

            XmlNode nodeBPE = xChildDoc.CreateNode(XmlNodeType.Element, "bpeout", null);
            nodeRoot.AppendChild(nodeBPE);

            XmlNode nodeFormControls = xChildDoc.CreateNode(XmlNodeType.Element, "FormControls", null);
            nodeBPE.AppendChild(nodeFormControls);

            XmlNode treeNode = xChildDoc.CreateNode(XmlNodeType.Element, parentTreeNodeName, null);
            nodeFormControls.AppendChild(treeNode);

            XmlNode nodeGridHeading = xChildDoc.CreateNode(XmlNodeType.Element, "GridHeading", null);
            treeNode.AppendChild(nodeGridHeading);

            XmlNode nodeColumns = xChildDoc.CreateNode(XmlNodeType.Element, "Columns", null);
            nodeGridHeading.AppendChild(nodeColumns);

            XmlNode colsList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + parentTreeNodeName + "//GridHeading//Columns");
            nodeColumns.InnerXml = colsList.InnerXml;

            XmlNode nodeRowList = xChildDoc.CreateNode(XmlNodeType.Element, "RowList", null);
            treeNode.AppendChild(nodeRowList);
            nodeRowList.InnerXml = "";

            StringBuilder strRowListBuilder = new StringBuilder();

            XmlNode rowList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + parentTreeNodeName + "//RowList");

            if (trxID != -1)
            {
                foreach (XmlNode childNode in rowList.ChildNodes)
                {
                    if (childNode.Attributes["JobID"] != null && Convert.ToInt32(childNode.Attributes["JobID"].Value) == trxID)
                    {
                        //nodeRowList.InnerXml = nodeRowList.InnerXml + childNode.OuterXml;
                        strRowListBuilder = strRowListBuilder.Append(childNode.OuterXml);
                    }
                }
            }
            else
            {
                //nodeRowList.InnerXml = rowList.InnerXml;
                strRowListBuilder = strRowListBuilder.Append(rowList.InnerXml);
            }

            nodeRowList.InnerXml = strRowListBuilder.ToString();
            xStoreRowList = nodeRowList;
            xChildDoc.AppendChild(nodeRoot);
            this.msFile = xChildDoc.OuterXml;
        }

        //for Loading Parent XML
        public ITextXMLStore(string sFile, string reportStyle)
        {
            this.msFile = sFile;
            this.isParent = true;
            this.reportStyle = reportStyle;
            //this.mbIsReadOnly = IsXMLReadOnly(sFile);
        }
        #endregion

        #region Private Methods
        private bool AdjustForUniqueID()
        {
            int i = 0;
            this.mnIdColumn = -1;
            bool bHasFieldWithUniqueID = false;
            this.mbIdColumnAdded = false;
            for (i = 0; i < this.mFields.Length; i++)
            {
                //--- see if this field is tagged as the one with the unique value
                if (bHasFieldWithUniqueID == false)
                {
                    //--- in case there is "case" mismatch ...
                    //--- first check it by making both upper case
                    if ((mFields[i].label.ToUpper()
                        == this.NameOfFieldWithUniqueID.ToUpper()))
                    {
                        bHasFieldWithUniqueID = true;
                        //--- in case there is "case" mismatch, save it
                        this.NameOfFieldWithUniqueID = mFields[i].label;
                    }
                }
                else
                    this.mnIdColumn = i;

                i++;
            }

            if (bHasFieldWithUniqueID == false)
            {
                Field fieldNew = new Field(this.NameOfFieldWithUniqueID, this.NameOfFieldWithUniqueID, DataType.String);
                //this.FieldAdd(ref this.mFields, fieldNew);
                this.FieldAdd(fieldNew);
                this.mbIdColumnAdded = true;
                this.mnIdColumn = this.mFields.Length - 1;
            }

            return this.mbIdColumnAdded;

        }

        private void FieldAdd(Field sNew)
        {
            Field[] sArrayNew = new Field[this.mFields.Length + 1];
            this.mFields.CopyTo(sArrayNew, 0);
            sArrayNew[this.mFields.Length] = sNew;
            this.mFields = sArrayNew;
        }

        private void FieldAdd(ref Field[] sArray, Field sNew)
        {
            Field[] sArrayNew = new Field[sArray.Length + 1];
            sArray.CopyTo(sArrayNew, 0);
            sArrayNew[sArray.Length] = sNew;
            sArray = sArrayNew;
        }

        private void ChildConstructor(XmlDocument m_xDoc, string childTreeNodeName, string subChildTreeNodeName, string trxTreeNodeName, string trxSecondTreeNodeName, string trxThirdTreeNodeName, long Link1)
        {
            XmlNodeList xnodelistFields = m_xDoc.SelectNodes("//Root//bpeout//FormControls//" + childTreeNodeName + "//GridHeading//Columns/Col");

            XmlDocument xChildDoc = new XmlDocument();

            StringBuilder strXChildDocBuilder = new StringBuilder();

            XmlNode nodeRoot = xChildDoc.CreateNode(XmlNodeType.Element, "Root", null);

            XmlNode nodeBPE = xChildDoc.CreateNode(XmlNodeType.Element, "bpeout", null);
            nodeRoot.AppendChild(nodeBPE);

            XmlNode nodeFormControls = xChildDoc.CreateNode(XmlNodeType.Element, "FormControls", null);
            nodeBPE.AppendChild(nodeFormControls);

            XmlNode treeNode = xChildDoc.CreateNode(XmlNodeType.Element, childTreeNodeName, null);
            nodeFormControls.AppendChild(treeNode);

            XmlNode nodeGridHeading = xChildDoc.CreateNode(XmlNodeType.Element, "GridHeading", null);
            treeNode.AppendChild(nodeGridHeading);

            XmlNode nodeColumns = xChildDoc.CreateNode(XmlNodeType.Element, "Columns", null);
            nodeGridHeading.AppendChild(nodeColumns);

            XmlNode childColsList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + childTreeNodeName + "//GridHeading//Columns");
            nodeColumns.InnerXml = childColsList.InnerXml;

            StringBuilder strColListBuilder = new StringBuilder();

            if (ReportStyle == "651" || ReportStyle == "653")
            {
                XmlNode subChildTreeColList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + subChildTreeNodeName + "//GridHeading//Columns");
                foreach (XmlNode subChildTreeNode in subChildTreeColList.ChildNodes)
                {
                    if (subChildTreeNode.Attributes["Label"].Value == "EndBal")
                    {
                        //nodeColumns.InnerXml = nodeColumns.InnerXml + subChildTreeNode.OuterXml;
                        strColListBuilder.Append(subChildTreeNode.OuterXml);
                        break;
                    }
                }

                XmlNode trxTreeColList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + trxTreeNodeName + "//GridHeading//Columns");
                foreach (XmlNode trxNode in trxTreeColList.ChildNodes)
                {
                    if (trxNode.Attributes["Label"].Value == "EndBal" || trxNode.Attributes["Label"].Value == "Ending")
                    {
                        if (trxNode.Attributes["Label"].Value == "EndBal")
                        {
                            trxNode.Attributes["Label"].Value = "Ending";
                        }
                        //nodeColumns.InnerXml = nodeColumns.InnerXml + trxNode.OuterXml;
                        strColListBuilder.Append(trxNode.OuterXml);
                        break;
                    }
                }

                XmlNode trxSecondTreeColList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + trxSecondTreeNodeName + "//GridHeading//Columns");
                foreach (XmlNode trxNode in trxSecondTreeColList.ChildNodes)
                {
                    if (trxNode.Attributes["Label"].Value == "EndBal" || trxNode.Attributes["Label"].Value == "Ending" || trxNode.Attributes["Label"].Value == "EndingBal")
                    {
                        if (trxNode.Attributes["Label"].Value == "EndBal")
                        {
                            trxNode.Attributes["Label"].Value = "EndingBal";
                        }
                        //nodeColumns.InnerXml = nodeColumns.InnerXml + trxNode.OuterXml;
                        strColListBuilder.Append(trxNode.OuterXml);
                        break;
                    }
                }

                if (ReportStyle == "653")
                {
                    XmlNode trxThirdTreeColList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + trxThirdTreeNodeName + "//GridHeading//Columns");
                    foreach (XmlNode trxNode in trxThirdTreeColList.ChildNodes)
                    {
                        if (trxNode.Attributes["Label"].Value == "EndBal" || trxNode.Attributes["Label"].Value == "Ending" || trxNode.Attributes["Label"].Value == "EndingBal")
                        {
                            if (trxNode.Attributes["Label"].Value == "EndingBal")
                            {
                                trxNode.Attributes["Label"].Value = "EndingBalance";
                            }
                            //nodeColumns.InnerXml = nodeColumns.InnerXml + trxNode.OuterXml;
                            strColListBuilder.Append(trxNode.OuterXml);
                            break;
                        }
                    }
                }

                nodeColumns.InnerXml = nodeColumns.InnerXml + strColListBuilder.ToString();

                mxTrxNodeListColData = nodeColumns.SelectNodes("Col");
            }

            XmlNode nodeRowList = xChildDoc.CreateNode(XmlNodeType.Element, "RowList", null);
            treeNode.AppendChild(nodeRowList);
            nodeRowList.InnerXml = "";

            StringBuilder strRowListBuilder = new StringBuilder();

            XmlNode childRowList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + childTreeNodeName + "//RowList");

            if (childRowList != null)
            {
                foreach (XmlNode childNode in childRowList.ChildNodes)
                {
                    if (ReportStyle != "2" && ReportStyle != "3" && ReportStyle != "5" && ReportStyle != "405" && ReportStyle != "7" && ReportStyle != "200" && ReportStyle != "400" && ReportStyle != "501" && ReportStyle != "502" && ReportStyle != "503")
                    {
                        if (childNode.Attributes["Link1"] != null && Convert.ToInt32(childNode.Attributes["Link1"].Value) == Link1)
                        {
                            //nodeRowList.InnerXml = nodeRowList.InnerXml + childNode.OuterXml;
                            strRowListBuilder = strRowListBuilder.Append(childNode.OuterXml);
                        }
                    }
                    else
                    {
                        if (ReportStyle == "400")
                        {
                            if (childNode.Attributes["AccountID"] != null && Convert.ToInt64(childNode.Attributes["AccountID"].Value) == Link1)
                            {
                                //nodeRowList.InnerXml = nodeRowList.InnerXml + childNode.OuterXml;
                                strRowListBuilder = strRowListBuilder.Append(childNode.OuterXml);
                            }
                        }
                        else
                        {
                            if (childNode.Attributes[ParentTreeNodeName + "_TrxID"] != null && Convert.ToInt32(childNode.Attributes[ParentTreeNodeName + "_TrxID"].Value) == Link1)
                            {
                                //nodeRowList.InnerXml = nodeRowList.InnerXml + childNode.OuterXml;
                                strRowListBuilder = strRowListBuilder.Append(childNode.OuterXml);
                            }
                        }
                    }
                }
            }

            nodeRowList.InnerXml = strRowListBuilder.ToString();

            if (ReportStyle == "651" || ReportStyle == "653")
            {
                XmlNode subChildRowList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + subChildTreeNodeName + "//RowList");
                if (subChildRowList != null)
                {
                    foreach (XmlNode childNode in nodeRowList.ChildNodes)
                    {
                        XmlAttribute xEndBal = xChildDoc.CreateAttribute("EndBal");
                        xEndBal.Value = "";
                        childNode.Attributes.Append(xEndBal);

                        foreach (XmlNode subChildTreeNode in subChildRowList.ChildNodes)
                        {
                            if (subChildTreeNode.Attributes["Link2"] != null && childNode.Attributes["Link2"].Value == subChildTreeNode.Attributes["Link2"].Value)
                            {
                                if (subChildTreeNode.Attributes["EndBal"] != null)
                                {
                                    xEndBal.Value = subChildTreeNode.Attributes["EndBal"].Value;
                                }
                                break;
                            }
                        }
                    }

                    XmlNode trxRowList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + trxTreeNodeName + "//RowList");
                    foreach (XmlNode childNode in nodeRowList.ChildNodes)
                    {
                        XmlAttribute xEndingBal = xChildDoc.CreateAttribute("Ending");
                        xEndingBal.Value = "";
                        childNode.Attributes.Append(xEndingBal);

                        foreach (XmlNode trxNode in trxRowList.ChildNodes)
                        {
                            if (trxNode.Attributes["Link2"] != null && childNode.Attributes["Link2"].Value == trxNode.Attributes["Link2"].Value)
                            {
                                if (trxNode.Attributes["EndBal"] != null)
                                {
                                    xEndingBal.Value = trxNode.Attributes["EndBal"].Value;
                                }
                                break;
                            }
                        }
                    }

                    XmlNode trxSecondRowList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + trxSecondTreeNodeName + "//RowList");
                    foreach (XmlNode childNode in nodeRowList.ChildNodes)
                    {
                        XmlAttribute xEndingBal = xChildDoc.CreateAttribute("EndingBal");
                        xEndingBal.Value = "";
                        childNode.Attributes.Append(xEndingBal);

                        foreach (XmlNode trxNode in trxSecondRowList.ChildNodes)
                        {
                            if (trxNode.Attributes["Link2"] != null && childNode.Attributes["Link2"].Value == trxNode.Attributes["Link2"].Value)
                            {
                                if (trxNode.Attributes["EndBal"] != null)
                                {
                                    xEndingBal.Value = trxNode.Attributes["EndBal"].Value;
                                }
                                break;
                            }
                        }
                    }

                    if (ReportStyle == "653")
                    {
                        XmlNode trxThirdRowList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + trxThirdTreeNodeName + "//RowList");
                        foreach (XmlNode childNode in nodeRowList.ChildNodes)
                        {
                            XmlAttribute xEndingBal = xChildDoc.CreateAttribute("EndingBalance");
                            xEndingBal.Value = "";
                            childNode.Attributes.Append(xEndingBal);

                            foreach (XmlNode trxNode in trxThirdRowList.ChildNodes)
                            {
                                if (trxNode.Attributes["Link2"] != null && childNode.Attributes["Link2"].Value == trxNode.Attributes["Link2"].Value)
                                {
                                    if (trxNode.Attributes["EndBal"] != null)
                                    {
                                        xEndingBal.Value = trxNode.Attributes["EndBal"].Value;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            xChildDoc.AppendChild(nodeRoot);

            this.msFile = xChildDoc.OuterXml;
            this.isParent = false;
        }

        private void SubChildConstructor(XmlDocument m_xDoc, string subChildTreeNodeName, string trxTreeNodeName, string trxSecondTreeNodeName, string trxThirdTreeNodeName, int Link2)
        {
            XmlNode xnodelistFields = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + subChildTreeNodeName + "//GridHeading//Columns/Col");

            XmlDocument xChildDoc = new XmlDocument();

            XmlNode nodeRoot = xChildDoc.CreateNode(XmlNodeType.Element, "Root", null);

            XmlNode nodeBPE = xChildDoc.CreateNode(XmlNodeType.Element, "bpeout", null);
            nodeRoot.AppendChild(nodeBPE);

            XmlNode nodeFormControls = xChildDoc.CreateNode(XmlNodeType.Element, "FormControls", null);
            nodeBPE.AppendChild(nodeFormControls);

            XmlNode treeNode = xChildDoc.CreateNode(XmlNodeType.Element, subChildTreeNodeName, null);
            nodeFormControls.AppendChild(treeNode);

            XmlNode nodeGridHeading = xChildDoc.CreateNode(XmlNodeType.Element, "GridHeading", null);
            treeNode.AppendChild(nodeGridHeading);

            XmlNode nodeColumns = xChildDoc.CreateNode(XmlNodeType.Element, "Columns", null);
            nodeGridHeading.AppendChild(nodeColumns);

            XmlNode childColsList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + subChildTreeNodeName + "//GridHeading//Columns");
            nodeColumns.InnerXml = childColsList.InnerXml;

            StringBuilder strColListBuilder = new StringBuilder();

            if (ReportStyle == "641" || ReportStyle == "642" || ReportStyle == "643")
            {
                XmlNode trxTreeColList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + trxTreeNodeName + "//GridHeading//Columns");
                foreach (XmlNode trxNode in trxTreeColList.ChildNodes)
                {
                    if (trxNode.Attributes["Label"].Value == "EndBal")
                    {
                        //nodeColumns.InnerXml = nodeColumns.InnerXml + trxNode.OuterXml;
                        strColListBuilder.Append(trxNode.OuterXml);
                        break;
                    }
                }

                if (ReportStyle == "642" || ReportStyle == "643")
                {
                    XmlNode trxSecondTreeColList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + trxSecondTreeNodeName + "//GridHeading//Columns");
                    foreach (XmlNode trxNode in trxSecondTreeColList.ChildNodes)
                    {
                        if (trxNode.Attributes["Label"].Value == "EndBal" || trxNode.Attributes["Label"].Value == "Ending")
                        {
                            if (trxNode.Attributes["Label"].Value == "EndBal")
                            {
                                trxNode.Attributes["Label"].Value = "Ending";
                            }
                            //nodeColumns.InnerXml = nodeColumns.InnerXml + trxNode.OuterXml;
                            strColListBuilder.Append(trxNode.OuterXml);
                            break;
                        }
                    }

                    if (ReportStyle == "643")
                    {
                        XmlNode trxThirdTreeColList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + trxThirdTreeNodeName + "//GridHeading//Columns");
                        foreach (XmlNode trxNode in trxThirdTreeColList.ChildNodes)
                        {
                            if (trxNode.Attributes["Label"].Value == "EndBal" || trxNode.Attributes["Label"].Value == "Ending")
                            {
                                if (trxNode.Attributes["Label"].Value == "EndBal")
                                {
                                    trxNode.Attributes["Label"].Value = "EndingBal";
                                }
                                //nodeColumns.InnerXml = nodeColumns.InnerXml + trxNode.OuterXml;
                                strColListBuilder.Append(trxNode.OuterXml);
                                break;
                            }
                        }
                    }

                    if (ReportStyle == "642")
                    {
                        XmlNode xVariance = xChildDoc.CreateNode(XmlNodeType.Element, "Col", null);

                        XmlAttribute labelAttr = xChildDoc.CreateAttribute("Label");
                        labelAttr.Value = "Variance";

                        XmlAttribute captionAttr = xChildDoc.CreateAttribute("Caption");
                        captionAttr.Value = "Variance";

                        XmlAttribute fullViewLengthAttr = xChildDoc.CreateAttribute("FullViewLength");
                        fullViewLengthAttr.Value = "10";

                        XmlAttribute controlTypeAttr = xChildDoc.CreateAttribute("ControlType");
                        controlTypeAttr.Value = "Amount";

                        xVariance.Attributes.Append(labelAttr);
                        xVariance.Attributes.Append(captionAttr);
                        xVariance.Attributes.Append(fullViewLengthAttr);
                        xVariance.Attributes.Append(controlTypeAttr);

                        //nodeColumns.InnerXml = nodeColumns.InnerXml + xVariance.OuterXml;
                        strColListBuilder.Append(xVariance.OuterXml);
                    }

                    if (ReportStyle == "643")
                    {
                        XmlNode xTotal = xChildDoc.CreateNode(XmlNodeType.Element, "Col", null);

                        XmlAttribute labelAttr = xChildDoc.CreateAttribute("Label");
                        labelAttr.Value = "Total";

                        XmlAttribute captionAttr = xChildDoc.CreateAttribute("Caption");
                        captionAttr.Value = "Total";

                        XmlAttribute fullViewLengthAttr = xChildDoc.CreateAttribute("FullViewLength");
                        fullViewLengthAttr.Value = "10";

                        XmlAttribute controlTypeAttr = xChildDoc.CreateAttribute("ControlType");
                        controlTypeAttr.Value = "Amount";

                        xTotal.Attributes.Append(labelAttr);
                        xTotal.Attributes.Append(captionAttr);
                        xTotal.Attributes.Append(fullViewLengthAttr);
                        xTotal.Attributes.Append(controlTypeAttr);

                        //nodeColumns.InnerXml = nodeColumns.InnerXml + xTotal.OuterXml;
                        strColListBuilder.Append(xTotal.OuterXml);
                    }
                }

                nodeColumns.InnerXml = nodeColumns.InnerXml + strColListBuilder.ToString();

                mxTrxNodeListColData = nodeColumns.SelectNodes("Col");
            }

            XmlNode nodeRowList = xChildDoc.CreateNode(XmlNodeType.Element, "RowList", null);
            treeNode.AppendChild(nodeRowList);
            nodeRowList.InnerXml = "";

            StringBuilder strRowListBuilder = new StringBuilder();

            XmlNode childRowList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + subChildTreeNodeName + "//RowList");
            foreach (XmlNode childNode in childRowList.ChildNodes)
            {
                if (childNode.Attributes["Link2"] != null && Convert.ToInt32(childNode.Attributes["Link2"].Value) == Link2)
                {
                    //nodeRowList.InnerXml = nodeRowList.InnerXml + childNode.OuterXml;
                    strRowListBuilder = strRowListBuilder.Append(childNode.OuterXml);
                }
            }

            nodeRowList.InnerXml = strRowListBuilder.ToString();

            if (ReportStyle == "641" || ReportStyle == "642" || ReportStyle == "643")
            {
                XmlNode trxRowList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + trxTreeNodeName + "//RowList");
                foreach (XmlNode childNode in nodeRowList.ChildNodes)
                {
                    XmlAttribute xEndBal = xChildDoc.CreateAttribute("EndBal");
                    xEndBal.Value = "";
                    childNode.Attributes.Append(xEndBal);

                    foreach (XmlNode trxNode in trxRowList.ChildNodes)
                    {
                        if (trxNode.Attributes["Link3"] != null && childNode.Attributes["Link3"].Value == trxNode.Attributes["Link3"].Value)
                        {
                            if (trxNode.Attributes["EndBal"] != null)
                            {
                                xEndBal.Value = trxNode.Attributes["EndBal"].Value;
                            }
                            break;
                        }
                    }
                }

                if (ReportStyle == "642" || ReportStyle == "643")
                {
                    XmlNode trxSecondRowList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + trxSecondTreeNodeName + "//RowList");
                    foreach (XmlNode childNode in nodeRowList.ChildNodes)
                    {
                        XmlAttribute xEndingBal = xChildDoc.CreateAttribute("Ending");
                        xEndingBal.Value = "";
                        childNode.Attributes.Append(xEndingBal);

                        foreach (XmlNode trxNode in trxSecondRowList.ChildNodes)
                        {
                            if (trxNode.Attributes["Link3"] != null && childNode.Attributes["Link3"].Value == trxNode.Attributes["Link3"].Value)
                            {
                                if (trxNode.Attributes["EndBal"] != null)
                                {
                                    xEndingBal.Value = trxNode.Attributes["EndBal"].Value;
                                }
                                break;
                            }
                        }
                    }

                    if (ReportStyle == "643")
                    {
                        XmlNode trxThirdRowList = m_xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + trxThirdTreeNodeName + "//RowList");
                        foreach (XmlNode childNode in nodeRowList.ChildNodes)
                        {
                            XmlAttribute xEndingBal = xChildDoc.CreateAttribute("EndingBal");
                            xEndingBal.Value = "";
                            childNode.Attributes.Append(xEndingBal);

                            foreach (XmlNode trxNode in trxThirdRowList.ChildNodes)
                            {
                                if (trxNode.Attributes["Link3"] != null && childNode.Attributes["Link3"].Value == trxNode.Attributes["Link3"].Value)
                                {
                                    if (trxNode.Attributes["EndBal"] != null)
                                    {
                                        xEndingBal.Value = trxNode.Attributes["EndBal"].Value;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            xChildDoc.AppendChild(nodeRoot);

            this.msFile = xChildDoc.OuterXml;
            this.isParent = false;
        }
        #endregion

        #region Public Methods
        public int LoadRecords()
        {
            return LoadRecords(this.msFile, this.msDataNodePath);
        }

        public int LoadRecords(string dataNodePath)
        {
            this.msDataNodePath = dataNodePath;
            return LoadRecords(this.msFile, dataNodePath);
        }

        public int LoadRecords(string xmlStoreFile, string dataNodePath)
        {
            int count = 0;
            this.msFile = xmlStoreFile;
            this.msDataNodePath = dataNodePath;
            try
            {
                //Create new instance of XmlDocument
                this.mxDoc = new XmlDocument();

                //Load it with the contents of the XmlStore file
                this.mxDoc.LoadXml(this.msFile);

                //Build Fields array with data in the schema
                this.msDataNodePath = "//Root//bpeout//FormControls//" + this.treeNodeName + "//RowList//Rows";

                this.LoadSchema(this.mxDoc);

                //Now load the actual data records
                if (this.mFields != null)
                {
                    if (this.Fields.Length > 0)
                    {
                        if (this.isParent)
                        {
                            //Select all the data record nodes 
                            this.mxParentNodeListData = this.mxDoc.SelectNodes(this.msDataNodePath);
                            count = this.mxParentNodeListData.Count;
                        }
                        else
                        {
                            this.mxChildNodeListData = this.mxDoc.SelectNodes(this.msDataNodePath);
                            count = this.mxChildNodeListData.Count;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string sMsg = ex.ToString();
                if (this.mbThrowExceptions)
                    throw new AccessViolationException(sMsg);
            }

            return count;
        }

        public int LoadSchema(XmlDocument xDoc)
        {
            int count = 0;
            int fieldCount = 0;
            this.mnIdColumn = -1;
            this.mbIdColumnAdded = false;
            try
            {
                if (xDoc != null)
                {
                    this.mxnodeSchema = xDoc.SelectSingleNode("//Root//bpeout//FormControls//" + this.treeNodeName + "//GridHeading//Columns");

                    if (mxnodeSchema != null || count > 0)
                    {
                        fieldCount = 0;

                        XmlNodeList xnodelistFields = xDoc.SelectNodes("//Root//bpeout//FormControls//" + this.treeNodeName + "//GridHeading//Columns/Col");

                        if (IsParent != true && isChild != true)
                        {
                            if (ReportStyle == "641" || ReportStyle == "642" || ReportStyle == "643")
                            {
                                xnodelistFields = mxTrxNodeListColData;
                            }
                        }

                        if (IsParent != true && isChild == true)
                        {
                            if (ReportStyle == "651" || ReportStyle == "653")
                            {
                                xnodelistFields = mxTrxNodeListColData;
                            }
                        }

                        if (xnodelistFields != null)
                        {
                            foreach (XmlElement elem in xnodelistFields)
                            {
                                if (elem.Attributes["FullViewLength"].Value != "0")
                                {
                                    if (elem.Attributes["Label"].Value != "Link1" && elem.Attributes["Label"].Value != "Link2")
                                    {
                                        switch (this.reportStyle)
                                        {
                                            default:
                                                {
                                                    this.mFields = new Field[fieldCount + 1];
                                                    fieldCount++;
                                                    break;
                                                }
                                            case "10":
                                                {
                                                    if (elem.Attributes["Label"].Value != "Classification" && elem.Attributes["Label"].Value != "Total")
                                                    {
                                                        this.mFields = new Field[fieldCount + 1];
                                                        fieldCount++;
                                                    }
                                                    break;
                                                }
                                            case "200":
                                                {
                                                    if (elem.Attributes["Label"].Value != "InvoiceComment")
                                                    {
                                                        this.mFields = new Field[fieldCount + 1];
                                                        fieldCount++;
                                                    }
                                                    break;
                                                }
                                            case "400":
                                                {
                                                    if (elem.Attributes["Label"].Value != "Caption5" && elem.Attributes["Label"].Value != "Amount5")
                                                    {
                                                        this.mFields = new Field[fieldCount + 1];
                                                        fieldCount++;
                                                    }
                                                    break;
                                                }
                                            case "622":
                                                {
                                                    if (elem.Attributes["Label"].Value != "SubTotal1" && elem.Attributes["Label"].Value != "SubTotal1Description")
                                                    {
                                                        this.mFields = new Field[fieldCount + 1];
                                                        fieldCount++;
                                                    }
                                                    break;
                                                }
                                            case "660":
                                            case "661":
                                                {
                                                    if (elem.Attributes["Label"].Value != "ProdSetRef" && elem.Attributes["Label"].Value != "NumberID")
                                                    {
                                                        this.mFields = new Field[fieldCount + 1];
                                                        fieldCount++;
                                                    }
                                                    break;
                                                }
                                        }

                                        #region Commented Code
                                        //if (this.reportStyle == "660" || this.reportStyle == "661")
                                        //{
                                        //    if (elem.Attributes["Label"].Value != "ProdSetRef" && elem.Attributes["Label"].Value != "NumberID")
                                        //    {
                                        //        this.mFields = new Field[fieldCount + 1];
                                        //        fieldCount++;
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    if (this.reportStyle == "622")
                                        //    {
                                        //        if (elem.Attributes["Label"].Value != "SubTotal1" && elem.Attributes["Label"].Value != "SubTotal1Description")
                                        //        {
                                        //            this.mFields = new Field[fieldCount + 1];
                                        //            fieldCount++;
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        if (ReportStyle == "200")
                                        //        {
                                        //            if (elem.Attributes["Label"].Value != "InvoiceComment")
                                        //            {
                                        //                this.mFields = new Field[fieldCount + 1];
                                        //                fieldCount++;
                                        //            }
                                        //        }
                                        //        else
                                        //        {
                                        //            if (ReportStyle == "10")
                                        //            {
                                        //                if (elem.Attributes["Label"].Value != "Classification" && elem.Attributes["Label"].Value != "Total")
                                        //                {
                                        //                    this.mFields = new Field[fieldCount + 1];
                                        //                    fieldCount++;
                                        //                }
                                        //            }
                                        //            else
                                        //            {
                                        //                if (ReportStyle == "400")
                                        //                {
                                        //                    if (elem.Attributes["Label"].Value != "Caption5" && elem.Attributes["Label"].Value != "Amount5")
                                        //                    {
                                        //                        this.mFields = new Field[fieldCount + 1];
                                        //                        fieldCount++;
                                        //                    }
                                        //                }
                                        //                else
                                        //                {
                                        //                    this.mFields = new Field[fieldCount + 1];
                                        //                    fieldCount++;
                                        //                }
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                        #endregion
                                    }
                                }
                            }

                            fieldCount = 0;
                            foreach (XmlElement elem in xnodelistFields)
                            {
                                if (elem.Attributes["FullViewLength"].Value != "0")
                                {
                                    if (elem.Attributes["Label"].Value != "Link1" && elem.Attributes["Label"].Value != "Link2")
                                    {
                                        switch (this.reportStyle)
                                        {
                                            default:
                                                {
                                                    this.mFields[fieldCount].label = elem.GetAttribute("Label");
                                                    this.mFields[fieldCount].caption = elem.GetAttribute("Caption");
                                                    this.mFields[fieldCount].controlType = elem.GetAttribute("ControlType");
                                                    this.mFields[fieldCount].type = DataType.String;
                                                    this.mFields[fieldCount].fullViewLength = elem.GetAttribute("FullViewLength");
                                                    fieldCount++;
                                                    break;
                                                }
                                            case "10":
                                                {
                                                    if (elem.Attributes["Label"].Value != "Classification" && elem.Attributes["Label"].Value != "Total")
                                                    {
                                                        this.mFields[fieldCount].label = elem.GetAttribute("Label");
                                                        this.mFields[fieldCount].caption = elem.GetAttribute("Caption");
                                                        this.mFields[fieldCount].controlType = elem.GetAttribute("ControlType");
                                                        this.mFields[fieldCount].type = DataType.String;
                                                        this.mFields[fieldCount].fullViewLength = elem.GetAttribute("FullViewLength");
                                                        fieldCount++;
                                                    }
                                                    break;
                                                }
                                            case "200":
                                                {
                                                    if (elem.Attributes["Label"].Value != "InvoiceComment")
                                                    {
                                                        this.mFields[fieldCount].label = elem.GetAttribute("Label");
                                                        this.mFields[fieldCount].caption = elem.GetAttribute("Caption");
                                                        this.mFields[fieldCount].controlType = elem.GetAttribute("ControlType");
                                                        this.mFields[fieldCount].type = DataType.String;
                                                        this.mFields[fieldCount].fullViewLength = elem.GetAttribute("FullViewLength");
                                                        fieldCount++;
                                                    }
                                                    break;
                                                }
                                            case "400":
                                                {
                                                    if (elem.Attributes["Label"].Value != "Caption5" && elem.Attributes["Label"].Value != "Amount5")
                                                    {
                                                        this.mFields[fieldCount].label = elem.GetAttribute("Label");
                                                        this.mFields[fieldCount].caption = elem.GetAttribute("Caption");
                                                        this.mFields[fieldCount].controlType = elem.GetAttribute("ControlType");
                                                        this.mFields[fieldCount].type = DataType.String;
                                                        this.mFields[fieldCount].fullViewLength = elem.GetAttribute("FullViewLength");
                                                        fieldCount++;
                                                    }
                                                    break;
                                                }
                                            case "622":
                                                {
                                                    if (elem.Attributes["Label"].Value != "SubTotal1" && elem.Attributes["Label"].Value != "SubTotal1Description")
                                                    {
                                                        this.mFields[fieldCount].label = elem.GetAttribute("Label");
                                                        this.mFields[fieldCount].caption = elem.GetAttribute("Caption");
                                                        this.mFields[fieldCount].controlType = elem.GetAttribute("ControlType");
                                                        this.mFields[fieldCount].type = DataType.String;
                                                        this.mFields[fieldCount].fullViewLength = elem.GetAttribute("FullViewLength");
                                                        fieldCount++;
                                                    }
                                                    break;
                                                }
                                            case "660":
                                            case "661":
                                                {
                                                    if (elem.Attributes["Label"].Value != "ProdSetRef" && elem.Attributes["Label"].Value != "NumberID")
                                                    {
                                                        this.mFields[fieldCount].label = elem.GetAttribute("Label");
                                                        this.mFields[fieldCount].caption = elem.GetAttribute("Caption");
                                                        this.mFields[fieldCount].controlType = elem.GetAttribute("ControlType");
                                                        this.mFields[fieldCount].type = DataType.String;
                                                        this.mFields[fieldCount].fullViewLength = elem.GetAttribute("FullViewLength");
                                                        fieldCount++;
                                                    }
                                                    break;
                                                }
                                        }

                                        #region Commented Code
                                        //if (this.reportStyle == "660" || this.reportStyle == "661")
                                        //{
                                        //    if (elem.Attributes["Label"].Value != "ProdSetRef" && elem.Attributes["Label"].Value != "NumberID")
                                        //    {
                                        //        this.mFields[fieldCount].label = elem.GetAttribute("Label");
                                        //        this.mFields[fieldCount].caption = elem.GetAttribute("Caption");
                                        //        this.mFields[fieldCount].controlType = elem.GetAttribute("ControlType");
                                        //        this.mFields[fieldCount].type = DataType.String;
                                        //        this.mFields[fieldCount].fullViewLength = elem.GetAttribute("FullViewLength");
                                        //        fieldCount++;
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    if (this.reportStyle == "622")
                                        //    {
                                        //        if (elem.Attributes["Label"].Value != "SubTotal1" && elem.Attributes["Label"].Value != "SubTotal1Description")
                                        //        {
                                        //            this.mFields[fieldCount].label = elem.GetAttribute("Label");
                                        //            this.mFields[fieldCount].caption = elem.GetAttribute("Caption");
                                        //            this.mFields[fieldCount].controlType = elem.GetAttribute("ControlType");
                                        //            this.mFields[fieldCount].type = DataType.String;
                                        //            this.mFields[fieldCount].fullViewLength = elem.GetAttribute("FullViewLength");
                                        //            fieldCount++;
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        //--- extract the label and caption for the field
                                        //        if (ReportStyle == "200")
                                        //        {
                                        //            if (elem.Attributes["Label"].Value != "InvoiceComment")
                                        //            {
                                        //                this.mFields[fieldCount].label = elem.GetAttribute("Label");
                                        //                this.mFields[fieldCount].caption = elem.GetAttribute("Caption");
                                        //                this.mFields[fieldCount].controlType = elem.GetAttribute("ControlType");
                                        //                this.mFields[fieldCount].type = DataType.String;
                                        //                this.mFields[fieldCount].fullViewLength = elem.GetAttribute("FullViewLength");
                                        //                fieldCount++;
                                        //            }
                                        //        }
                                        //        else
                                        //        {
                                        //            if (ReportStyle == "10")
                                        //            {
                                        //                if (elem.Attributes["Label"].Value != "Classification" && elem.Attributes["Label"].Value != "Total")
                                        //                {
                                        //                    this.mFields[fieldCount].label = elem.GetAttribute("Label");
                                        //                    this.mFields[fieldCount].caption = elem.GetAttribute("Caption");
                                        //                    this.mFields[fieldCount].controlType = elem.GetAttribute("ControlType");
                                        //                    this.mFields[fieldCount].type = DataType.String;
                                        //                    this.mFields[fieldCount].fullViewLength = elem.GetAttribute("FullViewLength");
                                        //                    fieldCount++;
                                        //                }
                                        //            }
                                        //            else
                                        //            {
                                        //                if (ReportStyle == "400")
                                        //                {
                                        //                    if (elem.Attributes["Label"].Value != "Caption5" && elem.Attributes["Label"].Value != "Amount5")
                                        //                    {
                                        //                        this.mFields[fieldCount].label = elem.GetAttribute("Label");
                                        //                        this.mFields[fieldCount].caption = elem.GetAttribute("Caption");
                                        //                        this.mFields[fieldCount].controlType = elem.GetAttribute("ControlType");
                                        //                        this.mFields[fieldCount].type = DataType.String;
                                        //                        this.mFields[fieldCount].fullViewLength = elem.GetAttribute("FullViewLength");
                                        //                        fieldCount++;
                                        //                    }
                                        //                }
                                        //                else
                                        //                {
                                        //                    this.mFields[fieldCount].label = elem.GetAttribute("Label");
                                        //                    this.mFields[fieldCount].caption = elem.GetAttribute("Caption");
                                        //                    this.mFields[fieldCount].controlType = elem.GetAttribute("ControlType");
                                        //                    this.mFields[fieldCount].type = DataType.String;
                                        //                    this.mFields[fieldCount].fullViewLength = elem.GetAttribute("FullViewLength");
                                        //                    fieldCount++;
                                        //                }
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                        #endregion
                                    }
                                }
                            }

                            if (this.mFields != null)
                            {
                                count = this.mFields.Length;
                            }
                            else
                            {
                                count = 0;
                            }
                        }
                    }
                    else
                    {
                        //XML file did not contain any schema info, so "build" it from
                        //The first record
                        XmlNodeList xnodes = this.mxDoc.DocumentElement.ChildNodes;
                        if (xnodes != null && xnodes.Count > 0)
                        {
                            XmlElement elem = (XmlElement)xnodes[0];
                            this.msDataNodePath = elem.Name;
                            int numAttributes = elem.Attributes.Count;
                            this.mFields = new Field[numAttributes];
                            for (int i = 0; i < numAttributes; i++)
                            {
                                this.mFields[i].label = elem.Attributes[i].Name;
                                this.mFields[i].caption = this.mFields[i].label;
                                this.mFields[i].type = DataType.String;
                            }

                            this.AdjustForUniqueID();

                            count = this.mFields.Length;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string sMsg = ex.ToString();
                Debug.WriteLine(sMsg);
                if (this.mbThrowExceptions)
                    throw new AccessViolationException(sMsg);
            }

            return count;
        }

        public long GenerateUID(int nRow)
        {
            DateTime dt = DateTime.Now.ToUniversalTime();
            long nID = dt.ToBinary();
            if (this.mbIdColumnAdded)
                nID += nRow;
            return nID;
        }

        public string GenerateSID(int nRow)
        {
            string sID = this.GenerateUID(nRow).ToString();
            return sID;
        }

        public string[] GetRecord(int nRow)
        {
            string[] rowCells = null;
            try
            {
                XmlElement elem = (XmlElement)mxParentNodeListData[nRow];

                if (elem != null)
                {
                    if (elem.GetAttribute(this.NameOfFieldWithUniqueID) == "")
                    {
                        string sID = this.GenerateSID(nRow);
                        elem.SetAttribute(this.NameOfFieldWithUniqueID, sID);
                    }
                    XmlAttributeCollection attributes = elem.Attributes;
                    int cols = this.Fields.Length;
                    if (rowCells == null)
                    {
                        rowCells = new string[cols];
                    }

                    XmlNodeList xnodelistFields = mxDoc.SelectNodes("Root/bpeout/FormControls/" + this.treeNodeName + "/GridHeading/Columns/Col");
                    int rowCellCount = 0;

                    foreach (XmlElement elm in xnodelistFields)
                    {
                        if (elm.Attributes["FullViewLength"].Value != "0")
                        {
                            if (elm.Attributes["Label"].Value != "Link1")
                            {
                                if (ReportStyle == "10")
                                {
                                    if (elm.Attributes["Label"].Value != "Classification" && elm.Attributes["Label"].Value != "Total")
                                    {
                                        if (attributes[elm.Attributes["Label"].Value] != null)
                                        {
                                            rowCells[rowCellCount] = attributes[elm.Attributes["Label"].Value].Value;
                                        }
                                        else
                                        {
                                            rowCells[rowCellCount] = "";
                                        }
                                        rowCellCount++;
                                    }
                                }
                                else
                                {
                                    if (attributes[elm.Attributes["Label"].Value] != null)
                                    {
                                        rowCells[rowCellCount] = attributes[elm.Attributes["Label"].Value].Value;
                                    }
                                    else
                                    {
                                        rowCells[rowCellCount] = "";
                                    }
                                    rowCellCount++;
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                string sMsg = ex.ToString();
                Debug.WriteLine(sMsg);
                if (this.mbThrowExceptions)
                    throw new AccessViolationException(sMsg);
            }

            return rowCells;
        }

        public string[] GetChildRecord(int nRow)
        {
            string[] rowCells = null;
            try
            {
                XmlElement elem = (XmlElement)mxChildNodeListData[nRow];

                if (elem != null)
                {
                    if (elem.GetAttribute(this.NameOfFieldWithUniqueID) == "")
                    {
                        string sID = this.GenerateSID(nRow);
                        elem.SetAttribute(this.NameOfFieldWithUniqueID, sID);
                    }

                    XmlAttributeCollection attributes = elem.Attributes;
                    int cols = this.Fields.Length;
                    if (rowCells == null)
                    {
                        rowCells = new string[cols];
                    }

                    XmlNodeList xnodelistFields = mxDoc.SelectNodes("//Root//bpeout//FormControls//" + this.treeNodeName + "//GridHeading//Columns/Col");

                    int rowCellCount = 0;

                    foreach (XmlElement elm in xnodelistFields)
                    {
                        if (elm.Attributes["FullViewLength"].Value != "0")
                        {
                            if (elm.Attributes["Label"].Value != "Link1")
                            {
                                switch (this.reportStyle)
                                {
                                    default:
                                        {
                                            if (attributes[elm.Attributes["Label"].Value] != null)
                                            {
                                                rowCells[rowCellCount] = attributes[elm.Attributes["Label"].Value].Value;
                                            }
                                            else
                                            {
                                                rowCells[rowCellCount] = "";
                                            }

                                            rowCellCount++;
                                            break;
                                        }
                                    case "660":
                                    case "661":
                                        {
                                            if (elm.Attributes["Label"].Value != "ProdSetRef" && elm.Attributes["Label"].Value != "NumberID")
                                            {
                                                if (attributes[elm.Attributes["Label"].Value] != null)
                                                {
                                                    rowCells[rowCellCount] = attributes[elm.Attributes["Label"].Value].Value;
                                                }
                                                else
                                                {
                                                    rowCells[rowCellCount] = "";
                                                }

                                                rowCellCount++;
                                            }
                                            break;
                                        }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string sMsg = ex.ToString();
                if (this.mbThrowExceptions)
                    throw new AccessViolationException(sMsg);
            }

            return rowCells;
        }

        public int GetColumnWidthsTotal()
        {
            int fullViewLengthTotal = 0;
            int col;

            for (col = 0; col < this.Fields.Length; col++)
            {
                fullViewLengthTotal += this.GetColumnWidth(col);
            }

            return fullViewLengthTotal;
        }

        public int GetColumnWidth(int col)
        {
            int fullViewLengthCol = 0;

            try
            {
                if (0 <= col && col < this.Fields.Length
                    && this.Fields[col].fullViewLength.Length > 0
                    && this.Fields[col].fullViewLength != null)
                    fullViewLengthCol = (int)Convert.ToDouble(this.Fields[col].fullViewLength);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                fullViewLengthCol = 0;
            }

            return fullViewLengthCol;
        }
        #endregion
    }
}