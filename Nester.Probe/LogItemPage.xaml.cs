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
	public partial class LogItemPage : ContentPage
	{
        private ProbeViewModel _viewModel;

        public LogItemPage(ProbeViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _viewModel.IsBusy = false;

            BindingContext = _viewModel;
        }
    }
}