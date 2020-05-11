using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATS.Models
{
    public class AttendanceInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string InTime { get; set; }

        public string OutTime { get; set; }

        public DateTime AttendanceDate { get; set; }

        public TimeSpan WorkingTime { get; set; }

        public string Aph { get; set; }
    }
}
