using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microcharts;
using SkiaSharp;

namespace Nester.Probe
{
    public partial class MainPage : ContentPage
    {
        private ProbeViewModel _viewModel;

        public MainPage(ProbeViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _viewModel.IsBusy = false;

            BindingContext = _viewModel;

            StatusButton.Clicked += StatusButton_ClickedAsync;
            InspectButton.Clicked += InspectButton_ClickedAsync;
            AboutButton.Clicked += AboutButton_ClickedAsync;

            LogAge.SelectedIndexChanged += LogAge_SelectedIndexChangedAsync;

            FinalizeLogResults();
        }

        public static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }

        async private void LogAge_SelectedIndexChangedAsync(object sender, EventArgs e)
        {
            int hours = (LogAge.SelectedIndex + 1) * 8;
            await _viewModel.Logs.QueryAsync(hours, 100 * hours);
        }

        async private void StatusButton_ClickedAsync(object sender, EventArgs e)
        {
            _viewModel.IsBusy = true;
            await Navigation.PushModalAsync(new StatusPage(_viewModel));
            _viewModel.IsBusy = false;
        }

        async private void InspectButton_ClickedAsync(object sender, EventArgs e)
        {
            if (_viewModel.Logs.Selected != null)
            {
                _viewModel.IsBusy = true;
                await Navigation.PushModalAsync(new LogItemPage(_viewModel));
                _viewModel.IsBusy = false;
            }
        }

        async private void AboutButton_ClickedAsync(object sender, EventArgs e)
        {
            _viewModel.IsBusy = true;
            await Navigation.PushModalAsync(new AboutPage(_viewModel));
            _viewModel.IsBusy = false;
        }

        private void FinalizeLogResults()
        {
            LogAge.SelectedIndex = 0;
            List<Microcharts.Entry> entries = new List<Microcharts.Entry>();

            if (_viewModel.Logs.List.Any())
            {
                _viewModel.Logs.Selected = _viewModel.Logs.List.First();
                Dictionary<string, SkiaSharp.SKColor> colorMap = new Dictionary<string, SKColor>();
                colorMap["FATAL"] = SKColor.Parse("#e74c3c");
                colorMap["ERROR"] = SKColor.Parse("#e67e22");
                colorMap["FAIL"] = SKColor.Parse("#95a5a6");
                colorMap["WARN"] = SKColor.Parse("#2c3e50");
                colorMap["INFO"] = SKColor.Parse("#3498db");
                colorMap["DEBUG"] = SKColor.Parse("#7f8c8d");
                colorMap["TRACE"] = SKColor.Parse("#16a085");

                foreach (var line in _viewModel.Logs.List.GroupBy(info => info.Level)
                                        .Select(group => new {
                                            Metric = group.Key,
                                            Count = group.Count()
                                        })
                                        .OrderBy(x => x.Metric))
                {
                    entries.Add(new Microcharts.Entry(line.Count)
                    {
                        Label = line.Metric,
                        ValueLabel = line.Count.ToString(),
                        Color = colorMap[line.Metric]
                    });
                }
            }

            ChartView.Chart = new PieChart() { Entries = entries };
        }
    }
}
