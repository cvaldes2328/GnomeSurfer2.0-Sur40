using System;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Linq;
using System.Windows.Controls;
using System.Text;

namespace GnomeSurfer2
{
    public class DataItem//:Thumb
    {
        private bool canDrop;
        private string source;
        private int width;
        private int height;
        private int scatterheight;
        private Gene g;

        public bool CanDrop
        {
            get { return canDrop; }
        }

        public string Src
        {
            get { return source; }
        }

        public int Wd
        {
            get { return width; }
        }

        public int Ht
        {
            get { return height; }
        }

        public int ScatterHeight
        {
            get { return scatterheight; }
        }

        public Gene gene
        {
            get { return g; }
        }

        public string name
        {
            get { return g.getID(); }
        }


        public DataItem(bool canDrop, string source, int width, int height, Gene _g)
        {
            this.canDrop = canDrop;
            this.source = source;
            this.width = width;
            this.height = height;
            this.g = _g;
            this.scatterheight = 70; // changes height of scatterview so that ElementMenuItems can be seen
        }
    }
}

