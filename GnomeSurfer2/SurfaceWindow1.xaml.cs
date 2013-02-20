using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Threading;

delegate void UpdateConnector(object sender, TouchEventArgs e);
delegate void SequenceMenuTapHandler(object sender, RoutedEventArgs e);

namespace GnomeSurfer2
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            mainSVIList = new List<ScatterViewItem>();//Initialize list that holds items in main scatterview
            RoutedEventHandler entireGeneHandler = new RoutedEventHandler(genSeq_ElementMenuItem_Click);
            RoutedEventHandler dnaHandler = new RoutedEventHandler(DNA_ElementMenuItem_Click);
            RoutedEventHandler rnaHandler = new RoutedEventHandler(RNA_ElementMenuItem_Click);
            RoutedEventHandler aaHandler = new RoutedEventHandler(AA_ElementMenuItem_Click);

            //Initialize the chromosomeBar and the genePolygon
            this._chromosomeBar = new ChromosomeBar(chrviewer, GeneGrid, legend, topGene, entireGeneHandler, dnaHandler, rnaHandler, aaHandler);
            this._genePolygon = new GenePolygon(polyLayer, _exonIntronViewLeftMostPoint, _exonIntronViewRightMostPoint);
            _currentSelectedGenome = Human;    // default selection is human genome
            _progressBarWrapper = new ProgressBarWrapper(new Action(showProgressBar), new Action(hideProgressBar));
            _chromosomeBar.Initialized += new InitializedHandler(_chromosomeBar_Initialized);
            SetGeneMenu();
        }

        void _chromosomeBar_Initialized(object sender, EventArgs e)
        {
            _genePolygon.setSelectedGeneItem(null);
        }

        /// <summary>
        /// Populate geneItems onInitialized
        /// </summary>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            DataContext = this;
        }

        /// <summary>
        /// Print the genes that are around gene that was searched for.
        /// </summary>
        /// <param name="geneId">Gene that was searched for</param>
        /// <param name="genome">Genome of the gene that was searched for</param>
        private void PrintSurroundingGenes(String geneId, String genome)
        {
            Gene centralGene;
            HideGeneNotFoundAlert();
            try
            {
                centralGene = Gene.GetGeneById(geneId, genome);
                _currentChromosomeLabel = centralGene.getChromosomeId();
            }
            catch (GeneNotFoundException e)
            {
                ShowGeneNotFoundAlert();
                return;
            }
            DockSearchMenu(searchmenu);
            List<Gene> surroundingGenes = Gene.GetSurroundingGenes(centralGene);
            _chromosomeBar.DisplayGenes(surroundingGenes, centralGene.GetName());
        }

        /// <summary>
        /// Creates a log of all the contacts of the surface.
        /// @ M. Strait
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainSVI_TouchDown(object sender, TouchEventArgs e)
        {
            //contacts_log.Add(e.Contact + "\t" + e.Timestamp + "\n");
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        #region Static Vars
        //used through out the life of the program in other classes

        private static String executablePath = System.AppDomain.CurrentDomain.BaseDirectory;
        public static String comp = executablePath.Substring(0, executablePath.Length - 10); //12 when launching from the surface, 10 when debugging

        //UPDATE the path to the directory containing gene data and blast executables:

        //surface - hcilab
        //public static String dataDir = "C:\\ProgramData\\Microsoft\\Surface\\Projects\\Data\\GnomeSurfer\\";
        //public static String blastDir = "C:\\ProgramData\\Microsoft\\Surface\\Projects\\Data\\GnomeSurfer\\Executables\\bin\\";

        //big mac screen - oshaer
        //public static String dataDir = "C:\\Users\\oshaer\\Documents\\Data\\GnomeSurfer\\";
        //public static String blastDir = "C:\\Users\\oshaer\\Documents\\Data\\GnomeSurfer\\Executables\\bin\\";

        //right station - orit44
        public static String dataDir = comp+"Data\\";
        public static String blastDir = comp + "Data\\Executables\\bin\\";

        //on left station - orit33
        //public static String dataDir = "C:\\Users\\orit33\\Documents\\Gnome_Surfer\\Data\\";
        //public static String blastDir = "C:\\Users\\orit33\\Documents\\Gnome_Surfer\\Data\\Executables\\bin\\";

        public static bool hasTestTubeTag;
        public static Point testTubePoint;
        public static Point PrintTag_Center;
        public static bool hasPrintTag;
        public static List<ScatterViewItem> mainSVIList;
        private static Random generator = new Random();
        public static Point exonIntronConnectorPoint = new Point(0, 0);//For creating blast sequence for returning to main canvas
        #endregion

        #region Constants
        private const String DNA_SEQUENCEBOX_NAME = "dnaText";
        private const String RNA_SEQUENCEBOX_NAME = "rnaText";
        private const String AA_SEQUENCEBOX_NAME = "aaText";
        //because these are not defined in their own xaml files, their values need to be saved like this to be referred to throughout application
        public const Byte ERASER_TAG_VALUE = 0x00;
        public const Byte PRINTER_TAG_VALUE = 0x02;

        //Chromosome count of each organimsm because we are not including any non-numbered chromosomes in the count, ie X or Y
        public const int DOG_CHR_COUNT = 39;
        public const int FISH_CHR_COUNT = 25;
        public const int HUMAN_CHR_COUNT = 22;
        public const int MONKEY_CHR_COUNT = 21;
        public const int MOUSE_CHR_COUNT = 19;
        public const int RAT_CHR_COUNT = 20;
        #endregion

        #region Instance Variables

        //Logs the contacts on the surface throughout the life of the application
        public List<String> contacts_log = new List<string>();

        // Selection Menu Labels
        private ScatterViewItem _currentSelectedGenome;//For search menu, creates the radio-button-like interaction with higlighting
        private double _currentChromosomeLabel = 0;

        private ChromosomeBar _chromosomeBar;
        //ElementMenu that pops up from selecting a gene in the chromosome bar
        private ElementMenu _geneElementMenu = new ElementMenu();
        private ElementMenuItem _expressionElementMenuItem = new ElementMenuItem();
        private ElementMenuItem _ontologyElementMenuItem = new ElementMenuItem();
        private ElementMenuItem _publicationsElementMenuItem = new ElementMenuItem();
        //private ElementMenuItem _entireAASequenceElementMenuItem = new ElementMenuItem();
        private ElementMenuItem _entireDNASequenceElementMenuItem = new ElementMenuItem();
        //private ElementMenuItem _entireRNASequenceElementMenuItem = new ElementMenuItem();

        //creating genePolygon that gives the context of the gene and exon/intron view
        private GenePolygon _genePolygon;
        private Boolean _genePolygonIsVisible;//Switch boolean for genePolygon to hide it when you're scrolling or trying to access the top gene ElementMenu or bottom genes under genePolygon
        private Point _exonIntronViewLeftMostPoint = new Point(0, 290);//For drawing the genePolygon so that it's ends match the ExonIntronview
        private Point _exonIntronViewRightMostPoint = new Point(1024, 290);//For drawing the genePolygon so that it's ends match the ExonIntronview
        private double _scrollOffset = 0;//used to update the genePolygon when the chromosomeBar is scrolled

        //Properties of the current gene being manipulated
        private bool _focusingOnGeneInTopRow = false; //whether you are working with a gene on the top row or bottom row
        private GeneItem _currentGeneItem;
        private Gene _currentGene;
        private String _currentSequence = "";

        private ProgressBarWrapper _progressBarWrapper;
        private Thread _currentLongOperationThread;//Thread reference for killing Blast thread in the case that Blast tag is removed mid-process

        private SystemProcess _systemProcess = new SystemProcess();//the process is instantiated her and used for printing and blast
        private bool _entireEntireGeneSequenceClicked = false;
        List<SequenceBox> _printedSequenceBoxes = new List<SequenceBox>();//Holds the sequences to be printed for docking
        //public List<Canvas> _printedSequenceBoxesLineParents = new List<Canvas>();//Holds a reference to the printedSequenceBoxes parent canvas
        //public List<Line> _printedSequenceBoxesLines = new List<Line>();//Holds the lines of the printedSequenceBoxes

        //Bases and BLAST variables
        private String _organismBeingBlasted = "";//Used in BLAST methods for calls and result formatting
        SequenceBox _blastSequenceBox;//Sequence that is docked for blast
        private int _numberOfBlastResultsInFlower = 8;//L:imits the number of blast results displayed in flower visualization
        List<SequenceBox> _blastedSequenceBoxes = new List<SequenceBox>();

        //Tags
        private Point _blastTagCenterPoint;
        private Point _trashTagCenterPoint;
        private bool _hasBlastTag = false;
        private bool _hasTrashTag = false;
        private bool _savedItemsVisibilityCheck = true;//checks to see what state the "saved" items are in

        //CONSUELO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! FIX!
        //Expression and Ontology
        private ErasableScatterViewItem pub_titles;
        private ErasableScatterViewItem pub_abstracts;
        private Grid contentArea;
        private Grid content;

        #endregion

       

        #region Search Menu (authors: M. Strait, C. Valdes, C. Fan, M. Lintz)

        /// <summary>
        /// When the search menu is released, tests to see if it needs to be docked
        /// @ Chloe Fan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchmenu_TouchUp(object sender, ContainerManipulationCompletedEventArgs e)
        {
            if (e.Source is ScatterViewItem)
            {
                ScatterViewItem menu = e.Source as ScatterViewItem;
                double currentCenterX = (menu.ActualCenter).X;
                double currentCenterY = (menu.ActualCenter).Y;

                if (currentCenterY <= 0)
                {
                    DockSearchMenu(menu);
                }
            }
        }

        /// <summary>
        /// method that docks the search menu when it has been released by the user and within "docking region"
        /// </summary>
        /// <param name="menu">Search menu SVI</param>
        private void DockSearchMenu(ScatterViewItem menu)
        {
            double labelHeight = -(searchmenu.Height / 2 - 30); // height up to lower label
            menu.Center = new Point(menu.ActualCenter.X, labelHeight);
            menu.Orientation = 0;
        }

        /// <summary>
        /// Applies a radio-button-like functionality to the organism SVIs of the search menu, and updates the search params
        /// based on the current selection.
        /// @ Megan Strait
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrgSelect_TouchDown(object sender, TouchEventArgs e)
        {
            ScatterViewItem svi = e.Source as ScatterViewItem;
            svi.BorderThickness = new Thickness(5);// define a blue border around selected org
            svi.BorderBrush = Brushes.DodgerBlue;
            svi.CanScale = false;

            if (_currentSelectedGenome == null) { _currentSelectedGenome = svi; } // on initialization, current_org must be set
            else if (_currentSelectedGenome != svi)// else, remove the border from the previous selection and update current org
            {
                _currentSelectedGenome.BorderThickness = new Thickness(5);
                _currentSelectedGenome.BorderBrush = Brushes.Black;
                _currentSelectedGenome = svi;
            }

            double maxNum;
            if (_currentSelectedGenome.Name.Equals("human")) { maxNum = 22; }
            else if (_currentSelectedGenome.Name.Equals("monkey")) { maxNum = 23; }
            else if (_currentSelectedGenome.Name.Equals("mouse")) { maxNum = 19; }
            else { maxNum = 20; }

            /*if (sm_slider != null)
            {
                sm_slider.Maximum = maxNum;
                chromosomeTickbar.Maximum = maxNum;
            }*/

            if (menuLabel != null)
            {
                menuLabel.Content = _currentSelectedGenome.Name + ", Chromosome " + _currentChromosomeLabel;
            }

        }

        /// <summary>
        /// LEGACY CODE 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void organismButton_Unchecked(object sender, RoutedEventArgs e)
        {
            SurfaceRadioButton checkedButton = e.Source as SurfaceRadioButton;
            string checkedOrganism = checkedButton.Name;

            checkedButton.Background = Brushes.Transparent;
        }

        /// <summary>
        /// LEGACY CODE: Updates the chromosome label based on a slider in the search menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sm_chromosome_SnappedValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _currentChromosomeLabel = (e.Source as SurfaceSlider).SnappedValue;
            if (menuLabel != null)
            {
                if (_currentSelectedGenome != null) { menuLabel.Content = _currentSelectedGenome.Name + ", Chromosome " + _currentChromosomeLabel; }
                else { menuLabel.Content = ", Chromosome " + _currentChromosomeLabel; }
            }
        }

        /// <summary>
        /// Called when the search menu search button is pressed. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_TouchUp(object sender, TouchEventArgs e)
        {
            GetSearchResults();
        }

        /// <summary>
        /// If user pressed enter act as if they pressed the search button.
        /// @ C. Valdes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void geneName_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                GetSearchResults();

            }
        }

        /// <summary>
        /// Kicks off the UI search results and refreshes the screen after each search.
        /// Pulls the info input by the user and updates the UI with the corresponding information.
        /// @ M. Lintz, C. Valdes
        /// </summary>
        private void GetSearchResults()
        {

            String genome = _currentSelectedGenome.Name;
            String geneId = geneName.Text.ToUpper();
            //_genePolygon.hide();

            PrintSurroundingGenes(geneId, genome);

            ChromSV.Visibility = Visibility.Visible;


            for (int i = 0; i < MainSV.Items.Count; i++)
            {
                ScatterViewItem s = (ScatterViewItem)MainSV.Items.GetItemAt(i);
                Console.WriteLine(s.Name);
                if (s.Name != "ChromSV" && s.Name != "searchmenu")
                {
                    //s.Visibility = Visibility.Hidden;
                    //s.BorderBrush = Brushes.Black;
                    //s.Center = new Point(100, 650);
                    //s.CanMove = true;
                    //s.Height = 100;
                    if (s.Name == "exonInt")
                        s.Visibility = Visibility.Collapsed;
                    else
                        DockSVI(s, new Point(110, 650));
                }
            }

            //Meant to take all of "previous search" stuff and put in corner for new search
            for (int i = 0; i < ExonScatter.Items.Count; i++)
            {
                ScatterViewItem scatt = (ScatterViewItem)ExonScatter.Items.GetItemAt(i);
                //scatt.BorderBrush = Brushes.Black;
                //scatt.Center = new Point(100, 650);
                //scatt.CanMove = true;
                //scatt.Height = 50;
                DockSVI(scatt, new Point(110, 650));
                if (scatt.Name.Contains("Text"))
                {
                    {
                        SequenceBox s = (SequenceBox)ExonScatter.Items.GetItemAt(i);
                        Line l = s.getConnector();
                        l.Visibility = Visibility.Hidden;
                    }
                }

            }
            menuLabel.Content = _currentSelectedGenome.Name + ", Chromosome " + _currentChromosomeLabel + " |  Gene: " + geneId;
            _genePolygon.hide();
        }

        private void ShowGeneNotFoundAlert()
        {
            GeneNotFoundAlert.Visibility = Visibility.Visible;
            menuLabel.Visibility = Visibility.Hidden;

        }

        private void HideGeneNotFoundAlert()
        {
            GeneNotFoundAlert.Visibility = Visibility.Hidden;
            menuLabel.Visibility = Visibility.Visible;

        }
        #endregion

        #region ChromosomeBar Events (authors: M. Lintz, C. Valdes, C. Grevet, M. Strait, C. Fan)
        /// <summary>
        /// Instantiates the ElementMenu, GeneMenu, to appear when contact is placed on a gene.
        /// *Still appears under the polygon.
        /// @ M. Strait
        /// </summary>
        private void SetGeneMenu()
        {
            _geneElementMenu.ActivationMode = ElementMenuActivationMode.AlwaysActive;
            _geneElementMenu.Height = 50; _geneElementMenu.Width = 50;
            _geneElementMenu.Visibility = Visibility.Hidden;
            _geneElementMenu.TouchDown += new EventHandler<TouchEventArgs>(GeneMenu_ContactDown);
            _geneElementMenu.TouchUp += new EventHandler<TouchEventArgs>(GeneMenu_TouchUp);

            _expressionElementMenuItem.Header = "expression";
            _ontologyElementMenuItem.Header = "ontology";
            _publicationsElementMenuItem.Header = "publications";
            //_entireAASequenceElementMenuItem.Header = "entire aa sequence";
            _entireDNASequenceElementMenuItem.Header = "DNA sequence";
            //_entireRNASequenceElementMenuItem.Header = "entire rna sequence";

            _geneElementMenu.Items.Add(_expressionElementMenuItem);
            _geneElementMenu.Items.Add(_ontologyElementMenuItem);
            _geneElementMenu.Items.Add(_publicationsElementMenuItem);
            //_geneElementMenu.Items.Add(_entireAASequenceElementMenuItem);
            _geneElementMenu.Items.Add(_entireDNASequenceElementMenuItem);
            //_geneElementMenu.Items.Add(_entireRNASequenceElementMenuItem);

            for (int i = 0; i < _geneElementMenu.Items.Count; i++)
            {
                ElementMenuItem emi = _geneElementMenu.Items.GetItemAt(i) as ElementMenuItem;
                emi.Height = 30; emi.Width = 30;
                emi.Background = Brushes.DodgerBlue;
                emi.Click += new RoutedEventHandler(GeneMenuItem_Click);
            }
            MenuCanvas.Children.Add(_geneElementMenu);
        }

        /// <summary>
        /// Change context line/polygon with scroll
        /// @ Mikey Lintz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chrviewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            SurfaceScrollViewer scrollViewer = (SurfaceScrollViewer)sender;
            if (scrollViewer.IsScrolling)
            {
                _scrollOffset = e.HorizontalOffset;
                _genePolygon.draw(_scrollOffset);
            }
            if (ExonScatter.Visibility == Visibility.Hidden)
                _genePolygon.hide();
        }

        /// <summary>
        /// Collapses the Polygon when the GeneMenu is open on the top genes, so that the ElementMenuItems can be selected.
        /// @ M. Strait
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GeneMenu_ContactDown(object sender, TouchEventArgs e)
        {
            if (_focusingOnGeneInTopRow) { _genePolygon.hide(); }
        }

        void GeneMenu_TouchUp(object sender, TouchEventArgs e)
        {
            if (_focusingOnGeneInTopRow) { _genePolygon.show(); }
        }

        /// <summary>
        /// Event handler that responds based on the selection made. Method concatenates the three different
        /// methods constructed
        /// @ M. Lintz, C. Valdes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GeneMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ElementMenuItem tempGeneElementMenu = e.OriginalSource as ElementMenuItem;

            /*if (tempGeneElementMenu.Header == "entire aa sequence")
            {
                GenerateEntireGeneSequence("AA");
            }
            else*/
            if (tempGeneElementMenu.Header == "DNA sequence")
            {
                GenerateEntireGeneSequence("DNA");
            }
            else /*if (tempGeneElementMenu.Header == "entire rna sequence")
            {
                GenerateEntireGeneSequence("RNA");
            }
            else*/
                if (tempGeneElementMenu.Header == "expression")
                {
                    if (Gene.MouseGenome.Equals(_currentGene.getGenome()))
                    {
                        GeneExpressionModelFactory modelFactory = new GeneExpressionModelFactory();
                        ExpressionViewFactory viewFactory = new ExpressionViewFactory(modelFactory, trashSVIs);
                        ScatterViewItem svi = viewFactory.GetMouseExpressionSVI(_currentGene);
                        MainSV.Items.Add(svi);
                    }
                    else
                    {
                        MakeExpressionSVI();
                    }
                }
                else if (tempGeneElementMenu.Header == "ontology")
                {
                    _progressBarWrapper.execute<Ontology>(_currentGeneItem.getOntology, showOntology);
                }
                else
                {//Then Publications was selected. Pull up progressIndicator and run longOp in background
                    Func<String, PubList> newPubList = delegate(String geneId)
                    {
                        return new PubList(geneId);
                    };
                    _progressBarWrapper.execute<String, PubList>(newPubList, _currentGene.getID(), showPublications);
                }
        }

        private void GenerateEntireGeneSequence(String entireSequenceType)
        {
            ErasableScatterViewItem entireSequenceESVI = new ErasableScatterViewItem(trashSVIs);
            //scatt.Name = ;
            entireSequenceESVI.Width = 350;
            entireSequenceESVI.Height = 200;
            entireSequenceESVI.CanScale = true;
            entireSequenceESVI.CanRotate = false;
            entireSequenceESVI.BorderThickness = new Thickness(15);
            entireSequenceESVI.BorderBrush = Brushes.Gray;
            entireSequenceESVI.Background = Brushes.White;
            entireSequenceESVI.Opacity = .75;
           // entireSequenceESVI.DecelerationRate = double.NaN;

            SurfaceScrollViewer viewer = new SurfaceScrollViewer();
            Canvas can = new Canvas();
            SurfaceTextBox textBox = new SurfaceTextBox();
            textBox.TextWrapping = TextWrapping.Wrap;
            textBox.IsReadOnly = true;
            String geneSequence = _currentGene.getSequence();
            int count = 0;
            int totalCharacterAmount = 0;

            StringBuilder formattedSequenceBuilder = new StringBuilder();
            formattedSequenceBuilder.Append(entireSequenceType + "Sequence for " + _currentGene.GetName() +
                "\nRange: " + _currentGene.getStart() + " to " + _currentGene.getStop() + " base pairs\n\n0\t");

            if (entireSequenceType.Equals("AA"))
            {
                for (int i = 0; i < geneSequence.Length; i++)
                {
                    formattedSequenceBuilder.Append(geneSequence.Substring(i, 1) + "\t");
                    count++;
                    totalCharacterAmount += 3;
                    if (count == 5)
                    {
                        formattedSequenceBuilder.Append("\n\n" + totalCharacterAmount + "\t");
                        count = 0;
                    }
                }
            }
            else
            {
                for (int i = 0; i < geneSequence.Length - 2; i++)
                {
                    formattedSequenceBuilder.Append(geneSequence.Substring(i, 3).ToUpper() + "\t");
                    i += 2;
                    count++;
                    totalCharacterAmount += 3;
                    if (count == 5)
                    {
                        formattedSequenceBuilder.Append("\n\n" + totalCharacterAmount + "\t");
                        count = 0;
                    }
                }
            }

            textBox.Text = formattedSequenceBuilder.ToString();

            textBox.Width = 300; textBox.Height = (totalCharacterAmount * 2 + 3) * 1.08;
            textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            can.Children.Add(textBox);

            Viewbox view = new Viewbox();
            view.Height = Double.NaN; view.Width = Double.NaN;
            view.StretchDirection = StretchDirection.Both;

            view.Stretch = Stretch.Fill;
            view.Height = Double.NaN;
            view.Width = Double.NaN;

            SurfaceInkCanvas ink = new SurfaceInkCanvas();
            ink.Background = Brushes.Transparent;
            //ink.UsesContactShape = false;
            ink.Height = textBox.Height;
            ink.HorizontalAlignment = HorizontalAlignment.Stretch;
            ink.VerticalAlignment = VerticalAlignment.Stretch;

            view.Child = ink;

            can.Children.Add(view);
            can.Height = textBox.Height;
            viewer.Content = can;
            viewer.CanContentScroll = true;
            viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            entireSequenceESVI.Content = viewer;

            ExonScatter.Items.Add(entireSequenceESVI);
            //mainSVIList.Add(scatt);//----------------------------------------------------------------------------------------------

            //_entireEntireGeneSequenceClicked = true;
        }

        /// <summary>
        /// Adapted from S. Elfenbein's expression EMI click event handler. Renamed most vars so we can understand them; removed about
        /// 50 lines of code by implementing a loop (could still be more concise). Not sure what all the numbers represent, so didn't
        /// change any of them. Updated the look of the expression canvas for a prettier and more cohesive appearance.
        /// @ M. Lintz 
        /// </summary>
        private void MakeExpressionSVI()
        {
            if ((_currentGeneItem != null) & !_currentGeneItem.getSelectionOpen(0))
            {
                //Expression is too crazy to make erasaqble right now. - CV, 2/21/10
                ScatterViewItem svi = new ScatterViewItem();
                svi.Background = Brushes.Black;
                svi.BorderThickness = new Thickness(10);
                svi.CanScale = false;
                svi.Opacity = 0.8;

                Canvas canvas = new Canvas();
                canvas.Height = 340; canvas.Width = 130;

                Grid grid = new Grid();
                grid.Height = 340; grid.Width = 130;
                grid.Background = Brushes.White;
                canvas.Children.Add(grid);

                Label label = new Label();
                label.Background = Brushes.White;
                label.Content = _currentGeneItem.geneName();
                canvas.Children.Add(label);

                Image human_img = new Image();
                human_img.Source = new BitmapImage(new Uri("Resources/Human.jpg", UriKind.Relative));
                grid.Children.Add(human_img);

                ElementMenu head = new ElementMenu();
                ElementMenu chest = new ElementMenu();
                ElementMenu gut = new ElementMenu();
                ElementMenu germ = new ElementMenu();
                ElementMenu limb = new ElementMenu();
                ElementMenu lymph = new ElementMenu();

                List<ElementMenu> eleList = new List<ElementMenu>();
                eleList.Add(head);
                eleList.Add(chest);
                eleList.Add(gut);
                eleList.Add(germ);
                eleList.Add(limb);
                eleList.Add(lymph);

                for (int i = 0; i < eleList.Count; i++) // loop through the EMIs in g1 (start at the first index)
                {
                    ElementMenu em = eleList.ElementAt(i);
                    grid.Children.Add(em);
                    em.ActivationMode = ElementMenuActivationMode.AlwaysActive;
                    em.SubmenuClosed += new RoutedEventHandler(ElementMenu_SubmenuClosed);
                    em.HorizontalAlignment = HorizontalAlignment.Center;
                    em.VerticalAlignment = VerticalAlignment.Top;
                }

                head.Orientation = -30;
                head.RenderTransform = new TranslateTransform(0, 0);
                chest.Orientation = 76;
                chest.RenderTransform = new TranslateTransform(0, 50);
                gut.Orientation = -110;
                gut.RenderTransform = new TranslateTransform(0, 100);
                germ.Orientation = 135;
                germ.VerticalAlignment = VerticalAlignment.Center;
                germ.RenderTransform = new TranslateTransform(0, 0);
                limb.Orientation = 180;
                limb.RenderTransform = new TranslateTransform(16, 224);
                lymph.Orientation = -63;
                lymph.RenderTransform = new TranslateTransform(-30, 50);

                new Human(head, chest, gut, germ, limb, lymph);

                svi.Content = canvas;
                MainSV.Items.Add(svi);
            }
        }

        private void ChromSV_TouchUp(object sender, TouchEventArgs e)
        {
            PointCollection pc = _genePolygon.GetPoints();

            if (pc != null)
            {
                if (_genePolygonIsVisible)
                {

                    if(e.GetTouchPoint(this).Position.X < (pc.ElementAt(2).X + pc.ElementAt(1).X) / 2 &&
                        e.GetTouchPoint(this).Position.Y > pc.ElementAt(0).Y &&
                        e.GetTouchPoint(this).Position.Y < pc.ElementAt(3).Y)
{
                        _genePolygon.hide();
                        _genePolygonIsVisible = false;
                    }
                }
                else
                {
                    _genePolygon.show();
                    _genePolygonIsVisible = true;
                }

            }
        }

        /// <summary>
        /// When the user taps on a gene, set the current itemGene to 
        /// be the gene touched, set the position of the gene info to be at the 
        /// position of the contact. Make the exon and intron view visible.
        /// @ Catherine Grevet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void topGene_TouchDown(object sender, TouchEventArgs e)
        {
            if (_currentGeneItem != null)
            {
                if (MainSV.Items.Contains(_currentGeneItem.exonIntView()))
                {
                    MainSV.Items.Remove(_currentGeneItem.exonIntView());
                }
            }
            _geneElementMenu.Visibility = Visibility.Visible;

            if (e.GetTouchPoint(this).Position.Y < 120) { _focusingOnGeneInTopRow = true; }
            else { _focusingOnGeneInTopRow = false; }

            FrameworkElement findSource = e.OriginalSource as FrameworkElement;
            GeneItem draggedElement = null;

            // Find the touched SurfaceListBoxItem object.
            while (draggedElement == null && findSource != null)
            {
                if ((draggedElement = findSource as GeneItem) == null)
                {
                    findSource = VisualTreeHelper.GetParent(findSource) as FrameworkElement;
                }
            }
            _currentGeneItem = draggedElement;

            _currentGeneItem.IsEnabled = true;
            _currentGene = _currentGeneItem.getGene();

            //Change the search menu label to reflect the current gene selected
            menuLabel.Content = _currentSelectedGenome.Name + ", Chromosome " + _currentGene.getChromosomeId() + " |  Gene: " + _currentGene.GetName() + " | Base Pairs: " + _currentGene.getStart() + " - " + _currentGene.getStop();

            positionGeneMenu(e.GetTouchPoint(this).Position.X);
            if (!MainSV.Items.Contains(_currentGeneItem.exonIntView()))
            {
                MainSV.Items.Add(_currentGeneItem.exonIntView());
                ExonMenu.Visibility = Visibility.Visible;
                _genePolygon.setSelectedGeneItem(_currentGeneItem);
                _genePolygon.draw(_scrollOffset);
            }

            for (int i = 0; i < ExonScatter.Items.Count; i++)
            {
                ScatterViewItem scatt = (ScatterViewItem)ExonScatter.Items.GetItemAt(i);
                if (scatt.Name.Contains("Text"))
                {
                    {
                        SequenceBox s = (SequenceBox)ExonScatter.Items.GetItemAt(i);
                        Line l = s.getConnector();
                        l.Visibility = Visibility.Hidden;
                    }
                }

            }
        }

        /// <summary>
        /// This event allows the chromosome view to scroll when the user drags their
        /// finger along the chromosome.
        /// @ Catherine Grevet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void topGene_TouchMove(object sender, TouchEventArgs e)
        {
            FrameworkElement findSource = e.OriginalSource as FrameworkElement;
            GeneItem draggedElement = null;

            // Find the touched SurfaceListBoxItem object.
            while (draggedElement == null && findSource != null)
            {
                if ((draggedElement = findSource as GeneItem) == null)
                {
                    findSource = VisualTreeHelper.GetParent(findSource) as FrameworkElement;
                }
            }
            _currentGeneItem = draggedElement;

            if (_currentGeneItem != null)
            {
                _currentGeneItem.IsHitTestVisible = false;
            }
        }

        /// <summary>
        /// When the user lifts their contact with the gene, set the itemGene 
        /// isHitTestVisible value to true.
        /// @ Catherine Grevet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void topGene_TouchUp(object sender, TouchEventArgs e)
        {
            if (_currentGeneItem != null) { _currentGeneItem.IsHitTestVisible = true; }
        }

        private void GeneBox_DragCompleted(object sender, SurfaceDragCompletedEventArgs e)
        {
            _genePolygon.draw(_scrollOffset);
            // need to take into account top or bottom gene row
        }

        #endregion

        #region BaseBox events MYSTERY CODE?!?!?!?!??!

        private void BasesBox_DragEnter(object sender, SurfaceDragDropEventArgs e)
        {
            DataItem data = e.Cursor.Data as DataItem;

            if (!data.CanDrop)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void BasesBox_DragLeave(object sender, SurfaceDragDropEventArgs e)
        {
            // Reset the effects.
            e.Effects = e.Cursor.AllowedEffects;
        }

        private void BasesBox_Drop(object sender, SurfaceDragDropEventArgs e)
        {
            //This is part of the genes layout, should not be erasable. - CV, 2/21/10
            ScatterViewItem svi = new ScatterViewItem();
            svi.CanMove = true;
            svi.CanScale = true;
            svi.BorderThickness = new Thickness(10);
            svi.Width = 200;
            svi.Height = 200;

            Grid can = new Grid();

            Bases.Content = svi; // error if use ink instead
        }

        private void OnTargetChanged(object sender, TargetChangedEventArgs e)
        {
            if (e.Cursor.CurrentTarget != null)
            {
                DataItem data = e.Cursor.Data as DataItem;
                e.Cursor.Visual.Tag = (data.CanDrop) ? "CanDrop" : "CannotDrop";
            }
            else { e.Cursor.Visual.Tag = null; }
        }

        #endregion

        #region Exon/Intron related (includes ElementMenu and connectors)

        /// <summary>
        /// Creates a SVI displaying the entire gene sequence
        /// @ MS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void genSeq_ElementMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (_entireEntireGeneSequenceClicked)
            {
                //ErasableScatterViewItem scatt = new ErasableScatterViewItem(trashSVIs);
                //scatt.Name = "allDNAtext";
                //scatt.Width = 350; 
                //scatt.Height = 250;
                //scatt.CanScale = true; 
                //scatt.CanRotate = false;
                //scatt.BorderThickness = new Thickness(20);
                //scatt.BorderBrush = Brushes.Gray;
                //scatt.Opacity = .8;
                //scatt.Background = Brushes.White;
                //scatt.DecelerationRate = double.NaN;

                //SurfaceScrollViewer viewer = new SurfaceScrollViewer();
                //Canvas can = new Canvas();

                //SurfaceTextBox geneText = new SurfaceTextBox();
                //geneText.TextWrapping = TextWrapping.Wrap;
                //geneText.IsReadOnly = true;
                //String geneSequence = _currentGene.getSequence();
                //int count = 0; 
                //int cTotal = 0;
                //StringBuilder formattedSequenceBuilder = new StringBuilder();
                //formattedSequenceBuilder.Append("DNA Sequence for " + _currentGene.getID() + 
                //    "\nRange: " + _currentGene.getStart() + " to " + _currentGene.getStop() + " base pairs\n\n0\t");
                //for (int i = 0; i < geneSequence.Length - 2; i++)
                //{
                //    formattedSequenceBuilder.Append(geneSequence.Substring(i, 3).ToUpper() + "\t");
                //    i += 2;
                //    count++;
                //    cTotal += 3;
                //    if (count == 5) { 
                //        formattedSequenceBuilder.Append("\n\n" + cTotal + "\t"); 
                //        count = 0; 
                //    }
                //}
                //geneText.Text = formattedSequenceBuilder.ToString();
                //_currentSequence = _currentGene.getSequence();
                //geneText.Width = 300; geneText.Height = (cTotal*2 + 3)*1.08;
                //geneText.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                //can.Children.Add(geneText);

                //Viewbox view = new Viewbox();
                //view.Height = Double.NaN; view.Width = Double.NaN;
                //view.StretchDirection = StretchDirection.Both;

                //view.Stretch = Stretch.Fill;
                //view.Height = Double.NaN; view.Width = Double.NaN;

                //SurfaceInkCanvas ink = new SurfaceInkCanvas();
                //ink.Background = Brushes.Transparent;
                //ink.UsesContactShape = false;
                //ink.Height = geneText.Height;
                //ink.HorizontalAlignment = HorizontalAlignment.Stretch;
                //ink.VerticalAlignment = VerticalAlignment.Stretch;

                //view.Child = ink;

                //can.Children.Add(view);
                //can.Height = geneText.Height;
                //viewer.Content = can;
                //viewer.CanContentScroll = true;
                //viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                //viewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                //scatt.Content = viewer;

                //ExonScatter.Items.Add(scatt);
                ////mainSVIList.Add(scatt);//----------------------------------------------------------------------------------------------
                //ElementMenuItem ele = e.Source as ElementMenuItem;
                //ElementMenu eleP = ele.Parent as ElementMenu;
                //eleP.ActivationMode = ElementMenuActivationMode.AlwaysActive;

                //_entireEntireGeneSequenceClicked = true;
            }
        }

        /// <summary>
        /// Creates a SVI displaying the DNA sequence from the selected exon
        /// @ MS + CG
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DNA_ElementMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ElementMenuItem menuItem = sender as ElementMenuItem;
            SequenceElementMenu seqMenu = menuItem.MenuRoot as SequenceElementMenu;
            String name = DNA_SEQUENCEBOX_NAME;
            Exon exon = seqMenu.getExon();
            String text_unformatted = exon.getDnaSequence();
            String text_formatted;

            text_formatted = formatSequence(exon, "DNA", text_unformatted);
            makeSequenceSVI(name, text_formatted, text_unformatted, e);
        }

        /// <summary>
        /// Creates a SVI displaying the RNA sequence from the selected exon
        /// @ CG + MS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RNA_ElementMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ElementMenuItem menuItem = sender as ElementMenuItem;
            SequenceElementMenu seqMenu = menuItem.MenuRoot as SequenceElementMenu;
            String name = RNA_SEQUENCEBOX_NAME;
            Exon exon = seqMenu.getExon();
            String text_unformatted = exon.getRnaSequence();
            String text_formatted = formatSequence(exon, "RNA", text_unformatted);
            makeSequenceSVI(name, text_formatted, text_unformatted, e);
        }

        /// <summary>
        /// Creates a SVI displaying the AA sequence from the selected exon
        /// @ MS + CG
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AA_ElementMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ElementMenuItem menuItem = sender as ElementMenuItem;
            SequenceElementMenu seqMenu = menuItem.MenuRoot as SequenceElementMenu;
            String name = AA_SEQUENCEBOX_NAME;
            Exon exon = seqMenu.getExon();
            String text_unformatted = exon.getAaSequence();
            String text_formatted = formatSequence(exon, "AA", text_unformatted);
            makeSequenceSVI(name, text_formatted, text_unformatted, e);


        }

        /// <summary>
        /// Formats the sequence into tab delimited string. It's the formating you see in the sequence box.
        /// @Mikey Lintz, Consuelo Valdes
        /// </summary>
        /// <param name="exon">Exon selected for the line</param>
        /// <param name="sequenceType">AA, DNA, or RNA</param>
        /// <param name="exonSequence"></param>
        /// <returns></returns>
        private String formatSequence(Exon exon, String sequenceType, String exonSequence)
        {
            int count = 0;
            int cTotal = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(sequenceType + " Sequence for " + _currentGene.GetName() +
                "\nRange: " + exon.getStart() + " to " + exon.getStop() + " base pairs\n\n0\t");
            if (sequenceType.Equals("AA"))
            {
                for (int i = 0; i < exonSequence.Length; i++)
                {
                    sb.Append(exonSequence.Substring(i, 1) + "\t");
                    count++;
                    cTotal += 3;
                    if (count == 5)
                    {
                        sb.Append("\n\n" + cTotal + "\t");
                        count = 0;
                    }
                }
            }
            else
            {
                for (int i = 0; i < exonSequence.Length - 2; i++)
                {
                    sb.Append(exonSequence.Substring(i, 3).ToUpper() + "\t");
                    i += 2;
                    count++;
                    cTotal += 3;
                    if (count == 5)
                    {
                        sb.Append("\n\n" + cTotal + "\t");
                        count = 0;
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Instantiates a ScatterViewItem to display a DNA, RNA, or AA sequence 
        /// @ Mikey Lintz
        /// </summary>
        /// <param name="connector">The Line connecting the SVI to the Exon/Intron bar</param>
        /// <param name="name">The title of the SVI</param>
        /// <param name="updateConnector">Handler for the Contact Event</param>
        /// <param name="text">Formatted DNA, RNA, or AA sequence</param>
        /// <param name="e">The handler event</param>
        private void makeSequenceSVI(String name, String text_formatted, String text_unformatted, RoutedEventArgs e)
        {
            ElementMenuItem menuItem = e.Source as ElementMenuItem;
            SequenceElementMenu seqMenu = menuItem.MenuRoot as SequenceElementMenu;
            Point exintContact = new Point(seqMenu.getExonLeft() - 15, 0);
            SequenceBox scatt = new SequenceBox(exintContact, name, text_formatted, text_unformatted, trashSVIs);
            ExonScatter.Items.Add(scatt);
            //mainSVIList.Add(scatt);//----------------------------------------------------------------------------------------------
            ElementMenuItem ele = e.Source as ElementMenuItem;
            ElementMenu eleP = ele.Parent as ElementMenu;
            eleP.ActivationMode = ElementMenuActivationMode.AlwaysActive;
            ExonMenu.Children.Add(scatt.getConnector());
            _currentSequence = text_formatted;
            scatt.ContainerManipulationCompleted += new ContainerManipulationCompletedEventHandler(scatt_ContainerManipulationCompleted);
        }

        /// <summary>
        /// Supports docking feature for Blast and Print tags. Also starts up printing
        /// @ MS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void scatt_ContainerManipulationCompleted(object sender, ContainerManipulationCompletedEventArgs e)
        {
            SequenceBox tempSequenceBox = sender as SequenceBox;

            //SequenceBox copyForMainCanvas  = sender as SequenceBox;
            //MainSV.Items.Add(copyForMainCanvas);
            if (hasPrintTag & tempSequenceBox.Center.X < PrintTag_Center.X + 300 & tempSequenceBox.Center.X > PrintTag_Center.X - 300)
            {
                _systemProcess.Save("Results\\", "Sequence", tempSequenceBox.get_text());
                _systemProcess.Print();

                _printedSequenceBoxes.Add(tempSequenceBox);
                //dock item to be printed under printTag
                tempSequenceBox.Center = new Point(PrintTag_Center.X - 150 + generator.Next(300), PrintTag_Center.Y - 150 + generator.Next(300));
                tempSequenceBox.Height = 100; tempSequenceBox.Width = 100;
                tempSequenceBox.BorderBrush = Brushes.SkyBlue;
                Line l = tempSequenceBox.getConnector();
                l.Visibility = Visibility.Hidden;
            }

            if (_hasBlastTag & tempSequenceBox.Center.X < _blastTagCenterPoint.X + 300 & tempSequenceBox.Center.X > _blastTagCenterPoint.X - 300)
            {

                _blastedSequenceBoxes.Add(tempSequenceBox);//save blasted sequences to a list for later access
                tempSequenceBox.Center = new Point(_blastTagCenterPoint.X - 150 + generator.Next(300), _blastTagCenterPoint.Y - 150 + generator.Next(300));
                tempSequenceBox.Height = 100; tempSequenceBox.Width = 150;
                tempSequenceBox.BorderBrush = Brushes.SkyBlue;
                Line l = tempSequenceBox.getConnector();
                l.Visibility = Visibility.Hidden;

                _genePolygon.hide();
                BlastResults.Visibility = Visibility.Visible;
                blastMenu.Visibility = Visibility.Visible;
                tempSequenceBox.Visibility = Visibility.Hidden;
                _blastSequenceBox = new SequenceBox(tempSequenceBox.get_text(), trashSVIs);
                _blastSequenceBox.Center = new Point(_blastTagCenterPoint.X, _blastTagCenterPoint.Y + 100);
                _blastSequenceBox.CanMove = false;
                _blastSequenceBox.Name = "blast_sequence_box";
                results.Items.Add(_blastSequenceBox);
                _systemProcess.Save("Results\\", "CurrSeq", tempSequenceBox.get_textun());
            }
        }

        #endregion

        #region Blast (authors: M. Strait, C. Valdes)

        /// <summary>
        /// Maintains an open ElementMenu
        /// @ M. Strait
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElementMenu_SubmenuClosed(object sender, RoutedEventArgs e)
        {
            ElementMenu em = e.Source as ElementMenu;
            em.IsSubmenuOpen = true;
        }

        /// <summary>
        /// Sets the organism for Blast and starts the process in a seperate thread
        /// @ MS, CV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElementMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ElementMenuItem tempElementMenuItem = sender as ElementMenuItem;

            if (tempElementMenuItem.Header.ToString() == "dog")
            {
                _organismBeingBlasted = "dog";
            }
            else if (tempElementMenuItem.Header.ToString() == "human")
            {
                _organismBeingBlasted = "human";
            }
            else if (tempElementMenuItem.Header.ToString() == "mouse")
            {
                _organismBeingBlasted = "mouse";
            }
            else
            {
                _organismBeingBlasted = "rat";
            }
            _currentLongOperationThread = _progressBarWrapper.execute<Blast>(RunLongBlastOperations, CreateBlastInstance);
            blastMenu.Visibility = Visibility.Hidden;

        }

        /// <summary>
        /// Long Operations for Blast that are called in separate thread by blast element menu handler
        /// @ CV, ML
        /// </summary>
        /// <returns></returns>
        private Blast RunLongBlastOperations()
        {
            _systemProcess.BlastSeq(_organismBeingBlasted);
            return new Blast(_organismBeingBlasted);
        }

        private void seqSVI_TouchDown(object sender, TouchEventArgs e) { currSeqLabel.Content = _currentSequence.Length + " base pairs"; }

        /// <summary>
        /// Creates SVIs with unique coloring of the BlastResults and adds them to the blastResults canvas.
        /// @ MS, CG, CV
        /// </summary>
        /// <param name="s"></param>
        private void CreateBlastInstance(Blast b)
        {
            byte red = new byte(); byte green = new byte(); byte blue = new byte();
            red = 180; green = 220; blue = 0;
            Color c1 = Color.FromRgb(red, green, blue);
            red = 130; green = 160; blue = 0;
            Color c2 = Color.FromRgb(red, green, blue);
            red = 70; green = 100; blue = 0;
            Color c3 = Color.FromRgb(red, green, blue);

            SolidColorBrush brush1 = new SolidColorBrush(c1);
            SolidColorBrush brush2 = new SolidColorBrush(c2);
            SolidColorBrush brush3 = new SolidColorBrush(c3);

            //This is the org image in the middle of the "flower" of BLAT results. 
            ScatterViewItem orgSVI = new ScatterViewItem();
            orgSVI.Height = 100; orgSVI.Width = 100;
            orgSVI.CanMove = false; orgSVI.CanRotate = false; orgSVI.CanScale = false;
            orgSVI.Center = new Point(125, 125);
            Image orgImg = new Image();
            orgImg.Source = new BitmapImage(new Uri("Resources\\org_" + _organismBeingBlasted + ".jpg", UriKind.Relative));
            orgSVI.Content = orgImg;

            if (_organismBeingBlasted.Equals("dog"))
            {
                dogSVI.Visibility = Visibility.Visible;
                dogRes.Items.Add(orgSVI);
            }
            else if (_organismBeingBlasted.Equals("human"))
            {
                humanSVI.Visibility = Visibility.Visible;
                humanRes.Items.Add(orgSVI);
            }
            else if (_organismBeingBlasted.Equals("mouse"))
            {
                mouseSVI.Visibility = Visibility.Visible;
                mouseRes.Items.Add(orgSVI);
            }
            else
            {
                ratSVI.Visibility = Visibility.Visible;
                ratRes.Items.Add(orgSVI);
            }

            AddBlastResultsToBlastCanvas(b, _organismBeingBlasted, brush1, brush2, brush3);

        }

        //MEGAN!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        /// <summary>
        /// Creates the flower visualization of Blast results
        /// @ MS
        /// </summary>
        /// <param name="b">Blast that holds all the data</param>
        /// <param name="org">Organism that was blasted against for the image of the results</param>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <param name="b3"></param>
        private void AddBlastResultsToBlastCanvas(Blast b, String org, SolidColorBrush b1, SolidColorBrush b2, SolidColorBrush b3)
        {
            List<BlastResult> blast_results = b.getBlasts();
            int xCoord = 125, yCoord = 125;     // center point of the results display

            for (int i = 0; i < _numberOfBlastResultsInFlower; i++)
            {
                ErasableScatterViewItem svi = new ErasableScatterViewItem(trashSVIs);
                svi.CanScale = false;
                svi.Name = org;

                svi.Background = b1; svi.Opacity = 1;

                svi.Content = blast_results[i].toString();

                int orient = blast_results[i].getOrient();
                if (orient == 0) { svi.Center = new Point(xCoord + 120, yCoord); }
                else if (orient == 45) { svi.Center = new Point(xCoord + 80, yCoord + 80); }
                else if (orient == 90) { svi.Center = new Point(xCoord, yCoord + 120); }
                else if (orient == 135) { svi.Center = new Point(xCoord - 80, yCoord + 80); }
                else if (orient == 180) { svi.Center = new Point(xCoord - 120, yCoord); }
                else if (orient == 225) { svi.Center = new Point(xCoord - 80, yCoord - 80); }
                else if (orient == 270) { svi.Center = new Point(xCoord, yCoord - 120); }
                else { svi.Center = new Point(xCoord + 80, yCoord - 80); }

                svi.Orientation = orient;
                svi.TouchDown += new EventHandler<TouchEventArgs>(AddBlastResultToMainView);

                if (org == "dog")
                {
                    dogRes.Items.Add(svi);
                }
                else if (org == "human")
                {
                    humanRes.Items.Add(svi);
                }
                else if (org == "mouse")
                {
                    mouseRes.Items.Add(svi);
                }
                else
                {
                    ratRes.Items.Add(svi);
                }
            }
            blastMenu.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Adds an instance of a BlastResult as a SVI to the ExonScatter layer.
        /// @ MS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool is_clicked = false;

        private void AddBlastResultToMainView(object sender, TouchEventArgs e)
        {
            ErasableScatterViewItem it1 = e.Source as ErasableScatterViewItem;
            Image img = new Image();
            img.Source = new BitmapImage(new Uri("Resources\\org_" + it1.Name + ".jpg", UriKind.Relative));

            if (it1.Background == Brushes.CadetBlue) { is_clicked = true; }
            else { is_clicked = false; }

            if (is_clicked == false)
            {
                it1.Background = Brushes.CadetBlue;
                String length = "";
                int i = 0;
                while (i < it1.Content.ToString().Length)
                {
                    if (it1.Content.ToString().Substring(i, 7) == "Length:")
                    {
                        i += 7;
                        while (!it1.Content.ToString().Substring(i, 1).Equals("b"))
                        { length += it1.Content.ToString().Substring(i, 1); i++; }
                        break;
                    }
                    i++;
                }
                double percentLength = Convert.ToInt64(length) * 1024 / _currentSequence.Length;

                ScatterViewItem compSVI = new ScatterViewItem();
                compSVI.CanMove = false; compSVI.CanRotate = false; compSVI.CanScale = false;
                compSVI.Background = Brushes.Gray; compSVI.Opacity = .5;
                compSVI.Height = 20; compSVI.Width = percentLength;
                compSVI.Center = new Point(percentLength / 2, 695);
                compSVI.Content = img;

                results.Items.Add(compSVI);
                is_clicked = true;
            }
            else
            {
                ErasableScatterViewItem temp = new ErasableScatterViewItem(trashSVIs);
                temp.CanScale = false;
                Canvas tempCanvas = new Canvas();
                tempCanvas.Background = Brushes.Gray;
                Label tempLabel = new Label();
                tempLabel.Content = it1.Content.ToString();
                img.Opacity = .5;
                tempCanvas.Children.Add(img);
                tempCanvas.Children.Add(tempLabel);

                temp.Content = tempCanvas;
                temp.Height = 140; temp.Width = 140;
                img.Height = 140; img.Width = 140;

                if (it1.Name == "dog")
                {
                    dogRes.Items.Remove(it1);
                }
                else if (it1.Name == "human")
                {
                    humanRes.Items.Remove(it1);
                }
                else if (it1.Name == "mouse")
                {
                    mouseRes.Items.Remove(it1);
                }
                else
                {
                    ratRes.Items.Remove(it1);
                }

                ExonScatter.Items.Add(temp);
                //mainSVIList.Add(temp);//----------------------------------------------------------------------------------------------
                //is_clicked = false;
            }
        }

        #endregion

        #region Gene Info (Expression, Ontology, Publications) (authors: M. Strait, M. Lintz, C. Valdes, C. Grevet, S. Elfenbein)

        /// <summary>
        /// Changes the position of the gene info element menu based on the user's contact position. The values for the orientation "feel right."
        /// Should they need to be changed I would suggest going up or down by intervals of 5 degrees until the menu is not cut off and relatively centered.
        /// @ C. Valdes
        /// </summary>
        /// <param name="x">The x coordinate of the user's contact</param>
        private void positionGeneMenu(double x)
        {
            TranslateTransform trs;
            if (_focusingOnGeneInTopRow)
            {
                trs = new TranslateTransform((x - 20) + _scrollOffset, 30);

                if (x < 200)
                    _geneElementMenu.Orientation = 160;
                else if (x > 900)
                    _geneElementMenu.Orientation = 240;
                else
                    _geneElementMenu.Orientation = 200;
            }
            else
            {
                trs = new TranslateTransform((x - 20) + _scrollOffset, 100);
                if (x < 200)
                    _geneElementMenu.Orientation = 60;
                else if (x > 900)
                    _geneElementMenu.Orientation = 345;
                else
                    _geneElementMenu.Orientation = 30;
                //makes it a \|/ straight up orientation
            }
            _geneElementMenu.RenderTransform = trs;
        }



        /// <summary>
        /// Generates a new pub abstract window when the user selects a particular publication title.
        /// @ Sarah Elfenbein
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            pub_abstracts = new ErasableScatterViewItem(trashSVIs);
            pub_abstracts.Height = 200;
            pub_abstracts.Width = 300;
            pub_abstracts.BorderThickness = new Thickness(15);
            pub_abstracts.Orientation = 0;
            pub_abstracts.Center = new Point(420, 540);
            pub_abstracts.BorderBrush = Brushes.Gray;
            pub_abstracts.Background = Brushes.White;
            pub_abstracts.Opacity = .75;
            contentArea = new Grid();

            SurfaceListBox s = e.Source as SurfaceListBox;
            content = ((SurfaceListBoxItem)s.SelectedItem).Tag as Grid;
            int sIndex = s.SelectedIndex;

            if (!_currentGeneItem.getAbstractOpen(sIndex))
            {
                /* This sets the instance variable abstractOpen in the GeneItem Class to true so that 
                 *  only one abstract window will open per publication title. A future implementation might set this
                 *  variable back to false when the window is closed or flicked off the screen.     
                 */
                _currentGeneItem.setAbstractOpen(sIndex, true);

                content.Background = Brushes.White;
                content.Margin = new Thickness(20);

                Label abstLabel = new Label();
                abstLabel.Content = _currentGeneItem.geneName();
                abstLabel.Content += "   Abstracts";
                contentArea.Children.Add(abstLabel);

                contentArea.Children.Add(content);

                pub_abstracts.Content = contentArea;
                MainSV.Items.Add(pub_abstracts);
            }
        }

        private void showPublications(PubList pubs)
        {
            if ((_currentGeneItem != null) & !_currentGeneItem.getSelectionOpen(2))
            {
                List<String> links = pubs.getLinks();
                //Create window that holds listing of abstracts
                pub_titles = new ErasableScatterViewItem(trashSVIs);
                pub_titles.Height = 200;
                pub_titles.Width = 300;
                pub_titles.BorderThickness = new Thickness(15);
                pub_titles.BorderBrush = Brushes.Gray;
                pub_titles.Background = Brushes.White;
                pub_titles.Opacity = .90;
                pub_titles.Orientation = 0;
                pub_titles.Center = new Point(420, 540);
                pub_titles.Visibility = Visibility.Visible;
                SurfaceListBox surfaceListBox = new SurfaceListBox();
                Label label = new Label();
                Grid grid = new Grid();
                grid.Background = Brushes.White;
                grid.Opacity = 90.00;
                surfaceListBox.Background = Brushes.White;
                Thickness sMargin = new Thickness(22.5);
                surfaceListBox.Margin = sMargin;
                label.Content = _currentGeneItem.geneName();
                label.Content += "  Related Articles in PubMed (via Entrez Gene): " + Math.Min(links.Count, 10) + " of " + links.Count + " shown";
                grid.Children.Add(label);
                grid.Children.Add(surfaceListBox);
                pub_titles.Content = grid;
                Label abstLabel = new Label();
                abstLabel.Content = _currentGeneItem.geneName();
                abstLabel.Content += "   Abstracts";


                //PubWindow myPubs = new PubWindow(surfaceListBox, label, pubs);
                /*_screen1 = contentSelector;
                _titles = myPubList.getTitles();
                _links = myPubList.getLinks();
                _id = myPubList.getGeneID();*/

                //_pubNum = publicationNum;
                //foreach (string title in titles)

                Func<List<String>> getAbstracts = delegate()
                {
                    List<string> abstracts = new List<string>();
                    for (int i = 0; i < Math.Min(links.Count, 10); i++)
                    {
                        PubAbstract pubAbstract = new PubAbstract(links.ElementAt(i), pubs.getAuthors());
                        abstracts.Add(pubAbstract.getAbstract());
                    }
                    return abstracts;
                };

                Action<List<String>> displayPublicationWindow = delegate(List<String> abstracts)
                {
                    for (int i = 0; i < Math.Min(links.Count, 10); i++)
                    {

                        SurfaceListBoxItem itemA = new SurfaceListBoxItem();
                        String abstractText = pubs.getTitles().ElementAt(i) + "\n\n";

                        if (pubs.getAuthors().Count < 0)
                            abstractText = abstractText + "Authors: Unavailable";
                        else
                            abstractText = abstractText + "Authors: " + pubs.getAuthors().ElementAt(i);

                        abstractText = abstractText + "Abstract:\n" + abstracts.ElementAt(i);

                        itemA.Content = (i + 1) + ". " + pubs.getTitles()[i];

                        Grid g1 = new Grid();
                        SurfaceTextBox tb1 = new SurfaceTextBox();

                        tb1.TextWrapping = TextWrapping.Wrap;
                        tb1.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                        tb1.IsReadOnly = true;
                        tb1.Text = abstractText;
                        g1.Children.Add(tb1);

                        itemA.Tag = g1;
                        surfaceListBox.Items.Add(itemA);

                    }
                    surfaceListBox.SelectionChanged += new SelectionChangedEventHandler(OnSelectionChanged); //This method displays abstracts (see above)

                    MainSV.Items.Add(pub_titles);
                };

                _progressBarWrapper.execute<List<String>>(getAbstracts, displayPublicationWindow);
            }
        }

        private void showOntology(Ontology ontology)
        {
            //System.Threading.Thread.Sleep(5000);
            if ((_currentGeneItem != null) & !_currentGeneItem.getSelectionOpen(1))
            {
                ErasableScatterViewItem ontologyAreaScatt = new ErasableScatterViewItem(trashSVIs);
                ontologyAreaScatt.Height = 200;
                ontologyAreaScatt.Width = 300;
                ontologyAreaScatt.BorderThickness = new Thickness(15);
                ontologyAreaScatt.BorderBrush = Brushes.Gray;
                ontologyAreaScatt.Background = Brushes.White;
                ontologyAreaScatt.Opacity = .90;
                ontologyAreaScatt.Orientation = 0;

                Label lb = new Label();
                Grid g = new Grid();
                //g.Width = 300;
                //g.Height = 200;
                string header = _currentGeneItem.geneName();
                header += "  Gene Summary from NCBI Entrez Gene";
                lb.Content = header;
                g.Children.Add(lb);

                ontology.displayOntology(g);
                ontologyAreaScatt.Content = g;
                MainSV.Items.Add(ontologyAreaScatt);

                /* This sets the instance variable selectionOpen in the GeneItem Class to true so that 
                 *  only one ontology window will open per gene */
                //itemGene.setSelectionOpen(1, true);
            }
        }

        private void showProgressBar()
        {
            ProgressIndicator.Visibility = Visibility.Visible;
        }

        private void hideProgressBar()
        {
            ProgressIndicator.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Tag Handlers (authors: M. Strait, C. Valdes, C. Grevet, C. Fan)

        /// <summary>
        /// Recognizes tags placed on the Surface.
        /// @ CF, MS, CV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisualizationAdded(object sender, TagVisualizerEventArgs e)
        {
            //int val = e.TagVisualization.VisualizedTag.Byte.Value;

            //if (val == BlastTag.Value) // blast
            //{
            //    _hasBlastTag = true;
            //    _blastTagCenterPoint = new Point(e.TagVisualization.Center.X, e.TagVisualization.Center.Y);
            //}

            //if (val == TubeTag.Value)  // save
            //{
            //    hasTestTubeTag = true;
            //    testTubePoint = e.TagVisualization.Center;
            //    if (!_savedItemsVisibilityCheck)//For the case when user has "saved", left and comes back
            //        _savedItemsVisibilityCheck = true;
            //    changeItemVisibility();
            //}

            //if (val == PrintTag.Value)  // print
            //{
            //    hasPrintTag = true;
            //    PrintTag_Center = new Point(e.TagVisualization.Center.X, e.TagVisualization.Center.Y);
            //}

            //if (val == TrashTag.Value)  // trash
            //{ _hasTrashTag = true; trash.Visibility = Visibility.Visible; }

            //if (val == SysAdmin.Value)
            //{
            //    String contact = "";
            //    SystemProcess process = new SystemProcess();
            //    process.Open("Logs\\", "temp");

            //    for (int i = 0; i < contacts_log.Count; i++)
            //    {
            //        contact = i + "\t" + contacts_log.ElementAt(i);
            //        process.WriteLine(contact);
            //    }
            //    process.Close();
            //}

        }

        /// <summary>
        /// Recognizes tags removed from Surface
        /// @ CF, MS, CV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewVisualizationRemoved(object sender, TagVisualizerEventArgs e)
        {
            int val = (int)e.TagVisualization.VisualizedTag.Value;

            if (val == BlastTag.Value)
            {
                _hasBlastTag = false;
                BlastResults.Visibility = Visibility.Hidden;
                _genePolygon.show();
                blastMenu.IsSubmenuOpen = false;
                blastMenu.Visibility = Visibility.Hidden;

                if (_currentLongOperationThread != null)
                    _currentLongOperationThread.Abort();
                ProgressIndicator.Visibility = Visibility.Hidden;

                //retrieve blasted sequence box to take back to main canvas
                for (int i = 0; i < _blastedSequenceBoxes.Count; i++)
                {
                    SequenceBox s = _blastedSequenceBoxes.ElementAt(i);
                    _blastedSequenceBoxes.RemoveAt(i);//remove from blasted sequence list or this loop will loop incorrectly

                    //remove sequenceBox from previous parent ScatterView otherwise will throw exception
                    ScatterView sParent = (ScatterView)s.Parent;
                    sParent.Items.Remove(s);

                    //format to default sequence box state
                    s.BorderBrush = Brushes.Gray;
                    s.Width = 350;
                    s.Height = 200;
                    Line l = s.getConnector();
                    l.X2 = s.Center.X;
                    l.Y2 = s.Center.Y - (s.Height / 2);
                    l.Visibility = Visibility.Visible;
                    s.Visibility = Visibility.Visible;
                    //add to main canvas again
                    MainSV.Items.Add(s);
                }
            }

            // Test tube tag
            if (val == TubeTag.Value)
            {
                hasTestTubeTag = false;
                _savedItemsVisibilityCheck = false;
                changeItemVisibility();
            }

            if (val == PrintTag.Value)
            {
                for (int i = 0; i < _printedSequenceBoxes.Count; i++)
                {
                    SequenceBox s = _printedSequenceBoxes.ElementAt(i);
                    s.BorderBrush = Brushes.Gray;
                    s.Width = 350;
                    s.Height = 200;
                    Line l = s.getConnector();
                    l.X2 = s.Center.X;
                    l.Y2 = s.Center.Y - (s.Height / 2);
                    l.Visibility = Visibility.Visible;
                }

                for (int i = 0; i < _printedSequenceBoxes.Count; i++)
                {
                    SequenceBox s = _printedSequenceBoxes.ElementAt(i);
                    _printedSequenceBoxes.RemoveAt(i);
                }
                hasPrintTag = false;
            }

            // Trash tag
            if (val == TrashTag.Value)
            {
                _hasTrashTag = false; trash.Visibility = Visibility.Hidden;
            }

        }

        /// <summary>
        /// Recognizes movement of tags on Surface
        /// @ CF + MS
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewVisualizationMoved(object sender, TagVisualizerEventArgs e)
        {
            if (_hasBlastTag)
            {
                _blastTagCenterPoint = new Point(e.TagVisualization.Center.X, e.TagVisualization.Center.Y);
                TranslateTransform blastCenter = new TranslateTransform(_blastTagCenterPoint.X - 30, _blastTagCenterPoint.Y - 30);
                blastMenu.RenderTransform = blastCenter;
            }

            if (hasTestTubeTag)// & itemsVisible)
            {
                testTubePoint = new Point(e.TagVisualization.Center.X, e.TagVisualization.Center.Y);
            }

            if (hasPrintTag)
            {
                PrintTag_Center = new Point(e.TagVisualization.Center.X, e.TagVisualization.Center.Y);
            }

            if (_hasTrashTag)
            {
                _trashTagCenterPoint = new Point(e.TagVisualization.Center.X, e.TagVisualization.Center.Y);
            }

        }

        /// <summary>
        /// This method will make the center of the SequenceBox specified be the point specified. This can simulate the docking 
        /// feature implemented throughout the program. It also gets rid of the pesky connector for you.
        /// @ C. Valdes
        /// </summary>
        /// <param name="scatterViewItem">The scatterViewItem os the item to be docked.</param>
        /// <param name="point">The point can be the center of a tag viz or the center of a spotlight for gene switch docking</param>
        private void DockSeqeunceBox(SequenceBox sequenceBox, Point point)
        {
            sequenceBox.Center = point;
            sequenceBox.Height = 100; //sequenceBox.Width = 100;
            //sequenceBox.BorderBrush = Brushes.Black;
            Line l = sequenceBox.getConnector();
            l.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// This method will make the center of the SVI specified be the point specified. This can simulate the docking 
        /// feature implemented throughout the program. 
        /// @ C. Valdes
        /// </summary>
        /// <param name="scatterViewItem">The scatterViewItem os the item to be docked.</param>
        /// <param name="point">The point can be the center of a tag viz or the center of a spotlight for gene switch docking</param>
        private void DockSVI(ScatterViewItem scatterViewItem, Point point)
        {
            scatterViewItem.Center = point;
            scatterViewItem.Height = 100; //scatterViewItem.Width = 100;
            //scatterViewItem.BorderBrush = Brushes.Gray;
        }

        /// <summary>
        /// This method makes all the items for a gene visible or invisible depending on whether the
        /// gene item is placed on the surface or removed.
        /// @ Chloe Fan, Consuelo Valdes 3/22/10
        /// </summary>
        public void changeItemVisibility()
        {
            if (mainSVIList.Count != 0)
            {
                foreach (ErasableScatterViewItem scatt in mainSVIList)
                {
                    if (scatt.Name != "searchmenu" && scatt.Name != "exonInt")
                    {
                        if (_savedItemsVisibilityCheck)
                        {
                            scatt.Visibility = Visibility.Visible;
                            scatt.unerase();
                        }
                        else
                        {
                            scatt.Visibility = Visibility.Hidden;
                            scatt.erase();
                        }
                    }
                }
            }
        }


        #endregion
    }
}