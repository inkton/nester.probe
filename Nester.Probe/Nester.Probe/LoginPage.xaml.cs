using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nester.Probe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private ProbeViewModel _viewModel;
        const int DefaultHours = 8;

        public LoginPage(ProbeViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            BindingContext = _viewModel;
            _viewModel.IsBusy = false;

            LoginButton.Clicked += LoginButton_ClickedAsync;
        }       
       
        async private void LoginButton_ClickedAsync(object sender, EventArgs e)
        {
            IsProgressing();

            try
            {
                _viewModel.InitEndpoint();

                await _viewModel.Logs.QueryAsync(DefaultHours, 100 * DefaultHours);

                await _viewModel.SaveSettingsAsync();

                Application.Current.MainPage = new MainPage(_viewModel);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Authentication", ex.Message, "OK");
            }

            IsProgressing(false);
        }

        private void IsProgressing(bool isBusy = true)
        {
            _viewModel.IsBusy = isBusy;
            LoginButton.IsEnabled = !isBusy;
        }
    }    
}