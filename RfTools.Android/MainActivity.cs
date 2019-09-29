using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using RfTools.App.Fragments;

namespace RfTools.App
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        TextView textMessage;
        Fragment_Home fragment_Home;
        Fragment_Result fragment_Result;
        Fragment_Settings fragment_Settings;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            textMessage = FindViewById<TextView>(Resource.Id.message);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            fragment_Home = new Fragment_Home();
            fragment_Result = new Fragment_Result();
            fragment_Settings = new Fragment_Settings();
            //把主界面fragment加入activi界面
            SupportFragmentManager.BeginTransaction().Add(Resource.Id.container, fragment_Home, "home").Commit();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            //FragmentManager .BeginTransaction().Replace
            

            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, fragment_Home,"home").Commit();
                    return true;
                case Resource.Id.navigation_dashboard:
                    //textMessage.SetText(Resource.String.title_dashboard);
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, fragment_Result, "result").Commit();

                    return true;
                case Resource.Id.navigation_notifications:
                    //textMessage.SetText(Resource.String.title_notifications);
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, fragment_Settings, "settings").Commit();

                    return true;
            }
            return false;
        }
    }
}

