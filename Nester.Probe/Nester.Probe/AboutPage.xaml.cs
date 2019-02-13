using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nester.Probe
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutPage : ContentPage
	{
        private ProbeViewModel _viewModel;

        public AboutPage (ProbeViewModel viewModel)
		{
			InitializeComponent ();

            _viewModel = viewModel;
            _viewModel.IsBusy = false;

            BindingContext = _viewModel;

            InfoLabel.Text = typeof(AboutPage).GetTypeInfo()
                .Assembly.GetName().Name;

            VersionLabel.Text = "Version " + typeof(AboutPage).GetTypeInfo()
                .Assembly.GetName().Version.ToString();

            SettingsButton.Clicked += SettingsButton_ClickedAsync;

            ShowSettings();
        }

        public void ShowProgressing(string notice)
        {
            // the app is in transition. the
            // notice popped until it lasts.
            _viewModel.IsBusy = true;
            MessageLabel.IsVisible = true;
            MessageLabel.Text = notice;
            SettingsButton.IsVisible = false;
        }

        public void ShowSettings()
        {
            _viewModel.IsBusy = false;
            MessageLabel.IsVisible = false;
            SettingsButton.IsVisible = true;
        }

        async void SettingsButton_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SettingsPage(_viewModel));
        }
    }
}