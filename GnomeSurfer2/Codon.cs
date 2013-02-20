/// <Summary>
/// FILE:                    Codon.cs
/// AUTHOR:                  Megan Strait
/// MODIFICATION HISTORY:    
///                          07/21/09    Getters and setters completed.
///                          08/07/09    Methods updated, and documentation completed.
/// FUNCTION:                Creates an instance of a Codon, taking a 3-letter substring of the DNA sequence as input.
/// </Summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GnomeSurfer2
{
    class Codon
    {
        String codon, dnaSubstring;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="s">input substring of set DNA sequence</param>
        public Codon(String s)
        {
            dnaSubstring = s;
            codon = "";
            setCodon();
        }

        /// <summary>
        /// Setter for Codon instance
        /// Sets the Codon by converting the 3-letter input DNA substring.
        /// </summary>
        public void setCodon()
        {
            for (int i = 0; i < 3; i++)
            {
                String temp = dnaSubstring.Substring(i, 1);
                if (temp.Equals("T")) { codon += "U"; }
                else { codon += temp; }
            }
        }

        /// <summary>
        /// Getter for Codon instance
        /// Returns the Codon as a String.
        /// </summary>
        /// <returns>codon</returns>
        public String getCodon() { return codon; }
    }
}
