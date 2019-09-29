using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
//using Android.App;
using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace RfTools.App.Fragments
{
    public class Fragment_Result : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState) => base.OnCreate(savedInstanceState);// Create your fragment here

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) =>
            // Use this to return your custom view for this Fragment
            inflater.Inflate(Resource.Layout.fragment_result, container, false);
    }
}