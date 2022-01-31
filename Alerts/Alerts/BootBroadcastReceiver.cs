using Alerts.Models;
using Android.App;
using Android.Content;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Alerts
{
    [BroadcastReceiver(Name = "com.companyname.alerts.BootBroadcastReceiver")]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class BootBroadcastReceiver : BroadcastReceiver
    {
        static readonly string TAG = "BootBroadcastReceiver";

        public override void OnReceive(Context context, Intent intent)
        {
            var util = new Alarms.AlarmUtil(context);

            List<Alert> lstMyNotyf = new List<Alert>();

            string result = LoadData(out lstMyNotyf);

            foreach (Alert alarm in lstMyNotyf.FindAll(x => x.start >= System.DateTime.Now))
                util.ScheduleAlarm(alarm);
        }

        public string LoadData(out List<Alert> lstAlarm)
        {
            string result = string.Empty;
            lstAlarm = new List<Alert>();
            try
            {
                SQLiteDatabase _db = Application.Context.OpenOrCreateDatabase("alertstore.db", Android.Content.FileCreationMode.Private, null);
                SQLiteCursor cursor = _db.RawQuery("SELECT * FROM " + DatabaseHelper.TABLE, null) as SQLiteCursor;
                while (cursor.MoveToNext())
                {
                    lstAlarm.Add(new Alert()
                    {
                        id = cursor.GetInt(cursor.GetColumnIndexOrThrow(DatabaseHelper.COLUMN_ID)),
                        title = cursor.GetString(cursor.GetColumnIndexOrThrow(DatabaseHelper.COLUMN_TITLE)),
                        note = cursor.GetString(cursor.GetColumnIndexOrThrow(DatabaseHelper.COLUMN_NOTE)),
                        start = DateTime.Parse(cursor.GetString(cursor.GetColumnIndexOrThrow(DatabaseHelper.COLUMN_START)), CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None)
                    });
                }
                cursor.Close();
                _db.Close();
            }
            catch (Exception err)
            {
                result = err.Message;
            }
            return result;
        }
    }
}