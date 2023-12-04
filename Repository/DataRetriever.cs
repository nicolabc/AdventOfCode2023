using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class DataRetriever : IDataRetriever
    {
        public IEnumerable<string> GetData(string filename) => File.ReadAllLines(@".\..\..\..\..\Repository\Data\" + filename);
    }
}
