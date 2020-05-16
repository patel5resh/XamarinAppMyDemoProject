using ATS.Data;
using ATS.Models;
using ATS.Services;
using ATS.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms.Xaml;

namespace ATS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAttendance : PopupPage
    {
        AttendanceInfo model = new AttendanceInfo();
        private NavigationService navigationService;
        private AttendanceService attendanceService;
        public AddAttendance(AttendanceInfo details)
        {
            InitializeComponent();
            navigationService = new NavigationService();
            attendanceService = new AttendanceService();
            BindingContext = new MainViewModel();
            if (details != null)
            {
                model = details;
                EditAttendance(model);
            }            
        }

        private void EditAttendance(AttendanceInfo modelAttendance)
        {
            dpPopup.Date = modelAttendance.AttendanceDate;

            string[] InTimeValues = model.InTime.Split(':');
            string[] OutTimeValues = model.OutTime.Split(':');

            TimeSpan InTime = new TimeSpan(Convert.ToInt32(InTimeValues[0]), Convert.ToInt32(InTimeValues[1]), 0);
            IntpPopup.Time = InTime;

            TimeSpan OutTime = new TimeSpan(Convert.ToInt32(OutTimeValues[0]), Convert.ToInt32(OutTimeValues[1]), 0);
            OuttpPopup.Time = OutTime;
        }

        private async void EditAttendce_Cliked(object sender, EventArgs e)
        {
            //Update Attendance
            string In_AM_PM;
            string Out_AM_PM;

            string Inhour = IntpPopup.Time.ToString("hh");
            string Inminute = IntpPopup.Time.ToString("mm");
            string Insencond = IntpPopup.Time.ToString("ss");

            string Outhour = OuttpPopup.Time.ToString("hh");
            string Outminute = OuttpPopup.Time.ToString("mm");
            string Outsencond = OuttpPopup.Time.ToString("ss");

            if (Convert.ToInt32(Inhour) < 12)
            {
                In_AM_PM = "AM";
            }
            else
            {
                In_AM_PM = "PM";
            }
            TimeSpan Ints = new TimeSpan(Convert.ToInt32(Inhour), Convert.ToInt32(Inminute), Convert.ToInt32(Insencond));

            if (Convert.ToInt32(Outhour) < 12)
            {
                Out_AM_PM = "AM";
            }
            else
            {
                Out_AM_PM = "PM";
            }

            TimeSpan LunchTime;
            using (var da = new DataAccess())
            {
                var lunchTime = da.First<AttendanceSettingModel>(true);
                LunchTime = lunchTime.LunchTime;
            }

            TimeSpan Outts = new TimeSpan(Convert.ToInt32(Outhour), Convert.ToInt32(Outminute), Convert.ToInt32(Outsencond));
            TimeSpan ts = Outts - Ints - LunchTime;

            model.AttendanceDate = dpPopup.Date;
            model.InTime = Ints + " " + In_AM_PM;
            model.OutTime = Outts + " " + Out_AM_PM;
            model.WorkingTime = ts;

            var result = attendanceService.UpdateAttendance(model);
            if (result)
            {
                await DisplayAlert("Save", "Attendance successfully saved", "Ok");
                await Navigation.PopPopupAsync();
                await navigationService.Navigate("AttendanceList");
            }
            else
            {
                await DisplayAlert("Error", "Something is wrong", "Ok");
            }
        }

        //private async void InsertAttend_Cliked(object sender, EventArgs e)
        //{
        //    if (model.Id == 0)
        //    {
        //        String In_AM_PM;
        //        String Out_AM_PM;

        //        string Inhour = IntpPopup.Time.ToString("hh");
        //        string Inminute = IntpPopup.Time.ToString("mm");
        //        string Insencond = IntpPopup.Time.ToString("ss");

        //        string Outhour = OuttpPopup.Time.ToString("hh");
        //        string Outminute = OuttpPopup.Time.ToString("mm");
        //        string Outsencond = OuttpPopup.Time.ToString("ss");

        //        if (Convert.ToInt32(Inhour) < 12)
        //        {
        //            In_AM_PM = "AM";
        //        }
        //        else
        //        {
        //            In_AM_PM = "PM";
        //        }
        //        TimeSpan Ints = new TimeSpan(Convert.ToInt32(Inhour), Convert.ToInt32(Inminute), Convert.ToInt32(Insencond));

        //        if (Convert.ToInt32(Outhour) < 12)
        //        {
        //            Out_AM_PM = "AM";
        //        }
        //        else
        //        {
        //            Out_AM_PM = "PM";
        //        }

        //        var data = attendanceSettingRepository.GetAllAttendancesSettingData();
        //        TimeSpan LunchTime = data[0].LunchTime;

        //        TimeSpan Outts = new TimeSpan(Convert.ToInt32(Outhour), Convert.ToInt32(Outminute), Convert.ToInt32(Outsencond));
        //        TimeSpan ts = Outts - Ints - LunchTime;

        //        model.AttendanceDate = dpPopup.Date;
        //        model.InTime = Ints + " " + In_AM_PM;
        //        model.OutTime = Outts + " " + Out_AM_PM;
        //        model.WorkingTime = ts;
        //        bool response = attendanceRepository.InsertAttendance(model);
        //        if (response)
        //        {
        //            await DisplayAlert("Save", "Attendance successfully saved", "Ok");
        //            await Navigation.PopPopupAsync();
        //        }
        //        else
        //        {
        //            await DisplayAlert("Save", "Attendance failed to saved", "Ok");
        //        }
        //        Clear();
        //    }
        //    else
        //    {
        //        //Update Attendance
        //        string In_AM_PM;
        //        string Out_AM_PM;

        //        string Inhour = IntpPopup.Time.ToString("hh");
        //        string Inminute = IntpPopup.Time.ToString("mm");
        //        string Insencond = IntpPopup.Time.ToString("ss");

        //        string Outhour = OuttpPopup.Time.ToString("hh");
        //        string Outminute = OuttpPopup.Time.ToString("mm");
        //        string Outsencond = OuttpPopup.Time.ToString("ss");

        //        if (Convert.ToInt32(Inhour) < 12)
        //        {
        //            In_AM_PM = "AM";
        //        }
        //        else
        //        {
        //            In_AM_PM = "PM";
        //        }
        //        TimeSpan Ints = new TimeSpan(Convert.ToInt32(Inhour), Convert.ToInt32(Inminute), Convert.ToInt32(Insencond));

        //        if (Convert.ToInt32(Outhour) < 12)
        //        {
        //            Out_AM_PM = "AM";
        //        }
        //        else
        //        {
        //            Out_AM_PM = "PM";
        //        }

        //        var data = attendanceSettingRepository.GetAllAttendancesSettingData();
        //        TimeSpan LunchTime = data[0].LunchTime;

        //        TimeSpan Outts = new TimeSpan(Convert.ToInt32(Outhour), Convert.ToInt32(Outminute), Convert.ToInt32(Outsencond));
        //        TimeSpan ts = Outts - Ints - LunchTime;

        //        model.AttendanceDate = dpPopup.Date;
        //        model.InTime = Ints + " " + In_AM_PM;
        //        model.OutTime = Outts + " " + Out_AM_PM;
        //        model.WorkingTime = ts;

        //        bool response = attendanceRepository.UpdateAttendance(model);
        //        if (response)
        //        {
        //            await Navigation.PushAsync(new AttendanceList());
        //        }
        //        else
        //        {
        //            await DisplayAlert("Update", "Attendance failed to updated", "Ok");
        //        }
        //    }
        //}
        //public void Clear()
        //{
        //    dpPopup.Date = DateTime.Now.Date;
        //    IntpPopup.Time = DateTime.Now.TimeOfDay;
        //    OuttpPopup.Time = DateTime.Now.TimeOfDay;
        //}
    }
}