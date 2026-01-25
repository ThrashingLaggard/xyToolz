using System.Collections.ObjectModel;
using OpenQA.Selenium;
using xyToolz.Driver;

namespace xyToolz.Browser
{
    /// <summary>
    /// Use-case facade for browser automation.
    /// Pure pass-through to internal Selenium driver.
    /// </summary>
    public sealed class Browser : IDisposable
    {
        private readonly xyBrowserDriver _driver;

        // Lifecycle 

        public Browser(int timeoutSeconds = 10)
            => _driver = new xyBrowserDriver(timeoutSeconds);

        public void Dispose()
            => _driver.Dispose();

        // Navigation 

        public Task GoToAsync(string url)
            => _driver.NavigateTo(url);

        // Ellement lookup 

        public Task<IWebElement> FindAsync(By by)
            => _driver.FindElement(by);

        public Task<ReadOnlyCollection<IWebElement>> FindAllAsync(By by)
            => _driver.FindElements(by);

        // Interaction

        public Task<bool> ClickAsync(By by)
            => _driver.Click(by);

        public Task TypeAsync(By by, string text)
            => _driver.EnterText(by, text);

        public Task CheckAsync(By by)
            => _driver.CheckCheckBox(by);

        public Task UncheckAsync(By by)
            => _driver.UncheckCheckbox(by);

        // Dropdowns

        public Task SelectAsync(By by, string value, SelectBy mode)
            => _driver.SelectDropdownOption(by, value, (int)mode);

        // Scripts 

        public static Task ExecuteScriptAsync(string script, params object[] args)
            => xyBrowserDriver.ExecuteScript(script, args);

        public static Task<T> ExecuteScriptAsync<T>(string script, params object[] args)
            => xyBrowserDriver.ExecuteScript<T>(script, args);

        // Screenshots 

        public Task ScreenshotAsync(string path)
            => _driver.TakeScreenshot(path);
    }
}
