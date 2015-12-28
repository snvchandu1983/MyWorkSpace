//This file contains the frequently called JS functions.

//IsSummed="1" BalanceLabel="ControlTotal" BalanceMethod="2"
function SumTotal(BalanceMethod,ColumnsTotal,ControlTotal)
{
    var Total;
   
    switch (BalanceMethod)
    {
    
    case "1":
           //(1)=sum of debits balance to control total &sum of DB+CR=0, 
           //alert(BalanceMethod +","+ ColumnsTotal +","+ ControlTotal);
          //Total=eval(ControlTotal)+eval(ColumnsTotal);
          // alert("Total"+Total);
          if(eval(ColumnsTotal) == eval(ControlTotal))
          {
             return true;
          } 
          else
          {
             return false;
          }
          break;
    
    case "2":
           //(2)=sum total + control total = 0, 
          // alert("ControlTotal"+ControlTotal);
          // alert("ColumnsTotal"+ColumnsTotal);
          if(eval(ColumnsTotal) == eval(ControlTotal))
          {
             return true;
             
          }
          else
          {
             return false;
          }
          break;
  
    case "3":
          //(3) sum total = control > total, 
          if(eval(ColumnsTotal) == eval(ControlTotal))
          {
             return true;
          }
          else
          {
             return false;
          }
          break;
    case 4:
          //(4) Sum nets to zero
          if(eval(ColumnsTotal) == 0)
          {
             return true;
          }
          else
          {
             return false;
          }
          break;      
    }
    
   //alert(return);
}

//...................................................................................................//

//Financial functions
// Note :
// ColumnSum function defination is in ChildGridView.js
 
 function ControlsTotal(gvClientID,columnIndex,BalanceMethod,BalanceLabel)
 {
     var hdnCurrAction = document.getElementById('ctl00_cphPageContents_BtnsUC_hdnCurrAction');
     //alert(hdnCurrAction.value);
     if(trim(hdnCurrAction.value.toUpperCase())=="ADD" || trim(hdnCurrAction.value.toUpperCase())=="MODIFY" || trim(hdnCurrAction.value.toUpperCase())=="CLONE")
     {
            //var table=document.getElementById(gvClientID);
            //alert(gvClientID+","+columnIndex+","+BalanceMethod+","+BalanceLabel);
            
            //alert(gvClientID+","+columnIndex+","+BalanceMethod+","+BalanceLabel);
            
            var controlname="ctl00_cphPageContents_txt"+BalanceLabel;
            var ColumnsTotal;
           // var ControlTotal=document.getElementById(controlname).value;
           var ControlTotal=document.getElementById(controlname).value.replace(/,/g,"");
             
           
            //alert(ControlTotal);
            
            if(ControlTotal != "")
            {
           
             // alert(ValidateDecimals(ControlTotal));
            
               if(ValidateDecimals(ControlTotal))
               {
                var Result;
                     ControlTotal=eval(ControlTotal);
                     
                    Result=GetResult(gvClientID,columnIndex,BalanceMethod,ControlTotal);
                   // alert("Result"+Result);
           
                    switch (Result)
                    {
                     case "0":
                               alert("Sum of control total is wrong");
                               return false;
                               break;
                     case "1":
                               return true;
                               break;
                     case "3":
                               alert("Please enter numbers only.");
                               return false;
                               break;
                     case "201":
                               alert("Credit and Debits are not equal");
                               return false;
                               break;
                      case "111":
                               alert("Please enter numbers only.");
                               return false;
                               break;         
                      case "222":
                               alert("Please enter two decimals only.");
                               return false;
                               break;                
                      default:
                               alert("Unable to evaluate"); 
                               return false;  
                               break;         
                    }
               
                }
                else
                {
                   alert("Please enter two decimals only."); 
                   return false;  
                }
            }
            else
            {
               return true;
            }
     }
     else
     {
        return true;
     }
  }    


//...................................................................................................//

//Get columnstotals 
//Note: SumTotal function defination in this page only
function GetResult(gvClientID,columnIndex,BalanceMethod,ControlTotal)
{
  var ColumnsTotal;
  var Result;
  var ErrorCode="0";
  var SumPositiveNegative=new Array();
  
  //Check Validations
  
 // alert('GetResult'+' '+gvClientID+' '+columnIndex+' '+BalanceMethod+' '+ControlTotal);
  
  ErrorCode=ValidateColumns(gvClientID,columnIndex);
  
  if (ErrorCode=="0")
  {
     //True  
    switch (BalanceMethod)
    {
     case "1":
            //returns array of Positive and Negative values
            SumPositiveNegative=ColumnSumPositiveNegative(gvClientID,columnIndex);
            if(SumPositiveNegative.length > 0)
            {
              ColumnsTotal=eval(SumPositiveNegative[0])+eval(SumPositiveNegative[1])
              if(ColumnsTotal==0)
              {
                 ColumnsTotal=SumPositiveNegative[0];
              }
              else
              {
                 ErrorCode="201";
              }
            }
            break;
     case "2":
            //returns the sum of controls
            ColumnsTotal=ColumnSum(gvClientID,columnIndex);
            break;  
     case "3":
            //returns the sum of controls
            ColumnsTotal=ColumnSum(gvClientID,columnIndex);
            break;  
     default:
            ColumnsTotal=ColumnSum(gvClientID,columnIndex);
            break;
    }
    
}
//else
//{  
//   //false invalid characters are entered
//   ErrorCode="3";
//}


   // alert(BalanceMethod+','+ColumnsTotal+','+ControlTotal);
    
    //Send this columntotal to formula
    if(ErrorCode=="0")
    {
       Result=SumTotal(BalanceMethod,ColumnsTotal,ControlTotal);
       if(Result)
       {
          //True       
          ErrorCode="1";
       }
       else
       {
          //False
          ErrorCode="0";
       }
    }
    return ErrorCode;
}

//...................................................................................................//

//Show Decimal places
function FilterAmount(objtxtamt)
{
    if(objtxtamt.value!="")
    {
      if(!isNaN(objtxtamt.value))
      {
         var finalamt=input_filterAmt(objtxtamt.value,2,1);
         objtxtamt.value=finalamt;
       }
    }
}

//...................................................................................................//

//return decimal value by passing parameter, amount, number of decimals, allow negatives.
function input_filterAmt(str, dec, bNeg)
{ // auto-correct input - force numeric data based on params.
	var cDec = '.'; // decimal point symbol
	var bDec = false; var val = "";
	var strf = ""; var neg = ""; var i = 0;
	if (str == "") return;
	parseFloat ("0").toFixed (dec);
	if (bNeg && str.charAt (i) == '-') { neg = '-'; i++; }
	for (i; i < str.length; i++)
	{
		val = str.charAt (i);
		if (val == cDec)
		{
			if (!bDec) { strf += val; bDec = true; }
		}
		else if (val >= '0' && val <= '9')
			strf += val;
	}
	strf = (strf == "" ? 0 : neg + strf);
	return parseFloat (strf).toFixed (dec);
}

//...................................................................................................//
