using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identification
{
    class clsDateConvert
    {
        public string MiladiToShamsi(DateTime miladiDate)
        {
            PersianCalendar pc = new PersianCalendar();
            StringBuilder sb = new StringBuilder();
            sb.Append(pc.GetYear(miladiDate).ToString("0000"));
            sb.Append("/");
            sb.Append(pc.GetMonth(miladiDate).ToString("00"));
            sb.Append("/");
            sb.Append(pc.GetDayOfMonth(miladiDate).ToString("00"));
            return sb.ToString();

        }


        public DateTime ShamsiToMiladi(string sal, string mah, string roz)
        {
            DateTime miladi;
            PersianCalendar pc = new PersianCalendar();

            for (int i = 1299; i < 1411; i = i + 4)
            {
                if (Convert.ToInt32(sal) == i)
                {
                    if (Convert.ToInt32(mah) == 12 & Convert.ToInt32(roz) > 29)
                    {
                        roz = "29";
                        break;
                    }
                }
            }

            miladi = pc.ToDateTime(Convert.ToInt32(sal), Convert.ToInt32(mah), Convert.ToInt32(roz), 0, 0, 0, 0);
            return miladi;

        }

        public void kabise(string sal, string mah, string roz)
        {

        }
    }
}
