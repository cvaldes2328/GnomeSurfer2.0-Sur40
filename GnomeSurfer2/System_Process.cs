/* FILE:        System_Process.cs
 * AUTHORS:     Megan Strait
 * MODIFIED:    20 Jan 2010
 * 
 * DESCRIPTION:
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GnomeSurfer2
{
    class System_Process
    {
        ProcessStartInfo psi = new ProcessStartInfo("cmd.exe");
        Process p = new Process();
        StreamWriter writer;

        String args, dir, dir_genome, dir_ouput, dir_seq, executable;
        int chr_count;

        public System_Process()
        {
            psi.CreateNoWindow = true;  // suppress the opening of the command window
            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;
            psi.RedirectStandardInput = true;
            psi.RedirectStandardOutput = true;
        }

        public void compare_sequence(String genome)
        { 
            dir =  SurfaceWindow1.blastDir;
            executable = "blastall";

            if (genome == "dog") { chr_count = SurfaceWindow1.DOG_CHR_COUNT; }
            else if ( genome == "fish") {chr_count = SurfaceWindow1.FISH_CHR_COUNT; }
            else if (genome == "human") { chr_count = SurfaceWindow1.HUMAN_CHR_COUNT; }
            else if (genome == "monkey") { chr_count = SurfaceWindow1.MONKEY_CHR_COUNT; }
            else if (genome == "mouse") { chr_count = SurfaceWindow1.MOUSE_CHR_COUNT; }
            else { chr_count = SurfaceWindow1.RAT_CHR_COUNT; }    // rat

            for (int i = 1; i <= chr_count; i++)
            {
                dir_genome = SurfaceWindow1.dataDir + "Genomes\\" + genome + "\\chr" + i + ".fa";
                dir_ouput = SurfaceWindow1.dataDir + "Results\\" + genome + "_chr" + i + ".txt";
                dir_seq = SurfaceWindow1.dataDir + "Results\\CurrSeq.txt";

                args = "-p blastn -d " + dir_genome + " -i " + dir_seq + " -o " + dir_ouput;

                run();
            }
        }

        public void run()
        {
            p = Process.Start(psi);
            p.StandardInput.WriteLine(@"cd " + dir);
            p.StandardInput.WriteLine(@executable + " " + args);
            p.StandardInput.WriteLine(@"EXIT");
            p.WaitForExit();
            p.Close();

        }

        public void save(String subdir, String filename, String s)
        {
            StreamWriter w = File.CreateText(SurfaceWindow1.dataDir + subdir + filename + ".txt");
            w.Write(s);
            w.Close();
        }

        public void open(String subdir, String filename)
        {
            writer = File.CreateText(SurfaceWindow1.dataDir + subdir + filename + ".txt");
        }

        public void write(String s)
        {
            writer.WriteLine(s);
        }

        public void close()
        {
            writer.Close();
        }

        public void print()
        {
            p = Process.Start(psi);
            p.StandardInput.WriteLine(@"notepad /p" + SurfaceWindow1.dataDir + "Sequence.txt");
            p.StandardInput.WriteLine(@"EXIT");
            p.Close();
        }
    }
}
