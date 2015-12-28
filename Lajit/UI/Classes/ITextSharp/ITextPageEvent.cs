using System;
using System.Collections;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace LAjitDev.Classes.ITextSharp
{
    public class ITextPageEvent : PdfPageEventHelper 
    {
        private PdfPTable m_TableNodeTitle;

        public PdfPTable TableNodeTitle
        {
            get { return m_TableNodeTitle; }
            set { m_TableNodeTitle = value; }
        }
        private PdfPTable m_TableSpacer;

        public PdfPTable TableSpacer
        {
            get { return m_TableSpacer; }
            set { m_TableSpacer = value; }
        }
        private PdfPTable m_TableColHeader;

        public PdfPTable TableColHeader
        {
            get { return m_TableColHeader; }
            set { m_TableColHeader = value; }
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
            //Fix the page overflow bug occurring in style 671.
            if (writer.PageNumber != 1)
            {
                document.Add(m_TableNodeTitle);
                document.Add(m_TableSpacer);
                document.Add(m_TableColHeader);
            }
        }
    }
}



