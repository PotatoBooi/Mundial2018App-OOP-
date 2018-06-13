using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mundial2018.Model
{
    public interface IDataService
    {
        List<Match> GetMatches();
        void AddData(List<Match> match);
    }
}
