using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Windows;
using System.Windows.Media;

namespace GnomeSurfer2
{
    class ExpressionViewFactory
    {
        private GeneExpressionModelFactory _factory;
        private ScatterView _trashSV;

        public ExpressionViewFactory(GeneExpressionModelFactory factory, ScatterView trashSV)
        {
            _factory = factory;
            _trashSV = trashSV;
        }

        public ScatterViewItem GetMouseExpressionSVI(Gene gene)
        {
            IMouseGeneExpressionModel model = _factory.GetMouseGeneExpressionModel(gene);
            return new MouseExpressionControl(_trashSV, model, gene);
        }
    }
}
