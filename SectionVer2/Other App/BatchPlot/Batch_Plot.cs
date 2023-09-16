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
using Microsoft.Office.Interop.Excel;
using System.Dynamic;
using System.Windows.Forms.VisualStyles;
using PCM = Autodesk.AutoCAD.PlottingServices.PlotConfigManager;
using System.IO;
using Sections;

namespace SectionVer2.Other_App.BatchPlot
{

    public partial class Batch_Plot : Form
    {
        [DllImport("accore.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "acedTrans")]
        static extern int acedTrans (Point3d point, IntPtr fromRb, IntPtr toRb, int disp, out Point3d result);
        PlotSettingsValidator psv = null;
        StringCollection devlist = null;        
        string blockname = "";
        Document doc = Application.DocumentManager.MdiActiveDocument;
        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;        
        Database db = Application.DocumentManager.MdiActiveDocument.Database;
        private static string[,] ScaleValueArray;
        private static string GlbScale = "Scale to Fit";
        private static string GlbctbFile;
        //private static string[,] ScaleValueArray;
        private static string[] GlbCanonicalArray;
        int BlockNO = 0;


        public class MyPlotParams
        {
            private string DwgPath;
            private string DeviceName;
            private string PaperSize;
            private string ctbName;
            private bool ScLw;
            public int Cnt;
            private Autodesk.AutoCAD.DatabaseServices.StdScaleType ScTyp;
            private Autodesk.AutoCAD.DatabaseServices.PlotRotation PltRot;
            private string CanonicalPaperName;
            private string TabOpt;
            private bool ShouldStamp;
            private bool Plt2File;
            private string PltFileName;

            public MyPlotParams() { }
            public MyPlotParams(string DwgPath, string DeviceName, string PaperSize, string ctbName, bool ScLw, int Cnt, Autodesk.AutoCAD.DatabaseServices.StdScaleType ScTyp, Autodesk.AutoCAD.DatabaseServices.PlotRotation PltRot, string CanonicalMedia)
            {
                this.DwgPath = DwgPath;
                this.DeviceName = DeviceName;
                this.Paper = PaperSize;
                this.ctbName = ctbName;
                this.ScLw = ScLw;
                this.Cnt = Cnt;
                this.ScTyp = ScTyp;
                this.PltRot = PltRot;
                this.CanonicalPaperName = CanonicalMedia;
            }

            public string DrawingPath
            {
                get { return DwgPath; }
                set { DwgPath = value; }
            }

            public string Device
            {
                get { return DeviceName; }
                set { DeviceName = value; }
            }

            public string Paper
            {
                get { return PaperSize; }
                set { PaperSize = value; }
            }

            public string ctbFile
            {
                get { return ctbName; }
                set { ctbName = value; }
            }

            public bool ScaleLineweight
            {
                get { return ScLw; }
                set { ScLw = value; }
            }

            public int Amount
            {
                get { return Cnt; }
                set { Cnt = value; }
            }

            public Autodesk.AutoCAD.DatabaseServices.StdScaleType AcScaleType
            {
                get { return ScTyp; }
                set { ScTyp = value; }
            }

            public Autodesk.AutoCAD.DatabaseServices.PlotRotation AcPlotRotation
            {
                get { return PltRot; }
                set { PltRot = value; }
            }

            public string CanonicalPaper
            {
                get { return CanonicalPaperName; }
                set { CanonicalPaperName = value; }
            }

            public string TabOption
            {
                get { return TabOpt; }
                set { TabOpt = value; }
            }

            public bool ApplyStamp
            {
                get { return ShouldStamp; }
                set { ShouldStamp = value; }
            }

            public bool PlotToFile
            {
                get { return Plt2File; }
                set { Plt2File = value; }
            }

            public string PlotFileLocation
            {
                get { return PltFileName; }
                set { PltFileName = value; }
            }
        }

        public MyPlotParams ApplySettings()
        {
            string RealScale = "";
            string DeviceName = LS_PrinterName.SelectedItem.ToString();
            string PaperSize = LS_Size.SelectedItem.ToString();
            string ctbFile = comboBoxPlotStyle.SelectedItem.ToString();
            string Scale = comboBoxScale.SelectedItem.ToString();
            int Amount = Convert.ToInt16(BlockcountLBL.Text);
            Autodesk.AutoCAD.DatabaseServices.PlotRotation PltRot = Autodesk.AutoCAD.DatabaseServices.PlotRotation.Degrees090;
            string TabOption;
            MyPlotParams mpp = null;
            TabOption = "Current";
            if (
                string.Compare(DeviceName, string.Empty) != 0
                &&
                string.Compare(PaperSize, string.Empty) != 0
                &&
                string.Compare(ctbFile, string.Empty) != 0
                &&
                string.Compare(Scale, string.Empty) != 0
               )
            {
                for (int i = 0; i < ScaleValueArray.Length; ++i)
                {
                    if (string.Compare(Scale, ScaleValueArray[i, 0]) == 0)
                    {
                        RealScale = ScaleValueArray[i, 1];
                        i = ScaleValueArray.Length;
                    }
                }
                StdScaleType ScaleType = (StdScaleType)Enum.Parse(typeof(StdScaleType), RealScale, false);
                if (!checkBoxLandscape.Checked) 
                    PltRot = Autodesk.AutoCAD.DatabaseServices.PlotRotation.Degrees000;
                mpp = new MyPlotParams(null, DeviceName, PaperSize, ctbFile,
                                        checkBoxLineWeight.Checked,
                                        Amount,
                                        ScaleType,
                                        PltRot,
                                        GlbCanonicalArray[LS_Size.SelectedIndex]);
                mpp.TabOption = TabOption;
                mpp.ApplyStamp = false;
                mpp.PlotToFile = checkBoxFile.Checked;
                mpp.PlotFileLocation = db.Filename.Remove(db.Filename.Length-4);
                mpp.Cnt = BlockNO;
                
            }
            return mpp;
        }

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
            BlockcountLBL.Text = "0";
            comboBoxScale.Items.Clear();
            comboBoxScale.Items.Clear();
            textBoxScale.Text = "";    
            LoadPlotProp();
        }        

        public void LoadPlotProp()
        {

            string tempStr;
            bool tempTest = false;
            Type Scales = typeof(StdScaleType);
            string[] ScaleArray = Enum.GetNames(Scales);
            int i = 0;
            ScaleValueArray = new string[ScaleArray.Length, 2];
            foreach (string str in Enum.GetNames(Scales))
            {
                tempStr = FormatStandardScale(str);
                comboBoxScale.Items.Add(tempStr);

                ScaleValueArray[i, 0] = tempStr;
                ScaleValueArray[i, 1] = str;
                ++i;
                if (string.Compare(tempStr, GlbScale) == 0) 
                    tempTest = true;
            }
            comboBoxScale.SelectedIndex = 0;


            StringCollection ctbNames = PCM.ColorDependentPlotStyles;
            foreach (string str in ctbNames)
            {
                string[] tempStrArray = str.Split(new char[] { '\\' });
                comboBoxPlotStyle.Items.Add(tempStrArray[tempStrArray.Length - 1]);
                if (string.Compare(tempStrArray[tempStrArray.Length - 1], GlbctbFile) == 0) tempTest = true;
            }
            if (tempTest)
            {
                comboBoxPlotStyle.Text = GlbctbFile;
                tempTest = false;
            }
            else 
                comboBoxPlotStyle.Text = comboBoxPlotStyle.Items[0].ToString();
            comboBoxPlotStyle.SelectedIndex = 0;
        }

        public void loadDeviceList(ref PlotSettingsValidator psv, ref StringCollection devlist)
        {
            
            // Assign default return values
            string DeviceName = "", PaperSize = "";
            psv = PlotSettingsValidator.Current;
            // Let's first select the device
            devlist = psv.GetPlotDeviceList();

            
        }

        public string FormatStandardScale(string str)
        {
            if (IsInString(str, "StdScale"))
            {
                str = str.Substring(8);
                if (IsInString(str, "Inch"))
                {
                    str = str.Replace("To", "/");
                    str = str.Replace("Inch", "\"");
                    str = str.Replace("Is", " = ");
                    str = str.Replace("ft", "\'");
                    return str;
                }
                else if (string.Compare(str, "1ftIs1ft") == 0)
                {
                    return "1\' = 1\'";
                }
                else
                {
                    str = str.Replace("To", ":");
                    return str;
                }
            }
            else return "Scale to Fit";
        }

        public bool IsInString(string ToCheck, string InQuestion)
        {
            for (int i = 0; i + InQuestion.Length < ToCheck.Length; ++i)
            {
                if (string.Compare(ToCheck.Substring(i, InQuestion.Length), InQuestion) == 0)
                {
                    return true;
                }
            }
            return false;
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
                PlotConfig pc = PCM.SetCurrentConfig(LS_PrinterName.SelectedItem.ToString());
                GlbCanonicalArray = new string[pc.CanonicalMediaNames.Count];
                int i = 0;
                psv.SetPlotConfigurationName(ps, LS_PrinterName.SelectedItem.ToString(), null);
                psv.RefreshLists(ps);
                StringCollection medlist = psv.GetCanonicalMediaNameList(ps);
                foreach (string medlist2 in medlist)
                {
                    LS_Size.Items.Add(medlist2);
                    GlbCanonicalArray[i] = medlist2;
                    i++;
                }
            }
            LS_Size.SelectedIndex = 0;
        }
        

        private void Plot_BTN_Click(object sender, EventArgs e)
        {
            Point2dCollection win = ExtentPoints();

            int numsheet = 5;// win.Count/2;
            WindowPlot( win);
            //this.Close();
        }        

        public Point2dCollection ExtentPoints()
        {            
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

        public int Block_NO()
        {
            BlockNO = 0;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                var blockTable = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                // open the model space BlockTableRecord
                var modelSpace = (BlockTableRecord)tr.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                // iterate through the model space 
                foreach (ObjectId id in modelSpace)
                {
                    // check if the current ObjectId is a block reference one
                    if (id.ObjectClass.DxfName == "INSERT")
                    {
                        // open the block reference
                        var bl = (BlockReference)tr.GetObject(id, OpenMode.ForRead);
                        if (bl.Name == blockname)
                        {
                            BlockNO++;  
                        }
                    }
                }
                tr.Commit();
                
            }
            return BlockNO;
        }

        private void BlockSelect_BTN_Click(object sender, EventArgs e)
        {
            BlockcountLBL.Text = "0";
            PromptSelectionResult polsel = ed.GetSelection();           
            
            if (polsel.Status == PromptStatus.Error) return;
            SelectionSet BLockAR = polsel.Value;
            if (BLockAR != null)
            {
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
                BlockcountLBL.Text = Block_NO().ToString();
                labelBlkName.Text = blockname;
            }
            else
                MessageBox.Show("Please Select Block Sample!");
            
        }       

        public void WindowPlot (Point2dCollection win)
        {            
            string DeviceName = LS_PrinterName.SelectedItem.ToString();
            string PaperSize = LS_Size.SelectedItem.ToString();
            int s = 1;
            for (int i=0; i< win.Count-1; i=i+2 )
            {
                Point3d p1 = new Point3d(win [i].X, win [i].Y, 0);
                Point3d p2 = new Point3d(win [i+1].X, win [i+1].Y, 0);

                // Transform from UCS to DCS

                ResultBuffer rbFrom = new ResultBuffer(new TypedValue(5003, 1)), rbTo = new ResultBuffer(new TypedValue(5003, 2));
                Point3d firres = new Point3d(0, 0, 0);
                Point3d secres = new Point3d(0, 0, 0);

                // Transform the first point...
                acedTrans(p2, rbFrom.UnmanagedObject, rbTo.UnmanagedObject, 0, out firres);

                // ... and the second
                acedTrans(p1, rbFrom.UnmanagedObject, rbTo.UnmanagedObject, 0, out secres);

                // We can safely drop the Z-coord at this stage
                
                Extents2d window = new Extents2d(firres[0], firres[1], secres[0], secres[1]);
                
                plotset(window, DeviceName, PaperSize, s);
                s++;
            }
        }

        public void plotset(Extents2d window, string DeviceName, string PaperSize, int no)
        {
            MyPlotParams mpp = ApplySettings();
            
            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {

                // We'll be plotting the current layout
                object backgroundPlot = Application.GetSystemVariable("BACKGROUNDPLOT");
                Application.SetSystemVariable("BACKGROUNDPLOT", 0);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForRead);

                Layout lo = (Layout)tr.GetObject(btr.LayoutId, OpenMode.ForRead);

                // We need a PlotInfo object
                // linked to the layout

                PlotInfo pi = new PlotInfo();
                pi.Layout = btr.LayoutId;

                // We need a PlotSettings object
                // based on the layout settings
                // which we then customize

                PlotSettings ps = new PlotSettings(lo.ModelType);
                ps.CopyFrom(lo);

                // The PlotSettingsValidator helps
                // create a valid PlotSettings object

                PlotSettingsValidator psv = PlotSettingsValidator.Current;

                // We'll plot the extents, centered and
                // scaled to fit

                if (textBoxScale.Text == "0")
                    MessageBox.Show("Scale must not be zero!");
                else
                {
                    if (textBoxScale.Text != "")
                    {
                        CustomScale type = new CustomScale(1, Convert.ToDouble(textBoxScale.Text));
                        //psv.SetUseStandardScale(ps, false);
                        psv.SetCustomPrintScale(ps, type);
                    }
                    else
                    {
                        psv.SetUseStandardScale(ps, true);
                        psv.SetStdScaleType(ps, mpp.AcScaleType);
                    }
                }
                

                psv.SetPlotWindowArea(ps, window);
                psv.SetPlotType(ps, Autodesk.AutoCAD.DatabaseServices.PlotType.Window);
                
                
                //psv.SetCustomPrintScale(ps,CustomScale type)
                psv.SetPlotCentered(ps, true);
                psv.SetCurrentStyleSheet(ps, mpp.ctbFile);
                // We'll use the standard DWF PC3, as
                // for today we're just plotting to file
                ps.ScaleLineweights = mpp.ScaleLineweight;
                psv.SetPlotRotation(ps, mpp.AcPlotRotation);
                psv.SetPlotConfigurationName(ps, mpp.Device, mpp.Paper);
                psv.SetPlotRotation(ps, mpp.AcPlotRotation);

                // We need to link the PlotInfo to the
                // PlotSettings and then validate it

                pi.OverrideSettings = ps;
                PlotInfoValidator piv = new PlotInfoValidator();
                piv.MediaMatchingPolicy = MatchingPolicy.MatchEnabled;
                piv.Validate(pi);

                // A PlotEngine does the actual plotting

                // (can also create one for Preview)

                if (PlotFactory.ProcessPlotState == ProcessPlotState.NotPlotting)
                {
                    PlotEngine pe = PlotFactory.CreatePublishEngine();
                    using (pe)
                    {

                        // Create a Progress Dialog to provide info
                        // and allow thej user to cancel

                        PlotProgressDialog ppd = new PlotProgressDialog(false, 1, true);

                        using (ppd)
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
                            
                            pe.BeginDocument(pi, doc.Name, null, 1, mpp.PlotToFile, mpp.PlotFileLocation+"-"+no.ToString()+".pdf");

                            // Which contains a single sheet

                            ppd.OnBeginSheet();
                            ppd.LowerSheetProgressRange = 0;
                            ppd.UpperSheetProgressRange = 100;
                            ppd.SheetProgressPos = 0;
                            PlotPageInfo ppi = new PlotPageInfo();
                            pe.BeginPage(ppi, pi, true, null);
                            pe.BeginGenerateGraphics(null);
                            pe.EndGenerateGraphics(null);

                            // Finish the sheet

                            pe.EndPage(null);
                            ppd.SheetProgressPos = 100;
                            ppd.OnEndSheet();

                            // Finish the document

                            pe.EndDocument(null);

                            // And finish the plot

                            ppd.PlotProgressPos = 100;
                            ppd.OnEndPlot();
                            pe.EndPlot(null);
                            ppd.Destroy();
                            pe.Destroy();
                        }
                    }
                }
                else
                {
                    ed.WriteMessage("\nAnother plot is in progress.");
                }
                tr.Commit();
            }
        }
        
    }
}
