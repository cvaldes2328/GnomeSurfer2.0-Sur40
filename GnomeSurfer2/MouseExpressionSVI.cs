using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GnomeSurfer2
{
    class MouseExpressionSVI : ErasableScatterViewItem
    {

        #region Constants
        /* Height and width of ScatterViewItem */
        private const int _height = 200;
        private const int _width = 340;

        /* Colors for individual expression sites */
        /** Top level expression sites **/
        private readonly Color _headColor = Color.FromRgb(0x00, 0x37, 0x00);
        private readonly Color _chestColor = Color.FromRgb(0x54, 0x00, 0x00);
        private readonly Color _gutColor = Color.FromRgb(0x1C, 0x00, 0x00);
        private readonly Color _germColor = Color.FromRgb(0x0B, 0x00, 0x00);
        private readonly Color _limbColor = Color.FromRgb(0x3D, 0x00, 0x00);
        private readonly Color _lymphColor = Color.FromRgb(0x39, 0x00, 0x00);

        /** Nested expression sites **/
        private readonly Color _fetalBrainColor = Color.FromRgb(0x00, 0x11, 0x00);
        private readonly Color _wholeBrainColor = Color.FromRgb(0x00, 0xD7, 0x00);
        private readonly Color _amygdalaColor = Color.FromRgb(0x00, 0x6D, 0x00);
        private readonly Color _thymusColor = Color.FromRgb(0xB4, 0x00, 0x00);
        private readonly Color _boneMarrowColor = Color.FromRgb(0x0C, 0x00, 0x00);
        private readonly Color _pbCd4TcellsColor = Color.FromRgb(0x69, 0x00, 0x00);
        private readonly Color _skinColor = Color.FromRgb(0x0C, 0x00, 0x00);
        private readonly Color _adipocyteColor = Color.FromRgb(0x8D, 0x00, 0x00);
        private readonly Color _pancreaticIsletsColor = Color.FromRgb(0x07, 0x00, 0x00);
        private readonly Color _heartColor = Color.FromRgb(0x1C, 0x00, 0x00);
        private readonly Color _lungColor = Color.FromRgb(0x10, 0x00, 0x00);
        private readonly Color _kidneyColor = Color.FromRgb(0x00, 0x02, 0x00);
        private readonly Color _liverColor = Color.FromRgb(0x1C, 0x00, 0x00);
        private readonly Color _ovaryColor = Color.FromRgb(0x02, 0x00, 0x00);
        private readonly Color _testisColor = Color.FromRgb(0x0B, 0x00, 0x00);

        /* Labels for individual expression sites */
        private const String _fetalBrainLabel = "fetal brain";
        private const String _wholeBrainLabel = "whole brain";
        private const String _amygdalaLabel = "amygdala";
        private const String _thymusLabel = "thymus";
        private const String _boneMarrowLabel = "bone marrow";
        private const String _pbCd4TcellsLabel = "PB-CD4+ Tcells";
        private const String _skinLabel = "skin";
        private const String _adipocyteLabel = "adipocyte";
        private const String _pancreaticIsletsLabel = "pancreatic islets";
        private const String _heartLabel = "heart";
        private const String _lungLabel = "lung";
        private const String _kidneyLabel = "kidney";
        private const String _liverLabel = "liver";
        private const String _ovaryLabel = "ovary";
        private const String _testisLabel = "testis";

        #endregion

        private GeneItem _geneItem;

        public MouseExpressionSVI(ScatterView trashSV, GeneItem geneItem)
            : base(trashSV)
        {
            this.Background = Brushes.Black;
            this.BorderThickness = new Thickness(10);
            this.CanScale = false;
            this.Opacity = 0.8;
            this._geneItem = geneItem;
            //this.Content = new SlideOutControl();
            /*
            Canvas canvas = new Canvas();
            canvas.Height = _height; 
            canvas.Width = _width;

            Grid grid = new Grid();
            grid.Height = _height; 
            grid.Width = _width;
            grid.Background = Brushes.White;
            canvas.Children.Add(grid);
            this.Content = canvas;

            Label label = new Label();
            label.Background = Brushes.White;
            label.Content = _geneItem.geneName();
            canvas.Children.Add(label);

            Image mouseImage = new Image();
            mouseImage.Source = new BitmapImage(new Uri("Resources/Mouse.png", UriKind.Relative));
            grid.Children.Add(mouseImage);

            ElementMenu head = new ElementMenu();
            ElementMenu chest = new ElementMenu();
            ElementMenu gut = new ElementMenu();
            ElementMenu germ = new ElementMenu();
            ElementMenu limb = new ElementMenu();
            ElementMenu lymph = new ElementMenu();

            List<ElementMenu> elementMenus = new List<ElementMenu>();
            elementMenus.Add(head);
            elementMenus.Add(chest);
            elementMenus.Add(gut);
            elementMenus.Add(germ);
            elementMenus.Add(limb);
            elementMenus.Add(lymph);

            foreach (ElementMenu elementMenu in elementMenus)
            {
                grid.Children.Add(elementMenu);
                elementMenu.ActivationMode = ElementMenuActivationMode.AlwaysActive;
                elementMenu.SubmenuClosed += new RoutedEventHandler(ElementMenu_SubmenuClosed);
                elementMenu.HorizontalAlignment = HorizontalAlignment.Center;
                elementMenu.VerticalAlignment = VerticalAlignment.Top;
            }

            head.Orientation = 0;
            head.RenderTransform = new TranslateTransform(85, 75);

            chest.Orientation = 76;
            chest.RenderTransform = new TranslateTransform(20, 100);

            gut.Orientation = -110;
            gut.RenderTransform = new TranslateTransform(-40, 50);

            germ.Orientation = 135;
            germ.RenderTransform = new TranslateTransform(-70, 80);

            limb.Orientation = 180;
            limb.RenderTransform = new TranslateTransform(60, 120);

            lymph.Orientation = -63;
            lymph.RenderTransform = new TranslateTransform(45, 75);

            head.Items.Add(makeElementMenuItem(_fetalBrainColor, _fetalBrainLabel));
            head.Items.Add(makeElementMenuItem(_wholeBrainColor, _wholeBrainLabel));
            head.Items.Add(makeElementMenuItem(_amygdalaColor, _amygdalaLabel));
            head.Background = new SolidColorBrush(_headColor);

            chest.Items.Add(makeElementMenuItem(_thymusColor, _thymusLabel));
            chest.Items.Add(makeElementMenuItem(_heartColor, _heartLabel));
            chest.Items.Add(makeElementMenuItem(_lungColor, _lungLabel));
            chest.Background = new SolidColorBrush(_chestColor);

            gut.Items.Add(makeElementMenuItem(_pancreaticIsletsColor, _pancreaticIsletsLabel));
            gut.Items.Add(makeElementMenuItem(_kidneyColor, _kidneyLabel));
            gut.Items.Add(makeElementMenuItem(_liverColor, _liverLabel));
            gut.Background = new SolidColorBrush(_gutColor);

            germ.Items.Add(makeElementMenuItem(_ovaryColor, _ovaryLabel));
            germ.Items.Add(makeElementMenuItem(_testisColor, _testisLabel));
            germ.Background = new SolidColorBrush(_germColor);

            limb.Items.Add(makeElementMenuItem(_boneMarrowColor, _boneMarrowLabel));
            limb.Items.Add(makeElementMenuItem(_skinColor, _skinLabel));
            limb.Items.Add(makeElementMenuItem(_adipocyteColor, _adipocyteLabel));
            limb.Background = new SolidColorBrush(_limbColor);

            lymph.Items.Add(makeElementMenuItem(_pbCd4TcellsColor, _pbCd4TcellsLabel));
            lymph.Background = new SolidColorBrush(_lymphColor);
            */
            
        }

        private ElementMenuItem makeElementMenuItem(Color iconColor, String label)
        {
            System.Windows.Shapes.Rectangle rext = new System.Windows.Shapes.Rectangle(); 
            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = iconColor;
            rext.Fill = solidColorBrush;

            ElementMenuItem elementMenuItem = new ElementMenuItem();
            elementMenuItem.Icon = rext;
            elementMenuItem.Header = label;
            return elementMenuItem;
        }
        private void ElementMenu_SubmenuClosed(object sender, RoutedEventArgs e)
        {
            ElementMenu em = e.Source as ElementMenu;
            em.IsSubmenuOpen = true;
        }
    }
}
