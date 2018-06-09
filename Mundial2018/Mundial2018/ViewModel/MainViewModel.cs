using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Mundial2018.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace Mundial2018.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        public RelayCommand AddMatchCommand { get; private set; }
        public RelayCommand OnLoadCommand { get; private set; }
        public RelayCommand<CancelEventArgs> OnCloseCommand { get; private set; }
        private string _hostName;
        private string _guestName;
        private string _score;
        private DateTime _datePicker;
        private ObservableCollection<Match> _matchCollection;

        public string Score
        {
            get { return _score; }
            set { Set(ref _score, value); }
        }


        public string GuestName
        {
            get { return _guestName; }
            set { Set(ref _guestName,value); }
        }

        public string HostName
        {
            get { return _hostName; }
            set { Set(ref _hostName, value); }
        }

        public DateTime DatePicker { get => _datePicker; set => Set(ref _datePicker, value); }
        public ObservableCollection<Match> MatchCollection { get => _matchCollection; set => Set(ref _matchCollection, value); }

        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _matchCollection = new ObservableCollection<Match>();
            // UpdateList();
            OnLoadCommand = new RelayCommand(UpdateList);
            OnCloseCommand = new RelayCommand<CancelEventArgs>((args)=> { AddData(); });
            AddMatchCommand = new RelayCommand(AddMatch);
        }

        private void AddMatch()
        {
            Team host = new Team(_hostName);
            Team guest = new Team(_guestName);

            Match newMatch = new Match(host, guest, _score, _datePicker);

            MatchCollection.Add(newMatch);
            RaisePropertyChanged(() => MatchCollection);
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
            foreach (var item in list)
            {
                if (!MatchCollection.Contains(item))
                {
                   MatchCollection.Add(item);
                }
            }
        }
     
    }
}