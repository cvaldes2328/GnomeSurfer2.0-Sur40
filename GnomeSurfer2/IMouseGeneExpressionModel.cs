/// <Summary>
/// FILE:                   IMouseGeneExpressionModel.cs
/// AUTHORS:                M. Strait
/// DESCRIPTION:            Interface for all MouseGeneExpression objects.
/// 
/// MODIFICATION HISTORY:   24-May-10   Documentation updated.
/// </Summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;

namespace GnomeSurfer2
{
    interface IMouseGeneExpressionModel
    {
        double CerebralCortexExpression { get; }
        double CerebellumExpression { get; }
        double StriatumExpression { get; }

        double ThymusExpression { get; }
        double HeartExpression { get; }
        double LungExpression { get; }

        double PancreaticIsletExpression { get; }
        double KidneyExpression { get; }
        double LiverExpression { get; }

        double OvaryExpression { get; }
        double TestisExpression { get; }

        double SkinExpression { get; }
        double BoneMarrowExpression { get; }
        double AdipocyteExpression { get; }

        double PBCD4Expression { get; }
    }
}
