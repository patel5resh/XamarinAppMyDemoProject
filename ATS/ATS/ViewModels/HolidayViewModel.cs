using ATS.Data;
using ATS.Models;
using ATS.Services;
using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Extensions;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ATS.ViewModels
{
    public class HolidayViewModel : HolidayInfo, INotifyPropertyChanged
    {
        #region Attributes
        private NavigationService navigationService;
        private DialogService dialogService;
        private HolidayService holidayService;
        private bool isRefreshingAttendances = false;
        #endregion

        #region Property
        public ObservableCollection<HolidayViewModel> Holidays { get; set; }
        public bool IsRefreshingAttendances
        {
            set
            {
                if (isRefreshingAttendances != value)
                {
                    isRefreshingAttendances = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingAttendances"));
                }
            }
            get
            {
                return isRefreshingAttendances;
            }
        }

        private TouchGesture selectionGesture = TouchGesture.Hold;
        public TouchGesture SelectionGesture
        {
            get { return selectionGesture; }
            set
            {
                SetProperty(ref selectionGesture, value);
            }
        }
        #endregion

        #region Constructor
        public HolidayViewModel()
        {
            //Create observable collections
            Holidays = new ObservableCollection<HolidayViewModel>();

            //Instance services
            dialogService = new DialogService();
            navigationService = new NavigationService();
            holidayService = new HolidayService();
            HolidayDate = DateTime.Now.Date;
            LoadHoliday();
        }
        #endregion

        #region Command
        public ICommand NewHolidayCommand { get { return new RelayCommand(NewHoliday); } }
        public ICommand AddNewHolidayCommand { get { return new RelayCommand(NewAddHoliday); } }
        #endregion

        #region Method
        private async void LoadHoliday()
        {
            try
            {
                var holidays = holidayService.GetHoliday();                
                ReloadHoliday(holidays);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        private async void ReloadHoliday(List<HolidayInfo> holidays)
        {
            try
            {
                await Task.Delay(1000);
                Holidays.Clear();
                foreach (var holiday in holidays.OrderByDescending(d => d.HolidayDate))
                {
                    Holidays.Add(new HolidayViewModel
                    {
                        Id = holiday.Id,
                        HolidayDate = holiday.HolidayDate,
                        Description = holiday.Description,
                    });
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        public async void NewHoliday()
        {
            await navigationService.Navigate("AddHoliday");
        }

        private async void NewAddHoliday()
        {
            var holiday = new HolidayInfo()
            {
                HolidayDate = HolidayDate.Date,
                Description = Description.Trim(),
                Aph = "Holiday",
            };
            try
            {
                using (var da = new DataAccess())
                {
                    da.Insert(holiday);
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
            HolidayDate = DateTime.Now.Date;
            Description = "";
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T backfield, T value,
            [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backfield, value))
            {
                return false;
            }
            backfield = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion
    }
}
