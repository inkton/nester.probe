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
	public partial class StatusPage : ContentPage
	{
        private ProbeViewModel _viewModel;

        public StatusPage (ProbeViewModel viewModel)
		{
			InitializeComponent ();

            _viewModel = viewModel;
            BindingContext = _viewModel;
        }
	}
}