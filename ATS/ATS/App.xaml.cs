using ATS.Helpers;
using ATS.Models;
using ATS.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ATS
{
    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }
        public static MasterPage Master { get; internal set; }
        public App()
        {
            // Initialize Live Reload.
            InitializeComponent();
            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjQyOTQzQDMxMzgyZTMxMmUzMG9PYXgveFJBTGdHMTNUYTlXM2FybWhYMS9IeS9jVGM5VWJpeEV5VERhd1U9");
            #if DEBUG
            HotReloader.Current.Run(this);
            #endif
            MainPage = new MasterPage();
            SetupCurrentTheme();
        }

        /// <summary>
        /// Set up current theme from app settings
        /// </summary>
        public void SetupCurrentTheme()
        {
            var currentTheme = Preferences.Get("CurrentAppTheme", null);
            if (currentTheme != null)
            {
                if (Enum.TryParse(currentTheme, out Theme currentThemeEnum))
                {
                    ThemeHelper.SetAppTheme(currentThemeEnum);
                }
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
