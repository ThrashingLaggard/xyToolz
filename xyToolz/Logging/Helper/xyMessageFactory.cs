using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace xyToolz.Logging.Helper
{
    /// <summary>
    /// Providing generalized messages for logging via a non static class 
    /// 
    /// The methods return strings for easy digestion
    /// 
    /// Really nice, lol
    /// </summary>
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
        public string LoginFail() => "The userdata is invalid and/ or incorrect, please check the entered dataset";
        
        #endregion
        

        #region "Security Messages"
        public string EncryptionFailed(string? target = null) => target is null? "Encryption failed!!!": $"Encryption failed for '{target}'!!!";

        public string DecryptionFailed(string? target = null) =>target is null? "Decryption failed!!!": $"Decryption failed for '{target}'!!!";

        public string InvalidCertificate(string? cert = null) =>cert is null? "Invalid certificate detected!!!": $"Invalid certificate '{cert}' detected!!!";
        #endregion


        #region "System Messages"
        public string OperationFailed(string? operation = null) =>operation is null? "Operation failed!!!": $"Operation '{operation}' failed!!!";

        public string ConfigurationError(string? config = null) =>config is null? "Configuration error detected!!!": $"Configuration error detected for '{config}'!!!";

        public string UnknownError(string? details = null) =>details is null? "An unknown error occurred!!!": $"An unknown error occurred: {details}";
        #endregion


    #endregion
    }




}

