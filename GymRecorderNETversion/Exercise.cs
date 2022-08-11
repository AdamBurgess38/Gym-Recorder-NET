using System;
using Newtonsoft.Json;

namespace GymRecorderNETversion
{
    public class Exercise
    {
        public string exerciseName
        {
            get;
            private set;
        }
        [JsonPropertyAttribute]
        private List<ExerciseInstant> iterations = new List<ExerciseInstant>();

        [JsonConstructor]
        public Exercise(string exerciseName)
        {
            this.exerciseName = exerciseName;
        }

        public Exercise(string exerciseName, double weight, double[] reps, double[] weights, string note, string date)
        {
            this.exerciseName = exerciseName;
            iterations.Add(new ExerciseInstant(findValidID(), weight, reps, weights, note, date));
        }

        public void addInstance(double weight, double[] reps, double[] weights, string note, string date)
        {
            iterations.Add(new ExerciseInstant(findValidID(), weight, reps, weights, note, date));
        }

        private int findValidID()
        {
            bool validID = false;
            
            int ID = iterations.Count + 1;
            while (!validID)
            {
                validID = true;
                
                foreach (var exceriseInstant in iterations)
                {
                    if (exceriseInstant.ID == ID)
                    {
                        validID = false;
                        ID++;
                        break;
                    }
                }
            }

            return ID;
        }

        public bool removeIteration(int ID)
        {
            foreach(var i in iterations)
            {
                if(i.ID == ID)
                {
                    iterations.Remove(i);
                    return true;
                }
            }
            return false;
        }

        public string getSimple()
        {
            string returnString = "";
            foreach(var i in iterations)
            {
                returnString += "Date: " + i.date + " Weights: " + i.getWeightsArray() + " Reps: " + i.getRepsArray()+"\n";
            }
            return returnString;
        }

        public string getMostRecent()
        {
            string returnString = "\nThe most recent iteration of: " + exerciseName + "\n";
            ExerciseInstant instance = iterations[iterations.Count - 1];
            returnString += "Date: " + instance.date + "\n";
            
            if (instance.totalWeight == (instance.averageRep * instance.averageWeight) * instance.sets)
            {
                returnString += "Your weights and reps were constant throughout.\nYou did " + instance.sets + " sets at " + instance.averageWeight + "kg for " + instance.averageRep + " reps at at time.\n";
            }
            else if (instance.totalWeight == (instance.averageRep * instance.getWeights()[0]) * instance.sets)
            {
                returnString += "Your weight was constant throughout, but your reps differed.\n You did " + instance.sets + " sets at " + instance.averageWeight + "kg the reps throughout were " + instance.getRepsArray() + ".\n";
            }
            else if (instance.totalWeight == (instance.averageWeight * instance.getReps()[0]) * instance.sets)
            {
                returnString += "Your reps were constant throughout, but your weight differed.\n You did " + instance.sets + " sets for " + instance.averageRep + " reps at the weights " + instance.getWeightsArray() + ".\n";
            }
            returnString += "The total weight moved was: " + instance.totalWeight+"\n";
            if(instance.note != "")
            {
                returnString += "The note left was: " + instance.note + "\n";
            }

            return returnString;
        }

        public string getOverallAverage()
        {
            string returnString = "";
            foreach (var i in iterations)
            {
                returnString += "Date: " + i.date + " Average Weight: " + i.averageWeight + "kg Average Reps: " + i.averageRep + " Average total: " + i.averageWeightRepTotal +"kg\n";
            }
            return returnString;
        }

        public override string ToString()
        {
            string returnString = "";
            foreach(var instant in iterations)
            {
                returnString += "\n------------------------------------------------------------------\n"+instant + "\n------------------------------------------------------------------\n";
            }
            return exerciseName + returnString;
        }


        private class ExerciseInstant
        {
            public int ID {
                get;
                private set;
            }
            public int sets
            {
                get;
                private set;
            }
            public double weight
            {
                get;
                private set;
            }
            [JsonPropertyAttribute]
            private double[] reps;
            [JsonPropertyAttribute]
            private double[] weights;
            [JsonPropertyAttribute]
            private string[] variance;
            public string date
            {
                get;
                private set;
            }
            public string note
            {
                get;
                private set;
            }
            public double totalWeight
            {
                get;
                private set;
            }
            public double averageWeight
            {
                get;
                private set;
            }
            public double averageRep
            {
                get;
                private set;
            }
            public double averageWeightRepTotal
            {
                get;
                private set;
            }

            public double[] getReps() {
                return reps;
            }

            public double[] getWeights()
            {
                return weights;
            }

            public string getWeightsArray()
            {
                string returnString = "";
                foreach(int w in weights)
                {
                    returnString += w + "kg,";
                }
                return returnString.Remove(returnString.Length - 1);
            }

            public string getRepsArray()
            {
                string returnString = "";
                foreach (int r in reps)
                {
                    returnString += r + ",";
                }
                return returnString.Remove(returnString.Length - 1);
            }

            [JsonConstructor]
            public ExerciseInstant(int ID, double weight, double[] reps, double[] weights, string note, string date)
            {

                this.ID = ID;
                this.sets = reps.Length;
                this.weight = weight;
                this.weights = new double[weights.Length];
                double[] tempWeights = (double[])weights.Clone();
                this.reps = new double[reps.Length];

                this.variance = new string[weights.Length];
                this.note = note;
                for (int i = 0; i < tempWeights.Length; i++)
                {
                    this.weights[i] = weights[i];
                    this.reps[i] = reps[i];
                    tempWeights[i] -= weight;

                    this.variance[i] = (tempWeights[i].ToString());
                    if (tempWeights[i] > 0)
                    {
                        this.variance[i] = "+" + tempWeights[i];
                    }
                    else if (tempWeights[i] == 0)
                    {
                        this.variance[i] = "0";
                    }
                }

                this.date = date;
                this.generateAnaylsisValues();

            }

            private void generateAnaylsisValues()
            {
                double[] totalsRepWeight = new double[reps.Length];
                for(int i = 0; i<reps.Length; i++)
                {
                    totalWeight += reps[i] * weights[i];
                    totalsRepWeight[i] = reps[i] * weights[i];
                }
                averageWeight = weights.Sum() / weights.Length;
                averageRep = reps.Sum() / reps.Length;
                averageWeightRepTotal = totalsRepWeight.Sum() / reps.Length;

            }

            public override string ToString()
            {
                string repsString = "";
                foreach (var i in reps)
                {
                    repsString += i + ",";
                }
                repsString = repsString.Remove(repsString.Length - 1);
                string weightsString = "";
                foreach (var i in weights)
                {
                    weightsString += i + "kg,";
                }
                weightsString = weightsString.Remove(weightsString.Length - 1);
                string finalNote = "";
                if(this.note != "" || note != null)
                {
                    finalNote = "Note: " + note;
                }
                return "Date: " + date + "\nPlanned weight: " + weight
                    + "kg\nNumber of sets: " + sets +
                    "\nReps per set: " + repsString + "\nWeights per set: " + weightsString +
                    "\nAverage Weight: " + averageWeight + "kg\nAverage Reps: " + averageRep + "\nAverage total: " + averageWeightRepTotal +"kg\n"
                    + finalNote + "\n"; 
            }



        }
    }
}

