using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundial2018.Model
{
    public class Match
    {
        private Team _host;
        private Team _guest;

        private string _score;
        private DateTime _date;
        private string _place;
        private string _stage;

        public Team Host { get => _host; set => _host = value; }
        public Team Guest { get => _guest; set => _guest = value; }
        public string Score { get => _score; set => _score = value; }
        public DateTime Date { get => _date; set => _date = value; }
        public string Place { get => _place; set => _place = value; }
        public string Stage { get => _stage; set => _stage = value; }

        public Match(Team host, Team guest, string score, DateTime date,string place,string stage)
        {
            _host = host;
            _guest = guest;
            _score = score;
            _date = date;
            _place = place;
            _stage = stage;
        }
    }
}
