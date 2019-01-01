using System;
using System.Collections.Generic;
using System.Text;

namespace Control
{
    public class Control
    {
        public bool ValidaBarcodeSN(string barcode)
        {
            if (
                ((barcode.Substring(0, 2)) == "21") && 
                (barcode.Length == 9)                
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
