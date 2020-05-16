using ATS.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ATS.Views
{
    public partial class ListofAttendancePage : ContentPage
    {
        ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
        public ObservableCollection<Employee> Employees { get { return employees; } }

        public ListofAttendancePage()
        {
            InitializeComponent();

            //EmployeeView.ItemsSource = employees;

            //// ObservableCollection allows items to be added after ItemsSource
            //// is set and the UI will react to changes
            //employees.Add(new Employee { DisplayName = "Rob Finnerty" });
            //employees.Add(new Employee { DisplayName = "Bill Wrestler" });
            //employees.Add(new Employee { DisplayName = "Dr. Geri-Beth Hooper" });
            //employees.Add(new Employee { DisplayName = "Dr. Keith Joyce-Purdy" });
            //employees.Add(new Employee { DisplayName = "Sheri Spruce" });
            //employees.Add(new Employee { DisplayName = "Burt Indybrick" });
            //employees.Add(new Employee { DisplayName = "Rob Finnerty" });
            //employees.Add(new Employee { DisplayName = "Bill Wrestler" });
            //employees.Add(new Employee { DisplayName = "Dr. Geri-Beth Hooper" });
            //employees.Add(new Employee { DisplayName = "Dr. Keith Joyce-Purdy" });
            //employees.Add(new Employee { DisplayName = "Sheri Spruce" });
            //employees.Add(new Employee { DisplayName = "Burt Indybrick" });
        }
    }
}