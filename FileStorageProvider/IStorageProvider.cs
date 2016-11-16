using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageProvider
{
    /// <summary>
    /// This is main storage interface for providers which are compliant with Streaming Services implementation
    /// </summary>
    public interface IStorageProvider
    {

        bool AddTextData(string[] textData, Guid transactionKey);

        Stream RetreiveTextData(Guid transactionKey);

        bool DiscardTextData(Guid transactionKey);

        bool EmptyStorage();

    }
}
