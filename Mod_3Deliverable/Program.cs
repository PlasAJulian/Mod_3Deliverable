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

namespace Mod_3Deliverable
{
    class Program
    {
        //This time instead of everything being placed in the main, now we create an object and run the methods that we need. 
        static void Main(string[] args)
        {
            //Manually type the file location to test without user error.
            //These files are found inside the files folder in the project. 
            //string fileText = File.ReadAllText("../../../files/welcome.scl");
            //string fileText = File.ReadAllText("../../../files/arduino_ex1.scl");  
            //string fileText = File.ReadAllText("../../../files/arrayex1b.scl"); 
            //string fileText = File.ReadAllText("../../../files/bitops1.scl"); 
            //string fileText = File.ReadAllText("../../../files/datablistp.scl"); 
            //string fileText = File.ReadAllText("../../../files/linkedg.scl"); 

            //this is the scanner from the first deliverable.
            //scanner s = new scanner();
            //s.scan();

            //this is the parser from the second deliverable
            parser p = new parser();
            p.begin();
        }

    }
}
