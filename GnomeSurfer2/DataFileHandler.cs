/// <Summary>
/// FILE:                    SequenceHandler.cs
/// AUTHOR:                  Megan Strait
/// MODIFICATION HISTORY:    
///                          07/21/09    Getters and setters, and toString methods completed.
///                          08/07/09    Methods updated, and documentation completed.
///                          
/// FUNCTION:                Handles DNA, RNA, and Amino Acid sequences; formats the DNA sequences by reading in a text file as a string, 
///                          converts the set DNA sequence into an RNA sequence (calls on the Codon class), and converts the set RNA sequence 
///                          into an Amino Acid sequence (calls on the AminoAcid class).
/// </Summary>

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GnomeSurfer2
{
    public class DataFileHandler
    {
        private static String executablePath = System.AppDomain.CurrentDomain.BaseDirectory;
        private String baseDirectory = executablePath.Substring(0, executablePath.Length - 10);
        //private String dataDir = "C:\\Users\\hcilab\\Documents\\GnomeSurfer_Data\\";

        private static String[] aminoAcidReferenceTable = new String[64] { "F", "F", "L", "L", "L", "L", "L", "L", "I", "I", "I", "M", "V", "V", "V", "V", "S", "S", "S", "S", "P", "P", "P", "P", "T", "T", "T", "T", "A", "A", "A", "A", "Y", "Y", "*", "*", "H", "H", "Q", "Q", "N", "N", "K", "K", "D", "D", "E", "E", "C", "C", "*", "W", "R", "R", "R", "R", "S", "S", "R", "R", "G", "G", "G", "G" };
        
        private int chromosomeNumber;
        private String geneId;
        private String genome;
        private int geneStart;

        public DataFileHandler(int chromosomeNumber, String geneId, String genome, int geneStart)
        {
            this.chromosomeNumber = chromosomeNumber;
            this.geneId = geneId;
            this.genome = genome;
            this.geneStart = geneStart;
        }


        private String retrieveFullSequence()
        {
           
            String file = SurfaceWindow1.dataDir + "Genomes\\" + genome + "\\chr" + chromosomeNumber + "\\" + geneId + ".txt";
            System.IO.StreamReader file_reader = new System.IO.StreamReader(file);
            StringBuilder dnaStringBuilder = new StringBuilder();

            while (!file_reader.EndOfStream) { 
                dnaStringBuilder.Append(file_reader.ReadLine()); 
            }
            file_reader.Close();

            return dnaStringBuilder.ToString();
        }

        public String getGeneSequence()
        {
            return retrieveFullSequence();
        }

        public String getExonDnaSequence(Exon exon)
        {
            //start = e.getStart() - g.getStart(); stop = e.getStop() - g.getStart(); 
            int start = exon.getStart() - geneStart;
            int stop = exon.getStop() - geneStart;
            int length = stop - start;
            String geneSequence = getGeneSequence();
            int geneSequenceLength = geneSequence.Length;
            return geneSequence.Substring(start, length).ToLower();
        }

        public String getExonRnaSequence(Exon exon)
        {
            String exonDnaSequence = getExonDnaSequence(exon);
            return exonDnaSequence.Replace('t', 'u');
        }

        public String getExonAaSequence(Exon exon)
        {
            String rnaSequence = getExonRnaSequence(exon);
            int codonLength = 3;
            String codon;
            StringBuilder aaSequenceBuilder = new StringBuilder();
            for (int i = 0; i < rnaSequence.Length - 2; i += codonLength)
            {
                codon = rnaSequence.Substring(i, codonLength);
                aaSequenceBuilder.Append(convertCodonToAminoAcid(codon));
            }
            return aaSequenceBuilder.ToString();
        }

        private String convertCodonToAminoAcid(String codon)
        {
            int first = 0;
            int second = 0;
            int third = 1;
            if (codon.Length != 3)
            {
                throw new Exception("Codon string must have a length of three");
            }
            for (int i = 0; i < 3; i++)
            {
                String temp = codon.Substring(i, 1).ToUpper();
                if (temp.Equals("U"))
                {
                    if (i == 0) { first = 0; }
                    else if (i == 1) { second = 0; }
                    else { third = 1; }
                }
                else if (temp.Equals("C"))
                {
                    if (i == 0) { first = 1; }
                    else if (i == 1) { second = 1; }
                    else { third = 2; }
                }
                else if (temp.Equals("A"))
                {
                    if (i == 0) { first = 2; }
                    else if (i == 1) { second = 2; }
                    else { third = 3; }
                }
                else
                {
                    if (i == 0) { first = 3; }
                    else if (i == 1) { second = 3; }
                    else { third = 4; }
                }
            }
            int index = 4 * first + 16 * second + third;
            return aminoAcidReferenceTable[index - 1];
        }

        /*
        public String dnaToString()
        {
            String temp = "DNA Sequence for " + g.getID() + "\nRange: " + start + " to " + stop + " base pairs\n\n0\t";
            int count = 0; 
            int cTotal = 0;
            for (int i = start; i < stop - 2; i++)
            {
                temp += dnaSeq.Substring(i, 3) + "\t";
                i += 2;
                count++;
                cTotal += 3;
                if (count == 5) { 
                    temp += "\n\n" + cTotal + "\t"; count = 0; 
                }
            }
            return temp;
        }

        /// <summary>
        /// Setter for RNA sequence
        /// Converts the unformatted DNA sequence into a List of codons.
        /// </summary>
        private void setRNA()
        {
            for (int i = 0; i < getDNA().Length - 2; i++)
            {
                String temp = getDNA().Substring(i, 3).ToUpper();
                Codon c = new Codon(temp);
                rnaSeq.Add(c);
                i += 2;
            }
        }

        /// <summary>
        /// Getter for RNA sequence
        /// Returns the formatted RNA sequence as a String.
        /// </summary>
        /// <returns>temp</returns>
        public String rnaToString()
        {
            String temp = "RNA Sequence for " + g.getID() + "\nRange: " + start + " to " + stop + " base pairs\n\n0\t";
            int count = 0;
            int cTotal = 0;
            for (int i = 0; i < rnaSeq.Count; i++)
            {
                temp += rnaSeq.ElementAt(i).getCodon() + "\t";
                count++;
                cTotal += 3;
                if (count == 5) { temp += "\n\n" + cTotal + "\t"; count = 0; }
            }
            return temp;
        }

        /// <summary>
        /// Setter for Amino Acid sequence
        /// Converts the RNA sequence (List of Codons) into a List of AminoAcids.
        /// </summary>
        private void setAA()
        {
            for (int i = 0; i < rnaSeq.Count; i++)
            {
                String temp = rnaSeq.ElementAt(i).getCodon();
                AminoAcid a = new AminoAcid(temp);
                aaSeq.Add(a);
            }
        }

        /// <summary>
        /// Getter for Amino Acid sequence
        /// Returns the formatted Amino Acid sequence as a String.
        /// </summary>
        /// <returns>temp</returns>
        public String aaToString()
        {
            String temp = "Amino Acid Sequence for " + g.getID() + "\nRange: " + start + " to " + stop + " base pairs\n\n0\t";
            int count = 0;
            int cTotal = 0;
            for (int i = 0; i < aaSeq.Count; i++)
            {
                temp += aaSeq.ElementAt(i).getAA() + "\t";
                count++;
                cTotal++;
                if (count == 5) { temp += "\n\n" + cTotal + "\t"; count = 0; }
            }
            return temp;
        }


        */
    }
}
