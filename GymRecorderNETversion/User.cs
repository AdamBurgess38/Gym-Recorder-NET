using System;
using System.Linq;
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
        private IDictionary<string, Exercise> usersExercise = new Dictionary<string, Exercise>();

        public User(string name)
        {
            
            this.name = name;
            
        }

        public string getName()
        {
           
            return name;
        }

        public string getAllExercises()
        {
            string returnString = "";
            foreach(var (key,value) in usersExercise)
            {
                returnString += "---------------------------------------------\n" + value + "---------------------------------------------\n";
            }
            return returnString;
        }

        public bool exerciseExists(string exerciseName)
        {
            return usersExercise.ContainsKey(exerciseName);
        }

        public void removeExerciseIteration(string exerciseName, int ID)
        {
            if (usersExercise[exerciseName].removeIteration(ID))
            {
                Console.WriteLine("Valid remove occurred, ID: " + ID + " removed from " + exerciseName);
            }
            else
            {
                Console.WriteLine("ID: " + ID + " not found for exercise " + exerciseName);
            }
        }

        public void printAverageOverall(string exerciseName)
        {
            Console.WriteLine(usersExercise[exerciseName].getOverallAverage());
        }

        public void printMostRecent(string exerciseName)
        {
            Console.WriteLine(usersExercise[exerciseName].getMostRecent());
        }

        public void printStandardStats(string exerciseName)
        {
            Console.WriteLine(usersExercise[exerciseName].ToString());
        }

        public void printSimpleStats(string exerciseName)
        {
            Console.WriteLine(usersExercise[exerciseName].getSimple());
        }




        public void removeExercise(string exerciseName)
        {
            Console.WriteLine("Removing: " + exerciseName + " from " + name + " database...");
            if (usersExercise.ContainsKey(exerciseName))
            {
                usersExercise.Remove(exerciseName);
                Console.WriteLine("Remove was successful.");
                return;
            }
            Console.WriteLine("Exercise:" + exerciseName +" was not found on the system");
        }

        public string getAllExercisesKeys()
        {
            string returnString = "";
            foreach (var (key, value) in usersExercise)
            {
                returnString += key + "\n";
            }
            return returnString;
        }

        public void addExerciseInstance(string exerciseName, double weight, double[] reps, double[] weights, string note)
        {
            if (reps.Length != weights.Length)
            {
                Console.WriteLine("Reps array must be the same Length as weights array");
                return;
            }
            if (usersExercise.ContainsKey(exerciseName))
            {
                usersExercise[exerciseName].addInstance(weight, reps, weights, note, DateTime.Now.ToShortDateString());
            }
            else
            {
                Exercise newExercise = new Exercise(exerciseName, weight, reps, weights, note, DateTime.Now.ToShortDateString());
                usersExercise.Add(exerciseName, newExercise);
            }

        }
    }
}

