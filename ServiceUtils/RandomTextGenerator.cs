using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceUtils
{
    /// <summary>
    /// RandomTextGenerator is gust provider of random text using alphanumeric characters and most common symbols 
    /// </summary>
    public class RandomTextGenerator
    {
        #region const

        const string POSSIBLE_CHARACTERS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.?!@#%";
        /// <summary>
        /// absolute limitation constants
        /// </summary>
        const int MAX_WORD_LENGHT = 15;
        const int MAX_LINE_LENGHT = 1000;
        const int MAX_LINE_NUMBER = 100000;

        #endregion

        #region Properties

        private Random random = new Random();
        /// <summary>
        /// limitation variables to set them use appropriat constructor
        /// </summary>
        private int maxWordLength = MAX_WORD_LENGHT;
        private int maxLineLength = MAX_LINE_LENGHT;
        private int maxLineNumber = MAX_LINE_NUMBER;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with initialization of limits to be folowed by generator
        /// </summary>
        /// <param name="maxWordLength"></param>
        /// <param name="maxLineLength"></param>
        /// <param name="maxLineNumber"></param>
        public RandomTextGenerator(int maxWordLength, int maxLineLength, int maxLineNumber)
        {
            this.maxWordLength = maxWordLength;
            this.maxLineLength = maxLineLength;
            this.maxLineNumber = maxLineNumber;
        }

        #endregion

        #region public methods

        /// <summary>
        /// method generates random text with random number of lines, random line legth and random words according to limitation variables
        /// </summary>
        /// <returns></returns>
        public List<string> GetRandomStringList()
        {
            List<string> retValue = new List<string>();
            int lines = random.Next(maxLineNumber);
            while(retValue.Count < lines)
            {
                retValue.Add(GetRandomLine());
            }
            return retValue;
        }
        #endregion

        #region private methods

        /// <summary>
        /// generates line of text with random lenght. Not need to be exposed to outside for now.
        /// </summary>
        /// <returns></returns>
        private string GetRandomLine()
        {
            char[] stringChars = new char[random.Next(maxLineLength)];
            int nextSpacePos = random.Next(maxWordLength);
            for (int i = 0; i < stringChars.Length; i++)
            {
                if (i == nextSpacePos)
                {
                    stringChars[i] = ' ';
                    nextSpacePos = i + random.Next(maxWordLength);
                }
                else
                {
                    stringChars[i] = POSSIBLE_CHARACTERS[random.Next(POSSIBLE_CHARACTERS.Length)];
                }
            }
            return new String(stringChars);
        }

        #endregion
    }
}
