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
using System.Xml;
using System.Windows.Markup;
using System.Collections;
using System.Windows.Media.Animation;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace GnomeSurfer2
{
    public partial class SlideOutControl : UserControl
    {
        private readonly TimeSpan _quarterSecondTimeSpan = TimeSpan.FromSeconds(0.25);
        private int _expandedSidePanelWidth = 200;
        private int _collapsedSidePanelWidth = 0;

        public UIElement MainPanelContent
        {
            get
            {
                return _mainPanel.Child;
            }

            set
            {
                _mainPanel.Child = value;
            }
        }

        public UIElement SidePanelContent
        {
            get
            {
                return (UIElement) _sidePanel.Content;
            }

            set
            {
                Storyboard storyboard = _makeSlideStoryboard(value);
                storyboard.Begin(_sidePanel);
            }
        }

        public int ExpandedSidePanelWidth
        {
            get
            {
                return _expandedSidePanelWidth;
            }

            set
            {
                if (_sidePanel.Width == _expandedSidePanelWidth)
                {
                    _sidePanel.Width = value;
                }
                _expandedSidePanelWidth = value;
            }
        }

        public SlideOutControl()
        {
            InitializeComponent();
        }

        public SlideOutControl(int expandedSidePanelWidth)
            : this()
        {
            _expandedSidePanelWidth = expandedSidePanelWidth;
        }

        public SlideOutControl(int expandedSidePanelWidth, int collapsedSidePanelWidth) 
            : this(expandedSidePanelWidth)
        {
            _sidePanel.Width = collapsedSidePanelWidth;
            _collapsedSidePanelWidth = collapsedSidePanelWidth;
        }

        private Storyboard _makeSlideStoryboard(UIElement newSidePanelContent)
        {
            Storyboard slideInStoryboard = new Storyboard();
            DoubleAnimation slideInAnimation = new DoubleAnimation(_collapsedSidePanelWidth, _quarterSecondTimeSpan);
            if (double.IsNaN(_sidePanel.Width))
            {
                slideInAnimation.From = _collapsedSidePanelWidth;
            }
            Storyboard.SetTargetProperty(slideInAnimation, new PropertyPath(Control.WidthProperty));
            slideInStoryboard.Children.Add(slideInAnimation);

            Storyboard slideOutStoryboard = new Storyboard();
            DoubleAnimation slideOutAnimation = new DoubleAnimation(_collapsedSidePanelWidth, _expandedSidePanelWidth, _quarterSecondTimeSpan);
            slideOutAnimation.BeginTime = _quarterSecondTimeSpan;
            Storyboard.SetTargetProperty(slideOutAnimation, new PropertyPath(Control.WidthProperty));
            slideOutStoryboard.Children.Add(slideOutAnimation);

            EventHandler slideInCompletedHandler = delegate(object sender, EventArgs e)
            {
                _sidePanel.Content = newSidePanelContent;
                if (newSidePanelContent != null)
                {
                    slideOutStoryboard.Begin(_sidePanel);
                }
            };

            slideInAnimation.Completed += slideInCompletedHandler;
            return slideInStoryboard;
        }
    }
}
