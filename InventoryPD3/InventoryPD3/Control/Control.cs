using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Control
{
    public class Control
    {
        const int Cod_Cliente_Len = 6;
      

        public bool ValidaBarcodeSN(string barcode)
        {
            var regexItem = new Regex("^[0-9 ]*$");
            if (
                ((barcode.Substring(0, 2)) == "21")
                && (barcode.Length == 9)               
                && (regexItem.IsMatch(barcode))
                )
            {
                return true;
            }
            else
            {
                return false;
            }

                
        }

       
       
    }
}
