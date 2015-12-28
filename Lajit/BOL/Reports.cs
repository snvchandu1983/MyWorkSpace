using System;
using System.Xml;
using System.Data;
using System.Collections;
using System.Configuration;
using LAjit_DAL;
using NLog;


namespace LAjit_BO
{
    public class Reports
    {
        public NLog.Logger logger = LogManager.GetCurrentClassLogger();
        private CommonDAL objDAL = new CommonDAL();
        private static string fileString = ConfigurationSettings.AppSettings["XmlFilePath"].ToString();
        Hashtable m_htGVColsPtn = new Hashtable();

        private string m_TrxID;

        public string TrxID
        {
            get { return m_TrxID; }
            set { m_TrxID = value; }
        }

        private string m_TrxType;

        public string TrxType
        {
            get { return m_TrxType; }
            set { m_TrxType = value; }
        }


        /// <summary>
        /// Gets the ForeGround Reporting Return XMl
        /// </summary>
        /// <param name="requestXML">requestXML</param>
        /// <param name="rptinfo">rptinfo Selected from DropDown</param>
        public string GetForeGroundReqXML(String requestXML, String rptinfo, String BPE)
        {
            
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(requestXML);

            //Creating the Report node
            XmlNode nodeReport = xDoc.CreateNode(XmlNodeType.Element, "Report", null);

            //Process Attribute
            XmlAttribute attProcess = xDoc.CreateAttribute("Process");
            attProcess.Value = "1";
            nodeReport.Attributes.Append(attProcess);

            //RedistributionID Attribute
            XmlAttribute attRedistributionID = xDoc.CreateAttribute("RedistributionID");
            attRedistributionID.Value = rptinfo;
            nodeReport.Attributes.Append(attRedistributionID);

            //Appending Report Node
            xDoc.SelectSingleNode("bpinfo").AppendChild(nodeReport);

            string bpinfo = xDoc.SelectSingleNode("bpinfo").OuterXml;

            //Creating the Root and bpe node
            XmlDocument xDocGV = new XmlDocument();
            XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);
            nodeRoot.InnerXml = BPE;

            //Creating the bpinfo node
            nodeRoot.InnerXml += bpinfo;

            //Retriieving the BPEOut and ReportOut XML
            string reqxml = nodeRoot.OuterXml;
            return reqxml;
        }

        /// <summary>
        /// Gets the BPEOut for the reports
        /// </summary>
        /// <param name="inputXML">RequestXML</param>        
        public string GetReportBPEOut(string inputXML)
        {
            #region NLog
            logger.Info("Gets the BPEOut for the reports."); 
            #endregion

            return objDAL.GetBPEOut(inputXML);
        }

        /// <summary>
        /// Generates the Notes DT
        /// </summary>
        /// <param></param>
        public DataTable GenerateNotesDatatable(DataTable dt)
        {
            DataTable dtNotes = new DataTable();

            try
            {
                DataColumn dcSNo = new DataColumn("S No", typeof(Int32));
                DataColumn dcNotes = new DataColumn("Notes", typeof(string));

                dtNotes.Columns.Add(dcSNo);
                dtNotes.Columns.Add(dcNotes);

                int slNo = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Notes"].ToString().Trim().Length > 0)
                    {
                        DataRow drow = dtNotes.NewRow();
                        drow["S No"] = slNo;
                        drow["Notes"] = dr["Notes"].ToString();
                        dtNotes.Rows.Add(drow);
                    }
                    slNo++;
                }
            }
            catch 
            {
                #region NLog
                logger.Fatal("Generate Notes Datatable"); 
                #endregion
            }
            return dtNotes;
        }

        /// <summary>
        /// Gets the BackGround Reporting Return XMl
        /// </summary>
        /// <param name="requestXML">requestXML</param>
        /// <param name="rptinfo">rptinfo Selected from DropDown</param>
        public string GetBackGroundReqXML(String requestXML, String rptinfo, String BPE)
        {
            //retrieving BPE and BPINFO from request XML
            //string requestXMl = ucCPGV1.GVRequestXml.ToString();
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(requestXML);

            //Creating the Report node
            XmlNode nodeReport = xDoc.CreateNode(XmlNodeType.Element, "Report", null);

            //Process Attribute
            XmlAttribute attProcess = xDoc.CreateAttribute("Process");
            attProcess.Value = "1";
            nodeReport.Attributes.Append(attProcess);

            //RedistributionID Attribute
            XmlAttribute attRedistributionID = xDoc.CreateAttribute("RedistributionID");
            attRedistributionID.Value = rptinfo;
            nodeReport.Attributes.Append(attRedistributionID);

            //Appending Report Node
            xDoc.SelectSingleNode("bpinfo").AppendChild(nodeReport);

            string bpinfo = xDoc.SelectSingleNode("bpinfo").OuterXml;



            //string bpInfo = xDoc.SelectSingleNode("Root/bpinfo").InnerXml;
            //string bpe = xDoc.SelectSingleNode("Root/bpe").OuterXml;

            //Creating new Request XML
            //Creating the Root 
            XmlDocument xDocGV = new XmlDocument();
            XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);

            XmlNode nodeWF = xDocGV.CreateNode(XmlNodeType.Element, "Workflow", null);
            //Process Attribute
            XmlAttribute attWFProcess = xDocGV.CreateAttribute("Process");
            attWFProcess.Value = "1";
            nodeWF.Attributes.Append(attWFProcess);

            //ProcessLevel Attribute
            XmlAttribute attProcessLevel = xDocGV.CreateAttribute("ProcessLevel");
            attProcessLevel.Value = "-1";
            nodeWF.Attributes.Append(attProcessLevel);

            //Process Attribute
            XmlAttribute attType = xDocGV.CreateAttribute("Type");
            attType.Value = "1";
            nodeWF.Attributes.Append(attType);

            //Appending WorkFlow Node
            nodeRoot.AppendChild(nodeWF);

            //Appending bpe node
            nodeRoot.InnerXml += BPE;

            nodeRoot.InnerXml += bpinfo;


            //Creating the WorkFlow node


            ////Creating the bpinfo node
            //XmlNode nodeBPInfo = xDocGV.CreateNode(XmlNodeType.Element, "bpinfo", null);
            ////Appending BPInfo Node
            //nodeRoot.AppendChild(nodeBPInfo);
            ////Adding BPInfo 
            //nodeBPInfo.InnerXml = bpInfo;

            ////Creating the Report node
            //XmlNode nodeReport = xDocGV.CreateNode(XmlNodeType.Element, "Report", null);

            ////Process Attribute
            //XmlAttribute attProcess = xDocGV.CreateAttribute("Process");
            //attProcess.Value = "1";
            //nodeReport.Attributes.Append(attProcess);

            ////RedistributionID Attribute
            //XmlAttribute attRedistributionID = xDocGV.CreateAttribute("RedistributionID");
            //attRedistributionID.Value = rptinfo;
            //nodeReport.Attributes.Append(attRedistributionID);
            ////Appending Report Node
            //nodeBPInfo.AppendChild(nodeReport);

            string reqxml = nodeRoot.OuterXml;
            return reqxml;
            //return "1";
        }

        public DataTable PivotTable(DataTable source)
        {
            DataTable dest = new DataTable("Pivoted" + source.TableName);

            // create shchema (string columns) for the destination
            // the first column is for the source column name
            dest.Columns.Add(" ");

            // the remaining dest columns are from each source table row (1stcolumn)
            foreach (DataRow r in source.Rows)
                dest.Columns.Add(r[0].ToString()); // assign each row the Productname (r[0])

            // now add one row to the dest table for each column in the source,except
            // the first which is the Product, in our case
            for (int i = 0; i < source.Columns.Count - 1; i++)
            {
                dest.Rows.Add(dest.NewRow());
            }

            // now move the source columns to their position in the destrow/cell matrix
            // starting down the destination rows, and across the columns
            for (int r = 0; r < dest.Rows.Count; r++)
            {
                for (int c = 0; c < dest.Columns.Count; c++)
                {
                    if (c == 0)
                        dest.Rows[r][0] = source.Columns[r + 1].ColumnName; // the Product name
                    else
                        dest.Rows[r][c] = source.Rows[c - 1][r + 1];
                }
            }
            dest.AcceptChanges();
            return dest;
        }

        /// <summary>
        /// Generated the xml to request data to be bound for the grid view.
        /// </summary>
        /// <param name="BPGID">BPGID of the grid view</param>
        /// <returns>XML Data.</returns>
        public string GenGenericProcessRequestXML(string BPEAction, string BPValue, string CntrlValues, string BPE, bool gridExists, string pageSize, string pageNo, string BPInfo, string treeNodeName, string gvXML)
        {
            #region NLog
            logger.Info("Generated the xml to request data to be bound for the grid view.");
            #endregion

            string ReqXMl = string.Empty;
            try
            {
                string m_GVTreeNodeName = string.Empty;
                XmlDocument xDocGV = new XmlDocument();
                XmlNode nodeRoot = xDocGV.CreateNode(XmlNodeType.Element, "Root", null);

                nodeRoot.InnerXml += BPE;

                if (BPEAction.ToUpper().Trim() != "PAGELOAD")
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
                        if (gvXML != string.Empty)
                        {
                            nodeBPInfo.InnerXml += gvXML;
                        }

                        XmlNode nodeAccountingLayout = xDocGV.CreateNode(XmlNodeType.Element, treeNodeName, null);
                        nodeBPInfo.AppendChild(nodeAccountingLayout);
                        //Creating the Rowlist node
                        XmlNode nodeRowList = xDocGV.CreateNode(XmlNodeType.Element, "RowList", null);

                        if (BPEAction.ToUpper().Trim() == "REPORT")
                        {
                            xDocGV.AppendChild(nodeRowList);
                            nodeRowList.InnerXml = CntrlValues;

                            XmlAttribute attrBPAction = xDocGV.CreateAttribute("BPAction");
                            attrBPAction.Value = BPEAction;
                            XmlNode nodeRow= nodeRowList.SelectSingleNode("Rows");
                            nodeRow.Attributes.Append(attrBPAction);
                            if (!string.IsNullOrEmpty(TrxID) && !string.IsNullOrEmpty(TrxType))
                            {
                                XmlAttribute attTrxID = xDocGV.CreateAttribute("TrxID");
                                attTrxID.Value = TrxID;
                                XmlAttribute attTrxType = xDocGV.CreateAttribute("TrxType");
                                attTrxType.Value = TrxType;
                                nodeRow.Attributes.Prepend(attTrxType);
                                nodeRow.Attributes.Prepend(attTrxID);
                            }

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
                if (treeNodeName != string.Empty)
                {
                    //Setting PageNo and PageSize tags
                    XmlDocument xDocum = new XmlDocument();
                    xDocum.LoadXml(nodeRoot.OuterXml);
                    XmlNode bpInfoNode = xDocum.SelectSingleNode("Root/bpinfo");
                    XmlNode treenode = xDocum.SelectSingleNode("Root/bpinfo/" + treeNodeName);
                    if (treenode != null)
                    {
                        treenode.InnerXml += @" <Gridview><Pagenumber>" + "1" + "</Pagenumber><Pagesize>" + "-1" + "</Pagesize><Sortcolumn></Sortcolumn><Sortorder></Sortorder></Gridview>";
                    }

                    ReqXMl = xDocum.OuterXml;
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex); 
                #endregion
            }
            return ReqXMl;
        }

        //ReportSubmit

        #region Commented Print Branch Methods
        /*
        public DataTable GetFullDetailsToPrint(string GVXml)
        {
            DataTable dt = new DataTable();
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(GVXml);

            //Get the Grid Layout nodes
            string treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

            //Getting the dataset to be bound to the grid.
            XmlNode nodeRows = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/RowList");
            XmlNodeReader read = new XmlNodeReader(nodeRows);
            DataSet dsRows = new DataSet();
            dsRows.ReadXml(read);

            //Getting the datatable                
            dt = dsRows.Tables[0];

            //Getting the columns to be displayed in grid
            XmlNode nodeCols = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + treeNodeName + "/GridHeading/Columns");
            m_htGVColumns.Clear();
            int colPos = 0;
            //Storing the columns names and captions in the HashTable
            foreach (XmlNode node in nodeCols)
            {
                if (!m_htGVColumns.Contains(node.Attributes["Label"].Value))
                {
                    if (node.Attributes["FullViewLength"].Value != "0")
                    {
                        m_htGVColumns.Add(node.Attributes["Label"].Value, node.Attributes["Caption"].Value);
                    }
                }
                if (!m_htGVColsPtn.Contains(node.Attributes["Label"].Value))
                {
                    if (node.Attributes["FullViewLength"].Value != "0")
                    {
                        m_htGVColsPtn.Add(node.Attributes["Label"].Value, colPos);
                    }
                }
                colPos++;
            }

            //Columns to be removed
            ArrayList arrColToRemove = new ArrayList();
            foreach (DataColumn dc in dt.Columns)
            {
                if (!(m_htGVColumns.ContainsKey(dc.ColumnName)))
                {
                    if (dc.ColumnName.ToString().Trim() != "Notes")
                    {
                        arrColToRemove.Add(dc);
                    }
                }
            }
            //Removing the redundant columns 
            foreach (DataColumn Rdc in arrColToRemove)
            {
                if (Rdc.ColumnName.ToString().Trim() != "TrxID")
                {
                    dt.Columns.Remove(Rdc);
                }
            }

            ////ReOrdering the Columns in the Datatable
            ////foreach (DictionaryEntry de in m_htGVColsPtn)
            ////{                                
            ////    string currentLabel = de.Key.ToString();
            ////    int currentColPos = (Int32)de.Value;
            ////    foreach(DataColumn dc in dt.Columns)
            ////    {
            ////        if (dc.ColumnName == currentLabel)
            ////        {
            ////            dc.SetOrdinal(currentColPos);
            ////        }
            ////    }
            ////}

            //Setting the Caption
            foreach (DictionaryEntry de in m_htGVColumns)
            {
                string currentCaption = de.Value.ToString();
                string currentLabel = de.Key.ToString();
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.ColumnName.ToString() == currentLabel)
                    {
                        dc.ColumnName = currentCaption;
                    }
                }
            }

            //DataTable dt = GetDataToPrint(GVXml);
            dt.Columns.Add("New");
            dt.Columns["New"].SetOrdinal(0);
            DataTable pDT = PivotTable(dt);
            return pDT;

        }

        public DataTable GetBranchDT(string GVXml, string parentTrxID)
        {
            DataTable branchDT = new DataTable();
            DataTable branchesDT = new DataTable();

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(GVXml);

            //Get the Grid Layout nodes
            string treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

            XmlNode nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
            if (nodeBranches != null)
            {
                foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                {
                    if (nodeBranch.Attributes["ControlType"] == null)
                    {
                        branchDT.Clear();
                        string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                        XmlNodeList nodeResRows = xDoc.SelectNodes("//" + branchNodeName + "/RowList/Rows[@" + treeNodeName + "_TrxID = '" + parentTrxID + "']");
                        XmlDocument branchDoc = new XmlDocument();
                        //Creating the new node
                        XmlNode nodeBranchRowList = xDoc.CreateNode(XmlNodeType.Element, "RowList", null);                        
                        
                        if (nodeResRows.Count>0)
                        {                            
                            //XmlNode nodeResRow = nodeResRows.Item(0);
                            for (int nodeRowCnt = 0; nodeRowCnt < nodeResRows.Count; nodeRowCnt++)
                            {
                                nodeBranchRowList.AppendChild(nodeResRows.Item(nodeRowCnt));
                            }

                            if (nodeBranchRowList != null)
                            {
                                XmlNodeReader read = new XmlNodeReader(nodeBranchRowList);
                                DataSet dsResRows = new DataSet();
                                dsResRows.ReadXml(read);

                                //Getting the datatable                
                                branchDT = dsResRows.Tables[0];
                                //Getting the columns to be displayed in grid
                                XmlNode nodeCols = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                                m_htGVColumns.Clear();
                                int colPos = 0;
                                //Storing the columns names and captions in the HashTable
                                foreach (XmlNode node in nodeCols)
                                {
                                    if (!m_htGVColumns.Contains(node.Attributes["Label"].Value))
                                    {
                                        //if (node.Attributes["FullViewLength"].Value != "0")
                                        if (node.Attributes["Label"].Value.ToString().Trim() != "IsPrimary")
                                        {
                                            m_htGVColumns.Add(node.Attributes["Label"].Value, node.Attributes["Caption"].Value);
                                        }
                                    }
                                    if (!m_htGVColsPtn.Contains(node.Attributes["Label"].Value))
                                    {
                                        //if (node.Attributes["FullViewLength"].Value != "0")
                                        {
                                            m_htGVColsPtn.Add(node.Attributes["Label"].Value, colPos);
                                        }
                                    }
                                    colPos++;
                                }

                                //Columns to be removed
                                ArrayList arrColToRemove = new ArrayList();
                                foreach (DataColumn dc in branchDT.Columns)
                                {
                                    if (!(m_htGVColumns.ContainsKey(dc.ColumnName.Trim().ToString())))
                                    {
                                        //if (dc.ColumnName.ToString().Trim() != "Notes")
                                        {
                                            arrColToRemove.Add(dc);
                                        }
                                    }
                                }
                                //Removing the redundant columns 
                                foreach (DataColumn Rdc in arrColToRemove)
                                {
                                    branchDT.Columns.Remove(Rdc);
                                }

                                ////ReOrdering the Columns in the Datatable
                                ////foreach (DictionaryEntry de in m_htGVColsPtn)
                                ////{                                
                                ////    string currentLabel = de.Key.ToString();
                                ////    int currentColPos = (Int32)de.Value;
                                ////    foreach(DataColumn dc in dt.Columns)
                                ////    {
                                ////        if (dc.ColumnName == currentLabel)
                                ////        {
                                ////            dc.SetOrdinal(currentColPos);
                                ////        }
                                ////    }
                                ////}

                                //Setting the Caption
                                foreach (DictionaryEntry de in m_htGVColumns)
                                {
                                    string currentCaption = de.Value.ToString();
                                    string currentLabel = de.Key.ToString();
                                    foreach (DataColumn dc in branchDT.Columns)
                                    {
                                        if (dc.ColumnName.ToString() == currentLabel)
                                        {
                                            dc.ColumnName = currentCaption;
                                        }
                                    }
                                }

                                //Appending a SubHead column                                
                                DataColumn colSubHead = new DataColumn();
                                colSubHead.ColumnName = branchNodeName;
                                branchDT.Columns.Add(colSubHead);
                                colSubHead.SetOrdinal(0);
                            }
                        }
                    }

                    if (branchDT.Rows.Count > 0)
                    {
                        if (branchDT.Columns.Contains("Description"))
                        {
                            branchDT.Columns["Description"].SetOrdinal(1);
                        }
                        if (branchDT.Columns.Contains("New"))
                        {
                            branchDT.Columns.Remove("New");
                        }
                        branchDT.Columns.Add("New");
                        branchDT.Columns["New"].SetOrdinal(0);
                        DataTable pDT = PivotTable(branchDT);
                        if (branchesDT.Columns.Count == 0)
                        {
                            for (int cols = 0; cols < 2; cols++)
                            {
                                //Creating Data table containing the column names                        
                                DataColumn colNames = new DataColumn();
                                colNames.ColumnName = pDT.Columns[cols].ColumnName;
                                colNames.DataType = pDT.Columns[cols].DataType;
                                branchesDT.Columns.Add(colNames);
                            }
                        }

                        foreach (DataRow dBranchRow in pDT.Rows)
                        {
                            DataRow dNewRow = branchesDT.NewRow();

                            for (int cols = 0; cols < 2; cols++)
                            {
                                dNewRow[cols] = dBranchRow[cols];
                            }
                            branchesDT.Rows.Add(dNewRow);
                        }
                        ////Adding empty row
                        //DataRow dEmptyRow = branchesDT.NewRow();
                        //dEmptyRow[0] = string.Empty;
                        //dEmptyRow[1] = string.Empty;
                        //branchesDT.Rows.Add(dEmptyRow);
                    }
                    //branchesDT

                }
            }
            //branchDT.Columns.Add("New");
            //branchDT.Columns["New"].SetOrdinal(0);
            //DataTable pDT = PivotTable(branchDT);
            return branchesDT;            
        }

        public DataTable GetBranchDataToPrint(string GVXml, string parentTrxID)
        {
            DataTable branchDT = new DataTable();
            DataTable branchesDT = new DataTable();

            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(GVXml);

            //Get the Grid Layout nodes
            string treeNodeName = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Node").InnerText;

            XmlNode nodeBranches = xDoc.SelectSingleNode("Root/bpeout/FormControls/GridLayout/Tree/Branches");
            if (nodeBranches != null)
            {
                foreach (XmlNode nodeBranch in nodeBranches.ChildNodes)
                {
                    if (nodeBranch.Attributes["ControlType"] == null)
                    {
                        branchDT.Clear();
                        string branchNodeName = nodeBranch.SelectSingleNode("Node").InnerText;
                        XmlNodeList nodeResRows = xDoc.SelectNodes("//" + branchNodeName + "/RowList/Rows[@" + treeNodeName + "_TrxID = '" + parentTrxID + "']");
                        
                        XmlDocument branchDoc = new XmlDocument();
                        //Creating the new node
                        XmlNode nodeBranchRowList = xDoc.CreateNode(XmlNodeType.Element, "RowList", null);

                        if (nodeResRows.Count > 0)
                        {
                            //XmlNode nodeResRow = nodeResRows.Item(0);
                            for (int nodeRowCnt = 0; nodeRowCnt < nodeResRows.Count; nodeRowCnt++)
                            {
                                nodeBranchRowList.AppendChild(nodeResRows.Item(nodeRowCnt));
                            }

                            if (nodeBranchRowList != null)
                            {
                                XmlNode nodePrimaryRow = nodeBranchRowList.SelectSingleNode("RowList/Rows[@IsPrimary='1']");

                                XmlNodeReader read = new XmlNodeReader(nodeBranchRowList);
                                DataSet dsResRows = new DataSet();
                                dsResRows.ReadXml(read);

                                //Getting the datatable                
                                branchDT = dsResRows.Tables[0];
                                //Getting the columns to be displayed in grid
                                XmlNode nodeCols = xDoc.SelectSingleNode("Root/bpeout/FormControls/" + branchNodeName + "/GridHeading/Columns");
                                m_htGVColumns.Clear();
                                int colPos = 0;
                                //Storing the columns names and captions in the HashTable
                                foreach (XmlNode node in nodeCols)
                                {
                                    if (!m_htGVColumns.Contains(node.Attributes["Label"].Value))
                                    {
                                        //if (node.Attributes["FullViewLength"].Value != "0")
                                        if (node.Attributes["Label"].Value.ToString().Trim() != "IsPrimary")
                                        {
                                            m_htGVColumns.Add(node.Attributes["Label"].Value, node.Attributes["Caption"].Value);
                                        }
                                    }
                                    if (!m_htGVColsPtn.Contains(node.Attributes["Label"].Value))
                                    {
                                        //if (node.Attributes["FullViewLength"].Value != "0")
                                        {
                                            m_htGVColsPtn.Add(node.Attributes["Label"].Value, colPos);
                                        }
                                    }
                                    colPos++;
                                }

                                //Columns to be removed
                                ArrayList arrColToRemove = new ArrayList();
                                foreach (DataColumn dc in branchDT.Columns)
                                {
                                    if (!(m_htGVColumns.ContainsKey(dc.ColumnName.Trim().ToString())))
                                    {
                                        //if (dc.ColumnName.ToString().Trim() != "Notes")
                                        {
                                            arrColToRemove.Add(dc);
                                        }
                                    }
                                }
                                //Removing the redundant columns 
                                foreach (DataColumn Rdc in arrColToRemove)
                                {
                                    branchDT.Columns.Remove(Rdc);
                                }

                                ////ReOrdering the Columns in the Datatable
                                ////foreach (DictionaryEntry de in m_htGVColsPtn)
                                ////{                                
                                ////    string currentLabel = de.Key.ToString();
                                ////    int currentColPos = (Int32)de.Value;
                                ////    foreach(DataColumn dc in dt.Columns)
                                ////    {
                                ////        if (dc.ColumnName == currentLabel)
                                ////        {
                                ////            dc.SetOrdinal(currentColPos);
                                ////        }
                                ////    }
                                ////}

                                //Setting the Caption
                                foreach (DictionaryEntry de in m_htGVColumns)
                                {
                                    string currentCaption = de.Value.ToString();
                                    string currentLabel = de.Key.ToString();
                                    foreach (DataColumn dc in branchDT.Columns)
                                    {
                                        if (dc.ColumnName.ToString() == currentLabel)
                                        {
                                            dc.ColumnName = currentCaption;
                                        }
                                    }
                                }

                                //Appending a SubHead column                                
                                DataColumn colSubHead = new DataColumn();
                                colSubHead.ColumnName = branchNodeName;
                                branchDT.Columns.Add(colSubHead);
                                colSubHead.SetOrdinal(0);
                            }
                        }
                    }
                    if (branchDT.Rows.Count > 0)
                    {
                        if (branchDT.Columns.Contains("Description"))
                        {
                            branchDT.Columns["Description"].SetOrdinal(1);
                        }
                        //if (branchesDT.Columns.Count == 0)
                        //{
                        //    for (int cols = 0; cols < branchDT.Columns.Count; cols++)
                        //    {
                        //        //Creating Data table containing the column names                        
                        //        DataColumn colNames = new DataColumn();
                        //        colNames.ColumnName = pDT.Columns[cols].ColumnName;
                        //        colNames.DataType = pDT.Columns[cols].DataType;
                        //        branchesDT.Columns.Add(colNames);
                        //    }
                        //}
                    }
                }
            }
            return branchesDT;


        }

         */
        #endregion
    }
}
