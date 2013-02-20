using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace ConsoleApplication1
{
    class Program
    {

        StreamReader reader;
        StreamWriter writer;
        String chr, genome, sequence;

        Text_Parser tp;

        static void Main(string[] args)
        {
            Program prog = new Program();
            prog.genome = "human";

            //for (int i = 10; i < 14; i++)
            //{
                prog.chr = "chr13";
                prog.set_data();
                prog.read_genome();
                prog.save_genes();
            //}
        }

        public void set_data()
        {
            tp = new Text_Parser(genome, chr);
        }

        public void read_genome()
        {
            Console.WriteLine("DEBUGGER: file open.");

            reader = new StreamReader("C:\\Users\\oshaer\\Desktop\\GnomeSurfer\\data\\" + genome + "\\" + chr + ".fa");
            StringBuilder builder = new StringBuilder();

            while (!reader.EndOfStream) {
                String line = reader.ReadLine();
                builder.Append(line); 
                Console.WriteLine(line);
            }
            sequence = builder.ToString();
            reader.Close();

            Console.WriteLine("DEBUGGER: file read.");
        }

        public void save_genes()
        {
            Directory.CreateDirectory("C:\\Users\\oshaer\\Desktop\\GnomeSurfer\\data\\" + genome + "\\" + chr);

            for (int i = 0; i < tp.get_length()-1; i++)
            {
                String geneID = tp.get_geneID(i);
                int start = tp.get_start(i) + 5;
                int stop = tp.get_stop(i) + 5;

                String gene_seq = sequence.Substring(start, stop - start);
                writer = File.CreateText("C:\\Users\\oshaer\\Desktop\\GnomeSurfer\\data\\" + genome + "\\" + chr + "\\" + geneID + ".txt");
                writer.Write(gene_seq);
                writer.Close();
            }
        }
    }
}
