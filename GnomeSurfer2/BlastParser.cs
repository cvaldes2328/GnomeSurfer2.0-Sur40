/// <Summary>
/// FILE:                   BlastParser.cs
/// AUTHORS:                Megan Strait
/// DESCRIPTION:            Parses the text files produced from running blastall.exe of a query sequence against a 
///                         genome; results start at line 27 of the file and are spaced evenly at 11 lines per result
/// MODIFICATION HISTORY:    
///                         22 Mar 2010 Getters and setters completed.
///                         30 May 2010 Issues with refreshing the query and sbjct variables fixed (also fixes 
///                                     CompareByLength method).
/// </Summary>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GnomeSurfer2
{
    class BlastParser
    {
        private int chromosomeCount;
        String chr, score, expect, identity, strand, query, sbjct, directory, filename;
        String[] array;
        List<BlastResult> results = new List<BlastResult>();
        StringBuilder result = new StringBuilder();
        //StringBuilder builder_sbjct = new StringBuilder();
        
        int resultsStart = 26;  //offset (number of lines) from beginning of the result file (array of lines) to the first result (index 26)
        int line_start = 1;    //first character of line
        int endOffset = 37;   //offset from the end index of the file to the last line of the last result
        int tag_length = 5;    //length of 'score' tag
        int result_offset = 10;  //line offset from start of result to end of result
        int query_offset = 5;   //line offset from start of result to the index of the query sequence
        int sbjct_offset = 7;   //line offset from start of result to the index of the sbjct sequence
        int seq_start = 7;  //char offset from start of the line to the start of the sequence
        int seq_wrap = 5;   //line offset from seq_start to the wrapped line of the sequence
        int score_start = 9;    //char offset from beginning of the line to the start of the score
        int score_length = 4;   //length (in char) of the score
        int expect_start = 34;  //char offset from beginning of the line to the start of the 'expect' value
        int identity_offset = 1;    //line offset from the beginning of the result to the line containing the 'identity' val
        int identity_start = 15;    //char offset from line beginning to the start of the 'identity' value
        int strand_offset = 2;  //line offset from the beginning of the result to the line containing the 'strand' val
        int strand_start = 10;  //char offset from the line beginning to the start of the 'strand' val
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        public BlastParser(String genome)
        {
            directory = SurfaceWindow1.dataDir + "results\\";

            if (genome == "dog") { chromosomeCount = SurfaceWindow1.DOG_CHR_COUNT; }
            else if (genome == "fish") { chromosomeCount = SurfaceWindow1.FISH_CHR_COUNT; }
            else if (genome == "human") { chromosomeCount = SurfaceWindow1.HUMAN_CHR_COUNT; }
            else if (genome == "monkey") { chromosomeCount = SurfaceWindow1.MONKEY_CHR_COUNT; }
            else if (genome == "mouse") { chromosomeCount = SurfaceWindow1.MOUSE_CHR_COUNT; }
            else { chromosomeCount = SurfaceWindow1.RAT_CHR_COUNT; }

            for (int i = 1; i < chromosomeCount; i++)
            {
                chr = "chr" + i;
                filename = genome + "_" + chr + ".txt";

                ReadFile();
            }
        }

        public void ReadFile()
        {
            array = File.ReadAllLines(directory + filename);    // read in the entire file

            for (int i = resultsStart; i < array.Length - endOffset; i++)   // loop through the lines containing result info
            {
                if (array[i].Substring(line_start, tag_length).Equals("Score")) // start of a new result
                {
                    if (array[i + result_offset] == "" || !array[i + result_offset].Substring(line_start, tag_length).Equals("Score")) // query and result sequences are only one line
                    {
                        query = array[i + query_offset].Substring(seq_start);
                        sbjct = array[i + sbjct_offset].Substring(seq_start);
                        FormatResult(i);
                        i += result_offset;
                    }
                    else                                // query and result sequences are more than 60 base pairs
                    {
                        int tempIndex = i;
                        while ((array[i + result_offset] != "") & (i + result_offset <= array.Length - endOffset))
                        {
                            query += array[i + query_offset].Substring(seq_start);
                            sbjct += array[i + sbjct_offset].Substring(seq_start);

                            i += seq_wrap;
                        }
                        FormatResult(tempIndex);
                        i += result_offset;
                    }
                }
            }
        }

        public void FormatResult(int index)
        {
            // chr (chromosome) var is set in for loop the constructor of BlastParser
            score = array[index].Substring(score_start, score_length);
            expect = array[index].Substring(expect_start);
            identity = array[index + identity_offset].Substring(identity_start);
            strand = array[index + strand_offset].Substring(strand_start);

            BlastResult result = new BlastResult(chr, score, expect, identity, strand, query,
                sbjct);
            results.Add(result);
            ClearVars();
        }

        private void ClearVars()
        {
            query = "";
            sbjct = "";
        }

        public List<BlastResult> get_results()
        {
            return results;
        }

    }
}
