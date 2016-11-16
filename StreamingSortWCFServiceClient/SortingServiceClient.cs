using StreamingSortWCFService;
using System;
using System.IO;
using System.ServiceModel;

namespace StreamingSortWCFServiceClient
{
    public class SortingServiceClient : ClientBase<IStreamingSortService>, IStreamingSortService 
    {
        public Guid BeginStream()
        {
            return Channel.BeginStream();
        }

        public void PutStreamData(Guid streamGuid, string[] text)
        {
            Channel.PutStreamData(streamGuid, text);
        }

        public Stream GetSortedStream(Guid streamGuid)
        {
            return Channel.GetSortedStream(streamGuid);
        }

        public void EndStream(Guid streamGuid)
        {
            Channel.GetSortedStream(streamGuid);
        }
    }
}
