using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solutions
{
    public abstract class AdventSolutionBase : IAdventSolution
    {
        private readonly IDataRetriever _dataRetriever;

        protected AdventSolutionBase(IDataRetriever dataRetriever)
        {
            _dataRetriever = dataRetriever;
        }

        public abstract int FirstQuestion(string filename);

        public abstract int FirstQuestion();

        public abstract int SecondQuestion(string filename);

        public abstract int SecondQuestion();

        protected IEnumerable<string> GetAllLines(string filename) => _dataRetriever.GetData(filename);
    }
}
