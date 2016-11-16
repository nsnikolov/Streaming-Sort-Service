using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageProvider
{
    public class StorageProviderManager
    {
        const string PREFERED_STORAGE_PROVIDER = "PREFERED_STORAGE_PROVIDER";
        const string DEFAULT_SORTABLE_PROVIDER = "File";

        private StorageProviderManager()
        {

        }

        /// <summary>
        /// Shoult return ISortableStorageProvider according to configuration
        /// implemented only for case of File for now
        /// </summary>
        /// <returns>ISortableStorageProvider</returns>
        public static ISortableStorageProvider GetDefaultSortableProvider()
        {

            ISortableStorageProvider retValue =null;
            string preferredSP = String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[PREFERED_STORAGE_PROVIDER]) ? DEFAULT_SORTABLE_PROVIDER : ConfigurationManager.AppSettings[PREFERED_STORAGE_PROVIDER];

            if (preferredSP.Contains("File"))
            {
                retValue = new FileStorageProvider();
            }
            else
            {
                throw new NotImplementedException();
            }
            return retValue;
        }

        /// <summary>
        /// R&D stuff
        /// Should return generic storage provider by generic type but it is not implemented yet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetDefaultProvider<T>()
        {
            T retValue = default(T);
            string preferredSP = String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[PREFERED_STORAGE_PROVIDER]) ? DEFAULT_SORTABLE_PROVIDER : ConfigurationManager.AppSettings[PREFERED_STORAGE_PROVIDER];


            if (preferredSP.Contains("File") && typeof(T).ToString().Contains("Sortable"))
            {
                try
                {
                    retValue = (T)Convert.ChangeType(new FileStorageProvider(), typeof(T));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GetDefaultSortableProvider ex " + ex + " " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("GetDefaultSortableProvider 2 ");
                throw new NotImplementedException();
            }
            return retValue;
        }

    }
}
