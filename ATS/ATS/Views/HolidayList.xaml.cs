using ATS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ATS.Views
{
	public partial class HolidayList : ContentPage
	{
        MainViewModel mainViewModel;
        public HolidayList()
		{
			InitializeComponent();
            mainViewModel = (MainViewModel)this.BindingContext;
            Appearing += (object sender, EventArgs e) =>
            {
                mainViewModel.RefreshHolidayCommand.Execute(this);
            };
        }
	}
}