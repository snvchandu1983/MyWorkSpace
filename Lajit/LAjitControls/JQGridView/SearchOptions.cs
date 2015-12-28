using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace LAjitControls.JQGridView
{
    public class SearchOptions
    {
        private Hashtable m_htFieldList;

        /// <summary>
        /// This option is valid only for the elements of type select - i.e stype:'select'. 
        /// The option represent the url from where we load the select element. 
        /// When this option is set the element will be filled with values from the ajax request. 
        /// The data should be a valid html select element with the desired options. 
        /// By example the request should contain <select><option value=“1”>One</option> <option value=“2”>Two</option></select>.
        /// This is called only once.
        /// </summary>
        public string DataUrl
        {
            get { return Convert.ToString(m_htFieldList["dataUrl"]); }
            set { m_htFieldList["dataUrl"] = value; }
        }

        /// <summary>
        /// Accepts a Javascript function.If set this function is called only once when the element is created. To this function we pass the element object.
        /// dataInit: function(elem) {
        ///             do something }
        /// Also use this function to attach datepicker, time picker and etc. Example:
        /// dataInit : function (elem) {
        /// $(elem).datepicker();}
        /// </summary>
        public string DataInit
        {
            get { return Convert.ToString(m_htFieldList["dataInit"]); }
            set { m_htFieldList["dataInit"] = value; }
        }

        /// <summary>
        /// Return Type is string for now. TODO: Class for DataEvents.
        /// List of events to apply to the data element; uses $(”#id”).bind(type, [data], fn) to bind events to data element. Should be described like this:
        /// dataEvents: [
        /// { type: 'click', data: { i: 7 }, fn: function(e) { console.log(e.data.i); }},
        /// { type: 'keypress', fn: function(e) { console.log('keypress'); } }
        /// ]
        /// </summary>
        public string DataEvents
        {
            get { return Convert.ToString(m_htFieldList["dataEvents"]); }
            set { m_htFieldList["dataEvents"] = value; }
        }

        /// <summary>
        /// attr is object where we can set valid attributes to the created element. By example:
        /// attr : { title: “Some title” }
        /// Will set a title of the searched element
        /// </summary>
        public string Attr
        {
            get { return Convert.ToString(m_htFieldList["attr"]); }
            set { m_htFieldList["attr"] = value; }
        }

        /// <summary>
        /// By default hidden elemnts in the grid are not searchable. 
        /// In order to enable searching when the field is hidden set this option to true
        /// </summary>
        public bool SearchHidden
        {
            get { return Convert.ToBoolean(m_htFieldList["searchhidden"]); }
            set { m_htFieldList["searchhidden"] = value; }
        }

        /// <summary>
        /// This option is used only in advanced single field searching and determines the operation that is applied to the element. If not set all the available options will be used. All available option are:
        /// ['eq','ne','lt','le','gt','ge','bw','bn','in','ni','ew','en','cn','nc']
        /// The corresponding texts are in language file and mean the following:
        /// ['equal','not equal', 'less', 'less or equal','greater','greater or equal', 'begins with','does not begin with','is in','is not in','ends with','does not end with','contains','does not contain']
        /// \nNote that the elements in sopt array can be mixed in any order.
        /// </summary>
        public string[] Sopt
        {
            get { return (string[])m_htFieldList["sopt"]; }
            set { m_htFieldList["sopt"] = value; }
        }

        /// <summary>
        /// If not empty set a default value in the search input element.
        /// </summary>
        public string DefaultValue
        {
            get { return Convert.ToString(m_htFieldList["defaultValue"]); }
            set { m_htFieldList["defaultValue"] = value; }
        }

        /// <summary>
        /// The option is used only for stype select and defines the select options in the search dialogs.
        /// When set for stype select and dataUrl option is not set, the value can be a string or object.
        /// If the option is a string it must contain a set of value:label pairs with the value separated from the label with a colon (:) and ended with(;). 
        /// The string should not ended with a (;)- editoptions:{value:“1:One;2:Two”}.
        /// If set as object it should be defined as pair name:value - editoptions:{value:{1:'One';2:'Two'}} 
        /// </summary>
        public string Value
        {
            get { return Convert.ToString(m_htFieldList["value"]); }
            set { m_htFieldList["value"] = value; }
        }

        public SearchOptions()
        {
            m_htFieldList = new Hashtable();
        }

        public override string ToString()
        {
            return GridHelper.ToCSV(m_htFieldList);
        }
    }
}
