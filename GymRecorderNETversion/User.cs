using System;
using Newtonsoft.Json;

namespace GymRecorderNETversion
{
    
    public class User
    {
        [JsonPropertyAttribute]
        public string name
        {
            get;
            private set;
        }

        [JsonPropertyAttribute]
        private IDictionary<int, string> numberNames = new Dictionary<int, string>();

        public User(string name)
        {
            
            this.name = name;
            numberNames.Add(10, "Hello");
        }

        public string getName()
        {
           
            return name;
        }

        
    }
}

