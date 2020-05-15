using ATS.Data;
using ATS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATS.Services
{
    public class AttendanceService
    {
        public List<AttendanceInfo> GetAttendances(int YearText, int MonthText)
        {
            using (var da = new DataAccess())
            {
                return da.GetList<AttendanceInfo>(true)
                    .Where(a => a.AttendanceDate.Year == YearText &&
                                a.AttendanceDate.Month == MonthText)
                    .ToList();
            }
        }

        public bool DeleteAttendance(AttendanceInfo model)
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

        public bool UpdateAttendance(AttendanceInfo model)
        {
            bool result = false;
            if (model.Id != 0)
            {
                using (var da = new DataAccess())
                {
                    da.Update(model);
                }
                result = true;
            }
            return result;
        }
    }
}
