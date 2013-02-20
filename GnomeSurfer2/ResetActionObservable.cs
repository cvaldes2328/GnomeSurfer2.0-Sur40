using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GnomeSurfer2
{
    class ResetActionObservable
    {
        private List<Action> _resetMethods;

        public ResetActionObservable()
        {
            _resetMethods = new List<Action>();
        }

        public void addResetMethod(Action resetAction)
        {
            _resetMethods.Add(resetAction);
        }

        public void resetAll()
        {
            foreach (Action resetMethod in _resetMethods)
            {
                resetMethod();
            }
        }

    }
}
