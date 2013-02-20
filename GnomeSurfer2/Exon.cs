/// <Summary>
/// FILE:                    Exon.cs
/// AUTHORS:                 Catherine Grevet, Megan Strait, Sarah Elfenbein
/// MODIFICATION HISTORY:    
///                          08/07/09    Documentation completed.
/// FUNCTION:                Creates an instance of an exon.
/// </Summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GnomeSurfer2
{
    public class Exon
    {
        private int startIndex;
        private int stopIndex;
        private int chromosomeId;
        private String genome;
        private String geneId;
        private DataFileHandler dataHandler;

        /// <summary>
        ///  Creates an Exon with a start, end and orientation.
        /// </summary>
        /// <param name="loc1">The start base of the Exon.</param>
        /// <param name="loc2">The end base of the Exon.</param>
        public Exon(int startIndex, int stopIndex, int chromosomeId, String genome, String geneId, DataFileHandler dataHandler)
        {
            this.startIndex = startIndex;
            this.stopIndex = stopIndex;
            this.chromosomeId = chromosomeId;
            this.genome = genome;
            this.geneId = geneId;
            this.dataHandler = dataHandler;
        }

        /// <summary>
        ///  Gets the start value of the Exon.
        /// </summary>
        /// <returns>The start base of the Exon.</returns>
        public int getStart()
        {
            return startIndex;
        }

        /// <summary>
        ///  Gets the end value of the Exon.
        /// </summary>
        /// <returns>The end base of the Exon.</returns>
        public int getStop()
        {
            return stopIndex;
        }

        public String getDnaSequence()
        {
            /*int count = 0;
            int cTotal = 0;
            for (int i = start; i < stop - 2; i++)
            {
                temp += dnaSeq.Substring(i, 3) + "\t";
                i += 2;
                count++;
                cTotal += 3;
                if (count == 5)
                {
                    temp += "\n\n" + cTotal + "\t"; count = 0;
                }
            }
            return temp;*/
            return dataHandler.getExonDnaSequence(this);
        }

        public String getRnaSequence()
        {
            return dataHandler.getExonRnaSequence(this);
        }

        public String getAaSequence()
        {
            return dataHandler.getExonAaSequence(this);
        }
    }
}


