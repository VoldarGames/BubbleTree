using Android.App;
using Android.Content.PM;
using Android.OS;
using BubbleTreeAppTest.Droid;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(GenericButtonRenderer))]
namespace BubbleTreeAppTest.Droid
{
    [Activity(Label = "BubbleTreeAppTest", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }

    

    
        public class GenericButtonRenderer : Xamarin.Forms.Platform.Android.ButtonRenderer
        {
        }
    
}

