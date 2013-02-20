using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using System.IO;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;


namespace GnomeSurfer2
{
    class Human
    {
        private ElementMenu head;
        private ElementMenu chest;
        private ElementMenu gut;
        private ElementMenu germ;
        private ElementMenu limb;
        private ElementMenu lymph;
        private ElementMenuItem[] allItems;


        /// <summary>
        /// Creates element menus for human body regions and colors the menu items according to 
        /// microarray expression data for TP53
        /// @ Sarah Elfenbein
        /// </summary>
        /// <param name="elm1">head</param> <param name="elm2">chest</param> <param name="elm3">gut</param>
        /// <param name="elm4">germ</param> <param name="elm5">limb</param> <param name="elm6">lymph</param>
        public Human(ElementMenu elm1, ElementMenu elm2, ElementMenu elm3,
            ElementMenu elm4, ElementMenu elm5, ElementMenu elm6)
        {
            head = elm1;
            chest = elm2;
            gut = elm3;
            germ = elm4;
            limb = elm5;
            lymph = elm6;

            allItems = new ElementMenuItem[15];
            string[] labelData = {"fetal brain", "whole brain", "amygdala", "thymus", "bone marrow", "PB-CD4+ Tcells", "skin", 
                                     "adipocyte", "pancreatic islets", "heart", "lung", "kidney", "liver", "ovary", "testis"};

            //This data for TP53 is from http://genome.ucsc.edu/cgi-bin/hgNear?hgsid=139063302&near_search=uc002gij.2
            //string[] colorData = {"001100", "003700", "002D00", "540000", "0C0000", "390000", "0C0000", "3D0000", "070000", 
            //                         "1C0000", "100000", "000200", "1C0000", "020000", "0B0000"};

            //Here is a link to a good color chart so that you can visualize the colors you use from html:
            //http://s23.org/wiki/Template:Color_Picker

            //These colors have been altered so that they appear brighter on the surface
            string[] colorData = {"001100", "00D700", "006D00", "B40000", "0C0000", "690000", "0C0000", "8D0000", "070000", 
                                     "1C0000", "100000", "000200", "1C0000", "020000", "0B0000"};

            
            /* This loop converts each of the colorData elements to rgb, and assigns the color to a brush,
             * which is used as the rectangle fill for the rectangle set as the current ElementMenuItem's icon. 
             * Each ElementMenuItem is then added to an array called allItems.
             */
            for (int i = 0; i < colorData.Length; i++)
            {
                System.Windows.Shapes.Rectangle rext = new System.Windows.Shapes.Rectangle(); 
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                byte r = Convert.ToByte(colorData[i].Remove(2), 16);
                byte g = Convert.ToByte(colorData[i].Substring(2, 2), 16);
                byte b = Convert.ToByte(colorData[i].Substring(4), 16);
                mySolidColorBrush.Color = System.Windows.Media.Color.FromRgb(r, g, b);
                rext.Fill = mySolidColorBrush;

                ElementMenuItem emi = new ElementMenuItem();
                emi.Icon = rext;
                emi.Icon = mySolidColorBrush;
                emi.Header = labelData[i];
                allItems[i] = emi;
            }


            // 3 ElementMenuItems added to the head ElementMenu
            head.Items.Add(allItems[1]);
            head.Items.Add(allItems[0]);
            head.Items.Add(allItems[2]);

            SolidColorBrush headBrush = new SolidColorBrush();
            headBrush.Color = System.Windows.Media.Color.FromRgb(Convert.ToByte("00", 16),
                                                                 Convert.ToByte("37", 16),
                                                                 Convert.ToByte("00", 16));
            head.Background = headBrush;


            // 3 ElementMenuItems added to the chest ElementMenu
            chest.Items.Add(allItems[9]);
            chest.Items.Add(allItems[3]);
            chest.Items.Add(allItems[10]);
            SolidColorBrush chestBrush = new SolidColorBrush();
            chestBrush.Color = System.Windows.Media.Color.FromRgb(Convert.ToByte("54", 16),
                                                                 Convert.ToByte("00", 16),
                                                                 Convert.ToByte("00", 16));
            chest.Background = chestBrush;


            // 3 ElementMenuItems added to the gut ElementMenu
            gut.Items.Add(allItems[11]);
            gut.Items.Add(allItems[12]);
            gut.Items.Add(allItems[8]);
            SolidColorBrush gutBrush = new SolidColorBrush();
            gutBrush.Color = System.Windows.Media.Color.FromRgb(Convert.ToByte("1C", 16),
                                                                 Convert.ToByte("00", 16),
                                                                 Convert.ToByte("00", 16));
            gut.Background = gutBrush;


            // 2 ElementMenuItems added to the germ ElementMenu
            germ.Items.Add(allItems[13]);
            germ.Items.Add(allItems[14]);
            SolidColorBrush germBrush = new SolidColorBrush();
            germBrush.Color = System.Windows.Media.Color.FromRgb(Convert.ToByte("0B", 16),
                                                                 Convert.ToByte("00", 16),
                                                                 Convert.ToByte("00", 16));
            germ.Background = germBrush;


            // 3 ElementMenuItems added to the limb ElementMenu
            limb.Items.Add(allItems[7]);
            limb.Items.Add(allItems[4]);
            limb.Items.Add(allItems[6]); 
            SolidColorBrush limbBrush = new SolidColorBrush();
            limbBrush.Color = System.Windows.Media.Color.FromRgb(Convert.ToByte("3D", 16),
                                                                 Convert.ToByte("00", 16),
                                                                 Convert.ToByte("00", 16));
            limb.Background = limbBrush;


            // 1 ElementMenuItem added to the lymph ElementMenu
            lymph.Items.Add(allItems[5]);
            SolidColorBrush lymphBrush = new SolidColorBrush();
            lymphBrush.Color = System.Windows.Media.Color.FromRgb(Convert.ToByte("39", 16),
                                                                  Convert.ToByte("00", 16),
                                                                  Convert.ToByte("00", 16));
            lymph.Background = lymphBrush;                
        }
    }
}



/* 
 * Chest:
 * thyroid
 * heart 
 * lung
 * 
 * Gut:
 * kidney
 * liver
 * pancreatic islets
 * 
 * Germ:
 * ovary
 * testis
 * 
 * Head:
 * fetal brain
 * whole brain
 * amygdala
 * 
 * Limb:
 * bone marrow
 * adipocyte
 * skin
 * 
 * Lymph:
 * PB-CD4+ Tcells
 * 
 */

//