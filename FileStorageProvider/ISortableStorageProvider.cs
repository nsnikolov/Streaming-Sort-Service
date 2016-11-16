using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageProvider
{
    /// <summary>
    /// combined interface for sortable storage providers
    /// </summary>
    public interface ISortableStorageProvider : IStorageProvider, ISortableStorage
    {
        Stream RetreiveSortedTextData(Guid transactionKey);
    }
}
