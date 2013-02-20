/// <Summary>
/// FILE:                   Blast.cs
/// AUTHORS:                Megan Strait
/// DESCRIPTION:            Mimics the NCBI BLAST function; takes a query sequence as input and creates a List of 
///                         BlastResults for any query and any genome of comparison. Called in the BlastInstance 
///                         method of SurfaceWindow1.xaml.cs.
/// MODIFICATION HISTORY:    
///                         21-Jul-09   Getters and setters completed.
///                         7-Aug-09    Methods updated, and documentation completed.
/// </Summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GnomeSurfer2
{
    class Blast
    {
        private BlastParser parser;
        private List<BlastResult> resultsList;
        private int maxLength, numResults;
        String param = "length";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="seq"></param>
        public Blast(String org)
        {
            parser = new BlastParser(org);
            resultsList = parser.get_results();
            numResults = 10;
            maxLength = 1;
            setMaxL(); setLevels(); setQualities(); setScales(); setOrientations();
            SortResults();
        }

        public void SortResults()
        {
            resultsList.Sort(CompareByLength);
        }

        private static int CompareByScore(BlastResult x, BlastResult y)
        {
            int retval = x.get_score().CompareTo(y.get_score());
            return retval;
        }

        private static int CompareByLength(BlastResult x, BlastResult y)
        {
            int retval = y.get_length().CompareTo(x.get_length());
            return retval;
        }


        /// <summary>
        /// Getter for resultsList
        /// Returns the List of BlastResults
        /// </summary>
        /// <returns>resultsList</returns>
        public void setBlasts() 
        {
            for (int i = 0; i < 10; i++)
            {
                resultsList[i] = parser.get_results()[i];
            }
        }

        /// <summary>
        /// Getter for resultsList
        /// Returns the List of BlastResults
        /// </summary>
        /// <returns>resultsList</returns>
        public List<BlastResult> getBlasts() { return resultsList; }

        /// <summary>
        /// Setter for maxL
        /// Finds the longest result out of all the BlastResults (used for setting levels and qualities).
        /// </summary>
        public void setMaxL()
        {
            for (int i = 0; i < resultsList.Count; i++) { if (resultsList.ElementAt(i).get_length() > maxLength) { maxLength = resultsList.ElementAt(i).get_length();
            }
            }
        }
        public int getMaxL() { return maxLength; }

        public void setScales()
        {
            int scale = 0;
            for (int i = 0; i < resultsList.Count; i++)
            {
                int l = resultsList.ElementAt(i).get_length() * 100 / maxLength;
                if (l > 89) { scale = 80; }
                else if (l > 79) { scale = 40; }
                else if (l > 69) { scale = 10; }
                else { scale = 0; }
                resultsList.ElementAt(i).setScale(scale);
            }
        }

        /// <summary>
        /// Setter for level
        /// Sets the level (1, 2, or 3) of the BlastResult ScatterViewItem to display best results closest to user.
        /// </summary>
        public void setLevels()
        {
            int level = 0;
            for (int i = 0; i < resultsList.Count; i++)
            {
                int m = resultsList.ElementAt(i).get_identity();
                int l = resultsList.ElementAt(i).get_length() * 100 / maxLength;

                if (m > 89)
                {
                    if (l > 69) { level = 1; }
                    else if (l > 39) { level = 2; }
                    else { level = 3; }
                }
                else if (m > 79)
                {
                    if (l > 89) { level = 1; }
                    else if (l > 59) { level = 2; }
                    else { level = 3; }
                }
                else { level = 3; }
                resultsList.ElementAt(i).setLevel(level);
            }
        }

        public void setOrientations()
        {
            int orient = 0;
            for (int i = 0; i < resultsList.Count; i++)
            {
                resultsList.ElementAt(i).setOrient(orient);
                orient += 45;
                if (orient > 359) { orient = 0; }
            }
        }

        /// <summary>
        /// Setter for qualities
        /// Sets the qualities (scores) of each BlastResult to give rank to the results.
        /// </summary>
        public void setQualities()
        {
            int quality = 0;
            for (int i = 0; i < resultsList.Count; i++)
            {
                int m = 2 * resultsList.ElementAt(i).get_identity() / 3;
                int l = resultsList.ElementAt(i).get_length() * 30 / maxLength;
                quality = m + l;
                resultsList.ElementAt(i).setQuality(quality);
            }
        }
    }
}