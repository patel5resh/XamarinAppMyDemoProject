using ATS.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ATS.ViewModels
{
    public class MenuItemViewModel
    {
        #region Properties
        private NavigationService navigationService;
        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }

        public ICommand NavigateCommand { get { return new RelayCommand(Navigate); } }
        #endregion

        #region Constructor
        public MenuItemViewModel()
        {
            navigationService = new NavigationService();
        }
        #endregion

        #region Method
        private async void Navigate()
        {
            await navigationService.Navigate(PageName);
        }
        #endregion
    }
}
