using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;



namespace xyToolz
{
    internal class xyWebDriver
    {


        public class BrowserDriver : IDisposable
        {
            private static IWebDriver _driver;
            private readonly WebDriverWait _wait;

            public BrowserDriver(int timeoutSeconds = 10)
            {
                TimeSpan timeout = TimeSpan.FromSeconds(timeoutSeconds);
                ChromeOptions chromeOptions = new();
                chromeOptions.AddArgument("--start-maximized");
                _driver = new ChromeDriver(chromeOptions);
                _wait = new WebDriverWait(_driver, timeout);
            }

            public async Task NavigateTo(string url)
            {
                try
                {
                    await _driver.Navigate().GoToUrlAsync(url);
                }
                catch (Exception ex)
                {
                    await xyLog.AsxExLog(ex);
                }
            }

            public async Task<IWebElement> FindElement(By by)
            {
                try
                {
                    if (_wait.Until(driver => driver.FindElement(by)) is IWebElement targetElement)
                    {
                        return targetElement;
                    }
                    else
                    {
                        string fail = $"Found nothíng while looking for {by.ToString} => {by.Mechanism}";
                        await xyLog.AsxLog(fail);
                    }
                }
                catch (Exception ex)
                {
                    await xyLog.AsxExLog(ex);
                }
                return null!;
            }

            public async Task<ReadOnlyCollection<IWebElement>> FindElements(By by)
            {
                try
                {
                    return _wait.Until(driver => driver.FindElements(by));
                }
                catch (Exception ex)
                {
                    await xyLog.AsxExLog(ex);
                    return null!;
                }
            }

            public async Task<bool> Click(By by)
            {
                IWebElement target = await FindElement(by);
                string text = target.Text;
                string success = $"Clicked on the element saying {text}....";
                bool isClicked = false;
                try
                {
                    target.Click();
                    await xyLog.AsxLog(success);
                    isClicked = true;
                }
                catch (Exception ex)
                {
                    await xyLog.AsxExLog(ex);
                }
                return isClicked;
            }

            public async Task EnterText(By by, string text)
            {
                IWebElement webElement = await FindElement(by);
                string success = $"Send {text} to the {webElement.TagName}";
                try
                {
                    webElement.Clear();
                    webElement.SendKeys(text);
                    await xyLog.AsxLog(success);
                }
                catch (Exception ex)
                {
                    await xyLog.AsxExLog(ex);
                }
            }

            public async Task TakeScreenshot(string path)
            {
                try
                {
                    Screenshot screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    screenshot.SaveAsFile(path);
                }
                catch (Exception ex)
                {
                    await xyLog.AsxExLog(ex);
                }
            }

            // Fix later
            public async Task SelectDropdownOption(By by, string target, int selector)
            {
                string invalidSelector = "Chose a valid selector";
                IWebElement webElement = await FindElement(by);
                try
                {
                    SelectElement selectElement = new(webElement);
                    Task selectTarget = selector switch
                    {

                        0 => Task.Run(() => { selectElement.SelectByText(target); }),
                        1 => Task.Run(() => { selectElement.SelectByValue(target); }),
                        2 => Task.Run(() => { selectElement.SelectByIndex(int.Parse(target)); }),
                        _ => xyLog.AsxLog(invalidSelector)
                    };
                    selectTarget.Start();
                }
                catch (Exception ex)
                {
                    await xyLog.AsxExLog(ex);
                }
            }


            public async Task CheckCheckBox(By by)
            {
                IWebElement checkBox = await FindElement(by);
                try
                {
                    if (!checkBox.Selected)
                    {
                        checkBox.Click();
                    }
                }
                catch (Exception ex)
                {
                    xyLog.ExLog(ex);
                }
            }

            public async Task UncheckCheckbox(By by)
            {
                IWebElement checkBox = await FindElement(by);
                try
                {
                    if (checkBox.Selected)
                    {
                        checkBox.Click();
                    }
                }
                catch (Exception ex)
                {
                    xyLog.ExLog(ex);
                }
            }



            public static async Task<T> ExecuteScript<T>(string script, params object[] args)
            {
                try
                {
                    IJavaScriptExecutor jsExec = (IJavaScriptExecutor)_driver;

                    if (jsExec.ExecuteAsyncScript(script) is T result)
                    {
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    await xyLog.AsxExLog(ex);
                }
                string fail = "Failed to execute the target script ";

                await xyLog.AsxLog(fail);
                return default!;
            }


            public static async Task ExecuteScript(string script, params object[] args)
            {
                try
                {
                    IJavaScriptExecutor jsExec = (IJavaScriptExecutor)_driver;

                    if (jsExec.ExecuteAsyncScript(script) is object result)
                    {
                        string success = $"Script was executed successfully and returned an instance of: {result.ToString()}";
                        await xyLog.AsxLog(success);
                    }
                    else
                    {
                        string fail = "Failed to execute the target script ";
                        await xyLog.AsxLog(fail);
                    }
                }
                catch (Exception ex)
                {
                    await xyLog.AsxExLog(ex);
                }
                return;
            }





            public void Dispose()
            {
                _driver.Quit();
                _driver.Dispose();

            }

        }




        /// <summary>
        /// Different kinds of accessing the select element
        /// </summary>
        public enum SelectBy
        {
            Text = 0,
            Value = 1,
            Index = 2
        }


    }

}

