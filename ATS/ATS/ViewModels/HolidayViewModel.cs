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
        readonly int Month = DateTime.Now.Month - 1;
        private Month _selectedMonth;
        private Year _selectedYear;
        private NavigationService navigationService;
        private DialogService dialogService;
        private HolidayService holidayService;
        private bool isRefreshingHoliday = false;
        private int flag = 0;
        private int Count = 0;
        private int TotalCount = 0;
        private int AllChecked = 0;
        private int IsEdit = 0;
        #endregion

        #region Property
        public List<Year> ListYears { get; set; }
        public List<Month> ListMonths { get; set; }

        public ObservableCollection<HolidayViewModel> Holidays { get; set; }
        //public bool IsRefreshingHoliday
        //{
        //    set
        //    {
        //        if (isRefreshingHoliday != value)
        //        {
        //            isRefreshingHoliday = value;
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingHoliday"));
        //        }
        //    }
        //    get
        //    {
        //        return isRefreshingHoliday;
        //    }
        //}

        //private bool footerIsVisible = false;
        //public bool FooterIsVisible
        //{
        //    set
        //    {
        //        footerIsVisible = value;
        //        OnPropertyChanged();
        //    }
        //    get
        //    {
        //        return footerIsVisible;
        //    }
        //}

        //private string selectedItemCount = "HolidayList";
        //public string SelectedItemCount
        //{
        //    get { return selectedItemCount; }
        //    set
        //    {
        //        SetProperty(ref selectedItemCount, value);
        //    }
        //}

        //private TouchGesture selectionGesture = TouchGesture.Hold;
        //public TouchGesture SelectionGesture
        //{
        //    get { return selectionGesture; }
        //    set
        //    {
        //        SetProperty(ref selectionGesture, value);
        //    }
        //}
        //public Year SelectedYear
        //{
        //    set
        //    {
        //        _selectedYear = value;

        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedYear"));
        //        //put here your code
        //        YearText = Convert.ToInt32(_selectedYear.Value);
        //        LoadHoliday();
        //    }
        //    get { return _selectedYear; }
        //}
        //private int _yearText;
        //public int YearText
        //{
        //    get { return _yearText; }
        //    set
        //    {
        //        SetProperty(ref _yearText, value);
        //    }
        //}
        //public Month SelectedMonth
        //{
        //    set
        //    {
        //        _selectedMonth = value;

        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedMonth"));
        //        //put here your code
        //        MonthText = Convert.ToInt32(_selectedMonth.Key);
        //        LoadHoliday();
        //    }
        //    get { return _selectedMonth; }
        //}
        //private int _monthText;
        //public int MonthText
        //{
        //    get { return _monthText; }
        //    set
        //    {
        //        SetProperty(ref _monthText, value);
        //    }
        //}

        //private bool _isRunning;
        //private bool _isEnabled;
        //public bool IsRunning
        //{
        //    get => _isRunning;
        //    set => SetProperty(ref _isRunning, value);
        //}

        //public bool IsEnabled
        //{
        //    get => _isEnabled;
        //    set => SetProperty(ref _isEnabled, value);
        //}

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


        //public Command<object> SelectionChangedCommand
        //{
        //    get { return selectionChangedCommand; }
        //    protected set { selectionChangedCommand = value; }
        //}

        //public Command<object> ItemHoldingCommand
        //{
        //    get { return itemHoldingCommand; }
        //    protected set { itemHoldingCommand = value; }
        //}

        //public Command<object> EditAttendanceCommand
        //{
        //    get { return editAttendanceCommand; }
        //    protected set { editAttendanceCommand = value; }
        //}

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

            //Command
            //selectionChangedCommand = new Command<object>(OnSelectionChanged);
            //itemHoldingCommand = new Command<object>(OnItemHolding);
            //EditAttendanceCommand = new Command<object>(EditAttendance);

            ////Call Method           
            //ListYearMonthPicker();
            //LoadHoliday();
        }
        #endregion

        #region Command
        public ICommand NewHolidayCommand { get { return new RelayCommand(NewHoliday); } }
        public ICommand AddNewHolidayCommand { get { return new RelayCommand(NewAddHoliday); } }

        //public ICommand RefreshHolidayCommand { get { return new RelayCommand(RefreshHoliday); } }


        //Command<object> selectionChangedCommand;

        //Command<object> itemHoldingCommand;

        //public ICommand AllSelectedCommand { get { return new RelayCommand(AllSelectionAttendance); } }
        //public ICommand DeleteAttendanceCommand { get { return new RelayCommand(DeleteAttendance); } }

        Command<object> editAttendanceCommand;

        #endregion

        #region Method
        //public async void CancelAllEvent()
        //{
        //    SelectionGesture = TouchGesture.Hold;
        //    FooterIsVisible = false;
        //    for (int i = 0; i < Holidays.Count; i++)
        //    {
        //        if (Holidays[i].IsSelected == true)
        //        {
        //            Holidays[i].IsSelected = false;
        //            Holidays[i].IsVisiable = false;
        //        }
        //    }
        //    SelectedItemCount = "HolidayList";
        //    if (IsEdit == 1)
        //    {
        //        IsEdit = 0;
        //        await navigationService.Back();
        //    }
        //}

        //private void AllSelectionAttendance()
        //{
        //    if (AllChecked == 0)
        //    {
        //        Count = 1;
        //        for (int i = 0; i < Holidays.Count; i++)
        //        {
        //            if (Holidays[i].IsSelected == false)
        //            {
        //                Holidays[i].IsSelected = true;
        //                Holidays[i].IsVisiable = true;
        //                SelectedItemCount = Count.ToString() + "/" + TotalCount.ToString() + " Selected";
        //            }
        //            Count++;
        //        }
        //        AllChecked = 1;
        //    }
        //    else
        //    {
        //        for (int i = 0; i < Holidays.Count; i++)
        //        {
        //            if (Holidays[i].IsSelected == true)
        //            {
        //                Holidays[i].IsSelected = false;
        //                Holidays[i].IsVisiable = false;
        //            }
        //        }
        //        SelectionGesture = TouchGesture.Hold;
        //        FooterIsVisible = false;
        //        SelectedItemCount = "HolidayList";
        //        Count = 0;
        //        AllChecked = 0;
        //    }
        //}

        //private async void OnItemHolding(object obj)
        //{
        //    try
        //    {
        //        var eventArgs = obj as ItemHoldingEventArgs;
        //        if (0 == flag)
        //        {
        //            SelectionGesture = TouchGesture.Tap;
        //            FooterIsVisible = true;
        //            flag = 1;
        //            Count = 1;
        //            (eventArgs.ItemData as HolidayViewModel).IsSelected = true;
        //            (eventArgs.ItemData as HolidayViewModel).IsVisiable = true;
        //            TotalCount = Holidays.Count;
        //            SelectedItemCount = Count.ToString() + "/" + TotalCount.ToString() + " Selected";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await dialogService.ShowMessage("Error", ex.Message);
        //    }
        //}

        //public async void OnSelectionChanged(object obj)
        //{
        //    try
        //    {
        //        if (flag == 1)
        //        {
        //            var eventArgs = obj as ItemSelectionChangedEventArgs;
        //            for (int i = 0; i < eventArgs.AddedItems.Count; i++)
        //            {
        //                var item = eventArgs.AddedItems[i];
        //                if ((item as HolidayViewModel).IsSelected == false)
        //                {
        //                    (item as HolidayViewModel).IsSelected = true;
        //                    (item as HolidayViewModel).IsVisiable = true;

        //                    if (Count >= 0)
        //                    {
        //                        if (Count == 0)
        //                        {
        //                            Count = 1;
        //                        }
        //                        else
        //                        {
        //                            Count = Count + 1;
        //                        }
        //                        SelectedItemCount = Count.ToString() + "/" + TotalCount.ToString() + " Selected";
        //                    }
        //                }
        //            }
        //            for (int i = 0; i < eventArgs.RemovedItems.Count; i++)
        //            {
        //                var item = eventArgs.RemovedItems[i];
        //                (item as HolidayViewModel).IsSelected = false;
        //                (item as HolidayViewModel).IsVisiable = false;

        //                if (Count >= 0)
        //                {
        //                    Count = Count - 1;
        //                    SelectedItemCount = Count.ToString() + "/" + TotalCount.ToString() + " Selected";
        //                    if (Count <= 0)
        //                    {
        //                        SelectionGesture = TouchGesture.Hold;
        //                        FooterIsVisible = false;
        //                        flag = 0;
        //                        Count = 0;
        //                        SelectedItemCount = "HolidayList";
        //                    }
        //                }
        //                else
        //                {
        //                    SelectionGesture = TouchGesture.Hold;
        //                    FooterIsVisible = false;
        //                    flag = 0;
        //                    Count = 0;
        //                    SelectedItemCount = "HolidayList";
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await dialogService.ShowMessage("Error", ex.Message);
        //    }
        //}

        //private void EditAttendance(object obj)
        //{
        //    if (Count == 1)
        //    {
        //        IsEdit = 1;
        //        for (int i = 0; i < Holidays.Count; i++)
        //        {
        //            if (Holidays[i].IsSelected == true)
        //            {
        //                HolidayInfo models = null;
        //                models = new HolidayInfo()
        //                {
        //                    Id = Holidays[i].Id,
        //                    HolidayDate = Holidays[i].HolidayDate,
        //                    Description = Holidays[i].Description
        //                };

        //                if (models != null)
        //                {
        //                    //Navigation.PushPopupAsync(new AddHoliday(models));
        //                }
        //            }
        //        }
        //    }
        //}

        //private void DeleteAttendance()
        //{
        //    int selectedAlert = 0;
        //    for (int i = 0; i < Holidays.Count; i++)
        //    {
        //        if (Holidays[i].IsSelected == true)
        //        {
        //            HolidayInfo models = null;
        //            models = new HolidayInfo()
        //            {
        //                Id = Holidays[i].Id,
        //                HolidayDate = Holidays[i].HolidayDate,
        //                Description = Holidays[i].Description
        //            };

        //            if (models != null)
        //            {
        //                bool result = holidayService.DeleteHoliday(models);
        //                selectedAlert++;
        //            }
        //        }
        //    }
        //    if (selectedAlert == 0)
        //    {
        //        var alert = dialogService.ShowMessage("Alert", "select one record");
        //    }
        //    else
        //    {
        //        var alert = dialogService.ShowMessage("Delete", "Delete" + selectedAlert + "Record?");
        //    }
        //    RefreshHoliday();
        //}

        //private void RefreshHoliday()
        //{
        //    var holidays = holidayService.GetHoliday(YearText, MonthText);
        //    ReloadHoliday(holidays);
        //    IsRefreshingHoliday = false;
        //    CancelAllEvent();
        //}

        //private async void LoadHoliday()
        //{
        //    try
        //    {
        //        var holidays = holidayService.GetHoliday(YearText, MonthText);                
        //        ReloadHoliday(holidays);
        //    }
        //    catch (Exception ex)
        //    {
        //        await dialogService.ShowMessage("Error", ex.Message);
        //    }
        //}

        //private async void ReloadHoliday(List<HolidayInfo> holidays)
        //{
        //    try
        //    {
        //        IsRunning = true;
        //        await Task.Delay(1000);
        //        Holidays.Clear();
        //        foreach (var holiday in holidays.OrderByDescending(d => d.HolidayDate))
        //        {
        //            Holidays.Add(new HolidayViewModel
        //            {
        //                Id = holiday.Id,
        //                HolidayDate = holiday.HolidayDate,
        //                Description = holiday.Description,
        //            });
        //        }
        //        IsRunning = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        await dialogService.ShowMessage("Error", ex.Message);
        //    }
        //}

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

        //private void ListYearMonthPicker()
        //{
        //    ListYears = PickerService.GetYear().ToList();
        //    ListMonths = PickerService.GetMonth().ToList();
        //    SelectedYear = ListYears[0];
        //    SelectedMonth = ListMonths[Month];
        //}

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(String name)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
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
