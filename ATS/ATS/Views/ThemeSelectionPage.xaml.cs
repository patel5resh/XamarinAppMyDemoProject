﻿using ATS.Helpers;
using ATS.Models;
using ATS.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ATS.Views
{
    public partial class ThemeSelectionPage : ContentPage
	{
		public ThemeSelectionPage()
		{
			InitializeComponent();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ThemePicker.ItemsSource = Enum.GetValues(typeof(Theme));

            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries.Count > 0)
            {
                var currentTheme = mergedDictionaries.First().GetType();

                if (currentTheme.FullName != null && currentTheme.FullName.Equals(typeof(LightTheme).FullName))
                {
                    ThemePicker.SelectedIndex = 0;
                }
                else
                if (currentTheme.FullName != null && currentTheme.FullName.Equals(typeof(DarkTheme).FullName))
                {
                    ThemePicker.SelectedIndex = 1;
                }
                else
                if (currentTheme.FullName != null && currentTheme.FullName.Equals(typeof(PinkTheme).FullName))
                {
                    ThemePicker.SelectedIndex = 2;
                }
                else
                if (currentTheme.FullName != null && currentTheme.FullName.Equals(typeof(GoldTheme).FullName))
                {
                    ThemePicker.SelectedIndex = 3;
                }
                else
                if (currentTheme.FullName != null && currentTheme.FullName.Equals(typeof(BlueTheme).FullName))
                {
                    ThemePicker.SelectedIndex = 4;
                }

                if (ThemePicker.SelectedItem != null)
                    statusLabel.Text = $"Currently, {ThemePicker.SelectedItem.ToString()} theme loaded.";
            }
        }

        void OnPickerSelectionChanged(object sender, EventArgs e)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();

                // parsing selected theme value
                if (Enum.TryParse(ThemePicker.SelectedItem.ToString(), out Theme currentThemeEnum))
                {
                    // setting up theme
                    if (ThemeHelper.SetAppTheme(currentThemeEnum))
                    {
                        // Theme setting successful
                        statusLabel.Text = $"{ThemePicker.SelectedItem.ToString()} theme loaded. Close this page.";
                        Preferences.Set("CurrentAppTheme", ThemePicker.SelectedItem.ToString());
                    }
                }
            }
        }
    }
}