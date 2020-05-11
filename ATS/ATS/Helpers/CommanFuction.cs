using System;
using System.Collections.Generic;
using System.Text;

namespace ATS.Helpers
{
    public class CommanFuction
    {
        public string CheckTimeForAM_PM(string TimeHours)
        {
            if (Convert.ToInt32(TimeHours) < 12)
            {
               return "AM";
            }
            else
            {
                return "PM";
            }
        }
    }
}
