using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GnomeSurfer2
{
    class BlastResult
    {
        String chr, strand, query, sbjct;
        Decimal score, expect;
        int identity, length, level, or, quality, scale;
        char[] delimiters = { ' ' };
        static Random generator = new Random();

        public BlastResult(String chromosome, String score, String expect, String identity,
            String strand, String query, String sbjct)
        {
            set_chr(chromosome);
            set_score(score);
            set_expect(expect);
            set_identity(identity);
            set_strand(strand);
            set_query(query);
            set_sbjct(sbjct);
        }

        public int get_length()
        {
            int start = 0;
            int stop = 0;

            String[] lineArray = sbjct.Split(delimiters);

            start = Convert.ToInt32(lineArray[0]);
            stop = Convert.ToInt32(lineArray[lineArray.Length-1]);

            length = stop - start;

            if (length < 0)//the starts and stops go in the direction of the gene so if it's negative, you need to make pos
                length = start - stop;

            return length;
        }

        /// <summary>
        /// Setter for level
        /// Sets the level of y-position of the BlastResult instance.
        /// </summary>
        /// <param name="l"></param>
        public void setLevel(int l) { level = l; }
        public int getLevel() { return level; }

        /// <summary>
        /// Setter for orientation
        /// Sets the scale (change in width) of the BlastResult for the ScatterViewItem created in the BlastInstance method of SurfaceWindow1.xaml.cs.
        /// </summary>
        /// <param name="s"></param>
        public void setOrient(int o) { or = o; }
        public int getOrient() { return or; }

        /// <summary>
        /// Setter for quality
        /// Sets the quality (score) of the BlastResult instance.
        /// </summary>
        /// <param name="q"></param>
        public void setQuality(int q) { quality = q; }
        public int getQuality() { return quality; }

        public void setScale(int s) { scale = s; }
        public int getScale() { return scale; }

        /// <summary>
        /// ToString method
        /// Formats the BlastResult instance as a String.
        /// </summary>
        /// <returns></returns>
        public String toString()
        {
            return ("Chromosome " + chr + "\nLength: " + length + " base pairs\nAlignment: " + identity + " percent");
        }



        public void set_chr(String s) { chr = s; }
        public String get_chr() { return chr; }

        public void set_score(String s) { score = Convert.ToDecimal(s); }
        public Decimal get_score() { return score; }

        public void set_expect(String s)
        {
            if (s.Contains("e"))
            { expect = 0; }
            else { expect = Convert.ToDecimal(s); }
        }
        public Decimal get_expect() { return expect; }

        public void set_identity(String s) 
        {
            int i = 0;

            while (i < s.Length)
            {
                if (s.Substring(i, 1).Equals("("))
                {
                    if (!s.Substring(i+3, 1).Equals("%"))
                    { identity = Convert.ToInt16(s.Substring(i+1,3)); }
                    else { identity = Convert.ToInt16(s.Substring(i+1,2)); }
                    break;
                }
                i++;
            }
        }
        public int get_identity() { return identity; }

        public void set_strand(String s) 
        {
            char[] delims = { '/' };

            String[] string_array = s.Split(delims);
            strand = string_array[string_array.Length - 1];

            //for (int i = 0; i < s.Length; i++)
            //{
            //    if (s.Substring(i, 1).Equals("/")) { strand = s.Substring(i + 2); }
            //}
        }
        public String get_strand() { return strand; }

        public void set_query(String s) { query = s; }
        public String get_query() { return query; }

        public void set_sbjct(String s) { sbjct = s; }
        public String get_sbjct() { return sbjct; }
    }
}
