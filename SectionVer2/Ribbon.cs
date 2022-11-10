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


using Autodesk.Windows;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace SectionToolBox
{
    internal class Ribbon
    {        
        public void Addribbon(string ribbonname, string comm)
        {
            Autodesk.Windows.RibbonControl ribbon = ComponentManager.Ribbon;
            if (ribbon != null)
            {
                RibbonTab rtab = ribbon.FindTab("Section");
                if (rtab != null)
                {
                    ribbon.Tabs.Remove(rtab);
                }
                rtab = new RibbonTab();
                rtab.Title = "Section";
                rtab.Id = "Testing";
                //Add the Tab
                ribbon.Tabs.Add(rtab);
                Autodesk.Windows.RibbonButton rb;

                RibbonPanelSource rps = new RibbonPanelSource();
                rps.Title = "Test One";
                RibbonPanel rp = new RibbonPanel();
                rp.Source = rps;

                //Create a Command Item that the Dialog Launcher can use,
                // for this test it is just a place holder.
                RibbonButton rci = new RibbonButton();
                rci.Name = "TestCommand";

                //assign the Command Item to the DialgLauncher which auto-enables
                // the little button at the lower right of a Panel
                rps.DialogLauncher = rci;

                rb = new RibbonButton();
                rb.Name = comm;
                rb.ShowText = true;
                rb.Text = ribbonname;
                //rb.CommandParameter = "csdpexport";
                rb.CommandHandler = new MyCmdHandler();

                //Add the Button to the Tab
                rps.Items.Add(rb);               

            }
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
                    //doc.Editor.WriteMessage(
                    //  "\nRibbonButton Executed: " + button.Name + "\n");
                    string cmd = string.Format("{0}{1}", new string((char)03, 2), button.Name);
                    doc.SendStringToExecute(cmd + " ", true, false, true);

                }
            }
        }
    }
}
