using ATS.Models;
using Rg.Plugins.Popup.Extensions;
using System.ComponentModel;
using Xamarin.Forms;

namespace ATS.ViewModels
{
    public class AddAttendanceViewModel 
    {
        public INavigation Navigation { get; set; }
       
        public AddAttendanceViewModel(INavigation navigation)
        {
            Navigation = navigation;        
        }

        public Command ClosePopUpCommand => new Command(() =>
        {
            Navigation.PopPopupAsync();
        });
     }
}
