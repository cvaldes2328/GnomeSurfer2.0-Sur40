/// <Summary>
/// FILE:                   SystemProcess.cs
/// AUTHORS:                Megan Strait
/// DESCRIPTION:            Creates an instance of a Process that launches the Command executable. StartInfo settings
///                         suppress the opening of a UI, and redirect the input and ouput through the SurfaceWindow1
///                         operations. Defined methods allow for execution of the running of the BLAST standalone, 
///                         blastall.exe; saving String objects to local .txt files; and printing local files via
///                         networked printers.
/// 
/// MODIFICATION HISTORY:   21-Jan-10   File creation and method definitions.   @ MS
///                         24-May-10   Documentation updated.  @ MS
/// </Summary>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GnomeSurfer2
{
    class SystemProcess
    {
        ProcessStartInfo properties;
        Process process;
        StreamWriter writer;
        String arguments, chromosomeDir, executable, outputDir, sequenceDir;
        int numChromosomes;

        public SystemProcess()
        {
            this.properties = new ProcessStartInfo("cmd.exe");  // run Command executable
            this.properties.CreateNoWindow = true;              // suppress opening of a UI
            this.properties.UseShellExecute = false;            
            this.properties.RedirectStandardError = true;       // redirect the input, output, and errors through the Surface UI
            this.properties.RedirectStandardInput = true;
            this.properties.RedirectStandardOutput = true;

            this.process = new Process();
        }

        /// <summary>
        /// Blasts each chromosome .fa file of the input genome against the selected sequence (saved to the CurrSeq.txt file) 
        /// using the blastall.exe standalone, and saves the output to the Results directory.
        /// </summary>
        /// <param name="genome"></param>
        public void BlastSeq(String genome)
        { 
            executable = "blastall";    // name of the executable file
            sequenceDir = SurfaceWindow1.dataDir + "Results\\CurrSeq.txt";

            switch (genome)             // sets numChromosomes based on the constants defined for each organism
            {
                case "dog": numChromosomes = SurfaceWindow1.DOG_CHR_COUNT; break;
                case "fish": numChromosomes = SurfaceWindow1.FISH_CHR_COUNT; break;
                case "human": numChromosomes = SurfaceWindow1.HUMAN_CHR_COUNT; break;
                case "monkey": numChromosomes = SurfaceWindow1.MONKEY_CHR_COUNT; break;
                case "mouse": numChromosomes = SurfaceWindow1.MOUSE_CHR_COUNT; break;
                case "rat": numChromosomes = SurfaceWindow1.RAT_CHR_COUNT; break;
            }

            for (int i = 1; i <= numChromosomes; i++)   // executes blast on each of the chromosome .fa files in the genome
            {
                chromosomeDir = SurfaceWindow1.dataDir + "Genomes\\" + genome + "\\chr" + i + ".fa";
                outputDir = SurfaceWindow1.dataDir + "Results\\" + genome + "_chr" + i + ".txt";

                arguments = "-p blastn -d " + chromosomeDir + " -i " + sequenceDir + " -o " + outputDir;    // format for blastall args

                Run();
            }
        }

        /// <summary>
        /// Starts a new Command process, sends it three line inputs, and exits the process at the finish of the blastall execution. 
        /// </summary>
        public void Run()
        {
            process = Process.Start(properties);
            process.StandardInput.WriteLine(@"cd " + SurfaceWindow1.blastDir);  // must move to the directory in which blastall is located
            process.StandardInput.WriteLine(@executable + " " + arguments);
            process.StandardInput.WriteLine(@"EXIT");
            process.WaitForExit();
            process.Close();

        }

        /// <summary>
        /// Writes a single string to the specified file.
        /// Note: the writer doesn't recognize newlines, so the WriteLine must be used instead (along with the Open and Close methods
        /// to open and close the file).
        /// </summary>
        /// <param name="subdir"></param>
        /// <param name="filename"></param>
        /// <param name="s"></param>
        public void Save(String subdir, String filename, String s)
        {
            writer = File.CreateText(SurfaceWindow1.dataDir + subdir + filename + ".txt");
            writer.Write(s);
            writer.Close();
        }

        /// <summary>
        /// Opens the specified file, or creates the file if it does not yet exist.
        /// </summary>
        /// <param name="subdir"></param>
        /// <param name="filename"></param>
        public void Open(String subdir, String filename)
        {
            writer = File.CreateText(SurfaceWindow1.dataDir + subdir + filename + ".txt");
        }

        /// <summary>
        /// Writes a single line to the open file.
        /// </summary>
        /// <param name="s"></param>
        public void WriteLine(String s)
        {
            writer.WriteLine(s);
        }

        /// <summary>
        /// Closes the file being written.
        /// </summary>
        public void Close()
        {
            writer.Close();
        }

        /// <summary>
        /// Prints the currently docked sequence.
        /// Note: further implementation should allow for the printing of any specified text file.
        /// </summary>
        public void Print()
        {
            process = Process.Start(properties);
            process.StandardInput.WriteLine(@"notepad /p" + SurfaceWindow1.dataDir + "Sequence.txt");
            process.StandardInput.WriteLine(@"EXIT");
            process.Close();
        }
    }
}
