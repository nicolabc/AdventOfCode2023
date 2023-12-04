using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solutions
{
    public interface IAdventSolution
    {
        int FirstQuestion();

        int FirstQuestion(string filename);

        int SecondQuestion();

        int SecondQuestion(string filename);
    }
}
