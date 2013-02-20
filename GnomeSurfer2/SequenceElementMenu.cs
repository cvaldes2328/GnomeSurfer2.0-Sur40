using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media;
using System.Windows;
using GnomeSurfer2;

namespace GnomeSurfer2
{
    class SequenceElementMenu : ElementMenu
    {
        private double exonLeft;
        private Exon exon;

        public SequenceElementMenu(RoutedEventHandler entireGeneHandler, RoutedEventHandler dnaHandler, 
                                    RoutedEventHandler rnaHandler, RoutedEventHandler aaHandler)
            : base()
        {
            this.Width = 30;
            this.Height = 30;
            this.ActivationMode = ElementMenuActivationMode.AlwaysActive;
            this.Orientation = 180;
            TranslateTransform trs = new TranslateTransform(10, 10);
            this.RenderTransform = trs;

            ElementMenuItem dna = new ElementMenuItem();
            dna.Height = 30; dna.Width = 30;
            dna.Header = "DNA";
            dna.Background = Brushes.OliveDrab;
            dna.Opacity = 0.5;
            dna.Click += dnaHandler;
            ElementMenuItem rna = new ElementMenuItem();
            rna.Header = "RNA";
            rna.Width = 30; rna.Height = 30;
            rna.Background = Brushes.OliveDrab;
            rna.Opacity = 0.5;
            rna.Click += rnaHandler;
            ElementMenuItem aa = new ElementMenuItem();
            aa.Background = Brushes.OliveDrab;
            aa.Opacity = 0.5;
            aa.Height = 30; aa.Width = 30;
            aa.Header = "AA";
            aa.Click += aaHandler;

            this.Items.Add(dna);
            this.Items.Add(rna);
            //this.Items.Add(entSeq);
            this.Items.Add(aa);
        }

        public void setExonLeft(double left)
        {
            this.exonLeft = left;
        }

        public double getExonLeft()
        {
            return exonLeft;
        }

        public void setExon(Exon exon)
        {
            this.exon = exon;
        }

        public Exon getExon()
        {
            return exon;
        }
    }
}
