using System.Diagnostics.CodeAnalysis;

namespace xyToolz.Logging.Helper
{
    /// <summary>
    /// Providing generalized messages for logging via a non static class 
    /// 
    /// The methods return strings for easy digestion
    /// 
    /// </summary>
    public class xyMessageFactory
    {
        #region "Debug"
        #region "Misc"

        /// <summary>
        /// List is empty
        /// </summary>
        /// <param name="nameOfTheList"></param>
        /// <returns></returns>
        public string EmptyList(string? nameOfTheList = null) => nameOfTheList== null? "The target list is empty! Please check recent operations and logs!":$"{nameOfTheList} is EMPTY!";
        
        /// <summary>
        /// No connection string found in config
        /// </summary>
        /// <param name="nameOfTheKey"></param>
        /// <returns></returns>
        public string ConnectionStringNotFound(string? nameOfTheKey = null) => $"No connection string found for {nameOfTheKey}, unable to connect to the target database!!!";
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
        /// Parameter is null
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string ParameterIsNull(string? paramName = null) => paramName == null ? "Input data is NULL! Please check your input!" : $"{paramName} is NULL! Please check your input!";
        
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
        public string NotRead() => "Found no valid equivalent for the entered data!";

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
        /// Context saved the changes
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ContextSaved([MaybeNull] string? name = null) => name == null ? $"Target DB-Context was saved successfully!" : $"{name} was saved successfully!";
        /// <summary>
        /// Context didnt save
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ContextNotSaved([MaybeNull] string? name = null) => name == null ? $"Failed to save the changes intarget DB-Context!" : $"Failed to save the changes in {name}!";
        #endregion


        #region "Key & Token Handling"

        /// <summary>
        /// No key found
        /// </summary>
        /// <returns></returns>
        public string KeyNotFound(string? key = null) =>key == null?  "Target key was not found!": $"{key} not found!";

        /// <summary>
        /// Token generation failed
        /// </summary>
        /// <returns></returns>
        public string TokenNotGenerated() => "Critical Failure in the token generation process!"; 
        #endregion



        #endregion




    }
}
