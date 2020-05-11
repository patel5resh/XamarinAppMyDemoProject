using ATS.Data;
using ATS.Helpers;
using ATS.Models;
using ATS.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace ATS.ViewModels
{
    public class AttendanceViewModel : AttendanceInfo, INotifyPropertyChanged
    {
        #region Property
        private DialogService dialogService;
        private NavigationService navigationService;
        private CommanFuction commanFuction;
        public TimeSpan inTime{ get; set; }
        public TimeSpan outTime { get; set; }

        /// <summary>
        /// Gets or sets a ImageSource that indicates Task's image. 
        /// </summary>
        private ImageSource selectedImage;
        public ImageSource SelectedImage
        {
            get
            {
                return selectedImage;
            }
            set
            {
                selectedImage = value;
                this.RaisePropertyChanged("SelectedImage");
            }
        }
        private bool isSelected;
        private bool isVisiable;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public bool IsVisiable
        {
            get { return isVisiable; }
            set
            {
                isVisiable = value;
                RaisePropertyChanged("IsVisiable");
            }
        }
        #endregion

        #region Constructor
        public AttendanceViewModel()
        {
            //Instance services
            dialogService = new DialogService();
            navigationService = new NavigationService();
            commanFuction = new CommanFuction();
            Clear();
        }
        #endregion

        #region Event
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(String name)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Command
        public ICommand AddNewAttendanceCommand { get { return new RelayCommand(NewAddAttendance); } }
        #endregion

        #region Method
        private async void NewAddAttendance()
        {
            //Get LunchTime
            TimeSpan LunchTime;
            using (var da = new DataAccess())
            {
                var lunchTime = da.First<AttendanceSettingModel>(true);
                LunchTime = lunchTime.LunchTime;
            }

            if (LunchTime != null)
            {
                //Calculation
                WorkingTime = outTime - inTime - LunchTime;
            }

            //Get Time Hour
            string inTimeHours = string.Format("{0:D2}", inTime.Hours);
            string outTimeHours = string.Format("{0:D2}", outTime.Hours);

            //Get AM PM
            string In_AM_PM = commanFuction.CheckTimeForAM_PM(inTimeHours);
            string Out_AM_PM = commanFuction.CheckTimeForAM_PM(outTimeHours);

            //Set String Time in DataBase Added
            string InTimes = string.Format("{0:D2}:{1:D2}:{2:D2}", inTime.Hours, inTime.Minutes,00) + " " + In_AM_PM;
            string OutTimes = string.Format("{0:D2}:{1:D2}:{2:D2}", outTime.Hours, outTime.Minutes,00) + " " + Out_AM_PM;
            
            var attendance = new AttendanceInfo()
            {
               AttendanceDate = AttendanceDate.Date,
               InTime = InTimes,
               OutTime = OutTimes,
               WorkingTime = WorkingTime,
               Aph = "Present",
            };
            try
            {
                using (var da = new DataAccess())
                {                   
                    da.Insert(attendance);
                    await dialogService.ShowMessage("Success", "Insert Data Successfully...!!!");
                    await navigationService.Back();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        public void Clear()
        {
            AttendanceDate = DateTime.Now.Date;
            inTime = DateTime.Now.TimeOfDay; 
            outTime = DateTime.Now.TimeOfDay;
        }
        #endregion
    }
}
