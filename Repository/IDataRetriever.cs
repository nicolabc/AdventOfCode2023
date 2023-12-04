using System.Runtime.CompilerServices;

namespace Repository
{
    public interface IDataRetriever
    {
        IEnumerable<string> GetData(string filename);
    }
}