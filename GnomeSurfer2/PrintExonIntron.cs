using System;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using SurfaceApplication1;
using System.Windows.Input;

namespace GnomeSurfer2
{
    class PrintExonIntron
    {
        private Canvas container; 
        private int screenSize;
        private int chromosomeHeight;
        private string comp;
        private SequenceElementMenu seqMenu;
        public String name = "exonIntronView";

        /// <summary>
        ///  This class prints out a exon/intro view for a specific gene.
        /// </summary>
        /// <param name="canvas">This is the MainCanvas from the SurfaceWindow1.xaml file</param>
        public PrintExonIntron(Gene g, Canvas container, String comp, RoutedEventHandler entireGeneHandler, 
                                RoutedEventHandler dnaHandler, RoutedEventHandler rnaHandler, 
                                RoutedEventHandler aaHandler)
        {
            this.container = container;
            this.comp = comp;
            chromosomeHeight = 70;
            screenSize = 1024; //allow space for the button to get the entire gene sequence
            double conVal = (g.getStop() - g.getStart()) / ((double) screenSize);
            for (int i = 0; i < g.getExonCount(); i++)
            {
                //A ExonItem is a ScatterView item with an exon associated to it.
                ExonSVI exonSVI = new ExonSVI(g.getExonAt(i));
                exonSVI.MinWidth = 10;

                //Scales the exon's width by the start & stop and divides all by 40... not as good as it should be
                exonSVI.Width = ((g.getExonAt(i).getStop() - g.getExonAt(i).getStart()) / 40);
                exonSVI.Height = chromosomeHeight;
                exonSVI.CanMove = false;
                exonSVI.CanRotate = false;
                exonSVI.CanScale = false;
                exonSVI.Background = System.Windows.Media.Brushes.OliveDrab;
                exonSVI.TouchDown += new EventHandler<TouchEventArgs>(exon_contactHandler);
                Canvas.SetLeft(exonSVI, (g.getExonAt(i).getStart() - g.getStart()) / conVal);
                container.Children.Add(exonSVI);
            }
            seqMenu = new SequenceElementMenu(entireGeneHandler, dnaHandler, rnaHandler, aaHandler);
            container.Children.Add(seqMenu);
        }

        private void exon_contactHandler(object sender, TouchEventArgs e)
        {
            //System.Windows.Point exintContact = new System.Windows.Point(e.Contact.BoundingRect.X, 0);
            //ExonSVI exonSvi = sender as ExonSVI;
            //double left = Canvas.GetLeft(exonSvi);
            //seqMenu.setExonLeft(left);
            //seqMenu.setExon(exonSvi.getExon());
            //TranslateTransform trs = new TranslateTransform(e.Contact.BoundingRect.X - 10, e.Contact.BoundingRect.Y - 298);
            //seqMenu.RenderTransform = trs;
            //if (e.Contact.BoundingRect.X < 50) 
            //{ 
            //    seqMenu.Orientation = 110; 
            //}
            //else if (e.Contact.BoundingRect.X > 900) 
            //{ 
            //    seqMenu.Orientation = 290; 
            //}
            //else 
            //{ 
            //    seqMenu.Orientation = 25; 
            //}
            //ScatterViewItem svi = sender as ScatterViewItem;
            //svi.Background = System.Windows.Media.Brushes.DodgerBlue;
            //svi.Opacity = 0.75;
        }

        
    }
}
