using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceUtils
{
    public class Logger
    {
        
        public static string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(2);

            return sf.GetMethod().Name;
        }

        public static void LogError(Exception ex)
        {
            // For current implementation just a simple logging is enough
            Console.WriteLine(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss") + ": Error occured in Method: "+GetCurrentMethod()+"  Exception message is:"+ex.Message);
        }
    }
}
