
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using xyToolz.Helper.Logging;
using xyToolz.Logging.Helper.Formatters;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace xyToolz.Logging.Helper
{
    /// <summary>
    /// Providing generalized messages for logging via a non static class 
    /// 
    /// The methods return strings for easy digestion
    /// 
    /// Really nice, lol
    /// </summary>
    [SuppressMessage("Style", "IDE1006:Benennungsstile", Justification = "Mimimiimimimimimimimimimimimii")]
    public class xyMessageFactory 
    {
        /// <summary>
        /// Add usefull information
        /// </summary>
        public string[]? Description { get; set; }


        #region "Debug"

        #region "Misc"



        #endregion


        #region "ListigeCollectionen

        /// <summary>
        /// List is empty
        /// </summary>
        /// <param name="nameOfTheList"></param>
        /// <returns></returns>
        public string EmptyList(string? nameOfTheList = null) => nameOfTheList== null? "The target list is empty! Please check recent operations and logs!":$"{nameOfTheList} is EMPTY!";


        #endregion



        #region "Database"
        /// <summary>
        /// No connection string found in config
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ConnectionStringNotFound(string? name = null) => name is null? $"No connection string found, unable to connect to database!!!" :  $"No connection string found for {name}, unable to connect to the target database!!!";

        public string DatabaseConnectionFailed(string? dbName = null) => dbName is null? "Database connection failed!!!": $"Database connection to '{dbName}' failed!!!";

        public string DatabaseQueryError(string? query = null) => query is null? "Database query execution failed!!!": $"Database query execution failed for query: {query}";

        #endregion


        #region "ModelState"

        public string ModelUnvalidated(string? modelName = null) => modelName is null? $"Model validation has not been performed yet!": $"Model '{modelName}' validation has not been performed yet!";

            public string ModelValid(string? modelName = null) => modelName is null ? $"Model is valid.": $"Model '{modelName}' is valid.";

            public string ModelInvalid(string? modelName = null) => modelName is null ? $"Model validation failed! Invalid model state.": $"Model '{modelName}' validation failed! Invalid state.";

            public string ModelSkipped(string? modelName = null) =>modelName is null? $"Model validation has been skipped.": $"Model '{modelName}' validation has been skipped.";

        #endregion



        #region "Serialization"

        /// <summary>
        /// Successfully serialized the target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public string SerializationSuccess(object? target = null) => target is null ? $"The target has been serialized!" : $"{target.ToString()} has been serialized!";

        /// <summary>
        /// Failed to serialize the target
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string SerializationFail(string? name = null) => name is null ? $"An error occured while trying to serialize the target" : $"An error occured while trying to serialize {nameof(name)}";
        
        
        /// <summary>
        /// Successfully deserialized the target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public string DeserializationSuccess(object?  target = null) => target is null? $"{target} has been deserialized!" : $"{target.ToString()} has been deserialized!";

        /// <summary>
        /// Failed to deserialize the target
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string DeserializationFail(string? name = null) => name is null ? $"An error occured while trying to deserialize the target" : $"An error occured while trying to deserialize {nameof(name)}";

        #endregion



        #region "Parameter"

        /// <summary>
        /// Parameter is OK
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string ParameterValid(string? paramName = null) => paramName == null ? $"Data from parameter is valid! ": $"{paramName} is valid";
        /// <summary>
        /// Parameter is incorrect
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string ParameterInvalid(string? paramName = null) => paramName == null? "Invalid data! Please check your input!": $"{paramName} is INVALID! Please check your input!";

        /// <summary>
        /// Lists the given parameters below each other
        /// </summary>
        /// <param name="paramNames"></param>
        /// <returns></returns>
        public string ParametersInvalid( string[]? paramNames = null)
        {
            if ( paramNames == null)
            {
                return "Invalid parameter in the parameter checking function!";
            }
            else
            {
                StringBuilder sb_Params = new();
                sb_Params.Append("The following parameters are INVALID:\n");
                foreach (var param in paramNames) 
                {
                    sb_Params.AppendLine(param);
                }
                sb_Params.AppendLine("Please check your input!");
                return sb_Params.ToString();
            }
        }
            

        /// <summary>
        /// Parameter is null
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string ParameterNull(string? paramName = null) => paramName == null ? "Input data is NULL! Please check your input!" : $"{paramName} is NULL! Please check your input!";

        /// <summary>
        /// Lists the given parameters below each other
        /// </summary>
        /// <param name="paramNames"></param>
        /// <returns></returns>
        public string ParametersNull(string[]? paramNames = null)
        {
            if (paramNames == null)
            {
                return "Invalid parameter data!";
            }
            else
            {
                StringBuilder sb_Params = new();
                sb_Params.Append("The following parameters are NULL:\n");
                foreach (var param in paramNames)
                {
                    sb_Params.AppendLine(param);
                }
                sb_Params.AppendLine("Please check your input!");
                return sb_Params.ToString();
            }
        }


        /// <summary>
        /// Parameter is null or invalid
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string ParameterNullOrInvalid(string? paramName = null) => paramName == null ? 
            $"Input data is NULL or INVALID! Please check your input!" : 
            $"{paramName} is NULL or INVALID! Please check your input!";

        /// <summary>
        /// Lists the given parameters below each other
        /// </summary>
        /// <param name="paramNames"></param>
        /// <returns></returns>
        public string ParametersNullOrInvalid(string[]? paramNames = null)
        {
            if (paramNames == null)
            {
                return "Parameters are NULL or hold INVALID data!";
            }
            else
            {
                StringBuilder sb_Params = new();
                sb_Params.Append("The following parameters are NULL or INVALID:\n");
                foreach (var param in paramNames)
                {
                    sb_Params.AppendLine(param);
                }
                sb_Params.AppendLine("Please check your input!");
                return sb_Params.ToString();
            }
        }


        /// <summary>
        /// Invalid ID
        /// </summary>
        /// <returns></returns>
        public string InvalidID(string ID) => $"The provided ID ({ID}) is invalid, please correct input!";

        /// <summary>
        /// Username or password is wrong
        /// </summary>
        /// <returns></returns>
        public string WrongUserNameOrPassword() => "Entered username or password is wrong, please correct input!";
        /// <summary>
        /// Username is wrong
        /// </summary>
        /// <returns></returns>
        public string WrongUserName() => "Entered username is wrong, please input correct username!";
        /// <summary>
        /// Password is wrong
        /// </summary>
        /// <returns></returns>
        public string WrongPassword() => "Entered password is wrong, please input correct password!";

        #endregion
            



        #region "CRUD"

        /// <summary>
        /// Created target -> ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string Created([MaybeNull]int? ID = null) => ID == null ? "Successfully created the target!":$"Successfully created the target with the ID {ID}!";
        /// <summary>
        /// Created target -> name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Created([MaybeNull] string? name = null) => name == null ? "Successfully created the target!" : $"Successfully created {name}!";
        /// <summary>
        /// Failed to create
        /// </summary>
        /// <returns></returns>
        public string NotCreated() => "Failed to create the target! Please check your input!";
        
        /// <summary>
        /// Read data from DB success
        /// </summary>
        /// <returns></returns>
        public string Read() => "Data from db was read successfully and stored in object to be used";
        /// <summary>
        /// Failed to read the target data from DB
        /// </summary>
        /// <returns></returns>
        public string NotRead(int? ID = null) => ID == null? "Found no valid equivalent for the entered data!" : $"No corresponding entry/ valid data found for ID{ID}";


        /// <summary>
        /// Updated target -> ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string Updated([MaybeNull] int? ID = null) => ID == null ? "Successfully updated the target!" : $"Successfully updated the target with the ID {ID}!";
        /// <summary>
        /// Updated target -> name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Updated([MaybeNull] string? name = null) => name == null ? "Successfully updated the target!" : $"Successfully updated {name}!";
        /// <summary>
        /// Failed to update
        /// </summary>
        /// <returns></returns>
        public string Updated() => "Failed to update the target! Please check your input!";

        /// <summary>
        /// Deleted target from DB
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string Deleted([MaybeNull] int? ID = null) => ID  == null ? $"Target was deleted from the database": $"target with the ID {ID} was removed from the database";
        /// <summary>
        /// Failed to delete target
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string NotDeleted([MaybeNull] int? ID = null) => ID == null ? $"Error! Target was not deleted from the database" : $"Error! The target with the ID {ID} was NOT deleted from the database";

        #endregion



        #region "EF-CORE CRUD"

        /// <summary>
        /// List with context entries
        /// </summary>
        /// <returns></returns>
        public string EntryList() => "This list is filled with all relevant entrys from the context";
        /// <summary>
        /// No entries in list
        /// </summary>
        /// <returns></returns>
        public string EntryEmptyList() => "No relevant entries found in the context, this list is empty!";


        /// <summary>
        /// Created entry
        /// </summary>
        /// <returns></returns>
        public string EntryCreated() => "Successfully created the context entry for the given data!";
        /// <summary>
        /// No entry created
        /// </summary>
        /// <returns></returns>
        public string EntryNotCreated() => "Failed to create the target! Please check your input!";
       

        /// <summary>
        /// Entry added to context
        /// </summary>
        /// <returns></returns>
        public string EntryAdded(string? contextName = null) => contextName == null ?"Successfully added the entry to the underlying DB-Context implementation!": $"Successfully added the entry to the {contextName}!";
        /// <summary>
        /// Entry created and added to context
        /// </summary>
        /// <param name="contextName"></param>
        /// <returns></returns>
        public string EntryCreatedAndAdded(string? contextName = null) => contextName == null ? "Successfully created and added the entry to the DB-Context!": $"Successfully created and added the entry to the {contextName}!";


        /// <summary>
        /// No entry found
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string EntryNotFound(object ID) => $"Couldnt find the corresponding entrie for the ID {ID}";
                ///// <summary>
        ///// No entry found
        ///// </summary>
        ///// <param name="ID"></param>
        ///// <returns></returns>
        //public string EntryNotFound(string ID) => $"Couldnt find the corresponding entrie for the ID {ID}";
        ///// <summary>
        ///// No entry found
        ///// </summary>
        ///// <param name="ID"></param>
        ///// <returns></returns>
        //public string EntryNotFound(int ID) => $"Couldnt find the corresponding entrie for the ID {ID}";


        /// <summary>
        /// Entry was updated
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string EntryUpdated([MaybeNull] string? name = null) => name == null ? $"Entry in DB-Context was updated successfully" : $"{name} was updated successfully updated in the context";
        /// <summary>
        /// Entry couldnt be updated
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string EntryNotUpdated([MaybeNull] string? name = null) => name == null ? $"Error! Entry in DB-Context was not updated!" : $"{name} was not updated in the context!";
        

        /// <summary>
        /// Removed entry  from context
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string EntryRemoved([MaybeNull]string? name = null) => name == null?$"Entry was removed from DB-Context successfully": $"{name} was removed from DB-Context successfully";
        /// <summary>
        /// Unable to remove entry from context
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string EntryNotRemoved([MaybeNull] string? name = null) => name == null ? $"Failed to remove target from DB-Context!" : $"Failed to remove {name} from DB-Context!";



        /// <summary>
        /// Context saved [...] changes
        /// </summary>
        /// <param name="name"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public string ContextSaved(string name, int count) => ( name is null && string.IsNullOrEmpty(count+"") )? $"Target DB-Context was saved successfully!" : $"All {count} changes in {name} have been saved successfully!";
        /// <summary>
        /// Context didnt save
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ContextNotSaved([MaybeNull] string? name = null) => name == null ? $"Failed to save the changes in target DB-Context!" : $"Failed to save the changes in {name}!";

        #endregion


        #region "Key & Token Handling"
        /// <summary>
        /// Successfully managed to set the value of the target key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string KeySet(string? key = null) => key == null ? "Set target value successfully!" : $"Set target value successfully for the key {key}!";
        /// <summary>
        /// Failed to set the value of the target key to the target value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string KeyNotSet(string? key = null) => key == null ? "Failed to set target value!" : $"Failed to set target value for the key {key}!";

        /// <summary>
        /// Key found
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string KeyFound(string? key = null) => key == null ? "Target key found!" : $"{key} was found!";
        /// <summary>
        /// No key found
        /// </summary>
        /// <returns></returns>
        public string KeyNotFound(string? key = null) =>key == null?  "Target key was not found!": $"{key} not found!";

        /// <summary>
        /// Token generated 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string TokenGenerated(string? token = null) =>  token ==null? $"Token was generated successfully" : $"Token generated:{token}";
        /// <summary>
        /// Token generation failed
        /// </summary>
        /// <returns></returns>
        public string TokenNotGenerated() => "Critical Failure in the token generation process!";

        public string TokenExpired(string? tokenId = null) => tokenId is null? "Authentication token has expired!!!" : $"Authentication token '{tokenId}' has expired!!!";

        #endregion


        #region "Network"
        public string NetworkUnavailable(string? host = null) =>  host is null? "Target network is unavailable!!!": $"Network unavailable for host '{host}'!!!";

        public string TimeoutOccurred(string? operation = null) =>operation is null? "A network timeout occurred!!!": $"A timeout occurred while performing '{operation}'!!!";

        public string HostUnreachable(string? host = null) =>host is null? "Host unreachable!!!" : $"Host '{host}' is unreachable!!!";

        #endregion


        #region Streams


        #region "FileStream"
        public string FileStreamError(string? file = null) => file is null ? "An Error occured while reading a file into stream" :$"An Error occured while reading '{file}' into a stream!!!";

        public string FileStreamSuccess(string? file = null) => file is null ? "Successfully read a file into the stream" : $"Successfully loaded {file} into the stream." ;
        #endregion
        #endregion

        #region "Paths" 
        public string PathNotFound(string? path = null) => path is null ? "The given path is null or empty." : $"The given path '{path}' is null or empty.";


        #endregion

        #region "File Operations"
        public string FileNotFound(string? file = null) =>file is null? "File not found!!!": $"File '{file}' not found!!!";

        public string FileAccessDenied(string? file = null) =>file is null? "File access denied!!!": $"Access denied for file '{file}'!!!";

        public string FileReadError(string? file = null) =>file is null? "File read error!!!": $"Error while reading file '{file}'!!!";

        public string FileContentError(string? file = null) => file is null ? "File content is empty or unreadable." : $"File content from '{file}' is unreadable or empty!!!";
     
        #endregion


        #region "User Messages"
        public string UserNotFound(string? user = null) =>user is null? "User not found!!!": $"User '{user}' not found!!!";

        public string UserAlreadyExists(string? user = null) =>user is null? "User already exists!!!": $"User '{user}' already exists!!!";

        public string UserLockedOut(string? user = null) =>user is null? "User account is locked out!!!": $"User '{user}' is locked out!!!";
        #endregion


        #region "Login"

        /// <summary>
        /// Login-data is valid
        /// </summary>
        /// <returns></returns>
        public string LoginSuccess(string? username)=> username is null? "The entered userdata seems valid and correct, you may proceed" : $"Login for {username} successfull";
        
        /// <summary>
        /// User failed to provide valid data for login
        /// </summary>
        /// <returns></returns>
        public string LoginFail(string? email) => email is null?"The userdata is invalid and/ or incorrect, please check the entered dataset" : $"Login failed for user: {email}.";
        
        public string LoginStart(string url) => $"Attempting login at: {url}.";

        public string LoginTimeout(int seconds) => $"Login process timed out after {seconds} seconds.";

        public string LoginUnexpected(Exception ex) => $"Unexpected error during login: {xyLogFormatter.FormatExceptionDetails(ex,LogLevel.Warning)}";

        #endregion


        #region "Security Messages"
        public string EncryptionFailed(string? target = null) => target is null? "Encryption failed!!!": $"Encryption failed for '{target}'!!!";

        public string DecryptionFailed(string? target = null) =>target is null? "Decryption failed!!!": $"Decryption failed for '{target}'!!!";

        public string InvalidCertificate(string? cert = null) =>cert is null? "Invalid certificate detected!!!": $"Invalid certificate '{cert}' detected!!!";
        #endregion


        #region "System"


        #region "Window & Tab Handling"

        public string WindowSwitch(string handle) => $"Switched to window with handle: {handle}";

        public string WindowClosed(string handle) => $"Window closed: {handle}";

        public string WindowOpened(string name) => $"New window opened: {name}";

        #endregion

        #region "Keyboard & Mouse Events"

        public string KeyPressed(string key) =>
            $"Pressed key: '{key}'";

        public string KeyCombinationPressed(string combo) =>
            $"Pressed key combination: '{combo}'";

        public string MouseMovedTo(string description) =>
            $"Mouse moved to: {description}";

        public string MouseClicked(string description) =>
            $"Mouse clicked on: {description}";

        public string MouseDoubleClicked(string description) =>
            $"Mouse double-clicked on: {description}";

        public string MouseRightClicked(string description) =>
            $"Mouse right-clicked on: {description}";

        public string MouseActionFailed(string action, string reason) =>
            $"Mouse action '{action}' failed: {reason}";

        #endregion

        #region "System Messages"
        public string OperationFailed(string? operation = null) =>operation is null? "Operation failed!!!": $"Operation '{operation}' failed!!!";

        public string ConfigurationError(string? config = null) =>config is null? "Configuration error detected!!!": $"Configuration error detected for '{config}'!!!";

        public string UnknownError(string? details = null) =>details is null? "An unknown error occurred!!!": $"An unknown error occurred: {details}";

        #endregion
        

        #endregion

        #region "Browser automatization"

        #region "Navigation"
        public string NavigationStart(string url) => $"Navigating to URL: {url}";

        public string NavigationTimeout(string url, int seconds) => $"Timeout while loading URL: {url} (after {seconds} seconds)";

        public string NavigationUnexpected(string url) => $"Unexpected error while navigating to URL: {url}";

        public string NavigationSuccess(string url) => $"Navigation to '{url}' completed.";
        #endregion

        #region "Elements"

        #endregion

        #region "Clicking"
        public string ClickSuccess(string textOrTag) => $"Clicked on the element: {textOrTag}.";

        public string ClickTimeout(By by, int seconds) => $"Timeout: Element '{by}' was not clickable within {seconds} seconds...";

        public string ClickStale(By by) => $"Stale reference: Element '{by}' is no longer attached to the DOM.";

        public string ClickUnexpected(By by) => $"Unexpected error while trying to click on element '{by}'!";

        public string ClickFail(By by) => $"Failed to click element '{by}'.";
        #endregion

        #region "Enter Stuff"
        public string EnterPassword(By by, string tag, string type) => $"Entering password into element '{by}' ({tag}, type={type}).";

        public string EnterText(By by, string input, string tag, string type) => $"Entering text '{input}' into element '{by}' ({tag}, type={type}).";

        public string EnterSkipButton(By by, string type) => $"Skipped: Element '{by}' is a button or submit input (type={type}).";

        public string EnterTimeout(By by, int seconds) => $"Timeout: Element '{by}' was not visible within {seconds} seconds.";

        public string EnterStale(By by) => $"Stale reference: Element '{by}' is no longer attached to the DOM.";

        public string EnterUnexpected(By by) => $"Unexpected error while trying to enter text into element '{by}'.";

        public string EnterFail(By by) => $"Failed to enter text into element '{by}'.";
        #endregion

        #region "Screenshots"

        public string ScreenshotSuccess(string filePath) => $"Screenshot successfully saved to: {filePath}.";

        public string ScreenshotFail(Exception ex) => $"Failed to take screenshot: {ex.Message}";

        #endregion

        #region "Downloads"

        public string DownloadStarted(string url) => $"Started download from: {url}.";

        public string DownloadSuccess(string filePath) => $"Download completed and saved to: {filePath}.";

        public string DownloadTimeout(string url, int seconds) => $"Timeout while downloading from '{url}' after {seconds} seconds.";

        public string DownloadFail(string url) => $"Failed to download from: {url}.";

        #endregion

        #region "Uploads"

        public string UploadStart(string filePath) => $"Starting upload of file: {filePath}.";

        public string UploadSuccess(string filePath) => $"File uploaded successfully: {filePath}.";

        public string UploadFail(string filePath) => $"Failed to upload file: {filePath}.";

        public string UploadElementNotFound(By by) => $"Upload element not found: {by}.";

        #endregion

        #region "Submitting FORMs"

        public string FormSubmitStart(By by) => $"Submitting form using element: {by}.";

        public string FormSubmitSuccess(By by) => $"Form submitted successfully using element: {by}.";

        public string FormSubmitFail(By by) => $"Form submission failed using element: {by}.";

        public string FormSubmitTimeout(By by, int seconds) => $"Timeout during form submission with element '{by}' after {seconds} seconds.";

        #endregion

        #region "Element lookup & validation"
        public string ElementVisible(By by) => $"Element '{by}' is visible.";

        public string ElementInvisible(By by) => $"Element '{by}' is not visible.";

        public string ElementClickable(By by) => $"Element '{by}' is clickable.";

        public string ElementNotClickable(By by) => $"Element '{by}' is not clickable.";

        public string ElementNotFound(By by) => $"Element '{by}' not found.";

        public string ElementFound(By by) => $"Element found: {by}.";

        public string ElementMissing(By by) => $"Element not found: {by}.";

        public string ElementValidationPass(By by) => $"Element '{by}' passed validation.";

        public string ElementValidationFail(By by) => $"Element '{by}' failed validation.";

        public string ElementAttributeMismatch(By by, string attr, string expected, string actual) => $"Validation failed: Attribute '{attr}' of element '{by}' expected '{expected}', but found '{actual}'.";

        #endregion

        #region "Cookie management"

        public string CookieAdded(string name) => $"Cookie '{name}' was added successfully.";

        public string CookieRetrieved(string name, string value) => $"Cookie '{name}' retrieved with value: {value}.";

        public string CookieNotFound(string name) => $"Cookie '{name}' was not found.";

        public string CookieDeleted(string name) => $"Cookie '{name}' was deleted.";

        public string CookieOperationFailed(string name, string action) => $"Cookie '{name}' could not be {action}.";

        #endregion

        #region "Window handling"

        public string WindowSwitched(string title) => $"Switched to window with title: {title}.";

        public string WindowNotFound(string title) => $"Window with title '{title}' not found.";

        public string WindowCloseSuccess(string title) => $"Closed window with title: {title}.";

        public string WindowCloseFail(string title) => $"Failed to close window with title: {title}.";

        #endregion

        #region "Switching tabs"

        public string TabSwitched(int index) => $"Switched to browser tab at index: {index}.";

        public string TabSwitchFailed(int index) => $"Failed to switch to tab at index: {index}.";

        public string TabClosed(int index) => $"Closed browser tab at index: {index}.";

        #endregion

        #region "JavaScript stuff"

        public string JsExecuted(string description) => $"Executed JavaScript: {description}.";

        public string JsExecutionFailed(string description) => $"Failed to execute JavaScript: {description}.";

        public string JsReturnedNull(string description) => $"JavaScript returned null or undefined: {description}.";

        public string JsReturnedValue(string description, string value) => $"JavaScript result for '{description}': {value}.";

        #endregion

        #region "Alerts aka ALARMs"

        public string AlertPresent(string text) => $"Alert with message: {text}.";

        public string AlertNotPresent() => $"No alert present.";

        public string AlertAccepted() => $"Alert was accepted.";

        public string AlertDismissed() => $"Alert was dismissed.";

        public string AlertHandlingFailed(string reason) => $"Failed to handle alert: {reason}.";

        #endregion

        #region "Drag and Drop"

        public string DragAndDropSuccess(By source, By target) => $"Dragged element '{source}' and dropped onto '{target}'.";

        public string DragAndDropFail(By source, By target) => $"Failed to drag element '{source}' onto '{target}'.";

        #endregion

        #region "Scrolling"

        public string ScrolledToElement(By by) => $"Scrolled to element '{by}'.";

        public string ScrollToElementFailed(By by) => $"Failed to scroll to element '{by}'.";

        public string ScrolledByOffset(int x, int y) => $"Scrolled page by offset (x={x}, y={y}).";

        #endregion

        #region "Wait conditions"

        public string WaitUntilVisible(By by, int seconds) => $"Waiting until element '{by}' is visible (timeout: {seconds} seconds).";

        public string WaitUntilClickable(By by, int seconds) => $"Waiting until element '{by}' is clickable (timeout: {seconds} seconds).";

        public string WaitConditionFail(By by, string condition) => $"Wait failed: Element '{by}' did not meet condition: {condition}.";

        #endregion

        #region "Modal Dialogs"

        public string ModalDetected(By by) => $"Modal dialog detected: '{by}'";

        public string ModalHandled(By by) => $"Modal dialog handled successfully: '{by}'";

        public string ModalNotFound(By by) => $"Modal dialog not found: '{by}'";

        public string ModalHandlingFailed(By by) => $"Failed to handle modal dialog: '{by}'";

        #endregion

        #region "Frame Switching"

        public string FrameSwitch(By by) => $"Switched to frame: '{by}'";

        public string SwitchedToDefaultContent() => $"Switched back to default content from frame.";

        public string FrameNotFound(By by) => $"Frame not found: '{by}'";

        public string FrameSwitchFailed(By by) => $"Failed to switch to frame: '{by}'";

        #endregion

        #region "Result and status messages"

        public string ActionSuccess(string description) => $"Success: {description}";

        public string ActionFailed(string description) => $"Failure: {description}";

        public string HttpStatusCode(int statusCode) => $"HTTP status code received: {statusCode}";

        public string OperationCompleted(string name) => $"Operation completed: {name}";

        #endregion







        #endregion

        #endregion
    }




}

