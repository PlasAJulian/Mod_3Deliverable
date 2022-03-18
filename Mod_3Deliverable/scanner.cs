using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mod_3Deliverable
{

    class scanner
    {
        //Empty list that will store our findings.  
        public static List<string> fileList = new List<string>();
        public static List<string> keyWords = new List<string>();
        public static List<string> operators = new List<string>();
        public static List<string> identifiers = new List<string>();
        public static List<string> constants = new List<string>();

        //These are two strings made using the grammar text file and the Operator text file which are used throughout this project.
        //These files are found inside the files folder in the project.
        private static string grammer = File.ReadAllText("../../../files/scl_grammar.txt");
        private static string op = File.ReadAllText("../../../files/operator.txt");

        public void scan()
        {
            //Takes in the file loction from the user threw the console.
            Console.WriteLine("Please type the full loction of the file and its name. then push enter \nan it should look simmaler to the following  '" + "C:/Users/name/Desktop/folderName/fileName.scl'");
            string location = Console.ReadLine();

            //Checks if the file exists. If now allows user to enter another location. 
            while (!File.Exists(location))
            {
                Console.WriteLine("The file loction '" + location + "' does not exists. Please try again.");
                Console.WriteLine("Please type the full loction of the file and its name. then push enter \nan it should look simmaler to the following  '" + "C:/Users/name/Desktop/folderName/fileName.scl'");
                location = Console.ReadLine();
            }


            //Reads the file given the path and makes the file given into a single string.
            string fileText = File.ReadAllText(location);



            //Splits string by line and puts it into list.
            fileList = fileText.Split('\n').ToList();

            //The list is cleaned to remove comments and descriptions but being sent to our clean method. This line also removes empty lines in the list.  
            fileList = clean(fileList).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            //The list is then sent to FindKeyWords method to pull string that are keywords into keywords list and pull strings that are identifiers.  
            FindKeyWords(fileList);

            //The list is being sent to the FindOperartor method to pull any string that is an operator.
            FindOperator(fileList);

            //Everything is printed to console using the display method.  
            // display();
        }
        //Displays each list and the strings within each list.
        public static void display()
        {
            //Displays all the keywords.
            Console.WriteLine("\n\nThe Follwing are the keywords found: ");
            foreach (var word in keyWords)
            {
                Console.Write(word.Trim() + ", ");
            }

            //Displays all the identifiers.
            Console.WriteLine("\n\nThe Follwing are the identifiers found: ");
            foreach (var word in identifiers)
            {
                Console.Write(word.Trim() + ", ");
            }

            //Displays all the operators.
            Console.WriteLine("\n\nThe Follwing are the operators found: ");
            foreach (var word in operators)
            {
                Console.Write(word.Trim() + ", ");
            }

            //Displays all the constants.
            Console.WriteLine("\n\nThe Follwing are the constants found: ");
            foreach (var word in constants)
            {
                Console.Write(word.Trim() + ", ");
            }
        }

        //When given a list it used the clean method to remove comments and descriptions. The cleaning is done in two steps. First removing comments then and addition text used to describe the code.  
        public static List<string> clean(List<string> list)
        {
            //This removes the comments. Comments are found but finding the double slash and removing anything after that.
            //Each line from the given file is treated as a single item in the list. This loop looks at each line of the text.
            foreach (var word in list.ToList())
            {
                //Gets the index of the current item and removes any additional spaces . 
                int i = list.IndexOf(word);
                list[i] = word.Trim();

                //Checks to see if a line as a double slash if so, it removes it and everything after that.  
                if (word.Contains("//"))
                {
                    //With every line being treated as a single string it finds the slash and removes it and everything after that. 
                    string cleanedWord = word.Substring(0, word.LastIndexOf("/") - 1);

                    //Updates the item in the list to the new string without the comment.
                    list[i] = cleanedWord;
                }
            }

            //Looks threw each item again this time to remove the description text
            foreach (var word in list.ToList())
            {
                //Tracks the index of the current item.
                int i = list.IndexOf(word);
                if (word == "description")
                {
                    //The description text is placed between the string "description" and "*/
                    //When the string description is found the following removes items in the list until it hit "*/" which is the end of the description text. 
                    i++;
                    for (int j = i; j < list.Count; j++)
                    {
                        while (list[i] != "*/")
                        {
                            list.Remove((list[j]));
                        }
                    }
                }
            }

            //The list with the removed description text is then returned.
            return list;
        }

        //FindkeyWord method take in a list, in this take the list we made with the file we are given, and find the keywords found in the scl_grammar text. 
        //Since most keywords are followed by an identifier this method also finds those. 
        //When a keyword or identifier is found, the string is added to its appropriate list.  
        public static void FindKeyWords(List<string> list)
        {
            //We made the whole string lower case to avoid and case sensitive words.  
            grammer = grammer.ToLower();

            //We loop there the given list. 
            foreach (string line in list)
            {
                //Since each line is a collection of strings, we make a sub list for each line splitting each word as a separate item in the list.  
                List<string> lineList = line.Split(' ').ToList();

                //We loop threw the sub list looking at each word separately. 
                foreach (string word in lineList)
                {
                    //If the word is found in the grammar string it is then added to the keyword list.  
                    if (grammer.Contains(" " + word + " ") && word != "")
                    {
                        keyWords.Add(word);
                        //Console.WriteLine(splitGrammer(word) +": "+word);
                        //Know that we know the word is a keyword found in the grammar string, we need to check if it comes with an identifier. 
                        //Using the splitGrammer boolean when its true it looks the next word after the keyword and adds it to the identifier list. 
                        if (splitGrammer(word) == true)
                        {
                            identifiers.Add(lineList[lineList.IndexOf(word) + 1]);
                        }
                        break;
                    }
                }
            }
        }

        //FindOperator is given a list and checks if any item in the list has an operator.
        //It uses the Operator text file, that list operators to check if any are found in the list.
        public static void FindOperator(List<string> list)
        {
            //We loop there the given list. 
            foreach (var item in list)
            {
                //Since each line is a collection of strings, we make a sub list for each line splitting each word as a separate item in the list. 
                List<string> lineList = item.Split(' ').ToList();

                //We loop threw the sub list looking at each word separately. 
                foreach (string word in lineList)
                {
                    //If the word is found in the grammar string it is then added to the keyword list. 
                    if (op.Contains(word) && word != "")
                    {
                        operators.Add(word);
                    }
                }
            }
        }

        //splitGrammer is a boolean that is used to check if the keyword found has a variable name following it.
        //The grammar text is used to find if this is true or not. 
        public static bool splitGrammer(string key)
        {
            //We make a sub list using the grammar string to make it easy to know if the keyword has an identifier.
            List<string> grammarLines = grammer.Split('\n').ToList();

            //We loop threw the grammar sub list looking at each word separately. 
            foreach (string line in grammarLines)
            {
                //We check if the line we are looking at has the keyword.
                if (line.Contains(key))
                {
                    //If the keyword is found in the line we check if the word after it is either and identifier, name_ref, or is define. These three can be seen as a variable name.
                    if (line.Contains(key + " identifier") || line.Contains(key + " name_ref") || key == "define")
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}