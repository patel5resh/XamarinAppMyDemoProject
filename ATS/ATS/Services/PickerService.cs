using ATS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATS.Services
{
    public class PickerService
    {
        public static List<Year> GetYear()
        {
            int i = 1;
            var yearList = new List<Year>();
            for (int intCount = DateTime.Now.Year; intCount >= 2000; intCount--, i++)
            {
                yearList.Add(new Year() { Key = i, Value = intCount.ToString() });
            }
            return yearList;
        }

        public static List<Month> GetMonth()
        {
            var monthList = new List<Month>()
            {
                new Month() {Key=1, Value="JAN"},
                new Month() {Key=2, Value="FEB"},
                new Month() {Key=3, Value="MAR"},
                new Month() {Key=4, Value="APR"},
                new Month() {Key=5, Value="MAY"},
                new Month() {Key=6, Value="JUN"},
                new Month() {Key=7, Value="JUL"},
                new Month() {Key=8, Value="AUG"},
                new Month() {Key=9, Value="SEP"},
                new Month() {Key=10, Value="OCT"},
                new Month() {Key=11, Value="NOV"},
                new Month() {Key=12, Value="DEC"}
            };
            return monthList;
        }
    }
}
