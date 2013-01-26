using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using ChromeCanaryBrowser.Properties;
using Microsoft.WebMatrix.Extensibility;

namespace ChromeCanaryBrowser 
{
    [Export(typeof(IBrowser))]
    class Browser : IBrowser
    {
        private const string BrowserPath = @"%localappdata%\Google\Chrome SxS\Application\";
        private const string ExeName = "chrome.exe";
        private const string CanaryDisplayName = "Google Chrome Canary";
        private const string CanaryDownloadUrl = @"https://www.google.com/intl/en/chrome/browser/canary.html";

        public Browser()
        {
        }

        private bool IsCanaryInstalled()
        {
            var path = GetCommand();
            return File.Exists(path);
        }

        private static string GetCommand()
        {
            var installPath = Path.Combine(BrowserPath, ExeName);
            var path = Environment.ExpandEnvironmentVariables(installPath);
            return path;
        }

        public void Browse(string url)
        {
            if (this.IsCanaryInstalled())
            {
                var processStartInfo = new ProcessStartInfo();
                processStartInfo.UseShellExecute = false;
                processStartInfo.FileName = GetCommand();
                processStartInfo.Arguments = url;
                try
                {
                    var process = Process.Start(processStartInfo);
                }
                catch (Exception)
                {
                }
            }
            else
            {
                Process.Start(CanaryDownloadUrl);
            }
        }

        public string DisplayName
        {
            get { return CanaryDisplayName; }
        }

        public System.Windows.Media.ImageSource Image
        {
            get { return Utility.ConvertBitmapToImageSource(Resources.ChromeCanary); }
        }

        public string Name
        {
            get { return ExeName; }
        }
    }
}
