/*
 * 2nd Deliverable
 * Kennesaw State University
 * Department of Computer Science 
 * CS 4308 - Concepts of Programming Languages W02 
 * Spring 2022 
 * 
 * Group members 
 * Plasencia, Julian
 * Puplampu, Jillian
 * Santiago, Jerry
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mod_3Deliverable
{
    class parser
    {
        //similar to the scanner we imported the grammar text file so it can be referenced to find keywords. 
        private static string grammer = File.ReadAllText("../../../files/scl_grammar.txt");
        //instead of having to write the same output into the console two strings were made. 
        public string inText = "Entering <keyword>\nEntering <term>\nEntering <operators>\n";
        public string outText = "Exiting <operators>\nExiting <term>\nExiting <keyword>\n";
        //empty list to store the scanned document and the identifiers. 
        public List<string> scanedDoc;
        public List<string> identifiersList = new List<string>();

        //this method runs the whole parser and is only called bt the method begin.
        private void start()
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
            scanedDoc = fileText.Split('\n').ToList();

            //The list is cleaned to remove comments and descriptions but being sent to our clean method. This line also removes empty lines in the list.  
            scanedDoc = clean(scanedDoc).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

            //returns the next token that is not comment.
            getNextToken();
        }
        //as stated in the project guideline this Boolean is used to make sure we are identifying the identifier once. 
        private Boolean identifierExists(string identifier)
        {
            if (identifiersList.Contains(identifier))
            {
                return true;
            }
            else {
                identifiersList.Add(identifier);
                return false;
            }
        }
        //returns the next token that is not comment. to ignore the comments the same clear method is used that was used in the scanner.  
        private void getNextToken()
        {
            //takes the grammar and makes all the characters lowercase to be make it easier to refences . 
            grammer = grammer.ToLower();
            foreach (string line in scanedDoc)
            {
                //when the document is scanned each line in the file is its own string in the list. 
                //this further splits the line to look at each individual word in that line. 
                List<string> words = line.Split(' ').ToList();
                
                //This loop loops therw each word individualy and checks if any of the words is a keyword.
                foreach (string i in words)
                {
                    //if a key word is found, it is printed and it checks if the next for is an identifier.
                    if (grammer.Contains(" " + i + " ") && i != "")
                    {
                        Console.WriteLine("Next token is: "+ scanedDoc.IndexOf(line)+ " Next Lexeme is "+ i);
                        Console.WriteLine(inText);
                        if (i != words.Last()) {
                            //If the next word is an identifier, it calls the identifierExists method to check if it is and identifier we already defined. 
                            if (splitGrammer(i) == true)
                            {
                                string id = words[words.IndexOf(i) + 1];
                                if (identifierExists(id) == false)
                                {
                                    //if the identifier it new it is printed.
                                    Console.WriteLine("Next token is: " + scanedDoc.IndexOf(line) + " Next Lexeme is " + id);
                                }
                                else
                                {
                                    break;
                                }
                                Console.WriteLine(outText);
                            }
                            else
                            {
                                Console.WriteLine(outText);
                            }
                        }
                    }
                }
            }
        }
        //This method is called the start method to run the code. This is the only method that can be called from the outside.  
        public void begin()
        {
            start();
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
                    if (line.Contains(key + " identifier") || line.Contains(key + " name_ref") || key == "define" || line.Contains(key + " header_file_name") )
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
