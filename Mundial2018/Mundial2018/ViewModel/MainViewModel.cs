using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Mundial2018.Model;
using Newtonsoft.Json;
using System;
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
        private string _hostName;
        private string _guestName;
        private string _score;
        private DateTime _datePicker;

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

        public MainViewModel(IDataService dataService)
        {
            AddMatchCommand = new RelayCommand(AddData);
        }

        private void AddData()
        {
            JsonSerializer serializer = new JsonSerializer();
            Team host = new Team(_hostName);
            Team guest = new Team(_guestName);

            Match newMatch = new Match(host, guest, _score, _datePicker);
            using (StreamWriter sw = new StreamWriter(@"..\json.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, newMatch);
             
            }
        }

     
    }
}