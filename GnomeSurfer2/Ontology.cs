/* FILE:        Ontology.cs
 * AUTHORS:     Consuelo Valdes, Sarah Elfenbein
 * MODIFIED:    May 28 2010
 * 
 * DESCRIPTION: Parses the summary section of the search results page from Entrez for a particular gene and stores the pertinent information 
 * into a String Array, to be accessed later, when the ontology is requested for a particular gene.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
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
using System.Text.RegularExpressions;

namespace GnomeSurfer2
{
    class Ontology
    {
        private String[] _gatheredOntologyInformation;
        private String _geneID;
        public Gene _gene;
        private String _genome;
        private int _count = 0;
        
        /// <summary>
        /// Creates a new Ontology item by fetching information from Entrez Gene
        /// @ Consuelo Valdes
        /// </summary>
        /// <param name="geneIDNum">RefSeq ID from XML file</param> 
        public Ontology(String comp, String genome, Gene gene)
        {
            this._geneID = gene.getID();
            _gatheredOntologyInformation = new String[8];
            _gatheredOntologyInformation[0] = "Official Symbol: " + gene.GetName();
            this._gene = gene;
            this._genome = genome;
            GetOntology();
            
        }

        /// <summary>
        /// This method opens a WebRequest to entrez and searches for the gene pulling up the gene info page.
        /// It takes that response stream and turns it into a string using a stringbuilder and then parses the HTML using 
        /// indicators below and chopping the HTML string as it goes along. If the algorithm should not work, check the HTML
        /// source code from the search page to see if the tags changed.
        /// @ Consuelo Valdes
        /// </summary>
        public void GetOntology()
        {
            //Change these strings if HTML tags change for parsing
            String fullNameIndicator = "Full Name</dt>";
            String geneTypeIndicator = "Gene type</dt>";
            String refSeqIndicator = "RefSeq status</dt>";
            String orgIndicator = "Organism</dt>";
            String lineageIndicator = "Lineage</dt>";
            String akaIndicator = "Known As</dt>";
            String summaryIndicator = "Summary</dt>";
            String startLabelIndicator = "<dd>";
            String endLabelIndicator = "</dd>";

            #region WebRequest stuff
            // used on each read operation
            byte[] buf = new byte[8192];

            // prepare the web page we will be asking for
            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create("http://www.ncbi.nlm.nih.gov/sites/entrez?db=gene&cmd=search&term=" + _geneID);

            // execute the request
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StringBuilder stringBuilder = new StringBuilder();

            // we will read data via the response stream
            Stream resStream = response.GetResponseStream();
            string tempString = null;
            _count = 0;
            do
            {
                // fill the buffer with data
                _count = resStream.Read(buf, 0, buf.Length);

                // make sure we read some data
                if (_count != 0)
                {
                    // translate from bytes to ASCII text
                    tempString = Encoding.ASCII.GetString(buf, 0, _count);

                    // continue building the string
                    stringBuilder.Append(tempString);
                }
            }
            while (_count > 0); // any more data to read?
            String htmlText = stringBuilder.ToString();
            int index = htmlText.Length;
            #endregion

            #region HTML Parsing

            //Find Full Name
            index = htmlText.IndexOf(fullNameIndicator);
            htmlText = htmlText.Substring(index + fullNameIndicator.Length);
            index = htmlText.IndexOf(startLabelIndicator);
            htmlText = htmlText.Substring(index + startLabelIndicator.Length);
            index = htmlText.IndexOf("<");
            _gatheredOntologyInformation[1] = "Full Name:\n" + htmlText.Substring(0, index);

            //Find Gene Type
            index = htmlText.IndexOf(geneTypeIndicator);
            if (index < 0)
                return;
            htmlText = htmlText.Substring(index + geneTypeIndicator.Length);
            index = htmlText.IndexOf(startLabelIndicator);
            htmlText = htmlText.Substring(index + startLabelIndicator.Length);
            index = htmlText.IndexOf(endLabelIndicator);
            _gatheredOntologyInformation[2] = "Gene Type:\n" + htmlText.Substring(0, index);

            //Find RefSeq Status
            index = htmlText.IndexOf(refSeqIndicator);
            if (index < 0)
                return;
            htmlText = htmlText.Substring(index + refSeqIndicator.Length);
            index = htmlText.IndexOf(startLabelIndicator);
            htmlText = htmlText.Substring(index + startLabelIndicator.Length);
            index = htmlText.IndexOf(endLabelIndicator);
            _gatheredOntologyInformation[3] = "RefSeq Status:\n" + htmlText.Substring(0, index);

            //Find Organism; there's a link which makes it more complicated than the others
            index = htmlText.IndexOf(orgIndicator);
            if (index < 0)
                return;
            htmlText = htmlText.Substring(index + orgIndicator.Length);
            index = htmlText.IndexOf(">");
            htmlText = htmlText.Substring(index +2);
            index = htmlText.IndexOf(">");
            htmlText = htmlText.Substring(index + 1);
            index = htmlText.IndexOf("</a>");
            _gatheredOntologyInformation[4] = "Organism:\n" + htmlText.Substring(0, index);

            //Find Lineage
            index = htmlText.IndexOf(lineageIndicator);
            if (index < 0)
                return;
            htmlText = htmlText.Substring(index + lineageIndicator.Length);
            index = htmlText.IndexOf(">");
            htmlText = htmlText.Substring(index + 1);
            index = htmlText.IndexOf(endLabelIndicator);
            _gatheredOntologyInformation[5] = "Lineage:\n" + htmlText.Substring(0, index);

            //Find AKA
            index = htmlText.IndexOf(akaIndicator);
            //if (index < 0)
               // return;
            htmlText = htmlText.Substring(index + akaIndicator.Length);
            index = htmlText.IndexOf(startLabelIndicator);
            htmlText = htmlText.Substring(index + startLabelIndicator.Length);
            index = htmlText.IndexOf(endLabelIndicator);
            _gatheredOntologyInformation[6] = "Also Known As:\n" + htmlText.Substring(0, index);


            if (_genome == "mouse") //if it's mouse, you need to go to JAXMICE because ENTREZ doesn't have any info really on mice
                GetMouseSummary();
            else
            {
                //Find Summary
                index = htmlText.IndexOf(summaryIndicator);
                htmlText = htmlText.Substring(index + summaryIndicator.Length);
                index = htmlText.IndexOf(startLabelIndicator);
                htmlText = htmlText.Substring(index + startLabelIndicator.Length);
                index = htmlText.IndexOf(endLabelIndicator);
                _gatheredOntologyInformation[7] = "Summary:\n" + htmlText.Substring(0, index);
            }
            #endregion
         }

        /// <summary>
        /// Goes to JAXMICE and parses the HTML with the tags defined below to get the appearance and description for the 5
        /// cases in the NEURO 200 lab series.
        /// @ Consuelo Valdes
        /// </summary>
        private void GetMouseSummary()
        {
            //mouse specific from JAXMICE
            String appearanceIndicator = "Appearance</b><br />";
            String endAppearanceIndicator = "<p>";
            String descriptionIndicator = "Description</b><br />";
            String endDescriptionIndicator = "<p>";
            String geneReferenceURL = "";

            byte[] buf = new byte[8192];

            if (_gene.GetName().ToUpper() == "RORA")
                geneReferenceURL = "002651";
            else if (_gene.GetName().ToUpper() == "SOD1")
                geneReferenceURL = "004435";
            else if (_gene.GetName().ToUpper() == "GRID2")
                geneReferenceURL = "001046";
            else if (_gene.GetName().ToUpper() == "KCNJ6")
                geneReferenceURL = "000247";
            else
                geneReferenceURL = "005521";

            //Going to JAXMICE
            #region WebRequest stuff

            // prepare the web page we will be asking for
            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create("http://jaxmice.jax.org/strain/" + geneReferenceURL);

            // execute the request
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            StringBuilder stringBuilder = new StringBuilder();

            // we will read data via the response stream
            Stream resStream = response.GetResponseStream();
            String tempString = null;
            _count = 0;
            do
            {
                // fill the buffer with data
                _count = resStream.Read(buf, 0, buf.Length);

                // make sure we read some data
                if (_count != 0)
                {
                    // translate from bytes to ASCII text
                    tempString = Encoding.ASCII.GetString(buf, 0, _count);

                    // continue building the string
                    stringBuilder.Append(tempString);
                }
            }
            while (_count > 0); // any more data to read?
            String htmlText = stringBuilder.ToString();
            int index = htmlText.Length;
            #endregion

            #region HTML Parsing (nothing but the appearance & description comprised of the appearance and description from JAXMICE)

            //Gets appearance and puts it in the information array in 
            index = htmlText.IndexOf(appearanceIndicator);//Sets index to the beginning of the appearance label
            if (index > 0)//check to see if there is anything to be parsed
            {
                htmlText = htmlText.Substring(index + appearanceIndicator.Length);//cuts to beginning of the Appearance
                index = htmlText.IndexOf(endAppearanceIndicator);//sets the end of the appearance section
                tempString = "Appearance:\n" + htmlText.Substring(0, index);
                //Find the first tag in appearance section and set the start and end indexes
                int openTagString = tempString.IndexOf("<");
                int closeTagString = tempString.IndexOf(">");
                String tagString = tempString.Substring(openTagString, (closeTagString - openTagString) + 1);
                
                while (closeTagString > 0)
                {                    
                    if(tagString.Equals("<br>"))//if it's a break, you want a new line after the tag
                    {
                        _gatheredOntologyInformation[7] = _gatheredOntologyInformation[7] + (tempString.Substring(0, tempString.IndexOf("<br>"))) +"\n";
                        tempString = (tempString.Substring(tempString.IndexOf("<")));
                        tempString = (tempString.Substring(tempString.IndexOf(">") + 1));

                        if (tempString.Substring(0, 4) == "<br>")//if there is another one you want a duplicate new line and chop after tag
                        {
                            _gatheredOntologyInformation[7] = _gatheredOntologyInformation[7] + "\n";
                            tempString.Substring(4);
                        }
                    }
                    else
                    {
                        if (tempString.Substring(0, 4) == "<br>")//you may miss the tag, odd and sad, I know
                        {
                            _gatheredOntologyInformation[7] = _gatheredOntologyInformation[7] + "\n";
                            tempString.Substring(4);
                        }
                        //gether text in info array and chop off label
                        _gatheredOntologyInformation[7] = _gatheredOntologyInformation[7] + (tempString.Substring(0, tempString.IndexOf("<")));
                        tempString = (tempString.Substring(tempString.IndexOf("<")));
                        tempString = (tempString.Substring(tempString.IndexOf(">") + 1));
                    }
                    //update our start and end variables
                    openTagString = tempString.IndexOf("<");
                    closeTagString = tempString.IndexOf(">");
                    if(closeTagString > 0)//don't update unless there is a tag
                        tagString = tempString.Substring(openTagString, (closeTagString - openTagString) + 1);
                }
                //formatting
                tempString = _gatheredOntologyInformation[7];
                _gatheredOntologyInformation[7] = tempString.Replace("\n\n\n", "\n\n");
                _gatheredOntologyInformation[7] = _gatheredOntologyInformation[7] + "\n\n";
                
                
            }


            //Get description
            index = htmlText.IndexOf(descriptionIndicator);//Sets index to the beginning of the appearance label
            if (index > 0)
            {
                htmlText = htmlText.Substring(index + descriptionIndicator.Length);//cuts to beginning of the Description
                index = htmlText.IndexOf(endDescriptionIndicator);//sets the end of the Description section
                tempString = "Description:\n" + htmlText.Substring(0, index);//gets Description and puts it in summary section
                
                int openTagSting = tempString.IndexOf("<");
                int closeTagString = tempString.IndexOf(">");
                String tagString = tempString.Substring(openTagSting, (closeTagString - openTagSting) + 1);

                while (tagString.Length > 0)
                {
                    if (openTagSting < 0)
                        break;

                    _gatheredOntologyInformation[7] = _gatheredOntologyInformation[7] + (tempString.Substring(0, tempString.IndexOf("<")));
                    tempString = (tempString.Substring(tempString.IndexOf("<")));
                    tempString = (tempString.Substring(tempString.IndexOf(">") + 1));
                
                    openTagSting = tempString.IndexOf("<");
                    closeTagString = tempString.IndexOf(">");
                    if (openTagSting > 0)
                        tagString = tempString.Substring(openTagSting, (closeTagString - openTagSting) + 1);
                }
                _gatheredOntologyInformation[7] = _gatheredOntologyInformation[7] + "[Provided by JAXMICE]\n\n";//formatting
            }
            #endregion
        }


        /// <summary>
        /// Returns an array of strings, which contain the information of the given gene
        /// @ Consuelo Valdes
        /// </summary>
        public string[] getInfo() 
        { 
            return _gatheredOntologyInformation; 
        }

        /// <summary>
        /// Sets up the interface components for displaying ontology information
        /// @ Consuelo Valdes
        /// </summary>
        /// <param name="contentArea"></param>
        public void displayOntology(Grid contentArea)
        {            
            SurfaceTextBox tb1 = new SurfaceTextBox();

            for (int i = 0; i < _gatheredOntologyInformation.Length; i++)
            {
                tb1.AppendText(_gatheredOntologyInformation[i] + "\n\n");            
            }    
   
            tb1.TextWrapping = TextWrapping.Wrap;
            tb1.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            tb1.IsReadOnly = true;
            
            /* This grid exists so that we can change the color of the text box and also provide               
             *  a transparent border around the text box which will allow the label added in the .cs file to be visible.            
             * For some reason, just changing the background of the text box didn't have an effect,
             *  and changing the scatterview item color will color the entire item.
             * The margin is important because it bounds the text box such that it doesn't overlap the label.
             */
            Grid textGrid = new Grid();
            textGrid.Background = Brushes.White;
            textGrid.Margin = new Thickness(25);
            textGrid.Children.Add(tb1);
            contentArea.Children.Add(textGrid);
        }
    }
}