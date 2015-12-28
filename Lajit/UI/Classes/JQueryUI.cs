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
using System.Collections.Generic;
using System.IO;

namespace LAjitDev
{
    public static class JQueryUI
    {
        public static string RegisterJQueryCollapsableExtender(Page page, string m_pnlTitleID, string m_pnlContentID, string m_imgID)
        {
            //StringBuilder sb = new StringBuilder();
            //sb.Append("$get('" + m_pnlTitleID + "').onclick=function(){");
            //sb.Append("jQuery('#" + m_pnlContentID + "').slideToggle('slow');");
            //sb.Append("var imgIcon=document.getElementById('" + m_imgID + "').getAttribute('src'); ");
            //sb.Append("var imgIconSrc=imgIcon.split('/');");
            //sb.Append("if(imgIconSrc[4]=='minus-icon.png'){");
            //sb.Append("jQuery('#" + m_imgID + "').attr('src','../App_Themes/" + HttpContext.Current.Session["MyTheme"] + "/Images/plus-icon.png');");
            //sb.Append("}");
            //sb.Append("else{");
            //sb.Append("jQuery('#" + m_imgID + "').attr('src','../App_Themes/" + HttpContext.Current.Session["MyTheme"] + "/Images/minus-icon.png');");
            //sb.Append("}");
            //sb.Append("};");
            //return sb.ToString();
            return "AttachCollapsibleExt('" + m_pnlTitleID + "','" + m_pnlContentID + "','" + m_imgID + "');";
        }
        public static string RegisterJQueryDatePicker(Page page, ArrayList m_alCalendarTBoxIDS, string dateFormat,string nodeType)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m_alCalendarTBoxIDS.Count; i++)
            {
                string isParent;
                if (nodeType == "Parent")
                {
                    ////Parent controls on image click displaying calender
                    //sb.Append("jQuery('#" + m_alCalendarTBoxIDS[i].ToString() + "').datepicker({dateFormat: '" + dateFormat + "',changeMonth: true,changeYear: true, onClose:CalClose,showOn: 'button', buttonImage: '../App_Themes/" + HttpContext.Current.Session["MyTheme"] + "/Images/calendar-icon.gif', buttonImageOnly: true, buttonText:'Calendar'});");
                    isParent = "true";
                }
                else
                {
                    ////Child controls on click of text box loading calender
                    //sb.Append("jQuery('#" + m_alCalendarTBoxIDS[i].ToString() + "').datepicker({dateFormat: '" + dateFormat + "',changeMonth: true,changeYear: true, onClose:CalClose });");
                    ////sb.Append("jQuery('#" + m_alCalendarTBoxIDS[i].ToString() + "').datepicker({dateFormat: '" + dateFormat + "',changeMonth: true,changeYear: true});");
                    isParent = "false";
                }
                sb.AppendFormat("AttachCal('{0}','{1}',{2});", m_alCalendarTBoxIDS[i].ToString(), dateFormat, isParent);
            }
            return sb.ToString();
        }

        public static string RegisterJQueryMaskEditor(Page page, ArrayList m_alMask, string format)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m_alMask.Count; i++)
            {
                sb.Append("jQuery('#" + m_alMask[i].ToString() + "').mask('" + format + "');");
            }
            return sb.ToString();
        }

        public static string RegisterJQueryCalculator(Page page, ArrayList m_alCalcTBoxIDS)
        {
            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < m_alCalcTBoxIDS.Count; i++)
            //{
            //    sb.Append("jQuery('#" + m_alCalcTBoxIDS[i].ToString() + "').bind('dblclick',javascript:ShowCalculator('" + m_alCalcTBoxIDS[i].ToString() + "'));");
            //    //sb.Append("jQuery('#" + m_alCalcTBoxIDS[i].ToString() + "').bind('dblclick', 'javascript:return ShowCalculator('" + m_alCalcTBoxIDS[i].ToString() + "')');");
            //    //sb.Append("jQuery('#" + m_alCalcTBoxIDS[i].ToString() + "').calculator({onClose:CalcClose,showOn: 'operator',layout:['MC_0_._=_+' + jQuery.calculator.CLOSE, 'MR_1_2_3_-' + jQuery.calculator.USE,'MS_4_5_6_*' + jQuery.calculator.ERASE, 'M+_7_8_9_/']});");
            //}
            return "";//sb.ToString();
        }


        public static string RegisterJQueryAutoComplete(Page page, Hashtable m_htAutoCompleteTBoxIDS,string strColAmount)
        {
            StringBuilder sb = new StringBuilder();
            IDictionaryEnumerator enumerator = m_htAutoCompleteTBoxIDS.GetEnumerator();
            while (enumerator.MoveNext())
            {
                sb.AppendFormat("AttachAutoComplete('{0}','{1}','{2}','{3}',true);", enumerator.Key, enumerator.Value.ToString().Trim(), page.Page.ToString(),strColAmount);
            }
            return sb.ToString();
        }
    }
}
