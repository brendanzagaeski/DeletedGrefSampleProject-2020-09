using Android.App;
using Android.OS;
using Android.Views;

namespace AndroidAppSingleView1
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            View view = FindViewById(Resource.Id.textView1);
            view.LayoutChange += View_LayoutChange;
        }

        static void View_LayoutChange(object sender, View.LayoutChangeEventArgs e)
        {
            var view = (View)sender;
            TernaryThenCollect(view.WindowToken);
        }

        static void TernaryThenCollect(IBinder windowToken)
        {
            System.IntPtr ptr = windowToken == null ? System.IntPtr.Zero : System.IntPtr.Zero;
            // `windowToken` should not be eligible for collection during this GC, but it is.
            System.GC.Collect();
        }
    }
}
