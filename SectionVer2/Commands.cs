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
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Microsoft.Win32;
using System.Reflection;


using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using RegistryKey = Autodesk.AutoCAD.Runtime.RegistryKey;
using Registry = Autodesk.AutoCAD.Runtime.Registry;

namespace Sections
{
    public class ToolBox
    {
        [CommandMethod("LoadAllCommand")]
        public void loadsection()
        {
            SectionToolBox.MyRibbon rb = new SectionToolBox.MyRibbon();
            rb.CreateRibbonPanel();
        }

        [CommandMethod("RegisterMyApp")]
        public void RegisterMyApp()
        {
            // Get the AutoCAD Applications key
            string sProdKey = HostApplicationServices.Current.UserRegistryProductRootKey;
            string sAppName = "Section Toolbox";
            RegistryKey regAcadProdKey = Registry.CurrentUser.OpenSubKey(sProdKey);
            RegistryKey regAcadAppKey = regAcadProdKey.OpenSubKey("Applications", true);

            // Check to see if the "MyApp" key exists

            string[] subKeys = regAcadAppKey.GetSubKeyNames();
            foreach (string subKey in subKeys)
            {
                // If the application is already registered, exit
                if (subKey.Equals(sAppName))
                {
                    regAcadAppKey.Close();
                    return;
                }
            }
            // Get the location of this module
            string sAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            // Register the application
            RegistryKey regAppAddInKey = regAcadAppKey.CreateSubKey(sAppName);
            regAppAddInKey.SetValue("DESCRIPTION", sAppName, RegistryValueKind.String);
            regAppAddInKey.SetValue("LOADCTRLS", 14, RegistryValueKind.DWord);
            regAppAddInKey.SetValue("LOADER", sAssemblyPath, RegistryValueKind.String);
            regAppAddInKey.SetValue("MANAGED", 1, RegistryValueKind.DWord);
            regAcadAppKey.Close();
        }

        [CommandMethod("UnregisterMyApp")]
        public void UnregisterMyApp()

        {

            // Get the AutoCAD Applications key

            string sProdKey = HostApplicationServices.Current.UserRegistryProductRootKey;

            string sAppName = "Section Toolbox";



            RegistryKey regAcadProdKey = Registry.CurrentUser.OpenSubKey(sProdKey);

            RegistryKey regAcadAppKey = regAcadProdKey.OpenSubKey("Applications", true);



            // Delete the key for the application

            regAcadAppKey.DeleteSubKeyTree(sAppName);

            regAcadAppKey.Close();

        }

        IExtensionApplication _application;

        [CommandMethod("subdivpoly")]
        public void Profilefrompolyline()
        {          
            SectionToolBox.Subdividing win = new SectionToolBox.Subdividing();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win);
        }

        public void Initialize()

        {
            var ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\nLoading custom addin: \"CivYam.dll\"...");
        }
        

        [CommandMethod("pfp")]
        public void profilefrompolyline()
        {
            Sections.Profiles.profilefrompolyline();
        }

        [CommandMethod("seceditor")]
        public void seceditor()
        {
            Sections.SectionViewEditor win = new Sections.SectionViewEditor();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win);
        }

        [CommandMethod("getanchor2")]
        public void labelprop2()
        {
            Sections.GetAnchor1.labelprop2();
        }

        [CommandMethod("getanchor")]
        public void labelprop1()
        {
            Sections.GetAnchor1.labelprop1();
        }        

        [CommandMethod("blockmatch")]
        public void DynamicBlocksmatch()
        {
            Sections.BlockMatch.DynamicBlocksmatch();
        }

        [CommandMethod("AutocadSection2Civil")]
        public void cmdAutocadSection2Civil()
        {
            var win = new Sections.AutocadSection2Civil();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win);
        }

        [CommandMethod("DesckeyTransfer")]
        public void cmdDesckeyTransfer()
        {            
            Sections.DescriptionKeyTansfer win = new Sections.DescriptionKeyTansfer();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win);
        }

        [CommandMethod("CreateTunnel")]
        public void cmdCreateTunnel()
        {
            Sections.CreateTunnel win = new Sections.CreateTunnel();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win);
        }

        [CommandMethod("CreateXYZSTAFromXYZ")]
        public void cmdCreateXYZSTAFromXYZ()
        {
            Sections.CreateSectionFromXYZ win = new Sections.CreateSectionFromXYZ();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win);
        }

        [CommandMethod("cordselect2")]
        public void cmdcordel2()
        {
            Sections.STR2 win = new Sections.STR2();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win);
        }

        [CommandMethod("ExportSection2Chainage")]
        public void cmdExportSection2Chainage()
        {
            Sections.ExportSection2Chainage win = new Sections.ExportSection2Chainage();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win);
        }

        [CommandMethod("Exportsec2xyzoffsta")]
        public void cmdexportsec2xyzoffsta()
        {
            Sections.Exportsec2xyzsatoff win = new Sections.Exportsec2xyzsatoff();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win);
        }

        [CommandMethod("CSDPExport")]
        public void cmdCSDPExport()
        {            
            Sections.CSDPSectionExport win = new Sections.CSDPSectionExport();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win);            
        }

        [CommandMethod("cordselect")]
        public void cmdcordel()
        {
            Sections.STR win = new Sections.STR();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win);
        }        

        [CommandMethod("CreateSectionFromFile")]
        public void cmdSectionFromfile()
        {
            Sections.CreateSectionFromFile win = new Sections.CreateSectionFromFile();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win);          
        }

        [CommandMethod("BatchPlot")]
        public static void PlotCurrentLayout()
        {

            SectionVer2.Other_App.BatchPlot.Batch_Plot win = new SectionVer2.Other_App.BatchPlot.Batch_Plot();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win);            
        }

        [CommandMethod("crtparcel")]
        public static void crtparcel()
        {
            Autodesk.AutoCAD.EditorInput.Editor editor = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            Autodesk.AutoCAD.DatabaseServices.Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
            Autodesk.AutoCAD.ApplicationServices.Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            CivilDocument civildoc = CivilApplication.ActiveDocument;
            //ObjectIdCollection siteids = civildoc.GetSiteIds();
            //Site site = null;
            ObjectId siteid = Site.Create(civildoc, "A");
            
            using (Transaction ts = db.TransactionManager.StartTransaction())
            {
                //foreach (ObjectId k in siteids)
                //{

                //    Site cursiteid = ts.GetObject(k, OpenMode.ForRead) as Site;
                //    if (cursiteid.Name == "Site 1")
                //    {
                //        site = cursiteid;
                //        break;
                //    }            
                //}
                Site site = ts.GetObject(siteid, OpenMode.ForWrite) as Site;
                PromptSelectionResult se = editor.GetSelection();
                if (se.Status == PromptStatus.Error) return;
                SelectionSet Set1 = se.Value;
                ObjectId[] ids1 = Set1.GetObjectIds();
                foreach (ObjectId entId1 in ids1)
                {
                    Autodesk.AutoCAD.DatabaseServices.Polyline pline = ts.GetObject(entId1, OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.Polyline;
                    if (pline != null)
                    {
                        dynamic acadsite = site.AcadObject;
                        dynamic parcellines = acadsite.ParcelSegments;
                        dynamic segment = parcellines.AddFromEntity(pline.AcadObject, true);
                    }
                }
                ts.Commit();        
            }
        }

    }
}
