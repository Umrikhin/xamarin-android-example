using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ListViewInBtn
{
    public class City
    {
        public string Name { get; set; }
        public bool IsSel { get; set; }
    }

    public class Land: Java.Lang.Object
    {
        public string name { get; set; } // страна
        public List<City> citys { get; set; }  // города
        public Land(string _name, List<City> _citys)
        {
            name = _name;
            citys = _citys;
        }

    }

    public class ItemAdapter : ArrayAdapter
    {
        private Activity activity;

        public ItemAdapter(Activity activity, Context context, int textViewResourceId, List<Land> objects)
            : base(context, textViewResourceId, objects)
        {
            this.activity = activity;
        }

        public void EditItemCity(int Position, List<City> citys)
        {
            if (Position > -1)
            {
                var item = (Land)this.GetItem(Position);

                item.citys = citys;
            }
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var view = (convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.item, parent, false)) as LinearLayout;
            TextView name = (TextView)view.FindViewById<TextView>(Resource.Id.textViewName);
            LinearLayout view_checkboxs = (LinearLayout)view.FindViewById<LinearLayout>(Resource.Id.linearLayoutCheckBoxs);
            Button btnSend = (Button)view.FindViewById<TextView>(Resource.Id.buttonSend);
            var item = (Land)this.GetItem(position);

            name.Text = item.name;

            //Динамически добавляем флажки, если их не было
            view_checkboxs.RemoveAllViews();
            if (view_checkboxs.ChildCount == 0)
            {
                for (int i = 0; i < item.citys.Count; i++)
                {
                    var check = new CheckBox(Context);
                    check.Text = item.citys[i].Name;
                    check.Checked = item.citys[i].IsSel;
                    check.Tag = item.citys[i].Name;
                    check.SetOnCheckedChangeListener(null);
                    check.SetOnCheckedChangeListener(new CheckedChangeListener(position, item.citys, this));
                    view_checkboxs.AddView(check);
                }
            }

            btnSend.Tag = item.name;
            btnSend.SetOnClickListener(null);
            btnSend.SetOnClickListener(new ButtonClickListener(item));
            return view;
        }

        private class ButtonClickListener : Java.Lang.Object, View.IOnClickListener
        {
            private Land land;

            public ButtonClickListener(Land land)
            {
                this.land = land;
            }

            public void OnClick(View v)
            {
                string selectedItemsCount = land.citys.Where(x => x.IsSel).Count().ToString();
                Toast.MakeText(Application.Context, v.Tag + ": " + selectedItemsCount, ToastLength.Long).Show();
            }
        }

        private class CheckedChangeListener : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener
        {
            private int Position;
            private List<City> citys;
            private ItemAdapter adapter;

            public CheckedChangeListener(int Position, List<City> citys, ItemAdapter adapter)
            {
                this.Position = Position;
                this.citys = citys;
                this.adapter = adapter;
            }

            public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
            {
                if (isChecked)
                {
                    string name = (string)buttonView.Tag;
                    string text = string.Format("{0} Отмечено.", name);
                    Toast.MakeText(Application.Context, text, ToastLength.Short).Show();
                    citys.Where(x => x.Name == name).ToList().ForEach(c => c.IsSel = true);
                }
                else
                {
                    string name = (string)buttonView.Tag;
                    string text = string.Format("{0} Снято.", name);
                    Toast.MakeText(Application.Context, text, ToastLength.Short).Show();
                    citys.Where(x => x.Name == name).ToList().ForEach(c => c.IsSel = false);
                }
                adapter.EditItemCity(Position, citys);
            }
        }
    }

}