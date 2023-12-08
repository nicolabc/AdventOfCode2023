using Repository;

namespace UnitTests
{
    public class TestDataRetriever : IDataRetriever
    {
        public IEnumerable<string> GetData(string filenameWithPath) => File.ReadAllLines(@".\..\..\..\" + filenameWithPath);
    }
}
