using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SQLiteDelRow
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        // набор данных, которые свяжем со списком
        List<CallPhone> calls = new List<CallPhone>();
        ListView callList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // начальная инициализация списка
            setInitialData();
            // получаем элемент ListView
            callList = FindViewById<ListView>(Resource.Id.CallsList);
            // создаем адаптер
            var callsAdapter = new CallPhone.CallPhoneAdapter(this, this, Resource.Layout.layout_call, calls);
            // устанавливаем для списка адаптер
            callList.Adapter = callsAdapter;
            RegisterForContextMenu(callList);
        }

        private void setInitialData()
        {
            Android.Database.Sqlite.SQLiteDatabase db = BaseContext.OpenOrCreateDatabase("base.db", Android.Content.FileCreationMode.Private, null);
            db.ExecSQL("CREATE TABLE IF NOT EXISTS tbCalls (Id INTEGER PRIMARY KEY AUTOINCREMENT, PhoneNum text, DateCall datetime)");
            Android.Database.Sqlite.SQLiteCursor queryCount = db.RawQuery("SELECT count(*) FROM tbCalls", null) as Android.Database.Sqlite.SQLiteCursor;
            queryCount.MoveToFirst();
            if (queryCount.GetInt(0) == 0)
            { 
                db.ExecSQL("INSERT INTO tbCalls(PhoneNum, DateCall) values('79815678978','" + DateTime.Parse("03.08.2021 14:05:56", CultureInfo.GetCultureInfo("ru")).ToString() + "');");
                db.ExecSQL("INSERT INTO tbCalls(PhoneNum, DateCall) values('79085678876','" + DateTime.Parse("13.08.2021 15:00:00", CultureInfo.GetCultureInfo("ru")).ToString() + "');");
                db.ExecSQL("INSERT INTO tbCalls(PhoneNum, DateCall) values('79035557890','" + DateTime.Parse("30.08.2021 16:10:16", CultureInfo.GetCultureInfo("ru")).ToString() + "');");
                db.ExecSQL("INSERT INTO tbCalls(PhoneNum, DateCall) values('79055677877','" + DateTime.Parse("23.08.2021 17:20:28", CultureInfo.GetCultureInfo("ru")).ToString() + "');");
                db.ExecSQL("INSERT INTO tbCalls(PhoneNum, DateCall) values('79076789878','" + DateTime.Parse("03.08.2021 04:30:32", CultureInfo.GetCultureInfo("ru")).ToString() + "');");
            }
            Android.Database.Sqlite.SQLiteCursor query = db.RawQuery("SELECT Id, PhoneNum, DateCall FROM tbCalls", null) as Android.Database.Sqlite.SQLiteCursor;
            if (query.MoveToFirst())
            {
                do
                {
                    calls.Add(new CallPhone(query.GetInt(0), query.GetString(1), DateTime.Parse(query.GetString(2))));
                }
                while (query.MoveToNext());
            }
            query.Close();
            db.Close();
        }

        private void DelRow(int id)
        {
            Android.Database.Sqlite.SQLiteDatabase db = BaseContext.OpenOrCreateDatabase("base.db", Android.Content.FileCreationMode.Private, null);
            db.ExecSQL($"DELETE FROM tbCalls WHERE Id={id.ToString()}");
            db.Close();
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            base.OnCreateContextMenu(menu, v, menuInfo);

            MenuInflater inflater = new MenuInflater(this);
            menu.SetHeaderTitle("Меню");
            inflater.Inflate(Resource.Menu.context_menu, menu);
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            CallPhone row = (callList.Adapter as CallPhone.CallPhoneAdapter).GetRow(info.Position);

            switch (item.ItemId)
            {
                case Resource.Id.mDel:
                    DelRow(row.Id);
                    (callList.Adapter as CallPhone.CallPhoneAdapter).Del(row);
                    (callList.Adapter as CallPhone.CallPhoneAdapter).NotifyDataSetChanged();
                    break;
            }

            return base.OnContextItemSelected(item);
        }
    }
}