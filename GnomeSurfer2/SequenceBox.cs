using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
using Microsoft.Surface.Presentation;
using System.Windows.Controls;
using System.Windows.Input;

namespace GnomeSurfer2
{
    class SequenceBox : ErasableScatterViewItem
    {
        private Line connector;
        private Canvas connectorParent;
        private String text_formatted;
        private String text_unformatted;

        public SequenceBox(
            Point exintContact,
            String name,
            String text_f,
            String text_un,
            ScatterView trashScatterView)
            : base(trashScatterView)
        {
            this.connector = new Line();
            connector.X1 = exintContact.X + 20;
            connector.Y1 = 360;
            connector.Stroke = Brushes.Black;
            connector.Opacity = 0.25;
            connector.StrokeThickness = 5;

            this.Name = name;
            this.Width = 350;
            this.Height = 200;
            this.CanScale = true;
            this.CanRotate = false;
            this.BorderThickness = new Thickness(15);
            this.BorderBrush = Brushes.Gray;
            this.Background = Brushes.White;
            this.Opacity = .75;

            this.ContainerManipulationDelta += new ContainerManipulationDeltaEventHandler(updateConnector);

            SurfaceScrollViewer viewer = new SurfaceScrollViewer();
            Canvas can = new Canvas();
            SurfaceTextBox textBox = new SurfaceTextBox();
            textBox.TextWrapping = TextWrapping.Wrap;
            textBox.IsReadOnly = true;
            textBox.Text = text_f;
            this.text_formatted = text_f;
            this.text_unformatted = text_un;
            textBox.Width = 300; textBox.Height = 3200;
            textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            can.Children.Add(textBox);

            Viewbox view = new Viewbox();
            view.Height = Double.NaN;
            view.Width = Double.NaN;
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
            this.Content = viewer;
            this.Center = new Point(512, 480);

            connector.X2 = this.Center.X;
            connector.Y2 = this.Center.Y - this.Height / 2;
        }

        public SequenceBox(String text_f, ScatterView trashScatterView) : base(trashScatterView)
        {
            this.Width = 200;
            this.Height = 100;
            this.CanScale = true;
            this.CanRotate = false;
            this.BorderThickness = new Thickness(15);
            this.BorderBrush = Brushes.DodgerBlue;
            this.Background = Brushes.White;
            this.Opacity = .75;

            SurfaceScrollViewer viewer = new SurfaceScrollViewer();
            Canvas can = new Canvas();
            SurfaceTextBox textBox = new SurfaceTextBox();
            textBox.TextWrapping = TextWrapping.Wrap;
            textBox.IsReadOnly = true;
            textBox.Text = text_f;
            this.text_formatted = text_f;
            textBox.Width = 300; textBox.Height = 3200;
            textBox.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            can.Children.Add(textBox);

            Viewbox view = new Viewbox();
            view.Height = Double.NaN;
            view.Width = Double.NaN;
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
            this.Content = viewer;
        }

        public void updateConnector(object sender, ContainerManipulationDeltaEventArgs e)
        {
            this.connector.X2 = this.Center.X;
            this.connector.Y2 = this.Center.Y - (this.Height / 2);
            //if(SurfaceWindow.)
        }

        public Line getConnector()
        {
            return connector;
        }

        public String get_text()
        {
            return text_formatted;
        }

        public String get_textun()
        {
            return text_unformatted;
        }

        private void updateConnector(object sender, TouchEventArgs e)
        {
            this.connector.X2 = this.Center.X;
            this.connector.Y2 = this.Center.Y - (this.Height / 2);
        }

        protected override void onErase() {
            this.connectorParent = connector.Parent as Canvas;
            this.connectorParent.Children.Remove(this.connector);
        }

        protected override void onUnerase()
        {
            this.connectorParent.Children.Add(this.connector);
        }
    }
}

