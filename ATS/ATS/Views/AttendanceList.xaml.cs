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
            //NavigationPage navigationPage = new NavigationPage();
            //navigationPage.SetHasBackButton(this, false);
        }

        //private void ListView_SelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        //{
        //    for (int i = 0; i < e.AddedItems.Count; i++)
        //    {
        //        var item = e.AddedItems[i];
        //        if ((item as AttendanceViewModel).IsSelected == false)
        //        {
        //            (item as AttendanceViewModel).IsSelected = true;
        //            (item as AttendanceViewModel).IsVisiable = true;

        //            if (Count == 0)
        //            {
        //                Count = 1;
        //            }
        //            if (Count > 0)
        //            {
        //                Count = Count + 1;
        //                NoCount.Text = Count.ToString() + "/" + totalcount.ToString() + "Selected";
        //            }

        //            var Items = (item as AttendanceViewModel);
        //        }
        //        else
        //        {
        //            if ((item as AttendanceViewModel).IsSelected == true)
        //            {
        //                if (Count >= 0)
        //                {
        //                    Count = Count - 1;
        //                    NoCount.Text = Count.ToString() + "/" + totalcount.ToString() + "Selected";
        //                    if (Count == 0)
        //                    {
        //                        listAttendance.SelectionGesture = TouchGesture.Hold;
        //                        gridVisible.IsVisible = false;
        //                        flag = 0;
        //                        (item as AttendanceViewModel).IsSelected = false;
        //                        (item as AttendanceViewModel).IsVisiable = false;
        //                    }
        //                }
        //            }
        //        }
        //        //ViewModel.GetData(Items);
        //    }

        //    for (int i = 0; i < e.RemovedItems.Count; i++)
        //    {
        //        var item = e.RemovedItems[i];
        //        (item as AttendanceViewModel).IsSelected = false;
        //        (item as AttendanceViewModel).IsVisiable = false;

        //        if (Count >= 0)
        //        {
        //            Count = Count - 1;
        //            NoCount.Text = Count.ToString() + "/" + totalcount.ToString() + "Selected";
        //        }
        //        var Items = (item as AttendanceViewModel);
        //        //ViewModel.GetData(Items);
        //    }
        //}

        //private void ListView_ItemHolding(object sender, ItemHoldingEventArgs e)
        //{
        //    if (0 == flag)
        //    {
        //        listAttendance.SelectionGesture = TouchGesture.Tap;
        //        gridVisible.IsVisible = true;
        //        flag = 1;
        //        Count = 1;
        //        (e.ItemData as AttendanceViewModel).IsSelected = true;
        //        (e.ItemData as AttendanceViewModel).IsVisiable = true;
        //        lblTitle.IsVisible = false;
        //        NoCount.IsVisible = true;
        //        totalcount = mainViewModel.Attendances.Count;
        //        NoCount.Text = Count.ToString() + "/" + totalcount.ToString() + "Selected";
        //    }
        //    //else
        //    //{
        //    //    listView.SelectionGesture = TouchGesture.Hold;
        //    //    gridVisible.IsVisible = false;
        //    //    flag = 0;
        //    //    (e.ItemData as MusicInfo).IsSelected = false;
        //    //    (e.ItemData as MusicInfo).IsVisiable = false;
        //    //    NoCount.Text = Count.ToString() + "Selected";
        //    //}
        //}

        //private void AllSelection_Clicked(object sender, EventArgs e)
        //{
        //    if (AllChecked == 0)
        //    {
        //        Count = 1;
        //        for (int i = 0; i < mainViewModel.Attendances.Count; i++)
        //        {
        //            if (mainViewModel.Attendances[i].IsSelected == false)
        //            {
        //                mainViewModel.Attendances[i].IsSelected = true;
        //                mainViewModel.Attendances[i].IsVisiable = true;
        //                NoCount.Text = Count.ToString() + "/" + totalcount.ToString() + "Selected";
        //            }
        //            Count++;
        //        }
        //        AllChecked = 1;
        //    }
        //    else
        //    {
        //        Count = 0;
        //        for (int i = 0; i < mainViewModel.Attendances.Count; i++)
        //        {
        //            if (mainViewModel.Attendances[i].IsSelected == true)
        //            {
        //                mainViewModel.Attendances[i].IsSelected = false;
        //                mainViewModel.Attendances[i].IsVisiable = false;
        //                NoCount.Text = Count.ToString() + "/" + totalcount.ToString() + "Selected";
        //            }
        //        }
        //        AllChecked = 0;
        //    }
        //}
    }
}