using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Logging.Helper
{
    /// <summary>
    /// Providing generalized messages for logging via non static class & functions this time
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

        #endregion
        #region "Parameter"
        /// <summary>
        /// Parameter is incorrect
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string InvalidParameter(string? paramName = null) => paramName == null? "Invalid data! Please check your input!": $"{paramName} is INVALID! Please check your input!";

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



        #endregion
        #region "EF-CORE CRUD"
        /// <summary>
        /// List with context entries
        /// </summary>
        /// <returns></returns>
        public string EntryList() => "This is a list filled with all relevant entrys from the context";
        /// <summary>
        /// No entries in list
        /// </summary>
        /// <returns></returns>
        public string EntryEmptyList() => "No entries found, this list is empty!";
        /// <summary>
        /// Created entry
        /// </summary>
        /// <returns></returns>
        public string EntryCreated() => "Successfully created the context entry for the given data!";
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
        /// No entry created
        /// </summary>
        /// <returns></returns>
        public string EntryNotCreated() => "Failed to create the target! Please check your input!";
        /// <summary>
        /// No entry found
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string EntryNotFound(string ID) => $"Couldnt find the corresponding entrie for the ID {ID}";
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
        #endregion

        #endregion




    }
}
