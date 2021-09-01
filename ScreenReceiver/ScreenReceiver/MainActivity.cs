using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;


namespace ScreenReceiver
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, ScreenListener.ScreenStateListener
    {
        ScreenListener mScreenListener;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            mScreenListener = new ScreenListener(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            mScreenListener.unregisterListener();
        }

        protected override void OnResume()
        {
            base.OnResume();
            mScreenListener.begin(this);

        }
        public void onScreenOn()
        {
            System.Diagnostics.Debug.WriteLine("onScreenOn");
        }

        public void onScreenOff()
        {
            System.Diagnostics.Debug.WriteLine("onScreenOff");
        }

        public void onUserPresent()
        {
            System.Diagnostics.Debug.WriteLine("onUserPresent");
        }

    }
}