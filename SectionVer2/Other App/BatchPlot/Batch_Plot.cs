using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.PlottingServices;
using System.Collections.Specialized;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace SectionVer2.Other_App.BatchPlot
{

    public partial class Batch_Plot : Form
    {
        [DllImport("accore.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "acedTrans")]
        static extern int acedTrans (Point3d point, IntPtr fromRb, IntPtr toRb, int disp, out Point3d result);
        PlotSettingsValidator psv = null;
        StringCollection devlist = null;        
        string blockname = "";
        public Batch_Plot()
        {
            InitializeComponent();            
            loadDeviceList(ref psv, ref devlist);
            foreach (string device in devlist)
            {
                LS_PrinterName.Items.Add(device);
            }
            LS_PrinterName.SelectedIndex = 0;
            LS_Size.SelectedIndex = 0;
            
        }

        public void loadDeviceList(ref PlotSettingsValidator psv, ref StringCollection devlist)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            // Assign default return values
            string devname = "", medname = "";
            psv = PlotSettingsValidator.Current;
            // Let's first select the device
            devlist = psv.GetPlotDeviceList();
            
        }

        private void LS_PrinterName_SelectedIndexChanged(object sender, EventArgs e)
        {  
            loadDeviceList(ref psv, ref devlist);
            LS_Size.Items.Clear();
            PlotSettings ps = new PlotSettings(true);
            using (ps)
            {
                // We should refresh the lists,
                // in case setting the device impacts
                // the available media
                psv.SetPlotConfigurationName(ps, LS_PrinterName.SelectedItem.ToString(), null);
                psv.RefreshLists(ps);
                StringCollection medlist = psv.GetCanonicalMediaNameList(ps);
                foreach (string medlist2 in medlist)
                {
                    LS_Size.Items.Add(medlist2);
                }
            }
            LS_Size.SelectedIndex = 0;
        }
        

        private void Plot_BTN_Click(object sender, EventArgs e)
        {
            int numsheet = 1;
            Point2dCollection win = ExtentPoints();
            WindowPlot(numsheet, win);
        }        

        public Point2dCollection ExtentPoints()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Point2dCollection win = new Point2dCollection();
            Point2d po1;
            Point2d po2;
            using ( Transaction tr = db.TransactionManager.StartTransaction() )
            {
                var blockTable = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                // open the model space BlockTableRecord
                var modelSpace = (BlockTableRecord)tr.GetObject(blockTable [BlockTableRecord.ModelSpace], OpenMode.ForRead);
                // iterate through the model space 
                foreach ( ObjectId id in modelSpace )
                {
                    // check if the current ObjectId is a block reference one
                    if ( id.ObjectClass.DxfName == "INSERT" )
                    {
                        // open the block reference
                        var bl = (BlockReference)tr.GetObject(id, OpenMode.ForRead);
                        if ( bl.Name == blockname )
                        {
                            po1 = new Point2d(bl.Bounds.Value.MaxPoint.X, bl.Bounds.Value.MaxPoint.Y);
                            po2 = new Point2d(bl.Bounds.Value.MinPoint.X, bl.Bounds.Value.MinPoint.Y);
                            win.Add(po1);
                            win.Add(po2);
                        }
                    }
                }
                tr.Commit();
                return win;
            }
        }

        private void BlockSelect_BTN_Click(object sender, EventArgs e)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            PromptSelectionResult polsel = ed.GetSelection();           
            
            if (polsel.Status == PromptStatus.Error) return;
            SelectionSet BLockAR = polsel.Value;
            ObjectId[] id = BLockAR.GetObjectIds();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockReference bl = tr.GetObject(id[0], OpenMode.ForRead) as BlockReference;
                if (bl != null)
                {
                   
                    blockname = bl.Name;
                }                
                tr.Commit();
            }
        }

        public void PlotLayout(double min_x, double min_y, double max_x, double max_y)
        {
            // Get the current document and database, and start a transaction
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            string devname = LS_PrinterName.SelectedItem.ToString(); //devmed[0];
            string medname = LS_Size.SelectedItem.ToString();// devmed[1];
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                // Reference the Layout Manager
                LayoutManager LayoutMgr = LayoutManager.Current;

                // Get the current layout and output its name in the Command Line window
                Layout Layout = tr.GetObject(LayoutMgr.GetLayoutId(LayoutMgr.CurrentLayout),
                                                    OpenMode.ForRead) as Layout;

                // Get the PlotInfo from the layout
                using (PlotInfo PlInfo = new PlotInfo())
                {
                    PlInfo.Layout = Layout.ObjectId;

                    // Get a copy of the PlotSettings from the layout
                    using (PlotSettings PS = new PlotSettings(Layout.ModelType))
                    {
                        PS.CopyFrom(Layout);

                        // Update the PlotSettings object
                        PlotSettingsValidator psv = PlotSettingsValidator.Current;

                        // Set the plot type
                        //psv.SetPlotType(PS, Autodesk.AutoCAD.DatabaseServices.PlotType.Extents);                        
                        psv.SetPlotWindowArea(PS, new Extents2d(min_x, min_y, max_x, max_y));
                        psv.SetPlotType(PS, Autodesk.AutoCAD.DatabaseServices.PlotType.Window);

                        // Set the plot scale
                        psv.SetUseStandardScale(PS, true);
                        psv.SetStdScaleType(PS, StdScaleType.ScaleToFit);

                        

                        // Center the plot
                        psv.SetPlotCentered(PS, true);

                        // Set the plot device to use
                        psv.SetPlotConfigurationName(PS, devname, medname);

                        // Set the plot info as an override since it will
                        // not be saved back to the layout
                        PlInfo.OverrideSettings = PS;

                        // Validate the plot info
                        using (PlotInfoValidator PlInfoVdr = new PlotInfoValidator())
                        {
                            PlInfoVdr.MediaMatchingPolicy = MatchingPolicy.MatchEnabled;
                            PlInfoVdr.Validate(PlInfo);

                            // Check to see if a plot is already in progress
                            if (PlotFactory.ProcessPlotState == ProcessPlotState.NotPlotting)
                            {
                                using (PlotEngine acPlEng = PlotFactory.CreatePublishEngine())
                                {
                                    // Track the plot progress with a Progress dialog
                                    using (PlotProgressDialog acPlProgDlg = new PlotProgressDialog(false, 1, true))
                                    {
                                        using ((acPlProgDlg))
                                        {
                                            // Define the status messages to display 
                                            // when plotting starts
                                            acPlProgDlg.set_PlotMsgString(PlotMessageIndex.DialogTitle, "Plot Progress");
                                            acPlProgDlg.set_PlotMsgString(PlotMessageIndex.CancelJobButtonMessage, "Cancel Job");
                                            acPlProgDlg.set_PlotMsgString(PlotMessageIndex.CancelSheetButtonMessage, "Cancel Sheet");
                                            acPlProgDlg.set_PlotMsgString(PlotMessageIndex.SheetSetProgressCaption, "Sheet Set Progress");
                                            acPlProgDlg.set_PlotMsgString(PlotMessageIndex.SheetProgressCaption, "Sheet Progress");

                                            // Set the plot progress range
                                            acPlProgDlg.LowerPlotProgressRange = 0;
                                            acPlProgDlg.UpperPlotProgressRange = 100;
                                            acPlProgDlg.PlotProgressPos = 0;

                                            // Display the Progress dialog
                                            acPlProgDlg.OnBeginPlot();
                                            acPlProgDlg.IsVisible = true;

                                            // Start to plot the layout
                                            acPlEng.BeginPlot(acPlProgDlg, null);

                                            // Define the plot output
                                            acPlEng.BeginDocument(PlInfo, doc.Name, null, 1, true, db.Filename);
                                            //db.Filename.Replace(db.Filename.Split('\\')[db.Filename.Split('\\').Length - 1], "").ToString()
                                            // Display information about the current plot
                                            acPlProgDlg.set_PlotMsgString(PlotMessageIndex.Status, "Plotting: " + doc.Name + " - " + Layout.LayoutName);

                                            // Set the sheet progress range
                                            acPlProgDlg.OnBeginSheet();
                                            acPlProgDlg.LowerSheetProgressRange = 0;
                                            acPlProgDlg.UpperSheetProgressRange = 100;
                                            acPlProgDlg.SheetProgressPos = 0;

                                            // Plot the first sheet/layout
                                            using (PlotPageInfo acPlPageInfo = new PlotPageInfo())
                                            {
                                                acPlEng.BeginPage(acPlPageInfo, PlInfo, true, null);
                                            }

                                            acPlEng.BeginGenerateGraphics(null);
                                            acPlEng.EndGenerateGraphics(null);

                                            // Finish plotting the sheet/layout
                                            acPlEng.EndPage(null);
                                            acPlProgDlg.SheetProgressPos = 100;
                                            acPlProgDlg.OnEndSheet();

                                            // Finish plotting the document
                                            acPlEng.EndDocument(null);

                                            // Finish the plot
                                            acPlProgDlg.PlotProgressPos = 100;
                                            acPlProgDlg.OnEndPlot();
                                            acPlEng.EndPlot(null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void WindowPlot (int numsheet, Point2dCollection win)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            string devname = LS_PrinterName.SelectedItem.ToString();
            string medname = LS_Size.SelectedItem.ToString();
            ///
            for (int i=0; i< win.Count-1; i=i+2 )
            {
                Point3d p1 = new Point3d(win [i].X, win [i].Y, 0);
                Point3d p2 = new Point3d(win [i+1].X, win [i+1].Y, 0);
                // Transform from UCS to DCS
                ResultBuffer rbFrom = new ResultBuffer(new TypedValue(5003, 1)), rbTo = new ResultBuffer(new TypedValue(5003, 2));
                Point3d firres = new Point3d(0, 0, 0);
                Point3d secres = new Point3d(0, 0, 0);

                // Transform the first point...
                acedTrans(p1, rbFrom.UnmanagedObject, rbTo.UnmanagedObject, 0, out firres);

                // ... and the second
                acedTrans(p2, rbFrom.UnmanagedObject, rbTo.UnmanagedObject, 0, out secres);

                // We can safely drop the Z-coord at this stage
                //Extents2d window = new Extents2d(firres.X, firres.Y, secres.X, secres.Y);
                Extents2d window = new Extents2d(firres.X, firres.Y, secres.X, secres.Y);


                Transaction tr2 = db.TransactionManager.StartTransaction();
                using ( tr2 )
                {
                    // We'll be plotting the current layout
                    BlockTableRecord btr = (BlockTableRecord)tr2.GetObject(db.CurrentSpaceId, OpenMode.ForRead);

                    Layout lo = (Layout)tr2.GetObject(btr.LayoutId, OpenMode.ForRead);

                    // We need a PlotInfo object
                    // linked to the layout
                    PlotInfo pi = new PlotInfo();
                    pi.Layout = btr.LayoutId;

                    // We need a PlotSettings object
                    // based on the layout settings
                    // which we then customize
                    PlotSettings ps = new PlotSettings(lo.ModelType);
                    ps.CopyFrom(lo);

                    object backgroundPlot = Application.GetSystemVariable("BACKGROUNDPLOT");
                    Application.SetSystemVariable("BACKGROUNDPLOT", 0);

                    // The PlotSettingsValidator helps
                    // create a valid PlotSettings object
                    PlotSettingsValidator psv = PlotSettingsValidator.Current;

                    // We'll plot the extents, centered and
                    // scaled to fit
                    psv.SetPlotWindowArea(ps, window);
                    psv.SetPlotType(ps, Autodesk.AutoCAD.DatabaseServices.PlotType.Window);
                    psv.SetUseStandardScale(ps, true);
                    psv.SetStdScaleType(ps, StdScaleType.ScaleToFit);
                    psv.SetPlotCentered(ps, true);

                    // We'll use the standard pdf PC3, as
                    // for today we're just plotting to file
                    //psv.SetPlotConfigurationName(ps, "DWG To PDF.pc3", "ISO full bleed A4 (210.00 x 297.00 MM)");
                    psv.SetPlotConfigurationName(ps, devname, medname);
                    // We need to link the PlotInfo to the
                    // PlotSettings and then validate it

                    pi.OverrideSettings = ps;
                    PlotInfoValidator piv = new PlotInfoValidator();
                    piv.MediaMatchingPolicy = MatchingPolicy.MatchEnabled;
                    piv.Validate(pi);

                    // A PlotEngine does the actual plotting
                    // (can also create one for Preview)
                    if ( PlotFactory.ProcessPlotState == ProcessPlotState.NotPlotting )
                    {
                        PlotEngine pe = PlotFactory.CreatePublishEngine();                        
                        using ( pe )
                        {
                            // Create a Progress Dialog to provide info
                            // and allow thej user to cancel
                            PlotProgressDialog ppd = new PlotProgressDialog(false, win.Count/2, true);
                            using ( ppd )
                            {
                                ppd.set_PlotMsgString(PlotMessageIndex.DialogTitle, "Custom Plot Progress");
                                ppd.set_PlotMsgString(PlotMessageIndex.CancelJobButtonMessage, "Cancel Job");
                                ppd.set_PlotMsgString(PlotMessageIndex.CancelSheetButtonMessage, "Cancel Sheet");
                                ppd.set_PlotMsgString(PlotMessageIndex.SheetSetProgressCaption, "Sheet Set Progress");
                                ppd.set_PlotMsgString(PlotMessageIndex.SheetProgressCaption, "Sheet Progress");
                                ppd.LowerPlotProgressRange = 0;
                                ppd.UpperPlotProgressRange = 100;
                                ppd.PlotProgressPos = 0;
                                
                                // Let's start the plot, at last
                                ppd.OnBeginPlot();
                                ppd.IsVisible = true;
                                pe.BeginPlot(ppd, null);

                                // We'll be plotting a single document
                                pe.BeginDocument(pi, doc.Name, null, 1, false, "c:\\Users\\a.txt");

                                // Which contains a single sheet
                                ppd.StatusMsgString = "Plotting " + doc.Name.Substring(doc.Name.LastIndexOf("\\") + 1) + " - sheet " + numsheet.ToString() + " of " + win.Count.ToString();
                                ppd.OnBeginSheet();
                                ppd.LowerSheetProgressRange = 0;
                                ppd.UpperSheetProgressRange = 100;
                                ppd.SheetProgressPos = 0;
                                PlotPageInfo ppi = new PlotPageInfo();
                                pe.BeginPage(ppi, pi, ( numsheet == win.Count/2 ), null);
                                pe.BeginGenerateGraphics(null);
                                pe.EndGenerateGraphics(null);

                                // Finish the sheet
                                pe.EndPage(null);
                                ppd.SheetProgressPos = 50;
                                ppd.OnEndSheet();

                                //// Finish the document
                                //pe.EndDocument(null);

                                //// And finish the plot
                                ppd.PlotProgressPos = 75;
                                ppd.OnEndPlot();
                                ppd.OnEndSheet();
                                //pe.EndPlot(null);
                                numsheet++;
                                ppd.PlotProgressPos = 100;
                            }
                            // Finish the document
                            pe.EndDocument(null);

                            // And finish the plot
                            //
                            pe.EndPlot(null);
                        }
                    }
                    else
                    {
                        ed.WriteMessage("\nAnother plot is in progress.");
                    }
                    tr2.Commit();
                    Application.SetSystemVariable("BACKGROUNDPLOT", backgroundPlot);
                }
            }
        }
    }
}
