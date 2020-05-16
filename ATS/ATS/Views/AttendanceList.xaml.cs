using ATS.ViewModels;
using System;
using Xamarin.Forms;

namespace ATS.Views
{
    public partial class AttendanceList : ContentPage
    {
        MainViewModel mainViewModel;
        public AttendanceList()
        {
            InitializeComponent();           
            mainViewModel = (MainViewModel)this.BindingContext;
            Appearing += (object sender, EventArgs e) =>
            {
                mainViewModel.RefreshAttendancesCommand.Execute(this);
            };
        }
    }
}