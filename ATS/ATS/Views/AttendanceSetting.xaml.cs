using ATS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ATS.Models;
using ATS.Data;
using System.Text.RegularExpressions;

namespace ATS.Views
{
    public partial class AttendanceSetting : ContentPage
    {
        AttendanceSettingModel model = new AttendanceSettingModel();
        public AttendanceSetting()
        {
            InitializeComponent();
            ShowData();
        }
        public void ShowData()
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var data = da.GetList<AttendanceSettingModel>(true).FirstOrDefault();
                    if (data != null)
                    {
                        string LunchTime = data.LunchTime.ToString();
                        txtLunchTime.Text = LunchTime;
                    }
                    else
                    {
                        string LunchTime = "00:00:00";
                        txtLunchTime.Text = LunchTime;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void btnSumbit_Clicked(object sender, EventArgs e)
        {
            Regex rgx = new Regex(@"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$");

            using (var da = new DataAccess())
            {
                var data = da.GetList<AttendanceSettingModel>(true).FirstOrDefault();

                string LTimes = txtLunchTime.Text.ToString();

                if (rgx.IsMatch(LTimes))
                {
                    if (data != null)
                    {
                        da.Delete<AttendanceSettingModel>(data);
                    }

                    string[] lunchTimes = LTimes.Split(':');

                    TimeSpan LunchTime = new TimeSpan(Convert.ToInt16(lunchTimes[0]), Convert.ToInt16(lunchTimes[1]), 0);
                    model.LunchTime = LunchTime;
                    bool response = false;
                    try
                    {
                        da.Insert<AttendanceSettingModel>(model);
                        response = true;
                    }
                    catch
                    {
                        response = false;
                    }
                    if (response)
                    {
                        await DisplayAlert("Save", "Lunch Time successfully saved", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Save", "Lunch time failed to saved", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Lunch time Formate accept only 'HH:MM:SS'", "Ok");
                }
            }
        }
    }
}