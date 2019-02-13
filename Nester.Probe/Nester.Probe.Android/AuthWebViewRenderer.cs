using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Android.Content;
using Android.Graphics;
using Android.Webkit;
using Nester.Probe;
using Nester.Probe.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WebView = Xamarin.Forms.WebView;

[assembly: ExportRenderer(typeof(AuthWebView), typeof(AuthWebViewRenderer))]
namespace Nester.Probe.Droid
{
    public class AuthWebViewRenderer : Xamarin.Forms.Platform.Android.WebViewRenderer
    {
        AuthWebViewClient _authWebClient = null;

        public AuthWebViewRenderer(Context context):base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var webView = e.NewElement as AuthWebView;

                _authWebClient = new AuthWebViewClient(
                    webView.Username, webView.Password);
            }
            Control.SetWebViewClient(_authWebClient);
        }
    }
    public class AuthWebViewClient : WebViewClient
    {
        private string Username
        {
            get;
        }
        private string Password
        {
            get;
        }
        public AuthWebViewClient(string username, string password)
        {
            Username = username;
            Password = password;
        }
        public override void OnReceivedHttpAuthRequest(Android.Webkit.WebView view, HttpAuthHandler handler, string host, string realm)
        {
            handler.Proceed(
                Username,
                Password);
        }
    }
}