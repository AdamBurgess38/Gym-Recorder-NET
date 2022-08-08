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

        private User logIn()
        {
            string username = fetchInput("Please enter your username");
            if (File.Exists(username + ".json") ? true : false)
            {
                Console.WriteLine("Username exists");
                string jsonString = File.ReadAllText(username + ".json");
                User user = JsonConvert.DeserializeObject<User>(jsonString);
                if (user != null)
                    Console.Write(user.getName());
                else
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
                int choice = fetchChoice(5, "[5] Save");
                switch (choice)
                {
                    case 5:
                        saveState();
                        break;
                }
            }
        }

        private void saveState()
        {
            
            //WrapperUser tempwrapper = new WrapperUser(user);
            string fileName = user.getName()+".json";
            //string jsonString = JsonSerializer.Serialize<User>(user);
            string jsonString = JsonConvert.SerializeObject(user, Formatting.Indented);

            Console.WriteLine(jsonString);
            
            File.WriteAllText(fileName, jsonString);
        }

        public string fetchInput(string statement)
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

