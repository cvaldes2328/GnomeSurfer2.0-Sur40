using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using GnomeSurfer2;

namespace GnomeSurfer2
{
    class ExonSVI : ScatterViewItem
    {
        private Exon exon;

        public ExonSVI(Exon exon)
        {
            this.exon = exon;
        }

        public Exon getExon()
        {
            return exon;
        }
    }
}
