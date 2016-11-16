using StorageProvider;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace StreamingSortWCFService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class StreamingSortService : IStreamingSortService
    {
        static ConcurrentBag<Guid> liveSessionGuids = new ConcurrentBag<Guid>();
        ISortableStorageProvider storageProvider = new FileStorageProvider();//StorageProviderManager.GetDefaultSortableProvider<ISortableStorageProvider>();

        public StreamingSortService()
        {
            //storageProvider.EmptyStorage();
        }

        public Guid BeginStream()
        {
            Guid sGuid = Guid.NewGuid();
            liveSessionGuids.Add(sGuid);
            Console.WriteLine("BeginStream r "+ sGuid);
            return sGuid;
        }

        public void EndStream(Guid streamGuid)
        {
            Console.WriteLine("EndStream " + streamGuid);
            if (streamGuid == null)
            {
                throw new ArgumentNullException("streamGuid");
            }
            else if (!liveSessionGuids.Contains(streamGuid))
            {
                throw new NonExistingSessionIdExeption(streamGuid);
            }
            storageProvider.DiscardTextData(streamGuid);
            liveSessionGuids.TryTake(out streamGuid);
        }

        public Stream GetSortedStream(Guid streamGuid)
        {
            Console.WriteLine("GetSortedStream " + streamGuid);
            if (streamGuid == null)
            {
                throw new ArgumentNullException("streamGuid");
            }
            else if (!liveSessionGuids.Contains(streamGuid))
            {
                throw new NonExistingSessionIdExeption(streamGuid);
            }
            return storageProvider.RetreiveSortedTextData(streamGuid);
        }

        public void PutStreamData(Guid streamGuid, string[] text)
        {
            Console.WriteLine("PutStreamData " + streamGuid + " " + text);
            foreach(string s in text)
            {
                Console.WriteLine(s);
            }
            if (streamGuid == null)
            {
                throw new ArgumentNullException("composite");
            }
            else if (!liveSessionGuids.Contains(streamGuid))
            {
                throw new NonExistingSessionIdExeption(streamGuid);
            }
            storageProvider.AddTextData(text, streamGuid);
        }


    }
}