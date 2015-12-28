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
using Gios.Pdf;
using System.Drawing;


namespace LAjitDev
{
    public partial class rptPDF
    {
        

        #region ReportStyle 512
        public void ReportStyle512(DataTable dt, DataTable dtHeader, Hashtable htPFormatModes, bool PLayout, int[] colWidths, DataTable dtChild, Hashtable htbPFormats, bool bPLayout, int[] bcolWidths, string fileName)
        {
            try
            {
                double posY = 0;
                double totPosY = 410;
                //
                myPdfDocument = new PdfDocument(PdfDocumentFormat.InCentimeters(21.6, 27.9));
                string parentTrxID = string.Empty;
                string parentTableName = string.Empty;
                Font HeaderFont = new Font("Verdana", 7, FontStyle.Regular);
                Font ColumnsFont = new Font("Verdana", 8, FontStyle.Regular);
                Font BoldFont = new Font("Verdana", 9, FontStyle.Bold);
                //
                if (dt != null)
                {
                    newPdfPage = myPdfDocument.NewPage();
                    for (int rw = 0; rw < dt.Rows.Count; rw++)
                    {
                        parentTrxID = dt.Rows[rw]["TrxID"].ToString();
                        parentTableName = dt.TableName.ToString();
                        //
                        if (dt.Rows.Count > 0)
                        {
                            if (dtChild != null)
                            {
                                //DataRow[] drFiltered = null;
                                //drFiltered = dtChild.Select(parentTableName + "_TrxID='" + parentTrxID + "'");
                                if (dtChild.Rows.Count > 0)
                                {
                                    for (int filRws = 0; filRws < dtChild.Rows.Count; filRws++)
                                    {
                                        if (filRws % 2 != 0)
                                        {
                                            posY = totPosY;
                                        }
                                        else
                                        {
                                            posY = 0;
                                        }
                                        int pos = filRws + 1;
                                        if (pos != 1)
                                        {
                                            if ((pos % 2) == 1)
                                            {
                                                newPdfPage.SaveToDocument();
                                                newPdfPage = myPdfDocument.NewPage();
                                            }
                                        }
                                        #region Parent
                                        foreach (DataColumn dcs in dt.Columns)
                                        {
                                            switch (dcs.ColumnName.ToString())
                                            {
                                                //case "FederalTaxID":
                                                //    {
                                                //        if (!string.IsNullOrEmpty(dt.Rows[0][dcs.ColumnName].ToString()))
                                                //        {
                                                //            PdfTextArea pTxtArea = new PdfTextArea(ColumnsFont,
                                                //                                Color.Black, new PdfArea(myPdfDocument, 40, posY + 135, 50, 50),
                                                //                                ContentAlignment.MiddleLeft, dt.Rows[0][dcs.ColumnName].ToString());
                                                //            newPdfPage.Add(pTxtArea);
                                                //        }
                                                //        break;
                                                //    }
                                                case "Company":
                                                    {
                                                        if (!string.IsNullOrEmpty(dt.Rows[0][dcs.ColumnName].ToString()))
                                                        {//54.5 //121.50
                                                            PdfTextArea pTxtArea = new PdfTextArea(BoldFont,
                                                                                Color.Black, new PdfArea(myPdfDocument, 63, posY + 73, 230, 50),
                                                                                ContentAlignment.MiddleLeft, dt.Rows[0][dcs.ColumnName].ToString());
                                                            newPdfPage.Add(pTxtArea);
                                                        }
                                                        break;
                                                    }
                                                case "Address1":
                                                    {
                                                        if (!string.IsNullOrEmpty(dt.Rows[0][dcs.ColumnName].ToString()))
                                                        {//99.50
                                                            PdfTextArea pTxtArea = new PdfTextArea(ColumnsFont,
                                                                                Color.Black, new PdfArea(myPdfDocument, 63, posY + 105, 230, 50),
                                                                                ContentAlignment.MiddleLeft, dt.Rows[0][dcs.ColumnName].ToString());
                                                            newPdfPage.Add(pTxtArea);
                                                        }
                                                        break;
                                                    }
                                                case "Address2":
                                                    {
                                                        if (!string.IsNullOrEmpty(dt.Rows[0][dcs.ColumnName].ToString()))
                                                        {//109.50
                                                            PdfTextArea pTxtArea = new PdfTextArea(ColumnsFont,
                                                                                Color.Black, new PdfArea(myPdfDocument, 63, posY + 115, 230, 50),
                                                                                ContentAlignment.MiddleLeft, dt.Rows[0][dcs.ColumnName].ToString());
                                                            newPdfPage.Add(pTxtArea);
                                                        }
                                                        break;
                                                    }
                                                case "Address3":
                                                    {
                                                        if (!string.IsNullOrEmpty(dt.Rows[0][dcs.ColumnName].ToString()))
                                                        {//119.50
                                                            PdfTextArea pTxtArea = new PdfTextArea(ColumnsFont,
                                                                                Color.Black, new PdfArea(myPdfDocument, 63, posY + 125, 230, 50),
                                                                                ContentAlignment.MiddleLeft, dt.Rows[0][dcs.ColumnName].ToString());
                                                            newPdfPage.Add(pTxtArea);
                                                        }
                                                        break;
                                                    }
                                                case "Address4":
                                                    {
                                                        if (!string.IsNullOrEmpty(dt.Rows[0][dcs.ColumnName].ToString()))
                                                        {//129.50
                                                            PdfTextArea pTxtArea = new PdfTextArea(ColumnsFont,
                                                                                Color.Black, new PdfArea(myPdfDocument, 63, posY + 135, 230, 50),
                                                                                ContentAlignment.MiddleLeft, dt.Rows[0][dcs.ColumnName].ToString());
                                                            newPdfPage.Add(pTxtArea);
                                                        }
                                                        break;
                                                    }
                                                case "City":
                                                    {
                                                        string totString = string.Empty;
                                                        bool chkCity = false;
                                                        bool chkRegion = false;
                                                        bool chkPostal = false;
                                                        //
                                                        if (dt.Rows[filRws][dcs.ColumnName] != null && dt.Rows[0][dcs.ColumnName].ToString() != "")
                                                        {
                                                            chkCity = true;
                                                        }
                                                        if (dt.Rows[0]["Region"] != null && dt.Rows[0]["Region"].ToString() != "")
                                                        {
                                                            chkRegion = true;
                                                        }
                                                        if (dt.Rows[0]["PostalCode"] != null && dt.Rows[0]["PostalCode"].ToString() != "")
                                                        {
                                                            chkPostal = true;
                                                        }
                                                        if ((bool)chkCity)
                                                        {
                                                            totString = totString + dt.Rows[0][dcs.ColumnName].ToString() + ", ";
                                                        }
                                                        if ((bool)chkRegion)
                                                        {
                                                            totString = totString + dt.Rows[0]["Region"].ToString() + ", ";
                                                        }
                                                        if ((bool)chkPostal)
                                                        {
                                                            totString = totString + dt.Rows[0]["PostalCode"].ToString() + " ";
                                                        }
                                                        if (totString.Contains(","))
                                                        {
                                                            totString = totString.Remove(totString.Length - 1);
                                                        }
                                                        PdfTextArea pTxtArea = new PdfTextArea(HeaderFont,
                                                                                Color.Black, new PdfArea(myPdfDocument, 63, posY + 140, 230, 50),
                                                                                ContentAlignment.MiddleLeft, totString);
                                                        newPdfPage.Add(pTxtArea);
                                                        break;
                                                    }
                                            }
                                        }
                                        #endregion
                                        #region Child
                                        foreach (DataColumn fildcs in dtChild.Columns)
                                        {
                                            switch (fildcs.ColumnName.ToString())
                                            {
                                                case "Total":
                                                    {
                                                        if (dtChild.Rows[filRws][fildcs.ColumnName] != null && dtChild.Rows[filRws][fildcs.ColumnName].ToString() != "")
                                                        {//207.50
                                                            PdfTextArea pTxtArea = new PdfTextArea(HeaderFont,
                                                                                Color.Black, new PdfArea(myPdfDocument, 440, posY + 214, 230, 50),
                                                                                ContentAlignment.MiddleLeft, dtChild.Rows[filRws][fildcs.ColumnName].ToString());
                                                            newPdfPage.Add(pTxtArea);
                                                        }
                                                        break;
                                                    }

                                                case "RecordCount":
                                                    {
                                                        if (dtChild.Rows[filRws][fildcs.ColumnName] != null && dtChild.Rows[filRws][fildcs.ColumnName].ToString() != "")
                                                        {//207.50
                                                            PdfTextArea pTxtArea = new PdfTextArea(HeaderFont,
                                                                                Color.Black, new PdfArea(myPdfDocument, 280, posY + 214, 230, 50),
                                                                                ContentAlignment.MiddleLeft, dtChild.Rows[filRws][fildcs.ColumnName].ToString());
                                                            newPdfPage.Add(pTxtArea);
                                                        }
                                                        break;
                                                    }

                                                case "Box1099MISC":
                                                    {
                                                        if (dtChild.Rows[filRws][fildcs.ColumnName] != null && dtChild.Rows[filRws][fildcs.ColumnName].ToString() != "")
                                                        {
                                                            // 306.50 /x54
                                                            PdfTextArea pTxtArea = new PdfTextArea(ColumnsFont,
                                                                                Color.Black, new PdfArea(myPdfDocument, 43, posY + 318, 50, 50),
                                                                                ContentAlignment.MiddleLeft, dtChild.Rows[filRws][fildcs.ColumnName].ToString());
                                                            newPdfPage.Add(pTxtArea);
                                                        }
                                                        break;
                                                    }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }

                        }
                    }
                    newPdfPage.SaveToDocument();
                    CreatePDFDocument(fileName);
                }
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion
            }
        }
        #endregion



        #region Report Style 642
        public void ReportStyle642(DataTable[] dt, DataTable dtHeader, Hashtable[] arrhtFormatModes, bool PLayout, int[][] arrallcolWidths, string fileName)
        {
            try
            {
                //myPdfDocument = new PdfDocument(PdfDocumentFormat.A4);
                if ((bool)PLayout)
                {
                    myPdfDocument = new PdfDocument(PdfDocumentFormat.A4_Horizontal);
                }
                else
                {
                    myPdfDocument = new PdfDocument(PdfDocumentFormat.A4);
                    pgHeight = 50;
                    pgWidth = 200;
                }
                string imgpath = PDFImagePath();
                PdfImage LogoImage = null;
                PdfTablePage myPdfHeaderPage = null;
                PdfLine pdfLineBrk = null;
                if (!string.IsNullOrEmpty(imgpath))
                {
                    LogoImage = myPdfDocument.NewImage(imgpath);
                }
                newPdfPage = GetHeaderPDFTableStyle1(dtHeader, out myHeaderPdfTablePage);
                ArrayList alRemovedColumns = null;
                //int t = 0;
                int dt0Rows = dt[0].Rows.Count;

                string[] strSumColumns ={ "Ending Balance", "Ending", "Variance" };



                //PARENT TABLE : Report Detail
                //DataTable dt[0] = dt[0];
                if (dt[0].Rows.Count > 0)
                {
                    string Child2Link3 = string.Empty;

                    // int colstart = 0;
                    //int[] colParentWidths = new int[(arrallcolWidths[0].Length + strSumColumns.Length) - 2];
                    //for (int ctr = 0; ctr < arrallcolWidths[0].Length; ctr++)
                    //{
                    //    if (!alRemovedColumns.Contains(ctr))
                    //    {
                    //        colParentWidths[colstart] = arrallcolWidths[0][ctr] - 5;
                    //        colstart++;
                    //    }
                    //}

                    //Remove columns TrxID and Link1 from table add width 10
                    int colstart = 0;
                    int[] colParentWidths = new int[(arrallcolWidths[0].Length + strSumColumns.Length) - 2];
                    colParentWidths[0] = arrallcolWidths[0][1] - 5;
                    colstart++;
                    //Add column widths for Division1,2,3
                    for (int i = 0; i < strSumColumns.Length; i++)
                    {
                        colParentWidths[colstart] = 19;
                        colstart++;
                    }
                    //


                    #region PDF Header
                    DataTable dtParentHeader = new DataTable();
                    dtParentHeader.Columns.Add("Caption");
                    dtParentHeader.Columns.Add("Actuals");
                    dtParentHeader.Columns.Add("Budget");
                    dtParentHeader.Columns.Add("Variance");

                    dtParentHeader.Rows.Add(dtParentHeader.NewRow());
                    for (int col = 0; col < dtParentHeader.Columns.Count; col++)
                    {
                        dtParentHeader.Rows[dtParentHeader.Rows.Count - 1][dtParentHeader.Columns[col].ToString()] = "SKIP";
                    }
                    PdfTable myPdfHeader = myPdfDocument.NewTable(RowFontBold, dtParentHeader.Rows.Count, dtParentHeader.Columns.Count, 1);
                    myPdfHeader.ImportDataTable(dtParentHeader);
                    myPdfHeader.SetBorders(Color.Black, 1, BorderType.None);
                    myPdfHeader.SetContentAlignment(ContentAlignment.MiddleLeft);
                    myPdfHeader.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                    myPdfHeader.SetColumnsWidth(colParentWidths);
                    foreach (PdfCell pHeaderCll in myPdfHeader.HeadersRow.Cells)
                    {
                        string labelName = pHeaderCll.Content.ToString();
                        switch (labelName)
                        {
                            case "Caption":
                                {
                                    pHeaderCll.SetBackgroundColor(Color.White);
                                    pHeaderCll.SetForegroundColor(Color.White);
                                    break;
                                }
                            case "Actuals":
                            case "Budget":
                            case "Variance":
                                {
                                    pHeaderCll.SetContentAlignment(ContentAlignment.MiddleCenter);
                                    break;
                                }
                        }
                    }
                    foreach (PdfCell pskipCll in myPdfHeader.Cells)
                    {
                        string labelValue = pskipCll.Content.ToString();
                        switch (labelValue)
                        {
                            case "SKIP":
                                {
                                    pskipCll.SetBackgroundColor(Color.White);
                                    pskipCll.SetForegroundColor(Color.White);
                                    break;
                                }
                        }
                    }
                    posY = currentYPos + 25;
                    currentYPos = posY;
                    //
                    myPdfHeaderPage = myPdfHeader.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, (height - (posY))));
                    newPdfPage.Add(myPdfHeaderPage);
                    //
                    double xPos = myPdfHeaderPage.CellArea(myPdfHeader.HeadersRow.Index, 0).TopLeftVertex.X;
                    double yPos = myPdfHeaderPage.CellArea(myPdfHeader.HeadersRow.Index, 0).TopLeftVertex.Y;
                    //
                    Point pStart = new Point(Convert.ToInt32(xPos), Convert.ToInt32(yPos));
                    Point pEnd = new Point(Convert.ToInt32(xPos + (myPdfDocument.PageWidth - 50)), Convert.ToInt32(yPos));
                    pdfLineBrk = new PdfLine(myPdfDocument, pStart, pEnd, Color.Black, 1);
                    newPdfPage.Add(pdfLineBrk);
                    //
                    #endregion
                    #region Table0
                    for (int dtCnt = 0; dtCnt < dt0Rows; dtCnt++)
                    {
                        DataTable pDT = new DataTable();
                        DataTable dtNew = new DataTable();
                        DataRow[] foundRows = null;
                        string parentTrxID = string.Empty;

                        foundRows = dt[0].Select("Link1 ='" + dt[0].Rows[0]["Link1"].ToString() + "'");
                        if (foundRows.Length > 0)
                        {
                            int rowIndex = dt[0].Rows.IndexOf(foundRows[0]);
                            DataRow dt2Row = dtNew.NewRow();
                            if (dtNew.Columns.Count == 0)
                            {
                                for (int x = 0; x < dt[0].Columns.Count; x++)
                                {
                                    dtNew.Columns.Add(dt[0].Columns[x].ColumnName);
                                }
                                //ADD Columns Divisions Ending Balance, Ending, Variance
                                for (int i = 0; i < strSumColumns.Length; i++)
                                {
                                    dtNew.Columns.Add(strSumColumns[i].ToString());
                                }
                            }
                            for (int i = 0; i < dt[0].Columns.Count; i++)
                            {
                                if (dtNew.Columns.Contains(dt[0].Columns[i].ColumnName))
                                {
                                    dt2Row[i] = foundRows[0].ItemArray[i].ToString();
                                }
                            }

                            //for (int i = 0; i < dt[0].Columns.Count; i++)
                            //{
                            //    if (dtNew.Columns.Contains(dt[0].Columns[i].ColumnName))
                            //    {
                            //        switch (dt[0].Columns[i].ColumnName)
                            //        {
                            //            case "Caption":
                            //                {
                            //                    dt2Row[i] = foundRows[0].ItemArray[i].ToString();
                            //                    break;
                            //                }
                            //        }
                            //    }
                            //}

                            dtNew.Rows.Add(dt2Row);
                            parentTrxID = dt[0].Rows[0]["Link1"].ToString();
                            if (dt[0].Rows.Count > 0)
                            {
                                dt[0].Rows[0].Delete();
                            }
                            dtNew.AcceptChanges();
                            dtNew.TableName = dt[0].TableName;

                            //First Row data add Ending Balance, Ending, Variance
                            //if (dtCnt == 0)
                            //{
                            //    for (int i = 0; i < strSumColumns.Length; i++)
                            //    {
                            //        dtNew.Rows[dtNew.Rows.Count - 1][strSumColumns[i].ToString()] = strSumColumns[i].ToString();
                            //    }
                            //}

                        }
                        alRemovedColumns = new ArrayList();
                        //To Get ordinals
                        if (dtNew.Columns.Contains("TrxID"))
                        {
                            alRemovedColumns.Add(dtNew.Columns["TrxID"].Ordinal);
                        }
                        if (dtNew.Columns.Contains("Link1"))
                        {
                            alRemovedColumns.Add(dtNew.Columns["Link1"].Ordinal);
                        }

                        //To Delete
                        if (dtNew.Columns.Contains("TrxID"))
                        {
                            dtNew.Columns.Remove("TrxID");
                        }
                        if (dtNew.Columns.Contains("Link1"))
                        {
                            dtNew.Columns.Remove("Link1");
                        }


                        //Remove columns TrxID and Link1 from table
                        //int colstart = 0;
                        //int[] colParentWidths = new int[arrallcolWidths[0].Length - 2];
                        //for (int ctr = 0; ctr < arrallcolWidths[0].Length; ctr++)
                        //{
                        //    if (!alRemovedColumns.Contains(ctr))
                        //    {
                        //        colParentWidths[colstart] = arrallcolWidths[0][ctr];
                        //        colstart++;
                        //    }
                        //}


                        //Add column widths for Division1,2,3
                        //for (int i = 0; i < strSumColumns.Length; i++)
                        //{
                        //    colParentWidths[colstart] = 19;
                        //    colstart++;
                        //}


                        //ADD PARENT TABLE ROW
                        if (dtNew.Rows.Count > 0)
                        {
                            PdfTable myPdfParent = myPdfDocument.NewTable(RowBoxFontBold, dtNew.Rows.Count, dtNew.Columns.Count, 1);
                            myPdfParent.ImportDataTable(dtNew);
                            myPdfParent.SetBorders(Color.Black, 1, BorderType.None);
                            myPdfParent.SetContentAlignment(ContentAlignment.MiddleLeft);
                            myPdfParent.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                            myPdfParent.SetColumnsWidth(colParentWidths);
                            myPdfParent.HeadersRow.SetFont(RowBoxFontBold);
                            myPdfParent.SetFont(HeaderPageTitleFont);
                            myPdfParent.HeadersRow.SetBackgroundColor(Color.White);
                            myPdfParent.HeadersRow.SetForegroundColor(Color.White);


                            while (!myPdfParent.AllTablePagesCreated)
                            {
                                //Setting the Y position and if required creating new page
                                if (currentYPos > myPdfDocument.PageHeight - 50)
                                {
                                    posY = 70;
                                    currentYPos = 70;
                                    newPdfPage.SaveToDocument();
                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                    newPdfPage = myPdfDocument.NewPage();
                                    newPdfPage.Add(myHeaderPdfTablePage);
                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                    posY = currentYPos + 25;
                                    if (myPdfHeaderPage != null)
                                    {
                                        currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                        posY = currentYPos;
                                    }
                                    newPdfPage.Add(myPdfHeaderPage);
                                    newPdfPage.Add(pdfLineBrk);
                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));

                                }
                                else
                                {
                                    if (posY >= currentYPos)
                                    {
                                        if (myPdfHeaderPage != null)
                                        {
                                            currentYPos = posY + 9 + (myPdfHeaderPage.Area.PosY - posY) / 2;
                                        }
                                        posY = currentYPos;
                                    }
                                    else
                                    {
                                        posY = currentYPos;
                                    }
                                }
                                if (myPdfDocument.PageHeight - 50 - posY < 50)
                                {
                                    posY = 70;
                                    currentYPos = 70;
                                    newPdfPage.SaveToDocument();
                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                    newPdfPage = myPdfDocument.NewPage();
                                    newPdfPage.Add(myHeaderPdfTablePage);
                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }

                                    posY = currentYPos + 25;
                                    if (myPdfHeaderPage != null)
                                    {
                                        currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                        posY = currentYPos;
                                    }
                                    newPdfPage.Add(myPdfHeaderPage);
                                    newPdfPage.Add(pdfLineBrk);
                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));

                                }
                                PdfTablePage newPdfTablePage2 = myPdfParent.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                newPdfPage.Add(newPdfTablePage2);
                                currentYPos = newPdfTablePage2.Area.BottomLeftVertex.Y;
                            }
                    #endregion
                            //Child DataTable
                            //CHILD TABLE : Report Category
                            string Child1Link2 = string.Empty;
                            Hashtable htGrandSumTable1 = null;
                            int[] colChild1Widths = null;
                            #region Table1
                            if (dt[1] != null)
                            {
                                //string childRowIndex1 = string.Empty;
                                //int childTrxID1 = 0;
                                int childRowIndex1 = 0;
                                //int dt1Rows = dt[1].Rows.Count;
                                DataRow[] drChildFound = dt[1].Select("Link1='" + parentTrxID + "'");
                                int dt1Rows = drChildFound.Length;
                                //int dt1RowsFound = 0;
                                htGrandSumTable1 = new Hashtable();
                                for (int dtch = 0; dtch < dt1Rows; dtch++)
                                {
                                    DataRow[] drChildRow1 = dt[1].Select("Link1='" + parentTrxID + "' AND Link2=" + dt[1].Rows[0]["Link2"].ToString());
                                    DataTable dtFullChild1 = new DataTable();
                                    int[] childIndexes1 = new int[drChildRow1.Length];
                                    Hashtable htGrandTotals = new Hashtable();
                                    if (drChildRow1.Length > 0)
                                    {
                                        for (int drFoundRws = 0; drFoundRws < drChildRow1.Length; drFoundRws++)
                                        {
                                            childRowIndex1 = dt[1].Rows.IndexOf(drChildRow1[drFoundRws]);
                                            Child1Link2 = dt[1].Rows[drFoundRws]["Link2"].ToString();
                                            //dt1RowsFound = drChildRow1.Length;
                                            DataRow dtChRow = dtFullChild1.NewRow();
                                            if (dtFullChild1.Columns.Count == 0)
                                            {
                                                for (int x = 0; x < dt[1].Columns.Count; x++)
                                                {
                                                    dtFullChild1.Columns.Add(dt[1].Columns[x].ColumnName);
                                                }
                                            }
                                            for (int i = 0; i < dtFullChild1.Columns.Count; i++)
                                            {
                                                dtChRow[i] = drChildRow1[drFoundRws].ItemArray[i].ToString();
                                            }
                                            dtFullChild1.Rows.Add(dtChRow);
                                            if (dt[1].Rows[0]["Link2"].ToString() != null)
                                            {
                                                Child1Link2 = dt[1].Rows[0]["Link2"].ToString();
                                            }
                                            if (dt[1].Rows.Count > 0)
                                            {
                                                dt[1].Rows[0].Delete();
                                            }
                                            if (childIndexes1[0] != 0)
                                            {
                                                childIndexes1[drFoundRws] = childRowIndex1 + drFoundRws;
                                            }
                                            else
                                            {
                                                childIndexes1[drFoundRws] = childRowIndex1;
                                            }
                                            dtFullChild1.AcceptChanges();
                                            dtFullChild1.TableName = dt[1].TableName;
                                            if (dtFullChild1.Rows.Count > 0)
                                            {
                                                alRemovedColumns = new ArrayList();
                                                //Get ordinals to add arraylist
                                                if (dtFullChild1.Columns.Contains("TrxID"))
                                                {
                                                    alRemovedColumns.Add(dtFullChild1.Columns["TrxID"].Ordinal);
                                                }
                                                if (dtFullChild1.Columns.Contains("Link1"))
                                                {
                                                    alRemovedColumns.Add(dtFullChild1.Columns["Link1"].Ordinal);
                                                }
                                                if (dtFullChild1.Columns.Contains("Link2"))
                                                {
                                                    alRemovedColumns.Add(dtFullChild1.Columns["Link2"].Ordinal);
                                                }
                                                //To Delete
                                                if (dtFullChild1.Columns.Contains("TrxID"))
                                                {
                                                    dtFullChild1.Columns.Remove("TrxID");
                                                }
                                                if (dtFullChild1.Columns.Contains("Link1"))
                                                {
                                                    dtFullChild1.Columns.Remove("Link1");
                                                }
                                                if (dtFullChild1.Columns.Contains("Link2"))
                                                {
                                                    dtFullChild1.Columns.Remove("Link2");
                                                }
                                            }
                                            //Remove columns TrxID and Link1 from colwidths
                                            colstart = 1;
                                            colChild1Widths = new int[(arrallcolWidths[1].Length + 1) - 3];
                                            colChild1Widths[0] = 1;
                                            for (int ctr = 0; ctr < arrallcolWidths[1].Length; ctr++)
                                            {
                                                if (!alRemovedColumns.Contains(ctr))
                                                {
                                                    colChild1Widths[colstart] = arrallcolWidths[1][ctr];
                                                    colstart++;
                                                }
                                            }
                                            if (dtFullChild1.Rows.Count > 0)
                                            {
                                                //Add Empty column for space
                                                dtFullChild1.Columns.Add("Col");
                                                dtFullChild1.Columns["Col"].SetOrdinal(0);
                                                //
                                                PdfTable myPdfChild = myPdfDocument.NewTable(HeaderPageTitleFont2, dtFullChild1.Rows.Count, dtFullChild1.Columns.Count, 1);
                                                myPdfChild.ImportDataTable(dtFullChild1);
                                                myPdfChild.SetBorders(Color.Black, 1, BorderType.None);
                                                myPdfChild.SetContentAlignment(ContentAlignment.MiddleLeft);
                                                myPdfChild.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                                myPdfChild.SetColumnsWidth(colChild1Widths);
                                                //myPdfChild.SetColumnsWidth(arrallcolWidths[1]);
                                                myPdfChild.HeadersRow.SetFont(HeaderPageTitleFont2);
                                                myPdfChild.HeadersRow.SetBackgroundColor(Color.White);
                                                myPdfChild.HeadersRow.SetForegroundColor(Color.White);
                                                //Setting Column Width for emtpy column To Show Like a Zig zag fashion
                                                //myPdfChild.Columns[0].SetWidth(1);

                                                //posX = posX + 5;
                                                while (!myPdfChild.AllTablePagesCreated)
                                                {
                                                    //Setting the Y position and if required creating new page
                                                    if (currentYPos > myPdfDocument.PageHeight - 50)
                                                    {
                                                        posY = 70;
                                                        currentYPos = 70;
                                                        newPdfPage.SaveToDocument();
                                                        //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                                        newPdfPage = myPdfDocument.NewPage();
                                                        newPdfPage.Add(myHeaderPdfTablePage);
                                                        if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                                        posY = currentYPos + 25;
                                                        if (myPdfHeaderPage != null)
                                                        {
                                                            currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                                            posY = currentYPos;
                                                        }
                                                        newPdfPage.Add(myPdfHeaderPage);
                                                        newPdfPage.Add(pdfLineBrk);
                                                        newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));
                                                    }
                                                    else
                                                    {
                                                        if (posY >= currentYPos)
                                                        {
                                                            if (myPdfHeaderPage != null)
                                                            {
                                                                currentYPos = posY + 9 + (myPdfHeaderPage.Area.PosY - posY) / 2;
                                                            }
                                                            posY = currentYPos;
                                                        }
                                                        else
                                                        {
                                                            posY = currentYPos;
                                                        }
                                                    }
                                                    if (myPdfDocument.PageHeight - 50 - posY < 50)
                                                    {
                                                        posY = 70;
                                                        currentYPos = 70;
                                                        newPdfPage.SaveToDocument();
                                                        //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                                        newPdfPage = myPdfDocument.NewPage();
                                                        newPdfPage.Add(myHeaderPdfTablePage);
                                                        if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                                        posY = currentYPos + 25;
                                                        if (myPdfHeaderPage != null)
                                                        {
                                                            currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                                            posY = currentYPos;
                                                        }
                                                        newPdfPage.Add(myPdfHeaderPage);
                                                        newPdfPage.Add(pdfLineBrk);
                                                        newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));
                                                    }
                                                    PdfTablePage newPdfTablePage2 = myPdfChild.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                                    newPdfPage.Add(newPdfTablePage2);
                                                    currentYPos = newPdfTablePage2.Area.BottomLeftVertex.Y;
                                                }
                                                //posX = posX - 5;
                                            }
                                        }
                                    }
                            #endregion
                                    //CHILD TABLE : Account Details
                                    #region Table 2
                                    DataTable dtTotals = new DataTable();
                                    DataTable dtGrandTotals = new DataTable();
                                    if (dt[2] != null)
                                    {
                                        string childTrxID = string.Empty;
                                        int childRowIndex = 0;
                                        string strCustom = string.Empty;
                                        string strTotalAmount = string.Empty;
                                        double TotalAmount = 0;

                                        int colEndBalOrdinal = 0;
                                        double TotalEndingBalance = 0;
                                        double TotalEnding = 0;
                                        double TotalVariance = 0;
                                        string strTotalVariance = string.Empty;
                                        DataRow[] drChildRow = dt[2].Select("Link1='" + parentTrxID + "' AND Link2=" + Child1Link2);
                                        DataTable dtFullChild = new DataTable();
                                        int[] childIndexes = new int[drChildRow.Length];
                                        //To store distinct subtotalids
                                        int dt2ColumnsCount = 0;
                                        if (drChildRow.Length > 0)
                                        {
                                            for (int drFoundRws2 = 0; drFoundRws2 < drChildRow.Length; drFoundRws2++)
                                            {
                                                childRowIndex = dt[2].Rows.IndexOf(drChildRow[drFoundRws2]);
                                                DataRow dtChRow2 = dtFullChild.NewRow();

                                                if (dtFullChild.Columns.Count == 0)
                                                {
                                                    for (int x = 0; x < dt[2].Columns.Count; x++)
                                                    {
                                                        dtFullChild.Columns.Add(dt[2].Columns[x].ColumnName);
                                                    }
                                                    dt2ColumnsCount = dtFullChild.Columns.Count;
                                                }
                                                for (int i = 0; i < dtFullChild.Columns.Count; i++)
                                                {
                                                    if (dt[2].Columns.Contains(dtFullChild.Columns[i].ColumnName))
                                                    {
                                                        dtChRow2[i] = drChildRow[drFoundRws2].ItemArray[i].ToString();
                                                    }
                                                }
                                                Child2Link3 = dt[2].Rows[0]["Link3"].ToString();
                                                dtFullChild.Rows.Add(dtChRow2);
                                                //Delete Row
                                                if (dt[2].Rows.Count > 0)
                                                {
                                                    dt[2].Rows[0].Delete();
                                                }
                                                dtFullChild.AcceptChanges();
                                                dtFullChild.TableName = dt[2].TableName;
                                                //Add Ending Balance from TrxDetail1 Table
                                                #region Table3
                                                if (dt[3] != null)
                                                {
                                                    int RowIndexDetail1 = 0;
                                                    DataRow[] drChildRowDetail1 = dt[3].Select("Link1='" + parentTrxID + "' AND Link2='" + Child1Link2 + "' AND  Link3='" + Child2Link3 + "'");
                                                    if (drChildRowDetail1.Length > 0)
                                                    {
                                                        RowIndexDetail1 = dt[3].Rows.IndexOf(drChildRowDetail1[0]);
                                                        if (drChildRowDetail1 != null)
                                                        {
                                                            TotalEndingBalance = Convert.ToDouble(drChildRowDetail1[0]["Ending Balance"].ToString());
                                                            if (dtFullChild.Columns.Contains("Ending Balance"))
                                                            {
                                                                dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Ending Balance"] = drChildRowDetail1[0]["Ending Balance"].ToString();
                                                            }
                                                            else
                                                            {
                                                                dtFullChild.Columns.Add("Ending Balance");
                                                                dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Ending Balance"] = drChildRowDetail1[0]["Ending Balance"].ToString();
                                                            }
                                                            if (dt[3].Columns.Contains("Ending Balance"))
                                                            {
                                                                colEndBalOrdinal = dt[3].Columns["Ending Balance"].Ordinal;
                                                            }

                                                            if (dt[3].Rows.Count > 0)
                                                            {
                                                                dt[3].Rows[0].Delete();
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                                #region Table4
                                                //Add Ending Bal from TrxDetail2 Table
                                                if (dt[4] != null)
                                                {
                                                    int RowIndexDetail2 = 0;
                                                    //Child2Link3 = drChildRow[drFoundRws]["Link3"].ToString();

                                                    DataRow[] drChildRowDetail2 = dt[4].Select("Link1='" + parentTrxID + "' AND Link2='" + Child1Link2 + "' AND  Link3='" + Child2Link3 + "'");

                                                    if (drChildRowDetail2.Length > 0)
                                                    {

                                                        RowIndexDetail2 = dt[4].Rows.IndexOf(drChildRowDetail2[0]);

                                                        if (drChildRowDetail2 != null)
                                                        {
                                                            TotalEnding = Convert.ToDouble(drChildRowDetail2[0]["Ending"].ToString());
                                                            if (dtFullChild.Columns.Contains("Ending"))
                                                            {
                                                                dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Ending"] = drChildRowDetail2[0]["Ending"].ToString();
                                                            }
                                                            else
                                                            {
                                                                dtFullChild.Columns.Add("Ending");
                                                                dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Ending"] = drChildRowDetail2[0]["Ending"].ToString();
                                                            }
                                                            //Delete 
                                                            if (dt[4].Rows.Count > 0)
                                                            {
                                                                dt[4].Rows[0].Delete();
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                                #region Add Variance Column
                                                //"Ending Balance", "Ending"
                                                if (dtFullChild.Columns.Contains("Ending Balance") && dtFullChild.Columns.Contains("Ending"))
                                                {
                                                    if (dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Ending"].ToString() != string.Empty)
                                                    {
                                                        //Column3 = Column2 - Column1
                                                        TotalVariance = TotalEnding - TotalEndingBalance;
                                                        strTotalVariance = Convert.ToString(TotalVariance);
                                                        strTotalVariance = clsReportsUICore.ConvertToCurrencyFormat(strTotalVariance);
                                                        if (dtFullChild.Columns.Contains("Variance"))
                                                        {
                                                            dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Variance"] = strTotalVariance;
                                                        }
                                                        else
                                                        {
                                                            dtFullChild.Columns.Add("Variance");
                                                            dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Variance"] = strTotalVariance;
                                                        }
                                                    }
                                                }
                                                #endregion
                                                //Add Sum Columns End Balance
                                                for (int i = 0; i < strSumColumns.Length; i++)
                                                {
                                                    if (dtFullChild.Columns.Contains(strSumColumns[i]))
                                                    {
                                                        strCustom = string.Empty;
                                                        decimal amount;
                                                        TotalAmount = 0;
                                                        string amt = string.Empty;
                                                        string colName = strSumColumns[i];
                                                        if (!htGrandTotals.ContainsKey(colName))
                                                        {
                                                            amt = clsReportsUICore.ConvertToCurrencyFormat(dtFullChild.Rows[dtFullChild.Rows.Count - 1][colName].ToString());
                                                            htGrandTotals.Add(colName, amt);
                                                        }
                                                        else
                                                        {
                                                            TotalAmount = Convert.ToDouble(htGrandTotals[colName].ToString());
                                                            if (!string.IsNullOrEmpty(dtFullChild.Rows[dtFullChild.Rows.Count - 1][colName].ToString()))
                                                            {
                                                                TotalAmount = TotalAmount + Convert.ToDouble(dtFullChild.Rows[dtFullChild.Rows.Count - 1][colName].ToString());
                                                            }
                                                            strTotalAmount = Convert.ToString(TotalAmount);
                                                            amt = clsReportsUICore.ConvertToCurrencyFormat(strTotalAmount);
                                                            htGrandTotals[colName] = amt;
                                                        }
                                                    }
                                                }
                                                //
                                                if (childIndexes[0] != 0)
                                                {
                                                    childIndexes[drFoundRws2] = childRowIndex + drFoundRws2;
                                                }
                                                else
                                                {
                                                    childIndexes[drFoundRws2] = childRowIndex;
                                                }
                                            }
                                        }
                                        //To set font for sum rows
                                        int sumRowIndex = 0;

                                        //Add Columns to DtTotal
                                        if (dtTotals.Columns.Count == 0)
                                        {
                                            for (int x = 0; x < dtFullChild.Columns.Count; x++)
                                            {
                                                dtTotals.Columns.Add(dtFullChild.Columns[x].ColumnName);
                                            }
                                        }
                                        IDictionaryEnumerator enumTot = htGrandTotals.GetEnumerator();
                                        if (htGrandTotals.Keys.Count > 0)
                                        {
                                            dtTotals.Rows.Add(dtTotals.NewRow());
                                            //ADDING TOTAL VALUE
                                            dtTotals.Rows[dtTotals.Rows.Count - 1]["Account Name"] = "Total " + dtFullChild1.Rows[0]["Caption"].ToString();

                                            sumRowIndex = dtTotals.Rows.Count - 1;
                                            TotalAmount = 0;
                                            string amt = string.Empty;
                                            while (enumTot.MoveNext())
                                            {
                                                //dtFullChild.Rows[dtFullChild.Rows.Count - 1][enumTot.Key.ToString()] = enumTot.Value.ToString();
                                                dtTotals.Rows[dtTotals.Rows.Count - 1][enumTot.Key.ToString()] = enumTot.Value.ToString();
                                                //Add GrandSum to Tabel1
                                                if (!htGrandSumTable1.ContainsKey(enumTot.Key.ToString()))
                                                {
                                                    //ADD
                                                    htGrandSumTable1.Add(enumTot.Key.ToString(), enumTot.Value.ToString());
                                                }
                                                else
                                                {
                                                    //UPDATE
                                                    TotalAmount = Convert.ToDouble(htGrandSumTable1[enumTot.Key.ToString()].ToString());
                                                    TotalAmount = TotalAmount + double.Parse(enumTot.Value.ToString());
                                                    strTotalAmount = Convert.ToString(TotalAmount);
                                                    amt = clsReportsUICore.ConvertToCurrencyFormat(strTotalAmount);
                                                    htGrandSumTable1[enumTot.Key.ToString()] = amt;
                                                }
                                                //
                                            }

                                        }
                                        if (dtFullChild.Rows.Count > 0)
                                        {
                                            alRemovedColumns = new ArrayList();
                                            //Get ordinals to add arraylist
                                            if (dtFullChild.Columns.Contains("TrxID"))
                                            {
                                                alRemovedColumns.Add(dtFullChild.Columns["TrxID"].Ordinal);
                                            }
                                            if (dtFullChild.Columns.Contains("Link1"))
                                            {
                                                alRemovedColumns.Add(dtFullChild.Columns["Link1"].Ordinal);
                                            }
                                            if (dtFullChild.Columns.Contains("Link2"))
                                            {
                                                alRemovedColumns.Add(dtFullChild.Columns["Link2"].Ordinal);
                                            }
                                            if (dtFullChild.Columns.Contains("Link3"))
                                            {
                                                alRemovedColumns.Add(dtFullChild.Columns["Link3"].Ordinal);
                                            }
                                            //To Delete columns
                                            if (dtFullChild.Columns.Contains("TrxID"))
                                            {
                                                dtFullChild.Columns.Remove("TrxID");
                                            }
                                            if (dtFullChild.Columns.Contains("Link1"))
                                            {
                                                dtFullChild.Columns.Remove("Link1");
                                            }
                                            if (dtFullChild.Columns.Contains("Link2"))
                                            {
                                                dtFullChild.Columns.Remove("Link2");
                                            }
                                            if (dtFullChild.Columns.Contains("Link3"))
                                            {
                                                dtFullChild.Columns.Remove("Link3");
                                            }
                                            //Delete columns in dtTotals
                                            if (dtTotals.Columns.Contains("TrxID"))
                                            {
                                                dtTotals.Columns.Remove("TrxID");
                                            }
                                            if (dtTotals.Columns.Contains("Link1"))
                                            {
                                                dtTotals.Columns.Remove("Link1");
                                            }
                                            if (dtTotals.Columns.Contains("Link2"))
                                            {
                                                dtTotals.Columns.Remove("Link2");
                                            }
                                            if (dtTotals.Columns.Contains("Link3"))
                                            {
                                                dtTotals.Columns.Remove("Link3");
                                            }
                                        }
                                        //Check if the row not contains Ending columns
                                        dt2ColumnsCount = Convert.ToInt32(dt2ColumnsCount - 4) + 3;
                                        if (dtFullChild.Columns.Count < dt2ColumnsCount)
                                        {
                                            for (int i = 0; i < strSumColumns.Length; i++)
                                            {
                                                dtFullChild.Columns.Add(strSumColumns[i]);
                                                dtTotals.Columns.Add(strSumColumns[i]);
                                            }
                                        }
                                        //Remove columns TrxID and Link1 from colwidths
                                        colstart = 0;
                                        //3 columns are actual,budget,variance 
                                        int[] colChild2Widths = new int[(arrallcolWidths[2].Length + 3) - 4];
                                        //empty column width
                                        for (int ctr = 0; ctr < arrallcolWidths[2].Length; ctr++)
                                        {
                                            if (!alRemovedColumns.Contains(ctr))
                                            {
                                                colChild2Widths[colstart] = arrallcolWidths[2][ctr];
                                                colstart++;
                                            }
                                        }
                                        //Add Ending Columns Widths
                                        int EndbalWidth = 20;
                                        for (int i = 0; i < 3; i++)
                                        {
                                            if (colEndBalOrdinal == 0)
                                            {
                                                colChild2Widths[colstart] = EndbalWidth;
                                            }
                                            else
                                            {
                                                colChild2Widths[colstart] = arrallcolWidths[3][colEndBalOrdinal];
                                            }
                                            colstart++;
                                        }
                                        if (colEndBalOrdinal == 0)
                                        {
                                            if (dtFullChild.Columns.Contains("Ending Balance"))
                                            {
                                                //set ordeinal for empty columns
                                                colEndBalOrdinal = dtFullChild.Columns["Ending Balance"].Ordinal;
                                            }
                                        }
                                        else
                                        {
                                            if (dtFullChild.Columns.Contains("Ending Balance"))
                                            {
                                                //set ordeinal for empty columns
                                                colEndBalOrdinal = dtFullChild.Columns["Ending Balance"].Ordinal;
                                            }
                                        }
                                        //ADD GrandTotals
                                        int grandSumRowIndex = 0;
                                        if (dtGrandTotals.Columns.Count == 0)
                                        {
                                            for (int x = 0; x < dtFullChild.Columns.Count; x++)
                                            {
                                                dtGrandTotals.Columns.Add(dtFullChild.Columns[x].ColumnName);
                                            }
                                        }
                                        if (dtch == dt1Rows - 1)
                                        {
                                            if (dtFullChild.Rows.Count > 0)
                                            {
                                                //ADD SKIP
                                                dtGrandTotals.Rows.Add(dtGrandTotals.NewRow());
                                                for (int col = 0; col < dtGrandTotals.Columns.Count; col++)
                                                {
                                                    dtGrandTotals.Rows[dtGrandTotals.Rows.Count - 1][dtGrandTotals.Columns[col].ToString()] = "SKIP";
                                                }
                                                //ADD GrandTotal sums for Tabel1 
                                                IDictionaryEnumerator enumGTot = htGrandSumTable1.GetEnumerator();
                                                if (htGrandSumTable1.Keys.Count > 0)
                                                {
                                                    //ADD GrandTotals
                                                    dtGrandTotals.Rows.Add(dtGrandTotals.NewRow());
                                                    dtGrandTotals.Rows[dtGrandTotals.Rows.Count - 1]["Account Name"] = "TOTAL " + dtNew.Rows[0]["Caption"].ToString().ToUpper();

                                                    grandSumRowIndex = dtGrandTotals.Rows.Count - 1;
                                                    while (enumGTot.MoveNext())
                                                    {
                                                        if (dtGrandTotals.Columns.Contains(enumGTot.Key.ToString()))
                                                        {
                                                            dtGrandTotals.Rows[dtGrandTotals.Rows.Count - 1][enumGTot.Key.ToString()] = enumGTot.Value.ToString();
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        int[] colChild3Widths = new int[colChild2Widths.Length + 1];
                                        colChild3Widths[0] = 1;
                                        for (int i = 0; i < colChild2Widths.Length; i++)
                                        {
                                            colChild3Widths[i + 1] = colChild2Widths[i];
                                        }

                                        if (dtFullChild.Rows.Count > 0)
                                        {

                                            //Add Empty column for space
                                            //
                                            dtFullChild.Columns.Add("Col");
                                            dtFullChild.Columns["Col"].SetOrdinal(0);

                                            int[] colModWidths = new int[colParentWidths.Length + 1];
                                            for (int wdts = 0; wdts < colModWidths.Length; wdts++)
                                            {
                                                if (wdts == 0)
                                                {
                                                    colModWidths[wdts] = 5;
                                                }
                                                else
                                                {
                                                    if (wdts == 1)
                                                    {
                                                        colModWidths[wdts] = colParentWidths[0] - colModWidths[0];
                                                    }
                                                    else
                                                    {
                                                        colModWidths[wdts] = colParentWidths[wdts - 1];
                                                    }
                                                }
                                            }
                                            //


                                            PdfTable myPdfChild = myPdfDocument.NewTable(FontRegular, dtFullChild.Rows.Count, dtFullChild.Columns.Count, 1);
                                            myPdfChild.ImportDataTable(dtFullChild);
                                            myPdfChild.SetBorders(Color.Black, 1, BorderType.None);
                                            myPdfChild.SetContentAlignment(ContentAlignment.MiddleLeft);
                                            myPdfChild.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                            // myPdfChild.SetColumnsWidth(colChild3Widths);
                                            myPdfChild.SetColumnsWidth(colModWidths);
                                            myPdfChild.HeadersRow.SetFont(FontRegular);
                                            myPdfChild.HeadersRow.SetBackgroundColor(Color.White);
                                            myPdfChild.HeadersRow.SetForegroundColor(Color.White);
                                            //Setting Column Width To Show Like a Zig zag fashion
                                            //myPdfChild.Columns[0].SetWidth(1);
                                            //
                                            for (int i = 0; i < dtFullChild.Rows.Count; i++)
                                            {
                                                //SET RIGHT ALIGNMENT
                                                myPdfChild.Rows[i][colEndBalOrdinal + 1].SetContentAlignment(ContentAlignment.MiddleRight);
                                                myPdfChild.Rows[i][colEndBalOrdinal + 2].SetContentAlignment(ContentAlignment.MiddleRight);
                                                myPdfChild.Rows[i][colEndBalOrdinal + 3].SetContentAlignment(ContentAlignment.MiddleRight);

                                                foreach (PdfCell pcll in myPdfChild.Rows[i].Cells)
                                                {
                                                    if (pcll.Content.ToString() == "SKIP")
                                                    {
                                                        pcll.SetBackgroundColor(Color.White);
                                                        pcll.SetForegroundColor(Color.White);
                                                    }
                                                }
                                                //TrxID WHITE
                                                //myPdfChild.Rows[i][0].SetForegroundColor(Color.White);
                                            }
                                            //posX = posX + 10;
                                            while (!myPdfChild.AllTablePagesCreated)
                                            {
                                                //Setting the Y position and if required creating new page
                                                if (currentYPos > myPdfDocument.PageHeight - 50)
                                                {
                                                    posY = 70;
                                                    currentYPos = 70;
                                                    newPdfPage.SaveToDocument();
                                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                                    newPdfPage = myPdfDocument.NewPage();
                                                    newPdfPage.Add(myHeaderPdfTablePage);
                                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                                    posY = currentYPos + 25;
                                                    if (myPdfHeaderPage != null)
                                                    {
                                                        currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                                        posY = currentYPos;
                                                    }
                                                    newPdfPage.Add(myPdfHeaderPage);
                                                    newPdfPage.Add(pdfLineBrk);
                                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));
                                                }
                                                else
                                                {
                                                    if (posY >= currentYPos)
                                                    {
                                                        if (myPdfHeaderPage != null)
                                                        {
                                                            currentYPos = posY + 9 + (myPdfHeaderPage.Area.PosY - posY) / 2;
                                                        }
                                                        posY = currentYPos;
                                                    }
                                                    else
                                                    {
                                                        posY = currentYPos;
                                                    }
                                                }
                                                if (myPdfDocument.PageHeight - 50 - posY < 50)
                                                {
                                                    posY = 70;
                                                    currentYPos = 70;
                                                    newPdfPage.SaveToDocument();
                                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                                    newPdfPage = myPdfDocument.NewPage();
                                                    newPdfPage.Add(myHeaderPdfTablePage);
                                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                                    posY = currentYPos + 25;
                                                    if (myPdfHeaderPage != null)
                                                    {
                                                        currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                                        posY = currentYPos;
                                                    }
                                                    newPdfPage.Add(myPdfHeaderPage);
                                                    newPdfPage.Add(pdfLineBrk);
                                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));
                                                }
                                                PdfTablePage newPdfTablePage2 = myPdfChild.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                                newPdfPage.Add(newPdfTablePage2);
                                                currentYPos = newPdfTablePage2.Area.BottomLeftVertex.Y;
                                            }
                                            //posX = posX - 10;
                                        }
                                        #region Table Totals

                                        if (dtTotals.Rows.Count > 0)
                                        {
                                            PdfTable myPdfChildTotals = myPdfDocument.NewTable(FontRegular, dtTotals.Rows.Count, dtTotals.Columns.Count, 1);
                                            myPdfChildTotals.ImportDataTable(dtTotals);
                                            myPdfChildTotals.SetBorders(Color.Black, 1, BorderType.None);
                                            myPdfChildTotals.SetContentAlignment(ContentAlignment.MiddleLeft);
                                            myPdfChildTotals.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                            // myPdfChildTotals.SetColumnsWidth(colChild2Widths);
                                            myPdfChildTotals.SetColumnsWidth(colParentWidths);
                                            myPdfChildTotals.HeadersRow.SetFont(FontRegular);
                                            myPdfChildTotals.HeadersRow.SetBackgroundColor(Color.White);
                                            myPdfChildTotals.HeadersRow.SetForegroundColor(Color.White);
                                            for (int i = 0; i < dtTotals.Rows.Count; i++)
                                            {
                                                //SET RIGHT ALIGNMENT
                                                myPdfChildTotals.Rows[i][colEndBalOrdinal].SetContentAlignment(ContentAlignment.MiddleRight);
                                                myPdfChildTotals.Rows[i][colEndBalOrdinal + 1].SetContentAlignment(ContentAlignment.MiddleRight);
                                                myPdfChildTotals.Rows[i][colEndBalOrdinal + 2].SetContentAlignment(ContentAlignment.MiddleRight);

                                                foreach (PdfCell pcll in myPdfChildTotals.Rows[i].Cells)
                                                {
                                                    //if (sumRowIndex != 0)
                                                    //{
                                                    if (i == sumRowIndex)
                                                    {
                                                        //SET SUM ROW FONT
                                                        pcll.SetFont(HeaderPageTitleFont2);
                                                    }
                                                    //}
                                                    //if (grandSumRowIndex != 0)
                                                    //{
                                                    if (i == grandSumRowIndex)
                                                    {
                                                        //Set GrandSum row font Tabel 1
                                                        pcll.SetFont(SumRowFont);
                                                    }
                                                    //}
                                                    if (pcll.Content.ToString() == "SKIP")
                                                    {
                                                        pcll.SetBackgroundColor(Color.White);
                                                        pcll.SetForegroundColor(Color.White);
                                                    }
                                                }
                                                //TrxID WHITE
                                                //myPdfChildTotals.Rows[i][0].SetForegroundColor(Color.White);
                                            }
                                            //posX = posX + 5;
                                            while (!myPdfChildTotals.AllTablePagesCreated)
                                            {
                                                //Setting the Y position and if required creating new page
                                                if (currentYPos > myPdfDocument.PageHeight - 50)
                                                {
                                                    posY = 70;
                                                    currentYPos = 70;
                                                    newPdfPage.SaveToDocument();
                                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                                    newPdfPage = myPdfDocument.NewPage();
                                                    newPdfPage.Add(myHeaderPdfTablePage);
                                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                                    posY = currentYPos + 25;
                                                    if (myPdfHeaderPage != null)
                                                    {
                                                        currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                                        posY = currentYPos;
                                                    }
                                                    newPdfPage.Add(myPdfHeaderPage);
                                                    newPdfPage.Add(pdfLineBrk);
                                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));

                                                }
                                                else
                                                {
                                                    if (posY >= currentYPos)
                                                    {
                                                        if (myPdfHeaderPage != null)
                                                        {
                                                            currentYPos = posY + 9 + (myPdfHeaderPage.Area.PosY - posY) / 2;
                                                        }
                                                        posY = currentYPos;
                                                    }
                                                    else
                                                    {
                                                        posY = currentYPos;
                                                    }
                                                }
                                                if (myPdfDocument.PageHeight - 50 - posY < 50)
                                                {
                                                    posY = 70;
                                                    currentYPos = 70;
                                                    newPdfPage.SaveToDocument();
                                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                                    newPdfPage = myPdfDocument.NewPage();
                                                    newPdfPage.Add(myHeaderPdfTablePage);
                                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                                    posY = currentYPos + 25;
                                                    if (myPdfHeaderPage != null)
                                                    {
                                                        currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                                        posY = currentYPos;
                                                    }
                                                    newPdfPage.Add(myPdfHeaderPage);
                                                    newPdfPage.Add(pdfLineBrk);
                                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));
                                                }

                                                PdfTablePage newPdfTablePage2 = myPdfChildTotals.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                                //PBOX 2 Rows and 2 Columns
                                                /*for (int i = 0; i < strSumColumns.Length; i++)
                                                {
                                                    int rowbox = 0;
                                                    if (i == 0)
                                                    {
                                                        rowbox = sumRowIndex;
                                                    }
                                                    else
                                                    {
                                                        rowbox = grandSumRowIndex;
                                                    }
                                                   // int cellbox = Convert.ToInt32(dtFullChild.Columns[strSumColumns[i]].Ordinal);

                                                    PdfRectangle pr = newPdfTablePage2.CellArea(rowbox, colEndBalOrdinal).ToRectangle(Color.Black, 0.5, Color.White);
                                                    pr.StrokeWidth = 1;
                                                    newPdfPage.Add(pr);

                                                    PdfRectangle pr1 = newPdfTablePage2.CellArea(rowbox, colEndBalOrdinal + 1).ToRectangle(Color.Black, 0.5, Color.White);
                                                    pr1.StrokeWidth = 1;
                                                    newPdfPage.Add(pr1);

                                                }*/
                                                //PBOX
                                                newPdfPage.Add(newPdfTablePage2);
                                                currentYPos = newPdfTablePage2.Area.BottomLeftVertex.Y;

                                            }
                                            //posX = posX - 5;
                                        }


                                        #endregion

                                        #region GrandTotals
                                        if (dtGrandTotals.Rows.Count > 0)
                                        {
                                            PdfTable myPdfGrandTotals = myPdfDocument.NewTable(FontRegular, dtGrandTotals.Rows.Count, dtGrandTotals.Columns.Count, 1);
                                            myPdfGrandTotals.ImportDataTable(dtGrandTotals);
                                            myPdfGrandTotals.SetBorders(Color.Black, 1, BorderType.None);
                                            myPdfGrandTotals.SetContentAlignment(ContentAlignment.MiddleLeft);
                                            myPdfGrandTotals.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                            //myPdfGrandTotals.SetColumnsWidth(colChild2Widths);
                                            myPdfGrandTotals.SetColumnsWidth(colParentWidths);
                                            myPdfGrandTotals.HeadersRow.SetFont(FontRegular);
                                            myPdfGrandTotals.HeadersRow.SetBackgroundColor(Color.White);
                                            myPdfGrandTotals.HeadersRow.SetForegroundColor(Color.White);
                                            for (int i = 0; i < dtGrandTotals.Rows.Count; i++)
                                            {
                                                //SET RIGHT ALIGNMENT
                                                for (int j = 0; j < strSumColumns.Length; j++)
                                                {
                                                    myPdfGrandTotals.Rows[i][colEndBalOrdinal + j].SetContentAlignment(ContentAlignment.MiddleRight);
                                                }
                                                foreach (PdfCell pcll in myPdfGrandTotals.Rows[i].Cells)
                                                {
                                                    //if (sumRowIndex != 0)
                                                    //{
                                                    if (i == sumRowIndex)
                                                    {
                                                        //SET SUM ROW FONT
                                                        pcll.SetFont(HeaderPageTitleFont2);
                                                    }
                                                    //}
                                                    //if (grandSumRowIndex != 0)
                                                    //{
                                                    if (i == grandSumRowIndex)
                                                    {
                                                        //Set GrandSum row font Tabel 1
                                                        pcll.SetFont(SumRowFont);
                                                    }
                                                    //}
                                                    if (pcll.Content.ToString() == "SKIP")
                                                    {
                                                        pcll.SetBackgroundColor(Color.White);
                                                        pcll.SetForegroundColor(Color.White);
                                                    }
                                                }
                                                //TrxID WHITE
                                                //myPdfGrandTotals.Rows[i][0].SetForegroundColor(Color.White);
                                            }

                                            while (!myPdfGrandTotals.AllTablePagesCreated)
                                            {
                                                //Setting the Y position and if required creating new page
                                                if (currentYPos > myPdfDocument.PageHeight - 50)
                                                {
                                                    posY = 70;
                                                    currentYPos = 70;
                                                    newPdfPage.SaveToDocument();
                                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                                    newPdfPage = myPdfDocument.NewPage();
                                                    newPdfPage.Add(myHeaderPdfTablePage);
                                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                                    posY = currentYPos + 25;
                                                    if (myPdfHeaderPage != null)
                                                    {
                                                        currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                                        posY = currentYPos;
                                                    }
                                                    newPdfPage.Add(myPdfHeaderPage);
                                                    newPdfPage.Add(pdfLineBrk);
                                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));
                                                }
                                                else
                                                {
                                                    if (posY >= currentYPos)
                                                    {
                                                        if (myPdfHeaderPage != null)
                                                        {
                                                            currentYPos = posY + 9 + (myPdfHeaderPage.Area.PosY - posY) / 2;
                                                        }
                                                        posY = currentYPos;
                                                    }
                                                    else
                                                    {
                                                        posY = currentYPos;
                                                    }

                                                }
                                                if (myPdfDocument.PageHeight - 50 - posY < 50)
                                                {
                                                    posY = 70;
                                                    currentYPos = 70;
                                                    newPdfPage.SaveToDocument();
                                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                                    newPdfPage = myPdfDocument.NewPage();
                                                    newPdfPage.Add(myHeaderPdfTablePage);
                                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                                    posY = currentYPos + 25;
                                                    if (myPdfHeaderPage != null)
                                                    {
                                                        currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                                        posY = currentYPos;
                                                    }
                                                    newPdfPage.Add(myPdfHeaderPage);
                                                    newPdfPage.Add(pdfLineBrk);
                                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));

                                                }
                                                PdfTablePage newPdfTablePage2 = myPdfGrandTotals.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                                newPdfPage.Add(newPdfTablePage2);
                                                currentYPos = newPdfTablePage2.Area.BottomLeftVertex.Y;
                                            }

                                        }
                                        #endregion
                                    } //dt[2]2 close
                                    #endregion
                                } //dt[1]if condtion close
                            } // Table 1 if
                        }//dtnew close
                    }//Table 0  parent for loop end
                } //dt if condtion
                newPdfPage.SaveToDocument();
                CreatePDFDocument(fileName);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }
        #endregion

        #region Report Style 643
        public void ReportStyle643(DataTable[] dt, DataTable dtHeader, Hashtable[] arrhtFormatModes, bool PLayout, int[][] arrallcolWidths, string fileName)
        {
            try
            {
                if ((bool)PLayout)
                {
                    myPdfDocument = new PdfDocument(PdfDocumentFormat.A4_Horizontal);
                }
                else
                {
                    myPdfDocument = new PdfDocument(PdfDocumentFormat.A4);
                    pgHeight = 50;
                    pgWidth = 200;
                }
                string imgpath = PDFImagePath();
                PdfImage LogoImage = null;
                PdfTablePage myPdfHeaderPage = null;
                PdfLine pdfLineBrk = null;
                if (!string.IsNullOrEmpty(imgpath))
                {
                    LogoImage = myPdfDocument.NewImage(imgpath);
                }
                newPdfPage = GetHeaderPDFTableStyle1(dtHeader, out myHeaderPdfTablePage);
                ArrayList alRemovedColumns = null;
                int dt0Rows = dt[0].Rows.Count;
                //
                string[] strSumColumns ={ "Division1", "Division2", "Division3", "Total" };
                //Remove columns TrxID and Link1 from table add width 10
                int colstart = 0;
                int[] colParentWidths = new int[(arrallcolWidths[0].Length + strSumColumns.Length) - 2];
                colParentWidths[0] = arrallcolWidths[0][1] - 5;
                colstart++;
                //Add column widths for Division1,2,3
                for (int i = 0; i < strSumColumns.Length; i++)
                {
                    colParentWidths[colstart] = 19;
                    colstart++;
                }
                //
                DataTable dtParentHeader = new DataTable();
                dtParentHeader.Columns.Add("Caption");
                dtParentHeader.Columns.Add("Divison1");
                dtParentHeader.Columns.Add("Divison2");
                dtParentHeader.Columns.Add("Divison3");
                dtParentHeader.Columns.Add("Total");
                dtParentHeader.Rows.Add(dtParentHeader.NewRow());
                for (int col = 0; col < dtParentHeader.Columns.Count; col++)
                {
                    dtParentHeader.Rows[dtParentHeader.Rows.Count - 1][dtParentHeader.Columns[col].ToString()] = "SKIP";
                }
                PdfTable myPdfHeader = myPdfDocument.NewTable(RowFontBold, dtParentHeader.Rows.Count, dtParentHeader.Columns.Count, 1);
                myPdfHeader.ImportDataTable(dtParentHeader);
                myPdfHeader.SetBorders(Color.Black, 1, BorderType.None);
                myPdfHeader.SetContentAlignment(ContentAlignment.MiddleLeft);
                myPdfHeader.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                myPdfHeader.SetColumnsWidth(colParentWidths);
                for (int i = 0; i < dtParentHeader.Rows.Count; i++)
                {
                    foreach (PdfCell pcll in myPdfHeader.Rows[i].Cells)
                    {
                        if (pcll.Content.ToString() == "SKIP")
                        {
                            pcll.SetBackgroundColor(Color.White);
                            pcll.SetForegroundColor(Color.White);
                        }
                    }
                }
                foreach (PdfCell pcell in myPdfHeader.HeadersRow.Cells)
                {
                    switch (pcell.Content.ToString())
                    {
                        case "Caption":
                            {
                                pcell.SetBackgroundColor(Color.White);
                                pcell.SetForegroundColor(Color.White);
                                break;
                            }
                        default:
                            {
                                pcell.SetContentAlignment(ContentAlignment.MiddleRight);
                                break;
                            }
                    }
                }
                posY = currentYPos + 25;
                currentYPos = posY;
                //
                myPdfHeaderPage = myPdfHeader.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, (height - (posY))));
                newPdfPage.Add(myPdfHeaderPage);
                //
                double xPos = myPdfHeaderPage.CellArea(myPdfHeader.HeadersRow.Index, 0).TopLeftVertex.X;
                double yPos = myPdfHeaderPage.CellArea(myPdfHeader.HeadersRow.Index, 0).TopLeftVertex.Y;
                //
                Point pStart = new Point(Convert.ToInt32(xPos), Convert.ToInt32(yPos));
                Point pEnd = new Point(Convert.ToInt32(xPos + (myPdfDocument.PageWidth - 50)), Convert.ToInt32(yPos));
                pdfLineBrk = new PdfLine(myPdfDocument, pStart, pEnd, Color.Black, 1);
                newPdfPage.Add(pdfLineBrk);
                //
                #region Table0
                if (dt[0].Rows.Count > 0)
                {
                    string Child2Link3 = string.Empty;
                    for (int dtCnt = 0; dtCnt < dt0Rows; dtCnt++)
                    {
                        DataTable pDT = new DataTable();
                        DataTable dtNew = new DataTable();
                        DataRow[] foundRows = null;
                        string parentTrxID = string.Empty;
                        int colDivisionOrdinal = 0;
                        foundRows = dt[0].Select("Link1 ='" + dt[0].Rows[0]["Link1"].ToString() + "'");
                        if (foundRows.Length > 0)
                        {
                            int rowIndex = dt[0].Rows.IndexOf(foundRows[0]);
                            DataRow dt2Row = dtNew.NewRow();
                            if (dtNew.Columns.Count == 0)
                            {
                                for (int x = 0; x < dt[0].Columns.Count; x++)
                                {
                                    dtNew.Columns.Add(dt[0].Columns[x].ColumnName);
                                }
                                //ADD Columns Divisions 1, 2, 3
                                for (int i = 0; i < strSumColumns.Length; i++)
                                {
                                    dtNew.Columns.Add(strSumColumns[i].ToString());
                                }
                            }
                            for (int i = 0; i < dt[0].Columns.Count; i++)
                            {
                                if (dtNew.Columns.Contains(dt[0].Columns[i].ColumnName))
                                {
                                    switch (dt[0].Columns[i].ColumnName)
                                    {
                                        case "Caption":
                                            {
                                                dt2Row[i] = foundRows[0].ItemArray[i].ToString();
                                                break;
                                            }
                                    }
                                }
                            }
                            dtNew.Rows.Add(dt2Row);
                            parentTrxID = dt[0].Rows[0]["Link1"].ToString();
                            if (dt[0].Rows.Count > 0)
                            {
                                dt[0].Rows[0].Delete();
                            }
                            dtNew.AcceptChanges();
                            dtNew.TableName = dt[0].TableName;
                        }
                        alRemovedColumns = new ArrayList();
                        //To Get ordinals
                        if (dtNew.Columns.Contains("TrxID"))
                        {
                            alRemovedColumns.Add(dtNew.Columns["TrxID"].Ordinal);
                        }
                        if (dtNew.Columns.Contains("Link1"))
                        {
                            alRemovedColumns.Add(dtNew.Columns["Link1"].Ordinal);
                        }
                        //To Delete
                        if (dtNew.Columns.Contains("TrxID"))
                        {
                            dtNew.Columns.Remove("TrxID");
                        }
                        if (dtNew.Columns.Contains("Link1"))
                        {
                            dtNew.Columns.Remove("Link1");
                        }
                        if (colDivisionOrdinal == 0)
                        {
                            if (dtNew.Columns.Contains(strSumColumns[0].ToString()))
                            {
                                colDivisionOrdinal = dtNew.Columns[strSumColumns[0].ToString()].Ordinal;
                            }
                        }
                        //ADD PARENT TABLE ROW
                        if (dtNew.Rows.Count > 0)
                        {
                            PdfTable myPdfParent = myPdfDocument.NewTable(RowBoxFontBold, dtNew.Rows.Count, dtNew.Columns.Count, 1);
                            myPdfParent.ImportDataTable(dtNew);
                            myPdfParent.SetBorders(Color.Black, 1, BorderType.None);
                            myPdfParent.SetContentAlignment(ContentAlignment.MiddleLeft);
                            myPdfParent.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                            myPdfParent.SetColumnsWidth(colParentWidths);
                            myPdfParent.HeadersRow.SetFont(RowBoxFontBold);
                            myPdfParent.SetFont(HeaderPageTitleFont);
                            myPdfParent.HeadersRow.SetBackgroundColor(Color.White);
                            myPdfParent.HeadersRow.SetForegroundColor(Color.White);
                            for (int i = 0; i < dtNew.Rows.Count; i++)
                            {
                                //RIGHT ALIGNMENT DIVISON#1,2,3
                                for (int j = 0; j < strSumColumns.Length; j++)
                                {
                                    myPdfParent.Rows[i][colDivisionOrdinal + j].SetContentAlignment(ContentAlignment.MiddleRight);
                                    myPdfParent.Rows[i][colDivisionOrdinal + j].SetFont(GridHeaderFont);
                                }
                            }
                            while (!myPdfParent.AllTablePagesCreated)
                            {
                                //Setting the Y position and if required creating new page
                                if (currentYPos > myPdfDocument.PageHeight - 50)
                                {
                                    posY = 70;
                                    currentYPos = 70;
                                    newPdfPage.SaveToDocument();
                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                    newPdfPage = myPdfDocument.NewPage();
                                    newPdfPage.Add(myHeaderPdfTablePage);
                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                    posY = currentYPos + 25;
                                    if (myPdfHeaderPage != null)
                                    {
                                        currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                        posY = currentYPos;
                                    }
                                    newPdfPage.Add(myPdfHeaderPage);
                                    newPdfPage.Add(pdfLineBrk);
                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));

                                }
                                else
                                {
                                    if (posY >= currentYPos)
                                    {
                                        if (myPdfHeaderPage != null)
                                        {
                                            currentYPos = posY + 9 + (myPdfHeaderPage.Area.PosY - posY) / 2;
                                        }
                                        posY = currentYPos;
                                    }
                                    else
                                    {
                                        posY = currentYPos;
                                    }
                                }
                                if (myPdfDocument.PageHeight - 50 - posY < 50)
                                {
                                    posY = 70;
                                    currentYPos = 70;
                                    newPdfPage.SaveToDocument();
                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                    newPdfPage = myPdfDocument.NewPage();
                                    newPdfPage.Add(myHeaderPdfTablePage);
                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                    posY = currentYPos + 25;
                                    if (myPdfHeaderPage != null)
                                    {
                                        currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                        posY = currentYPos;
                                    }
                                    newPdfPage.Add(myPdfHeaderPage);
                                    newPdfPage.Add(pdfLineBrk);
                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));

                                }
                                PdfTablePage newPdfTablePage2 = myPdfParent.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                newPdfPage.Add(newPdfTablePage2);
                                currentYPos = newPdfTablePage2.Area.BottomLeftVertex.Y;
                                //
                            }
                #endregion
                            //Child DataTable
                            //CHILD TABLE : Report Category
                            string Child1Link2 = string.Empty;
                            Hashtable htGrandSumTable1 = null;
                            int[] colChild1Widths = null;
                            #region Table1
                            if (dt[1] != null)
                            {
                                int childRowIndex1 = 0;
                                DataRow[] drChildFound = dt[1].Select("Link1='" + parentTrxID + "'");
                                int dt1Rows = drChildFound.Length;
                                htGrandSumTable1 = new Hashtable();
                                for (int dtch = 0; dtch < dt1Rows; dtch++)
                                {
                                    DataRow[] drChildRow1 = dt[1].Select("Link1='" + parentTrxID + "' AND Link2=" + dt[1].Rows[0]["Link2"].ToString());
                                    DataTable dtFullChild1 = new DataTable();
                                    int[] childIndexes1 = new int[drChildRow1.Length];
                                    Hashtable htGrandTotals = new Hashtable();
                                    if (drChildRow1.Length > 0)
                                    {
                                        for (int drFoundRws = 0; drFoundRws < drChildRow1.Length; drFoundRws++)
                                        {
                                            childRowIndex1 = dt[1].Rows.IndexOf(drChildRow1[drFoundRws]);
                                            Child1Link2 = dt[1].Rows[drFoundRws]["Link2"].ToString();
                                            DataRow dtChRow = dtFullChild1.NewRow();
                                            if (dtFullChild1.Columns.Count == 0)
                                            {
                                                for (int x = 0; x < dt[1].Columns.Count; x++)
                                                {
                                                    dtFullChild1.Columns.Add(dt[1].Columns[x].ColumnName);
                                                }
                                            }
                                            for (int i = 0; i < dtFullChild1.Columns.Count; i++)
                                            {
                                                dtChRow[i] = drChildRow1[drFoundRws].ItemArray[i].ToString();
                                            }
                                            dtFullChild1.Rows.Add(dtChRow);
                                            if (dt[1].Rows[0]["Link2"].ToString() != null)
                                            {
                                                Child1Link2 = dt[1].Rows[0]["Link2"].ToString();
                                            }
                                            if (dt[1].Rows.Count > 0)
                                            {
                                                dt[1].Rows[0].Delete();
                                            }
                                            if (childIndexes1[0] != 0)
                                            {
                                                childIndexes1[drFoundRws] = childRowIndex1 + drFoundRws;
                                            }
                                            else
                                            {
                                                childIndexes1[drFoundRws] = childRowIndex1;
                                            }
                                            dtFullChild1.AcceptChanges();
                                            dtFullChild1.TableName = dt[1].TableName;
                                            if (dtFullChild1.Rows.Count > 0)
                                            {
                                                alRemovedColumns = new ArrayList();
                                                //Get ordinals to add arraylist
                                                if (dtFullChild1.Columns.Contains("TrxID"))
                                                {
                                                    alRemovedColumns.Add(dtFullChild1.Columns["TrxID"].Ordinal);
                                                }
                                                if (dtFullChild1.Columns.Contains("Link1"))
                                                {
                                                    alRemovedColumns.Add(dtFullChild1.Columns["Link1"].Ordinal);
                                                }
                                                if (dtFullChild1.Columns.Contains("Link2"))
                                                {
                                                    alRemovedColumns.Add(dtFullChild1.Columns["Link2"].Ordinal);
                                                }
                                                //To Delete
                                                if (dtFullChild1.Columns.Contains("TrxID"))
                                                {
                                                    dtFullChild1.Columns.Remove("TrxID");
                                                }
                                                if (dtFullChild1.Columns.Contains("Link1"))
                                                {
                                                    dtFullChild1.Columns.Remove("Link1");
                                                }
                                                if (dtFullChild1.Columns.Contains("Link2"))
                                                {
                                                    dtFullChild1.Columns.Remove("Link2");
                                                }
                                            }
                                            //Remove columns TrxID and Link1 from colwidths
                                            colstart = 0;
                                            colChild1Widths = new int[arrallcolWidths[1].Length - 3];
                                            for (int ctr = 0; ctr < arrallcolWidths[1].Length; ctr++)
                                            {
                                                if (!alRemovedColumns.Contains(ctr))
                                                {
                                                    colChild1Widths[colstart] = arrallcolWidths[1][ctr];
                                                    colstart++;
                                                }
                                            }
                                            if (dtFullChild1.Rows.Count > 0)
                                            {
                                                //
                                                dtFullChild1.Columns.Add("Col");
                                                dtFullChild1.Columns["Col"].SetOrdinal(0);
                                                //
                                                PdfTable myPdfChild = myPdfDocument.NewTable(HeaderPageTitleFont2, dtFullChild1.Rows.Count, dtFullChild1.Columns.Count, 1);
                                                myPdfChild.ImportDataTable(dtFullChild1);
                                                myPdfChild.SetBorders(Color.Black, 1, BorderType.None);
                                                myPdfChild.SetContentAlignment(ContentAlignment.MiddleLeft);
                                                myPdfChild.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                                myPdfChild.HeadersRow.SetFont(HeaderPageTitleFont2);
                                                myPdfChild.HeadersRow.SetBackgroundColor(Color.White);
                                                myPdfChild.HeadersRow.SetForegroundColor(Color.White);
                                                //Setting Column Width To Show Like a Zig zag fashion
                                                myPdfChild.Columns[0].SetWidth(1);
                                                //
                                                while (!myPdfChild.AllTablePagesCreated)
                                                {
                                                    //Setting the Y position and if required creating new page
                                                    if (currentYPos > myPdfDocument.PageHeight - 50)
                                                    {
                                                        posY = 70;
                                                        currentYPos = 70;
                                                        newPdfPage.SaveToDocument();
                                                        //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                                        newPdfPage = myPdfDocument.NewPage();
                                                        newPdfPage.Add(myHeaderPdfTablePage);
                                                        if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                                        posY = currentYPos + 25;
                                                        if (myPdfHeaderPage != null)
                                                        {
                                                            currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                                            posY = currentYPos;
                                                        }
                                                        newPdfPage.Add(myPdfHeaderPage);
                                                        newPdfPage.Add(pdfLineBrk);
                                                        newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));

                                                    }
                                                    else
                                                    {
                                                        if (posY >= currentYPos)
                                                        {
                                                            if (myPdfHeaderPage != null)
                                                            {
                                                                currentYPos = posY + 9 + (myPdfHeaderPage.Area.PosY - posY) / 2;
                                                            }
                                                            posY = currentYPos;
                                                        }
                                                        else
                                                        {
                                                            posY = currentYPos;
                                                        }
                                                    }
                                                    if (myPdfDocument.PageHeight - 50 - posY < 50)
                                                    {
                                                        posY = 70;
                                                        currentYPos = 70;
                                                        newPdfPage.SaveToDocument();
                                                        //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                                        newPdfPage = myPdfDocument.NewPage();
                                                        newPdfPage.Add(myHeaderPdfTablePage);
                                                        if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                                        posY = currentYPos + 25;
                                                        if (myPdfHeaderPage != null)
                                                        {
                                                            currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                                            posY = currentYPos;
                                                        }
                                                        newPdfPage.Add(myPdfHeaderPage);
                                                        newPdfPage.Add(pdfLineBrk);
                                                        newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));

                                                    }
                                                    PdfTablePage newPdfTablePage2 = myPdfChild.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                                    newPdfPage.Add(newPdfTablePage2);
                                                    currentYPos = newPdfTablePage2.Area.BottomLeftVertex.Y;
                                                }
                                            }
                                        }
                                    }
                            #endregion
                                    //CHILD TABLE : Account Details
                                    #region Table 2
                                    DataTable dtTotals = new DataTable();
                                    DataTable dtGrandTotals = new DataTable();
                                    if (dt[2] != null)
                                    {
                                        string childTrxID = string.Empty;
                                        int childRowIndex = 0;
                                        string strCustom = string.Empty;
                                        string strTotalAmount = string.Empty;
                                        double TotalAmount = 0;
                                        decimal amountRowTotal = 0;
                                        decimal amount;
                                        string strAmount = string.Empty;
                                        int colEndBalOrdinal = 0;
                                        DataRow[] drChildRow = dt[2].Select("Link1='" + parentTrxID + "' AND Link2=" + Child1Link2);
                                        DataTable dtFullChild = new DataTable();
                                        int[] childIndexes = new int[drChildRow.Length];
                                        //To store distinct subtotalids
                                        int dt2ColumnsCount = 0;
                                        if (drChildRow.Length > 0)
                                        {
                                            for (int drFoundRws2 = 0; drFoundRws2 < drChildRow.Length; drFoundRws2++)
                                            {
                                                childRowIndex = dt[2].Rows.IndexOf(drChildRow[drFoundRws2]);
                                                DataRow dtChRow2 = dtFullChild.NewRow();
                                                strAmount = string.Empty;
                                                amountRowTotal = 0;
                                                if (dtFullChild.Columns.Count == 0)
                                                {
                                                    for (int x = 0; x < dt[2].Columns.Count; x++)
                                                    {
                                                        dtFullChild.Columns.Add(dt[2].Columns[x].ColumnName);
                                                    }
                                                    dt2ColumnsCount = dtFullChild.Columns.Count;
                                                }
                                                for (int i = 0; i < dtFullChild.Columns.Count; i++)
                                                {
                                                    if (dt[2].Columns.Contains(dtFullChild.Columns[i].ColumnName))
                                                    {
                                                        dtChRow2[i] = drChildRow[drFoundRws2].ItemArray[i].ToString();
                                                    }
                                                }
                                                Child2Link3 = dt[2].Rows[0]["Link3"].ToString();
                                                dtFullChild.Rows.Add(dtChRow2);
                                                //Delete Row
                                                if (dt[2].Rows.Count > 0)
                                                {
                                                    dt[2].Rows[0].Delete();
                                                }
                                                dtFullChild.AcceptChanges();
                                                dtFullChild.TableName = dt[2].TableName;
                                                //Add Ending Balance from TrxDetail1 Table
                                                #region Table3
                                                if (dt[3] != null)
                                                {
                                                    int RowIndexDetail1 = 0;
                                                    DataRow[] drChildRowDetail1 = dt[3].Select("Link1='" + parentTrxID + "' AND Link2='" + Child1Link2 + "' AND  Link3='" + Child2Link3 + "'");
                                                    if (drChildRowDetail1.Length > 0)
                                                    {
                                                        RowIndexDetail1 = dt[3].Rows.IndexOf(drChildRowDetail1[0]);
                                                        if (drChildRowDetail1 != null)
                                                        {
                                                            strAmount = drChildRowDetail1[0]["Ending Balance"].ToString();
                                                            Decimal.TryParse(strAmount, out amount);
                                                            amountRowTotal = amountRowTotal + amount;
                                                            if (dtFullChild.Columns.Contains("Division1"))
                                                            {
                                                                dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Division1"] = strAmount;
                                                            }
                                                            else
                                                            {
                                                                dtFullChild.Columns.Add("Division1");
                                                                dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Division1"] = strAmount;
                                                            }
                                                            if (dt[3].Columns.Contains("Ending Balance"))
                                                            {
                                                                colEndBalOrdinal = dt[3].Columns["Ending Balance"].Ordinal;
                                                            }
                                                            if (dt[3].Rows.Count > 0)
                                                            {
                                                                dt[3].Rows[0].Delete();
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                                #region Table4
                                                //Add Ending Bal from TrxDetail2 Table
                                                if (dt[4] != null)
                                                {
                                                    int RowIndexDetail2 = 0;
                                                    DataRow[] drChildRowDetail2 = dt[4].Select("Link1='" + parentTrxID + "' AND Link2='" + Child1Link2 + "' AND  Link3='" + Child2Link3 + "'");
                                                    if (drChildRowDetail2.Length > 0)
                                                    {
                                                        RowIndexDetail2 = dt[4].Rows.IndexOf(drChildRowDetail2[0]);
                                                        if (drChildRowDetail2 != null)
                                                        {
                                                            strAmount = drChildRowDetail2[0]["Ending"].ToString();
                                                            Decimal.TryParse(strAmount, out amount);
                                                            amountRowTotal = amountRowTotal + amount;
                                                            if (dtFullChild.Columns.Contains("Division2"))
                                                            {
                                                                dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Division2"] = strAmount;
                                                            }
                                                            else
                                                            {
                                                                dtFullChild.Columns.Add("Division2");
                                                                dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Division2"] = strAmount;
                                                            }
                                                            //Delete 
                                                            if (dt[4].Rows.Count > 0)
                                                            {
                                                                dt[4].Rows[0].Delete();
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                                #region Table5
                                                //Add Ending Bal from TrxDetail2 Table
                                                if (dt[5] != null)
                                                {
                                                    int RowIndexDetail3 = 0;
                                                    DataRow[] drChildRowDetail3 = dt[5].Select("Link1='" + parentTrxID + "' AND Link2='" + Child1Link2 + "' AND  Link3='" + Child2Link3 + "'");
                                                    if (drChildRowDetail3.Length > 0)
                                                    {
                                                        RowIndexDetail3 = dt[5].Rows.IndexOf(drChildRowDetail3[0]);
                                                        strAmount = drChildRowDetail3[0]["Ending"].ToString();
                                                        Decimal.TryParse(strAmount, out amount);
                                                        amountRowTotal = amountRowTotal + amount;
                                                        if (drChildRowDetail3 != null)
                                                        {
                                                            if (dtFullChild.Columns.Contains("Division3"))
                                                            {
                                                                dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Division3"] = strAmount;
                                                            }
                                                            else
                                                            {
                                                                dtFullChild.Columns.Add("Division3");
                                                                dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Division3"] = strAmount;
                                                            }
                                                            //Delete 
                                                            if (dt[5].Rows.Count > 0)
                                                            {
                                                                dt[5].Rows[0].Delete();
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                                #region Add Total Column
                                                if (strAmount != string.Empty)
                                                {
                                                    strAmount = Convert.ToString(amountRowTotal);
                                                    strAmount = clsReportsUICore.ConvertToCurrencyFormat(strAmount);
                                                    if (dtFullChild.Columns.Contains("Total"))
                                                    {
                                                        dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Total"] = strAmount;
                                                    }
                                                    else
                                                    {
                                                        dtFullChild.Columns.Add("Total");
                                                        dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Total"] = strAmount;
                                                    }
                                                }
                                                #endregion
                                                //Add Sum Columns End Balance
                                                for (int i = 0; i < strSumColumns.Length; i++)
                                                {
                                                    if (dtFullChild.Columns.Contains(strSumColumns[i]))
                                                    {
                                                        strCustom = string.Empty;
                                                        TotalAmount = 0;
                                                        string amt = string.Empty;
                                                        string colName = strSumColumns[i];
                                                        if (!htGrandTotals.ContainsKey(colName))
                                                        {
                                                            amt = clsReportsUICore.ConvertToCurrencyFormat(dtFullChild.Rows[dtFullChild.Rows.Count - 1][colName].ToString());
                                                            htGrandTotals.Add(colName, amt);
                                                        }
                                                        else
                                                        {
                                                            TotalAmount = Convert.ToDouble(htGrandTotals[colName].ToString());
                                                            if (!string.IsNullOrEmpty(dtFullChild.Rows[dtFullChild.Rows.Count - 1][colName].ToString()))
                                                            {
                                                                TotalAmount = TotalAmount + Convert.ToDouble(dtFullChild.Rows[dtFullChild.Rows.Count - 1][colName].ToString());
                                                            }
                                                            strTotalAmount = Convert.ToString(TotalAmount);
                                                            amt = clsReportsUICore.ConvertToCurrencyFormat(strTotalAmount);
                                                            htGrandTotals[colName] = amt;
                                                        }
                                                    }
                                                }
                                                //
                                                if (childIndexes[0] != 0)
                                                {
                                                    childIndexes[drFoundRws2] = childRowIndex + drFoundRws2;
                                                }
                                                else
                                                {
                                                    childIndexes[drFoundRws2] = childRowIndex;
                                                }
                                            }
                                        }
                                        //To set font for sum rows
                                        int sumRowIndex = 0;
                                        //Add Columns to DtTotal
                                        if (dtTotals.Columns.Count == 0)
                                        {
                                            for (int x = 0; x < dtFullChild.Columns.Count; x++)
                                            {
                                                dtTotals.Columns.Add(dtFullChild.Columns[x].ColumnName);
                                            }
                                        }
                                        IDictionaryEnumerator enumTot = htGrandTotals.GetEnumerator();
                                        if (htGrandTotals.Keys.Count > 0)
                                        {
                                            dtTotals.Rows.Add(dtTotals.NewRow());
                                            //ADDING TOTAL VALUE
                                            dtTotals.Rows[dtTotals.Rows.Count - 1]["Account Name"] = "Total " + dtFullChild1.Rows[0]["Caption"].ToString();
                                            sumRowIndex = dtTotals.Rows.Count - 1;
                                            TotalAmount = 0;
                                            string amt = string.Empty;
                                            while (enumTot.MoveNext())
                                            {
                                                dtTotals.Rows[dtTotals.Rows.Count - 1][enumTot.Key.ToString()] = enumTot.Value.ToString();
                                                //Add GrandSum to Tabel1
                                                if (!htGrandSumTable1.ContainsKey(enumTot.Key.ToString()))
                                                {
                                                    //ADD
                                                    htGrandSumTable1.Add(enumTot.Key.ToString(), enumTot.Value.ToString());
                                                }
                                                else
                                                {
                                                    //UPDATE
                                                    TotalAmount = Convert.ToDouble(htGrandSumTable1[enumTot.Key.ToString()].ToString());
                                                    TotalAmount = TotalAmount + double.Parse(enumTot.Value.ToString());
                                                    strTotalAmount = Convert.ToString(TotalAmount);
                                                    amt = clsReportsUICore.ConvertToCurrencyFormat(strTotalAmount);
                                                    htGrandSumTable1[enumTot.Key.ToString()] = amt;
                                                }
                                            }
                                        }
                                        if (dtFullChild.Rows.Count > 0)
                                        {
                                            alRemovedColumns = new ArrayList();
                                            //Get ordinals to add arraylist
                                            if (dtFullChild.Columns.Contains("TrxID"))
                                            {
                                                alRemovedColumns.Add(dtFullChild.Columns["TrxID"].Ordinal);
                                            }
                                            if (dtFullChild.Columns.Contains("Link1"))
                                            {
                                                alRemovedColumns.Add(dtFullChild.Columns["Link1"].Ordinal);
                                            }
                                            if (dtFullChild.Columns.Contains("Link2"))
                                            {
                                                alRemovedColumns.Add(dtFullChild.Columns["Link2"].Ordinal);
                                            }
                                            if (dtFullChild.Columns.Contains("Link3"))
                                            {
                                                alRemovedColumns.Add(dtFullChild.Columns["Link3"].Ordinal);
                                            }
                                            //To Delete columns
                                            if (dtFullChild.Columns.Contains("TrxID"))
                                            {
                                                dtFullChild.Columns.Remove("TrxID");
                                            }
                                            if (dtFullChild.Columns.Contains("Link1"))
                                            {
                                                dtFullChild.Columns.Remove("Link1");
                                            }
                                            if (dtFullChild.Columns.Contains("Link2"))
                                            {
                                                dtFullChild.Columns.Remove("Link2");
                                            }
                                            if (dtFullChild.Columns.Contains("Link3"))
                                            {
                                                dtFullChild.Columns.Remove("Link3");
                                            }
                                            //Delete columns in dtTotals
                                            if (dtTotals.Columns.Contains("TrxID"))
                                            {
                                                dtTotals.Columns.Remove("TrxID");
                                            }
                                            if (dtTotals.Columns.Contains("Link1"))
                                            {
                                                dtTotals.Columns.Remove("Link1");
                                            }
                                            if (dtTotals.Columns.Contains("Link2"))
                                            {
                                                dtTotals.Columns.Remove("Link2");
                                            }
                                            if (dtTotals.Columns.Contains("Link3"))
                                            {
                                                dtTotals.Columns.Remove("Link3");
                                            }
                                        }
                                        //Check if the row not contains Ending columns
                                        dt2ColumnsCount = Convert.ToInt32(dt2ColumnsCount - 4) + 4;
                                        if (dtFullChild.Columns.Count < dt2ColumnsCount)
                                        {
                                            for (int i = 0; i < strSumColumns.Length; i++)
                                            {
                                                dtFullChild.Columns.Add(strSumColumns[i]);
                                                dtTotals.Columns.Add(strSumColumns[i]);
                                            }
                                        }
                                        //Remove columns TrxID and Link1 from colwidths
                                        colstart = 0;
                                        int[] colChild2Widths = new int[(arrallcolWidths[2].Length + 4) - 4];
                                        for (int ctr = 0; ctr < arrallcolWidths[2].Length; ctr++)
                                        {
                                            if (!alRemovedColumns.Contains(ctr))
                                            {
                                                colChild2Widths[colstart] = arrallcolWidths[2][ctr];
                                                colstart++;
                                            }
                                        }
                                        //Add Ending Columns Widths
                                        int EndbalWidth = 20;
                                        for (int i = 0; i < strSumColumns.Length; i++)
                                        {
                                            if (colEndBalOrdinal == 0)
                                            {
                                                colChild2Widths[colstart] = EndbalWidth;
                                            }
                                            else
                                            {
                                                colChild2Widths[colstart] = arrallcolWidths[3][colEndBalOrdinal];
                                            }
                                            colstart++;
                                        }
                                        if (colEndBalOrdinal == 0)
                                        {
                                            if (dtFullChild.Columns.Contains("Division1"))
                                            {
                                                //set ordeinal for empty columns
                                                colEndBalOrdinal = dtFullChild.Columns["Division1"].Ordinal;
                                            }
                                        }
                                        else
                                        {
                                            if (dtFullChild.Columns.Contains("Division1"))
                                            {
                                                //set ordeinal for empty columns
                                                colEndBalOrdinal = dtFullChild.Columns["Division1"].Ordinal;
                                            }
                                        }
                                        //ADD GrandTotals
                                        int grandSumRowIndex = 0;
                                        if (dtGrandTotals.Columns.Count == 0)
                                        {
                                            for (int x = 0; x < dtFullChild.Columns.Count; x++)
                                            {
                                                dtGrandTotals.Columns.Add(dtFullChild.Columns[x].ColumnName);
                                            }
                                        }
                                        if (dtch == dt1Rows - 1)
                                        {
                                            if (dtFullChild.Rows.Count > 0)
                                            {
                                                //ADD SKIP
                                                dtGrandTotals.Rows.Add(dtGrandTotals.NewRow());
                                                for (int col = 0; col < dtGrandTotals.Columns.Count; col++)
                                                {
                                                    dtGrandTotals.Rows[dtGrandTotals.Rows.Count - 1][dtGrandTotals.Columns[col].ToString()] = "SKIP";
                                                }
                                                //ADD GrandTotal sums for Tabel1 
                                                IDictionaryEnumerator enumGTot = htGrandSumTable1.GetEnumerator();
                                                if (htGrandSumTable1.Keys.Count > 0)
                                                {
                                                    //ADD GrandTotals
                                                    dtGrandTotals.Rows.Add(dtGrandTotals.NewRow());
                                                    dtGrandTotals.Rows[dtGrandTotals.Rows.Count - 1]["Account Name"] = "TOTAL " + dtNew.Rows[0]["Caption"].ToString().ToUpper();

                                                    grandSumRowIndex = dtGrandTotals.Rows.Count - 1;
                                                    while (enumGTot.MoveNext())
                                                    {
                                                        if (dtGrandTotals.Columns.Contains(enumGTot.Key.ToString()))
                                                        {
                                                            dtGrandTotals.Rows[dtGrandTotals.Rows.Count - 1][enumGTot.Key.ToString()] = enumGTot.Value.ToString();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (dtFullChild.Rows.Count > 0)
                                        {
                                            dtFullChild.Columns.Add("Col");
                                            dtFullChild.Columns["Col"].SetOrdinal(0);
                                            //
                                            int[] colModWidths = new int[colParentWidths.Length + 1];
                                            for (int wdts = 0; wdts < colModWidths.Length; wdts++)
                                            {
                                                if (wdts == 0)
                                                {
                                                    colModWidths[wdts] = 5;
                                                }
                                                else
                                                {
                                                    if (wdts == 1)
                                                    {
                                                        colModWidths[wdts] = colParentWidths[0] - colModWidths[0];
                                                    }
                                                    else
                                                    {
                                                        colModWidths[wdts] = colParentWidths[wdts - 1];
                                                    }
                                                }
                                            }
                                            //
                                            PdfTable myPdfChild = myPdfDocument.NewTable(FontRegular, dtFullChild.Rows.Count, dtFullChild.Columns.Count, 1);
                                            myPdfChild.ImportDataTable(dtFullChild);
                                            myPdfChild.SetBorders(Color.Black, 1, BorderType.None);
                                            myPdfChild.SetContentAlignment(ContentAlignment.MiddleLeft);
                                            myPdfChild.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                            myPdfChild.SetColumnsWidth(colModWidths);
                                            myPdfChild.HeadersRow.SetFont(FontRegular);
                                            myPdfChild.HeadersRow.SetBackgroundColor(Color.White);
                                            myPdfChild.HeadersRow.SetForegroundColor(Color.White);
                                            //
                                            colEndBalOrdinal = 2;
                                            //
                                            for (int i = 0; i < dtFullChild.Rows.Count; i++)
                                            {
                                                //SET RIGHT ALIGNMENT
                                                for (int j = 0; j < strSumColumns.Length; j++)
                                                {
                                                    if (j < (colEndBalOrdinal + j))
                                                    {
                                                        myPdfChild.Rows[i][colEndBalOrdinal + j].SetContentAlignment(ContentAlignment.MiddleRight);
                                                    }
                                                }
                                                foreach (PdfCell pcll in myPdfChild.Rows[i].Cells)
                                                {
                                                    if (pcll.Content.ToString() == "SKIP")
                                                    {
                                                        pcll.SetBackgroundColor(Color.White);
                                                        pcll.SetForegroundColor(Color.White);
                                                    }
                                                }
                                            }
                                            colEndBalOrdinal = 1;
                                            while (!myPdfChild.AllTablePagesCreated)
                                            {
                                                //Setting the Y position and if required creating new page
                                                if (currentYPos > myPdfDocument.PageHeight - 50)
                                                {
                                                    posY = 70;
                                                    currentYPos = 70;
                                                    newPdfPage.SaveToDocument();
                                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                                    newPdfPage = myPdfDocument.NewPage();
                                                    newPdfPage.Add(myHeaderPdfTablePage);
                                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                                    posY = currentYPos + 25;
                                                    if (myPdfHeaderPage != null)
                                                    {
                                                        currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                                        posY = currentYPos;
                                                    }
                                                    newPdfPage.Add(myPdfHeaderPage);
                                                    newPdfPage.Add(pdfLineBrk);
                                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));

                                                }
                                                else
                                                {
                                                    if (posY >= currentYPos)
                                                    {
                                                        if (myPdfHeaderPage != null)
                                                        {
                                                            currentYPos = posY + 9 + (myPdfHeaderPage.Area.PosY - posY) / 2;
                                                        }
                                                        posY = currentYPos;
                                                    }
                                                    else
                                                    {
                                                        posY = currentYPos;
                                                    }
                                                }
                                                if (myPdfDocument.PageHeight - 50 - posY < 50)
                                                {
                                                    posY = 70;
                                                    currentYPos = 70;
                                                    newPdfPage.SaveToDocument();
                                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                                    newPdfPage = myPdfDocument.NewPage();
                                                    newPdfPage.Add(myHeaderPdfTablePage);
                                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                                    posY = currentYPos + 25;
                                                    if (myPdfHeaderPage != null)
                                                    {
                                                        currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                                        posY = currentYPos;
                                                    }
                                                    newPdfPage.Add(myPdfHeaderPage);
                                                    newPdfPage.Add(pdfLineBrk);
                                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));

                                                }
                                                PdfTablePage newPdfTablePage2 = myPdfChild.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                                newPdfPage.Add(newPdfTablePage2);
                                                currentYPos = newPdfTablePage2.Area.BottomLeftVertex.Y;
                                            }
                                        }
                                        #region Table Totals
                                        if (dtTotals.Rows.Count > 0)
                                        {
                                            PdfTable myPdfChildTotals = myPdfDocument.NewTable(FontRegular, dtTotals.Rows.Count, dtTotals.Columns.Count, 1);
                                            myPdfChildTotals.ImportDataTable(dtTotals);
                                            myPdfChildTotals.SetBorders(Color.Black, 1, BorderType.None);
                                            myPdfChildTotals.SetContentAlignment(ContentAlignment.MiddleLeft);
                                            myPdfChildTotals.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                            //myPdfChildTotals.SetColumnsWidth(colChild2Widths);
                                            myPdfChildTotals.SetColumnsWidth(colParentWidths);
                                            myPdfChildTotals.HeadersRow.SetFont(FontRegular);
                                            myPdfChildTotals.HeadersRow.SetBackgroundColor(Color.White);
                                            myPdfChildTotals.HeadersRow.SetForegroundColor(Color.White);
                                            for (int i = 0; i < dtTotals.Rows.Count; i++)
                                            {
                                                //SET RIGHT ALIGNMENT
                                                for (int j = 0; j < strSumColumns.Length; j++)
                                                {
                                                    myPdfChildTotals.Rows[i][colEndBalOrdinal + j].SetContentAlignment(ContentAlignment.MiddleRight);
                                                }
                                                foreach (PdfCell pcll in myPdfChildTotals.Rows[i].Cells)
                                                {
                                                    if (i == sumRowIndex)
                                                    {
                                                        //SET SUM ROW FONT
                                                        pcll.SetFont(HeaderPageTitleFont2);
                                                    }
                                                    if (i == grandSumRowIndex)
                                                    {
                                                        //Set GrandSum row font Tabel 1
                                                        pcll.SetFont(SumRowFont);
                                                    }
                                                    if (pcll.Content.ToString() == "SKIP")
                                                    {
                                                        pcll.SetBackgroundColor(Color.White);
                                                        pcll.SetForegroundColor(Color.White);
                                                    }
                                                }
                                                //TrxID WHITE
                                            }
                                            while (!myPdfChildTotals.AllTablePagesCreated)
                                            {
                                                //Setting the Y position and if required creating new page
                                                if (currentYPos > myPdfDocument.PageHeight - 50)
                                                {
                                                    posY = 70;
                                                    currentYPos = 70;
                                                    newPdfPage.SaveToDocument();
                                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                                    newPdfPage = myPdfDocument.NewPage();
                                                    newPdfPage.Add(myHeaderPdfTablePage);
                                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                                    posY = currentYPos + 25;
                                                    if (myPdfHeaderPage != null)
                                                    {
                                                        currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                                        posY = currentYPos;
                                                    }
                                                    newPdfPage.Add(myPdfHeaderPage);
                                                    newPdfPage.Add(pdfLineBrk);
                                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));

                                                }
                                                else
                                                {
                                                    if (posY >= currentYPos)
                                                    {
                                                        if (myPdfHeaderPage != null)
                                                        {
                                                            currentYPos = posY + 9 + (myPdfHeaderPage.Area.PosY - posY) / 2;
                                                        }
                                                        posY = currentYPos;
                                                    }
                                                    else
                                                    {
                                                        posY = currentYPos;
                                                    }
                                                }
                                                if (myPdfDocument.PageHeight - 50 - posY < 50)
                                                {
                                                    posY = 70;
                                                    currentYPos = 70;
                                                    newPdfPage.SaveToDocument();
                                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                                    newPdfPage = myPdfDocument.NewPage();
                                                    newPdfPage.Add(myHeaderPdfTablePage);
                                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                                    posY = currentYPos + 25;
                                                    if (myPdfHeaderPage != null)
                                                    {
                                                        currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                                        posY = currentYPos;
                                                    }
                                                    newPdfPage.Add(myPdfHeaderPage);
                                                    newPdfPage.Add(pdfLineBrk);
                                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));

                                                }
                                                PdfTablePage newPdfTablePage2 = myPdfChildTotals.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                                newPdfPage.Add(newPdfTablePage2);
                                                currentYPos = newPdfTablePage2.Area.BottomLeftVertex.Y;
                                            }
                                        }
                                        #endregion
                                        #region GrandTotals
                                        if (dtGrandTotals.Rows.Count > 0)
                                        {
                                            PdfTable myPdfGrandTotals = myPdfDocument.NewTable(FontRegular, dtGrandTotals.Rows.Count, dtGrandTotals.Columns.Count, 1);
                                            myPdfGrandTotals.ImportDataTable(dtGrandTotals);
                                            myPdfGrandTotals.SetBorders(Color.Black, 1, BorderType.None);
                                            myPdfGrandTotals.SetContentAlignment(ContentAlignment.MiddleLeft);
                                            myPdfGrandTotals.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                            //myPdfGrandTotals.SetColumnsWidth(colChild2Widths);
                                            myPdfGrandTotals.SetColumnsWidth(colParentWidths);
                                            myPdfGrandTotals.HeadersRow.SetFont(FontRegular);
                                            myPdfGrandTotals.HeadersRow.SetBackgroundColor(Color.White);
                                            myPdfGrandTotals.HeadersRow.SetForegroundColor(Color.White);
                                            for (int i = 0; i < dtGrandTotals.Rows.Count; i++)
                                            {
                                                //SET RIGHT ALIGNMENT
                                                for (int j = 0; j < strSumColumns.Length; j++)
                                                {
                                                    myPdfGrandTotals.Rows[i][colEndBalOrdinal + j].SetContentAlignment(ContentAlignment.MiddleRight);
                                                }
                                                foreach (PdfCell pcll in myPdfGrandTotals.Rows[i].Cells)
                                                {
                                                    if (i == sumRowIndex)
                                                    {
                                                        //SET SUM ROW FONT
                                                        pcll.SetFont(HeaderPageTitleFont2);
                                                    }
                                                    if (i == grandSumRowIndex)
                                                    {
                                                        //Set GrandSum row font Tabel 1
                                                        pcll.SetFont(SumRowFont);
                                                    }
                                                    if (pcll.Content.ToString() == "SKIP")
                                                    {
                                                        pcll.SetBackgroundColor(Color.White);
                                                        pcll.SetForegroundColor(Color.White);
                                                    }
                                                }
                                            }
                                            while (!myPdfGrandTotals.AllTablePagesCreated)
                                            {
                                                //Setting the Y position and if required creating new page
                                                if (currentYPos > myPdfDocument.PageHeight - 50)
                                                {
                                                    posY = 70;
                                                    currentYPos = 70;
                                                    newPdfPage.SaveToDocument();
                                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                                    newPdfPage = myPdfDocument.NewPage();
                                                    newPdfPage.Add(myHeaderPdfTablePage);
                                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                                    posY = currentYPos + 25;
                                                    if (myPdfHeaderPage != null)
                                                    {
                                                        currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                                        posY = currentYPos;
                                                    }
                                                    newPdfPage.Add(myPdfHeaderPage);
                                                    newPdfPage.Add(pdfLineBrk);
                                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));

                                                }
                                                else
                                                {
                                                    if (posY >= currentYPos)
                                                    {
                                                        if (myPdfHeaderPage != null)
                                                        {
                                                            currentYPos = posY + 9 + (myPdfHeaderPage.Area.PosY - posY) / 2;
                                                        }
                                                        posY = currentYPos;
                                                    }
                                                    else
                                                    {
                                                        posY = currentYPos;
                                                    }
                                                }
                                                if (myPdfDocument.PageHeight - 50 - posY < 50)
                                                {
                                                    posY = 70;
                                                    currentYPos = 70;
                                                    newPdfPage.SaveToDocument();
                                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                                    newPdfPage = myPdfDocument.NewPage();
                                                    newPdfPage.Add(myHeaderPdfTablePage);
                                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                                    posY = currentYPos + 25;
                                                    if (myPdfHeaderPage != null)
                                                    {
                                                        currentYPos = posY + 9 + ((myPdfHeaderPage.Area.PosY - posY) / 2);
                                                        posY = currentYPos;
                                                    }
                                                    newPdfPage.Add(myPdfHeaderPage);
                                                    newPdfPage.Add(pdfLineBrk);
                                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));

                                                }
                                                PdfTablePage newPdfTablePage2 = myPdfGrandTotals.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                                newPdfPage.Add(newPdfTablePage2);
                                                currentYPos = newPdfTablePage2.Area.BottomLeftVertex.Y;
                                            }
                                        }
                                        #endregion

                                    } //dt[2]2 close
                                    #endregion
                                } //dt[1]if condtion close
                            } // Table 1 if
                        }//dtnew close
                    }//Table 0  parent for loop end
                } //dt if condtion
                newPdfPage.SaveToDocument();
                CreatePDFDocument(fileName);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }
        #endregion


        #region Report Style 651
        public void ReportStyle651(DataTable[] dt, DataTable dtHeader, Hashtable[] arrhtFormatModes, bool PLayout, int[][] arrallcolWidths, Hashtable[] ArrhtColNameValues, string fileName)
        {
            try
            {
                if ((bool)PLayout)
                {
                    myPdfDocument = new PdfDocument(PdfDocumentFormat.A4_Horizontal);
                }
                else
                {
                    myPdfDocument = new PdfDocument(PdfDocumentFormat.A4);
                    pgHeight = 50;
                    pgWidth = 200;
                }
                string imgpath = PDFImagePath();
                PdfImage LogoImage = null;
                if (!string.IsNullOrEmpty(imgpath))
                {
                    LogoImage = myPdfDocument.NewImage(imgpath);
                }
                newPdfPage = GetHeaderPDFTableStyle1(dtHeader, out myHeaderPdfTablePage);
                ArrayList alRemovedColumns = null;
                int dt0Rows = dt[0].Rows.Count;
                Font fontMedium = new Font("Verdana", 8, FontStyle.Bold);
                Font fontSumTotals = new Font("Verdana", 8, FontStyle.Bold);
                Font fontGrid = new Font("Verdana", 8, FontStyle.Regular);
                PdfTablePage pdfMainHeaderPage = null;
                PdfLine pdfLineBrk = null;
                int[] colHeaderWidths = null;
                int actualfullview = 19;
                //
                #region PDF Header
                DataTable dtFormattedHeader = new DataTable();
                dtFormattedHeader.Columns.Add("Caption");
                dtFormattedHeader.Columns.Add("Actual");
                dtFormattedHeader.Columns.Add("Budget");
                dtFormattedHeader.Columns.Add("Variance");
                dtFormattedHeader.Rows.Add(dtFormattedHeader.NewRow());
                for (int col = 0; col < dtFormattedHeader.Columns.Count; col++)
                {
                    dtFormattedHeader.Rows[dtFormattedHeader.Rows.Count - 1][dtFormattedHeader.Columns[col].ToString()] = "SKIP";
                }
                colHeaderWidths = new int[dtFormattedHeader.Columns.Count];
                int colcnt = 0;
                for (int i = 0; i < colHeaderWidths.Length; i++)
                {
                    if (i == 0)
                    {
                        //Caption width
                        colHeaderWidths[colcnt] = arrallcolWidths[0][1];
                    }
                    else
                    {
                        colHeaderWidths[colcnt] = actualfullview;
                    }
                    colcnt++;
                }
                PdfTable pdfMainHeader = myPdfDocument.NewTable(FontRegular, dtFormattedHeader.Rows.Count, dtFormattedHeader.Columns.Count, 1);
                pdfMainHeader.ImportDataTable(dtFormattedHeader);
                pdfMainHeader.SetBorders(Color.Black, 1, BorderType.None);
                pdfMainHeader.SetContentAlignment(ContentAlignment.MiddleLeft);
                pdfMainHeader.HeadersRow.SetContentAlignment(ContentAlignment.MiddleCenter);
                pdfMainHeader.SetColumnsWidth(colHeaderWidths);
                pdfMainHeader.HeadersRow.SetFont(HeaderFont);
                //
                foreach (PdfCell pHeaderCll in pdfMainHeader.HeadersRow.Cells)
                {
                    string labelName = pHeaderCll.Content.ToString();
                    switch (labelName)
                    {
                        case "Caption":
                            {
                                pHeaderCll.SetBackgroundColor(Color.White);
                                pHeaderCll.SetForegroundColor(Color.White);
                                break;
                            }
                        case "Actuals":
                        case "Budget":
                        case "Variance":
                            {
                                pHeaderCll.SetContentAlignment(ContentAlignment.MiddleCenter);
                                break;
                            }
                    }
                }
                foreach (PdfCell pskipCll in pdfMainHeader.Cells)
                {
                    string labelValue = pskipCll.Content.ToString();
                    switch (labelValue)
                    {
                        case "SKIP":
                            {
                                pskipCll.SetBackgroundColor(Color.White);
                                pskipCll.SetForegroundColor(Color.White);
                                break;
                            }
                    }
                }
                //
                posY = currentYPos + 25;
                currentYPos = posY;
                //
                pdfMainHeaderPage = pdfMainHeader.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, (height - (posY))));
                newPdfPage.Add(pdfMainHeaderPage);
                //
                double xPos = pdfMainHeaderPage.CellArea(pdfMainHeader.HeadersRow.Index, 0).TopLeftVertex.X;
                double yPos = pdfMainHeaderPage.CellArea(pdfMainHeader.HeadersRow.Index, 0).TopLeftVertex.Y;
                //
                Point pStart = new Point(Convert.ToInt32(xPos), Convert.ToInt32(yPos));
                Point pEnd = new Point(Convert.ToInt32(xPos + (myPdfDocument.PageWidth - 50)), Convert.ToInt32(yPos));
                pdfLineBrk = new PdfLine(myPdfDocument, pStart, pEnd, Color.Black, 1);
                newPdfPage.Add(pdfLineBrk);
                #endregion
                DataTable dtChild1 = dt[1];
                DataTable dtChild2 = dt[2];
                DataTable dtChild3 = dt[3];
                //Calculations
                DataTable dtProfits = new DataTable();
                Hashtable htRevenue = new Hashtable();
                Hashtable htGrossProfit = new Hashtable();
                Hashtable htNetIncomeBeforeTaxes = new Hashtable();
                //PARENT TABLE : Report Detail
                if (dt[0].Rows.Count > 0)
                {
                    string Child2Link3 = string.Empty;
                    string[] strSumColumns ={ "Actual", "Budget", "Variance" };
                    #region Table0
                    for (int dtCnt = 0; dtCnt < dt0Rows; dtCnt++)
                    {
                        DataTable pDT = new DataTable();
                        DataTable dtNew = new DataTable();
                        DataRow[] foundRows = null;
                        string parentTrxID = string.Empty;
                        foundRows = dt[0].Select("Link1 ='" + dt[0].Rows[0]["Link1"].ToString() + "'");
                        if (foundRows.Length > 0)
                        {
                            int rowIndex = dt[0].Rows.IndexOf(foundRows[0]);
                            DataRow dt2Row = dtNew.NewRow();
                            if (dtNew.Columns.Count == 0)
                            {
                                for (int x = 0; x < dt[0].Columns.Count; x++)
                                {
                                    dtNew.Columns.Add(dt[0].Columns[x].ColumnName);
                                }
                            }
                            for (int i = 0; i < dtNew.Columns.Count; i++)
                            {
                                dt2Row[i] = foundRows[0].ItemArray[i].ToString();
                            }
                            dtNew.Rows.Add(dt2Row);
                            parentTrxID = dt[0].Rows[0]["Link1"].ToString();
                            if (dt[0].Rows.Count > 0)
                            {
                                dt[0].Rows[0].Delete();
                            }
                            dtNew.AcceptChanges();
                            dtNew.TableName = dt[0].TableName;
                        }
                        alRemovedColumns = new ArrayList();
                        //To Get ordinals
                        if (dtNew.Columns.Contains("TrxID"))
                        {
                            alRemovedColumns.Add(dtNew.Columns["TrxID"].Ordinal);
                        }
                        if (dtNew.Columns.Contains("Link1"))
                        {
                            alRemovedColumns.Add(dtNew.Columns["Link1"].Ordinal);
                        }
                        int colstart = 0;
                        //Remove columns TrxID and Link1 from table
                        int[] colParentWidths = new int[(arrallcolWidths[0].Length - 2) + 3];
                        for (int ctr = 0; ctr < arrallcolWidths[0].Length; ctr++)
                        {
                            if (!alRemovedColumns.Contains(ctr))
                            {
                                colParentWidths[colstart] = arrallcolWidths[0][ctr];
                                colstart++;
                            }
                        }

                        //ADD Profit columns
                        for (int i = 0; i < strSumColumns.Length; i++)
                        {
                            if (!dtNew.Columns.Contains(strSumColumns[i].ToString()))
                            {
                                dtNew.Columns.Add(strSumColumns[i].ToString());
                                colParentWidths[colstart] = actualfullview;
                                colstart++;
                            }
                        }
                        //set actual oridnal
                        int colActualOridnal = 0;
                        if (dtNew.Columns.Contains(strSumColumns[0].ToString()))
                        {
                            colActualOridnal = dtNew.Columns[strSumColumns[0].ToString()].Ordinal;
                        }
                        #region Profit Calcuations
                        if (dtNew.Rows.Count > 0)
                        {
                            double amtprofit1 = 0;
                            double amtprofit2 = 0;
                            double amtdiff = 0;
                            string strAmount = string.Empty;
                            string strFinalamt = string.Empty;
                            string strCaption = dtNew.Rows[dtNew.Rows.Count - 1]["Link1"].ToString().ToUpper();
                            switch (strCaption)
                            {
                                case "30":
                                    {
                                        // Gross Profit = Revenue - Production
                                        //ADD NEW ROW for profit percentage
                                        dtNew.Rows.Add(dtNew.NewRow());
                                        for (int i = 0; i < strSumColumns.Length; i++)
                                        {
                                            if (dtProfits != null)
                                            {
                                                amtprofit1 = 0;
                                                amtprofit2 = 0;

                                                if (dtProfits.Rows[0] != null)
                                                {
                                                    amtprofit1 = Convert.ToDouble(dtProfits.Rows[0][strSumColumns[i].ToString()]);
                                                }
                                                if (dtProfits.Rows.Count == 2)
                                                {
                                                    if (dtProfits.Rows[1] != null)
                                                    {
                                                        amtprofit2 = Convert.ToDouble(dtProfits.Rows[1][strSumColumns[i].ToString()]);
                                                    }
                                                }
                                                amtdiff = amtprofit1 - amtprofit2;
                                                strAmount = Convert.ToString(amtdiff);
                                                strAmount = clsReportsUICore.ConvertToCurrencyFormat(strAmount);
                                                //Gross profit
                                                dtNew.Rows[dtNew.Rows.Count - 2][strSumColumns[i].ToString()] = strAmount;
                                                //Add Gross profit to hashtable
                                                if (htGrossProfit.Contains(strSumColumns[i].ToString()))
                                                {
                                                    //UPDATE
                                                    htGrossProfit[strSumColumns[i].ToString()] = strAmount;
                                                }
                                                else
                                                {
                                                    //ADD
                                                    htGrossProfit.Add(strSumColumns[i].ToString(), strAmount);
                                                }
                                                //Gross pfofit in percentage  
                                                if (htRevenue.Contains(strSumColumns[i].ToString()))
                                                {
                                                    //Grossprofit / Revenue
                                                    amtprofit1 = Convert.ToDouble(strAmount);
                                                    amtprofit2 = Convert.ToDouble(htRevenue[strSumColumns[i].ToString()].ToString());
                                                    amtdiff = 0;
                                                    if (amtprofit2 != 0)
                                                    {
                                                        amtdiff = (amtprofit1 / amtprofit2) * 100;
                                                    }
                                                    strAmount = Convert.ToString(amtdiff);
                                                    strAmount = clsReportsUICore.ConvertToCurrencyFormat(strAmount) + "%";
                                                    dtNew.Rows[dtNew.Rows.Count - 1][strSumColumns[i].ToString()] = strAmount;
                                                }
                                            }
                                        }
                                        //Clear the dtProfits table
                                        dtProfits.Clear();
                                        break;
                                    }
                                case "60":
                                    {
                                        // NetIncoeBeforeTaxes =  GrossProft - OtherCost + OtherIncome
                                        //ADD NEW ROW for profit percentage
                                        dtNew.Rows.Add(dtNew.NewRow());
                                        for (int i = 0; i < strSumColumns.Length; i++)
                                        {
                                            if (dtProfits != null)
                                            {
                                                amtprofit1 = 0;
                                                amtprofit2 = 0;
                                                if (dtProfits.Rows[0] != null)
                                                {
                                                    amtprofit1 = Convert.ToDouble(dtProfits.Rows[0][strSumColumns[i].ToString()]);
                                                }
                                                if (dtProfits.Rows.Count == 2)
                                                {
                                                    if (dtProfits.Rows[1] != null)
                                                    {
                                                        amtprofit2 = Convert.ToDouble(dtProfits.Rows[1][strSumColumns[i].ToString()]);
                                                    }
                                                }
                                                //Get grossprofit amount
                                                if (htGrossProfit.Contains(strSumColumns[i].ToString()))
                                                {
                                                    //here amtdiff is grossprofit
                                                    amtdiff = Convert.ToDouble(htGrossProfit[strSumColumns[i].ToString()].ToString());
                                                    amtdiff = (amtdiff - (amtprofit1 + amtprofit2));
                                                }
                                                strAmount = Convert.ToString(amtdiff);
                                                strAmount = clsReportsUICore.ConvertToCurrencyFormat(strAmount);
                                                //Net Income Before Taxes
                                                dtNew.Rows[dtNew.Rows.Count - 2][strSumColumns[i].ToString()] = strAmount;

                                                //Add NetIncomeBeforeTaxes to hashtable
                                                if (htNetIncomeBeforeTaxes.Contains(strSumColumns[i].ToString()))
                                                {
                                                    //UPDATE
                                                    htNetIncomeBeforeTaxes[strSumColumns[i].ToString()] = strAmount;
                                                }
                                                else
                                                {
                                                    //ADD
                                                    htNetIncomeBeforeTaxes.Add(strSumColumns[i].ToString(), strAmount);
                                                }
                                                //Net Income in percentage  
                                                if (htRevenue.Contains(strSumColumns[i].ToString()))
                                                {
                                                    //Net Income / Revenue
                                                    amtprofit1 = Convert.ToDouble(strAmount);
                                                    amtprofit2 = Convert.ToDouble(htRevenue[strSumColumns[i].ToString()].ToString());
                                                    amtdiff = 0;
                                                    if (amtprofit2 != 0)
                                                    {
                                                        amtdiff = (amtprofit1 / amtprofit2) * 100;
                                                    }
                                                    strAmount = Convert.ToString(amtdiff);
                                                    strAmount = clsReportsUICore.ConvertToCurrencyFormat(strAmount) + "%";
                                                    dtNew.Rows[dtNew.Rows.Count - 1][strSumColumns[i].ToString()] = strAmount;
                                                }
                                            }
                                        }
                                        //Clear the dtProfits table
                                        dtProfits.Clear();
                                        break;
                                    }
                                case "900":
                                    {
                                        // NetIncome = NetIncomeBeforeTaxes - TaxTotal
                                        //ADD NEW ROW for profit percentage
                                        dtNew.Rows.Add(dtNew.NewRow());
                                        for (int i = 0; i < strSumColumns.Length; i++)
                                        {
                                            if (dtProfits != null)
                                            {
                                                amtprofit1 = 0;
                                                amtprofit2 = 0;

                                                if (htNetIncomeBeforeTaxes.Contains(strSumColumns[i].ToString()))
                                                {
                                                    amtprofit1 = Convert.ToDouble(htNetIncomeBeforeTaxes[strSumColumns[i].ToString()].ToString());
                                                }
                                                if (dtProfits.Rows[0] != null)
                                                {
                                                    amtprofit2 = Convert.ToDouble(dtProfits.Rows[0][strSumColumns[i].ToString()]);
                                                }

                                                amtdiff = amtprofit1 - amtprofit2;
                                                strAmount = Convert.ToString(amtdiff);
                                                strAmount = clsReportsUICore.ConvertToCurrencyFormat(strAmount);
                                                //Net Income
                                                dtNew.Rows[dtNew.Rows.Count - 2][strSumColumns[i].ToString()] = strAmount;

                                                //Net Income in percentage  
                                                if (htRevenue.Contains(strSumColumns[i].ToString()))
                                                {
                                                    //Grossprofit / Revenue
                                                    amtprofit1 = Convert.ToDouble(strAmount);
                                                    amtprofit2 = Convert.ToDouble(htRevenue[strSumColumns[i].ToString()].ToString());
                                                    amtdiff = 0;
                                                    if (amtprofit2 != 0)
                                                    {
                                                        amtdiff = (amtprofit1 / amtprofit2) * 100;
                                                    }
                                                    strAmount = Convert.ToString(amtdiff);
                                                    strAmount = clsReportsUICore.ConvertToCurrencyFormat(strAmount) + "%";
                                                    dtNew.Rows[dtNew.Rows.Count - 1][strSumColumns[i].ToString()] = strAmount;
                                                }
                                            }
                                        }
                                        //Clear the dtProfits table
                                        dtProfits.Clear();
                                        break;
                                    }
                                default:
                                    break;
                            }
                            //To Delete
                            if (dtNew.Columns.Contains("TrxID"))
                            {
                                dtNew.Columns.Remove("TrxID");
                            }
                            if (dtNew.Columns.Contains("Link1"))
                            {
                                dtNew.Columns.Remove("Link1");
                            }
                        }
                        #endregion
                        //ADD PARENT TABLE ROW
                        if (dtNew.Rows.Count > 0)
                        {
                            PdfTable myPdfParent = myPdfDocument.NewTable(RowBoxFontBold, dtNew.Rows.Count, dtNew.Columns.Count, 1);
                            myPdfParent.ImportDataTable(dtNew);
                            myPdfParent.SetBorders(Color.Black, 1, BorderType.None);
                            myPdfParent.SetContentAlignment(ContentAlignment.MiddleLeft);
                            myPdfParent.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                            //myPdfParent.SetColumnsWidth(colParentWidths);
                            myPdfParent.SetColumnsWidth(colHeaderWidths);
                            myPdfParent.HeadersRow.SetFont(RowBoxFontBold);
                            myPdfParent.SetFont(HeaderPageTitleFont);
                            myPdfParent.HeadersRow.SetBackgroundColor(Color.White);
                            myPdfParent.HeadersRow.SetForegroundColor(Color.White);
                            for (int i = 0; i < dtNew.Rows.Count; i++)
                            {
                                colActualOridnal = 1;
                                //SET RIGHT ALIGNMENT
                                for (int j = 0; j < strSumColumns.Length; j++)
                                {
                                    myPdfParent.Rows[i][colActualOridnal + j].SetContentAlignment(ContentAlignment.MiddleRight);
                                    myPdfParent.Rows[i][colActualOridnal + j].SetFont(fontGrid);
                                }
                                foreach (PdfCell pcll in myPdfParent.Rows[i].Cells)
                                {
                                    //if (i == sumRowIndex)
                                    //{
                                    //    //SET SUM ROW FONT
                                    //    pcll.SetFont(HeaderPageTitleFont2);
                                    //}
                                    if (pcll.Content.ToString() == "SKIP")
                                    {
                                        pcll.SetBackgroundColor(Color.White);
                                        pcll.SetForegroundColor(Color.White);
                                    }
                                }
                            }
                            //SET BOLD FOR PERCENTAGE
                            colActualOridnal = 1;
                            for (int j = 0; j < strSumColumns.Length; j++)
                            {
                                myPdfParent.Rows[dtNew.Rows.Count - 1][colActualOridnal + j].SetContentAlignment(ContentAlignment.MiddleRight);
                                myPdfParent.Rows[dtNew.Rows.Count - 1][colActualOridnal + j].SetFont(RowFontBold);
                            }
                            while (!myPdfParent.AllTablePagesCreated)
                            {
                                //Setting the Y position and if required creating new page
                                if (currentYPos > myPdfDocument.PageHeight - 50)
                                {
                                    posY = 70;
                                    currentYPos = 70;
                                    newPdfPage.SaveToDocument();
                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                    newPdfPage = myPdfDocument.NewPage();
                                    newPdfPage.Add(myHeaderPdfTablePage);
                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                    posY = currentYPos + 25;
                                    currentYPos = posY + 9 + ((pdfMainHeaderPage.Area.PosY - posY) / 2);
                                    posY = currentYPos;
                                    newPdfPage.Add(pdfMainHeaderPage);
                                    newPdfPage.Add(pdfLineBrk);
                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));
                                }
                                else
                                {
                                    if (posY >= currentYPos)
                                    {
                                        currentYPos = posY + 9 + (pdfMainHeaderPage.Area.PosY - posY) / 2;
                                        posY = currentYPos;
                                    }
                                    else
                                    {
                                        posY = currentYPos;
                                    }
                                }
                                if (myPdfDocument.PageHeight - 50 - posY < 50)
                                {
                                    posY = 70;
                                    currentYPos = 70;
                                    newPdfPage.SaveToDocument();
                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                    newPdfPage = myPdfDocument.NewPage();
                                    newPdfPage.Add(myHeaderPdfTablePage);
                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                    //
                                    posY = currentYPos + 25;
                                    currentYPos = posY + 9 + ((pdfMainHeaderPage.Area.PosY - posY) / 2);
                                    posY = currentYPos;
                                    newPdfPage.Add(pdfMainHeaderPage);
                                    newPdfPage.Add(pdfLineBrk);
                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));
                                }
                                PdfTablePage newPdfTablePage2 = myPdfParent.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                newPdfPage.Add(newPdfTablePage2);
                                currentYPos = newPdfTablePage2.Area.BottomLeftVertex.Y;
                            }
                    #endregion
                            //Child DataTable
                            //CHILD TABLE : Report Category
                            string Child1Link2 = string.Empty;
                            Hashtable htGrandSumTable1 = null;
                            int[] colChild1Widths = null;
                            //CHILD TABLE : Account Details
                            #region Table 1
                            DataTable dtTotals = new DataTable();
                            DataTable dtGrandTotals = new DataTable();
                            if (dtChild1 != null)
                            {
                                string childTrxID = string.Empty;
                                int childRowIndex = 0;
                                string strCustom = string.Empty;
                                string strTotalAmount = string.Empty;
                                double TotalAmount = 0;

                                int colEndBalOrdinal = 0;
                                DataRow[] drChildRow = dtChild1.Select("Link1='" + parentTrxID + "'");
                                DataTable dtFullChild = new DataTable();
                                int[] childIndexes = new int[drChildRow.Length];
                                Hashtable htGrandTotals = new Hashtable();

                                //To store distinct subtotalids
                                int dt2ColumnsCount = 0;
                                if (drChildRow.Length > 0)
                                {
                                    for (int drFoundRws1 = 0; drFoundRws1 < drChildRow.Length; drFoundRws1++)
                                    {
                                        childRowIndex = dt[1].Rows.IndexOf(drChildRow[drFoundRws1]);
                                        DataRow dtChRow2 = dtFullChild.NewRow();

                                        //Calculation items
                                        string strActual = string.Empty;
                                        string strBudget = string.Empty;
                                        string strVariance = string.Empty;
                                        if (dtFullChild.Columns.Count == 0)
                                        {
                                            for (int x = 0; x < dt[1].Columns.Count; x++)
                                            {
                                                dtFullChild.Columns.Add(dt[1].Columns[x].ColumnName);
                                            }
                                            dt2ColumnsCount = dtFullChild.Columns.Count;
                                        }
                                        for (int i = 0; i < dtFullChild.Columns.Count; i++)
                                        {
                                            if (dt[1].Columns.Contains(dtFullChild.Columns[i].ColumnName))
                                            {
                                                dtChRow2[i] = drChildRow[drFoundRws1].ItemArray[i].ToString();
                                            }
                                        }
                                        //Child1Link2 = dt[1].Rows[drFoundRws1]["Link2"].ToString();
                                        Child1Link2 = drChildRow[drFoundRws1]["Link2"].ToString();
                                        dtFullChild.Rows.Add(dtChRow2);
                                        //Delete Row
                                        if (dt[1].Rows.Count > 0)
                                        {
                                            dt[1].Rows[childRowIndex].Delete();
                                            //dt[1].Rows[0].Delete();
                                        }
                                        dtFullChild.AcceptChanges();
                                        dtFullChild.TableName = dt[1].TableName;
                                        //Add Ending Balance from TrxDetail1 Table
                                        #region Table2
                                        if (dtChild2 != null)
                                        {
                                            int RowIndexDetail1 = 0;
                                            DataRow[] drChildRowDetail1 = dtChild2.Select("Link1='" + parentTrxID + "' AND Link2='" + Child1Link2 + "'");
                                            if (drChildRowDetail1.Length > 0)
                                            {
                                                for (int drFoundRws2 = 0; drFoundRws2 < drChildRowDetail1.Length; drFoundRws2++)
                                                {
                                                    RowIndexDetail1 = dt[2].Rows.IndexOf(drChildRowDetail1[drFoundRws2]);
                                                    strActual = drChildRowDetail1[drFoundRws2]["EndBal"].ToString();

                                                    if (drChildRowDetail1 != null)
                                                    {
                                                        if (dtFullChild.Columns.Contains("Actual"))
                                                        {
                                                            dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Actual"] = strActual;
                                                        }
                                                        else
                                                        {
                                                            dtFullChild.Columns.Add("Actual");
                                                            dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Actual"] = strActual;
                                                        }
                                                        if (dt[2].Columns.Contains("EndBal"))
                                                        {
                                                            colEndBalOrdinal = dt[2].Columns["EndBal"].Ordinal;
                                                        }

                                                        if (dt[2].Rows.Count > 0)
                                                        {
                                                            //dt[2].Rows[RowIndexDetail1].Delete();
                                                            //dt[2].Rows[0].Delete();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Table3
                                        //Add Ending Bal from TrxDetail2 Table
                                        if (dtChild3 != null)
                                        {
                                            int RowIndexDetail2 = 0;
                                            //Child2Link3 = drChildRow[drFoundRws]["Link3"].ToString();

                                            DataRow[] drChildRowDetail2 = dtChild3.Select("Link1='" + parentTrxID + "' AND Link2='" + Child1Link2 + "'");

                                            if (drChildRowDetail2.Length > 0)
                                            {
                                                for (int drFoundRws3 = 0; drFoundRws3 < drChildRowDetail2.Length; drFoundRws3++)
                                                {
                                                    RowIndexDetail2 = dt[3].Rows.IndexOf(drChildRowDetail2[drFoundRws3]);
                                                    strBudget = drChildRowDetail2[drFoundRws3]["EndBal"].ToString();
                                                    if (drChildRowDetail2 != null)
                                                    {
                                                        if (dtFullChild.Columns.Contains("Budget"))
                                                        {
                                                            dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Budget"] = strBudget;
                                                        }
                                                        else
                                                        {
                                                            dtFullChild.Columns.Add("Budget");
                                                            dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Budget"] = strBudget;
                                                        }
                                                        //Delete 
                                                        if (dt[3].Rows.Count > 0)
                                                        {
                                                            //dt[3].Rows[RowIndexDetail2].Delete();
                                                            // dt[3].Rows[0].Delete();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Table4 Add Variance column
                                        //Add Variance column
                                        // Variance Amount= Actual Amount - Budget Amount
                                        if ((strActual != string.Empty) && (strBudget != string.Empty))
                                        {
                                            double amtActual = 0;
                                            double amtBudget = 0;
                                            double amtVariance = 0;

                                            amtActual = Convert.ToDouble(strActual);
                                            amtBudget = Convert.ToDouble(strBudget);
                                            amtVariance = amtActual - amtBudget;
                                            strVariance = Convert.ToString(amtVariance);
                                            strVariance = clsReportsUICore.ConvertToCurrencyFormat(strVariance);
                                            if (dtFullChild.Columns.Contains("Variance"))
                                            {
                                                dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Variance"] = strVariance;
                                            }
                                            else
                                            {
                                                dtFullChild.Columns.Add("Variance");
                                                dtFullChild.Rows[dtFullChild.Rows.Count - 1]["Variance"] = strVariance;
                                            }
                                        }
                                        #endregion
                                        //Add Sum Columns End Balance
                                        for (int i = 0; i < strSumColumns.Length; i++)
                                        {
                                            if (dtFullChild.Columns.Contains(strSumColumns[i]))
                                            {
                                                strCustom = string.Empty;
                                                decimal amount;
                                                TotalAmount = 0;
                                                string amt = string.Empty;
                                                string colName = strSumColumns[i];
                                                if (!htGrandTotals.ContainsKey(colName))
                                                {
                                                    amt = clsReportsUICore.ConvertToCurrencyFormat(dtFullChild.Rows[dtFullChild.Rows.Count - 1][colName].ToString());
                                                    htGrandTotals.Add(colName, amt);
                                                }
                                                else
                                                {
                                                    TotalAmount = Convert.ToDouble(htGrandTotals[colName].ToString());
                                                    if (!string.IsNullOrEmpty(dtFullChild.Rows[dtFullChild.Rows.Count - 1][colName].ToString()))
                                                    {
                                                        TotalAmount = TotalAmount + Convert.ToDouble(dtFullChild.Rows[dtFullChild.Rows.Count - 1][colName].ToString());
                                                    }
                                                    strTotalAmount = Convert.ToString(TotalAmount);
                                                    amt = clsReportsUICore.ConvertToCurrencyFormat(strTotalAmount);
                                                    htGrandTotals[colName] = amt;
                                                }
                                            }
                                        }
                                        //
                                        if (childIndexes[0] != 0)
                                        {
                                            childIndexes[drFoundRws1] = childRowIndex + drFoundRws1;
                                        }
                                        else
                                        {
                                            childIndexes[drFoundRws1] = childRowIndex;
                                        }
                                    }
                                }
                                //To set font for sum rows
                                int sumRowIndex = 0;

                                //Add Columns to DtTotal
                                if (dtTotals.Columns.Count == 0)
                                {
                                    for (int x = 0; x < dtFullChild.Columns.Count; x++)
                                    {
                                        dtTotals.Columns.Add(dtFullChild.Columns[x].ColumnName);
                                    }
                                }
                                IDictionaryEnumerator enumTot = htGrandTotals.GetEnumerator();
                                if (htGrandTotals.Keys.Count > 0)
                                {
                                    dtTotals.Rows.Add(dtTotals.NewRow());
                                    //ADDING TOTAL VALUE
                                    dtTotals.Rows[dtTotals.Rows.Count - 1]["AccountName"] = "Total " + dtNew.Rows[0]["Caption"].ToString();
                                    sumRowIndex = dtTotals.Rows.Count - 1;
                                    TotalAmount = 0;
                                    string amt = string.Empty;

                                    //ADD NEW ROW in datatable dtProfits
                                    dtProfits.Rows.Add(dtProfits.NewRow());

                                    while (enumTot.MoveNext())
                                    {
                                        //dtFullChild.Rows[dtFullChild.Rows.Count - 1][enumTot.Key.ToString()] = enumTot.Value.ToString();
                                        dtTotals.Rows[dtTotals.Rows.Count - 1][enumTot.Key.ToString()] = enumTot.Value.ToString();

                                        //Add Revenue Amounts
                                        if (dtNew.Rows[0]["Caption"].ToString().ToUpper() == "REVENUE")
                                        {
                                            if (!htRevenue.ContainsKey(enumTot.Key.ToString()))
                                            {
                                                //ADD
                                                htRevenue.Add(enumTot.Key.ToString(), enumTot.Value.ToString());
                                            }
                                            else
                                            {
                                                //UPDATE
                                                TotalAmount = Convert.ToDouble(htRevenue[enumTot.Key.ToString()].ToString());
                                                TotalAmount = TotalAmount + double.Parse(enumTot.Value.ToString());
                                                strTotalAmount = Convert.ToString(TotalAmount);
                                                amt = clsReportsUICore.ConvertToCurrencyFormat(strTotalAmount);
                                                htRevenue[enumTot.Key.ToString()] = amt;
                                            }
                                        }
                                        //Add Totals to dtProfits
                                        if (dtProfits.Columns.Contains(enumTot.Key.ToString()))
                                        {
                                            //Add value to dtprofits
                                            dtProfits.Rows[dtProfits.Rows.Count - 1][enumTot.Key.ToString()] = enumTot.Value.ToString();
                                        }
                                        else
                                        {
                                            //Add column and value to dtprofits
                                            dtProfits.Columns.Add(enumTot.Key.ToString());
                                            dtProfits.Rows[dtProfits.Rows.Count - 1][enumTot.Key.ToString()] = enumTot.Value.ToString();
                                        }
                                        //
                                    }
                                }
                                int[] colChild2Widths = null;
                                if (dtFullChild.Rows.Count > 0)
                                {
                                    alRemovedColumns = new ArrayList();
                                    //Get ordinals to add arraylist
                                    if (dtFullChild.Columns.Contains("TrxID"))
                                    {
                                        alRemovedColumns.Add(dtFullChild.Columns["TrxID"].Ordinal);
                                    }
                                    if (dtFullChild.Columns.Contains("Link1"))
                                    {
                                        alRemovedColumns.Add(dtFullChild.Columns["Link1"].Ordinal);
                                    }
                                    if (dtFullChild.Columns.Contains("Link2"))
                                    {
                                        alRemovedColumns.Add(dtFullChild.Columns["Link2"].Ordinal);
                                    }

                                    //To Delete columns
                                    if (dtFullChild.Columns.Contains("TrxID"))
                                    {
                                        dtFullChild.Columns.Remove("TrxID");
                                    }
                                    if (dtFullChild.Columns.Contains("Link1"))
                                    {
                                        dtFullChild.Columns.Remove("Link1");
                                    }
                                    if (dtFullChild.Columns.Contains("Link2"))
                                    {
                                        dtFullChild.Columns.Remove("Link2");
                                    }
                                    //Delete columns in dtTotals
                                    if (dtTotals.Columns.Contains("TrxID"))
                                    {
                                        dtTotals.Columns.Remove("TrxID");
                                    }
                                    if (dtTotals.Columns.Contains("Link1"))
                                    {
                                        dtTotals.Columns.Remove("Link1");
                                    }
                                    if (dtTotals.Columns.Contains("Link2"))
                                    {
                                        dtTotals.Columns.Remove("Link2");
                                    }
                                    //Check if the row not contains Ending columns
                                    dt2ColumnsCount = Convert.ToInt32(dt2ColumnsCount - 3) + 3;
                                    if (dtFullChild.Columns.Count < dt2ColumnsCount)
                                    {
                                        for (int i = 0; i < strSumColumns.Length; i++)
                                        {
                                            dtFullChild.Columns.Add(strSumColumns[i]);
                                            dtTotals.Columns.Add(strSumColumns[i]);
                                        }
                                    }
                                    //Remove columns TrxID and Link1 from colwidths
                                    colstart = 0;
                                    colChild2Widths = new int[(arrallcolWidths[1].Length + 3) - 3];
                                    for (int ctr = 0; ctr < arrallcolWidths[1].Length; ctr++)
                                    {
                                        if (!alRemovedColumns.Contains(ctr))
                                        {
                                            colChild2Widths[colstart] = arrallcolWidths[1][ctr];
                                            colstart++;
                                        }
                                    }
                                    //Add Ending Columns Widths
                                    int EndbalWidth = 20;
                                    for (int i = 0; i < 3; i++)
                                    {
                                        if (colEndBalOrdinal == 0)
                                        {
                                            colChild2Widths[colstart] = EndbalWidth;
                                        }
                                        else
                                        {
                                            colChild2Widths[colstart] = arrallcolWidths[2][colEndBalOrdinal];
                                        }
                                        colstart++;
                                    }

                                    if (colEndBalOrdinal == 0)
                                    {
                                        if (dtFullChild.Columns.Contains("Actual"))
                                        {
                                            //set ordeinal for empty columns
                                            colEndBalOrdinal = dtFullChild.Columns["Actual"].Ordinal;
                                        }
                                    }
                                    else
                                    {
                                        if (dtFullChild.Columns.Contains("Actual"))
                                        {
                                            //set ordeinal for empty columns
                                            colEndBalOrdinal = dtFullChild.Columns["Actual"].Ordinal;
                                        }
                                    }
                                    //ADD GrandTotals
                                    //int grandSumRowIndex = 0;
                                    //if (dtGrandTotals.Columns.Count == 0)
                                    //{
                                    //    for (int x = 0; x < dtFullChild.Columns.Count; x++)
                                    //    {
                                    //        dtGrandTotals.Columns.Add(dtFullChild.Columns[x].ColumnName);
                                    //    }
                                    //}
                                }

                                if (dtFullChild.Rows.Count > 0)
                                {
                                    dtFullChild.Columns.Add("Col");
                                    dtFullChild.Columns["Col"].SetOrdinal(0);
                                    //
                                    int[] colModWidths = new int[colHeaderWidths.Length + 1];
                                    for (int wdts = 0; wdts < colModWidths.Length; wdts++)
                                    {
                                        if (wdts == 0)
                                        {
                                            colModWidths[wdts] = 5;
                                        }
                                        else
                                        {
                                            if (wdts == 1)
                                            {
                                                colModWidths[wdts] = colParentWidths[0] - colModWidths[0];
                                            }
                                            else
                                            {
                                                colModWidths[wdts] = colParentWidths[wdts - 1];
                                            }
                                        }
                                    }
                                    //
                                    PdfTable myPdfChild = myPdfDocument.NewTable(FontRegular, dtFullChild.Rows.Count, dtFullChild.Columns.Count, 1);
                                    myPdfChild.ImportDataTable(dtFullChild);
                                    myPdfChild.SetBorders(Color.Black, 1, BorderType.None);
                                    myPdfChild.SetContentAlignment(ContentAlignment.MiddleLeft);
                                    myPdfChild.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                    //myPdfChild.SetColumnsWidth(colChild2Widths);
                                    myPdfChild.SetColumnsWidth(colModWidths);
                                    myPdfChild.HeadersRow.SetFont(FontRegular);
                                    myPdfChild.HeadersRow.SetBackgroundColor(Color.White);
                                    myPdfChild.HeadersRow.SetForegroundColor(Color.White);
                                    //
                                    colEndBalOrdinal = 2;
                                    //
                                    for (int i = 0; i < dtFullChild.Rows.Count; i++)
                                    {
                                        //SET RIGHT ALIGNMENT
                                        for (int j = 0; j < strSumColumns.Length; j++)
                                        {
                                            if (j < (colEndBalOrdinal + j))
                                            {
                                                myPdfChild.Rows[i][colEndBalOrdinal + j].SetContentAlignment(ContentAlignment.MiddleRight);
                                            }
                                        }
                                        foreach (PdfCell pcll in myPdfChild.Rows[i].Cells)
                                        {
                                            if (pcll.Content.ToString() == "SKIP")
                                            {
                                                pcll.SetBackgroundColor(Color.White);
                                                pcll.SetForegroundColor(Color.White);
                                            }
                                        }
                                    }
                                    for (int i = 0; i < dtFullChild.Rows.Count; i++)
                                    {
                                        //SET RIGHT ALIGNMENT
                                        for (int j = 0; j < strSumColumns.Length; j++)
                                        {
                                            myPdfChild.Rows[i][colEndBalOrdinal + j].SetContentAlignment(ContentAlignment.MiddleRight);
                                        }
                                        foreach (PdfCell pcll in myPdfChild.Rows[i].Cells)
                                        {
                                            if (pcll.Content.ToString() == "SKIP")
                                            {
                                                pcll.SetBackgroundColor(Color.White);
                                                pcll.SetForegroundColor(Color.White);
                                            }
                                        }
                                    }
                                    colEndBalOrdinal = 1;
                                    while (!myPdfChild.AllTablePagesCreated)
                                    {
                                        //Setting the Y position and if required creating new page
                                        if (currentYPos > myPdfDocument.PageHeight - 50)
                                        {
                                            posY = 70;
                                            currentYPos = 70;
                                            newPdfPage.SaveToDocument();
                                            //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                            newPdfPage = myPdfDocument.NewPage();
                                            newPdfPage.Add(myHeaderPdfTablePage);
                                            if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                            posY = currentYPos + 25;
                                            currentYPos = posY + 9 + ((pdfMainHeaderPage.Area.PosY - posY) / 2);
                                            posY = currentYPos;
                                            newPdfPage.Add(pdfMainHeaderPage);
                                            newPdfPage.Add(pdfLineBrk);
                                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));
                                        }
                                        else
                                        {
                                            if (posY >= currentYPos)
                                            {
                                                currentYPos = posY + 9 + (pdfMainHeaderPage.Area.PosY - posY) / 2;
                                                posY = currentYPos;
                                            }
                                            else
                                            {
                                                posY = currentYPos;
                                            }
                                        }
                                        if (myPdfDocument.PageHeight - 50 - posY < 50)
                                        {
                                            posY = 70;
                                            currentYPos = 70;
                                            newPdfPage.SaveToDocument();
                                            //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                            newPdfPage = myPdfDocument.NewPage();
                                            newPdfPage.Add(myHeaderPdfTablePage);
                                            if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                            //
                                            posY = currentYPos + 25;
                                            currentYPos = posY + 9 + ((pdfMainHeaderPage.Area.PosY - posY) / 2);
                                            posY = currentYPos;
                                            newPdfPage.Add(pdfMainHeaderPage);
                                            newPdfPage.Add(pdfLineBrk);
                                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));
                                        }
                                        PdfTablePage newPdfTablePage2 = myPdfChild.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                        newPdfPage.Add(newPdfTablePage2);
                                        currentYPos = newPdfTablePage2.Area.BottomLeftVertex.Y;
                                    }
                                }
                                //Total and grand totals starts
                                #region Table Totals
                                if (dtTotals.Rows.Count > 0)
                                {
                                    dtTotals.Columns.Add("Col");
                                    dtTotals.Columns["Col"].SetOrdinal(0);
                                    //
                                    int[] colModWidths = new int[colHeaderWidths.Length + 1];
                                    for (int wdts = 0; wdts < colModWidths.Length; wdts++)
                                    {
                                        if (wdts == 0)
                                        {
                                            colModWidths[wdts] = 5;
                                        }
                                        else
                                        {
                                            if (wdts == 1)
                                            {
                                                colModWidths[wdts] = colParentWidths[0] - colModWidths[0];
                                            }
                                            else
                                            {
                                                colModWidths[wdts] = colParentWidths[wdts - 1];
                                            }
                                        }
                                    }
                                    PdfTable myPdfChildTotals = myPdfDocument.NewTable(FontRegular, dtTotals.Rows.Count, dtTotals.Columns.Count, 1);
                                    myPdfChildTotals.ImportDataTable(dtTotals);
                                    myPdfChildTotals.SetBorders(Color.Black, 1, BorderType.None);
                                    myPdfChildTotals.SetContentAlignment(ContentAlignment.MiddleLeft);
                                    myPdfChildTotals.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                    //myPdfChildTotals.SetColumnsWidth(colChild2Widths);
                                    myPdfChildTotals.SetColumnsWidth(colModWidths);
                                    myPdfChildTotals.HeadersRow.SetFont(FontRegular);
                                    myPdfChildTotals.HeadersRow.SetBackgroundColor(Color.White);
                                    myPdfChildTotals.HeadersRow.SetForegroundColor(Color.White);
                                    //for (int i = 0; i < dtTotals.Rows.Count; i++)
                                    //{
                                    //    //SET RIGHT ALIGNMENT
                                    //    for (int j = 0; j < strSumColumns.Length; j++)
                                    //    {
                                    //        myPdfChildTotals.Rows[i][colEndBalOrdinal + j].SetContentAlignment(ContentAlignment.MiddleRight);
                                    //    }
                                    //    foreach (PdfCell pcll in myPdfChildTotals.Rows[i].Cells)
                                    //    {
                                    //        if (pcll.Content.ToString() == "SKIP")
                                    //        {
                                    //            pcll.SetBackgroundColor(Color.White);
                                    //            pcll.SetForegroundColor(Color.White);
                                    //        }
                                    //    }
                                    //}
                                    //
                                    //
                                    colEndBalOrdinal = 2;
                                    //
                                    for (int i = 0; i < dtTotals.Rows.Count; i++)
                                    {
                                        //SET RIGHT ALIGNMENT
                                        for (int j = 0; j < strSumColumns.Length; j++)
                                        {
                                            if (j < (colEndBalOrdinal + j))
                                            {
                                                myPdfChildTotals.Rows[i][colEndBalOrdinal + j].SetContentAlignment(ContentAlignment.MiddleRight);
                                            }
                                        }
                                        foreach (PdfCell pcll in myPdfChildTotals.Rows[i].Cells)
                                        {
                                            if (pcll.Content.ToString() == "SKIP")
                                            {
                                                pcll.SetBackgroundColor(Color.White);
                                                pcll.SetForegroundColor(Color.White);
                                            }
                                        }
                                    }
                                    for (int i = 0; i < dtTotals.Rows.Count; i++)
                                    {
                                        //SET RIGHT ALIGNMENT
                                        for (int j = 0; j < strSumColumns.Length; j++)
                                        {
                                            myPdfChildTotals.Rows[i][colEndBalOrdinal + j].SetContentAlignment(ContentAlignment.MiddleRight);
                                        }
                                        foreach (PdfCell pcll in myPdfChildTotals.Rows[i].Cells)
                                        {
                                            if (pcll.Content.ToString() == "SKIP")
                                            {
                                                pcll.SetBackgroundColor(Color.White);
                                                pcll.SetForegroundColor(Color.White);
                                            }
                                        }
                                    }
                                    colEndBalOrdinal = 1;
                                    //
                                    while (!myPdfChildTotals.AllTablePagesCreated)
                                    {
                                        //Setting the Y position and if required creating new page
                                        if (currentYPos > myPdfDocument.PageHeight - 50)
                                        {
                                            posY = 70;
                                            currentYPos = 70;
                                            newPdfPage.SaveToDocument();
                                            //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                            newPdfPage = myPdfDocument.NewPage();
                                            newPdfPage.Add(myHeaderPdfTablePage);
                                            if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                        }
                                        else
                                        {
                                            posY = currentYPos;
                                        }
                                        if (myPdfDocument.PageHeight - 50 - posY < 50)
                                        {
                                            posY = 70;
                                            currentYPos = 70;
                                            newPdfPage.SaveToDocument();
                                            //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                            newPdfPage = myPdfDocument.NewPage();
                                            newPdfPage.Add(myHeaderPdfTablePage);
                                            if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                        }
                                        PdfTablePage newPdfTablePage2 = myPdfChildTotals.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                        newPdfPage.Add(newPdfTablePage2);
                                        currentYPos = newPdfTablePage2.Area.BottomLeftVertex.Y;
                                    }
                                }
                                #endregion
                            } //dt[1]2 close
                            #endregion
                        }//dtnew close
                    }//Table 0  parent for loop end
                } //dt if condtion
                newPdfPage.SaveToDocument();
                CreatePDFDocument(fileName);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }
        #endregion


        #region Report Style 653
        public void ReportStyle653(DataTable[] dt, DataTable dtHeader, Hashtable[] arrhtFormatModes, bool PLayout, int[][] arrallcolWidths, Hashtable[] ArrhtColNameValues, string fileName)
        {
            try
            {
                if ((bool)PLayout)
                {
                    myPdfDocument = new PdfDocument(PdfDocumentFormat.A4_Horizontal);
                }
                else
                {
                    myPdfDocument = new PdfDocument(PdfDocumentFormat.A4);
                    pgHeight = 50;
                    pgWidth = 200;
                }
                string imgpath = PDFImagePath();
                PdfImage LogoImage = null;
                if (!string.IsNullOrEmpty(imgpath))
                {
                    LogoImage = myPdfDocument.NewImage(imgpath);
                }
                newPdfPage = GetHeaderPDFTableStyle1(dtHeader, out myHeaderPdfTablePage);
                ArrayList alRemovedColumns = null;
                int dt0Rows = dt[0].Rows.Count;
                Font fontMedium = new Font("Verdana", 8, FontStyle.Bold);
                Font fontSumTotals = new Font("Verdana", 8, FontStyle.Bold);
                Font fontGrid = new Font("Verdana", 8, FontStyle.Regular);
                PdfTablePage pdfMainHeaderPage = null;
                PdfLine pdfLineBrk = null;
                int FirstQuarterfullview = 19;
                int[] colHeaderWidths = null;
                //



                #region PDF Header
                DataTable dtFormattedHeader = new DataTable();
                dtFormattedHeader.Columns.Add("Caption");
                /*dtFormattedHeader.Columns.Add("First Quarter");
                dtFormattedHeader.Columns.Add("Second Quarter");
                dtFormattedHeader.Columns.Add("Third Quarter");
                dtFormattedHeader.Columns.Add("Fourth Quarter");
                dtFormattedHeader.Columns.Add("Total Year");*/

                dtFormattedHeader.Columns.Add("First Qtr");
                dtFormattedHeader.Columns.Add("Second Qtr");
                dtFormattedHeader.Columns.Add("Third Qtr");
                dtFormattedHeader.Columns.Add("Fourth Qtr");
                dtFormattedHeader.Columns.Add("Total Year");


                dtFormattedHeader.Rows.Add(dtFormattedHeader.NewRow());
                for (int col = 0; col < dtFormattedHeader.Columns.Count; col++)
                {
                    //if (col == 0)
                    //{
                    //    dtFormattedHeader.Rows[dtFormattedHeader.Rows.Count - 1][dtFormattedHeader.Columns[col].ToString()] = "SKIP";
                    //}
                    //else if (col == dtFormattedHeader.Columns.Count-1)
                    //{
                    //    dtFormattedHeader.Rows[dtFormattedHeader.Rows.Count - 1][dtFormattedHeader.Columns[col].ToString()] = "TOTAL";
                    //}
                    //else
                    //{
                    //    dtFormattedHeader.Rows[dtFormattedHeader.Rows.Count - 1][dtFormattedHeader.Columns[col].ToString()] = "QUARTER";
                    //}

                    dtFormattedHeader.Rows[dtFormattedHeader.Rows.Count - 1][dtFormattedHeader.Columns[col].ToString()] = "SKIP";
                }

                colHeaderWidths = new int[dtFormattedHeader.Columns.Count];
                int colcnt = 0;
                for (int i = 0; i < colHeaderWidths.Length; i++)
                {
                    if (i == 0)
                    {
                        //Caption width
                        colHeaderWidths[colcnt] = arrallcolWidths[0][1];
                    }
                    else
                    {
                        colHeaderWidths[colcnt] = FirstQuarterfullview;
                    }
                    colcnt++;
                }


                PdfTable pdfMainHeader = myPdfDocument.NewTable(FontRegular, dtFormattedHeader.Rows.Count, dtFormattedHeader.Columns.Count, 1);
                pdfMainHeader.ImportDataTable(dtFormattedHeader);
                pdfMainHeader.SetBorders(Color.Black, 1, BorderType.None);
                pdfMainHeader.SetContentAlignment(ContentAlignment.MiddleRight);
                pdfMainHeader.HeadersRow.SetContentAlignment(ContentAlignment.MiddleRight);
                pdfMainHeader.SetColumnsWidth(colHeaderWidths);
                pdfMainHeader.HeadersRow.SetFont(HeaderFont);
                //pdfMainHeader.HeadersRow.SetFont(RowBoxFontBold);

                //
                foreach (PdfCell pHeaderCll in pdfMainHeader.HeadersRow.Cells)
                {
                    string labelName = pHeaderCll.Content.ToString();
                    switch (labelName)
                    {
                        case "Caption":
                            {
                                pHeaderCll.SetBackgroundColor(Color.White);
                                pHeaderCll.SetForegroundColor(Color.White);
                                break;
                            }
                        //case "FIRST QUARTER":
                        //case "SECOND QUARTER":
                        //case "THIRD QUARTER":
                        //case "FOURTH QUARTER":
                        //case "TOTAL YEAR":
                        default:
                            {
                                pHeaderCll.SetContentAlignment(ContentAlignment.MiddleCenter);
                                break;
                            }
                    }
                }
                foreach (PdfCell pskipCll in pdfMainHeader.Cells)
                {
                    string labelValue = pskipCll.Content.ToString();
                    switch (labelValue)
                    {
                        case "SKIP":
                            {
                                pskipCll.SetBackgroundColor(Color.White);
                                pskipCll.SetForegroundColor(Color.White);
                                break;
                            }
                    }
                }
                //
                posY = currentYPos + 25;
                currentYPos = posY;
                //
                pdfMainHeaderPage = pdfMainHeader.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, (height - (posY))));
                newPdfPage.Add(pdfMainHeaderPage);
                //
                double xPos = pdfMainHeaderPage.CellArea(pdfMainHeader.HeadersRow.Index, 0).TopLeftVertex.X;
                double yPos = pdfMainHeaderPage.CellArea(pdfMainHeader.HeadersRow.Index, 0).TopLeftVertex.Y;
                //
                Point pStart = new Point(Convert.ToInt32(xPos), Convert.ToInt32(yPos));
                Point pEnd = new Point(Convert.ToInt32(xPos + (myPdfDocument.PageWidth - 50)), Convert.ToInt32(yPos));
                pdfLineBrk = new PdfLine(myPdfDocument, pStart, pEnd, Color.Black, 1);
                newPdfPage.Add(pdfLineBrk);
                #endregion

                DataTable dtChild1 = dt[1];
                DataTable dtChild2 = dt[2];
                DataTable dtChild3 = dt[3];
                DataTable dtChild4 = dt[4];
                DataTable dtChild5 = dt[5];


                //Calculations
                DataTable dtProfits = new DataTable();
                Hashtable htRevenue = new Hashtable();
                Hashtable htGrossProfit = new Hashtable();
                Hashtable htNetIncomeBeforeTaxes = new Hashtable();



                //PARENT TABLE : Report Detail
                if (dt[0].Rows.Count > 0)
                {
                    string Child2Link3 = string.Empty;
                    string[] strSumColumns ={ "FirstQuarter", "SecondQuarter", "ThirdQuarter", "FourthQuarter", "TotalYear" };
                    #region Table0 : Report Detail
                    for (int dtCnt = 0; dtCnt < dt0Rows; dtCnt++)
                    {
                        DataTable pDT = new DataTable();
                        DataTable dtNew = new DataTable();
                        DataRow[] foundRows = null;
                        string parentTrxID = string.Empty;
                        foundRows = dt[0].Select("Link1 ='" + dt[0].Rows[0]["Link1"].ToString() + "'");
                        if (foundRows.Length > 0)
                        {
                            int rowIndex = dt[0].Rows.IndexOf(foundRows[0]);
                            DataRow dt2Row = dtNew.NewRow();
                            if (dtNew.Columns.Count == 0)
                            {
                                for (int x = 0; x < dt[0].Columns.Count; x++)
                                {
                                    dtNew.Columns.Add(dt[0].Columns[x].ColumnName);
                                }
                            }
                            for (int i = 0; i < dtNew.Columns.Count; i++)
                            {
                                dt2Row[i] = foundRows[0].ItemArray[i].ToString();
                            }
                            dtNew.Rows.Add(dt2Row);
                            parentTrxID = dt[0].Rows[0]["Link1"].ToString();
                            if (dt[0].Rows.Count > 0)
                            {
                                dt[0].Rows[0].Delete();
                            }
                            dtNew.AcceptChanges();
                            dtNew.TableName = dt[0].TableName;
                        }
                        alRemovedColumns = new ArrayList();
                        //To Get ordinals
                        if (dtNew.Columns.Contains("TrxID"))
                        {
                            alRemovedColumns.Add(dtNew.Columns["TrxID"].Ordinal);
                        }
                        if (dtNew.Columns.Contains("Link1"))
                        {
                            alRemovedColumns.Add(dtNew.Columns["Link1"].Ordinal);
                        }
                        int colstart = 0;
                        //Remove columns TrxID and Link1 from table
                        int[] colParentWidths = new int[(arrallcolWidths[0].Length - 2) + 5];

                        for (int ctr = 0; ctr < arrallcolWidths[0].Length; ctr++)
                        {
                            if (!alRemovedColumns.Contains(ctr))
                            {
                                colParentWidths[colstart] = arrallcolWidths[0][ctr];
                                colstart++;
                            }
                        }
                        //ADD Profit columns
                        for (int i = 0; i < strSumColumns.Length; i++)
                        {
                            if (!dtNew.Columns.Contains(strSumColumns[i].ToString()))
                            {
                                dtNew.Columns.Add(strSumColumns[i].ToString());
                                colParentWidths[colstart] = FirstQuarterfullview;
                                colstart++;
                            }
                        }
                        //set FirstQuarter oridnal
                        int colFirstQuarterOridnal = 0;
                        if (dtNew.Columns.Contains(strSumColumns[0].ToString()))
                        {
                            colFirstQuarterOridnal = dtNew.Columns[strSumColumns[0].ToString()].Ordinal;
                        }
                        #region Profit Calcuations
                        if (dtNew.Rows.Count > 0)
                        {
                            double amtprofit1 = 0;
                            double amtprofit2 = 0;
                            double amtdiff = 0;
                            string strAmount = string.Empty;
                            string strFinalamt = string.Empty;
                            string strCaption = dtNew.Rows[dtNew.Rows.Count - 1]["Link1"].ToString().ToUpper();
                            switch (strCaption)
                            {
                                case "30":
                                    {
                                        // Gross Profit = Revenue - Production
                                        //ADD NEW ROW for profit percentage
                                        dtNew.Rows.Add(dtNew.NewRow());
                                        for (int i = 0; i < strSumColumns.Length; i++)
                                        {
                                            if (dtProfits != null)
                                            {
                                                amtprofit1 = 0;
                                                amtprofit2 = 0;
                                                if (dtProfits.Rows[0] != null)
                                                {
                                                    amtprofit1 = Convert.ToDouble(dtProfits.Rows[0][strSumColumns[i].ToString()]);
                                                }
                                                if (dtProfits.Rows.Count == 2)
                                                {
                                                    if (dtProfits.Rows[1] != null)
                                                    {
                                                        amtprofit2 = Convert.ToDouble(dtProfits.Rows[1][strSumColumns[i].ToString()]);
                                                    }
                                                }
                                                amtdiff = amtprofit1 - amtprofit2;
                                                strAmount = Convert.ToString(amtdiff);
                                                strAmount = clsReportsUICore.ConvertToCurrencyFormat(strAmount);
                                                //Gross profit
                                                dtNew.Rows[dtNew.Rows.Count - 2][strSumColumns[i].ToString()] = strAmount;
                                                //Add Gross profit to hashtable
                                                if (htGrossProfit.Contains(strSumColumns[i].ToString()))
                                                {
                                                    //UPDATE
                                                    htGrossProfit[strSumColumns[i].ToString()] = strAmount;
                                                }
                                                else
                                                {
                                                    //ADD
                                                    htGrossProfit.Add(strSumColumns[i].ToString(), strAmount);
                                                }

                                                //Gross pfofit in percentage  
                                                if (htRevenue.Contains(strSumColumns[i].ToString()))
                                                {
                                                    //Grossprofit / Revenue
                                                    amtprofit1 = Convert.ToDouble(strAmount);
                                                    amtprofit2 = Convert.ToDouble(htRevenue[strSumColumns[i].ToString()].ToString());
                                                    amtdiff = 0;
                                                    if (amtprofit2 != 0)
                                                    {
                                                        amtdiff = (amtprofit1 / amtprofit2) * 100;
                                                    }
                                                    strAmount = Convert.ToString(amtdiff);
                                                    strAmount = clsReportsUICore.ConvertToCurrencyFormat(strAmount) + "%";
                                                    dtNew.Rows[dtNew.Rows.Count - 1][strSumColumns[i].ToString()] = strAmount;
                                                }
                                            }
                                        }
                                        //Clear the dtProfits table
                                        dtProfits.Clear();
                                        break;
                                    }
                                case "60":
                                    {
                                        // NetIncoeBeforeTaxes =  GrossProft - OtherCost + OtherIncome
                                        //ADD NEW ROW for profit percentage
                                        dtNew.Rows.Add(dtNew.NewRow());
                                        for (int i = 0; i < strSumColumns.Length; i++)
                                        {
                                            if (dtProfits != null)
                                            {
                                                amtprofit1 = 0;
                                                amtprofit2 = 0;
                                                if (dtProfits.Rows[0] != null)
                                                {
                                                    amtprofit1 = Convert.ToDouble(dtProfits.Rows[0][strSumColumns[i].ToString()]);
                                                }
                                                if (dtProfits.Rows.Count == 2)
                                                {
                                                    if (dtProfits.Rows[1] != null)
                                                    {
                                                        amtprofit2 = Convert.ToDouble(dtProfits.Rows[1][strSumColumns[i].ToString()]);
                                                    }
                                                }
                                                //Get grossprofit amount
                                                if (htGrossProfit.Contains(strSumColumns[i].ToString()))
                                                {
                                                    //here amtdiff is grossprofit
                                                    amtdiff = Convert.ToDouble(htGrossProfit[strSumColumns[i].ToString()].ToString());
                                                    amtdiff = amtdiff - amtprofit1 + amtprofit2;
                                                }
                                                strAmount = Convert.ToString(amtdiff);
                                                strAmount = clsReportsUICore.ConvertToCurrencyFormat(strAmount);
                                                //Net Income Before Taxes
                                                dtNew.Rows[dtNew.Rows.Count - 2][strSumColumns[i].ToString()] = strAmount;

                                                //Add NetIncomeBeforeTaxes to hashtable
                                                if (htNetIncomeBeforeTaxes.Contains(strSumColumns[i].ToString()))
                                                {
                                                    //UPDATE
                                                    htNetIncomeBeforeTaxes[strSumColumns[i].ToString()] = strAmount;
                                                }
                                                else
                                                {
                                                    //ADD
                                                    htNetIncomeBeforeTaxes.Add(strSumColumns[i].ToString(), strAmount);
                                                }
                                                //Net Income in percentage  
                                                if (htRevenue.Contains(strSumColumns[i].ToString()))
                                                {
                                                    //Net Income / Revenue
                                                    amtprofit1 = Convert.ToDouble(strAmount);
                                                    amtprofit2 = Convert.ToDouble(htRevenue[strSumColumns[i].ToString()].ToString());
                                                    amtdiff = 0;
                                                    if (amtprofit2 != 0)
                                                    {
                                                        amtdiff = (amtprofit1 / amtprofit2) * 100;
                                                    }
                                                    strAmount = Convert.ToString(amtdiff);
                                                    strAmount = clsReportsUICore.ConvertToCurrencyFormat(strAmount) + "%";
                                                    dtNew.Rows[dtNew.Rows.Count - 1][strSumColumns[i].ToString()] = strAmount;
                                                }
                                            }
                                        }
                                        //Clear the dtProfits table
                                        dtProfits.Clear();
                                        break;
                                    }
                                case "900":
                                    {
                                        // NetIncome = NetIncomeBeforeTaxes - TaxTotal
                                        //ADD NEW ROW for profit percentage
                                        dtNew.Rows.Add(dtNew.NewRow());
                                        for (int i = 0; i < strSumColumns.Length; i++)
                                        {
                                            if (dtProfits != null)
                                            {
                                                amtprofit1 = 0;
                                                amtprofit2 = 0;

                                                if (htNetIncomeBeforeTaxes.Contains(strSumColumns[i].ToString()))
                                                {
                                                    amtprofit1 = Convert.ToDouble(htNetIncomeBeforeTaxes[strSumColumns[i].ToString()].ToString());
                                                }
                                                if (dtProfits.Rows[0] != null)
                                                {
                                                    amtprofit2 = Convert.ToDouble(dtProfits.Rows[0][strSumColumns[i].ToString()]);
                                                }

                                                amtdiff = amtprofit1 - amtprofit2;
                                                strAmount = Convert.ToString(amtdiff);
                                                strAmount = clsReportsUICore.ConvertToCurrencyFormat(strAmount);
                                                //Net Income
                                                dtNew.Rows[dtNew.Rows.Count - 2][strSumColumns[i].ToString()] = strAmount;

                                                //Net Income in percentage  
                                                if (htRevenue.Contains(strSumColumns[i].ToString()))
                                                {
                                                    //Grossprofit / Revenue
                                                    amtprofit1 = Convert.ToDouble(strAmount);
                                                    amtprofit2 = Convert.ToDouble(htRevenue[strSumColumns[i].ToString()].ToString());
                                                    amtdiff = 0;
                                                    if (amtprofit2 != 0)
                                                    {
                                                        amtdiff = (amtprofit1 / amtprofit2) * 100;
                                                    }
                                                    strAmount = Convert.ToString(amtdiff);
                                                    strAmount = clsReportsUICore.ConvertToCurrencyFormat(strAmount) + "%";
                                                    dtNew.Rows[dtNew.Rows.Count - 1][strSumColumns[i].ToString()] = strAmount;
                                                }
                                            }
                                        }
                                        //Clear the dtProfits table
                                        dtProfits.Clear();
                                        break;
                                    }
                                default:
                                    break;
                            }
                            //To Delete
                            if (dtNew.Columns.Contains("TrxID"))
                            {
                                dtNew.Columns.Remove("TrxID");
                            }
                            if (dtNew.Columns.Contains("Link1"))
                            {
                                dtNew.Columns.Remove("Link1");
                            }
                        }
                        #endregion
                        //ADD PARENT TABLE ROW
                        if (dtNew.Rows.Count > 0)
                        {
                            PdfTable myPdfParent = myPdfDocument.NewTable(RowBoxFontBold, dtNew.Rows.Count, dtNew.Columns.Count, 1);
                            myPdfParent.ImportDataTable(dtNew);
                            myPdfParent.SetBorders(Color.Black, 1, BorderType.None);
                            myPdfParent.SetContentAlignment(ContentAlignment.MiddleLeft);
                            myPdfParent.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                            myPdfParent.SetColumnsWidth(colParentWidths);
                            myPdfParent.HeadersRow.SetFont(RowBoxFontBold);
                            myPdfParent.SetFont(HeaderPageTitleFont);
                            myPdfParent.HeadersRow.SetBackgroundColor(Color.White);
                            myPdfParent.HeadersRow.SetForegroundColor(Color.White);
                            for (int i = 0; i < dtNew.Rows.Count; i++)
                            {
                                colFirstQuarterOridnal = 1;
                                //SET RIGHT ALIGNMENT
                                for (int j = 0; j < strSumColumns.Length; j++)
                                {
                                    myPdfParent.Rows[i][colFirstQuarterOridnal + j].SetContentAlignment(ContentAlignment.MiddleRight);
                                    myPdfParent.Rows[i][colFirstQuarterOridnal + j].SetFont(fontGrid);

                                }
                                foreach (PdfCell pcll in myPdfParent.Rows[i].Cells)
                                {
                                    //if (i == sumRowIndex)
                                    //{
                                    //    //SET SUM ROW FONT
                                    //    pcll.SetFont(HeaderPageTitleFont2);
                                    //}
                                    if (pcll.Content.ToString() == "SKIP")
                                    {
                                        pcll.SetBackgroundColor(Color.White);
                                        pcll.SetForegroundColor(Color.White);
                                    }
                                }
                            }
                            //SET BOLD FOR PERCENTAGE
                            colFirstQuarterOridnal = 1;
                            for (int j = 0; j < strSumColumns.Length; j++)
                            {
                                myPdfParent.Rows[dtNew.Rows.Count - 1][colFirstQuarterOridnal + j].SetContentAlignment(ContentAlignment.MiddleRight);
                                myPdfParent.Rows[dtNew.Rows.Count - 1][colFirstQuarterOridnal + j].SetFont(RowFontBold);
                            }
                            while (!myPdfParent.AllTablePagesCreated)
                            {
                                //Setting the Y position and if required creating new page
                                if (currentYPos > myPdfDocument.PageHeight - 50)
                                {
                                    posY = 70;
                                    currentYPos = 70;
                                    newPdfPage.SaveToDocument();
                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                    newPdfPage = myPdfDocument.NewPage();
                                    newPdfPage.Add(myHeaderPdfTablePage);
                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                    posY = currentYPos + 25;
                                    currentYPos = posY + 9 + ((pdfMainHeaderPage.Area.PosY - posY) / 2);
                                    posY = currentYPos;
                                    newPdfPage.Add(pdfMainHeaderPage);
                                    newPdfPage.Add(pdfLineBrk);
                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));
                                }
                                else
                                {
                                    if (posY >= currentYPos)
                                    {
                                        currentYPos = posY + 9 + (pdfMainHeaderPage.Area.PosY - posY) / 2;
                                        posY = currentYPos;
                                    }
                                    else
                                    {
                                        posY = currentYPos;
                                    }
                                }
                                if (myPdfDocument.PageHeight - 50 - posY < 50)
                                {
                                    posY = 70;
                                    currentYPos = 70;
                                    newPdfPage.SaveToDocument();
                                    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                    newPdfPage = myPdfDocument.NewPage();
                                    newPdfPage.Add(myHeaderPdfTablePage);
                                    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                    //
                                    posY = currentYPos + 25;
                                    currentYPos = posY + 9 + ((pdfMainHeaderPage.Area.PosY - posY) / 2);
                                    posY = currentYPos;
                                    newPdfPage.Add(pdfMainHeaderPage);
                                    newPdfPage.Add(pdfLineBrk);
                                    newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));
                                }
                                PdfTablePage newPdfTablePage2 = myPdfParent.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                newPdfPage.Add(newPdfTablePage2);
                                currentYPos = newPdfTablePage2.Area.BottomLeftVertex.Y;
                            }
                    #endregion
                            //Child DataTable
                            //CHILD TABLE : Report Category
                            string Child1Link2 = string.Empty;
                            Hashtable htGrandSumTable1 = null;
                            int[] colChild1Widths = null;
                            //CHILD TABLE : Account Details
                            #region Table 1 : AccountDetail
                            DataTable dtTotals = new DataTable();
                            DataTable dtGrandTotals = new DataTable();
                            if (dtChild1 != null)
                            {
                                string childTrxID = string.Empty;
                                int childRowIndex = 0;
                                string strCustom = string.Empty;
                                string strTotalAmount = string.Empty;
                                double TotalAmount = 0;

                                int colEndBalOrdinal = 0;
                                DataRow[] drChildRow = dtChild1.Select("Link1='" + parentTrxID + "'");
                                DataTable dtFullChild = new DataTable();
                                int[] childIndexes = new int[drChildRow.Length];
                                Hashtable htGrandTotals = new Hashtable();
                                //To store distinct subtotalids
                                int dt2ColumnsCount = 0;
                                if (drChildRow.Length > 0)
                                {
                                    for (int drFoundRws1 = 0; drFoundRws1 < drChildRow.Length; drFoundRws1++)
                                    {
                                        childRowIndex = dt[1].Rows.IndexOf(drChildRow[drFoundRws1]);
                                        DataRow dtChRow2 = dtFullChild.NewRow();

                                        //Calculation items

                                        string strFirstQuarter = string.Empty;
                                        string strSecondQuarter = string.Empty;
                                        string strThirdQuarter = string.Empty;
                                        string strFourthQuarter = string.Empty;


                                        string strTotalYear = string.Empty;


                                        if (dtFullChild.Columns.Count == 0)
                                        {
                                            for (int x = 0; x < dt[1].Columns.Count; x++)
                                            {
                                                dtFullChild.Columns.Add(dt[1].Columns[x].ColumnName);
                                            }
                                            dt2ColumnsCount = dtFullChild.Columns.Count;
                                        }
                                        for (int i = 0; i < dtFullChild.Columns.Count; i++)
                                        {
                                            if (dt[1].Columns.Contains(dtFullChild.Columns[i].ColumnName))
                                            {
                                                dtChRow2[i] = drChildRow[drFoundRws1].ItemArray[i].ToString();
                                            }
                                        }
                                        //Child1Link2 = dt[1].Rows[drFoundRws1]["Link2"].ToString();
                                        Child1Link2 = drChildRow[drFoundRws1]["Link2"].ToString();
                                        dtFullChild.Rows.Add(dtChRow2);
                                        //Delete Row
                                        if (dt[1].Rows.Count > 0)
                                        {
                                            dt[1].Rows[childRowIndex].Delete();
                                            //dt[1].Rows[0].Delete();
                                        }
                                        dtFullChild.AcceptChanges();
                                        dtFullChild.TableName = dt[1].TableName;
                                        //Add Ending Balance from TrxDetail1 Table
                                        #region Table2 :TrxDetail1
                                        if (dtChild2 != null)
                                        {
                                            int RowIndexDetail1 = 0;
                                            DataRow[] drChildRowDetail1 = dtChild2.Select("Link1='" + parentTrxID + "' AND Link2='" + Child1Link2 + "'");
                                            if (drChildRowDetail1.Length > 0)
                                            {
                                                for (int drFoundRws2 = 0; drFoundRws2 < drChildRowDetail1.Length; drFoundRws2++)
                                                {
                                                    RowIndexDetail1 = dt[2].Rows.IndexOf(drChildRowDetail1[drFoundRws2]);
                                                    strFirstQuarter = drChildRowDetail1[drFoundRws2]["EndBal"].ToString();

                                                    if (drChildRowDetail1 != null)
                                                    {
                                                        if (dtFullChild.Columns.Contains("FirstQuarter"))
                                                        {
                                                            dtFullChild.Rows[dtFullChild.Rows.Count - 1]["FirstQuarter"] = strFirstQuarter;
                                                        }
                                                        else
                                                        {
                                                            dtFullChild.Columns.Add("FirstQuarter");
                                                            dtFullChild.Rows[dtFullChild.Rows.Count - 1]["FirstQuarter"] = strFirstQuarter;
                                                        }
                                                        if (dt[2].Columns.Contains("EndBal"))
                                                        {
                                                            colEndBalOrdinal = dt[2].Columns["EndBal"].Ordinal;
                                                        }

                                                        if (dt[2].Rows.Count > 0)
                                                        {
                                                            //dt[2].Rows[RowIndexDetail1].Delete();
                                                            //dt[2].Rows[0].Delete();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Table3 : TrxDetail2
                                        //Add Ending Bal from TrxDetail2 Table
                                        if (dtChild3 != null)
                                        {
                                            int RowIndexDetail2 = 0;
                                            //Child2Link3 = drChildRow[drFoundRws]["Link3"].ToString();

                                            DataRow[] drChildRowDetail2 = dtChild3.Select("Link1='" + parentTrxID + "' AND Link2='" + Child1Link2 + "'");

                                            if (drChildRowDetail2.Length > 0)
                                            {
                                                for (int drFoundRws3 = 0; drFoundRws3 < drChildRowDetail2.Length; drFoundRws3++)
                                                {
                                                    RowIndexDetail2 = dt[3].Rows.IndexOf(drChildRowDetail2[drFoundRws3]);
                                                    strSecondQuarter = drChildRowDetail2[drFoundRws3]["EndBal"].ToString();
                                                    if (drChildRowDetail2 != null)
                                                    {
                                                        if (dtFullChild.Columns.Contains("SecondQuarter"))
                                                        {
                                                            dtFullChild.Rows[dtFullChild.Rows.Count - 1]["SecondQuarter"] = strSecondQuarter;
                                                        }
                                                        else
                                                        {
                                                            dtFullChild.Columns.Add("SecondQuarter");
                                                            dtFullChild.Rows[dtFullChild.Rows.Count - 1]["SecondQuarter"] = strSecondQuarter;
                                                        }
                                                        //Delete 
                                                        if (dt[3].Rows.Count > 0)
                                                        {
                                                            //dt[3].Rows[RowIndexDetail2].Delete();
                                                            // dt[3].Rows[0].Delete();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Table4 : TrxDetail3
                                        //Add Ending Bal from TrxDetail3 Table
                                        if (dtChild4 != null)
                                        {
                                            int RowIndexDetail3 = 0;
                                            //Child2Link3 = drChildRow[drFoundRws]["Link3"].ToString();

                                            DataRow[] drChildRowDetail3 = dtChild4.Select("Link1='" + parentTrxID + "' AND Link2='" + Child1Link2 + "'");

                                            if (drChildRowDetail3.Length > 0)
                                            {
                                                for (int drFoundRws3 = 0; drFoundRws3 < drChildRowDetail3.Length; drFoundRws3++)
                                                {
                                                    RowIndexDetail3 = dt[3].Rows.IndexOf(drChildRowDetail3[drFoundRws3]);
                                                    strThirdQuarter = drChildRowDetail3[drFoundRws3]["EndBal"].ToString();
                                                    if (drChildRowDetail3 != null)
                                                    {
                                                        if (dtFullChild.Columns.Contains("ThirdQuarter"))
                                                        {
                                                            dtFullChild.Rows[dtFullChild.Rows.Count - 1]["ThirdQuarter"] = strSecondQuarter;
                                                        }
                                                        else
                                                        {
                                                            dtFullChild.Columns.Add("ThirdQuarter");
                                                            dtFullChild.Rows[dtFullChild.Rows.Count - 1]["ThirdQuarter"] = strSecondQuarter;
                                                        }
                                                        //Delete 
                                                        if (dt[4].Rows.Count > 0)
                                                        {
                                                            //dt[3].Rows[RowIndexDetail3].Delete();
                                                            // dt[3].Rows[0].Delete();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Table5 : TrxDetail4
                                        //Add Ending Bal from TrxDetail3 Table
                                        if (dtChild5 != null)
                                        {
                                            int RowIndexDetail4 = 0;
                                            //Child2Link3 = drChildRow[drFoundRws]["Link3"].ToString();

                                            DataRow[] drChildRowDetail4 = dtChild5.Select("Link1='" + parentTrxID + "' AND Link2='" + Child1Link2 + "'");

                                            if (drChildRowDetail4.Length > 0)
                                            {
                                                for (int drFoundRws3 = 0; drFoundRws3 < drChildRowDetail4.Length; drFoundRws3++)
                                                {
                                                    RowIndexDetail4 = dt[3].Rows.IndexOf(drChildRowDetail4[drFoundRws3]);
                                                    strFourthQuarter = drChildRowDetail4[drFoundRws3]["EndBal"].ToString();
                                                    if (drChildRowDetail4 != null)
                                                    {
                                                        if (dtFullChild.Columns.Contains("FourthQuarter"))
                                                        {
                                                            dtFullChild.Rows[dtFullChild.Rows.Count - 1]["FourthQuarter"] = strSecondQuarter;
                                                        }
                                                        else
                                                        {
                                                            dtFullChild.Columns.Add("FourthQuarter");
                                                            dtFullChild.Rows[dtFullChild.Rows.Count - 1]["FourthQuarter"] = strSecondQuarter;
                                                        }
                                                        //Delete 
                                                        if (dt[5].Rows.Count > 0)
                                                        {
                                                            //dt[3].Rows[RowIndexDetail4].Delete();
                                                            // dt[3].Rows[0].Delete();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Table6 Add TotalYear column
                                        //Add TotalYear column
                                        // TotalYear Amount= FirstQuarter Amount - SecondQuarter Amount
                                        if ((strFirstQuarter != string.Empty) && (strSecondQuarter != string.Empty))
                                        {
                                            double amtFirstQuarter = 0;
                                            double amtSecondQuarter = 0;
                                            double amtThirdQuarter = 0;
                                            double amtFourthQuarter = 0;
                                            double amtTotalYear = 0;

                                            amtFirstQuarter = Convert.ToDouble(strFirstQuarter);
                                            amtSecondQuarter = Convert.ToDouble(strSecondQuarter);
                                            amtThirdQuarter = Convert.ToDouble(strThirdQuarter);
                                            amtFourthQuarter = Convert.ToDouble(strFourthQuarter);


                                            amtTotalYear = amtFirstQuarter + amtSecondQuarter + amtThirdQuarter + amtFourthQuarter;
                                            strTotalYear = Convert.ToString(amtTotalYear);
                                            strTotalYear = clsReportsUICore.ConvertToCurrencyFormat(strTotalYear);
                                            if (dtFullChild.Columns.Contains("TotalYear"))
                                            {
                                                dtFullChild.Rows[dtFullChild.Rows.Count - 1]["TotalYear"] = strTotalYear;
                                            }
                                            else
                                            {
                                                dtFullChild.Columns.Add("TotalYear");
                                                dtFullChild.Rows[dtFullChild.Rows.Count - 1]["TotalYear"] = strTotalYear;
                                            }
                                        }
                                        #endregion
                                        //Add Sum Columns End Balance
                                        for (int i = 0; i < strSumColumns.Length; i++)
                                        {
                                            if (dtFullChild.Columns.Contains(strSumColumns[i]))
                                            {
                                                strCustom = string.Empty;
                                                decimal amount;
                                                TotalAmount = 0;
                                                string amt = string.Empty;
                                                string colName = strSumColumns[i];
                                                if (!htGrandTotals.ContainsKey(colName))
                                                {
                                                    amt = clsReportsUICore.ConvertToCurrencyFormat(dtFullChild.Rows[dtFullChild.Rows.Count - 1][colName].ToString());
                                                    htGrandTotals.Add(colName, amt);
                                                }
                                                else
                                                {
                                                    TotalAmount = Convert.ToDouble(htGrandTotals[colName].ToString());
                                                    if (!string.IsNullOrEmpty(dtFullChild.Rows[dtFullChild.Rows.Count - 1][colName].ToString()))
                                                    {
                                                        TotalAmount = TotalAmount + Convert.ToDouble(dtFullChild.Rows[dtFullChild.Rows.Count - 1][colName].ToString());
                                                    }
                                                    strTotalAmount = Convert.ToString(TotalAmount);
                                                    amt = clsReportsUICore.ConvertToCurrencyFormat(strTotalAmount);
                                                    htGrandTotals[colName] = amt;
                                                }
                                            }
                                        }
                                        //
                                        if (childIndexes[0] != 0)
                                        {
                                            childIndexes[drFoundRws1] = childRowIndex + drFoundRws1;
                                        }
                                        else
                                        {
                                            childIndexes[drFoundRws1] = childRowIndex;
                                        }
                                    }
                                }
                                //To set font for sum rows
                                int sumRowIndex = 0;

                                //Add Columns to DtTotal
                                if (dtTotals.Columns.Count == 0)
                                {
                                    for (int x = 0; x < dtFullChild.Columns.Count; x++)
                                    {
                                        dtTotals.Columns.Add(dtFullChild.Columns[x].ColumnName);
                                    }
                                }
                                IDictionaryEnumerator enumTot = htGrandTotals.GetEnumerator();
                                if (htGrandTotals.Keys.Count > 0)
                                {
                                    dtTotals.Rows.Add(dtTotals.NewRow());
                                    //ADDING TOTAL VALUE
                                    dtTotals.Rows[dtTotals.Rows.Count - 1]["AccountName"] = "Total " + dtNew.Rows[0]["Caption"].ToString();
                                    sumRowIndex = dtTotals.Rows.Count - 1;
                                    TotalAmount = 0;
                                    string amt = string.Empty;

                                    //ADD NEW ROW in datatable dtProfits
                                    dtProfits.Rows.Add(dtProfits.NewRow());

                                    while (enumTot.MoveNext())
                                    {
                                        //dtFullChild.Rows[dtFullChild.Rows.Count - 1][enumTot.Key.ToString()] = enumTot.Value.ToString();
                                        dtTotals.Rows[dtTotals.Rows.Count - 1][enumTot.Key.ToString()] = enumTot.Value.ToString();

                                        //Add Revenue Amounts
                                        if (dtNew.Rows[0]["Caption"].ToString().ToUpper() == "REVENUE")
                                        {
                                            if (!htRevenue.ContainsKey(enumTot.Key.ToString()))
                                            {
                                                //ADD
                                                htRevenue.Add(enumTot.Key.ToString(), enumTot.Value.ToString());
                                            }
                                            else
                                            {
                                                //UPDATE
                                                TotalAmount = Convert.ToDouble(htRevenue[enumTot.Key.ToString()].ToString());
                                                TotalAmount = TotalAmount + double.Parse(enumTot.Value.ToString());
                                                strTotalAmount = Convert.ToString(TotalAmount);
                                                amt = clsReportsUICore.ConvertToCurrencyFormat(strTotalAmount);
                                                htRevenue[enumTot.Key.ToString()] = amt;
                                            }
                                        }
                                        //Add Totals to dtProfits
                                        if (dtProfits.Columns.Contains(enumTot.Key.ToString()))
                                        {
                                            //Add value to dtprofits
                                            dtProfits.Rows[dtProfits.Rows.Count - 1][enumTot.Key.ToString()] = enumTot.Value.ToString();
                                        }
                                        else
                                        {
                                            //Add column and value to dtprofits
                                            dtProfits.Columns.Add(enumTot.Key.ToString());
                                            dtProfits.Rows[dtProfits.Rows.Count - 1][enumTot.Key.ToString()] = enumTot.Value.ToString();
                                        }
                                        //
                                    }
                                    //SET ORIDNALS for dtprofits table

                                    for (int j = 0; j < strSumColumns.Length; j++)
                                    {
                                        if (dtProfits.Columns.Contains(strSumColumns[j].ToString()))
                                        {
                                            dtProfits.Columns[strSumColumns[j]].SetOrdinal(j);
                                        }
                                    }


                                }
                                int[] colChild2Widths = null;
                                if (dtFullChild.Rows.Count > 0)
                                {
                                    alRemovedColumns = new ArrayList();
                                    //Get ordinals to add arraylist
                                    if (dtFullChild.Columns.Contains("TrxID"))
                                    {
                                        alRemovedColumns.Add(dtFullChild.Columns["TrxID"].Ordinal);
                                    }
                                    if (dtFullChild.Columns.Contains("Link1"))
                                    {
                                        alRemovedColumns.Add(dtFullChild.Columns["Link1"].Ordinal);
                                    }
                                    if (dtFullChild.Columns.Contains("Link2"))
                                    {
                                        alRemovedColumns.Add(dtFullChild.Columns["Link2"].Ordinal);
                                    }

                                    //To Delete columns
                                    if (dtFullChild.Columns.Contains("TrxID"))
                                    {
                                        dtFullChild.Columns.Remove("TrxID");
                                    }
                                    if (dtFullChild.Columns.Contains("Link1"))
                                    {
                                        dtFullChild.Columns.Remove("Link1");
                                    }
                                    if (dtFullChild.Columns.Contains("Link2"))
                                    {
                                        dtFullChild.Columns.Remove("Link2");
                                    }

                                    //Delete columns in dtTotals
                                    if (dtTotals.Columns.Contains("TrxID"))
                                    {
                                        dtTotals.Columns.Remove("TrxID");
                                    }
                                    if (dtTotals.Columns.Contains("Link1"))
                                    {
                                        dtTotals.Columns.Remove("Link1");
                                    }
                                    if (dtTotals.Columns.Contains("Link2"))
                                    {
                                        dtTotals.Columns.Remove("Link2");
                                    }

                                    //Check if the row not contains Ending columns
                                    dt2ColumnsCount = Convert.ToInt32(dt2ColumnsCount - 3) + 3;
                                    if (dtFullChild.Columns.Count < dt2ColumnsCount)
                                    {
                                        for (int i = 0; i < strSumColumns.Length; i++)
                                        {
                                            dtFullChild.Columns.Add(strSumColumns[i]);
                                            dtTotals.Columns.Add(strSumColumns[i]);
                                        }
                                    }
                                    //Remove columns TrxID and Link1 from colwidths
                                    colstart = 0;
                                    colChild2Widths = new int[(arrallcolWidths[1].Length + 5) - 3];
                                    for (int ctr = 0; ctr < arrallcolWidths[1].Length; ctr++)
                                    {
                                        if (!alRemovedColumns.Contains(ctr))
                                        {
                                            colChild2Widths[colstart] = arrallcolWidths[1][ctr];
                                            colstart++;
                                        }
                                    }
                                    //Add Ending Columns Widths
                                    int EndbalWidth = 20;
                                    for (int i = 0; i < 5; i++)
                                    {
                                        if (colEndBalOrdinal == 0)
                                        {
                                            colChild2Widths[colstart] = EndbalWidth;
                                        }
                                        else
                                        {
                                            colChild2Widths[colstart] = arrallcolWidths[2][colEndBalOrdinal];
                                        }
                                        colstart++;
                                    }
                                    if (colEndBalOrdinal == 0)
                                    {
                                        if (dtFullChild.Columns.Contains("FirstQuarter"))
                                        {
                                            //set ordeinal for empty columns
                                            colEndBalOrdinal = dtFullChild.Columns["FirstQuarter"].Ordinal;
                                        }
                                    }
                                    else
                                    {
                                        if (dtFullChild.Columns.Contains("FirstQuarter"))
                                        {
                                            //set ordeinal for empty columns
                                            colEndBalOrdinal = dtFullChild.Columns["FirstQuarter"].Ordinal;
                                        }
                                    }
                                    //ADD GrandTotals
                                    //int grandSumRowIndex = 0;
                                    //if (dtGrandTotals.Columns.Count == 0)
                                    //{
                                    //    for (int x = 0; x < dtFullChild.Columns.Count; x++)
                                    //    {
                                    //        dtGrandTotals.Columns.Add(dtFullChild.Columns[x].ColumnName);
                                    //    }
                                    //}
                                }

                                if (dtFullChild.Rows.Count > 0)
                                {
                                    PdfTable myPdfChild = myPdfDocument.NewTable(FontRegular, dtFullChild.Rows.Count, dtFullChild.Columns.Count, 1);
                                    myPdfChild.ImportDataTable(dtFullChild);
                                    myPdfChild.SetBorders(Color.Black, 1, BorderType.None);
                                    myPdfChild.SetContentAlignment(ContentAlignment.MiddleLeft);
                                    myPdfChild.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                    myPdfChild.SetColumnsWidth(colChild2Widths);
                                    myPdfChild.HeadersRow.SetFont(FontRegular);
                                    myPdfChild.HeadersRow.SetBackgroundColor(Color.White);
                                    myPdfChild.HeadersRow.SetForegroundColor(Color.White);
                                    for (int i = 0; i < dtFullChild.Rows.Count; i++)
                                    {
                                        //SET RIGHT ALIGNMENT
                                        for (int j = 0; j < strSumColumns.Length; j++)
                                        {
                                            myPdfChild.Rows[i][colEndBalOrdinal + j].SetContentAlignment(ContentAlignment.MiddleRight);
                                        }
                                        foreach (PdfCell pcll in myPdfChild.Rows[i].Cells)
                                        {
                                            if (pcll.Content.ToString() == "SKIP")
                                            {
                                                pcll.SetBackgroundColor(Color.White);
                                                pcll.SetForegroundColor(Color.White);
                                            }
                                        }
                                        //TrxID WHITE
                                        //myPdfChild.Rows[i][0].SetForegroundColor(Color.White);
                                    }
                                    posX = posX + 10;
                                    while (!myPdfChild.AllTablePagesCreated)
                                    {
                                        ////Setting the Y position and if required creating new page
                                        //if (currentYPos > myPdfDocument.PageHeight - 50)
                                        //{
                                        //    posY = 70;
                                        //    currentYPos = 70;
                                        //    newPdfPage.SaveToDocument();
                                        //    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                        //    newPdfPage = myPdfDocument.NewPage();
                                        //    newPdfPage.Add(myHeaderPdfTablePage);
                                        //    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                        //}
                                        //else
                                        //{
                                        //    posY = currentYPos;
                                        //}
                                        //if (myPdfDocument.PageHeight - 50 - posY < 50)
                                        //{
                                        //    posY = 70;
                                        //    currentYPos = 70;
                                        //    newPdfPage.SaveToDocument();
                                        //    //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                        //    newPdfPage = myPdfDocument.NewPage();
                                        //    newPdfPage.Add(myHeaderPdfTablePage);
                                        //    if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                        //}
                                        //Setting the Y position and if required creating new page
                                        if (currentYPos > myPdfDocument.PageHeight - 50)
                                        {
                                            posY = 70;
                                            currentYPos = 70;
                                            newPdfPage.SaveToDocument();
                                            //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                            newPdfPage = myPdfDocument.NewPage();
                                            newPdfPage.Add(myHeaderPdfTablePage);
                                            if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                            posY = currentYPos + 25;
                                            currentYPos = posY + 9 + ((pdfMainHeaderPage.Area.PosY - posY) / 2);
                                            posY = currentYPos;
                                            newPdfPage.Add(pdfMainHeaderPage);
                                            newPdfPage.Add(pdfLineBrk);
                                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));
                                        }
                                        else
                                        {
                                            if (posY >= currentYPos)
                                            {
                                                currentYPos = posY + 9 + (pdfMainHeaderPage.Area.PosY - posY) / 2;
                                                posY = currentYPos;
                                            }
                                            else
                                            {
                                                posY = currentYPos;
                                            }
                                        }
                                        if (myPdfDocument.PageHeight - 50 - posY < 50)
                                        {
                                            posY = 70;
                                            currentYPos = 70;
                                            newPdfPage.SaveToDocument();
                                            //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                            newPdfPage = myPdfDocument.NewPage();
                                            newPdfPage.Add(myHeaderPdfTablePage);
                                            if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                            //
                                            posY = currentYPos + 25;
                                            currentYPos = posY + 9 + ((pdfMainHeaderPage.Area.PosY - posY) / 2);
                                            posY = currentYPos;
                                            newPdfPage.Add(pdfMainHeaderPage);
                                            newPdfPage.Add(pdfLineBrk);
                                            newPdfPage.Add(new PdfTextArea(FontRegular, Color.Black, new PdfArea(myPdfDocument, 100, myPdfDocument.PageHeight - 50, 450, 50), ContentAlignment.MiddleRight, "Page " + Convert.ToString((++pageCnt))));
                                        }
                                        PdfTablePage newPdfTablePage2 = myPdfChild.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                        newPdfPage.Add(newPdfTablePage2);
                                        currentYPos = newPdfTablePage2.Area.BottomLeftVertex.Y;
                                    }
                                    posX = posX - 10;
                                }
                                //Total and grand totals starts
                                #region Table Totals

                                if (dtTotals.Rows.Count > 0)
                                {
                                    PdfTable myPdfChildTotals = myPdfDocument.NewTable(FontRegular, dtTotals.Rows.Count, dtTotals.Columns.Count, 1);
                                    myPdfChildTotals.ImportDataTable(dtTotals);
                                    myPdfChildTotals.SetBorders(Color.Black, 1, BorderType.None);
                                    myPdfChildTotals.SetContentAlignment(ContentAlignment.MiddleLeft);
                                    myPdfChildTotals.HeadersRow.SetContentAlignment(ContentAlignment.MiddleLeft);
                                    myPdfChildTotals.SetColumnsWidth(colChild2Widths);
                                    myPdfChildTotals.HeadersRow.SetFont(FontRegular);
                                    myPdfChildTotals.HeadersRow.SetBackgroundColor(Color.White);
                                    myPdfChildTotals.HeadersRow.SetForegroundColor(Color.White);
                                    for (int i = 0; i < dtTotals.Rows.Count; i++)
                                    {
                                        //SET RIGHT ALIGNMENT
                                        for (int j = 0; j < strSumColumns.Length; j++)
                                        {
                                            myPdfChildTotals.Rows[i][colEndBalOrdinal + j].SetContentAlignment(ContentAlignment.MiddleRight);
                                        }
                                        foreach (PdfCell pcll in myPdfChildTotals.Rows[i].Cells)
                                        {

                                            /*if (i == sumRowIndex)
                                            {
                                                //SET SUM ROW FONT
                                                pcll.SetFont(HeaderPageTitleFont2);
                                            }*/

                                            if (pcll.Content.ToString() == "SKIP")
                                            {
                                                pcll.SetBackgroundColor(Color.White);
                                                pcll.SetForegroundColor(Color.White);
                                            }
                                        }
                                        //TrxID WHITE
                                        //myPdfChildTotals.Rows[i][0].SetForegroundColor(Color.White);
                                    }
                                    posX = posX + 10;
                                    while (!myPdfChildTotals.AllTablePagesCreated)
                                    {
                                        //Setting the Y position and if required creating new page
                                        if (currentYPos > myPdfDocument.PageHeight - 50)
                                        {
                                            posY = 70;
                                            currentYPos = 70;
                                            newPdfPage.SaveToDocument();
                                            //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                            newPdfPage = myPdfDocument.NewPage();
                                            newPdfPage.Add(myHeaderPdfTablePage);
                                            if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                        }
                                        else
                                        {
                                            posY = currentYPos;
                                        }
                                        if (myPdfDocument.PageHeight - 50 - posY < 50)
                                        {
                                            posY = 70;
                                            currentYPos = 70;
                                            newPdfPage.SaveToDocument();
                                            //Adding new page and adding Header table,logo image and pageNoand adding Header table,logo image and pageNo 
                                            newPdfPage = myPdfDocument.NewPage();
                                            newPdfPage.Add(myHeaderPdfTablePage);
                                            if (LogoImage != null) { newPdfPage.Add(LogoImage, imgPosX, imgPosY); }
                                        }

                                        PdfTablePage newPdfTablePage2 = myPdfChildTotals.CreateTablePage(new PdfArea(myPdfDocument, posX, posY, width, height - posY));
                                        //PBOX 2 Rows and 2 Columns
                                        /*for (int i = 0; i < strSumColumns.Length; i++)
                                        {
                                            int rowbox = 0;
                                            if (i == 0)
                                            {
                                                rowbox = sumRowIndex;
                                            }
                                            else
                                            {
                                                rowbox = grandSumRowIndex;
                                            }
                                           // int cellbox = Convert.ToInt32(dtFullChild.Columns[strSumColumns[i]].Ordinal);

                                            PdfRectangle pr = newPdfTablePage2.CellArea(rowbox, colEndBalOrdinal).ToRectangle(Color.Black, 0.5, Color.White);
                                            pr.StrokeWidth = 1;
                                            newPdfPage.Add(pr);

                                            PdfRectangle pr1 = newPdfTablePage2.CellArea(rowbox, colEndBalOrdinal + 1).ToRectangle(Color.Black, 0.5, Color.White);
                                            pr1.StrokeWidth = 1;
                                            newPdfPage.Add(pr1);

                                        }*/
                                        //PBOX
                                        newPdfPage.Add(newPdfTablePage2);
                                        currentYPos = newPdfTablePage2.Area.BottomLeftVertex.Y;

                                    }
                                    posX = posX - 10;
                                }
                                #endregion

                                //grand totals

                            } //dt[1]2 close
                            #endregion


                        }//dtnew close
                    }//Table 0  parent for loop end
                } //dt if condtion
                newPdfPage.SaveToDocument();
                CreatePDFDocument(fileName);
            }
            catch (Exception ex)
            {
                #region NLog
                logger.Fatal(ex);
                #endregion

                throw ex;
            }
        }
        #endregion
    }
}
