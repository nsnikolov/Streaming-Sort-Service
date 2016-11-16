using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace StreamingSortWCFService
{

    [ServiceContract]
    public interface IStreamingSortService
    {

        [OperationContract]
        Guid BeginStream();

        [OperationContract]
        void PutStreamData(Guid streamGuid, string[] text);

        [OperationContract]
        Stream GetSortedStream(Guid streamGuid);

        [OperationContract]
        void EndStream(Guid streamGuid);

    }
   
}
