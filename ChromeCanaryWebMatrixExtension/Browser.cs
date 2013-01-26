//-----------------------------------------------------------------------
// <copyright file="Browser.cs" company="CompanyName">
//     CopyRight(c) Peter Tate, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using ChromeCanaryBrowser.Properties;
using Microsoft.WebMatrix.Extensibility;

namespace ChromeCanaryBrowser 
{
    /// <summary>
    /// Implements an instance of the Chrome Canary Browser
    /// </summary>
    [Export(typeof(IBrowser))]
    public class Browser : IBrowser
    {
        /// <summary>
        /// The path to the Chrome Canary Install location
        /// </summary>
        private const string BrowserPath = @"%localappdata%\Google\Chrome SxS\Application\";

        /// <summary>
        /// The name of the Chrome Canary executable
        /// </summary>
        private const string ExeName = "chrome.exe";

        /// <summary>
        /// The Display name in WebMatrix
        /// </summary>
        private const string CanaryDisplayName = "Google Chrome Canary";

        /// <summary>
        /// The canary download URL if the user does not have Chrome Canary installed
        /// </summary>
        private const string CanaryDownloadUrl = @"https://www.google.com/intl/en/chrome/browser/canary.html";

        /// <summary>
        /// Initializes a new instance of the <see cref="Browser"/> class.
        /// </summary>
        public Browser()
        {
        }

        /// <summary>
        /// Gets the short name displayed on menus.
        /// </summary>
        public string DisplayName
        {
            get { return CanaryDisplayName; }
        }

        /// <summary>
        /// Gets the image to be displayed on the menu and overlaid on run button.
        /// </summary>
        public System.Windows.Media.ImageSource Image
        {
            get { return Utility.ConvertBitmapToImageSource(Resources.ChromeCanary); }
        }

        /// <summary>
        /// Gets the un-localized ID of the browser.
        /// </summary>
        public string Name
        {
            get { return ExeName; }
        }

        /// <summary>
        /// Called to cause the browser to display the specified URL in Chrome Canary.
        /// </summary>
        /// <param name="url">The location to browse to.</param>
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

        /// <summary>
        /// Gets the full command line to start the browser.
        /// </summary>
        /// <returns>The command line as a string</returns>
        private static string GetCommand()
        {
            var installPath = Path.Combine(BrowserPath, ExeName);
            var path = Environment.ExpandEnvironmentVariables(installPath);
            return path;
        }

        /// <summary>
        /// Determines whether Chrome Canary is installed.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if Chrome Canary is installed; otherwise, <c>false</c>.
        /// </returns>
        private bool IsCanaryInstalled()
        {
            var path = GetCommand();
            return File.Exists(path);
        }
    }
}
