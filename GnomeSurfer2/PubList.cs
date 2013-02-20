using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.Windows.Threading;
using System.IO;
using System.Windows.Controls;
using System.Text.RegularExpressions;


namespace GnomeSurfer2
{
    class PubList
    {
        List<string> titles;
        List<string> links;
        List<string> authors;
        int count;
        string id;

        /// <summary>
        /// 3/14/10
        /// Creates lists of publication titles and links to abstracts by parsing source code from Entrez Gene search page 
        /// and then parsing PubMed search results page.
        /// @ Mikey Lintz & Consuelo Valdes
        /// </summary>
        /// <param name="geneID">Name of gene to search for in Entrez.</param>
        public PubList(string geneID)
        {
            titles = new List<string>();
            links = new List<string>();
            authors = new List<string>();
            id = geneID;

            //Go to main gene page in Entrez and find PubMed ID for gene
            // used to build entire input
            StringBuilder sb = new StringBuilder();

            // used on each read operation
            byte[] buf = new byte[8192];

            // prepare the web page we will be asking for
            HttpWebRequest request = 
                (HttpWebRequest)WebRequest.Create("http://www.ncbi.nlm.nih.gov/sites/entrez?db=gene&cmd=search&term=" + geneID);

            // execute the request
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // we will read data via the response stream
            Stream resStream = response.GetResponseStream();
            string tempString = null;
            count = 0;
            do
            {
                // fill the buffer with data
                count = resStream.Read(buf, 0, buf.Length);

                // make sure we read some data
                if (count != 0)
                {
                    // translate from bytes to ASCII text
                    tempString = Encoding.ASCII.GetString(buf, 0, count);

                    // continue building the string
                    sb.Append(tempString);
                }
            }
            while (count > 0); // any more data to read?
            String htmlText = sb.ToString();
            
            //--------------Parsing Entrez gene results page for gene pubmed reference number in page source------------------------------------------
            String pubMedNumberIDIndicatorTag = "IdsFromResult=";
            Regex numbers = new Regex("[0-9]*"); //Regex to contain only numbers

            //Find PubMedID for gene in htmlText
            int index = htmlText.IndexOf(pubMedNumberIDIndicatorTag) + pubMedNumberIDIndicatorTag.Length;
            String entrezGeneReference = htmlText.Substring(index,10);
            
            //parse it to only contain numbers for PubMedID
            index = entrezGeneReference.IndexOf("\"");//close tag of pubmed numerical reference
            if (index < 1)
            {
                returnNoPubs();
                return;
            }
            else
                entrezGeneReference = entrezGeneReference.Substring(0, index);

            //Create uri for pubMed search results page from PubMedID
            Uri toPubs = new Uri("http://www.ncbi.nlm.nih.gov/sites/entrez?Db=pubmed&DbFrom=gene&Cmd=Link&LinkName=gene_pubmed&LinkReadableName=PubMed&IdsFromResult=" + entrezGeneReference + "&ordinalpos=NaN&itool=EntrezSystem2.PEntrez.Gene.Gene_ResultsPanel.Gene_RVFullReport.GeneRightColMenuLinksP'");
            
            // prepare to ask for PubMed links and parse the page to gather links & titles to publications
            request = (HttpWebRequest)WebRequest.Create(toPubs);

            // execute the request
            response = (HttpWebResponse)request.GetResponse();

            // we will read data via the response stream
            resStream = response.GetResponseStream();
            tempString = null;
            count = 0;
            do
            {
                // fill the buffer with data
                count = resStream.Read(buf, 0, buf.Length);

                // make sure we read some data
                if (count != 0)
                {
                    // translate from bytes to ASCII text
                    tempString = Encoding.ASCII.GetString(buf, 0, count);

                    // continue building the string
                    sb.Append(tempString);
                }
            }
            while (count > 0); // any more data to read?
            htmlText = sb.ToString();

            //-------------Search PubMed for results and parse code for publication links and authors---------------------

            //Link template for PubMed abstracts
            String link = "http://www.ncbi.nlm.nih.gov/pubmed/";
            //Vars necessary to parse the source code
            Char[] delimiters = {','};
            String htmlRefToArticles = "href=\"/pubmed/";
            String htmlRefToArticleTitles = "\" ref=\"ordinalpos=";
            String noisyArticleReference = "&amp;log$=free";
            String authorsListStart = "\"rprtbody\">";

            //Find the beginning of the search results in the HTML
            index = htmlText.IndexOf("rprt");
            htmlText = htmlText.Substring(index);
            //Try to parse page source. 30 because we want at most 15 articles and for each article there is an additional loop
            //to account for noise. -CV, 3/22/10
            for (int i = 1; i < 20; i++)
            {
                index = htmlText.IndexOf(htmlRefToArticles);
                htmlText = htmlText.Substring(index + htmlRefToArticles.Length);
                index = htmlText.IndexOf(htmlRefToArticleTitles);

                tempString = htmlText.Substring(0, index + htmlRefToArticleTitles.Length + 20);
                if (!tempString.Contains(noisyArticleReference))
                {
                    //article reference found
                    index = htmlText.IndexOf("\"");
                    tempString = htmlText.Substring(0, index);//gets number reference to article for templating
                    links.Add(link + tempString);

                    index = htmlText.IndexOf(htmlRefToArticleTitles);
                    //i (number in search results and it can be more than one digit), 2 because there are 2 chars after 1
                    htmlText = htmlText.Substring(index + htmlRefToArticleTitles.Length + 3);
                    index = htmlText.IndexOf("</a>");
                    tempString = htmlText.Substring(0, index);
                    if (tempString.StartsWith(">"))
                    {
                        tempString = tempString.Substring(1);
                        titles.Add(tempString);
                    }
                    titles.Add(tempString);

                    if (authors.Count() < 15)//After you do the 15th, you get nothing but noise. -CV, 3/22/10
                    {
                        index = htmlText.IndexOf(authorsListStart);
                        htmlText = htmlText.Substring(index + authorsListStart.Length);
                        index = htmlText.IndexOf(","); //"</p>");
                        authors.Add(htmlText.Substring(0, index) + ", et. al.\n\n");
                    }
                }
            }
          
            resStream.Close();
            response.Close();

        }

        /// <summary>
        /// If no publication titles can be obtained, sets the first title to "Publications not available".
        /// @ Sarah Elfenbein
        /// </summary>
        private void returnNoPubs()
        {
            if (count < 1)
            {
                titles.Add("Publications not available");
                links.Add("-1");             //this -1 is used in PubAbstract to determine if there's a valid link
                count++;
            }
        }

        public List<string> getTitles()
        {
            return titles;
        }

        public List<string> getLinks()
        {
            return links;
        }

        public List<string> getAuthors()
        {
            return authors;
        }

        public string getGeneID()
        {
            return id;
        }

        /*Temporary method: The following method was used to initialize (and then changed to append to) the
         * file called PublicationsReserve.txt. This method was called in the GeneItem Class. You do not need this 
         * method to run PubList, but you can use these code blocks if you want to create an updated publications list.
         */ 
        
        //public void pubsToFile(string FILE_NAME)
        //{
        //string FILE_NAME = "PublicationsReserve.txt";

        /* Use this code if you're starting a new file that doesn't alread exist: I used this first
         * and then replaced this block with the one below.
         
        if (File.Exists(FILE_NAME))
        {
            Console.WriteLine("{0} already exists.", FILE_NAME);
            return;
        }
        using (StreamWriter sw = File.CreateText(FILE_NAME))
        {
            int printed = 15;
            if (count < 15)
                printed = count;

            for (int i = 0; i < printed; i++)
            {
                sw.WriteLine(titles[i]);
            }
                
            sw.Close();
        }
        */

        /* Use this code if you're appending to an already existing file. I used this after I had already
         * created PublicationsReserve.txt and put it into Gene_Browse.
         
            using (StreamWriter sw = File.AppendText(FILE_NAME))
            {
                sw.WriteLine(id);

                int printed = 15;
                if (count < 15)
                    printed = count;

                for (int i = 0; i < printed; i++)
                {
                    sw.WriteLine(titles[i]);
                }

                sw.WriteLine("This line signifies end of publication list for this gene");
                sw.WriteLine("");
                sw.Close();
            }
        }
        */

    }
}

//