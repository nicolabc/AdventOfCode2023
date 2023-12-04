using Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class TestDataRetriever : IDataRetriever
    {
        public IEnumerable<string> GetData(string filenameWithPath) => File.ReadAllLines(@".\..\..\..\" + filenameWithPath);
    }
}
