using ATS.Helpers;
using ATS.Models;
using ATS.Services;
using ATS.Views;
using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Extensions;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ATS.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Attributes
        readonly int Month = DateTime.Now.Month - 1;
        private Month _selectedMonth;
        private Year _selectedYear;
        private bool isRefreshingAttendances = false;
        private NavigationService navigationService;
        private AttendanceService attendanceService;
        private DialogService dialogService;
        private int flag = 0;
        private int Count = 0;
        private int TotalCount = 0;
        private int AllChecked = 0;
        private int IsEdit = 0;
        public INavigation Navigation { get; set; }
        #endregion

        #region Attributes Holiday
        readonly int MonthHoliday = DateTime.Now.Month - 1;
        private Month _selectedMonthHoliday;
        private Year _selectedYearHoliday;
        private HolidayService holidayService;
        private bool isRefreshingHoliday = false;
        private int flagHoliday = 0;
        private int CountHoliday = 0;
        private int TotalCountHoliday = 0;
        private int AllCheckedHoliday = 0;
        private int IsEditHoliday = 0;
        #endregion

        #region Properties      
        public List<Year> ListYears { get; set; }
        public List<Month> ListMonths { get; set; }
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public ObservableCollection<AttendanceViewModel> Attendances { get; set; }
        public AttendanceViewModel NewAddAttendance { get; set; }
       
        public Year SelectedYear
        {           
            set
            {
                _selectedYear = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedYear"));
                //put here your code
                YearText = Convert.ToInt32(_selectedYear.Value);
                LoadAttendances();
            }
            get { return _selectedYear; }
        }
        private int _yearText;
        public int YearText
        {
            get { return _yearText; }
            set
            {
                SetProperty(ref _yearText, value);
            }
        }
        public Month SelectedMonth
        {          
            set
            {
                _selectedMonth = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedMonth"));
                //put here your code
                MonthText = Convert.ToInt32(_selectedMonth.Key);
                LoadAttendances();
            }
            get { return _selectedMonth; }
        }
        private int _monthText;
        public int MonthText
        {
            get { return _monthText; }
            set
            {
                SetProperty(ref _monthText, value);
            }
        }
        private string _totalTimeSummary;
        public string TotalTimeSummary
        {
            get { return _totalTimeSummary; }
            set
            {
                SetProperty(ref _totalTimeSummary, value);
            }
        }
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

        private bool footerIsVisible = false;
        public bool FooterIsVisible
        {
            set
            {
                footerIsVisible = value;
                OnPropertyChanged();
            }
            get
            {
                return footerIsVisible;
            }
        }
        
        private string selectedItemCount = "AttendanceList";
        public string SelectedItemCount
        {
            get { return selectedItemCount; }
            set
            {
                SetProperty(ref selectedItemCount, value);
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
        public Command<object> SelectionChangedCommand
        {
            get { return selectionChangedCommand; }
            protected set { selectionChangedCommand = value; }
        }

        public Command<object> ItemHoldingCommand
        {
            get { return itemHoldingCommand; }
            protected set { itemHoldingCommand = value; }
        }

        public Command<object> EditAttendanceCommand
        {
            get { return editAttendanceCommand; }
            protected set { editAttendanceCommand = value; }
        }
        #endregion

        #region Properties Holiday
        public ObservableCollection<HolidayViewModel> Holidays { get; set; }
        public HolidayViewModel NewAddHoliday { get; set; }
        public bool IsRefreshingHoliday
        {
            set
            {
                if (isRefreshingHoliday != value)
                {
                    isRefreshingHoliday = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshingHoliday"));
                }
            }
            get
            {
                return isRefreshingHoliday;
            }
        }

        private bool footerIsVisibleHoliday = false;
        public bool FooterIsVisibleHoliday
        {
            set
            {
                footerIsVisibleHoliday = value;
                OnPropertyChanged();
            }
            get
            {
                return footerIsVisibleHoliday;
            }
        }

        private string selectedItemCountHoliday = "HolidayList";
        public string SelectedItemCountHoliday
        {
            get { return selectedItemCountHoliday; }
            set
            {
                SetProperty(ref selectedItemCountHoliday, value);
            }
        }

        private TouchGesture selectionGestureHoliday = TouchGesture.Hold;
        public TouchGesture SelectionGestureHoliday
        {
            get { return selectionGestureHoliday; }
            set
            {
                SetProperty(ref selectionGestureHoliday, value);
            }
        }

        private bool _isRunningHoliday;
        private bool _isEnabledHoliday;
        public bool IsRunningHoliday
        {
            get => _isRunningHoliday;
            set => SetProperty(ref _isRunningHoliday, value);
        }

        public bool IsEnabledHoliday
        {
            get => _isEnabledHoliday;
            set => SetProperty(ref _isEnabledHoliday, value);
        }

        private bool isSelectedHoliday;
        private bool isVisiableHoliday;
        public bool IsSelectedHoliday
        {
            get { return isSelectedHoliday; }
            set
            {
                isSelectedHoliday = value;
                RaisePropertyChanged("IsSelectedHoliday");
            }
        }

        public bool IsVisiableHoliday
        {
            get { return isVisiableHoliday; }
            set
            {
                isVisiableHoliday = value;
                RaisePropertyChanged("IsVisiableHoliday");
            }
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }
        private void RaisePropertyChanged(String name)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public Command<object> EditHolidayCommand
        {
            get { return editHolidayCommand; }
            protected set { editHolidayCommand = value; }
        }

        #endregion

        #region Constructor
        public MainViewModel()
        {
            //Create observable collections
            Menu = new ObservableCollection<MenuItemViewModel>();
            Attendances = new ObservableCollection<AttendanceViewModel>();
            Holidays = new ObservableCollection<HolidayViewModel>();

            //CreateView
            NewAddAttendance = new AttendanceViewModel();
            NewAddHoliday = new HolidayViewModel();

            //Instance services
            navigationService = new NavigationService();
            attendanceService = new AttendanceService();
            holidayService = new HolidayService();
            dialogService = new DialogService();

            //AttendanceCommand
            selectionChangedCommand = new Command<object>(OnSelectionChanged);
            itemHoldingCommand = new Command<object>(OnItemHolding);
            EditAttendanceCommand = new Command<object>(EditAttendance);
            //HolidayCommand
            EditHolidayCommand = new Command<object>(EditHoliday);
            holidaySelectionChangedCommand = new Command<object>(HolidayOnSelectionChanged);
            holidayItemHoldingCommand = new Command<object>(HolidayOnItemHolding);

            //Call Method
            LoadMenu();
            ListYearMonthPicker();
            LoadAttendances();
        }

        #endregion

        #region Event
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Singleton

        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }

        #endregion
      
        #region Command
        public ICommand NewAttendanceCommand { get { return new RelayCommand(NewAttendances); } }

        public ICommand RefreshAttendancesCommand { get { return new RelayCommand(RefreshAttendances); } }

        Command<object> selectionChangedCommand;

        Command<object> itemHoldingCommand;

        public ICommand AllSelectedCommand { get { return new RelayCommand(AllSelectionAttendance); } }
        public ICommand DeleteAttendanceCommand { get { return new RelayCommand(DeleteAttendance); } }

        Command<object> editAttendanceCommand;

        public Command ClosePopUpCommand => new Command(() =>
        {
            Navigation.PopPopupAsync();
        });
        #endregion

        #region Method         
        public async void CancelAllEvent()
        {
            SelectionGesture = TouchGesture.Hold;
            FooterIsVisible = false;          
            for (int i = 0; i < Attendances.Count; i++)
            {
                if (Attendances[i].IsSelected == true)
                {
                    Attendances[i].IsSelected = false;
                    Attendances[i].IsVisiable = false;
                }
            }
            SelectedItemCount = "AttendanceList";
            if (IsEdit == 1)
            {
                IsEdit = 0;
                await navigationService.Back();
            }
        }

        private void AllSelectionAttendance()
        {
            if (AllChecked == 0)
            {
                Count = 1;
                for (int i = 0; i < Attendances.Count; i++)
                {
                    if (Attendances[i].IsSelected == false)
                    {
                        Attendances[i].IsSelected = true;
                        Attendances[i].IsVisiable = true;
                        SelectedItemCount = Count.ToString() + "/" + TotalCount.ToString() + " Selected";                        
                    }
                    Count++;                    
                }
                AllChecked = 1;
            }
            else
            {                
                for (int i = 0; i < Attendances.Count; i++)
                {
                    if (Attendances[i].IsSelected == true)
                    {
                        Attendances[i].IsSelected = false;
                        Attendances[i].IsVisiable = false;
                    }
                }
                SelectionGesture = TouchGesture.Hold;
                FooterIsVisible = false;
                SelectedItemCount = "AttendanceList";
                Count = 0;
                AllChecked = 0;
            }
        }

        private async void OnItemHolding(object obj)
        {
            try
            {
                var eventArgs = obj as ItemHoldingEventArgs;
                if (0 == flag)
                {
                    SelectionGesture = TouchGesture.Tap;
                    FooterIsVisible = true;
                    flag = 1;
                    Count = 1;
                    (eventArgs.ItemData as AttendanceViewModel).IsSelected = true;
                    (eventArgs.ItemData as AttendanceViewModel).IsVisiable = true;
                    TotalCount = Attendances.Count;
                    SelectedItemCount = Count.ToString() + "/" + TotalCount.ToString() + " Selected";
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        public async void OnSelectionChanged(object obj)
        {
            try
            {
                if (flag == 1)
                {
                    var eventArgs = obj as ItemSelectionChangedEventArgs;
                    for (int i = 0; i < eventArgs.AddedItems.Count; i++)
                    {
                        var item = eventArgs.AddedItems[i];
                        if ((item as AttendanceViewModel).IsSelected == false)
                        {
                            (item as AttendanceViewModel).IsSelected = true;
                            (item as AttendanceViewModel).IsVisiable = true;

                            if (Count >= 0)
                            {
                                if (Count == 0)
                                {
                                    Count = 1;
                                }
                                else
                                {
                                    Count = Count + 1;
                                }
                                SelectedItemCount = Count.ToString() + "/" + TotalCount.ToString() + " Selected";
                            }
                        }
                    //    else
                    //    {
                    //        if ((item as AttendanceViewModel).IsSelected == true)
                    //        {
                    //            if (Count >= 0)
                    //            {
                    //                Count = Count - 1;
                    //                SelectedItemCount = Count.ToString() + "/" + TotalCount.ToString() + " Selected";
                    //                if (Count == 1)
                    //                {
                    //                    SelectionGesture = TouchGesture.Hold;
                    //                    FooterIsVisible = false;
                    //                    flag = 0;
                    //                    (item as AttendanceViewModel).IsSelected = false;
                    //                    (item as AttendanceViewModel).IsVisiable = false;
                    //                    SelectedItemCount = "AttendanceList";
                    //                }
                    //                else
                    //                {
                    //                    if(Count <= 1)
                    //                    {
                    //                        SelectionGesture = TouchGesture.Hold;
                    //                        FooterIsVisible = false;
                    //                        flag = 0;
                    //                        (item as AttendanceViewModel).IsSelected = false;
                    //                        (item as AttendanceViewModel).IsVisiable = false;
                    //                        SelectedItemCount = "AttendanceList";
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    //}
                    }
                    for (int i = 0; i < eventArgs.RemovedItems.Count; i++)
                    {
                        var item = eventArgs.RemovedItems[i];
                        (item as AttendanceViewModel).IsSelected = false;
                        (item as AttendanceViewModel).IsVisiable = false;

                        if (Count >= 0)
                        {
                            Count = Count - 1;
                            SelectedItemCount = Count.ToString() + "/" + TotalCount.ToString() + " Selected";
                            if (Count <= 0)
                            {
                                SelectionGesture = TouchGesture.Hold;
                                FooterIsVisible = false;
                                flag = 0;
                                Count = 0;
                                SelectedItemCount = "AttendanceList";
                            }
                        }
                        else
                        {
                            SelectionGesture = TouchGesture.Hold;
                            FooterIsVisible = false;
                            flag = 0;
                            Count = 0;
                            SelectedItemCount = "AttendanceList";
                        }
                    }                  
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        private void EditAttendance(object obj)
        {
            if (Count == 1)
            {
                IsEdit = 1;
                for (int i = 0; i < Attendances.Count; i++)
                {
                    if (Attendances[i].IsSelected == true)
                    {
                        AttendanceInfo models = null;
                        models = new AttendanceInfo()
                        {
                            Id = Attendances[0].Id,
                            AttendanceDate = Attendances[0].AttendanceDate,
                            InTime = Attendances[0].InTime,
                            OutTime = Attendances[0].OutTime,
                            WorkingTime = Attendances[0].WorkingTime
                        };

                        if (models != null)
                        {
                            Navigation.PushPopupAsync(new AddAttendance(models));
                        }
                    }
                }
            }
        }

        private void DeleteAttendance()
        {
            int selectedAlert = 0;
            for (int i = 0; i < Attendances.Count; i++)
            {
                if (Attendances[i].IsSelected == true)
                {
                    AttendanceInfo models = null;
                    models = new AttendanceInfo(){
                        Id = Attendances[i].Id,
                        AttendanceDate = Attendances[i].AttendanceDate,
                        InTime = Attendances[i].InTime,
                        OutTime = Attendances[i].OutTime,
                        WorkingTime = Attendances[i].WorkingTime};

                    if (models != null)
                    {
                        var result = attendanceService.DeleteAttendance(models);
                        selectedAlert++;                        
                    }
                }               
            }
            if(selectedAlert == 0)
            {
                var alert = dialogService.ShowMessage("Alert", "select one record");
            }
            else
            {
                var alert = dialogService.ShowMessage("Delete", "Delete" + selectedAlert + "Record?");
            }
            RefreshAttendances();
        }
        
        private void RefreshAttendances()
        {
            var attendances = attendanceService.GetAttendances(YearText, MonthText);
            if (attendances.Count > 0)
            {
                TimeSpan SumOfTime = new TimeSpan(attendances.Sum(p => p.WorkingTime.Ticks));
                TimeSpan totalminutes = TimeSpan.FromMinutes(SumOfTime.Minutes);
                TimeSpan totalseconds = TimeSpan.FromSeconds(SumOfTime.Seconds);
                double whours = SumOfTime.Days * 24 + SumOfTime.Hours;
                double wminutes = totalminutes.Minutes;
                double wseconds = totalseconds.Seconds;
                string TotalWorkingTime = Convert.ToInt32(whours) + ":" + Convert.ToInt32(wminutes) + ":" + Convert.ToInt32(wseconds);
                TotalTimeSummary = TotalWorkingTime.ToString() + "hrs";
            }
            else
            {
                TotalTimeSummary = "00:00:00 hrs";
            }
            ReloadAttendances(attendances);
            IsRefreshingAttendances = false;
            CancelAllEvent();
        }

        public async void NewAttendances()
        {
            await navigationService.Navigate("AddAttendancePage");          
        }

        private async void LoadAttendances()
        {
            try
            {
                var attendances = attendanceService.GetAttendances(YearText, MonthText);
                if (attendances.Count > 0)
                {
                    TimeSpan SumOfTime = new TimeSpan(attendances.Sum(p => p.WorkingTime.Ticks));
                    TimeSpan totalminutes = TimeSpan.FromMinutes(SumOfTime.Minutes);
                    TimeSpan totalseconds = TimeSpan.FromSeconds(SumOfTime.Seconds);
                    double whours = SumOfTime.Days * 24 + SumOfTime.Hours;
                    double wminutes = totalminutes.Minutes;
                    double wseconds = totalseconds.Seconds;
                    string TotalWorkingTime = Convert.ToInt32(whours) + ":" + Convert.ToInt32(wminutes) + ":" + Convert.ToInt32(wseconds);
                    TotalTimeSummary = TotalWorkingTime.ToString() + "hrs";
                }
                else
                {
                    TotalTimeSummary = "00:00:00 hrs";
                }
                ReloadAttendances(attendances);
            }
            catch(Exception ex)
            {
                //Console.WriteLine(ex.Message);
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        private async void ReloadAttendances(List<AttendanceInfo> attendances)
        {
            try
            {
                await Task.Delay(1000);
                Attendances.Clear();
                foreach (var attendance in attendances.OrderByDescending(d => d.AttendanceDate))
                {
                    Attendances.Add(new AttendanceViewModel
                    {
                        Id = attendance.Id,
                        InTime = attendance.InTime,
                        OutTime = attendance.OutTime,
                        AttendanceDate = attendance.AttendanceDate,
                        WorkingTime = attendance.WorkingTime,
                    });
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        private void ListYearMonthPicker()
        {
            ListYears = PickerService.GetYear().ToList();
            ListMonths = PickerService.GetMonth().ToList();
            SelectedYear = ListYears[0];
            SelectedMonth = ListMonths[Month];
        }

        private void LoadMenu()
        {
            Menu.Add(new MenuItemViewModel
            {
                Icon = IconFont.Clock,
                PageName = "AttendanceList",
                Title = "Attendance",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = IconFont.Calendar,
                PageName = "HolidayList",
                Title = "Holiday",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = IconFont.ThemeLightDark,
                PageName = "ThemeSelectionPage",
                Title = "Themes",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = IconFont.Cog,
                PageName = "AttendanceSetting",
                Title = "Setting",
            });
        }
        #endregion

        #region Holiday Command
        Command<object> holidaySelectionChangedCommand;

        Command<object> holidayItemHoldingCommand;

        Command<object> editHolidayCommand;

        public ICommand NewHolidayCommand { get { return new RelayCommand(NewHoliday); } }
        //public ICommand AddNewHolidayCommand { get { return new RelayCommand(NewAddHoliday); } }

        public ICommand RefreshHolidayCommand { get { return new RelayCommand(RefreshHoliday); } }

        public ICommand AllSelectedHolidayCommand { get { return new RelayCommand(AllSelectionHoliday); } }
        public ICommand DeleteHolidayCommand { get { return new RelayCommand(DeleteHoliday); } }


        #endregion

        #region Holiday Method
        public async void CancelAllHolidayEvent()
        {
            SelectionGesture = TouchGesture.Hold;
            FooterIsVisible = false;
            for (int i = 0; i < Holidays.Count; i++)
            {
                if (Holidays[i].IsSelected == true)
                {
                    Holidays[i].IsSelected = false;
                    Holidays[i].IsVisiable = false;
                }
            }
            SelectedItemCount = "HolidayList";
            if (IsEdit == 1)
            {
                IsEdit = 0;
                await navigationService.Back();
            }
        }

        private void AllSelectionHoliday()
        {
            if (AllChecked == 0)
            {
                Count = 1;
                for (int i = 0; i < Holidays.Count; i++)
                {
                    if (Holidays[i].IsSelected == false)
                    {
                        Holidays[i].IsSelected = true;
                        Holidays[i].IsVisiable = true;
                        SelectedItemCount = Count.ToString() + "/" + TotalCount.ToString() + " Selected";
                    }
                    Count++;
                }
                AllChecked = 1;
            }
            else
            {
                for (int i = 0; i < Holidays.Count; i++)
                {
                    if (Holidays[i].IsSelected == true)
                    {
                        Holidays[i].IsSelected = false;
                        Holidays[i].IsVisiable = false;
                    }
                }
                SelectionGesture = TouchGesture.Hold;
                FooterIsVisible = false;
                SelectedItemCount = "HolidayList";
                Count = 0;
                AllChecked = 0;
            }
        }

        private async void HolidayOnItemHolding(object obj)
        {
            try
            {
                var eventArgs = obj as ItemHoldingEventArgs;
                if (0 == flag)
                {
                    SelectionGesture = TouchGesture.Tap;
                    FooterIsVisible = true;
                    flag = 1;
                    Count = 1;
                    (eventArgs.ItemData as HolidayViewModel).IsSelected = true;
                    (eventArgs.ItemData as HolidayViewModel).IsVisiable = true;
                    TotalCount = Holidays.Count;
                    SelectedItemCount = Count.ToString() + "/" + TotalCount.ToString() + " Selected";
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        public async void HolidayOnSelectionChanged(object obj)
        {
            try
            {
                if (flag == 1)
                {
                    var eventArgs = obj as ItemSelectionChangedEventArgs;
                    for (int i = 0; i < eventArgs.AddedItems.Count; i++)
                    {
                        var item = eventArgs.AddedItems[i];
                        if ((item as HolidayViewModel).IsSelected == false)
                        {
                            (item as HolidayViewModel).IsSelected = true;
                            (item as HolidayViewModel).IsVisiable = true;

                            if (Count >= 0)
                            {
                                if (Count == 0)
                                {
                                    Count = 1;
                                }
                                else
                                {
                                    Count = Count + 1;
                                }
                                SelectedItemCount = Count.ToString() + "/" + TotalCount.ToString() + " Selected";
                            }
                        }
                    }
                    for (int i = 0; i < eventArgs.RemovedItems.Count; i++)
                    {
                        var item = eventArgs.RemovedItems[i];
                        (item as HolidayViewModel).IsSelected = false;
                        (item as HolidayViewModel).IsVisiable = false;

                        if (Count >= 0)
                        {
                            Count = Count - 1;
                            SelectedItemCount = Count.ToString() + "/" + TotalCount.ToString() + " Selected";
                            if (Count <= 0)
                            {
                                SelectionGesture = TouchGesture.Hold;
                                FooterIsVisible = false;
                                flag = 0;
                                Count = 0;
                                SelectedItemCount = "HolidayList";
                            }
                        }
                        else
                        {
                            SelectionGesture = TouchGesture.Hold;
                            FooterIsVisible = false;
                            flag = 0;
                            Count = 0;
                            SelectedItemCount = "HolidayList";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        private void EditHoliday(object obj)
        {
            if (Count == 1)
            {
                IsEdit = 1;
                for (int i = 0; i < Holidays.Count; i++)
                {
                    if (Holidays[i].IsSelected == true)
                    {
                        HolidayInfo models = null;
                        models = new HolidayInfo()
                        {
                            Id = Holidays[i].Id,
                            HolidayDate = Holidays[i].HolidayDate,
                            Description = Holidays[i].Description
                        };

                        if (models != null)
                        {
                            //Navigation.PushPopupAsync(new AddHoliday(models));
                        }
                    }
                }
            }
        }

        private void DeleteHoliday()
        {
            int selectedAlert = 0;
            for (int i = 0; i < Holidays.Count; i++)
            {
                if (Holidays[i].IsSelected == true)
                {
                    HolidayInfo models = null;
                    models = new HolidayInfo()
                    {
                        Id = Holidays[i].Id,
                        HolidayDate = Holidays[i].HolidayDate,
                        Description = Holidays[i].Description
                    };

                    if (models != null)
                    {
                        bool result = holidayService.DeleteHoliday(models);
                        selectedAlert++;
                    }
                }
            }
            if (selectedAlert == 0)
            {
                var alert = dialogService.ShowMessage("Alert", "select one record");
            }
            else
            {
                var alert = dialogService.ShowMessage("Delete", "Delete" + selectedAlert + "Record?");
            }
            RefreshHoliday();
        }

        private void RefreshHoliday()
        {
            var holidays = holidayService.GetHoliday(YearText, MonthText);
            ReloadHoliday(holidays);
            IsRefreshingHoliday = false;
            CancelAllHolidayEvent();
        }

        private async void LoadHoliday()
        {
            try
            {
                var holidays = holidayService.GetHoliday(YearText, MonthText);
                ReloadHoliday(holidays);
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        private async void ReloadHoliday(List<HolidayInfo> holidays)
        {
            try
            {
                IsRunningHoliday = true;
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
                IsRunningHoliday = false;
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

        #endregion
    }
}
