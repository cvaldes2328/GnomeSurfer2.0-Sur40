using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Data;

namespace GnomeSurfer2
{
    class GeneItem:ScatterViewItem
    {
        private Gene g;
        //private Ontology o;
        private String genome;
        private PubList p;
        private ScatterView workspace;
        private Canvas exonIntCanvas;
        private ScatterViewItem exonInt;
        private PrintExonIntron exonIntronView;
        private ElementMenu menu;
        private String comp;
        private bool isMenu;

        //0.Expression 1.Ontology 2.Pubs (can add more later) - this variable indicates whether the user already
        // a particular window
        private bool[] selectionOpen = {false, false, false};
        //size of abstractOpen corresponds to how many publications are displayed in the surface list box (here 15)
        private bool[] abstractOpen = {false, false, false, false, false, false, false, false, false, false,
                                       false, false, false, false, false};


        public GeneItem(Gene _g, String comp, RoutedEventHandler entireGeneHandler,
                                RoutedEventHandler dnaHandler, RoutedEventHandler rnaHandler,
                                RoutedEventHandler aaHandler, String genome, double chromosomeLabel)
        {
            //Console.WriteLine("\n\n             Starting new gene "+_g.getID()+"\n\n");
            g = _g;
            this.genome = genome;
            workspace = new ScatterView();
            this.comp = comp;
            isMenu = false;
            int chrome = (int) chromosomeLabel;
            exonInt = new ScatterViewItem();
            exonInt.Name = "exonInt";
            exonInt.CanRotate = false;
            exonInt.CanScale = false;
            exonInt.CanMove = false;
            exonInt.ShowsActivationEffects = false;
            exonInt.Orientation = 0;
            exonInt.Center = new Point(512, 320);
            exonInt.Height = 60;
            exonInt.Width = 1024;
            exonIntCanvas = new Canvas();
            System.Windows.Controls.Image background = new System.Windows.Controls.Image();
            background.Source = new BitmapImage(new Uri(comp + "exbg.png", UriKind.RelativeOrAbsolute));
            exonIntCanvas.Children.Add(background);

            exonIntronView = new PrintExonIntron(g, exonIntCanvas, comp, entireGeneHandler, dnaHandler, rnaHandler, aaHandler);
            exonInt.Content = exonIntCanvas;
        }

       public Gene getGene() { return g; }

        //Used to get gene's name to disaply on gene image -CV
        public String geneName()
        {
            return g.GetName();
        }

        public Ontology getOntology() 
        {
           Ontology o = new Ontology(comp, genome, g);
            return o; 
        }

        public PubList getPubList()
        {
            return p;
        }

        public ScatterViewItem exonIntView()
        {
            return exonInt;
        }

        /// <summary>
        /// Returns whether the window for a particular elementmenu selection (expression, ontology, or pubs) is
        /// open for the current gene.
        /// @ Sarah Elfenbein
        /// </summary>
        /// <param name="index">0, 1, or 2 for expres., ontol., or pubs.</param>
        /// <returns></returns>
        public bool getSelectionOpen(int index)
        {
            return selectionOpen[index];
        }

        /// <summary>
        /// Sets the bool indicating whether the window for a particular elementmenu 
        /// selection (expression, ontology, or pubs) is open for this Gene Item.
        /// @ Sarah Elfenbein
        /// </summary>
        /// <param name="index">0, 1, or 2 for expres., ontol., or pubs.</param>
        /// <param name="value">whether a window is open for this Gene Item</param>
        public void setSelectionOpen(int index, bool value)
        {
            selectionOpen[index] = value;
        }

        /// <summary>
        /// Returns whether the abstract for a particular publication has been opened.
        /// @ Sarah Elfenbein
        /// </summary>
        /// <param name="index">pub. index: 0-9</param>
        /// <returns></returns>
        public bool getAbstractOpen(int index)
        {
            return abstractOpen[index];
        }

        /// <summary>
        /// Sets the bool indicating whether the abstract for a particular publication has been opened.
        /// @ Sarah Elfenbein
        /// </summary>
        /// <param name="index">pub index: 0-9</param>
        /// <param name="value">whether an abstract window is open for a particular publication</param>
        public void setAbstractOpen(int index, bool value)
        {
            abstractOpen[index] = value;
        }

        public double LeftXCoordinate
        {
            get
            {
                double centerX = this.Center.X;
                double width = this.Width;
                return centerX - width / 2;
            }
        }

        public double RightXCoordinate
        {
            get
            {
                double centerX = this.Center.X;
                double width = this.Width;
                return centerX + width / 2;
            }
        }

        public double BottomYCoordinate
        {
            get
            {
                double centerY = this.Center.Y;
                double height = this.Height;
                return centerY - height / 2;
            }
        }

        public double TopYCoordinate
        {
            get
            {
                double centerY = this.Center.Y;
                double height = this.Height;
                return centerY + height / 2;
            }
        }

        public String toString()
        {
            return geneName();
        }

    }
}

//
