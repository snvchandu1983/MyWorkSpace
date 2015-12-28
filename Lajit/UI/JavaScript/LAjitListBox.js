// Selecting Multiple Items in a Listbox w/o pressing control Key
var arrOldValues;
function FillListValues(CONTROL)
{
     var arrNewValues;
    var intNewPos;
    var strTemp = GetSelectValues(CONTROL);
    arrNewValues = strTemp.split(",");
    for(var i=0;i<arrNewValues.length-1;i++){
    if(arrNewValues[i]==1){
    intNewPos = i;
    }
    }

    for(var i=0;i<arrOldValues.length-1;i++){
    if(arrOldValues[i]==1 && i != intNewPos){
    CONTROL.options[i].selected= true;
    }
    else if(arrOldValues[i]==0 && i != intNewPos){
    CONTROL.options[i].selected= false;
    }

    if(arrOldValues[intNewPos]== 1){
    CONTROL.options[intNewPos].selected = false;
    }
    else{
    CONTROL.options[intNewPos].selected = true;
    }
    }
}

//...................................................................................................//


function GetSelectValues(CONTROL)
{
    var strTemp = "";
    for(var i = 0;i < CONTROL.length;i++)
    {
          if(CONTROL.options[i].selected == true )
          {
             if(CONTROL.options[i].innerHTML!='Selection Required')
             {
               strTemp += "1,";
             }
            }
            else
            {
              strTemp += "0,";
             }
      }
      return strTemp;
  }

//...................................................................................................//

function GetCurrentListValues(CONTROL){
    var strValues = "";
    strValues = GetSelectValues(CONTROL);
    arrOldValues = strValues.split(",")
}

//...................................................................................................//