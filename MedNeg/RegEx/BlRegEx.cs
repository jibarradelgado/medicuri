using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MedNeg.RegEx
{
    public class BlRegEx
    {

        /// <summary>
        /// Regular expression, which is used to validate an E-Mail address.
        /// </summary>
        public const string MatchEmailPattern =
                  @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
           + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
           + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
           + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";


        public bool ValidarCorreo(string sCorreo)
        {
            if (sCorreo != null) 
                return Regex.IsMatch(sCorreo, MatchEmailPattern);
            else 
                return false;
        }

    }
}
