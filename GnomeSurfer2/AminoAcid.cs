/// <Summary>
/// FILE:                    AminoAcid.cs
/// AUTHOR:                  Megan Strait
/// MODIFICATION HISTORY:    
///                          07/21/09    Getters and setters, and print methods completed.
///                          08/07/09    Methods updated, and documentation completed.
/// FUNCTION:                Creates an instance of an AminoAcid; takes a Codon as input, and matches it to the appropriate Amino Acid using a simple algorithm.
/// </Summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GnomeSurfer2
{
    class AminoAcid
    {
        String aa;
        String[] refTable;
        String codon;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="s"></param>
        public AminoAcid(String s)
        {
            aa = "";
            refTable = new String[64] { "F", "F", "L", "L", "L", "L", "L", "L", "I", "I", "I", "M", "V", "V", "V", "V", "S", "S", "S", "S", "P", "P", "P", "P", "T", "T", "T", "T", "A", "A", "A", "A", "Y", "Y", "*", "*", "H", "H", "Q", "Q", "N", "N", "K", "K", "D", "D", "E", "E", "C", "C", "*", "W", "R", "R", "R", "R", "S", "S", "R", "R", "G", "G", "G", "G" };
            codon = s;

            setAA();
        }

        /// <summary>
        /// Setter for AminoAcid instance
        /// Sets aa by calculating its location in the refTable from the given codon.
        /// </summary>
        public void setAA()
        {
            int first = 0;
            int second = 0;
            int third = 1;

            for (int i = 0; i < 3; i++)
            {
                String temp = codon.Substring(i, 1);
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
            aa = refTable[index-1];
        }

        /// <summary>
        /// Getter for AminoAcid instance
        /// Returns the AminoAcid as a String.
        /// </summary>
        /// <returns>aa</returns>
        public String getAA() { return aa; }
    }
}
