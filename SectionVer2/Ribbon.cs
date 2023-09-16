using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using System.IO;
using Autodesk.Civil;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
//using Autodesk.AutoCAD.Customization;

using Autodesk.Windows;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using System.Windows.Media.Imaging;
using Autodesk.AutoCAD.Customization;
using RibbonPanelSource = Autodesk.Windows.RibbonPanelSource;
using RibbonButton = Autodesk.Windows.RibbonButton;
using RibbonSeparator = Autodesk.Windows.RibbonSeparator;
using RibbonSplitButton = Autodesk.Windows.RibbonSplitButton;
using RibbonSeparatorStyle = Autodesk.Windows.RibbonSeparatorStyle;
using SectionToolBox.Properties;
using System.Drawing.Imaging;
using System.Drawing;
using Microsoft.VisualBasic.CompilerServices;
using System.Windows.Media.Media3D;
using Autodesk.AutoCAD.GraphicsInterface;
using System.Security.Cryptography;

namespace SectionToolBox
{
    internal class MyRibbon
    {
        
        public void CreateRibbonPanel()
        {
            Autodesk.Windows.RibbonControl ribbonCont = ComponentManager.Ribbon;
            if (ribbonCont != null)
            {
                RibbonTab rtab = ribbonCont.FindTab("Testing");
                if (rtab != null)
                {
                    ribbonCont.Tabs.Remove(rtab);
                }
                rtab = new RibbonTab();
                rtab.Title = "Section ToolBox";
                rtab.Id = "Testing";
                //Add the Tab
                ribbonCont.Tabs.Add(rtab);


                RibbonPanelSource rps = new RibbonPanelSource();
                rps.Title = "Section Tool Box";
                RibbonPanel rp = new RibbonPanel();
                rp.Source = rps;
                rtab.Panels.Add(rp);
                rtab.IsActive = true;
                RibbonSeparator rbsep = new RibbonSeparator();

                //Create a Command Item that the Dialog Launcher can use,
                // for this test it is just a place holder.

                RibbonButton rb0 = NewButton("Batch Plot", "Batchplot");
                //rb.Image = LoadImage(Properties.Resources.Csdpplus,180,180);
                RibbonButton rb1 = NewButton("Section Editor", "seceditor");
                RibbonButton rb2 = NewButton("Create Tunnel", "CreateTunnel");
                RibbonButton rb3 = NewButton("CC,CF,T", "cordselect");
                RibbonButton rb4 = NewButton("Autocad Sections Civil", "AutocadSection2Civil");
                RibbonButton rb5 = NewButton("Export CSDP Sections", "CSDPExport");
                RibbonButton rb6 = NewButton("Export Section to XYZ,OFF,Sta", "Exportsec2xyzoffsta");
                RibbonButton rb7 = NewButton("Export Section to Chainage", "ExportSection2Chainage");
                RibbonButton rb8 = NewButton("Create Profile from Polyline", "pfp");
                RibbonButton rb9 = NewButton("Create XYZSTA from XYZ", "CreateXYZSTAFromXYZ");
                RibbonButton rb10 = NewButton("Create Section From File", "CreateSectionFromFile");
                RibbonButton rb11 = NewButton("Description Keys Transfer", "DesckeyTransfer");
                rps.Items.Add(rb0); rps.Items.Add(rbsep);
                rps.Items.Add(rb1); rps.Items.Add(rbsep);
                rps.Items.Add(rb2); rps.Items.Add(rbsep);
                rps.Items.Add(rb3); rps.Items.Add(rbsep);
                rps.Items.Add(rb4); rps.Items.Add(rbsep);
                rps.Items.Add(rb5); rps.Items.Add(rbsep);
                rps.Items.Add(rb6); rps.Items.Add(rbsep);
                rps.Items.Add(rb7); rps.Items.Add(rbsep);
                rps.Items.Add(rb8); rps.Items.Add(rbsep);
                rps.Items.Add(rb9); rps.Items.Add(rbsep);
                rps.Items.Add(rb10); rps.Items.Add(rbsep);
                rps.Items.Add(rb11); rps.Items.Add(rbsep);
            }
        }

        private RibbonButton NewButton(string ribbonname, string command)
        {
            RibbonButton rci = new RibbonButton();
            rci.Name = "TestCommand33";
            Autodesk.Windows.RibbonButton rb = new RibbonButton();
            rb.Name = command;                        
            rb.ShowText = true;
            rb.Text = ribbonname;
            rb.CommandParameter = "";
            rb.CommandHandler = new MyCmdHandler();            
            return rb;
        }
        
        private static BitmapImage LoadImage(Bitmap imageToLoad, int Height, int Width)
        {
            BitmapImage image2;
            BitmapImage image = new BitmapImage();
            try
            {
                Bitmap bitmap = imageToLoad;
                MemoryStream stream = new MemoryStream();
                bitmap.Save(stream, ImageFormat.Png);
                image.BeginInit();
                image.StreamSource = stream;
                image.DecodePixelHeight = Height;
                image.DecodePixelWidth = Width;
                image.EndInit();

                image2 = image;
            }
            catch (System.Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                image2 = null;
                ProjectData.ClearProjectError();
            }
            return image2;
        }       

        public class MyCmdHandler : System.Windows.Input.ICommand
        {
            public bool CanExecute(object parameter)
            {
                return true;
            }
            public event EventHandler CanExecuteChanged;
            public void Execute(object parameter)
            {
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                
                if (parameter is RibbonButton)
                {
                    RibbonButton button = parameter as RibbonButton;
                    
                    string cmd = string.Format("{0}{1}", new string((char)03, 2), button.Name);
                    doc.SendStringToExecute(cmd + " ", true, false, true);

                }
            }
        }
    }
}
