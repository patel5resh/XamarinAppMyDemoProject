using ATS.Data;
using ATS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATS.Services
{
    public class HolidayService
    {
        public List<HolidayInfo> GetHoliday()
        {
            using (var da = new DataAccess())
            {
                return da.GetList<HolidayInfo>(true);
            }
        }

    }
}
