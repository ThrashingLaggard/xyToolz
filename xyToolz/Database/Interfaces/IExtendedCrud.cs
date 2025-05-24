using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Database.Interfaces
{
    public interface IExtendedCrud<T> : IBaseCrud<T>, IBaseCrudExtensions<T>, IEfCoreCrudExtensions<T> where T: class
    {
    }
}
