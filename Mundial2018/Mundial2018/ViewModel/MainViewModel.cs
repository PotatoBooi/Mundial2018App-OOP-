using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Mundial2018.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;

using System.Windows.Data;

namespace Mundial2018.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly IDataService _dataService;

        private readonly List<string> _stageList = new List<string>(new string[] { "Faza grupowa", "1/8 finału", "Ćwierćfinał", "Półfinał", "Finał" });
        private readonly List<string> _citiesList = new List<string>(new string[] { "Moskwa", "Soczi", "Kaliningrad", "Terek" });
        private readonly List<string> _teamsList = new List<string>(new string[] { "Rosja", "Francja", "Portugalia", "Niemcy", "Serbia",
            "Polska", "Anglia", "Hiszpania", "Belgia", "Islandia", "Szwajcaria", "Szwecja", "Chorwacja", "Dania", "Tunezja", "Nigeria", "Maroko",
            "Senegal", "Egipt", "Iran", "Japonia", "Korea Płd", "Arabia Saud", "Brazylia", "Urugwaj", "Argentyna", "Kolumbia", "Meksyk", "Panama", "Australia", "Peru" });
        private Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();
 
        private string _hostName;
        private string _guestName;
        private int _leftScore;
        private int _rightScore;
        private DateTime _datePicker;
        private ObservableCollection<Match> _matchCollection;
        private string _score;
        private string _selectedStage;
        private string _selectedCity;
        private Match _selectedMatch;
        private ObservableCollection<string> _stageCollection;
        private ObservableCollection<string> _citiesCollection;

        public RelayCommand AddMatchCommand { get; private set; }
        public RelayCommand OnLoadCommand { get; private set; }
        public RelayCommand<CancelEventArgs> OnCloseCommand { get; private set; }
        public RelayCommand RemoveMatchCommand { get; private set; }

        public string GuestName
        {
            get { return _guestName; }
            set { Set(ref _guestName, value); }
        }

        public string HostName
        {
            get { return _hostName; }
            set { Set(ref _hostName, value); }
        }


        public List<string> SuggestedTeams { get => _teamsList; }
        public DateTime DatePicker { get => _datePicker; set => Set(ref _datePicker, value); }
        public ObservableCollection<Match> MatchCollection { get => _matchCollection; set => Set(ref _matchCollection, value); }

        public int LeftScore
        {
            get => _leftScore;
            set => Set(ref _leftScore, value);
        }
        public int RightScore { get => _rightScore; set => Set(ref _rightScore, value); }
        public ObservableCollection<string> StageCollection { get => _stageCollection; set => Set(ref _stageCollection, value); }
        public ObservableCollection<string> CitiesCollection { get => _citiesCollection; set => Set(ref _citiesCollection, value); }
        public string SelectedStage { get => _selectedStage; set => Set(ref _selectedStage, value); }
        public string SelectedCity { get => _selectedCity; set => Set(ref _selectedCity, value); }
        public Match SelectedMatch
        {
            get => _selectedMatch; set =>
Set(ref _selectedMatch, value);
        }



        #region Error Handling
        public string Error => string.Empty;

        public string this[string propertyName]
        {
            get
            {
                //_error = string.Empty;



                //if(IsSameName())
                //{
                //    _error = "Nazwy zespołów nie mogą być takie same";
                //}
                //if(!string.IsNullOrEmpty(HostName)&& !IsValidName(HostName))
                //{
                //    _error = "Tylko litery dowzwolone!";
                //}
                //if(!string.IsNullOrEmpty(HostName) && char.IsWhiteSpace(HostName[0]))
                //{
                //    _error = "Nazwa powinna zaczynać się z dużej litery";
                //}
                //if (!string.IsNullOrEmpty(GuestName) && char.IsWhiteSpace(GuestName[0]))
                //{
                //    _error = "Nazwa powinna zaczynać się z dużej litery";
                //}
                CollectErrors();
                return Errors.ContainsKey(propertyName) ? Errors[propertyName] : string.Empty;


            }
        }
        #endregion
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _matchCollection = new ObservableCollection<Match>();
            _stageCollection = new ObservableCollection<string>(_stageList);
            _citiesCollection = new ObservableCollection<string>(_citiesList);
            _selectedStage = _stageCollection[0];
            _selectedCity = _citiesCollection[0];
            _datePicker = DateTime.Today;
            OnLoadCommand = new RelayCommand(UpdateList);
            OnCloseCommand = new RelayCommand<CancelEventArgs>((args) => { AddData(); });
            AddMatchCommand = new RelayCommand(AddMatch, canAdd);
            RemoveMatchCommand = new RelayCommand(RemoveMatch, CanRemove);

        }

        private void CollectErrors()
        {
            Errors.Clear();
            if (string.IsNullOrEmpty(HostName))
            {
                Errors.Add(nameof(HostName), "Wprowadź nazwę zespołu");
            }
            else if (!IsValidName(HostName))
            {
                Errors.Add(nameof(HostName), "Tylko litery dowzwolone!");
            }


            if (string.IsNullOrEmpty(GuestName))
            {
                Errors.Add(nameof(GuestName), "Wprowadź nazwę zespołu");
            }
            else if (!IsValidName(GuestName))
            {
                Errors.Add(nameof(GuestName), "Tylko litery dowzwolone!");
            }
            if (IsValidName(GuestName) && IsValidName(HostName) && IsSameName())
            {

                Errors.Add(nameof(HostName), "Podaj różne zespoły");
                Errors.Add(nameof(GuestName), "Podaj różne zespoły");

            }
            AddMatchCommand.RaiseCanExecuteChanged();
        }
        private bool hasErrors => (Errors.Count > 0);
        private bool canAdd()
        {
            return !hasErrors && !MatchCollection.Contains(CreateMatch());
        }
        private void AddMatch()
        {

            Match newMatch = CreateMatch();
            MatchCollection.Add(newMatch);
            _selectedMatch = MatchCollection[MatchCollection.IndexOf(newMatch)];
            RaisePropertyChanged(() => MatchCollection);
            HostName = string.Empty;
            GuestName = string.Empty;
            LeftScore = 0;
            RightScore = 0;
            DatePicker = DateTime.Today;

        }
        private Match CreateMatch()
        {
            Team host = new Team(_hostName);
            Team guest = new Team(_guestName);
            _score = LeftScore + ":" + RightScore;

            Match newMatch = new Match(host, guest, _score, _datePicker, _selectedCity, _selectedStage);
            return newMatch;
        }
        private void AddData()
        {

            List<Match> output = new List<Match>();
            foreach (var item in _matchCollection)
            {
                output.Add(item);
            }

            _dataService.AddData(output);
            // UpdateList();

        }
        private void UpdateList()
        {
            var list = _dataService.GetMatches();
            if (list.Count == 0)
                return;
            foreach (var item in list)
            {
                if (!MatchCollection.Contains(item))
                {
                    MatchCollection.Add(item);
                }
            }
        }
        private void RemoveMatch()
        {

            _matchCollection.Remove(_selectedMatch);
            //RaisePropertyChanged(() => MatchCollection);
        }
        private bool CanRemove()
        {
            if (SelectedMatch == null || MatchCollection.Count == 0)
            {
                return false;

            }
            else
                return true;
        }

        private bool IsSameName()
        {
            if (HostName != null && GuestName != null)
            {
                string name1 = HostName?.ToLower();
                string name2 = GuestName?.ToLower();
                if (RemoveSpaces(name1) == RemoveSpaces(name2))
                {
                    return true;
                }


            }
            return false;

        }

        private string RemoveSpaces(string input)
        {
            var s = new StringBuilder(input.Length); // (input.Length);
            using (var reader = new StringReader(input))
            {
                int i = 0;
                char c;
                for (; i < input.Length; i++)
                {
                    c = (char)reader.Read();
                    if (!char.IsWhiteSpace(c))
                    {
                        s.Append(c);
                    }
                }
            }

            return s.ToString();
        }
        private bool IsValidName(string name)
        {
            if (name != null)
            {

                for (int i = 0; i < name.Length; i++)
                {
                    if (char.IsLetter(name[i])) return true;
                    //if (i > 2 && char.IsWhiteSpace(name[i])) return true;


                }

            }
            return false;

        }

    }
}