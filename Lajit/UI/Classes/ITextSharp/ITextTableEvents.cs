using System;
using System.Collections.Generic;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace LAjitDev
{
    class MyTableEvent : IPdfPTableEvent
    {
        public void TableLayout(PdfPTable table
                              , float[][] width     //width[row][col] 
                              , float[] heights     //height[row]
                              , int headerRows
                              , int rowStart
                              , PdfContentByte[] canvases)
        {
            float[] colWidths = width[0];
            PdfContentByte canvas;  // = canvases[PdfPTable.TEXTCANVAS];
            float x, y, w, h;

            //---------------------------------------------------
            // WARNING: As in PostScript, Y increases UPWARDS 
            //          (origin is a BOTTOM LEFT corner)
            //---------------------------------------------------

            int numCols = colWidths.Length;
            int numRows = heights.Length;
            int firstCol = 0;
            int firstRow = 0;
            int lastCol = numCols - 1;
            int lastRow = numRows - 1;

            //Debug.WriteLine(String.Format("rowStart={0}: numCols={1},numRow={2}"
            //                             , rowStart, numCols, numRows));

            //--------------------------------------------------- TEXTCANVAS
            canvas = canvases[PdfPTable.TEXTCANVAS];
            canvas.SaveState();
            {
                canvas.SetLineWidth(2);
                canvas.SetRGBColorStroke(255, 0, 0);        //set line color to RED
                x = colWidths[firstCol];                        //X of first col
                y = heights[lastRow];                           //Y of last row
                w = colWidths[lastCol] - colWidths[firstCol];   //W of all columns on the page
                h = heights[firstRow] - heights[lastRow];       //H of rows on the page
                canvas.Rectangle(x, y, w, h);
                canvas.Stroke();

#if true
                if (headerRows > 0)
                {
                    float headerHeight = heights[firstRow];
                    for (int row = firstRow; row < headerRows; ++row)
                    {
                        headerHeight += heights[row];
                    }

                    canvas.SetRGBColorStroke(0, 0, 255);    //set line color to BLUE
                    x = colWidths[firstCol];                        //X of first col
                    y = heights[headerRows];                        //Y of last HEADER row
                    w = colWidths[lastCol] - colWidths[firstCol];   //W of all columns on the page
                    h = heights[firstRow] - heights[headerRows];   //H of HEADER rows on the page
                    canvas.Rectangle(x, y, w, h);
                    canvas.Stroke();

                    //canvas.SetRGBColorFillF(0.8f, 0.8f, 0.8f);
                    //canvas.FillStroke();
                    //canvas.Rectangle(x, y, w, h);
                    // --- draw a medium gray colored diagonal line
                    canvas.SetRGBColorStrokeF((float)0.5
                                            , (float)0.5
                                            , (float)0.5);
                    canvas.MoveTo(x, y);      //FROM HEADER left,top
                    canvas.LineTo(x + w, y + h);  //TO   HEADER right,bottom 
                    canvas.Stroke();
                }
#endif
            }
            canvas.RestoreState();

            //--------------------------------------------------- BASECANVAS

            canvas = canvases[PdfPTable.BASECANVAS];
            canvas.SaveState();
            {
                canvas.SetLineWidth(0.5f);
                Random r = new Random();
                for (int row = firstRow; row < lastRow; ++row)
                {
                    colWidths = width[row];
                    float fR = 0.8f;
                    float fG = 0.8f;
                    float fB = 0.8f;
                    for (int col = firstCol; col < lastCol; ++col)
                    {
                        //--- if it is the first cell, set up an action URL
                        string sURL = "http://www.geocities.com/itextpdf";
                        if (row == firstRow && col == firstCol)
                            canvas.SetAction(new PdfAction(sURL)
                                            , colWidths[col]
                                            , heights[row + 1]
                                            , colWidths[col + 1]
                                            , heights[row]);

                        // --- draw a random colored horizontal line
                        canvas.SetRGBColorStrokeF(fR, fG, fB);

                        canvas.MoveTo(colWidths[col], heights[row]);  //FROM cell left ,top
                        canvas.LineTo(colWidths[col + 1], heights[row]);  //TO   cell right,top 
                        canvas.Stroke();

                        // --- draw a random colored vertical line
                        canvas.SetRGBColorStrokeF(fR, fG, fB);
                        canvas.MoveTo(colWidths[col], heights[row]);  //FROM cell left,top
                        canvas.LineTo(colWidths[col], heights[row + 1]);  //TO   cell left,bottom 
                        canvas.Stroke();
#if !true
                        // --- draw a medium gray colored diagonal line
                        canvas.SetRGBColorStrokeF((float)0.5
                                                , (float)0.5
                                                , (float)0.5);
                        canvas.MoveTo(colWidths[col], heights[row]);        //FROM cell left,top
                        canvas.LineTo(colWidths[col + 1], heights[row + 1]);  //TO   cell right,bottom 
                        canvas.Stroke();
#endif
                    }
                }
            }
            canvas.RestoreState();
        }
    }

    // this class, based on the above class, is used for XmlStore, 
    // but, very likely, can be used for any datasource for creating a PDF Table 
    // (but based on iTextSharp.PdfPTable)
    class XmlStoreEvent : IPdfPTableEvent
    {
        // Member variables
        private string msMsg = "";
        private string msEOL = Environment.NewLine;

        private string msPageTitle = "";
        private string msPageTitleFormat = "{0}";

        private int mnPagesTotal = 0;
        private int mnPageNumber = 0;
        private int mnPageSection = 0;
        private string msPageNumFormat = "";//"Page {0}";    //to get printing section too use
        //"Page {0}-{1}";

        private string msWatermarkText = "";
        private string msWatermarkFile = "";

        //--- internal use members
        private float mPageW;
        private float mPageH;
        private BaseFont mBaseFont;

        // Properties
        public int PageNumberStartingValue
        {
            get { return mnPageNumber; }
            set { mnPageNumber = value; }
        }
        public int PageSectionStartingValue
        {
            get { return mnPageSection; }
            set { mnPageSection = value; }
        }
        public string PageNumberFormat
        {
            get { return msPageNumFormat; }
            set { msPageNumFormat = value; }
        }
        public int PagesTotal
        {
            get { return mnPagesTotal; }
            set { mnPagesTotal = value; }
        }
        public string PageTitle
        {
            get { return msPageTitle; }
            set { msPageTitle = value; }
        }
        public string PageTitleFormat
        {
            get { return msPageTitleFormat; }
            set { msPageTitleFormat = value; }
        }
        public string WatermarkText
        {
            get { return msWatermarkText; }
            set { msWatermarkText = value; }
        }
        public string WatermarkFile
        {
            get { return msWatermarkFile; }
            set { msWatermarkFile = value; }
        }
        public string Message
        {
            get { return msMsg; }
            set { msMsg = value; }
        }
        public XmlStoreEvent()
        {
        }
        public XmlStoreEvent(int nStartPageNum)
        {
            this.mnPageNumber = nStartPageNum;
        }

        public void TableLayout(PdfPTable table
                              , float[][] width     //width[row][col] 
                              , float[] heights     //height[row]
                              , int headerRows
                              , int rowStart
                              , PdfContentByte[] canvases)
        {
            float[] colWidths = width[0];
            PdfContentByte canvas;  // = canvases[PdfPTable.TEXTCANVAS];

            //---------------------------------------------------
            // WARNING: As in PostScript, Y increases UPWARDS 
            //          (origin is a BOTTOM LEFT corner)
            //---------------------------------------------------

            int numCols = colWidths.Length;
            int numRows = heights.Length;
            int firstCol = 0;
            int firstRow = 0;
            int lastCol = numCols - 1;
            int lastRow = numRows - 1;

            //--- get dimensions of page
            canvas = canvases[PdfPTable.BACKGROUNDCANVAS];
            mPageW = canvas.PdfDocument.Right + canvas.PdfDocument.RightMargin;
            mPageH = canvas.PdfDocument.Top + canvas.PdfDocument.TopMargin;
            //mnPagesTotal = canvas.PdfDocument.PageCount;

            //--- increment page number
            mnPageNumber++;

            //Debug.WriteLine(String.Format("nPageNumber={0}: numCols={1},numRow={2}"
            //                             , mnPageNumber, numCols, numRows));
            //Debug.WriteLine(String.Format("rowStart={0}: numCols={1},numRow={2}"
            //                             , rowStart, numCols, numRows));

            mBaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);


            #region TEXTCANVAS
            //--------------------------------------------------- TEXTCANVAS
            canvas = canvases[PdfPTable.TEXTCANVAS];
            canvas.SaveState();
            {
                //---Title
                //   on this canvas it will appear in black, the default color of the font
                this.DoDrawPageTitle(canvas);

                //---Page Number
                //   on this canvas it will appear in black, the default color of the font
                this.DoDrawPageNumber(canvas);
            }
            canvas.RestoreState();
            #endregion //TEXTCANVAS

            #region BACKGROUNDCANVAS
            //--------------------------------------------------- BACKGROUNDCANVAS
            canvas = canvases[PdfPTable.BACKGROUNDCANVAS];
            canvas.SaveState();
            {
                //--- doing this on this canvas, makes the color appear BEHIND the header text
                this.DoDrawHeaderBackground(canvas, colWidths, heights, headerRows, rowStart);
            }
            canvas.RestoreState();
            #endregion //TEXTCANVAS

            #region BASECANVAS
            //--------------------------------------------------- BASECANVAS

            canvas = canvases[PdfPTable.BASECANVAS];
            canvas.SaveState();
            {
                canvas.SetLineWidth(0.5f);
                Random r = new Random();
                for (int row = firstRow; row < lastRow; ++row)
                {
                    colWidths = width[row];
                    float fR = 0.8f;
                    float fG = 0.8f;
                    float fB = 0.8f;
                    for (int col = firstCol; col < lastCol; ++col)
                    {
                        //--- if it is the first cell, set up an action URL
                        string sURL = "http://www.geocities.com/itextpdf";
                        if (row == firstRow && col == firstCol)
                            canvas.SetAction(new PdfAction(sURL)
                                            , colWidths[col]
                                            , heights[row + 1]
                                            , colWidths[col + 1]
                                            , heights[row]);

                        // --- draw a random colored horizontal line
                        canvas.SetRGBColorStrokeF(fR, fG, fB);

                        canvas.MoveTo(colWidths[col], heights[row]);  //FROM cell left ,top
                        canvas.LineTo(colWidths[col + 1], heights[row]);  //TO   cell right,top 
                        canvas.Stroke();

                        // --- draw a random colored vertical line
                        canvas.SetRGBColorStrokeF(fR, fG, fB);
                        canvas.MoveTo(colWidths[col], heights[row]);  //FROM cell left,top
                        canvas.LineTo(colWidths[col], heights[row + 1]);  //TO   cell left,bottom 
                        canvas.Stroke();
#if !true
                        // --- draw a medium gray colored diagonal line
                        canvas.SetRGBColorStrokeF((float)0.5
                                                , (float)0.5
                                                , (float)0.5);
                        canvas.MoveTo(colWidths[col], heights[row]);        //FROM cell left,top
                        canvas.LineTo(colWidths[col + 1], heights[row + 1]);  //TO   cell right,bottom 
                        canvas.Stroke();
#endif
                    }
                }
            }
            canvas.RestoreState();
            #endregion //BASECANVAS
        }

        private bool DoDrawPageTitle(PdfContentByte canvas)
        {
            bool bRet = false;

            if (this.msPageTitle.Length > 0)
            {
                try
                {
                    canvas.BeginText();
                    string sTitle;
                    sTitle = String.Format(this.msPageTitleFormat, this.msPageTitle);

                    float fontSize = 10f;
                    canvas.SetFontAndSize(mBaseFont, fontSize);
                    float x = mPageW / 2;
                    float y = mPageH - (canvas.PdfDocument.TopMargin - 8);
                    canvas.ShowTextAligned(PdfContentByte.ALIGN_CENTER, sTitle, x, y, 0);
                    canvas.EndText();
                }
                catch (DocumentException de)
                {
                    this.Message += de.Message + msEOL;
                }
            }
            return bRet;
        }

        private bool DoDrawPageNumber(PdfContentByte canvas)
        {
            bool bRet = false;

            try
            {
                if (this.msPageNumFormat.Length > 0)
                {
                    canvas.BeginText();
                    string sPage;
                    if (this.msPageNumFormat.IndexOf("{1}") > 0)
                        sPage = String.Format(msPageNumFormat, this.mnPageNumber, this.mnPageSection);
                    else
                        sPage = String.Format(msPageNumFormat, this.mnPageNumber);

                    if (mnPagesTotal != 0)
                        sPage += " of " + this.mnPagesTotal.ToString();

                    canvas.SetFontAndSize(mBaseFont, 8);
                    float rotation = 0f;
                    float x = mPageW / 2;
                    float y = 20;
                    canvas.ShowTextAligned(PdfContentByte.ALIGN_CENTER, sPage, x, y, rotation);
                    canvas.EndText();
                }
            }
            catch (DocumentException de)
            {
                this.Message += de.Message + msEOL;
            }

            return bRet;
        }

        private bool DoDrawWatermarkText(PdfContentByte canvas)
        {
            bool bRet = false;

            try
            {
                canvas.BeginText();
                canvas.SetFontAndSize(mBaseFont, 72);
                float rotation = 45f;
                float x = mPageW / 2;
                float y = mPageH / 2;
                canvas.ShowTextAligned(PdfContentByte.ALIGN_CENTER, msWatermarkText, x, y, rotation);
                canvas.EndText();
            }
            catch (DocumentException de)
            {
                this.Message += de.Message + msEOL;
            }

            return bRet;
        }

        private void DoDrawLine(PdfContentByte canvas
                                , float x1, float y1, float x2, float y2)
        {
            DoDrawLine(canvas, x1, y1, x2, y2, 0.5f, 0.5f, 0.5f);
        }

        private void DoDrawLine(PdfContentByte canvas
                                , float x1, float y1, float x2, float y2
                                , float fR, float fG, float fB)
        {
            canvas.SetRGBColorStrokeF(fR, fG, fB);
            canvas.MoveTo(x1, y1);
            canvas.LineTo(x2, y2);
            canvas.Stroke();
        }

        private void DoDrawHeaderBackground(PdfContentByte canvas
                                          , float[] colWidths       //width[col] 
                                          , float[] heights         //height[row]
                                          , int headerRows
                                          , int rowStart)
        {
            //--- these are just to make things a little more readable
            int numCols = colWidths.Length;
            int numRows = heights.Length;
            int firstCol = 0;
            int firstRow = 0;
            int lastCol = numCols - 1;
            int lastRow = numRows - 1;

            //--- calc the height of the header row(s)
            float headerHeight = heights[firstRow];
            for (int row = firstRow; row < headerRows; ++row)
            {
                headerHeight += heights[row];
            }

            //--- fill header rectangle with a color
            float x = colWidths[firstCol];                          //X of first col
            float y = heights[headerRows];                          //Y of last HEADER row
            float w = colWidths[lastCol] - colWidths[firstCol];     //W of all columns on the page
            float h = heights[firstRow] - heights[headerRows];      //H of HEADER rows on the page
            canvas.Rectangle(x, y, w, h);

            canvas.SetRGBColorFillF(0.8f, 0.8f, 0.8f);  //'light' gray 
            // basically multipliers for 255
            canvas.FillStroke();                        // Adobe-speak for "Fill"!!!

        }
    }
}
