using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Helper.Logging
{
    /// <summary>
    ///  Providing reusable samples for logging
    /// </summary>
    public static class xyLogMsgFactory
    {



        #region "Browser automatisation"
        public static string ClickSuccess(string textOrTag) => $"Clicked on the element: {textOrTag}";

        public static string ClickTimeout(By by, int seconds) =>
            $"Timeout: Element '{by}' was not clickable within {seconds} seconds.";

        public static string ClickStale(By by) =>
            $"Stale reference: Element '{by}' is no longer attached to the DOM.";

        public static string ClickUnexpected(By by) =>
            $"Unexpected error while trying to click on element '{by}'.";

        public static string ClickFail(By by) =>
            $"Failed to click element '{by}'.";


        public static string EnterPassword(By by, string tag, string type) =>
            $"Entering password into element '{by}' ({tag}, type={type}).";

        public static string EnterText(By by, string input, string tag, string type) =>
            $"Entering text '{input}' into element '{by}' ({tag}, type={type}).";

        public static string EnterSkipButton(By by, string type) =>
            $"Skipped: Element '{by}' is a button or submit input (type={type}).";

        public static string EnterTimeout(By by, int seconds) =>
            $"Timeout: Element '{by}' was not visible within {seconds} seconds.";

        public static string EnterStale(By by) =>
            $"Stale reference: Element '{by}' is no longer attached to the DOM.";

        public static string EnterUnexpected(By by) =>
            $"Unexpected error while trying to enter text into element '{by}'.";

        public static string EnterFail(By by) =>
            $"Failed to enter text into element '{by}'.";

        #endregion
    }
}
