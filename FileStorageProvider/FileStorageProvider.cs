using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ServiceUtils;

namespace StorageProvider
{
    /// <summary>
    /// Storage provider which uses file for storage 
    /// </summary>
    public class FileStorageProvider : ISortableStorageProvider
    {
        #region constants

        const string OPERATION_FOLDER_NAME = "FileStorageFolder";
        const string DEFAULT_OPERATION_FOLDER_NAME = "FileStorageOF";
        const string CHUNK_SIZE = "FileStorageFolder";
        const long DEFAULT_CHUNK_SIZE = 10000;

        #endregion

        #region Interface implemetation methods
        private bool sortDirectionAccending = true;

        public bool SortDirectionAccending
        {
            get
            {
                return sortDirectionAccending;
            }

            set
            {
                sortDirectionAccending = value;
            }
        }

        public bool AddTextData(string[] textData, Guid transactionGuid)
        {
            bool retValue = true;
            try
            {
                string path = GetFilenameForGuid(transactionGuid);

                if (!File.Exists(path))
                {
                    using (FileStream fs = File.Create(path))
                    {

                    }
                }
                using (StreamWriter sw = File.AppendText(path))
                {
                    foreach(string line in textData)
                    {
                        sw.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                retValue = false;
                Logger.LogError(ex);
            }
            
            return retValue;
        }

        public bool DiscardTextData(Guid transactionGuid)
        {
            bool retValue = true;
            try
            {
                string path = GetFilenameForGuid(transactionGuid);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                retValue = false;
                Logger.LogError(ex);
            }

            return retValue;
        }

        public bool EmptyStorage()
        {
            bool retValue = true;
            try
            {
                Array.ForEach(Directory.GetFiles(GetOperationFolder()), File.Delete);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return retValue;
        }

        public Stream RetreiveSortedTextData(Guid transactionGuid)
        {
            Console.WriteLine("RetreiveSortedTextData"+ transactionGuid);
            Sort(transactionGuid);

            string path = GetFilenameForSortedFileForGuid(transactionGuid);

            return GetStreamForFile(path);
        }

        public Stream RetreiveTextData(Guid transactionGuid)
        {
           string path = GetFilenameForGuid(transactionGuid);
            
            return GetStreamForFile(path);
        }

        
        public void Sort(Guid transactionGuid)
        {
            KWayFileSorter sorter = new KWayFileSorter();
            sorter.SortFile(GetFilenameForGuid(transactionGuid), GetFilenameForSortedFileForGuid(transactionGuid), GetChunkFileSize());
        }
        #endregion

        #region private methods

        private string GetFilenameForGuid(Guid trGuid)
        {
            string fName = Path.Combine(GetOperationFolder(), trGuid.ToString()+".txt");
            return fName;
        }

        private string GetFilenameForSortedFileForGuid(Guid trGuid)
        {
            string fName = GetFilenameForGuid(trGuid).Replace(".txt", "_Sorted.txt");
            return fName;
        }

        private string GetOperationFolder()
        {

            string retValue = Path.GetTempPath();
            retValue = Path.Combine(retValue, String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[OPERATION_FOLDER_NAME]) ? DEFAULT_OPERATION_FOLDER_NAME : ConfigurationManager.AppSettings[OPERATION_FOLDER_NAME]);
            System.IO.Directory.CreateDirectory(retValue);
            return retValue;
        }

        private long GetChunkFileSize()
        {

            long retValue = DEFAULT_CHUNK_SIZE;
            string retValueStr = String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings[CHUNK_SIZE]) ? ""+DEFAULT_CHUNK_SIZE : ConfigurationManager.AppSettings[CHUNK_SIZE];
            bool parseSuccess = long.TryParse(retValueStr, out retValue);
            return retValue;
        }

        private Stream GetStreamForFile(string fileName)
        {
            Stream retValue = null;

            try
            {
                if (!File.Exists(fileName))
                {
                    File.Create(fileName);
                }
                retValue = File.OpenRead(fileName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return retValue;
        }

        #endregion
    }
}
