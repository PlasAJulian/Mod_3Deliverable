using System;
using System.Collections.Generic;
using System.Text;

namespace Mod_3Deliverable
{
    class parser
    {
        public string inText = "Entering <keyword>\nEntering <term>\nEntering <operators>\n";
        public string outText = "Exiting <operators>\nExiting <term>\nExiting <keyword>\n";
        public List<string> scanedDoc;
        public List<string> identifiersList;

        private void start()
        {

        }
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
        private void getNextToken()
        {
            
        }
        public void begin()
        {
            start();
        }
        public void passList(List<string> list, List<string> id)
        {
            scanedDoc = list;
            identifiersList = id;
        }
    }
}
