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

            if (!File.Exists(filePath))
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer, match);
                   
                }
               
            }
            else
            {

                var jsonData = File.ReadAllText(filePath);

                jsonData = JsonConvert.SerializeObject(match);
                File.WriteAllText(filePath, jsonData);

            }
        }

        public List<Match> GetMatches()
        {
            if (!File.Exists(filePath)) return new List<Match>();
            var jsonData = File.ReadAllText(filePath);
            var matchList = JsonConvert.DeserializeObject<List<Match>>(jsonData);

            return matchList;
        }
    }
}