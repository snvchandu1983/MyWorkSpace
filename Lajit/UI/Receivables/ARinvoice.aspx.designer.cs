//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LAjitDev.Receivables {
    
    public partial class ARinvoice {
        protected System.Web.UI.WebControls.Panel pnlContent;
        protected System.Web.UI.UpdatePanel updtPnlContent;
        protected System.Web.UI.UserControl BtnsUC;
        protected System.Web.UI.WebControls.Panel pnlCPGV1Title;
        protected System.Web.UI.HtmlControls.HtmlTableCell htcCPGV1;
        protected System.Web.UI.HtmlControls.HtmlTableCell htcCPGV1Auto;
        protected System.Web.UI.WebControls.Panel pnlGVContent;
        protected System.Web.UI.UserControl GVUC;
        protected System.Web.UI.WebControls.Panel pnlEntryForm;
        protected System.Web.UI.HtmlControls.HtmlTableRow trPopupHeader;
        protected System.Web.UI.HtmlControls.HtmlTableCell htcPopEntryForm;
        protected System.Web.UI.WebControls.Label lblPopupEntry;
        protected System.Web.UI.HtmlControls.HtmlTableRow trProcessLinks;
        protected System.Web.UI.HtmlControls.HtmlTableCell tdProcessLinks;
        protected System.Web.UI.WebControls.Panel pnlBPCContainer;
        protected System.Web.UI.HtmlControls.HtmlTableRow trSubject;
        protected System.Web.UI.WebControls.Label lblPageSubject;
        protected System.Web.UI.HtmlControls.HtmlTableRow trSoxApprovedStatus;
        protected LAjitControls.LAjitImageButton imgbtnIsApproved;
        protected System.Web.UI.HtmlControls.HtmlTableRow trCheckMessage_JournalDoc;
        protected LAjitControls.LAjitLabel lblCheckMessage_JournalDoc_Value;
        protected System.Web.UI.HtmlControls.HtmlTableRow trCustomer_JournalDoc;
        protected System.Web.UI.WebControls.Label lblCustomer_JournalDoc;
        protected LAjitControls.LAjitTextBox txtCustomer_JournalDoc;
        protected LAjitControls.LAjitRequiredFieldValidator reqCustomer_JournalDoc;
        protected System.Web.UI.HtmlControls.HtmlTableRow trSelectCustContact_JournalDoc;
        protected System.Web.UI.WebControls.Label lblSelectCustContact_JournalDoc;
        protected LAjitControls.LAjitDropDownList ddlSelectCustContact_JournalDoc;
        protected LAjitControls.LAjitRequiredFieldValidator reqSelectCustContact_JournalDoc;
        protected System.Web.UI.HtmlControls.HtmlTableRow trDescription;
        protected System.Web.UI.WebControls.Label lblDescription;
        protected LAjitControls.LAjitTextBox txtDescription;
        protected LAjitControls.LAjitRequiredFieldValidator reqDescription;
        protected System.Web.UI.HtmlControls.HtmlTableRow trInvoiceNumber;
        protected System.Web.UI.WebControls.Label lblInvoiceNumber;
        protected LAjitControls.LAjitTextBox txtInvoiceNumber;
        protected LAjitControls.LAjitRequiredFieldValidator reqInvoiceNumber;
        protected System.Web.UI.HtmlControls.HtmlTableRow trTrxDate;
        protected System.Web.UI.WebControls.Label lblTrxDate;
        protected LAjitControls.LAjitTextBox txtTrxDate;
        protected LAjitControls.LAjitRequiredFieldValidator reqTrxDate;
        protected System.Web.UI.HtmlControls.HtmlTableRow trSelectCustInvoice_JournalDoc;
        protected System.Web.UI.WebControls.Label lblSelectCustInvoice_JournalDoc;
        protected LAjitControls.LAjitTextBox txtSelectCustInvoice_JournalDoc;
        protected LAjitControls.LAjitRequiredFieldValidator reqSelectCustInvoice_JournalDoc;
        protected System.Web.UI.HtmlControls.HtmlTableRow trCustPayTerm_JournalDoc;
        protected System.Web.UI.WebControls.Label lblCustPayTerm_JournalDoc;
        protected LAjitControls.LAjitDropDownList ddlCustPayTerm_JournalDoc;
        protected LAjitControls.LAjitRequiredFieldValidator reqCustPayTerm_JournalDoc;
        protected LAjitControls.LAjitLinkButton lnkBtnCustomer_JournalDoc;
        protected System.Web.UI.HtmlControls.HtmlTableRow trSelectBatch_JournalDoc;
        protected System.Web.UI.WebControls.Label lblSelectBatch_JournalDoc;
        protected LAjitControls.LAjitDropDownList ddlSelectBatch_JournalDoc;
        protected LAjitControls.LAjitRequiredFieldValidator reqSelectBatch_JournalDoc;
        protected System.Web.UI.HtmlControls.HtmlTableRow trInvoiceAmount;
        protected System.Web.UI.WebControls.Label lblInvoiceAmount;
        protected LAjitControls.LAjitTextBox txtInvoiceAmount;
        protected LAjitControls.LAjitRequiredFieldValidator reqInvoiceAmount;
        protected LAjitControls.LAjitRegularExpressionValidator regInvoiceAmount;
        protected System.Web.UI.HtmlControls.HtmlTableRow trPostDate;
        protected System.Web.UI.WebControls.Label lblPostDate;
        protected LAjitControls.LAjitTextBox txtPostDate;
        protected LAjitControls.LAjitRequiredFieldValidator reqPostDate;
        protected System.Web.UI.HtmlControls.HtmlTableRow trCurrencyTypeCompany;
        protected System.Web.UI.WebControls.Label lblCurrencyTypeCompany;
        protected LAjitControls.LAjitDropDownList ddlCurrencyTypeCompany;
        protected LAjitControls.LAjitRequiredFieldValidator reqCurrencyTypeCompany;
        protected System.Web.UI.HtmlControls.HtmlTableRow trSalesRep_JournalDoc;
        protected System.Web.UI.WebControls.Label lblSalesRep_JournalDoc;
        protected LAjitControls.LAjitDropDownList ddlSalesRep_JournalDoc;
        protected LAjitControls.LAjitRequiredFieldValidator reqSalesRep_JournalDoc;
        protected System.Web.UI.HtmlControls.HtmlTableRow trInvoiceComment_JournalDoc;
        protected System.Web.UI.WebControls.Label lblInvoiceComment_JournalDoc;
        protected LAjitControls.LAjitTextBox txtInvoiceComment_JournalDoc;
        protected LAjitControls.LAjitRequiredFieldValidator reqInvoiceComment_JournalDoc;
        protected LAjitDev.UserControls.ChildGridView CGVUC;
        protected LAjitControls.LAjitImageButton imgbtnSubmit;
        protected System.Web.UI.WebControls.ImageButton imgbtnContinueAdd;
        protected System.Web.UI.WebControls.ImageButton imgbtnAddClone;
        protected System.Web.UI.WebControls.ImageButton imgbtnCancel;
        protected System.Web.UI.WebControls.Label lblmsg;
        protected System.Web.UI.WebControls.ImageButton imgbtnPrevious;
        protected System.Web.UI.WebControls.ImageButton imgbtnNext;
        protected System.Web.UI.Timer timerEntryForm;
        protected System.Web.UI.WebControls.Panel pnlContentError;
        protected System.Web.UI.WebControls.Label lblError;
    }
}
