using System;
using System.Collections.Generic;
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
        
        public string Created(string? ID = null) => ID == null ? "Successfully created the target!":$"Successfully created the target with the ID {ID}!";
        public string NotCreated() => "Failed to create the target! Please check your input!";



        #endregion
        #region "EF-CORE CRUD"

        public string EntryCreated() => "Successfully created the entry for the given data!";
        public string EntryAdded() => "Successfully added the entry to the DB-Context!";
        public string EntryCreatedAndAdded() => "Successfully created and added the entry for the given data to the DB-Context!";
        public string EntryNotCreated() => "Failed to create the target! Please check your input!";



        #endregion

        #endregion




    }
}
