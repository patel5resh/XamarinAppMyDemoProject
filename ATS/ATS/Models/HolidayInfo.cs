using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATS.Models
{
    public class HolidayInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Description { get; set; }
        
        public DateTime HolidayDate { get; set; }

        public string Aph { get; set; }
    }
}
