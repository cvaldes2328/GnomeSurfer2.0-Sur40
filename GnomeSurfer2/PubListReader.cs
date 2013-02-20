using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Net;
using System.IO;

namespace GnomeSurfer2
{
    class PubListReader
    {
        /* This is a class specially designed to read PublicationsReserve.txt and AbstractReserve.txt,
         * which are text files that contains publication titles and abstracts, respectively. This class 
         * is meant to be used instead of PubList when you want to use publications from a file instead 
         * of accessing the internet and parsing html.
         */

        List<string> titles;
        List<string> abstracts;
        int count; //keeps track of how many titles there are
        string id;


        /// <summary>
        /// Has the same functionality as PubList, but reads information from files rather than
        /// accessing the Internet.
        /// @ Sarah Elfenbein
        /// </summary>
        /// <param name="geneID"></param>
        /// <param name="FILE_NAME1">PublicationsReserve.txt</param>
        /// <param name="FILE_NAME2">AbstractReserve.txt</param>
        public PubListReader(string geneID, string FILE_NAME1, string FILE_NAME2)
        {

            Console.WriteLine("PubListReader Start.");

            titles = new List<string>();
            abstracts = new List<string>();
            count = 0;
            id = geneID;


            //Get titles
            try
            {
                using (StreamReader sr = new StreamReader(FILE_NAME1))
                {     
                    // Read lines from the file until the end of the file is reached.
                    String line = sr.ReadLine();
                    while (line != null & (!line.Equals(geneID)))
                    {
                        line = sr.ReadLine();
                    }
                    line = sr.ReadLine();
                    while (line != null & (!line.Equals("This line signifies end of publication list for this gene")))
                    {
                        titles.Add(line);
                        Console.WriteLine(titles[count]);
                        count++;
                        line = sr.ReadLine();
                    }
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }


            //Get abstracts
            try
            {
                using (StreamReader sr = new StreamReader(FILE_NAME2))
                {
                    // Read lines from the file until the end of the file is reached.
                    String line = sr.ReadLine();
                    while (line != null & (!line.Equals(geneID)))
                    {
                        line = sr.ReadLine();
                    }
                    while (line != null & (!line.Equals("This line signifies end of abstract list for this gene")))
                    {
                        string abstr = "";
                        int lineCounter = 1; //this variable counts lines so that we know when we've hit the abstract summay

                        line = sr.ReadLine();
                        while (line != null & (!line.Equals("This line signifies end of abstract list for this gene")) & (!line.Equals(geneID)))
                        {
                            if (lineCounter < 5) 
                            {
                                abstr += line + "\n\n"; //when lineCounter = 5, we're at the abstract summary, so we don't want spaces
                                lineCounter++;
                                line = sr.ReadLine();
                            }
                            else
                            {
                                abstr += line;
                                line = sr.ReadLine();
                            }
                        }

                        abstracts.Add(abstr);
                    }
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        public List<string> getTitles()
        {      
            return titles;
        }

        public List<string> getAbstracts()
        {
            return abstracts;
        }

        public int getCount()
        {
            return count;
        }

        public string getGeneID()
        {
            return id;
        }

    }
}

//