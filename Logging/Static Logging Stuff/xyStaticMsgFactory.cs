using OpenQA.Selenium;

namespace xyToolz.Logging.Helper
{
    /// <summary>
    ///  Providing reusable samples for logging
    /// </summary>
    public static class xyStaticMsgFactory 
    {



        #region "Browser automatization"

        #region "Navigation"
        public static string NavigationStart(string url) =>                              $"Navigating to URL: {url}";

        public static string NavigationTimeout(string url, int seconds) =>      $"Timeout while loading URL: {url} (after {seconds} seconds)";

        public static string NavigationUnexpected(string url) =>                    $"Unexpected error while navigating to URL: {url}";

        public static string NavigationSuccess(string url) =>                          $"Navigation to '{url}' completed.";
        #endregion

        #region "Elements"
     
        #endregion

        #region "Clicking"
        public static string ClickSuccess(string textOrTag) =>     $"Clicked on the element: {textOrTag}.";

        public static string ClickTimeout(By by, int seconds) =>    $"Timeout: Element '{by}' was not clickable within {seconds} seconds...";

        public static string ClickStale(By by) =>                            $"Stale reference: Element '{by}' is no longer attached to the DOM.";

        public static string ClickUnexpected(By by) =>                  $"Unexpected error while trying to click on element '{by}'!";

        public static string ClickFail(By by) =>                               $"Failed to click element '{by}'.";
        #endregion

        #region "Enter Stuff"
        public static string EnterPassword(By by, string tag, string type) =>                  $"Entering password into element '{by}' ({tag}, type={type}).";
                                                                                                                                   
        public static string EnterText(By by, string input, string tag, string type) =>     $"Entering text '{input}' into element '{by}' ({tag}, type={type}).";
                                                                                                                                    
        public static string EnterSkipButton(By by, string type) =>                                 $"Skipped: Element '{by}' is a button or submit input (type={type}).";

        public static string EnterTimeout(By by, int seconds) =>                                     $"Timeout: Element '{by}' was not visible within {seconds} seconds.";

        public static string EnterStale(By by) =>                                                             $"Stale reference: Element '{by}' is no longer attached to the DOM.";

        public static string EnterUnexpected(By by) =>                                                   $"Unexpected error while trying to enter text into element '{by}'.";

        public static string EnterFail(By by) =>                                                                $"Failed to enter text into element '{by}'.";
        #endregion

        #region "Screenshots"

        public static string ScreenshotSuccess(string filePath) =>  $"Screenshot successfully saved to: {filePath}.";

        public static string ScreenshotFail(Exception ex) =>            $"Failed to take screenshot: {ex.Message}";

        #endregion

        #region "Downloads"

        public static string DownloadStarted(string url) =>                         $"Started download from: {url}.";

        public static string DownloadSuccess(string filePath) =>                 $"Download completed and saved to: {filePath}.";

        public static string DownloadTimeout(string url, int seconds) =>     $"Timeout while downloading from '{url}' after {seconds} seconds.";

        public static string DownloadFail(string url) =>                                $"Failed to download from: {url}.";

        #endregion

        #region "Login"

        public static string LoginStart(string url) =>                 $"Attempting login at: {url}.";

        public static string LoginSuccess(string email) =>          $"Login successful for user: {email}.";

        public static string LoginFail(string email) =>                 $"Login failed for user: {email}.";
                                                                                      
        public static string LoginTimeout(int seconds) =>           $"Login process timed out after {seconds} seconds.";

        public static string LoginUnexpected(Exception ex) =>  $"Unexpected error during login: {ex.Message}";

        #endregion

        #region "Uploads"

        public static string UploadStart(string filePath) =>        $"Starting upload of file: {filePath}.";

        public static string UploadSuccess(string filePath) =>    $"File uploaded successfully: {filePath}.";

        public static string UploadFail(string filePath) =>           $"Failed to upload file: {filePath}.";

        public static string UploadElementNotFound(By by) =>   $"Upload element not found: {by}.";

        #endregion

        #region "Submitting FORMs"

        public static string FormSubmitStart(By by) =>                          $"Submitting form using element: {by}.";
                                                                                                           
        public static string FormSubmitSuccess(By by) =>                      $"Form submitted successfully using element: {by}.";
                                                                                                           
        public static string FormSubmitFail(By by) =>                             $"Form submission failed using element: {by}.";
                                                                                                           
        public static string FormSubmitTimeout(By by, int seconds) =>  $"Timeout during form submission with element '{by}' after {seconds} seconds.";

        #endregion

        #region "Element lookup & validation"
        public static string ElementVisible(By by) =>            $"Element '{by}' is visible.";

        public static string ElementInvisible(By by) =>         $"Element '{by}' is not visible.";

        public static string ElementClickable(By by) =>         $"Element '{by}' is clickable.";

        public static string ElementNotClickable(By by) =>   $"Element '{by}' is not clickable.";

        public static string ElementNotFound(By by) =>        $"Element '{by}' not found.";
                                                                                         
        public static string ElementFound(By by) =>              $"Element found: {by}.";
                                                                                         
        public static string ElementMissing(By by) =>           $"Element not found: {by}.";
                                                                                         
        public static string ElementValidationPass(By by) => $"Element '{by}' passed validation.";
                                                                                         
        public static string ElementValidationFail(By by) =>  $"Element '{by}' failed validation.";

        public static string ElementAttributeMismatch(By by, string attr, string expected, string actual) =>    $"Validation failed: Attribute '{attr}' of element '{by}' expected '{expected}', but found '{actual}'.";

        #endregion

        #region "Cookie management"

        public static string CookieAdded(string name) =>                                        $"Cookie '{name}' was added successfully.";

        public static string CookieRetrieved(string name, string value) =>              $"Cookie '{name}' retrieved with value: {value}.";

        public static string CookieNotFound(string name) =>                                  $"Cookie '{name}' was not found.";

        public static string CookieDeleted(string name) =>                                     $"Cookie '{name}' was deleted.";

        public static string CookieOperationFailed(string name, string action) =>   $"Cookie '{name}' could not be {action}.";

        #endregion

        #region "Window handling"

        public static string WindowSwitched(string title) =>        $"Switched to window with title: {title}.";

        public static string WindowNotFound(string title) =>       $"Window with title '{title}' not found.";

        public static string WindowCloseSuccess(string title) =>  $"Closed window with title: {title}.";

        public static string WindowCloseFail(string title) =>         $"Failed to close window with title: {title}.";

        #endregion

        #region "Switching tabs"

        public static string TabSwitched(int index) =>          $"Switched to browser tab at index: {index}.";

        public static string TabSwitchFailed(int index) =>    $"Failed to switch to tab at index: {index}.";

        public static string TabClosed(int index) =>              $"Closed browser tab at index: {index}.";

        #endregion

        #region "JavaScript stuff"

        public static string JsExecuted(string description) =>                                  $"Executed JavaScript: {description}.";

        public static string JsExecutionFailed(string description) =>                        $"Failed to execute JavaScript: {description}.";
                                                                                                                             
        public static string JsReturnedNull(string description) =>                            $"JavaScript returned null or undefined: {description}.";
                                                                                                                             
        public static string JsReturnedValue(string description, string value) =>      $"JavaScript result for '{description}': {value}.";

        #endregion

        #region "Alerts aka ALARMs"

        public static string AlertPresent(string text) =>                $"Alert with message: {text}.";

        public static string AlertNotPresent() =>                           $"No alert present.";
                                                                                                  
        public static string AlertAccepted() =>                              $"Alert was accepted.";

        public static string AlertDismissed() =>                             $"Alert was dismissed.";

        public static string AlertHandlingFailed(string reason) => $"Failed to handle alert: {reason}.";

        #endregion

        #region "Drag and Drop"

        public static string DragAndDropSuccess(By source, By target) =>    $"Dragged element '{source}' and dropped onto '{target}'.";

        public static string DragAndDropFail(By source, By target) =>           $"Failed to drag element '{source}' onto '{target}'.";

        #endregion

        #region "Scrolling"

        public static string ScrolledToElement(By by) =>            $"Scrolled to element '{by}'.";
                                                                                               
        public static string ScrollToElementFailed(By by) =>      $"Failed to scroll to element '{by}'.";
                                                                                               
        public static string ScrolledByOffset(int x, int y) =>      $"Scrolled page by offset (x={x}, y={y}).";

        #endregion

        #region "Wait conditions"

        public static string WaitUntilVisible(By by, int seconds) =>                 $"Waiting until element '{by}' is visible (timeout: {seconds} seconds).";
                                                                                                                    
        public static string WaitUntilClickable(By by, int seconds) =>              $"Waiting until element '{by}' is clickable (timeout: {seconds} seconds).";

        public static string WaitConditionFail(By by, string condition) =>     $"Wait failed: Element '{by}' did not meet condition: {condition}.";

        #endregion

        #region "Modal Dialogs"

        public static string ModalDetected(By by) =>            $"Modal dialog detected: '{by}'";

        public static string ModalHandled(By by) =>              $"Modal dialog handled successfully: '{by}'";
                                                                                           
        public static string ModalNotFound(By by) =>           $"Modal dialog not found: '{by}'";

        public static string ModalHandlingFailed(By by) =>    $"Failed to handle modal dialog: '{by}'";

        #endregion

        #region "Frame Switching"

        public static string FrameSwitch(By by) =>                  $"Switched to frame: '{by}'";

        public static string SwitchedToDefaultContent() =>    $"Switched back to default content from frame.";

        public static string FrameNotFound(By by) =>              $"Frame not found: '{by}'";

        public static string FrameSwitchFailed(By by) =>         $"Failed to switch to frame: '{by}'";

        #endregion

        #region "Result and status messages"

        public static string ActionSuccess(string description) =>   $"Success: {description}";

        public static string ActionFailed(string description) =>      $"Failure: {description}";

        public static string HttpStatusCode(int statusCode) =>     $"HTTP status code received: {statusCode}";

        public static string OperationCompleted(string name) =>   $"Operation completed: {name}";

        #endregion







        #endregion

        #region "System"

        #region "Window & Tab Handling"

        public static string WindowSwitch(string handle) =>     $"Switched to window with handle: {handle}";

        public static string WindowClosed(string handle) =>              $"Window closed: {handle}";

        public static string WindowOpened(string name) =>               $"New window opened: {name}";

        #endregion

        #region "Keyboard & Mouse Events"

        public static string KeyPressed(string key) =>
            $"Pressed key: '{key}'";

        public static string KeyCombinationPressed(string combo) =>
            $"Pressed key combination: '{combo}'";

        public static string MouseMovedTo(string description) =>
            $"Mouse moved to: {description}";

        public static string MouseClicked(string description) =>
            $"Mouse clicked on: {description}";

        public static string MouseDoubleClicked(string description) =>
            $"Mouse double-clicked on: {description}";

        public static string MouseRightClicked(string description) =>
            $"Mouse right-clicked on: {description}";

        public static string MouseActionFailed(string action, string reason) =>
            $"Mouse action '{action}' failed: {reason}";

        #endregion





        #endregion
    }
}
