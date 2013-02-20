/// <Summary>
/// FILE:                    Chromosome.cs
/// AUTHOR:                  Catherine Grevet, Megan Strait, Sarah Elfenbein
/// MODIFICATION HISTORY:    
///                          08/07/09    Documentation updated (CG).
/// FUNCTION:                Creates an instance of a Chromosome.
/// </Summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;

namespace GnomeSurfer2
{
    class Chromosome : IEnumerable<Gene>
    {
        private double sm_chromosome;
        private List<Gene> geneList;

        public Chromosome(double sm_chromosome, List<Gene> geneList)
        {
            this.sm_chromosome = sm_chromosome;
            this.geneList = geneList;
        }

        /// <summary>
        ///  Gets the number of Genes in this Chromosome.
        /// </summary>
        /// <returns>The number of Genes.</returns>
        public int geneCount() { 
            return geneList.Count; 
        }

        /// <summary>
        ///  Gets the range of bases for this Chromosome.
        /// </summary>
        /// <returns>The range of the Chromosme.</returns>
        public int range()
        {
            int geneCount = geneList.Count;
            if (geneCount == 0)
                return 0;
            else
                return geneList[geneCount - 1].getExons()[geneList[geneCount - 1].getExonCount() - 1].getStop();
        }

        /// <summary>
        ///  Gets the range of bases for this Chromosome.
        /// </summary>
        /// <param name="i">The index of the Gene in the Chromosome.</param>
        /// <returns>The Gene at the specified index in the Chromosme.</returns>
        public Gene geneAt(int i) { 
            return geneList[i]; 
        }

        /// <summary>
        /// Returns the gene with the given id.
        /// </summary>
        /// <param name="id">The id of the gene</param>
        /// <returns>The gene, if present. Null otherwise.</returns>
        public Gene getGeneById(String id)
        {
            foreach (Gene gene in geneList)
            {
                if (gene.getID() == id)
                {
                    return gene;
                }
            }
            return null;
        }

        public ReadOnlyCollection<Gene> getGenes()
        {
            return new ReadOnlyCollection<Gene>(geneList);
        }

        public List<Gene> getGenesWithoutIsoforms() {
            List<Gene> genesWithoutIsoforms = new List<Gene>();
            for (int i=0; i<geneList.Count; i++) {
                if (i != 0) {
                    if (geneList.ElementAt(i).GetName() == geneList.ElementAt(i-1).GetName()) { // Isoforms have the same gene name
                        continue;
                    }
                }
                genesWithoutIsoforms.Add(geneList.ElementAt(i));
            }
            return genesWithoutIsoforms;
        }

        public IEnumerator<Gene> GetEnumerator()
        {
            return geneList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}