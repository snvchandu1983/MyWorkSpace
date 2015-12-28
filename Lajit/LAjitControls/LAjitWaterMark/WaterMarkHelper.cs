using System;
using System.Collections.Generic;
using System.Text;

namespace LAjitControls.LAjitWaterMark
{
    public class WaterMarkHelper
    {
        public static void ApplyWaterMarkToTextBox(System.Web.UI.WebControls.TextBox textBox, string waterMarkText, string waterMarkStyle, string normalStyle)
        {
            textBox.Attributes.Add("OnFocus", "javascript:js_waterMark_Focus('" + textBox.ClientID + "', '" + waterMarkText.Replace("'", "\'") + "','" + waterMarkStyle + "', '" + normalStyle + "')");
            textBox.Attributes.Add("OnBlur", "javascript:js_waterMark_Blur('" + textBox.ClientID + "', '" + waterMarkText.Replace("'", "\'") + "','" + waterMarkStyle + "', '" + normalStyle + "')");
            if (textBox.Text.Trim().Length == 0 || textBox.Text== waterMarkText)
            {
                textBox.Text = waterMarkText;
                textBox.CssClass = waterMarkStyle;
            }
            else
            {
                textBox.CssClass = normalStyle;
            }
            if (!textBox.Page.ClientScript.IsClientScriptBlockRegistered("WaterMarkScript"))
            {
                System.Text.StringBuilder scriptBuilder = new System.Text.StringBuilder();
                scriptBuilder.Append("<script language = \"javascript\">" + System.Environment.NewLine);
                scriptBuilder.Append("  function js_waterMark_Focus(objname, waterMarkText, waterMarkStyle, normalStyle)" + System.Environment.NewLine);
                scriptBuilder.Append("  {" + System.Environment.NewLine);
                scriptBuilder.Append("      obj = document.getElementById(objname);" + System.Environment.NewLine);
                scriptBuilder.Append("      if(obj.value == waterMarkText)" + System.Environment.NewLine);
                scriptBuilder.Append("      {" + System.Environment.NewLine);
                scriptBuilder.Append("          obj.value=\"\";" + System.Environment.NewLine);
                scriptBuilder.Append("          obj.className = normalStyle" + System.Environment.NewLine);
                scriptBuilder.Append("      }" + System.Environment.NewLine);
                scriptBuilder.Append("  }" + System.Environment.NewLine);
                scriptBuilder.Append("  function js_waterMark_Blur(objname, waterMarkText, waterMarkStyle, normalStyle)" + System.Environment.NewLine);
                scriptBuilder.Append("  {" + System.Environment.NewLine);
                scriptBuilder.Append("      obj = document.getElementById(objname);" + System.Environment.NewLine);
                scriptBuilder.Append("      if(obj.value == \"\")" + System.Environment.NewLine);
                scriptBuilder.Append("      {" + System.Environment.NewLine);
                scriptBuilder.Append("  	    obj.value=waterMarkText;" + System.Environment.NewLine);
                scriptBuilder.Append("          obj.className = waterMarkStyle" + System.Environment.NewLine);
                scriptBuilder.Append("      }" + System.Environment.NewLine);
                scriptBuilder.Append("      else" + System.Environment.NewLine);
                scriptBuilder.Append("      {" + System.Environment.NewLine);
                scriptBuilder.Append("          obj.className = normalStyle" + System.Environment.NewLine);
                scriptBuilder.Append("      }" + System.Environment.NewLine);
                scriptBuilder.Append("  }" + System.Environment.NewLine);
                scriptBuilder.Append("</script>" + System.Environment.NewLine);
                textBox.Page.ClientScript.RegisterClientScriptBlock(textBox.Page.GetType(), "WaterMarkScript", scriptBuilder.ToString(), false);
            }
        }
    }
}
