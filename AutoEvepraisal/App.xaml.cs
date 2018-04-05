using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AutoEvepraisal
{
    public partial class App : Application
    {
        private const string ItemLineRegexFormat = "^[0-9]+ [A-Za-z0-9\\- ]+$";

        private HttpClient _httpClient;
        private string _lastClipboardContents;

        public App()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://evepraisal.com");
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };

            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                var clipboardText = Clipboard.GetText();
                if (_lastClipboardContents != clipboardText)
                {
                    _lastClipboardContents = clipboardText;

                    var match = Regex.Match(clipboardText, ItemLineRegexFormat, RegexOptions.Multiline);
                    if (match.Success)
                    {
                        var response = _httpClient.PostAsync($"appraisal.json?market=jita&raw_textarea={Uri.EscapeUriString(clipboardText)}&persist=no", new StringContent(String.Empty)).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            var popup = new Popup();
                            popup.Text = response.Content.ReadAsStringAsync().Result;
                            popup.Show();

                            var hideTimer = new DispatcherTimer
                            {
                                Interval = TimeSpan.FromSeconds(4),
                            };
                            hideTimer.Tick += (s, _) =>
                            {
                                popup.Close();
                            };
                            hideTimer.Start();
                        }
                    }
                }
            }
        }
    }
}
