/* FILE:        Text_Parser.cs
 * AUTHORS:     Megan Strait and Consuelo Valdes
 * MODIFIED:    18 Feb 2010
 * 
 * DESCRIPTION: Modified version of Text_Parser.cs in the geneBrowse namespace. Parses the specified tab-delimited file into a 
 *              2D array. Getters for each data type are declared.
 *              
 * ISSUES:      Runs into errors when parsing different genomic info (ie rat versus human). The rat text files have only 10 types of
 *              information, whereas the human and mouse text files have 17 types. 
 *              Start and stop indices are also variable across genomes.
 *              File directories are hard-coded. 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace ConsoleApplication1
{
    class Text_Parser
    {
        String directory, file;
        char[] delimiters = { '\t' };
        string[] processed_file;
        string[,] genes;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Text_Parser(String genome, String chr)
        {
            directory = "C:\\Users\\oshaer\\Desktop\\Gnome_Surfer\\data\\" + genome + "\\"; 
            Console.WriteLine("BUGGER: accessing " + directory);
            file = chr + ".txt";

            processed_file = File.ReadAllLines(directory + file);

            set_array();    // must set the array in order to loop through the data file
            set_genes();
        }

        /// <summary>
        /// Sets the size of the 2D chromosomes array (w/out setting the size, the 2D array will not populate).
        /// @ MS & CV
        /// </summary>
        public void set_array()
        {
            int i = processed_file.Length;
            genes = new String[i, 10];
        }

        public int get_length() { return processed_file.Length; }

        /// <summary>
        /// Populates the 2D chromosomes array
        /// @ MS & CV
        /// </summary>
        public void set_genes()
        {
            string[] temp;
            for (int row = 0; row < processed_file.Length - 1; row++)
            {
                temp = processed_file[row + 1].Split(delimiters);   // splits the data into a hor. array of 17 types (10 for the rat) by tab-delimitations
                for (int column = 0; column < 10; column++) { genes[row, column] = temp[column]; }
            }
        }

        public String get_geneID(int index) { return genes[index, 1]; }
        public int get_start(int index) { return System.Convert.ToInt32(genes[index, 3]); } // start and stop are at indices 4 and 5 respectively for the mouse and human genomes
        public int get_stop(int index) { return System.Convert.ToInt32(genes[index, 4]); }
    }
}
