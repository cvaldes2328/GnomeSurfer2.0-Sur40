/// <Summary>
/// FILE:                   IErasable.cs
/// AUTHORS:                Mikey Lintz
/// DESCRIPTION:            Interface for all UI objects that can be erased.
/// </Summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;

namespace GnomeSurfer2
{
    interface IErasable
    {
        /// <summary>
        /// Erases this object by removing it from its
        /// current parent and moving it to the "Trash" ScatterView.
        /// If this object is already erased, this method does nothing.
        /// 
        /// @ Mikey Lintz
        /// </summary>
        void erase();
        
        /// <summary>
        /// Unerases this object by removing it from the ScatterView specified
        /// in erase() and moving it to this object's parent before erase() was called.
        /// 
        /// @ Mikey Lintz
        /// </summary>
        void unerase();
    }
}
