using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GnomeSurfer2
{
    class ExpressionControl : ErasableScatterViewItem
    {
        private const string _allTissueLabel = "All Tissue";

        protected UIElement MainPanel
        {
            get
            {
                return _mainSlideOutControl.MainPanelContent;
            }
            set
            {
                _mainSlideOutControl.MainPanelContent = value;
            }
        }
        private TissueExpressionModelCollection _expressionModels;
        private SlideOutControl _mainSlideOutControl;

        public ExpressionControl(ScatterView trashSV)
            : base(trashSV)
        {
            this._expressionModels = new TissueExpressionModelCollection();
            this._mainSlideOutControl = new SlideOutControl();
            this.Content = _mainSlideOutControl;
            this.Style = (Style)App.Current.FindResource("ScatterViewItemBindToContentSize");
        }

        private static Brush _expressionValueToBrush(double expressionValue)
        {
            const byte minimumColorComponentValue = 0;
            const byte maximumColorComponentValue = 255;
            if (expressionValue > 1 || expressionValue < -1)
            {
                throw new ArgumentException("Expression values must be between -1 and 1");
            }
            
            byte redComponentValue = minimumColorComponentValue;
            byte greenComponentValue = minimumColorComponentValue;
            byte blueComponentValue = minimumColorComponentValue;
            if (expressionValue > 0)
            {
                greenComponentValue = (byte)(maximumColorComponentValue * expressionValue);
            }
            else
            {
                redComponentValue = (byte)(maximumColorComponentValue * Math.Abs(expressionValue));
            }
            Color expressionColor = Color.FromRgb(redComponentValue, greenComponentValue, blueComponentValue);
            return new SolidColorBrush(expressionColor);
        }

        protected void AddTissueExpression(string tissueGroup, string label, 
            double expressionValue, string imageName) 
        {
            _expressionModels.Add(tissueGroup, label, expressionValue, imageName);
        }

        protected void AddTissueExpression(string tissueGroup, string label,
            double expressionValue)
        {
            this.AddTissueExpression(tissueGroup, label, expressionValue, null);
        }

        protected void DisplayAllTissueGroups()
        {
            StackPanel mainPanel = new StackPanel();
            Grid bodyPanel = new Grid();
            ColumnDefinition leftColumnDefinition = new ColumnDefinition();
            leftColumnDefinition.Width = new GridLength(_mainSlideOutControl.ExpandedSidePanelWidth / 2);
            ColumnDefinition rightColumnDefinition = new ColumnDefinition();
            rightColumnDefinition.Width = new GridLength(_mainSlideOutControl.ExpandedSidePanelWidth / 2);
            bodyPanel.ColumnDefinitions.Add(leftColumnDefinition);
            bodyPanel.ColumnDefinitions.Add(rightColumnDefinition);

            StackPanel leftColumn = new StackPanel();
            StackPanel rightColumn = new StackPanel();
            Grid.SetColumn(leftColumn, 0);
            Grid.SetColumn(rightColumn, 1);
            bodyPanel.Children.Add(leftColumn);
            bodyPanel.Children.Add(rightColumn);

            List<TissueExpressionModel> allTissueExpressionModels = _expressionModels.All;
            for (int i = 0; i < allTissueExpressionModels.Count; i++)
            {
                TissueExpressionModel model = allTissueExpressionModels.ElementAt(i);
                DockPanel row = _makeTissueExpressionRow(model);
                if (i % 2 == 0)
                {
                    leftColumn.Children.Add(row);
                }
                else
                {
                    rightColumn.Children.Add(row);
                }
            }
            UIElement header = MakeHeader(_allTissueLabel);
            mainPanel.Children.Add(header);
            mainPanel.Children.Add(bodyPanel);
            _mainSlideOutControl.SidePanelContent = mainPanel;
        }

        protected void DisplayTissueGroup(String tissueGroup) 
        {
            if (!_expressionModels.ContainsTissueGroup(tissueGroup)) 
            {
                throw new ArgumentException("No expression information for " + tissueGroup + " has been added");
            }
            StackPanel newSidePanelContent = new StackPanel();
            newSidePanelContent.Style = (Style)App.Current.FindResource("SidebarStackPanel");

            UIElement header = MakeHeader(tissueGroup);
            newSidePanelContent.Children.Add(header);
            List<TissueExpressionModel> tissueExpressionModels = _expressionModels.GetTissueGroup(tissueGroup);
            foreach (TissueExpressionModel model in tissueExpressionModels)
            {
                if (model.ImageName != null)
                {
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri("Resources\\" + model.ImageName, UriKind.Relative));
                    image.Style = (Style)App.Current.FindResource("SidebarImage");
                    newSidePanelContent.Children.Add(image);
                }
                DockPanel dockPanel = _makeTissueExpressionRow(model);
                newSidePanelContent.Children.Add(dockPanel);
            }
            
            _mainSlideOutControl.SidePanelContent = newSidePanelContent;
        }

        private UIElement MakeHeader(String tissueGroup)
        {
            Label titleLabel = new Label();
            titleLabel.Content = tissueGroup;
            titleLabel.Style = (Style)App.Current.FindResource("SidebarTitle");
            SurfaceButton slideInControl = new SurfaceButton();
            slideInControl.TouchUp += delegate(object sender, TouchEventArgs e)
                {
                    _mainSlideOutControl.SidePanelContent = null;
                };
            slideInControl.Content = "<<";
            DockPanel header = new DockPanel();
            header.LastChildFill = false;
            DockPanel.SetDock(slideInControl, Dock.Right);
            DockPanel.SetDock(titleLabel, Dock.Left);
            header.Children.Add(titleLabel);
            header.Children.Add(slideInControl);
            return header;
        }

        private static DockPanel _makeTissueExpressionRow(TissueExpressionModel model)
        {
            DockPanel dockPanel = new DockPanel();
            dockPanel.Style = (Style)App.Current.FindResource("SidebarDockPanel");
            Ellipse ellipse = new Ellipse();
            ellipse.Style = (Style)App.Current.FindResource("SidebarEllipse");
            ellipse.Fill = _expressionValueToBrush(model.ExpressionValue);
            dockPanel.Children.Add(ellipse);
            Label label = new Label();
            label.Style = (Style)App.Current.FindResource("SidebarLabel");
            label.Content = model.Label;
            dockPanel.Children.Add(label);
            return dockPanel;
        }

        private class TissueExpressionModel
        {
            public String Label;
            public Double ExpressionValue;
            public String ImageName;

            public TissueExpressionModel(String label, Double expressionValue,
                String imageName)
            {
                this.Label = label;
                this.ExpressionValue = expressionValue;
                this.ImageName = imageName;
            }

            public TissueExpressionModel(String label, Double expressionValue)
                : this(label, expressionValue, null)
            {
            }
        }

        private class TissueExpressionModelCollection
        {
            private Dictionary<string, List<TissueExpressionModel>> _tissueExpressionDictionary;
            private List<TissueExpressionModel> _allTissueExpressionModels;

            public List<TissueExpressionModel> All
            {
                get
                {
                    return _allTissueExpressionModels;
                }
            }

            public TissueExpressionModelCollection()
            {
                _tissueExpressionDictionary = new Dictionary<string, List<TissueExpressionModel>>();
                _allTissueExpressionModels = new List<TissueExpressionModel>();
            }

            public bool ContainsTissueGroup(string tissueGroup)
            {
                return _tissueExpressionDictionary.ContainsKey(tissueGroup);
            }

            public void Add(string tissueGroup, string label, double expressionValue, string imageName)
            {
                if (!_tissueExpressionDictionary.ContainsKey(tissueGroup))
                {
                    _tissueExpressionDictionary.Add(tissueGroup, new List<TissueExpressionModel>());
                }
                TissueExpressionModel model = new TissueExpressionModel(label, expressionValue, imageName);
                _tissueExpressionDictionary[tissueGroup].Add(model);
                _allTissueExpressionModels.Add(model);
            }

            public void Add(string tissueGroup, string label, double expressionValue)
            {
                this.Add(tissueGroup, label, expressionValue, null);
            }

            public List<TissueExpressionModel> GetTissueGroup(String group)
            {
                return _tissueExpressionDictionary[group];
            }
        }

    }
}
