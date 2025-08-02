using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Logging.Helper
{
    public class xyMessageFactory
    {
        #region "Debug"

        #region "Parameter"
        public string InvalidParameter() => "Invalid data! Please check your input!";

        #endregion

        #region "CRUD"
        
        public string Created([MaybeNull]string? ID = null) => ID == null ? "Successfully created the target!":$"Successfully created the target with the ID {ID}!";
        public string NotCreated() => "Failed to create the target! Please check your input!";



        #endregion
        #region "EF-CORE CRUD"

        public string EntryList() => "This is a list filled with all relevant entrys from the context";
        public string EntryEmptyList() => "No entries found, this list is empty!";

        public string EntryCreated() => "Successfully created the context entry for the given data!";
        public string EntryAdded() => "Successfully added the entry to the underlying DB-Context implementation!";
        public string EntryCreatedAndAdded() => "Successfully created and added the entry for the given data to the DB-Context!";
        public string EntryNotCreated() => "Failed to create the target! Please check your input!";

        public string EntryNotFound(string ID) => $"Couldnt find the corresponding entrie for the ID {ID}";

        public string EntryUpdated([MaybeNull] string? name = null) => name == null ? $"Entry in DB-Context was updated successfully" : $"{name} was updated successfully updated in the context";
        public string EntryNotUpdated([MaybeNull] string? name = null) => name == null ? $"Error! Entry in DB-Context was not updated!" : $"{name} was not updated in the context!";

        public string EntryRemoved([MaybeNull]string? name = null) => name == null?$"Entry was removed from DB-Context successfully": $"{name} was removed from DB-Context successfully";
        public string EntryNotRemoved([MaybeNull] string? name = null) => name == null ? $"Failed to remove target from DB-Context!" : $"Failed to remove {name} from DB-Context!";
        #endregion

        #endregion




    }
}
