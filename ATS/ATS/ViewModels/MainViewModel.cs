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
        #endregion

        #region Properties      
        public List<Year> ListYears { get; set; }
        public List<Month> ListMonths { get; set; }
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public ObservableCollection<AttendanceViewModel> Attendances { get; set; }
        public AttendanceViewModel NewAddAttendance { get; set; }
        public INavigation Navigation { get; set; }

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

        private bool editEnabled = true;
        public bool EditEnabled
        {
            set
            {
                editEnabled = value;
                OnPropertyChanged();
            }
            get
            {
                return editEnabled;
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

        #region Constructor
        public MainViewModel()
        {
            //Create observable collections
            Menu = new ObservableCollection<MenuItemViewModel>();
            Attendances = new ObservableCollection<AttendanceViewModel>();

            //CreateView
            NewAddAttendance = new AttendanceViewModel();

            //Instance services
            navigationService = new NavigationService();
            attendanceService = new AttendanceService();
            dialogService = new DialogService();

            //Command
            selectionChangedCommand = new Command<object>(OnSelectionChanged);
            itemHoldingCommand = new Command<object>(OnItemHolding);
            EditAttendanceCommand = new Command<object>(EditAttendance);

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
        public ICommand CancelEventCommand { get { return new RelayCommand(CancelAllEvent); } }

        Command<object> editAttendanceCommand;

        #endregion

        #region Method             
        public void CancelAllEvent()
        {
            SelectionGesture = TouchGesture.Tap;
            FooterIsVisible = true;          
            for (int i = 0; i < Attendances.Count; i++)
            {
                if (Attendances[i].IsSelected == true)
                {
                    Attendances[i].IsSelected = false;
                    Attendances[i].IsVisiable = false;
                }
            }
            SelectedItemCount = "AttendanceList";
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
                Count = 0;
                for (int i = 0; i < Attendances.Count; i++)
                {
                    if (Attendances[i].IsSelected == true)
                    {
                        Attendances[i].IsSelected = false;
                        Attendances[i].IsVisiable = false;
                        SelectedItemCount = Count.ToString() + "/" + TotalCount.ToString() + " Selected";                        
                    }
                }
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
                    else
                    {
                        if ((item as AttendanceViewModel).IsSelected == true)
                        {
                            if (Count >= 0)
                            {
                                Count = Count - 1;
                                SelectedItemCount = Count.ToString() + "/" + TotalCount.ToString() + " Selected";
                                if (Count == 0)
                                {
                                    SelectionGesture = TouchGesture.Hold;
                                    FooterIsVisible = false;
                                    flag = 0;
                                    (item as AttendanceViewModel).IsSelected = false;
                                    (item as AttendanceViewModel).IsVisiable = false;                                   
                                    SelectedItemCount = "AttendanceList";
                                }
                            }
                        }
                    }
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
                        if (Count == 0)
                        {
                            SelectionGesture = TouchGesture.Hold;
                            FooterIsVisible = false;
                            flag = 0;
                            SelectedItemCount = "AttendanceList";
                        }
                    }
                    else
                    {
                        SelectionGesture = TouchGesture.Hold;
                        FooterIsVisible = false;
                        flag = 0;
                        SelectedItemCount = "AttendanceList";
                    }
                }
                if(Count == 1)
                {
                    editEnabled = true;
                }
                else
                {
                    editEnabled = false;        
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", ex.Message);
            }
        }

        private void EditAttendance(object obj)
        {
            if (Attendances.Count == 1)
            {
                if (Attendances[0].IsSelected == true)
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
                        SelectedImage = ImageSource.FromResource("ATS.Images.Selected.png"),
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
                Icon = FontAwesomeFont.ClockO,
                PageName = "AttendanceList",
                Title = "Attendance",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = FontAwesomeFont.PlusCircle,
                PageName = "AddAttendancePage",
                Title = "Add",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = FontAwesomeFont.Cog,
                PageName = "AttendanceSetting",
                Title = "Setting",
            });
        }
        #endregion
    }
}
