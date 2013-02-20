/// <summary>
/// FILE:       File_Splicer
/// AUTHOR:     Megan Strait
/// MODIFIED:   18-Feb-10
/// 
/// DESCRIPTION:    Executes via the console to splice the chromosome .fa files of an organism into individual 
///                 gene files by looping through the number of chromosomes, plus an X and/or Y chromosome if 
///                 it exists. Organism, number of chromosomes, and existance of X and Y chromosome values must
///                 be specified by the user.
///                 
/// ISSUES:         Data directories are hard-coded. Issue with deallocation, as the computer must be shut down
///                 and rebooted before and after execution of the code.
/// 
/// </summary>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class File_Splicer
    {
        StreamReader reader;
        StreamWriter writer;
        StringBuilder builder;
        Text_Parser tp;
        String chr, genome;
        int num_chrs, offset;
        bool x, y;

        static void Main(string[] args)
        {
            File_Splicer splicer = new File_Splicer();
            // *****VARIABLES TO SET*****
            splicer.genome = "rat";               // genome to be spliced
            splicer.num_chrs = 20;                  // number of chromosomes in the org's genome
            splicer.x = true; splicer.y = false;     // true if org has an X and/or Y chromosome
            // **************************
            splicer.offset = 5;                     // each chr file begins with '>chr*', which should be removed

            if (splicer.x)
            {
                splicer.chr = "chrX";
                splicer.run();
            }

            if (splicer.y)
            {
                splicer.chr = "chrY";
                splicer.run();
            }

            for (int i = 1; i <= splicer.num_chrs; i++)    // loops through each numbered chromosome in the genome
            {
                splicer.chr = "chr" + i;
                if (i > 9) { splicer.offset = 6; }          // file begins with '>chr**', so offset is 6
                splicer.run();
            }
        }

        /// <summary>
        /// Runs the splicing processes for each chromosome.
        /// </summary>
        public void run()
        {
            set_data();
            read_genome();
            save_genes();
        }

        /// <summary>
        /// Sets the chromosome info (genes, starts, stops, etc) for use in determining splicing coordinates.
        /// </summary>
        public void set_data()
        {
            tp = new Text_Parser(genome, chr);
        }

        /// <summary>
        /// Reads in a chromosome .fa file (entire dna sequence of a chromosome) line by line, and stores the characters
        /// as one continuous string using a string builder.
        /// @ Megan Strait
        /// </summary>
        public void read_genome()
        {
            Console.WriteLine("\t" + chr + ".fa open...");

            reader = new StreamReader("C:\\Users\\oshaer\\Desktop\\Gnome_Surfer\\data\\" + genome + "\\" + chr + ".fa");    // location of the chromosome sequence file
            builder = new StringBuilder();
            builder.Capacity = 300000000;   // must allot space; each .fa file is between 50 and 200 mb

            while (!reader.EndOfStream) {
                String line = reader.ReadLine();
                builder.Append(line);
            }
            reader.Close();

            Console.WriteLine("\tfile read.");
        }

        /// <summary>
        /// Writes the sequence of each gene in the chromosome to a text file by looping through the
        /// gene array and getting the substring of the StringBuilder at the given start and stop coordinates.
        /// @ Megan Strait
        /// </summary>
        public void save_genes()
        {
            Console.WriteLine("\tSaving genes...");
            Directory.CreateDirectory("C:\\Users\\oshaer\\Desktop\\Gnome_Surfer\\data\\" + genome + "\\" + chr);

            for (int i = 0; i < tp.get_length()-1; i++) // loop through array of genes in the chromosome (created by Text_Parser instance)
            {
                String geneID = tp.get_geneID(i);   // used as name of gene text file
                int start = tp.get_start(i) + offset;
                int stop = tp.get_stop(i) + offset;

                String gene_seq = builder.ToString(start, stop - start);
                writer = File.CreateText("C:\\Users\\oshaer\\Desktop\\Gnome_Surfer\\data\\" + genome + "\\" + chr + "\\" + geneID + ".txt");
                writer.Write(gene_seq);
                writer.Close();
            }
            Console.WriteLine("\tsave complete.");
        }
    }
}
