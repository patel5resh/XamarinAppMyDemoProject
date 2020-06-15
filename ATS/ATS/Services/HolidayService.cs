using ATS.Data;
using ATS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATS.Services
{
    public class HolidayService
    {
        public List<HolidayInfo> GetHoliday(int YearText, int MonthText)
        {
            using (var da = new DataAccess())
            {
                return da.GetList<HolidayInfo>(true)
                    .Where(a => a.HolidayDate.Year == YearText &&
                                a.HolidayDate.Month == MonthText)
                    .ToList();
            }
        }
        public bool DeleteHoliday(HolidayInfo model)
        {
            bool result = false;
            if (model.Id != 0)
            {
                using (var da = new DataAccess())
                {
                    da.Delete(model);
                }
                result = true;
            }
            return result;
        }
    }
}
