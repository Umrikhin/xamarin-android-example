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

namespace SQLiteDelRow
{
    public class CallPhone : Java.Lang.Object
    {
        public int Id { get; set; } //Идентификатор
        public string phone { get; set; } // Телефон
        public DateTime time { get; set; }  // Время вызова
        public CallPhone(int _id, string _phone, DateTime _time)
        {
            Id = _id;
            phone = _phone;
            time = _time;
        }

        public class CallPhoneAdapter : ArrayAdapter

        {
            private Activity activity;
            public CallPhoneAdapter(Activity activity, Context context, int viewResourceId, List<CallPhone> objects)
                : base(context, viewResourceId, objects)
            {
                this.activity = activity;
            }

            public CallPhone GetRow(int Position)
            {
                return (CallPhone)this.GetItem(Position);
            }

            public void Del(CallPhone _call)
            {
                this.Remove(_call);
            }

            public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
            {
                var view = (convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.layout_call, parent, false)) as LinearLayout;
                TextView phoneView = (TextView)view.FindViewById<TextView>(Resource.Id.phone);
                TextView timeView = (TextView)view.FindViewById<TextView>(Resource.Id.time);
                var item = (CallPhone)this.GetItem(position);
                phoneView.Text = "Телефон: "+ item.phone;
                timeView.Text = "Время вызова: " +item.time.ToString("dd.MM.yyyy HH:mm:ss");
                return view;
            }
        }
    }
}