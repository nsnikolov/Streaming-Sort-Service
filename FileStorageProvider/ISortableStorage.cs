using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageProvider
{
    /// <summary>
    /// Should be implemented by providers which has sorting capabilities
    /// </summary>
    public interface ISortableStorage
    {
        bool SortDirectionAccending{ get; set; }

        void Sort(Guid transactionKey);
    }
}
