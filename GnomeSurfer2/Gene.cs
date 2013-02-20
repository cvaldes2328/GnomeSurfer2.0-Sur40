using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;

namespace GnomeSurfer2
{
    public class Gene
    {
        //private static String dataDir = "C:\\Users\\hcilab\\Documents\\GnomeSurfer_Data\\";
        private string id;
        private string name;
        private List<Exon> exonList;
        private string orientation;
        private int start;
        private int stop;
        private double chrom;
        private DataFileHandler fileHandler;
        private String genome;

        public const string HumanGenome = "human";
        public const string MouseGenome = "mouse";
        public const string FishGenome = "fish";
        public const string RatGenome = "rat";

        /// <summary>
        ///  Creates a Gene object.
        /// </summary>
        /// <param name="idName">This is the name of the Gene.</param>
        public Gene(double chromosomeId, String id, String name, List<Exon> exons, String orientation, 
            int start, int stop, String genome, DataFileHandler fileHandler)
        {
            this.chrom = chromosomeId;
            this.id = id;
            this.exonList = exons;
            this.orientation = orientation;
            this.start = start;
            this.stop = stop;
            this.name = name;
            this.fileHandler = fileHandler;
            this.genome = genome;
        }

        public static Gene GetGeneById(String geneName, String genome)
        {
            const int idIndex = 1;
            const int chromosomeIndex = 2;
            const int nameIndex = 12;
            const int orientationIndex = 3;
            const int startIndex = 4;
            const int stopIndex = 5;
            const int exonCountIndex = 8;
            const int exonStartsIndex = 9;
            const int exonStopsIndex = 10;

            String directory = SurfaceWindow1.dataDir + "Genomes\\" + genome + "\\";
            String file = "genome.txt";
            String uri = directory + file;
            String[] processedFile = File.ReadAllLines(uri);
            String[] row;
            char[] tabDelimiters = { '\t' };
            char[] commaDelimiter = { ',' };
            for (int rowIndex = 1; rowIndex < processedFile.Length; rowIndex++)
            {
                row = processedFile[rowIndex].Split(tabDelimiters);
                if (row[nameIndex].ToLower().Equals(geneName.ToLower())) {
                    String chromosomeNumber = row[chromosomeIndex].Substring(3);
                    int chromosomeInt = Int32.Parse(chromosomeNumber);
                    String name = row[nameIndex];
                    String id = row[idIndex];
                    String orientation = row[orientationIndex];
                    int start = Int32.Parse(row[startIndex]);
                    int stop = Int32.Parse(row[stopIndex]);
                    int exonCount = Int32.Parse(row[exonCountIndex]);
                    String[] exonStarts = row[exonStartsIndex].Split(commaDelimiter);
                    String[] exonStops = row[exonStopsIndex].Split(commaDelimiter);
                    DataFileHandler dataFileHandler = new DataFileHandler(chromosomeInt, id, genome, start);
                    List<Exon> exons = new List<Exon>();
                    for (int i = 0; i < exonCount; i++)
                    {
                        int exonStop = Int32.Parse(exonStops[i]);
                        int exonStart = Int32.Parse(exonStarts[i]);
                        Exon exon = new Exon(exonStart, exonStop, chromosomeInt, genome, geneName, dataFileHandler);
                        exons.Add(exon);
                    }
                    return new Gene(chromosomeInt, id, name, exons, orientation, start, stop, genome, dataFileHandler);
                }
            }
            throw new GeneNotFoundException();
        }

        public static List<Gene> GetSurroundingGenes(Gene centralGene)
        {
            const int numberOfSurroundingGenes = 20;
            TextParser textParser = new TextParser(SurfaceWindow1.comp, centralGene.getGenome(), centralGene.getChromosomeId());
            Chromosome chromosome = textParser.getChromosome();
            List<Gene> genes = chromosome.getGenesWithoutIsoforms();
            int centralGeneIndex = genes.IndexOf(centralGene);
            int totalNumberOfGenes = genes.Count();

            if (totalNumberOfGenes < numberOfSurroundingGenes)
            {
                return new List<Gene>(genes);
            }
            else if (centralGeneIndex + 1 < numberOfSurroundingGenes / 2)
            {
                List<Gene> surroundingGenes = new List<Gene>();
                for (int i = 0; i < numberOfSurroundingGenes; i++)
                {
                    surroundingGenes.Add(genes.ElementAt(i));
                }
                return surroundingGenes;
            }
            else if (totalNumberOfGenes - centralGeneIndex <= numberOfSurroundingGenes / 2)
            {
                List<Gene> surroundingGenes = new List<Gene>();
                for (int i = totalNumberOfGenes - numberOfSurroundingGenes; i < totalNumberOfGenes; i++)
                {
                    surroundingGenes.Add(genes.ElementAt(i));
                }
                return surroundingGenes;
            }
            else
            {
                List<Gene> surroundingGenes = new List<Gene>();
                for (int i = centralGeneIndex - numberOfSurroundingGenes / 2; i < centralGeneIndex + numberOfSurroundingGenes / 2; i++)
                {
                    surroundingGenes.Add(genes.ElementAt(i));
                }
                return surroundingGenes;
            }
        }

        public string getOrientation()
        {
            return orientation;
        }

        public String getGenome()
        {
            return genome;
        }

        public String getSequence()
        {
            /*(if (sequence == null){
                System.IO.StreamReader file_reader = new System.IO.StreamReader(file);
                StringBuilder sequenceBuilder = new StringBuilder();
                while (!file_reader.EndOfStream)
                {
                    sequenceBuilder.Append(file_reader.ReadLine());
                }
                this.sequence = sequenceBuilder.ToString();
                file_reader.Close();
            }
            return sequence;*/
            return fileHandler.getGeneSequence();
        }

        /// <summary>
        /// Returns the chromosome double. Created for dynamically calling SequenceHandler.
        /// </summary>
        /// <returns>The chromosome from which the gene is from.</returns>
        public int getChromosomeId()
        {
            return (int) chrom;
        }

        /// <summary>
        ///  Gets the ID of the Gene object.
        /// </summary>
        /// <returns>The ID of this Gene.</returns>
        public string getID()
        {
            return id;
        }

        public String GetName()
        {
            return name;
        }

        /// <summary>
        ///  Gets the starting base for the Gene.
        /// </summary>
        /// <returns>The start base of this Gene.</returns>
        public int getStart()
        {
            return start;
        }

        /// <summary>
        ///  Gets the end base for the Gene.
        /// </summary>
        /// <returns>The end base of this Gene.</returns>
        public int getStop()
        {
            return stop;
        }

        /// <summary>
        ///  Gets the list of Exons for the Gene.
        /// </summary>
        /// <returns>The list of Exons of this Gene.</returns>
        public List<Exon> getExons()
        {
            return exonList;
        }

        /// <summary>
        ///  Gets the number of Exons for the Gene.
        /// </summary>
        /// <returns>The number of Exons of this Gene.</returns>
        public int getExonCount()
        {
            return exonList.Count;
        }

        /// <summary>
        ///  Gets the range of bases of the Gene.
        /// </summary>
        /// <returns>The range of the Gene.</returns>
        public int getGeneWidth()
        {
            /*//Console.WriteLine("exon stop " + exonList.Count);
            if (exonList.Count > 0)
            {
                return exonList[exonList.Count - 1].getStop() - exonList[0].getStart();
            }
            else
            {
                return 0;
            }*/
            return stop - start;
        }

        /// <summary>
        ///  Gets the Exon at a specific index in the Gene.
        /// </summary>
        /// <param name="i">The index of the Exon wanted.</param>
        /// <returns>The Exon at index i.</returns>
        public Exon getExonAt(int i)
        {
            return exonList[i];
        }

        public override bool Equals(Object that)
        {
            if (!(that is Gene))
            {
                return false;
            }
            Gene thatGene = (Gene)that;
            return this.getID().ToLower().Equals(thatGene.getID().ToLower());
        }

        public override string ToString()
        {
            return name;
        }
    }
}
