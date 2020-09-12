using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using BranchXamarinSDK;
using LocalyticsXamarin.Android;

namespace MobileAppFormsTabbed1.Droid
{

    [Application(Theme = "@android:style/Theme.Holo.Light")]
    public class CustomApplication : Application
    {
        public CustomApplication(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            BranchAndroid.GetAutoInstance(this.ApplicationContext);

            Localytics.SetOption("ll_app_key", "contoso");
            Localytics.AutoIntegrate(this);
            BranchAndroid.GetAutoInstance(this.ApplicationContext);
        }
    }
}
