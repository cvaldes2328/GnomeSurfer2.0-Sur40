using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace GnomeSurfer2
{
    class GeneExpansionPolygon
    {
        private GeneItem selectedGeneItem;
        private Polygon polygon;
        private Point rightBasePoint;
        private Point leftBasePoint;
        private Canvas parent;

        public GeneExpansionPolygon(Canvas parent, Point leftBasePoint, Point rightBasePoint)
        {
            this.polygon = new Polygon();
            this.polygon.Fill = Brushes.DodgerBlue;
            this.polygon.Opacity = 0.25;
            this.leftBasePoint = leftBasePoint;
            this.rightBasePoint = rightBasePoint;
            this.parent = parent;
            polygon.Visibility = Visibility.Collapsed;
            parent.Children.Add(polygon);
        }

        public void setSelectedGeneItem(GeneItem selectedGeneItem)
        {
            this.selectedGeneItem = selectedGeneItem;
        }

        public void draw(double scrollOffset)
        {
            if (selectedGeneItem == null)
            {
                return;
            }
            PointCollection points = new PointCollection();
            double upperleftX = selectedGeneItem.LeftXCoordinate - scrollOffset;
            double upperrightX = selectedGeneItem.RightXCoordinate - scrollOffset;
            if (selectedGeneItem.getGene().getOrientation() == "+")
            {
                upperrightX -= ChromosomeBar.TriangleLength;
            }
            else
            {
                upperleftX += ChromosomeBar.TriangleLength;
            }
            points.Add(new Point(upperleftX, selectedGeneItem.BottomYCoordinate + 107));
            points.Add(new Point(upperrightX, selectedGeneItem.BottomYCoordinate + 107));
            points.Add(rightBasePoint);
            points.Add(leftBasePoint);
            polygon.Points = points;
            show();
        }

        public void hide() 
        {
            polygon.Visibility = Visibility.Collapsed;
        }

        public void show() 
        {
            polygon.Visibility = Visibility.Visible;
        }
    }
}
