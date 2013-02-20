/// <Summary>
/// FILE:                   GeneExpressionModelFactory.cs
/// AUTHORS:                M. Strait
/// DESCRIPTION:            
/// 
/// MODIFICATION HISTORY:   24-May-10   Documentation updated.
/// </Summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using System.Xml;
using System.Net;
using System.IO;
using SurfaceApplication1;

namespace GnomeSurfer2
{
    class GeneExpressionModelFactory
    {
        public GeneExpressionModelFactory() 
        { 
        }

        public IMouseGeneExpressionModel GetMouseGeneExpressionModel(Gene gene)
        {
            return new FakeImplementationOfIMouseGeneExpression();
        }


        ///// <summary>
        ///// 
        ///// </summary>
        //private class HumanGeneExpressionModel : IHumanGeneExpressionModel
        //{
        //    private Chromosome chromosome;
        //    private static String executablePath = System.AppDomain.CurrentDomain.BaseDirectory;
        //    private String comp = executablePath.Substring(0, executablePath.Length - 10);
        //    private String genome;

        //    String directory, filename;
        //    char[] delimiters = { '\t' };
        //    String[] array;
        //    string[,] genes;

        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    /// <param name="g"></param>
        //    public HumanGeneExpressionModel(Gene g)
        //    {


        //        //this.genome = org.ToLower();
        //        directory = SurfaceWindow1.dataDir + "Genomes\\" + genome + "\\";
        //        //filename = "chr" + sm_chromosome + ".txt";
        //        array = File.ReadAllLines(directory + filename);

        //        int i = array.Length;
        //        String[,] fileData = new String[i, 16];
        //    }

        //    public Chromosome getChromosome()
        //    {
        //        return chromosome;
        //    }

        //    public int get_length()
        //    {
        //        return array.Length;
        //    }

        //    /// <summary>
        //    /// Populates the 2D chromosomes array
        //    /// @ MS & CV
        //    /// </summary>
        //    private Chromosome constructChromosome(int sm_chromosome, String[] processedFile)
        //    {
        //        char[] commaDelimiter = { ',' };
        //        char[] tabDelimiters = { '\t' };
        //        string[] row;
        //        String id;
        //        String name;
        //        List<Exon> exons;
        //        List<Gene> genes = new List<Gene>();
        //        String orientation;
        //        String[] exonStarts;
        //        String[] exonStops;
        //        int start;
        //        int stop;
        //        int exonCount;
        //        int exonStart;
        //        int exonStop;
        //        Exon exon;
        //        Gene gene;
        //        DataFileHandler dataFileHandler;

        //        for (int rowIndex = 0; rowIndex < processedFile.Length - 1; rowIndex++)
        //        {
        //            exons = new List<Exon>();
        //            row = processedFile[rowIndex + 1].Split(tabDelimiters);   // splits the data into a horizontal array of 17 types according to tab-delimitations
        //            id = row[1];
        //            orientation = row[3];
        //            start = Int32.Parse(row[4]);
        //            stop = Int32.Parse(row[5]);
        //            name = row[12];
        //            dataFileHandler = new DataFileHandler(sm_chromosome, id, genome, start);
        //            exonCount = Int32.Parse(row[8]);
        //            exonStarts = row[9].Split(commaDelimiter);
        //            exonStops = row[10].Split(commaDelimiter);
        //            for (int i = 0; i < exonCount; i++)
        //            {
        //                exonStop = Int32.Parse(exonStops[i]);
        //                exonStart = Int32.Parse(exonStarts[i]);
        //                exon = new Exon(exonStart, exonStop, sm_chromosome, genome, id, dataFileHandler);
        //                exons.Add(exon);
        //            }
        //            gene = new Gene(sm_chromosome, id, name, exons, orientation, start, stop, genome, dataFileHandler);
        //            genes.Add(gene);
        //        }
        //        return new Chromosome(sm_chromosome, genes);
        //    }

        //    public String get_genes(int index)
        //    {
        //        return genes[index, 0];
        //    }
        //}

        /////// <summary>
        /////// 
        /////// </summary>
        ////private class MouseGeneExpressionModel : IMouseGeneExpressionModel
        ////{
        ////    String url;
        ////    XmlTextReader reader;
        ////    Double cerebellum, cerebralCortex, hippocampus, hypothalamus, medulla, olfactoryBulb, striatum, thalamus;

        ////    /// <summary>
        ////    /// 
        ////    /// </summary>
        ////    /// <param name="g"></param>
        ////    public MouseGeneExpressionModel(Gene g)
        ////    {
        ////        url = "http://mouse.brain-map.org/brain/" + g.GetName() + ".xml";
        ////        reader = new XmlTextReader(url);

        ////        while (reader.Read())
        ////        {
        ////            String level = "";

        ////            switch (reader.NodeType)
        ////            {
        ////                case XmlNodeType.Element: // The node is an element.
        ////                    if (reader.Name.Equals("gene-expression"))
        ////                    {
        ////                        reader.MoveToAttribute(1);
        ////                        level = reader.Value;

        ////                        Console.WriteLine(level);
        ////                    }
        ////                    break;
        ////                //case XmlNodeType.Text: //Display the text in each element.
        ////                //    Console.WriteLine(reader.Value);
        ////                //    break;
        ////                case XmlNodeType.EndElement: //Display the end of the element.
        ////                    Console.Write("</" + reader.Name);
        ////                    Console.WriteLine(">");
        ////                    break;
        ////            }
        ////        }
        ////    }

        ////    Double getCerebellum()
        ////    {
        ////        return cerebellum;
        ////    }

        ////    Double getCerebralCortex()
        ////    {
        ////        return cerebralCortex;
        ////    }

        ////    Double getHippocampus()
        ////    {
        ////        return hippocampus;
        ////    }

        ////    Double getHypothalamus()
        ////    {
        ////        return hypothalamus;
        ////    }

        ////    Double getMedulla()
        ////    {
        ////        return medulla;
        ////    }

        ////    Double getOlfactoryBulb()
        ////    {
        ////        return olfactoryBulb;
        ////    }

        ////    Double getStriatum()
        ////    {
        ////        return striatum;
        ////    }

        ////    Double getThalamus()
        ////    {
        ////        return thalamus;
        ////    }
        ////}



        //String url;
        //XmlTextReader reader;
        //Double cerebellum, cerebralCortex, hippocampus, hypothalamus, medulla, olfactoryBulb, striatum, thalamus;

        //public GeneExpressionModelFactory(Gene g)
        //{
        //    if (g.getGenome().Equals("mouse"))
        //    {

        //        url = "http://mouse.brain-map.org/brain/" + g.GetName() + ".xml";
        //        reader = new XmlTextReader(url);

        //        while (reader.Read())
        //        {
        //            String level = "";

        //            switch (reader.NodeType)
        //            {
        //                case XmlNodeType.Element: // The node is an element.
        //                    if (reader.Name.Equals("avglevel"))
        //                    {
        //                        reader.Read();
        //                        level = reader.Value;
        //                        Console.WriteLine("LEVEL: " + level);
        //                    }

        //                        break;
        //                //case XmlNodeType.Text: //Display the text in each element.
        //                //    Console.WriteLine(reader.Value);
        //                //    break;
        //                case XmlNodeType.EndElement: //Display the end of the element.
        //                    break;
        //            }
        //        }
        //    }
        //}

        ////public IHumanGeneExpressionModel getHumanGeneExpressionModel()
        ////{
        ////}

        ////public IMouseGeneExpressionModel getMouseGeneExpressionModel()
        ////{
        ////}
    }
}