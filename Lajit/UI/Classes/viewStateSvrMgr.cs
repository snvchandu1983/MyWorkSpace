using System;

using System.Collections;
using System.Configuration;
using app = LAjitDev.Classes.AppGlobal;


namespace LAjitDev.Classes
{
	public class viewStateSvrMgr
	{
		// This uses an array and mod to cycle repeatedly through an array (so limited size)

		//WARNING:  When the user uses the "back" button on the browser, IE will not rerequest the page, so 
		// if the user posts again they need the viewstate to still be present on the server.  Need to set the VIEW_STATE_NUM_PAGES
		// to a tradeoff of number of back pages allowed and 
		// the amount of memory consumed by the viewstate kept per page.  
		private const short VIEW_STATE_NUM_PAGES = 1;		//Number of pages to keep viewstate for

		//Name of storage location for viewstate information
	 //	private const string SESSION_VIEW_STATE_MGR = "VIEW_STATE_MGR1";
    //private  static string SESSION_VIEW_STATE_MGR=System.Web.HttpContext.Current.Session.SessionID.ToString();
		private long lPageCount = 0;	//Number of pages seen by this customer 
		private string[] ViewStates = new string[VIEW_STATE_NUM_PAGES];	//Store for viewstates

		//Determine if server side is enabled or not from web.config file
		public bool ServerSideEnabled
		{
			get
			{
				//Not a problem if someone changes the value in web.config, because new AppDomain will
				// be started and all in process session is lost anyway
				 bool test= Convert.ToBoolean(ConfigurationSettings.AppSettings["ServerSideViewState"]);
				return test;
			}
		}

		public static string ViewStateStore
		{
			get
			{
        string viewStateStore=Convert.ToString(ConfigurationSettings.AppSettings["ViewStateStore"]);
               return viewStateStore;
			}

		}

		public viewStateSvrMgr()
		{
		}

		public long SaveViewState(string szViewState)
		{
			//Increment the total page seen counter
			lPageCount++;

			//Now use the modulas operator (%) to find remainder of that and size of viewstate storage, this creates a
			// circular array where it continually cycles through the array index range (effectively keeps
			// the last requests to match size of storage)
			short siIndex = (short)(lPageCount % VIEW_STATE_NUM_PAGES);

			//Now save the viewstate for this page to the current position.  
			ViewStates[siIndex] = szViewState;

			return lPageCount;
		}


		public string GetViewState(long lRequestNumber)
		{
			//Could cycle though the array and make sure that the given request number is actually
			// present (in case the array is not big enough).  Much faster to just take the
			// given request number and recalculate where it should be stored
			short siIndex = (short)(lRequestNumber % VIEW_STATE_NUM_PAGES);

			return ViewStates[siIndex];
		}


		public static viewStateSvrMgr GetViewStateSvrMgr()
		{
			viewStateSvrMgr oViewStateMgr=null;
	
    // SESSION
			if(viewStateSvrMgr.ViewStateStore=="session")
			{
				if (null == System.Web.HttpContext.Current.Session[System.Web.HttpContext.Current.Session.SessionID.ToString()])				
				{
					oViewStateMgr = new viewStateSvrMgr();
					System.Web.HttpContext.Current.Session[System.Web.HttpContext.Current.Session.SessionID.ToString()] = oViewStateMgr;
				}
				else
				{
					oViewStateMgr = (viewStateSvrMgr)System.Web.HttpContext.Current.Session[System.Web.HttpContext.Current.Session.SessionID.ToString()];				
				}
			}
			
			// APPLICATION
			if(viewStateSvrMgr.ViewStateStore=="application")
			{
			
				if (null == System.Web.HttpContext.Current.Application[System.Web.HttpContext.Current.Session.SessionID.ToString()])
				{					
					oViewStateMgr = new viewStateSvrMgr();				
					System.Web.HttpContext.Current.Application[System.Web.HttpContext.Current.Session.SessionID.ToString()]=oViewStateMgr;
				}
				else
				{				
					oViewStateMgr=(viewStateSvrMgr)System.Web.HttpContext.Current.Application[System.Web.HttpContext.Current.Session.SessionID.ToString()];
				}
			}
	
       //APPGLOBALS
			if(viewStateSvrMgr.ViewStateStore=="appglobals")
			{
			
			if(null==app.MySettings[System.Web.HttpContext.Current.Session.SessionID.ToString()])					
				{					
					oViewStateMgr = new viewStateSvrMgr();				
					 app.MySettings[System.Web.HttpContext.Current.Session.SessionID.ToString()]=oViewStateMgr;				
				}
				else
				{
					oViewStateMgr=(viewStateSvrMgr)app.MySettings[System.Web.HttpContext.Current.Session.SessionID.ToString()];		
				}
			}

			//CACHE

			if(viewStateSvrMgr.ViewStateStore=="cache")
			{
			
				if(null==System.Web.HttpContext.Current.Cache[System.Web.HttpContext.Current.Session.SessionID.ToString()])					
				{					
					oViewStateMgr = new viewStateSvrMgr();				
					System.Web.HttpContext.Current.Cache[System.Web.HttpContext.Current.Session.SessionID.ToString()]=oViewStateMgr;				
				}
				else
				{
					oViewStateMgr=(viewStateSvrMgr)System.Web.HttpContext.Current.Cache[System.Web.HttpContext.Current.Session.SessionID.ToString()];		
				}
			}








			return oViewStateMgr;
		}
	}
}
