using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;

namespace ListViewInBtn
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, ConfigurationChanges = Android.Content.PM.ConfigChanges.ScreenSize | Android.Content.PM.ConfigChanges.Orientation)]
    public class MainActivity : AppCompatActivity
    {
        List<Land> lands = new List<Land>();
        ListView listViewMain;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            setInitialData();

            // получаем элемент ListView
            listViewMain = FindViewById<ListView>(Resource.Id.listViewMain);

            // создаем адаптер
            var landAdapter = new ItemAdapter(this, this, Resource.Layout.item, lands);

            // устанавливаем для списка адаптер
            listViewMain.Adapter = landAdapter;

        }

        private void setInitialData()
        {
            lands.Add(new Land("Россия", new List<City>() { new City() { Name = "Москва", IsSel = true }, new City() { Name = "Новгород", IsSel = false }, new City() { Name = "Воронеж", IsSel = false }, new City() { Name = "Волгоград", IsSel = false }, new City() { Name = "Краснодар", IsSel = false }, new City() { Name = "И еще множество городов", IsSel = false }
            }));

            lands.Add(new Land("Украина", new List<City>() { new City() { Name = "Киев", IsSel = false }, new City() { Name = "Полтава", IsSel = false }, new City() { Name = "Ровно", IsSel = false }
            }));

            lands.Add(new Land("Германия", new List<City>() { new City() { Name = "Берлин", IsSel = false }, new City() { Name = "Мюнхен", IsSel = false }, new City() { Name = "Гамбург", IsSel = false }, new City() { Name = "Бремен", IsSel = false }, new City() { Name = "Дрезден", IsSel = false }
            }));

            lands.Add(new Land("Италия", new List<City>() { new City() { Name = "Рим", IsSel = false }, new City() { Name = "Милан", IsSel = false }, new City() { Name = "Сан-Ремо", IsSel = false }, new City() { Name = "Палермо", IsSel = false }
            }));

            lands.Add(new Land("Великобритания", new List<City>() { new City() { Name = "Лондон", IsSel = false }, new City() { Name = "Манчестер", IsSel = false }
            }));

            lands.Add(new Land("Франция", new List<City>() { new City() { Name = "Париж", IsSel = false }, new City() { Name = "Прованс", IsSel = false }, new City() { Name = "Канны", IsSel = false }, new City() { Name = "Какой угодно город. Во Франции много красивых городов.", IsSel = false }
            }));
        }

    }
}