using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ServiceUtils
{
    /// <summary>
    /// Merge file sorter used by sortable file storage
    /// implementation is based on article: http://www.sinbadsoft.com/blog/sorting-big-files-using-k-way-merge-sort/
    /// please note that you need to install Sinbadsoft.Lib.Collections from nuGet
    /// </summary>
    public class KWayFileSorter
    {
        /// <summary>
        /// public interface for sorting files using 
        /// </summary>
        /// <param name="filepath">Path to file to be sorted</param>
        /// <param name="resultFilePath">Path to file where we want results to be written</param>
        /// <param name="chunkSize">size of chunks(number of lines), file to be divided duiriing sorting process</param>
        public void SortFile(string filepath, string resultFilePath, long chunkSize)
        {
            var chunkFilePaths = SplitInSortedChunks(filepath, chunkSize);
            KwayMerge(chunkFilePaths, resultFilePath);
        }

        #region private implemetation methods
        private void KwayMerge(IEnumerable<string> chunkFilePaths, string resultFilePath)
        {
            var chunkReaders = chunkFilePaths
                .Select(path => new StreamReader(path))
                .Where(chunkReader => !chunkReader.EndOfStream)
                .ToList();

            var queue = new Sinbadsoft.Lib.Collections.PriorityQueue<string, TextReader>((x, y) => -string.CompareOrdinal(x, y));
            chunkReaders.ForEach(chunkReader => queue.Enqueue(chunkReader.ReadLine(), chunkReader));

            using (var resultWriter = new StreamWriter(resultFilePath, false))
                while (queue.Count > 0)
                {
                    var smallest = queue.Dequeue();
                    var line = smallest.Key;
                    var chunkReader = smallest.Value;

                    resultWriter.WriteLine(line);

                    var nextLine = chunkReader.ReadLine();
                    if (nextLine != null)
                    {
                        queue.Enqueue(nextLine, chunkReader);
                    }
                    else
                    {
                        chunkReader.Dispose();
                    }
                }
        }
        private string FlushBuffer(List<string> buffer)
        {
            buffer.Sort(StringComparer.Ordinal);
            var chunkFilePath = Path.GetTempFileName();
            File.WriteAllLines(chunkFilePath, buffer);
            buffer.Clear();
            return chunkFilePath;
        }
        private IEnumerable<string> SplitInSortedChunks(string filepath, long chunkSize)
        {
            var buffer = new List<string>();
            var size = 0L;

            using (var reader = new StreamReader(filepath))
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    if (size + line.Length + 2 >= chunkSize)
                    {
                        size = 0L;
                        yield return FlushBuffer(buffer);
                    }

                    size += line.Length + 2;
                    buffer.Add(line);
                }

            if (buffer.Any())
            {
                yield return FlushBuffer(buffer);
            }
        }
        #endregion
    }
}
