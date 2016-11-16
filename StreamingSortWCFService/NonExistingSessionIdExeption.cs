using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingSortWCFService
{
    [Serializable]
    public class NonExistingSessionIdExeption : Exception
    {
        public NonExistingSessionIdExeption(Guid guid)
            : base($"There is no live session with ID {guid} ")
        {
        }
    }
}
