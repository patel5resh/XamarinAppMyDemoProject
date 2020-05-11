using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATS.Models
{
    public class AttendanceSettingModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public TimeSpan LunchTime { get; set; }
    }
}
