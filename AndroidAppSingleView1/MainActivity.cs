using Android.App;
using Android.OS;
using Android.Views.InputMethods;
using Android.Views;
using Java.Interop;
using Android.Runtime;

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

        private void View_LayoutChange(object sender, View.LayoutChangeEventArgs e)
        {
            var view = (View)sender;
            for (var i = 0; i < 1000; i++)
            {
                InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Activity.InputMethodService);
                inputMethodManager.HideSoftInputFromWindow(WindowToken(view), HideSoftInputFlags.None);
                JniPeerMembers.Dispose(inputMethodManager.JniPeerMembers);
            }
        }

        public unsafe IBinder WindowToken(View view)
        {
            Java.Interop.JniObjectReference val = view.JniPeerMembers.InstanceMethods.InvokeVirtualObjectMethod("getWindowToken.()Landroid/os/IBinder;", (IJavaPeerable)(object)view, null);
            System.Reflection.MethodInfo createInstance = typeof(Java.Interop.TypeManager).GetMethod("CreateInstance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static, null, new System.Type[] { typeof(System.IntPtr), typeof(JniHandleOwnership), typeof(System.Type) }, null);
            return (IBinder)createInstance.Invoke(null, new object[] { val.Handle, JniHandleOwnership.TransferLocalRef, typeof(IBinder) });
        }
    }
}
