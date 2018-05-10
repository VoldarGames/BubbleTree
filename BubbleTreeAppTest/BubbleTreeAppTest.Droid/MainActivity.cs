using Android.App;
using Android.Content.PM;
using Android.OS;
using BubbleTreeAppTest.Android;
using Xamarin.Forms;
using Android.Content;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Button), typeof(GenericButtonRenderer))]
namespace BubbleTreeAppTest.Android
{
    [Activity(Label = "BubbleTreeAppTest", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
    }

    public class GenericButtonRenderer : ButtonRenderer
    {
        public GenericButtonRenderer(Context context) : base(context) { }
    }
}

