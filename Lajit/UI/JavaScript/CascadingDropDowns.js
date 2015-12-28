
    function FillSingLevelDropDowns(DropDownId1,DropDownId2,HiddenId1,HiddenId2)
    {
         var IndexValue = $get(DropDownId1).selectedIndex;
         var SelectedDataValue = $get(DropDownId1).options[IndexValue].value;
         FillNextDropDown(SelectedDataValue,DropDownId2,HiddenId1,HiddenId2);
    }
    
    //................................................................................................................................................................................................//
  
    //Filling dropdownlist Entry Account  
    function FillNextDropDown(DataField,DropDownId2,HiddenId1,HiddenId2)
    {
        var FinalData;
        var data;
        var subdata;
        var ddl1Status=false; 
        //clear dropdowndata  
        ClearDropdownData(DropDownId2);
        document.getElementById("ctl00_cphPageContents_hdnCOA").value="";
        //Setting COA value
        GetCOAValue(DataField,HiddenId1);
        //Assigning COA value
        var COA=document.getElementById("ctl00_cphPageContents_hdnCOA").value; 
        if(COA !="")
        {
            FinalData=document.getElementById(HiddenId2).value;
            FinalData=FinalData.split(";")
            for(i = 0; i < FinalData.length; i++)
            {
                data=FinalData[i];
                data=data.split(",");
                if(data.length >0)
                {
                    if(data[2]!=null)
                    {
                        //Get COA Value
                        subdata=data[2].split("=");
                        //COA value is equavalent to selected Source
                        if(subdata[1] == COA)
                        {
                            ddl1Status=true;  
                            var opt = document.createElement("option");
                            // Add an Option object to Drop Down/List Box
                            document.getElementById(DropDownId2).options.add(opt);
                            // Assign text and value to Option object
                            subdata=data[1].split("=");
                            opt.text = subdata[1];
                            subdata=data[0].split("=");
                            opt.value = subdata[1];
                            document.getElementById(DropDownId2).disabled=false;
                        }
                    }
                 }
                 if(ddl1Status==false)
                 {
                     document.getElementById(DropDownId2).disabled=true;
                 }
            }
        }
    }
   
  //-----------------------------------------------------------------------------------------------------------------------------------//
    
  // Fill Dropdowns based on selection used in Journal and Cash Journal pages
    function FillCascadingDropDowns(ControlID)
    {
        var ddlEntryJob=ControlID+"_ddlEntryJob";
        var IndexValue = $get(ddlEntryJob).selectedIndex;
        var SelectedDataValue = $get(ddlEntryJob).options[IndexValue].value;
        var SelectedVal = SelectedDataValue.split("~");
        var DataField;
        DataField= SelectedVal[0]; 
        var DropDownId2=ControlID+"_ddl"+"EntryAccount";
        var HiddenId1="ctl00_cphPageContents_hdn"+"EntryJob";
        var HiddenId2="ctl00_cphPageContents_hdn"+"EntryAccount";
        FillNextDropDown(SelectedDataValue,DropDownId2,HiddenId1,HiddenId2);
        //Fill dropdowns for SubAccount1 to 5
        for(k=1;k<=5;k++)
        { 
            var  colLabel="EntrySubAccount"+k;
            var  controlname=ControlID+"_ddlEntrySubAccount"+k;
            var  hdncontrol="ctl00_cphPageContents_hdn"+colLabel;
            var  FinalData;
            if(document.getElementById(controlname)!=null)
            {
                //clear dropdowndata
                ClearDropdownData(controlname);
                FillDropDowns(controlname,hdncontrol,DataField);
            } 
        } 
    }
 
  //................................................................................................................................................................................//
 
    // Clearing dropdown data  
    function ClearDropdownData(controlname)
    {
        //clearing data
        var e = document.getElementById(controlname);
        if(e.options.length > 0)
        {
            var count=1;
            for ( ; count < e.options.length ;)
            {   
                e.remove(count);
                count = 1;
            }
        }
    }
 
 //...................................................................................................//
 
    // Filling text and value in given dropdown control   
    function FillDropDowns(controlname,hdncontrolname,DataField)
    {
        var FinalData;
        FinalData=document.getElementById(hdncontrolname).value;
        FinalData=FinalData.split(";")
        var data;
        var subdata;
        for(i = 0; i < FinalData.length; i++)
        {
            data=FinalData[i];
            data=data.split(",");
            if(data.length >0)
            {
                if(data[2]!=null)
                {
                    //To get JobID value
                    subdata=data[2].split("=");
                    //JobID and Selected TrxID equalents items added to dropdownlist
                    if(subdata[1] == DataField)
                    {
                        var opt = document.createElement("option");
                        // Add an Option object to Drop Down/List Box
                        document.getElementById(controlname).options.add(opt);
                        // Assign text and value to Option object
                        subdata=data[1].split("=");
                        opt.text = subdata[1];
                        subdata=data[0].split("=");
                        opt.value = subdata[1];
                    }
                }
            }
        } 
        //For loop close
    }  
 
   //...................................................................................................//
 
   // Getting COA Value for selected dropdown list 
    function GetCOAValue(DataField,HiddenId1)
    {
        var Finaldata;
        FinalData=document.getElementById(HiddenId1).value;
        FinalData=FinalData.split(";");
        var data;
        var subdata;
        for(i = 0; i < FinalData.length; i++)
        {
            if(FinalData[i]!=null)
            {
                data=FinalData[i];
                data=data.split(",");
                if(data.length > 0)
                {
                    subdata=data[0].split("=");
                    if(subdata[1] == DataField)
                    {
                        subdata=data[2].split("=");
                        if(subdata.length > 0)
                        {
                            document.getElementById("ctl00_cphPageContents_hdnCOA").value=subdata[1];
                            return;
                        }
                    }
                }
            }        
        }
    }

//...................................................................................................//


















