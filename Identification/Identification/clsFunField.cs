using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identification
{
    class clsFunField
    {
        //  only field name
        /// <summary>
        /// input field with type
        /// </summary>
        /// <param name="strFieldNm"></param>
        /// <returns>field name</returns>
        public string fieldname(string strFieldNm)
        {
            string strFieldName = strFieldNm.Substring(0, strFieldNm.IndexOf("[")).Trim();
            return strFieldName;
        }

    }
}
