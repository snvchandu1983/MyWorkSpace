using System;
using System.Collections.Generic;
using System.Text;
using LAjit_DAL;


namespace LAjit_BO
{
   public class MyReportsBO
    {
        private CommonDAL objDAL = new CommonDAL();

       /// <summary>
       
       /// </summary>
       /// <param name="inputXML"></param>
       /// <returns></returns>
        public string GetDataForReports(string inputXML)
        {
            return objDAL.GetBPEOut(inputXML);
        }
       /// <summary>
       /// Deleting Report
       /// </summary>
       /// <param name="inputXML"></param>
       /// <returns></returns>
       public string DeleteReport(string inputXML)
       {
           return objDAL.GetBPEOut(inputXML);
       }

    }
}
