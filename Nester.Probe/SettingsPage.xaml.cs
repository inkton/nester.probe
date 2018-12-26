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
	public partial class SettingsPage : ContentPage
	{
        private ProbeViewModel _viewModel;

        public SettingsPage(ProbeViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _viewModel.IsBusy = false;

            BindingContext = _viewModel;

            SaveButton.Clicked += SaveButton_ClickedAsync;
            DefaultsButton.Clicked += DefaultsButton_ClickedAsync;
        }

        async private void DefaultsButton_ClickedAsync(object sender, EventArgs e)
        {
            var yes = await DisplayAlert("Alert", 
                "This will reset all settings including\nthe currently authenticated session. Do you wish to proceed? ", "Yes", "No");

            if (yes)
            {
                _viewModel.AppTag = string.Empty;
                _viewModel.Password = string.Empty;

                await _viewModel.SaveSettingsAsync();

                await DisplayAlert("Alert", "Please exit the app and re-login", "Okay");
            }
        }

        async private void SaveButton_ClickedAsync(object sender, EventArgs e)
        {
            await _viewModel.SaveSettingsAsync();

            await DisplayAlert("Settings", "All settings saved", "Okay");

        }
    }
}