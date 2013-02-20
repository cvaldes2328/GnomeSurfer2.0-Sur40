/// <Summary>
/// FILE:                   TextParser.cs
/// AUTHORS:                C. Valdes, M. Strait
/// DESCRIPTION:            
/// 
/// MODIFICATION HISTORY:   20-Jan-10   
/// </Summary>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GnomeSurfer2
{
    class TextParser
    {
		private Chromosome chromosome;
        private static String executablePath = System.AppDomain.CurrentDomain.BaseDirectory;
        private String comp = executablePath.Substring(0, executablePath.Length - 10);
        private String genome;

        String directory, filename;
        char[] delimiters = {'\t'};
        String[] array;
        string[,] genes;
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        public TextParser(String comp, String org, int sm_chromosome)
        {
            this.genome = org.ToLower();
            directory = SurfaceWindow1.dataDir + "Genomes\\" + genome + "\\";
            filename = "chr" + sm_chromosome + ".txt";
            array = File.ReadAllLines(directory + filename);

            int i = array.Length;
            String[,] fileData = new String[i, 16];
            this.chromosome = constructChromosome(sm_chromosome, array);
        }

        public Chromosome getChromosome()
        {
            return chromosome;
        }

        public int get_length()
        {
            return array.Length;
        }

        /// <summary>
        /// Populates the 2D chromosomes array
        /// @ MS & CV
        /// </summary>
        private Chromosome constructChromosome(int sm_chromosome, String[] processedFile)
        {
            char[] commaDelimiter = { ',' };
            char[] tabDelimiters = { '\t' };
            string[] row;
            String id;
            String name;
            List<Exon> exons;
            List<Gene> genes = new List<Gene>();
            String orientation;
            String[] exonStarts;
            String[] exonStops;
            int start;
            int stop;
            int exonCount;
            int exonStart;
            int exonStop;
            Exon exon;
            Gene gene;
            DataFileHandler dataFileHandler;

            for (int rowIndex = 0; rowIndex < processedFile.Length - 1; rowIndex++)
            {
                exons = new List<Exon>();
                row = processedFile[rowIndex + 1].Split(tabDelimiters);   // splits the data into a horizontal array of 17 types according to tab-delimitations
                id = row[1];
                orientation = row[3];
                start = Int32.Parse(row[4]);
                stop = Int32.Parse(row[5]);
                name = row[12];
                dataFileHandler = new DataFileHandler(sm_chromosome, id, genome, start);
                exonCount = Int32.Parse(row[8]);
                exonStarts = row[9].Split(commaDelimiter);
                exonStops = row[10].Split(commaDelimiter);
                for (int i = 0; i < exonCount; i++)
                {
                    exonStop = Int32.Parse(exonStops[i]);
                    exonStart = Int32.Parse(exonStarts[i]);
                    exon = new Exon(exonStart, exonStop, sm_chromosome, genome, id, dataFileHandler); 
                    exons.Add(exon);
                }
                gene = new Gene(sm_chromosome, id, name, exons, orientation, start, stop, genome, dataFileHandler);
                genes.Add(gene);
            }
            return new Chromosome(sm_chromosome, genes);
        }

        public String get_genes(int index)
        {
            return genes[index,0];
        }
    }
}
