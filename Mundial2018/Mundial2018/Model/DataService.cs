using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mundial2018.Model
{
    public class DataService : IDataService
    {

        private const string filePath = @"..\json.json";
       
        public void AddData(List<Match> match)
        {
            //using (StreamWriter sw = new StreamWriter(filePath))
            //using (JsonWriter writer = new JsonTextWriter(sw))
            //{
            //    serializer.Serialize(writer, match);

            //}

            var jsonData = File.ReadAllText(filePath);
            
            jsonData = JsonConvert.SerializeObject(match);
            File.WriteAllText(filePath, jsonData);


        }

        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to connect to the actual data service

            var item = new DataItem("Welcome to MVVM Light");
            callback(item, null);
        }

        public IEnumerable<Match> GetMatches()
        {
            var jsonData = File.ReadAllText(filePath);
            var matchList = JsonConvert.DeserializeObject<List<Match>>(jsonData);
            
            return matchList;
        }
    }
}