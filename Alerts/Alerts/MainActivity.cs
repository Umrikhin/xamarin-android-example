using Alerts.Models;
using Android.App;
using Android.Content;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Alerts
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop, ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation)]
    public class MainActivity : AppCompatActivity
    {
        // набор данных, которые свяжем со списком
        List<Alert> alerts = new List<Alert>();
        ListView listAlert;
        DatabaseHelper databaseHelper;
        SQLiteDatabase db;
        SQLiteCursor alertCursor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // получаем элемент ListView
            listAlert = FindViewById<ListView>(Resource.Id.listAlert);
            listAlert.ItemClick += ListAlert_ItemClick;
            databaseHelper = new DatabaseHelper(ApplicationContext);
        }

        private void ListAlert_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Alert selectedAlert = (Alert)e.Parent.GetItemAtPosition(e.Position);
            Intent intent = new Intent(ApplicationContext, typeof(AlertActivity));
            intent.PutExtra("id", selectedAlert.id);
            StartActivity(intent);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.ActionModeAdd:
                    //Переход к добавлению
                    Intent intent = new Intent(this, typeof(AlertActivity));
                    StartActivity(intent);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            // открываем подключение
            db = databaseHelper.ReadableDatabase;
            //получаем данные из бд в виде курсора
            alertCursor = runQuery();
            alerts.Clear();
            while (alertCursor.MoveToNext())
            {
                alerts.Add(new Alert()
                {
                    id = alertCursor.GetInt(alertCursor.GetColumnIndexOrThrow(DatabaseHelper.COLUMN_ID)),
                    title = alertCursor.GetString(alertCursor.GetColumnIndexOrThrow(DatabaseHelper.COLUMN_TITLE)),
                    note = alertCursor.GetString(alertCursor.GetColumnIndexOrThrow(DatabaseHelper.COLUMN_NOTE)),
                    start = DateTime.Parse(alertCursor.GetString(alertCursor.GetColumnIndexOrThrow(DatabaseHelper.COLUMN_START)), CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None)
                });
            }
            var alertAdapter = new AlertAdapter(this, this, Resource.Layout.list_item, alerts);
            // устанавливаем для списка адаптер
            listAlert.Adapter = alertAdapter;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            // Закрываем подключение и курсор
            db.Close();
            alertCursor.Close();
        }

        private SQLiteCursor runQuery()
        {
            return db.RawQuery("select * from " + DatabaseHelper.TABLE, null) as SQLiteCursor;
        }
    }
}