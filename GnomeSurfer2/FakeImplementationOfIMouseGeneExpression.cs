using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GnomeSurfer2
{
    class FakeImplementationOfIMouseGeneExpression : IMouseGeneExpressionModel
    {

        /*
         *  These values are completely fake.
         */
 
        #region IMouseGeneExpressionModel Members

        public double CerebralCortexExpression
        {
            get 
            { 
                return -1; 
            }
        }

        public double CerebellumExpression
        {
            get 
            { 
                return 0; 
            }
        }

        public double StriatumExpression
        {
            get
            { 
                return 0.6; 
            }
        }

        public double ThymusExpression
        {
            get 
            { 
                return -0.5; 
            }
        }

        public double HeartExpression
        {
            get 
            { 
                return 0.1; 
            }
        }

        public double LungExpression
        {
            get
            {
                return -0.89;
            }
        }

        public double PancreaticIsletExpression
        {
            get 
            { 
                return -0.69; 
            }
        }

        public double KidneyExpression
        {
            get 
            { 
                return 0.99; 
            }
        }

        public double LiverExpression
        {
            get 
            { 
                return -0.43; 
            }
        }

        public double OvaryExpression
        {
            get 
            { 
                return 0.67; 
            }
        }

        public double TestisExpression
        {
            get 
            { 
                return 0.32; 
            }
        }

        public double SkinExpression
        {
            get 
            { 
                return 0.53; 
            }
        }

        public double BoneMarrowExpression
        {
            get 
            { 
                return -0.87; 
            }
        }

        public double AdipocyteExpression
        {
            get 
            { 
                return 0.65; 
            }
        }

        public double PBCD4Expression
        {
            get 
            { 
                return -0.73; 
            }
        }

        #endregion
    }
}
