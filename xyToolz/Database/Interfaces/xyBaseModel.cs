using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Database.Basix
{
    /// <summary>
    /// Providing basic properties and methods for models:
    /// 
    /// ID, Name, Description, Comment
    /// 
    /// ToString() => Name
    /// 
    /// 
    /// </summary>
    public abstract class xyBaseModel
    {
        /// <summary>
        /// Index
        /// </summary>
        public virtual uint ID { get; set; }

        /// <summary>
        /// Name the target
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Add usefull information for the target
        /// </summary>
        public virtual string Description { get; set; }
        
        /// <summary>
        /// Comment or note for the object.
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// Returns the name property
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Name}";

        // , [CallerMemberName] string? callerName = null

    }
    public interface xyBasicModel
    {
        
    }

}
