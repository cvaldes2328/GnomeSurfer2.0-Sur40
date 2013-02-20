/* FILE:        ErasableScatterViewItem.cs
 * AUTHORS:     Mikey Lintz, Consuelo Valdes
 * MODIFIED:    22 Mar 2010
 * 
 * DESCRIPTION: Implements IErasable to create erasable SVIs if the eraser tag touches them or "saves" them if the
 * testube tag is on the surface.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Windows.Input;

namespace GnomeSurfer2
{
    class ErasableScatterViewItem : ScatterViewItem, IErasable
    {
        private ScatterView _trashScatterView;
        private ScatterView _parentScatterView;
        private bool _docked;

        public ErasableScatterViewItem(ScatterView trashScatterView)
            : base()
        {
            
            this.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(uneraseOnContact);
            this.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(ErasableScatterViewItem_TouchDown);
            this.ContainerManipulationCompleted += new ContainerManipulationCompletedEventHandler(manipulateErasableItem); 
            this._trashScatterView = trashScatterView;
        }



        void ErasableScatterViewItem_TouchDown(object sender, TouchEventArgs e)
        {
            //if (SurfaceWindow1.hasTestTubeTag)
            //{
            //    if (this.Center.X == SurfaceWindow1.testTubePoint.X)
            //    {
            //        _docked = true;
            //    }
            //}
            
            //if (!e.Contact.IsTagRecognized)
            //{
            //    return;
            //}
            //if (e.Contact.Tag.Type == TagType.Byte && e.Contact.Tag.Byte.Value == SurfaceWindow1.ERASER_TAG_VALUE)
            //{
            //    this.erase();
            //}
            
        }
    
        public void erase()
        {
            ScatterView parent = this.Parent as ScatterView;
            if (parent == _trashScatterView)
            {
                return;
            }
            parent.Items.Remove(this);
            _trashScatterView.Items.Add(this);
            this._parentScatterView = parent;
            onErase();
            
        }

        protected virtual void onErase() { } // Hook method for descendants of ErasableScatterViewItem

        public void unerase()
        {
            if (this._trashScatterView == null)
            {
                return;
            }
            if (!this._trashScatterView.Items.Contains(this))
            {
                return;
            }
            this._trashScatterView.Items.Remove(this);
            this._parentScatterView.Items.Add(this);
            onUnerase();
        }

        protected virtual void onUnerase() { } // Hook method for descendants of ErasableScatterViewItem

        private void uneraseOnContact(object sender, TouchEventArgs e)
        {
            this.unerase();
        }

        //Docks scatt if TestTube tag is on screen and overlaps with scatt. Also checks if out of screen range and deletes, if so.
        private void manipulateErasableItem(object sender, ContainerManipulationCompletedEventArgs e)
        {
          if (SurfaceWindow1.hasTestTubeTag)
            {//If testTube tag is on surface. check if user wants to save it; if so, dock at tag's center. -CV, 3/22/10
                //Set bounds for SVI in relation to save tag
                double horizontalBoundingMin = (this.ActualCenter).X - this.ActualWidth / 2;
                double horizontalBoundingMax = (this.ActualCenter).X + this.ActualWidth / 2;
                double verticalBoundingMin = (this.ActualCenter).Y - this.ActualHeight / 2;
                double verticalBoundingMax = (this.ActualCenter).Y + this.ActualHeight / 2;

                if (_docked)
                {
                    this.Center = this.ActualCenter;
                    SurfaceWindow1.mainSVIList.Remove(this);
                    _docked = false;
                    this.BorderBrush = Brushes.Gray;

                    //if (this.Name == "results")
                    //{
                    //    this.Height = 100;
                    //    this.Width = 100;
                    //}
                    //else 
                    if (this.Name == "mouse" || this.Name == "dog" || this.Name == "human")
                        {
                            this.Height = 140;
                            this.Width = 140;
                        }
                        else
                        {
                            this.Height = 200;
                            this.Width = 300;
                        }
                    //Makes the line visible. It is "unerased" when the tube is returned to the surface, so no need to erase here. - CV, 3/23/10
                    if (this.Name == "sequenceBoxFromSearchScreen")
                    {
                        SequenceBox seq = sender as SequenceBox;
                        Line l = seq.getConnector();
                        l.Visibility = Visibility.Visible;
                    }
                }
                else

                    if ((SurfaceWindow1.testTubePoint.X >= horizontalBoundingMin && SurfaceWindow1.testTubePoint.X <= horizontalBoundingMax)
                        && (SurfaceWindow1.testTubePoint.Y >= verticalBoundingMin && SurfaceWindow1.testTubePoint.Y <= verticalBoundingMax))
                    {
                        this.Center = SurfaceWindow1.testTubePoint;
                        _docked = true;
                        this.BorderBrush = Brushes.DodgerBlue;
                        this.Height = 100;
                        this.Width = 100;
                        SurfaceWindow1.mainSVIList.Add(this);
                        //Makes the line invisible. It is "erased" when the tube is removed, so no need to erase here. - CV, 3/23/10
                        if (this.Name == "rnaText" || this.Name == "aaText" || this.Name == "dnaText")
                        {
                            SequenceBox seq = sender as SequenceBox;
                            Line l = seq.getConnector();
                            l.Visibility = Visibility.Hidden;
                        
                        }
                    }
            }

            if ((this.ActualCenter).X <= 20 || (this.ActualCenter).X >= 1005)
              {
                this.erase();
              }

        }

    }
}