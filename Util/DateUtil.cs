using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader.Util
{
    class DateUtil
    {   
        public static int calcDateDiff(string srcDate, string targetDate)
        {
            const int approximateDaysPerMonth = 30;
            int[] daysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            int y1 = getYear(srcDate);
            int m1 = getMonth(srcDate);
            int d1 = getDay(srcDate);

            int y2 = getYear(targetDate);
            int m2 = getMonth(targetDate);
            int d2 = getDay(targetDate);

            int monthRemainder = (y2 - y1) * 12;
            monthRemainder += m2 - m1;
            int dayRemainder = d2 - d1;

            if(monthRemainder >= 2)
            {
                dayRemainder += monthRemainder * approximateDaysPerMonth * monthRemainder;
            }
            else if(monthRemainder == 1)
            {
                if(m1 == 2 && isLeapYear(y1))
                {
                    dayRemainder += 29;
                }
                else
                {
                    dayRemainder += daysInMonth[m1 - 1];
                }
            }

            return dayRemainder;
        }

        protected static int getYear(string date)
        {
            if(date.Length < 8)
            {
                return 0;
            }
            string str = date.Substring(0, 4);
            return int.Parse(str);
        }

        protected static int getMonth(string date)
        {
            if (date.Length < 8)
            {
                return 0;
            }
            string str = date.Substring(4, 2);
            return int.Parse(str);
        }

        protected static int getDay(string date)
        {
            if (date.Length < 8)
            {
                return 0;
            }
            string str = date.Substring(6, 2);
            return int.Parse(str);
        }

        protected static bool isLeapYear(int year) // 是否是闰年
        {
            if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
            {
                return true;
            }

            return false;
        }

        public static string getDateToday()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        //public 

        public static string getDateMonthDay()
        {
            string date = getDateToday();
            return date.Substring(4, 4);
        }

        public static int compareDate(string srcDate, string targetDate)
        {
            int m1 = getMonth(srcDate);
            int d1 = getDay(srcDate);
            int m2 = getMonth(targetDate);
            int d2 = getDay(targetDate);

            if(m2 > m1)
            {
                return 1;
            }
            else if(m2 < m1)
            {
                return -1;
            }

            if(d2 > d1)
            {
                return 1;
            }
            else if(d2 < d1)
            {
                return -1;
            }

            return 0;
        }

        public static DateTime dateTime1970 = DateTime.Parse("1970-1-1");
        public static long getCurTimestamp()
        {
            TimeSpan ts = DateTime.Now - dateTime1970;
            return (long)ts.TotalMilliseconds;
        }
    }
}
