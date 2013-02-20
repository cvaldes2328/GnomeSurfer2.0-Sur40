using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Surface.Presentation.Controls;
using System.Windows;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls.Primitives;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Input;

namespace GnomeSurfer2
{
    public delegate void InitializedHandler(object sender, EventArgs e);

    class ChromosomeBar
    {
        public event InitializedHandler Initialized;

        public static readonly double TriangleLength = 30; 
        
        private const double maxWidth = 1000; // max width of a gene...
        private const double minWidth = 100; // min width of a gene...
        private const double idealWidth = 316; // ideal rendered width of a gene in pixels
        private const double minDistanceBetweenGeneItems = 10;
        private const double basepairIntervalRatio = 0.05; // this "feels right"

        private SurfaceScrollViewer chromosomeViewer;
        private Canvas geneGrid;
        private StackPanel legend;
        private ScatterView geneScatterView;
        private double chromosomeHeight;

        private RoutedEventHandler entireGeneHandler;
        private RoutedEventHandler dnaHandler;
        private RoutedEventHandler rnaHandler;
        private RoutedEventHandler aaHandler;

        private List<GeneItem> upperGenes;
        private List<GeneItem> lowerGenes;

        private double margin = minWidth;
        private double scaleValue = 0;

        private GeneItem mainGeneItem;
        private String mainGeneName;



        //private TextBox box;

        public int LeftmostBasePairIndex
        {
            get
            {
                if (scaleValue == 0)
                {
                    return 0;
                }
                return Math.Min(lowerGenes.First().getGene().getStart(), upperGenes.First().getGene().getStart())
                    - (int) (margin / scaleValue);
            }
        }

        public int RightmostBasePairIndex
        {
            get
            {
                if (scaleValue == 0)
                {
                    return 0;
                }
                return LeftmostBasePairIndex + (int) (geneGrid.Width / scaleValue);
            }
        }

        private int Interval
        {
            get
            {
                return (int) ((RightmostBasePairIndex - LeftmostBasePairIndex) * basepairIntervalRatio);
            }
        }

        public ChromosomeBar(SurfaceScrollViewer chromosomeViewer, Canvas geneGrid, StackPanel legend, ScatterView topGene, 
            RoutedEventHandler entireGeneHandler, RoutedEventHandler dnaHandler, RoutedEventHandler rnaHandler, RoutedEventHandler aaHandler)
        {
            this.chromosomeViewer = chromosomeViewer;
            this.geneGrid = geneGrid;
            this.legend = legend;
            this.geneScatterView = topGene;
            this.entireGeneHandler = entireGeneHandler;
            this.dnaHandler = dnaHandler;
            this.rnaHandler = rnaHandler;
            this.aaHandler = aaHandler;
            this.upperGenes = new List<GeneItem>();
            this.lowerGenes = new List<GeneItem>();
        }

        private void initialize()
        {
            Canvas.SetTop(chromosomeViewer, 50);
            chromosomeViewer.Height = 200;
            geneGrid.Height = chromosomeViewer.Height - 20;
            legend.Height = chromosomeViewer.Height / 4;
            Canvas.SetTop(legend, ((chromosomeViewer.Height - 20) / 2) - legend.Height / 2);
            chromosomeHeight = (chromosomeViewer.Height - legend.Height - 30) / 2;
            chromosomeViewer.Visibility = Visibility.Visible;
            geneScatterView.Items.Clear();
            upperGenes.Clear();
            lowerGenes.Clear();
            Initialized(this, new EventArgs());
        }

        /// <summary>
        /// Displays a list of genes on the chromosome bar. The list
        /// of genes must be in order.
        /// 
        /// @ Mikey Lintz
        /// </summary>
        /// <param name="genes">A list of genes to be displayed. The
        /// list must be in order.</param>
        public void DisplayGenes(List<Gene> genes, String mainGeneName)
        {
            initialize();
            this.mainGeneName = mainGeneName;
            // calculate scale value
            int cumulativeRange = 0;
            foreach (Gene gene in genes)
            {
                cumulativeRange += (gene.getStop() - gene.getStart());
            }
            int averageRange = cumulativeRange / genes.Count;
            scaleValue = idealWidth / averageRange;

            // scale the chromosome bar appropriately
            Gene firstGene = genes.First();
            Gene lastGene = genes.Last();
            geneGrid.Width = (lastGene.getStop() - firstGene.getStart()) * scaleValue + 2 * margin;
            
            // render each gene
            foreach (Gene gene in genes)
            {
                drawGene(scaleValue, gene, firstGene.getStart()); 
            }
            double maxWidth = Math.Max(upperGenes.Last().RightXCoordinate, lowerGenes.Last().RightXCoordinate) -
                Math.Max(upperGenes.First().LeftXCoordinate, lowerGenes.First().LeftXCoordinate) + 2 * margin;
            if (geneGrid.Width < maxWidth)
            {
                geneGrid.Width = maxWidth;
            }
            drawTickMarks();
            if (mainGeneItem != null)
            {
                chromosomeViewer.ScrollToHorizontalOffset(mainGeneItem.LeftXCoordinate - margin);
            }
        }

        private void drawGene(double scaleValue, Gene gene, int leftmostBase)
        {
            String genesComp = SurfaceWindow1.comp + "genes\\";
            double geneWidth = (gene.getGeneWidth() * scaleValue);
            if (geneWidth < minWidth)
            {
                geneWidth = minWidth;
            }
            else if (geneWidth > maxWidth)
            {
                geneWidth = maxWidth;
            }

            //A GeneItem is a ScatterView item with a gene associated to it.
            GeneItem geneItem = new GeneItem(gene, genesComp, entireGeneHandler, dnaHandler, rnaHandler, aaHandler, gene.getGenome(), gene.getChromosomeId());
            if (gene.GetName() == mainGeneName)
            {
                mainGeneItem = geneItem;
            }
            geneItem.MinWidth = 5;
            geneItem.Width = geneWidth;
            geneItem.Height = chromosomeHeight;
            geneItem.CanMove = false;
            geneItem.CanRotate = false;
            geneItem.CanScale = false;

            //Create a new image of the gene.
            //Build gene from a point image, a body image and an end image.
            System.Drawing.Image imgPoint = System.Drawing.Image.FromFile(genesComp + "geneRight.png");
            if (gene.getOrientation() == "-")
            {
                imgPoint = System.Drawing.Image.FromFile(genesComp + "geneLeft.png");
            }
            System.Drawing.Image imgBody = System.Drawing.Image.FromFile(genesComp + "geneRepeat.png");
            System.Drawing.Image imgEnd = System.Drawing.Image.FromFile(genesComp + "geneEnd.png");

            //Start the graphics that will combine the images to make a gene
            int geneRange = gene.getGeneWidth();
            System.Drawing.Bitmap geneBitmap = new System.Drawing.Bitmap((int)geneWidth, (int) chromosomeHeight);
            System.Drawing.Graphics geneGraphics = System.Drawing.Graphics.FromImage(geneBitmap);
            System.Windows.Controls.Image geneImage = new System.Windows.Controls.Image();
            Canvas content = new Canvas();
            content.Opacity = .50;
            content.Background = Brushes.DodgerBlue;
            Label geneName = new Label();

            //GeneName displays the gene name on the geneItem
           
            geneName.FontSize = 9;
            geneName.Opacity = 1;

            //Content is the container for the geneItem: it contains a gene image and a gene name
            content.Children.Add(geneImage);
            content.Children.Add(geneName);
            Canvas.SetTop(geneName, 15);
            //Trying to set the left & right sentering of the geneNameLabel... Not too successful
            if (gene.getOrientation() == "-")
            {
                Canvas.SetRight(geneName, 5);
                geneName.FlowDirection = FlowDirection.RightToLeft;
                geneName.Content = geneItem.geneName() + "\n" + gene.getStop() + "-" + gene.getStart();
            }
            else
            {
                Canvas.SetLeft(geneName, 5);
                geneName.Content = geneItem.geneName() + "\n" + gene.getStart() + "-" + gene.getStop();
            }
            geneName.VerticalAlignment = VerticalAlignment.Top;
            geneName.VerticalContentAlignment = VerticalAlignment.Top;
            geneName.Padding = new Thickness (0);

            //Create a StreamGeometry to use to make the shape of the geneItem be the gene.
            StreamGeometry geometry = new StreamGeometry();
            geometry.FillRule = FillRule.EvenOdd;

            if (gene.getOrientation() == "-")
            {
                // Open a StreamGeometryContext that can be used to describe this StreamGeometry 
                // object's contents.
                using (StreamGeometryContext ctx = geometry.Open())
                {
                    ctx.BeginFigure(new System.Windows.Point(TriangleLength, 0), true, true);
                    ctx.LineTo(new System.Windows.Point(0, 30), true, false);
                    ctx.LineTo(new System.Windows.Point(TriangleLength, 60), true, false);
                    ctx.LineTo(new System.Windows.Point(geneWidth, 60), true, false);
                    ctx.LineTo(new System.Windows.Point(geneWidth, 0), true, false);
                    ctx.LineTo(new System.Windows.Point(TriangleLength, 0), true, false);
                    ctx.Close();
                }
            }
            else
            {
                using (StreamGeometryContext ctx = geometry.Open())
                {

                    // Begin the triangle at the point specified. Notice that the shape is set to 
                    // be closed so only two lines need to be specified below to make the triangle.
                    ctx.BeginFigure(new System.Windows.Point(0, 0), true, true);
                    ctx.LineTo(new System.Windows.Point(0, 60), true, false);
                    ctx.LineTo(new System.Windows.Point(geneWidth - TriangleLength, 60), true, false);
                    ctx.LineTo(new System.Windows.Point(geneWidth, 30), true, false);
                    ctx.LineTo(new System.Windows.Point(geneWidth - TriangleLength, 0), true, false);
                    ctx.LineTo(new System.Windows.Point(0, 0), true, false);
                }
            }

            geneItem.Content = content;  //Set the geneItem content
            geometry.Freeze(); // Freeze the geometry (make it unmodifiable) for additional performance benefits.
            geneItem.Clip = geometry; //Clip the gene to fit the geometry shape
            int xposition = (int)(scaleValue * (gene.getStart() - leftmostBase) + geneItem.Width / 2 + margin);
            if (gene.getOrientation() == "-")
            {
                geneItem.Center = new System.Windows.Point(xposition, (geneGrid.Height - 5) - (geneItem.Height / 2) - 10);
                if (lowerGenes.Count > 0)
                {
                    shiftGeneItemRightIfOverlaps(geneItem, lowerGenes.Last());
                }
                lowerGenes.Add(geneItem);
            }
            else
            {
                geneItem.Center = new System.Windows.Point(xposition, (geneItem.Height / 2) + 5 + 25);
                //geneItem.Center = new System.Windows.Point(xposition, (geneItem.Height / 2)+ 10);
                if (upperGenes.Count > 0)
                {
                    shiftGeneItemRightIfOverlaps(geneItem, upperGenes.Last());
                }
                upperGenes.Add(geneItem);
            }
            geneScatterView.Items.Add(geneItem);
        }

        private void shiftGeneItemRightIfOverlaps(GeneItem newGeneItem, GeneItem leftmostGeneItem)
        {
            double xDistance = newGeneItem.LeftXCoordinate - leftmostGeneItem.RightXCoordinate;
            if (xDistance - TriangleLength < 0) // i.e. the geneitems will overlap 
            {
                Point newCenter = new Point(newGeneItem.Center.X + -1*xDistance + minDistanceBetweenGeneItems, 
                    newGeneItem.Center.Y);
                newGeneItem.Center = newCenter;
            }
        }

        private void drawTickMarks()
        {
            for (int i = LeftmostBasePairIndex / Interval; i < RightmostBasePairIndex / Interval; i++)
            {
                Thickness thick = new Thickness();
                thick.Bottom = 0;
                thick.Left = 0;
                thick.Top = 0;
                thick.Right = 0;
                SurfaceTextBox box = new SurfaceTextBox();
                box.IsReadOnly = true;
                box.BorderThickness = thick;
                box.Text = (i * Interval).ToString();
                //box.Height = 12;
                box.Margin = thick;
                box.Padding = thick;
                box.Background = Brushes.Transparent;
                ScatterViewItem item = new ScatterViewItem();
                item.MinHeight = 0;
                item.MinWidth = 0;
                item.Background = Brushes.Transparent;
                item.Content = box;
                geneScatterView.Items.Add(item);
                item.Height = 20;
                double xposition = convertBaseIndexToXCoordinate(i * Interval);
                item.Center = new Point(xposition, item.Height / 2);
                //item.Center = new Point(xposition, 87.5);
                item.Orientation = 0;
            }
        }

        private double convertBaseIndexToXCoordinate(int basePairIndex)
        {
            return (basePairIndex - LeftmostBasePairIndex) * scaleValue;
        }
    }
}
