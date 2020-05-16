using ATS.Views;
using System.Threading.Tasks;

namespace ATS.Services
{
    public class NavigationService
    {
        public NavigationService()
        {
        }
        public async Task Navigate(string PageName)
        {
            App.Master.IsPresented = false;
            switch (PageName)
            {              
                case "AttendanceList":
                    await App.Navigator.PushAsync(new AttendanceList());
                    break;
                case "HolidayList":
                    await App.Navigator.PushAsync(new HolidayList());
                    break;
                case "ThemeSelectionPage":
                    await App.Navigator.PushAsync(new ThemeSelectionPage());
                    break;
                case "AttendanceSetting":
                    await App.Navigator.PushAsync(new AttendanceSetting());
                    break;
                case "AddAttendancePage":
                    await App.Navigator.PushAsync(new AddAttendancePage());
                    break;
                case "AddHoliday":
                    await App.Navigator.PushAsync(new AddHoliday());
                    break;
                case "MainPage":
                    await App.Navigator.PopToRootAsync();
                    break;
                default:
                    break;
            }
        }
        public async Task Back()
        {
            await App.Navigator.PopAsync();
        }
    }
}
