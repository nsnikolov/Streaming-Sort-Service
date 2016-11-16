using ServiceUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingSortWCFServiceClient
{
    class Program
    {
        const int MAX_STREAM_ATTEMPTS = 10;

        static void Main(string[] args)
        {
            Random random = new Random();
            List<string> sentText = new List<string>();
            List<string> sortedText = new List<string>();
            SortingServiceClient ssClient = new SortingServiceClient();
            int putDataAttemps = 0;
            bool showResultTexts = false;
            RandomTextGenerator rtg = new RandomTextGenerator(10,60,20);
            try
            {
                SortingServiceClient serviceClient = new SortingServiceClient();

                Guid transactionGuid = ssClient.BeginStream();
                Console.WriteLine("Streaming began for session ID:\n"+ transactionGuid + "\nPlease wait ...\n");
                while (/*Console.ReadKey().Key != ConsoleKey.Escape &&*/ putDataAttemps++< MAX_STREAM_ATTEMPTS)
                {
                    Console.WriteLine("sentText.Count " + sentText.Count);
                    List<string> rndList = new List<string>();
                    rndList.AddRange(rtg.GetRandomStringList());
                    

                    sentText.AddRange(rndList);
                    ssClient.PutStreamData(transactionGuid, rndList.ToArray());
                }
                Console.WriteLine("Streaming ended.");
                CollectSortedText(serviceClient, transactionGuid, sortedText);

                sentText.Sort((x, y) => String.CompareOrdinal(x, y));
                bool isEqual = sentText.SequenceEqual(sortedText);
                if (showResultTexts)
                {
                    Console.WriteLine("sentText \n");
                    foreach (string line in sentText)
                    {
                        Console.WriteLine(line);
                    }
                    Console.WriteLine("sortedText \n");
                    foreach (string line in sortedText)
                    {
                        Console.WriteLine(line);
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("Service operation test: " + (isEqual?"Passed":"Failed"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException occured. The message is : " + ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Press <ENTER> to exit application.");
            Console.ReadLine();

        }

        /// <summary>
        /// Simple reader of service sorted result. 
        /// </summary>
        /// <param name="ssClient">instance of sorting service client</param>
        /// <param name="transactionGuid">Session Guid</param>
        /// <param name="sortedText">List to be filled with sorted result</param>
        private static void CollectSortedText(SortingServiceClient ssClient, Guid transactionGuid, List<string> sortedText)
        {
            if (transactionGuid == default(Guid))
            {
                throw new ArgumentException("Invalid Session ID");
            }

            var sortedStream = ssClient.GetSortedStream(transactionGuid);
            using (StreamReader reader = new StreamReader(sortedStream))
            {
                while (!reader.EndOfStream)
                {
                    sortedText.Add(reader.ReadLine());
                }
            }
        }
        
    }

}
