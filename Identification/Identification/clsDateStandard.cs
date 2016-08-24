using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identification
{
    class clsDateStandard
    {
        #region gloabl
        string roz, mon, sal, tarikh;
        clsDateConvert clsdate = new clsDateConvert();
        #endregion
        public string time(string dtime)
        {
            if (dtime.Length < 10)
            {
                if (dtime.Length == 4 & dtime.IndexOf("/") == -1)
                {
                    tarikh = (Convert.ToInt32(dtime) + 621).ToString();
                }
                else if (dtime.PadLeft(4).IndexOf("/") == -1)
                {
                    tarikh = (Convert.ToInt32(dtime.PadLeft(4)) + 621).ToString();
                }
                else if (dtime.PadRight(4).IndexOf("/") == -1)
                {
                    tarikh = (Convert.ToInt32(dtime.PadRight(4)) + 621).ToString();
                }
            }
            else
            {
                roz = dtime.Substring(0, 2);
                mon = dtime.Substring(3, 2);
                sal = dtime.Substring(6, 4);

                if (Convert.ToInt32(sal) < 1300)
                {
                    tarikh = "-1";
                }
                if (Convert.ToInt32(mon) > 12 & Convert.ToInt32(roz) <= 12)
                {
                    tarikh = clsdate.ShamsiToMiladi(sal, roz, mon).ToShortDateString();
                }
                else
                {
                    tarikh = clsdate.ShamsiToMiladi(sal, mon, roz).ToShortDateString();
                }

            }
            return tarikh;
        }

        public string date(string Date)
        {

            return tarikh;
        }
    }
}
