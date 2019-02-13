using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Inkton.Nest.Cloud;
using Inkton.Nest.Model;
using Inkton.Nester;
using Inkton.Nester.Cloud;
using Inkton.Nester.ViewModels;

namespace Nester.Probe
{   
    public class NesterLogs : ViewModel
    {
        private ObservableCollection<NestLog> _list;
        private INesterControl _nesterControl = null;
        private NestLog _logItem = null;

        public NesterLogs(INesterControl nesterControl)
        {
            _nesterControl = nesterControl;
        }

        public NestLog Selected
        {
            get { return _logItem; }
            set
            {
                SetProperty(ref _logItem, value, "Selected");
            }
        }

        public ObservableCollection<NestLog> List
        {
            get { return _list; }
        }

        public async Task<ResultMultiple<NestLog>> QueryAsync(int hours, int limit = 200)
        {
            DateTime fromTimeUTC = DateTime.UtcNow.Subtract(new TimeSpan(hours, 0, 0));
            long unixEpochSecsSince = fromTimeUTC.ToUnixTime();

            // nest_log table id represents ms since epoch
            string sql = FormSql("nest_log", "*", string.Format("id > {0}",
                        unixEpochSecsSince * 1000000 // to microseconds
                    ), "id asc", limit);

            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("sql", sql);

            NestLog logsSeed = new NestLog();
            ResultMultiple<NestLog> result = await ResultMultipleUI<NestLog>.WaitForObjectAsync(
                _nesterControl.Backend, true, logsSeed, false, data);

            if (result.Code == 0)
            {
                _list = result.Data.Payload;
            }

            return result;
        }

        private string FormSql(string table, string fields = "*",
            string filter = null, string orderBy = null, int limit = -1)
        {
            string sql = string.Format("select {0} from {1}", fields, table);

            if (filter != null)
            {
                sql += " where " + filter;
            }
            if (orderBy != null)
            {
                sql += " order by " + orderBy;
            }
            if (limit >= 0)
            {
                sql += " limit " + limit.ToString();
            }
            return sql;
        }
    }

    public class ProbeViewModel : ViewModel
    {
        public string _appTag;
        public string _password;
        public NesterLogs _logs;

        public ProbeViewModel()
        {
            Logs = new NesterLogs(NesterControl);
        }

        public string AppTag
        {
            get { return _appTag; }
            set
            {
                SetProperty(ref _appTag, value, "AppTag");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value, "Password");
            }
        }

        public string MetricsUrl
        {
            get { return string.Format(
                "https://{0}.nestapp.yt/metrics_raw/", _appTag); }
        }

        public NesterLogs Logs
        {
            get { return _logs; }
            set { _logs = value; }
        }

        public void InitEndpoint()
        {
            NesterControl.Backend.Endpoint =
                string.Format("https://{0}.nestapp.yt/", _appTag);

            NesterControl.Backend.BasicAuth = new BasicAuth(
                true, _appTag, _password);
        }

        async public Task SaveSettingsAsync()
        {
            Application.Current.Properties["AppTag"] = _appTag;
            Application.Current.Properties["Password"] = _password;
            await Application.Current.SavePropertiesAsync();
        }

        public void RestoreSettings()
        {
            AppTag = Application.Current.Properties["AppTag"] as string;
            Password = Application.Current.Properties["Password"] as string;
        }
    }
}
