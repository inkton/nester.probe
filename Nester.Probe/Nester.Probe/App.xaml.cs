using System;
using System.IO;
using System.Resources;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Newtonsoft.Json;
using Inkton.Nester;
using Inkton.Nester.Cloud;
using Inkton.Nester.Helpers;
using Inkton.Nester.Storage;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Nester.Probe
{
    public partial class App : Application, INesterControl
    {
        private const int ApiVersion = 2;
        private LogService _log;
        private NesterService _backend;
        const int DefaultHours = 8;

        public App()
        {
            InitializeComponent();

            _log = new LogService(Path.Combine(
                    Path.GetTempPath(), "NesterProbeLog"));

            /* 
             * Query objects can be cached locally. Specify the 
             * location of the cache here
             */
            StorageService cache = new StorageService(Path.Combine(
                    Path.GetTempPath(), "NesterProbeCache"));
            cache.Clear();

            _backend = new NesterService(
                ApiVersion, DeviceInfo.Name, cache);

            LaunchWindowAsync();
        }

        public NesterService Backend
        {
            get { return _backend; }
        }

        public LogService Log
        {
            get { return _log; }
        }

        public ResourceManager GetResourceManager()
        {
            ResourceManager resmgr = new ResourceManager(
                  "Nester.Probe",
                  typeof(App).GetTypeInfo().Assembly);
            return resmgr;
        }

        async private void LaunchWindowAsync()
        {
            ProbeViewModel viewModel = new ProbeViewModel();
            AboutPage aboutPage = new AboutPage(viewModel);
            aboutPage.ShowProgressing("Please wait ...");

            MainPage = aboutPage;

            bool authenticated = Application
                .Current.Properties.ContainsKey("AppTag");

            if (authenticated)
            {
                try
                {
                    viewModel.RestoreSettings();

                    viewModel.InitEndpoint();

                    await viewModel.Logs.QueryAsync(DefaultHours, 100 * DefaultHours);
                }
                catch (Exception)
                {
                    authenticated = false;
                }
            }

            if (!authenticated)
            {
                MainPage = new LoginPage(viewModel);
            }
            else
            {
                MainPage = new MainPage(viewModel);
            }
        }
    }
}
