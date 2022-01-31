using Android.App;
using Android.Content;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alerts
{
    public class DatabaseHelper : SQLiteOpenHelper

    {
        static string DATABASE_NAME = "alertstore.db"; // название бд
        static int SCHEMA = 1; // версия базы данных
        public static string TABLE = "alerts"; // название таблицы в бд
        // названия столбцов
        public static string COLUMN_ID = "_id";
        public static string COLUMN_TITLE = "title";
        public static string COLUMN_NOTE = "note";
        public static string COLUMN_START = "start";
        public DatabaseHelper(Context context) : base(context, DATABASE_NAME, null, SCHEMA) { }
        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL("CREATE TABLE alerts (" + COLUMN_ID
                + " INTEGER PRIMARY KEY AUTOINCREMENT," + COLUMN_TITLE
                + " TEXT, " + COLUMN_NOTE + " TEXT, " + COLUMN_START + " TEXT);");
        }
        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL("DROP TABLE IF EXISTS " + TABLE);
            OnCreate(db);
        }
    }
}