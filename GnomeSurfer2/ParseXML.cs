using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;

namespace GnomeSurfer2
{
    class ParseXML
    {
        
       /* private Chromosome chr;
        private String comp;

        /// <summary>
        ///  Creates a new ParseXML object.
        /// </summary>
        public ParseXML(String comp)
        {
            this.comp = comp;
            populateArrays();
        }

        /// <summary>
        ///  This method returns the Chromosome created from the XML file.
        /// </summary>
        /// <returns>The Chromsome being worked on.</returns>
        public Chromosome getChromosome()
        {
            return chr;
        }

             
        
        /// <summary>
        ///  This method parses the XML file.
        /// </summary>
        private void populateArrays()
        {
            string sm_chromosome = "17";
            string sUrl = comp + sm_chromosome + ".1.1000000.xml";
            Console.WriteLine(sUrl);

            //Parse the XML file 
            StringBuilder oBuilder = new StringBuilder();
            XmlTextReader oXmlReader = new XmlTextReader(sUrl);

            Console.WriteLine("reading xmlText");

            chr = new Chromosome(sm_chromosome);
            string currentGene = "";
            bool readNewGene = false;
            Gene newGene = new Gene(null);
            int startVariable = -1;
            int endVariable = -1;
            string orientation = "none";

            try
            {
                //While loop to read through the XML file and read the chromosome into an array
                while (oXmlReader.Read())
                { 
                    //An exon starts at the FEATURE tag in the xml file
                    //When the tag is an opening FEATURE tag, then it is the start of a new exon and a new gene.
                    //If this exon is the first exon of a new gene, then make this new gene as the currentGene and
                    //add the previous gene to the list of genes in the chromosome.
                    //Otherwise, just continue to reading the start and end of this exon.
                    if (oXmlReader.Name.Equals("FEATURE") & !oXmlReader.NodeType.Equals(XmlNodeType.EndElement))
                    {
                        string attribName = oXmlReader.GetAttribute(1); //gets the name of the gene of this exon
                        Console.WriteLine(attribName);
                        if (!currentGene.Equals(attribName))
                        {
                            currentGene = attribName;
                            newGene = new Gene(attribName);         //create a new gene
                            //isoform = false;                        //we don't know if it is an isoform of an existing gene
                            readNewGene = true;                     //this is a new gene
                        }
                    }

                    if (oXmlReader.NodeType == XmlNodeType.Element)
                    {
                        // The node is an element.
                        // Console.Write("<" + oXmlReader.Name + "  " + oXmlReader.HasAttributes);

                        //If the tag is an openning START tag, then remember the start value.
                        if (oXmlReader.Name.Equals("START"))
                        {
                            oXmlReader.Read();
                            startVariable = Convert.ToInt32(oXmlReader.Value);
                            Console.WriteLine(startVariable);
                        }

                        //If the tag is an openning END tag, then remember the end value.
                        if (oXmlReader.Name.Equals("END"))
                        {
                            oXmlReader.Read();
                            endVariable = Convert.ToInt32(oXmlReader.Value);
                            Console.WriteLine(endVariable);
                        }

                        //If the tag is an openning ORIENTATION tag, then remember the orientation value.
                        if (oXmlReader.Name.Equals("ORIENTATION"))
                        {
                            oXmlReader.Read();
                            orientation = oXmlReader.Value;
                            Console.WriteLine(orientation);
                        }

                        //If it is currently reading a new gene, then set all the gene values.
                        //Also add the gene to the orientation list.
                        if (readNewGene)
                        {
                            if (startVariable > -1 & endVariable > -1 & orientation != "none")
                            {
                                newGene.setStart(startVariable);
                                newGene.setOrientation(orientation);

                                if (!chr.isIsoform(newGene))
                                {
                                    if (orientation == "-")
                                        chr.addGenetoLeftOrientation(newGene);
                                    else
                                        chr.addGenetoRightOrientation(newGene);
                                    chr.addGene(newGene);
                                }
                                readNewGene = false;
                            }
                        }

                        //Add the exon to the last gene in the chromosome list.
                        //Update the end value for the gene based on the end value of the exon.
                        if (startVariable > -1 & endVariable > -1 & orientation != "none")
                        {
                            Exon newExon = new Exon();
                            // REMEMBER TO ADD OTHER CHARACTERISTICS OF THE EXON (direction...)
                            newExon.setStart(startVariable);
                            newExon.setStop(endVariable);
                            newExon.setOrientation(orientation);
                            chr.getGeneAt(chr.getGeneCount() - 1).addExon(newExon);
                            chr.getGeneAt(chr.getGeneCount() - 1).setStop(endVariable);
                            if (chr.getGeneAt(chr.getGeneCount() - 1).getOrientation() == "-")
                            {
                                chr.getLeftOrientedGenes()[chr.getLeftOrientedGenes().Count - 1].setStop(endVariable);
                                //chr.getLeftOrientedGenes()[chr.getLeftOrientedGenes().Count - 1].addExon(newExon);
                            }
                            else
                            {
                                chr.getRightOrientedGenes()[chr.getRightOrientedGenes().Count - 1].setStop(endVariable);
                                //chr.getRightOrientedGenes()[chr.getRightOrientedGenes().Count - 1].addExon(newExon);
                            }
                            Console.WriteLine("end of adding exon");
                            startVariable = -1;
                            endVariable = -1;
                            orientation = "none";
                        }
                    }
                } // end while loop
            }
            catch (Exception ex)
            {
                Console.Write("Exception : " + ex.Message);
            }

            Console.WriteLine("################################################################");
            oXmlReader.Close();
            Console.WriteLine("Parsing complete");
            //oXmlWriter.Close();
        }*/
    }
}

