using System;
using System.Collections.Generic;
using System.Text;

namespace LAjitControls
{
    public class LAjitImageButton :  System.Web.UI.WebControls.ImageButton
    {
        private string m_BPGID;
        private string m_MapBranchNode;
        private string m_MapXML;


        public string BPGID
        {
            get { return m_BPGID; }
            set { m_BPGID = value; }
        }

        public string MapXML
        {
            get { return m_MapXML; }
            set { m_MapXML = value; }
        }

        public string MapBranchNode
        {
            get { return m_MapBranchNode; }
            set { m_MapBranchNode = value; }
        }

    }
}
