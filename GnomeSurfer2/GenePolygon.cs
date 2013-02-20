using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Surface.Presentation.Controls;

namespace GnomeSurfer2
{
    class GenePolygon
    {
        private GeneItem selectedGeneItem;
        private Polygon polygon;
        private Point leftBasePoint, rightBasePoint, leftTopPoint, rightTopPoint;
        private PointCollection points;
        private Canvas parent;

        public GenePolygon(Canvas parent, Point leftBasePoint, Point rightBasePoint)
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
            points = new PointCollection();
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

            rightTopPoint = new Point(upperrightX, selectedGeneItem.BottomYCoordinate + 107);
            leftTopPoint = new Point(upperleftX, selectedGeneItem.BottomYCoordinate + 107);

            points.Add(leftTopPoint);
            points.Add(rightTopPoint);
            points.Add(rightBasePoint);
            points.Add(leftBasePoint);
            polygon.Points = points;
            show();
        }

        public PointCollection GetPoints()
        {
            return points;
        }

        public void hide() 
        {
            polygon.Visibility = Visibility.Collapsed;
        }

        public void show() 
        {
            if (selectedGeneItem == null)
            {
                return;
            }
            polygon.Visibility = Visibility.Visible;
        }
    }
}
