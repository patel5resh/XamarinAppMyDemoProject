using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Services
{
    public class DialogService
    {
        public async Task ShowMessage(string title, string message)
        {
            await App.Current.MainPage.DisplayAlert(title, message, "Ok");
        }
    }
}
