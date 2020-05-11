using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ATS.Droid;
using ATS.Infrastructure;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(Config))]
namespace ATS.Droid
{
    public class Config : IConfig
    {
        public Config() { }
        public SQLiteConnection DBConnect()
        {
            var dbName = "ATS.db3";
            var dbpath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var path = Path.Combine(dbpath, dbName);
            var conn = new SQLiteConnection(path);
            return conn;
        }
    }
}