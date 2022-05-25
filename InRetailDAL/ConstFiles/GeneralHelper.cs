using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.ConstFiles
{
    public class GeneralHelper
    {
        public static bool IsDefaultDate(DateTime date)
        {
            bool flag = false;
            if (date.Month == 1 && date.Day == 1 && date.Year == 1)
                flag = true;
            return flag;
        }

        public static string GetRandomPassword()
        {
            Random r = new Random();
            var x = r.Next(ConstHelper.PWD_RANDOM_MIN_RANGE, ConstHelper.PWD_RANDOM_MAX_RANGE);
            string s = x.ToString(ConstHelper.PWD_STRING);

            return s;
        }

        public static string GetRandomOTP()
        {
            Random r = new Random();
            var x = r.Next(ConstHelper.CODE_RANDOM_MIN_RANGE, ConstHelper.CODE_RANDOM_MAX_RANGE);
            string s = x.ToString(ConstHelper.CODE_STRING);

            return s;
        }
    }
}
