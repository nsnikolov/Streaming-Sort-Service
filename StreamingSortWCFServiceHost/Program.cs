using StreamingSortWCFService;
using System;
using System.ServiceModel;
using System.Configuration;

namespace StreamingSortWCFServiceHost
{
    class Program
    {
        #region constants

        const string SSSERVICE_HOST = "SERVICE_HOST";
        const string SSSERVICE_PORT = "SERVICE_PORT";
        const string DEFAULT_SSSERVICE_HOST = "localhost";
        const string DEFAULT_SSSERVICE_PORT = "8900"; //from https://en.wikipedia.org/wiki/List_of_TCP_and_UDP_port_numbers it seems that thsi port should be available

        #endregion

        static void Main(string[] args)
        {
            ServiceHost serviceHost = CeateStreamingSortWCFServiceHost(
                String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[SSSERVICE_HOST]) ? DEFAULT_SSSERVICE_HOST : ConfigurationManager.AppSettings[SSSERVICE_HOST],
                String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[SSSERVICE_PORT]) ? DEFAULT_SSSERVICE_PORT : ConfigurationManager.AppSettings[SSSERVICE_PORT]
                );

            try
            {
                serviceHost.Open();

                Console.WriteLine("WCFStreamingSortService is running.");
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.ReadLine();

                serviceHost.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception occurred: {0}", ex.Message);
                serviceHost.Abort();
            }
        }

        private static ServiceHost CeateStreamingSortWCFServiceHost(string host, string port)
        {
            if(String.IsNullOrWhiteSpace(host) || String.IsNullOrWhiteSpace(port))
            {
                throw new ArgumentNullException(String.IsNullOrWhiteSpace(host)?host:port);
            }

            Uri address = new Uri("net.tcp://" + host + ":" + port);
            ServiceHost selfHost = new ServiceHost(typeof(StreamingSortService), address);
            NetTcpBinding netTcpBinding = new NetTcpBinding { TransferMode = TransferMode.Streamed };
            netTcpBinding.SendTimeout = TimeSpan.FromSeconds(120);
            netTcpBinding.ReceiveTimeout = TimeSpan.FromSeconds(120);
            netTcpBinding.MaxBufferSize = 50 * 1024 * 1024;
            netTcpBinding.MaxBufferPoolSize = 1000 * 1024 * 1024;
            netTcpBinding.MaxReceivedMessageSize = 1000 * 1024 * 1024;

            selfHost.AddServiceEndpoint(typeof(IStreamingSortService), netTcpBinding, "StreamingSortService");

            return selfHost;
        }
    
 
    }
}
