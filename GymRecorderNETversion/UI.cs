using System;

using Newtonsoft.Json;

namespace GymRecorderNETversion
{
    public class UI
    {
        private User user = null; 
        public void onStart()
        {
            Console.WriteLine("Welcome!\n");
            string statement = "[1] Log in [2] Create Account\n";
            while (user == null){
                int choice = fetchChoice(2, statement);
                switch (choice)
                {
                    case 1:
                        user = logIn();
                        break;
                    case 2:
                        user = createAccount();
                        break;
                }
            }
            
            systemChoices();
        }

        private void saveState()
        {

            //WrapperUser tempwrapper = new WrapperUser(user);
            string fileName = user.getName() + ".json";
            //string jsonString = JsonSerializer.Serialize<User>(user);
            string jsonString = JsonConvert.SerializeObject(user, Formatting.Indented);

            Console.WriteLine(jsonString);

            File.WriteAllText(fileName, jsonString);
        }

        private User logIn()
        {
            string username = fetchInput("Please enter your username");
            if (File.Exists(username + ".json") ? true : false)
            {
                Console.WriteLine("Username exists");
                string jsonString = File.ReadAllText(username + ".json");
                User user = JsonConvert.DeserializeObject<User>(jsonString);
                if(user == null)
                    Console.Write("Error generating user\n");
                return user;
            }

            Console.WriteLine("This username does not exist, please try again");
            writeNewLine();
            return null;


        }

        private User createAccount()
        {
            string statement = "Please enter what you would like your username to be:";
            string username = fetchInput(statement);
            Console.WriteLine(username);
            while(File.Exists(username+".json") ? true : false)
            {
                Console.WriteLine("Username already exists, try a different username.");
                writeNewLine();
                username = fetchInput(statement);
            }

            return new User(username);
        }

        private void systemChoices()
        {
            while (true)
            {
                writeNewLine();
                int choice = fetchChoice(5, "[1] Add exercise instance [2] View an exercise [3] Delete an exercise [4] View all exercises [5] Save\n");
                switch (choice)
                {
                    case 1:
                        addExerciseInstance();
                        break;
                    case 2:
                        viewAnExercise();
                        break;
                    case 3:
                        deleteExercise();
                        break;
                    case 4:
                        viewAllExercises();
                        break;
                    case 5:
                        saveState();
                        break;
                }
            }
        }

        private void viewAnExercise()
        {
            string exerciseName = fetchInput("What exercise would you like to view?");
            if (user.exerciseExists(exerciseName))
            {
                int choice = fetchChoice(4, "What format would you like?\n[1] Standard stats [2] Average overall [3] Simple stats  [4] Most recent\n");
                switch (choice)
                {
                    case 1:
                        user.printStandardStats(exerciseName);
                        break;
                    case 2:
                        user.printAverageOverall(exerciseName);
                        break;
                    case 3:
                        user.printSimpleStats(exerciseName);
                        break;
                    case 4:
                        user.printMostRecent(exerciseName);
                        break;
                }
            }
        }

        public void deleteExercise()
        {
            int choice = fetchChoice(2,"[1] Delete a single iteration [2] Delete all of an exercise\n");
            
            switch (choice)
            {
                case 1:
                    this.deleteSingleIteration();
                    break;
                case 2:
                    this.deleteEntireExercise();
                    break;
            }
        }

        private void deleteSingleIteration()
        {
            string exerciseName = fetchInput("State the exercise you would like to delete?");
            
            if (!this.user.exerciseExists(exerciseName))
            {
                Console.WriteLine("Excerise not found");
                return;
            };
            
            int ID = fetchInteger("State the ID of the exercise you would like to delete?");
            this.user.removeExerciseIteration(exerciseName, ID);
           
        }

        private void deleteEntireExercise()
        {
            string exerciseName = fetchInput("What exercise do you wish to remove?");
            if(binaryQuery("Are you sure you want to delete" + exerciseName + "?"))
            {
                user.removeExercise(exerciseName);
            }
        }

        private void viewAllExercises()
        {
            int choice = fetchChoice(2, "[1] All names [2] All iterations of every exercise\n");
            switch(choice)
            {
                case 1:
                    Console.WriteLine(user.getAllExercisesKeys());
                    break;
                case 2:
                    Console.WriteLine(user.getAllExercises());
                    break;
            }
        }



        private void addExerciseInstance()
        {
            string exerciseName = fetchInput("State the name of the exercise:");
            double weight = fetchDouble("Please state the weight you worked at:");
            bool sameLengthArrays = false;
            bool constantWeight = binaryQuery("Was the weight constant throughout?");
            int sets;
            int tempReps;
            bool constantReps = binaryQuery("Were the reps the same throughout?");
            double[] reps = new double[1];
            double[] weights = new double[1];
            if (constantWeight && constantReps)
            {
                tempReps = fetchInteger("What was this value?");
                sets = fetchInteger("How many sets did you do?");
                reps = new double[sets];
                weights = new double[sets];
                Array.Fill(reps, tempReps);
                Array.Fill(weights, weight);
            }
            else if (constantWeight)
            {
                reps = fetchDoubleArray("Please state the reps per set:");
                weights = new double[reps.Length];
                Array.Fill(weights, weight);
            }
            else if (constantReps)
            {
                tempReps = fetchInteger("What was this value?");
                weights = fetchDoubleArray("Please state the weights per set:");
                reps = new double[weights.Length];
                Array.Fill(reps, tempReps);
            }
            else if(!constantReps && !constantWeight)
            {
                while (!sameLengthArrays)
                {
                    reps = fetchDoubleArray("Please state the reps per set:");
                    weights = fetchDoubleArray("Please state the weights per set:");
                    if (reps.Length == weights.Length)
                    {
                        sameLengthArrays = true;
                    }
                }
            }
            string note = "";
            if(binaryQuery("Would you like to leave a note? [Y] Yes [N] No:"))
            {
                note = fetchInput("Please type in your note");
            }

            user.addExerciseInstance(exerciseName, weight, reps, weights, note);
        }

        private bool binaryQuery(string statement)
        {
            Console.WriteLine(statement);
            string returnString = Console.ReadLine().ToUpper();
            
            while (returnString == null || returnString != "Y" && returnString != "N")
            {
                Console.Write("Please type in something, value cannot be null.\n" + statement + "\n");
                returnString = Console.ReadLine().ToUpper();
            }

            return returnString == "Y" ? true: false ;
        }
        

        private double[] fetchDoubleArray(string statement)
        {
            Console.WriteLine(statement);
            double[] returnString = Array.ConvertAll(Console.ReadLine().Split(' '), double.Parse);
            while (returnString == null)
            {
                Console.Write("Please type a set of numbers, value cannot be null.\n" + statement + "\n");
                returnString = Array.ConvertAll(Console.ReadLine().Split(' '), double.Parse);
            }

            return returnString;
        }

        private string fetchInput(string statement)
        {
            Console.WriteLine(statement);
            string returnString = Console.ReadLine();
            while(returnString == "" || returnString == null)
            {
                Console.Write("Please type in something, value cannot be null.\n" + statement + "\n");
                returnString = Console.ReadLine();
            }
            
            return returnString;
        }

        private int fetchInteger(string statement = "")
        {
            bool validValue = false;
            int currentValue = -1;
            while (!validValue)
            {
                Console.Write(statement + "\n");
                try
                {
                    currentValue = Convert.ToInt32(Console.ReadLine());
                    validValue = true;
                }
                catch
                {
                    Console.WriteLine("Please enter a number");
                }
            }
            return currentValue;
        }

        private double fetchDouble(string statement = "")
        {
            bool validValue = false;
            double currentValue = -1;
            while (!validValue)
            {
                Console.Write(statement+"\n");
                try
                {
                    currentValue = Convert.ToDouble(Console.ReadLine());
                    validValue = true;
                }
                catch
                {
                    Console.WriteLine("Please enter a number");
                }
            }
            return currentValue;
        }


        public int fetchChoice(int maxInput, string statement="")
        {
            bool validValue = false;
            int currentValue = -1;
            while (!validValue)
            {
                Console.Write(statement);
                try
                {
                    currentValue = Convert.ToInt32(Console.ReadLine());
                    if ((maxInput - currentValue) >= 0 && (maxInput - currentValue) <= maxInput && currentValue <= maxInput && currentValue != 0)
                    {
                        validValue = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a integer between 1 and " + maxInput);
                    }

                }
                catch
                {
                    Console.WriteLine("Please enter a integer between 1 and " + maxInput);
                }
            }
            return currentValue;
        }

        private void writeNewLine()
        {
            Console.WriteLine("-----------------------------------------------------------------------------------------------");
        }


    }
}

