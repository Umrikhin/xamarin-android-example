using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alerts.Models
{
    public class Alert : Java.Lang.Object
    {
        public int id { get; set; } //идентификатор
        public string title { get; set; } // наименование
        public string note { get; set; } // описание
        public DateTime start { get; set; }  // время запуска
    }

    public class AlertAdapter : ArrayAdapter

    {
        private Activity activity;
        public AlertAdapter(Activity activity, Context context, int viewResourceId, List<Alert> objects)
            : base(context, viewResourceId, objects)
        {
            this.activity = activity;
        }
        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var view = (convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.list_item, parent, false)) as LinearLayout;
            TextView titleView = (TextView)view.FindViewById<TextView>(Resource.Id.title);
            TextView startView = (TextView)view.FindViewById<TextView>(Resource.Id.start);
            var item = (Alert)this.GetItem(position);
            titleView.Text = item.title;
            startView.Text = item.start.ToString("dd:MM:yyyy HH:mm");
            return view;
        }
    }
}