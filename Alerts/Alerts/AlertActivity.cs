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
    [Activity(Label = "AlertActivity", ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation)]
    public class AlertActivity : Activity
    {
        EditText titleBox;
        EditText noteBox;
        EditText startBox;
        Button buttonDel;
        Button buttonSave;
        DatabaseHelper sqlHelper;
        SQLiteDatabase db;
        SQLiteCursor alertCursor;
        int alertId = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_alarm);
            titleBox = FindViewById<EditText>(Resource.Id.title);
            noteBox = FindViewById<EditText>(Resource.Id.note);
            startBox = FindViewById<EditText>(Resource.Id.start);
            buttonDel = FindViewById<Button>(Resource.Id.buttonDel);
            buttonDel.Click += (o, e) => { delete(); };
            buttonSave = FindViewById<Button>(Resource.Id.buttonSave);
            buttonSave.Click += (o, e) => {
                DateTime d = DateTime.Now;
                if (!DateTime.TryParse(startBox.Text, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out d))
                {
                    Toast.MakeText(Application.Context, "Не врено задано выражение для времени запуска. Укажите время в дд.ММ.гггг ЧЧ:мм:cc", ToastLength.Short).Show();
                    return;
                }
                if (titleBox.Text.Trim().Length == 0)
                {
                    Toast.MakeText(Application.Context, "Укажите заголовок", ToastLength.Short).Show();
                    return;
                }
                if (noteBox.Text.Trim().Length == 0)
                {
                    Toast.MakeText(Application.Context, "Укажите описание", ToastLength.Short).Show();
                    return;
                }
                save(); 
            };
            sqlHelper = new DatabaseHelper(this);
            db = sqlHelper.WritableDatabase;
            Bundle extras = Intent.Extras;
            if (extras != null)
            {
                alertId = extras.GetInt("id");
            }
            // если 0, то добавление
            if (alertId > 0)
            {
                // получаем элемент по id из бд
                alertCursor = db.RawQuery("select * from " + DatabaseHelper.TABLE + " where " +
                        DatabaseHelper.COLUMN_ID + "=?", new string[] { alertId.ToString() }) as SQLiteCursor;
                alertCursor.MoveToFirst();
                titleBox.Text = alertCursor.GetString(1);
                noteBox.Text = alertCursor.GetString(2);
                startBox.Text = DateTime.Parse(alertCursor.GetString(3), CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None).ToString("dd.MM.yyyy HH:mm:ss");
                alertCursor.Close();
            }
            else
            {
                // скрываем кнопку удаления
                buttonDel.Visibility = ViewStates.Gone;
                startBox.Text = DateTime.Now.AddMinutes(5).ToString("dd.MM.yyyy HH:mm:ss");
            }
        }
        public void save()
        {
            ContentValues cv = new ContentValues();
            cv.Put(DatabaseHelper.COLUMN_TITLE, titleBox.Text);
            cv.Put(DatabaseHelper.COLUMN_NOTE, noteBox.Text);
            cv.Put(DatabaseHelper.COLUMN_START, startBox.Text);
            if (alertId > 0)
            {
                db.Update(DatabaseHelper.TABLE, cv, DatabaseHelper.COLUMN_ID + "=" + alertId.ToString(), null);
                var alert = new Models.Alert() { id = alertId, title = titleBox.Text, note = noteBox.Text, start = DateTime.Parse(startBox.Text, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None) };
                Alarms.AlarmUtil alarmUtil = new Alarms.AlarmUtil(Application.Context);
                alarmUtil.CancelAlarm(alert);
                alarmUtil.ScheduleAlarm(alert);
            }
            else
            {
                db.Insert(DatabaseHelper.TABLE, null, cv);
                Alarms.AlarmUtil alarmUtil = new Alarms.AlarmUtil(Application.Context);
                // получаем максимальный элемент по id из бд
                var cursor = db.RawQuery("select max(" + DatabaseHelper.COLUMN_ID + ") from " + DatabaseHelper.TABLE, null) as SQLiteCursor;
                cursor.MoveToFirst();
                var newId = cursor.GetInt(0);
                alarmUtil.ScheduleAlarm(new Models.Alert() { id = newId, title = titleBox.Text, note = noteBox.Text, start = DateTime.Parse(startBox.Text, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None) });
            }
            goBack();
        }
        public void delete()
        {
            db.Delete(DatabaseHelper.TABLE, "_id = ?", new string[] { alertId.ToString() });
            var alert = new Models.Alert() { id = alertId, title = titleBox.Text, note = noteBox.Text, start = DateTime.Parse(startBox.Text, CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None) };
            Alarms.AlarmUtil alarmUtil = new Alarms.AlarmUtil(Application.Context);
            alarmUtil.CancelAlarm(alert);
            goBack();
        }
        private void goBack()
        {
            // закрываем подключение
            db.Close();
            // переход к главной activity
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
            StartActivity(intent);
        }
    }
}