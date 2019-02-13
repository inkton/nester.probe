using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Nester.Probe
{
    public class AuthWebView : WebView
    {
        public static readonly BindableProperty UsernameProperty = BindableProperty.Create(
        propertyName: "Username",
            returnType: typeof(string),
            declaringType: typeof(AuthWebView),
          defaultValue: default(string));

        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public static readonly BindableProperty PasswordProperty = BindableProperty.Create(
        propertyName: "Password",
            returnType: typeof(string),
            declaringType: typeof(AuthWebView),
          defaultValue: default(string));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
    }
}