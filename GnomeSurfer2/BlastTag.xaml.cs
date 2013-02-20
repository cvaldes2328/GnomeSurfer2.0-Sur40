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

namespace SurfaceApplication1
{
    /// <summary>
    /// Interaction logic for BlastTag.xaml
    /// </summary>
    public partial class BlastTag : TagVisualization
    {
        public BlastTag()
        {
            InitializeComponent();
        }

        private void BlastTag_Loaded(object sender, RoutedEventArgs e)
        {
            //TODO: customize BlastTag's UI based on this.VisualizedTag here
        }
    }
}
