///* FILE:        File_Splicer.cs
// * AUTHORS:     Megan Strait
// * MODIFIED:    21 Jan 2010
// * 
// * DESCRIPTION: 
// */

//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace GnomeSurfer2
//{
//    class File_Splicer
//    {
//        StreamReader reader;
//        Text_Parser parser;
//        String directory, file, gene, genome, sequence;
//        int chr_count;

//        public File_Splicer(String comp, String genome)
//        {
//            directory = comp;
//            this.genome = genome;
//            set_count(); read_genome(); splice_chromosomes();
//        }

//        public void set_count()
//        {
//            if (genome == "human") { chr_count = 22; }
//            else { chr_count = 19; }
//        }

//        public void read_genome()
//        {
//            reader = new StreamReader("C:\\Users\\oshaer\\Desktop\\GnomeSurfer\\data\\human\\chr1.txt");
//            while (!reader.EndOfStream) { sequence += reader.ReadLine(); }
//            reader.Close();
//            Console.WriteLine("file read: " + sequence.Substring(1, 10));
//        }

//        public void splice_chromosomes()
//        {
//            //for (int i = 1; i <= chr_count; i++)
//            //{
//            //    parser = new Text_Parser(directory, file, genome);
//            //    save_genes();
//            //}
//            parser = new Text_Parser(directory, "chr1.txt", "human");
//            save_genes();
//        }

//        public void save_genes()
//        {
//            //for (int i = 0; i < parser.get_length(); i++)
//            //{
//                String geneID = parser.get_genes(0);
//                int start = parser.get_start(geneID);
//                int stop = parser.get_stop(geneID);

//                gene = sequence.Substring(start, stop - start);

//                StreamWriter writer = File.CreateText("C:\\Users\\oshaer\\Desktop\\" + geneID);
//                writer.Write(gene);
//                writer.Close();
//            //}
//        }
//    }
//}
