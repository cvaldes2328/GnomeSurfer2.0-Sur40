using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Controls;
using Microsoft.Surface.Presentation;
using System.Windows.Input;

namespace GnomeSurfer2
{
    class MouseExpressionControl : ExpressionControl
    {
        private const string _headGroup = "Head";
        private const string _chestGroup = "Chest";
        private const string _gutGroup = "Gut";
        private const string _germGroup = "Germ";
        private const string _limbGroup = "Limb";
        private const string _lymphGroup = "Lymph";

        public MouseExpressionControl(ScatterView trashSV, IMouseGeneExpressionModel model, Gene gene) : base(trashSV)
        {
            MouseExpressionMainPanel mainPanel = new MouseExpressionMainPanel(gene);
            MainPanel = mainPanel;

            AddTissueExpression(_headGroup, "Cerebellum", model.CerebellumExpression, "cerebellum.jpg");
            AddTissueExpression(_headGroup, "Cerebral Cortex", model.CerebralCortexExpression, "cerebralcortex.jpg");
            AddTissueExpression(_headGroup, "Striatum", model.StriatumExpression, "striatum.jpg");

            AddTissueExpression(_chestGroup, "Thymus", model.ThymusExpression);
            AddTissueExpression(_chestGroup, "Heart", model.HeartExpression);
            AddTissueExpression(_chestGroup, "Lung", model.LungExpression);

            AddTissueExpression(_gutGroup, "Pancreatic Islets", model.PancreaticIsletExpression);
            AddTissueExpression(_gutGroup, "Kidney", model.KidneyExpression);
            AddTissueExpression(_gutGroup, "Liver", model.LiverExpression);

            AddTissueExpression(_germGroup, "Testis", model.TestisExpression);
            AddTissueExpression(_germGroup, "Ovary", model.OvaryExpression);

            AddTissueExpression(_limbGroup, "Bone Marrow", model.BoneMarrowExpression);
            AddTissueExpression(_limbGroup, "Skin", model.SkinExpression);
            AddTissueExpression(_limbGroup, "Adipocyte", model.AdipocyteExpression);

            AddTissueExpression(_lymphGroup, "PB-CD4+ Tcells", model.PBCD4Expression);
     
            mainPanel.HeadContact.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(HeadContact_TouchDown);
            mainPanel.ChestContact.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(ChestContact_TouchDown);
            mainPanel.GutContact.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(GutContact_TouchDown);
            mainPanel.GermContact.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(GermContact_TouchDown);
            mainPanel.LimbContact.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(LimbContact_TouchDown);
            mainPanel.LymphContact.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(LymphContact_TouchDown);
            mainPanel.AllTissuesContact.TouchDown += new EventHandler<System.Windows.Input.TouchEventArgs>(AllTissuesContact_TouchDown);
        }

        void AllTissuesContact_TouchDown(object sender, TouchEventArgs e)
        {
            DisplayAllTissueGroups();
        }

        void LymphContact_TouchDown(object sender, TouchEventArgs e)
        {
            DisplayTissueGroup(_lymphGroup);
        }

        void LimbContact_TouchDown(object sender, TouchEventArgs e)
        {
            DisplayTissueGroup(_limbGroup);
        }

        void GermContact_TouchDown(object sender, TouchEventArgs e)
        {
            DisplayTissueGroup(_germGroup);
        }

        void GutContact_TouchDown(object sender, TouchEventArgs e)
        {
            DisplayTissueGroup(_gutGroup);
        }

        void ChestContact_TouchDown(object sender, TouchEventArgs e)
        {
            DisplayTissueGroup(_chestGroup);
        }

        void HeadContact_TouchDown(object sender, TouchEventArgs e)
        {
            DisplayTissueGroup(_headGroup);
        }
    }
}
