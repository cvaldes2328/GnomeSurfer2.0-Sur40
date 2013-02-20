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
using Microsoft.Surface.Presentation.Controls;

namespace GnomeSurfer2
{
    /// <summary>
    /// Interaction logic for MouseExpressionMainPanel.xaml
    /// </summary>
    public partial class MouseExpressionMainPanel : UserControl
    {
        public MouseExpressionMainPanel(Gene gene)
        {
            InitializeComponent();
            GeneNameLabel.Content = gene.GetName();
        }
    }
}
