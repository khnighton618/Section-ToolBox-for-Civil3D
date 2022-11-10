using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
namespace Sections
{
    public partial class CSDPSectionExport : Form
    {
        private StreamReader filereader;
        Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
        Database db = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Database;
        Autodesk.AutoCAD.DatabaseServices.Polyline PolyLine = null;
        private StreamWriter filewriter;
        List<string> sbG = new List<string>();
        List<string> sbR = new List<string>();
        List<string> exEG = new List<string>();
        List<string> exFG = new List<string>();
        List<double> liny = new List<double>();
        List<string> sbsub = new List<string>();
        List<string> exsub = new List<string>();
        Point2dCollection PolyPoints = new Point2dCollection();
        int NOErr = 0;
        int indexDat = 0;
        int indexst = 0;
        int indexscale = 0;
        int indexblock = 0;
        int indexpl = 0;
        string STRError;
        double maxX;
        double maxY;
        double minX;
        double minY;
        double Area;
        string sLayerName1 = "PolyEG";
        string sLayerName2 = "PolyFG";
        SelectionSet selSet;
        TypedValue[] tv = new TypedValue[] { new TypedValue((int)DxfCode.Start, "*POLY*") };
        TypedValue[] tvline = new TypedValue[] { new TypedValue((int)DxfCode.Start, "LINE") };
        TypedValue[] tv2 = { new TypedValue(0, "INSERT"), new TypedValue((int)DxfCode.BlockName, "ARROW") };
        int indexG = 0;
        int GlobalIndex = 0;
        int indexR = 0;
        string KMStlye;
        double X_KMText1;
        double H_KMText;
        double X_KMText2 = -1e10;
        string kmstr;
        int KMcolor;
        double KMrot;
        double AveXKMText1;
        double AveXKMText2 = -1e-10;
        double datX = 5;
        double MtextDatum;
        string datumTex;
        Stopwatch elaptime = new Stopwatch();
        public void SaveFiles(List<string> inputTXTEG, List<string> inputTXTFG, string otherfiles)
        {
            string filename;
            string f1;
            string f2;
            DialogResult result = new DialogResult();
            using (SaveFileDialog filechooser = new SaveFileDialog())
            {
                filechooser.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                filechooser.FilterIndex = 1;
                result = filechooser.ShowDialog();
                filechooser.DefaultExt = "EG.txt";
                filename = filechooser.FileName;
            }
            if (result == DialogResult.OK)
            {
                try
                {
                    if (otherfiles == "EG-FG")
                    {
                        
                        f1 = filename.Trim(".txt".ToCharArray()) + "-EG.txt";
                        FileStream inputEG = new FileStream(f1, FileMode.OpenOrCreate, FileAccess.Write);
                        filewriter = new StreamWriter(inputEG);
                        for (int i = 0; i < inputTXTEG.Count; i++)
                        {
                            filewriter.WriteLine(inputTXTEG[i]);
                        }
                        filewriter.Close();
                        if (inputTXTFG != null)
                        {
                            f2 = filename.Trim(".txt".ToCharArray()) + "-FG.txt";
                            FileStream inputFG = new FileStream(f2, FileMode.OpenOrCreate, FileAccess.Write);
                            filewriter = new StreamWriter(inputFG);
                            for (int i = 0; i < inputTXTFG.Count; i++)
                            {
                                filewriter.WriteLine(inputTXTFG[i]);
                            }
                        }
                        filewriter.Close();
                    }
                    else
                    { 
                        f1 = filename;
                        if (otherfiles == "Sub")
                            f1 = filename.Split('.')[0] + "-Sub.txt";
                        FileStream inputEG = new FileStream(f1, FileMode.OpenOrCreate, FileAccess.Write);
                        filewriter = new StreamWriter(inputEG);
                        for (int i = 0; i < inputTXTEG.Count; i++)
                        {
                            filewriter.WriteLine(inputTXTEG[i]);
                        }
                        filewriter.Close();
                    }
                    
                }
                catch (IOException)
                {
                    MessageBox.Show("Error writing to file", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //System.Diagnostics.Process.Start(f1);
            //System.Diagnostics.Process.Start(f2);
        }
        public void ErrorDebugger(int indexDat, int indexst, int indexscale, int indexblock, int indexpl,
            int GlobalIndex1, double sta)
        {
            int v = 0;
            if (indexst == 0 || indexDat == 0 || indexscale == 0 || indexblock == 0 || indexpl == 0)
            {
                v++;
                if (indexst == 0)
                    //if (sta == -1e10) sta = 0;
                    STRError = STRError + "\n You must edit Station Text or Select another One at Station: " + sta.ToString();
                //MessageBox.Show("You must edit Station Text or Select another One!", "Station Text Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (indexDat == 0)
                    STRError = STRError + "\n You must edit Datum Text or Select another One at Station: " + sta.ToString();
                //MessageBox.Show("You must edit Datum Text or Select another One!", "Datum Text Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (indexscale == 0)
                    STRError = STRError + "\n You must edit Scale Text at the bottom Left or Select another On at Station: " + sta.ToString();
                //MessageBox.Show("You must edit Scale Text at the bottom Left or Select another One!", "Scale Text Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (indexblock == 0)
                    STRError = STRError + "\n You must edit Datum Block Text or Select another One at Station: " + sta.ToString();
                //MessageBox.Show("You must edit Datum Block Text or Select another One!", "Datum Block Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (indexpl == 0)
                    STRError = STRError + "\n You must edit PolyLine or Select another One at Station: " + sta.ToString();
                //MessageBox.Show("You must edit PolyLine or Select another One!", "PolyLine Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
                return;

            NOErr = NOErr + v;

            //ErrorNO.Text = "Number of Error: " + v.ToString();
        }
        public void createPolyLine(TypedValue[] fltPol, Point2dCollection po2dcol)
        {
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    int v = 0;
                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                    Autodesk.AutoCAD.DatabaseServices.Polyline pol2d = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                    pol2d.SetDatabaseDefaults();
                    foreach (Point2d po in po2dcol)
                    {
                        pol2d.AddVertexAt(v, po, 0, 0, 0);
                        v++;
                    }
                    pol2d.Closed = false;
                    if ((int)fltPol[1].Value == indexG)
                        pol2d.Layer = sLayerName1;
                    else
                        pol2d.Layer = sLayerName2;

                    pol2d.ColorIndex = (int)fltPol[1].Value;
                    btr.AppendEntity(pol2d);
                    tr.AddNewlyCreatedDBObject(pol2d, true);
                    tr.Commit();


                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
            }
        }
        public void CreateFiles()
        {
            TypedValue[] fltGreenPol = { new TypedValue(0, "*POLY*"), new TypedValue((int)DxfCode.Color, indexG) };
            TypedValue[] fltRedPol = { new TypedValue(0, "*POLY*"), new TypedValue((int)DxfCode.Color, indexR) };
            TypedValue[] fltsub = { new TypedValue(0, "*POLY*"), new TypedValue((int)DxfCode.Color, 194) };
            sbG.Clear();
            sbR.Clear();
            exEG.Clear();
            exFG.Clear();
            sbsub.Clear();
            exsub.Clear();
            sbG.Add("STA,OFFSET,Z");
            sbR.Add("STA,OFFSET,Z");
            sbsub.Add("STA,OFFSET,Z");
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    #region Layer
                    LayerTable acLyrTbl;
                    acLyrTbl = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                    if (acLyrTbl.Has(sLayerName1) == false)
                    {
                        using (LayerTableRecord acLyrTblRec = new LayerTableRecord())
                        {
                            // Assign the layer the ACI color 3 and a name
                            //acLyrTblRec.Color = Color.FromColorIndex(ColorMethod.ByAci, 3);
                            acLyrTblRec.Name = sLayerName1;
                            // Upgrade the Layer table for write
                            acLyrTbl.UpgradeOpen();
                            // Append the new layer to the Layer table and the transaction
                            acLyrTbl.Add(acLyrTblRec);
                            tr.AddNewlyCreatedDBObject(acLyrTblRec, true);
                        }
                    }
                    if (acLyrTbl.Has(sLayerName2) == false)
                    {
                        using (LayerTableRecord acLyrTblRec = new LayerTableRecord())
                        {
                            // Assign the layer the ACI color 3 and a name
                            //acLyrTblRec.Color = Color.FromColorIndex(ColorMethod.ByAci, 3);
                            acLyrTblRec.Name = sLayerName2;
                            // Upgrade the Layer table for write
                            acLyrTbl.UpgradeOpen();
                            // Append the new layer to the Layer table and the transaction
                            acLyrTbl.Add(acLyrTblRec);
                            tr.AddNewlyCreatedDBObject(acLyrTblRec, true);
                        }
                    }
                    #endregion
                    #region initial values
                    List<double> stalist = new List<double>();
                    int linIndex = 0;
                    double maxpolyX = 0;
                    double maxpolyY = 0;
                    double minpolyX = 0;
                    double minpolyY = 0;
                    double aveX = 0;
                    double aveY = 0;
                    int linID = 0;
                    double sta = 0;
                    int progbarID = 0;
                    //RTXTBox.Clear();
                    //EGExportTXT.Clear();
                    System.Data.DataTable tbG = new System.Data.DataTable();
                    tbG.Columns.Add("STA", typeof(double));
                    tbG.Columns.Add("OFF", typeof(double));
                    tbG.Columns.Add("H", typeof(double));
                    System.Data.DataTable tbR = new System.Data.DataTable();
                    tbR.Columns.Add("STA", typeof(double));
                    tbR.Columns.Add("OFF", typeof(double));
                    tbR.Columns.Add("H", typeof(double));

                    System.Data.DataTable tbsub = new System.Data.DataTable();
                    tbsub.Columns.Add("STA", typeof(double));
                    tbsub.Columns.Add("OFF", typeof(double));
                    tbsub.Columns.Add("H", typeof(double));

                    ObjectId[] ids = selSet.GetObjectIds();
                    //progressBar1.Maximum = ids.Count();
                    ProgBar.Maximum = ids.Count();
                    ProgBar.Step = 1;// / ids.Count();
                    ProgBar.Value = 0;
                    #endregion
                    foreach (ObjectId entId in ids)
                    {
                        progbarID++;
                        ProgBar.Value = progbarID;// / ids.Count() * 1000;
                        //Percentlbl.Text = ProgBar.Value.ToString();
                        //Percentlbl.Text = Math.Floor(Convert.ToDouble(progbarID / ProgBar.Maximum)*100).ToString() + "%";

                        Autodesk.AutoCAD.DatabaseServices.Polyline poly = tr.GetObject(entId, OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.Polyline;
                        if (poly != null && Math.Abs(poly.Area - Area) <= 0.001)
                        {
                            
                            percentLbl.Text = "Sec: "+progbarID.ToString();
                            maxpolyX = poly.Bounds.Value.MaxPoint.X;
                            maxpolyY = poly.Bounds.Value.MaxPoint.Y;
                            minpolyX = poly.Bounds.Value.MinPoint.X;
                            minpolyY = poly.Bounds.Value.MinPoint.Y;
                            maxX = maxpolyX;
                            maxY = maxpolyY;
                            minX = minpolyX;
                            minY = minpolyY;
                            aveX = (minpolyX + maxpolyX) / 2;
                            aveY = (minpolyY + maxpolyY) / 2;
                            linID = 0;
                            liny.Clear();
                            PromptSelectionResult polsel;
                            polsel = ed.SelectCrossingWindow(new Point3d(minpolyX, minpolyY - 0.1 * (Convert.ToDouble(ScaleBox.Text)), 0), new Point3d(maxpolyX, maxpolyY + 0.1 * (Convert.ToDouble(ScaleBox.Text)), 0),
                                new SelectionFilter(tvline));
                            if (polsel.Status == PromptStatus.Error) return;
                            SelectionSet Dat = polsel.Value;

                            for (int i = 0; i < Dat.Count; i++)
                            {
                                Line lin = tr.GetObject(Dat[i].ObjectId, OpenMode.ForRead) as Line;
                                if (lin != null && Math.Abs(lin.StartPoint.X - minpolyX) > .001 || lin.StartPoint.Y > maxpolyY
                                    || lin.StartPoint.Y < minpolyY - 0.1 * (Convert.ToDouble(ScaleBox.Text))
                                    || Math.Abs(lin.StartPoint.X - maxpolyX) < .001) continue;
                                if (lin != null && (Math.Abs(maxpolyX - minpolyX - lin.Length) <= .001))
                                {
                                    liny.Add(lin.StartPoint.Y);
                                    linID++;
                                }
                            }
                            liny.Sort();
                            Point2dCollection po2dcol1 = new Point2dCollection();
                            Point2dCollection po2dcolsub = new Point2dCollection();
                            if (linID == 1)
                            {
                                //--- زمین طبیعی----
                                DataView dv1G = null;
                                DataView dvSub = null;
                                exportTable(maxpolyX, maxpolyY, minpolyX, minpolyY, fltGreenPol, ref dv1G, ref sta, ref po2dcol1
                                    , ref indexDat, ref indexst, ref indexscale, ref indexblock, ref indexpl, ref dvSub, ref po2dcolsub);
                                ErrorDebugger(indexDat, indexst, indexscale, indexblock, indexpl, GlobalIndex, sta);
                                createPolyLine(fltGreenPol, po2dcol1);
                                po2dcol1.Clear();
                                po2dcolsub.Clear();
                                stalist.Add(sta);
                                DataView dv1R = null;
                                if (chkEGFGOnly.Checked == false)
                                {
                                    //---سطح نهایی----
                                    
                                    exportTable(maxpolyX, maxpolyY, minpolyX, minpolyY, fltRedPol, ref dv1R, ref sta, ref po2dcol1
                                        , ref indexDat, ref indexst, ref indexscale, ref indexblock, ref indexpl, ref dvSub, ref po2dcolsub);
                                    ErrorDebugger(indexDat, indexst, indexscale, indexblock, indexpl, GlobalIndex, sta);
                                    createPolyLine(fltRedPol, po2dcol1);
                                    createPolyLine(fltsub, po2dcolsub);
                                    po2dcol1.Clear();
                                    po2dcolsub.Clear();
                                }
                                    
                                //---------------------
                                if (stalist.IndexOf(sta) == stalist.LastIndexOf(sta))
                                {
                                    if (chkEGFGOnly.Checked == false)
                                    {
                                        for (int k = 0; k < dv1R.Count; k++)
                                        {
                                            tbR.Rows.Add(sta, dv1R[k][0], dv1R[k][1]);
                                        }
                                        if (chkSub.Checked == true)
                                        {
                                            for (int k = 0; k < dvSub.Count; k++)
                                            {
                                                tbsub.Rows.Add(sta, dvSub[k][0], dvSub[k][1]);
                                            }
                                        }
                                    }
                                    for (int j = 0; j < dv1G.Count; j++)
                                    {
                                        tbG.Rows.Add(sta, dv1G[j][0], dv1G[j][1]);
                                    }
                                }
                            }
                            else if (linID == 0 && linIndex == 0)
                            {
                                MessageBox.Show("You must edit Sheet Bottom line or run overkill command \n or add line at the bottom of each sheets", "SheetBottomLine Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                linIndex = 1;
                            }

                            else
                            {
                                for (int i = 0; i < liny.Count; i++)
                                {

                                    if (i == liny.Count - 1)
                                    {
                                        minpolyY = liny[i];
                                        maxpolyY = poly.Bounds.Value.MaxPoint.Y;
                                    }
                                    else
                                    {
                                        minpolyY = liny[i];
                                        maxpolyY = liny[i + 1];
                                    }
                                    if (Math.Abs(minpolyY - maxpolyY) <= 0.001)
                                        continue;
                                    //--- زمین طبیعی----
                                    DataView dv1G = null;
                                    DataView dvSub = null;
                                    exportTable(maxpolyX, maxpolyY, minpolyX, minpolyY, fltGreenPol, ref dv1G, ref sta, ref po2dcol1
                                        , ref indexDat, ref indexst, ref indexscale, ref indexblock, ref indexpl, ref dvSub, ref po2dcolsub);
                                    ErrorDebugger(indexDat, indexst, indexscale, indexblock, indexpl, GlobalIndex, sta);
                                    createPolyLine(fltGreenPol, po2dcol1);
                                    po2dcol1.Clear();
                                    po2dcolsub.Clear();
                                    stalist.Add(sta);
                                    //---سطح نهایی----
                                    DataView dv1R = null;
                                    if (chkEGFGOnly.Checked == false)
                                    {
                                        exportTable(maxpolyX, maxpolyY, minpolyX, minpolyY, fltRedPol, ref dv1R, ref sta, ref po2dcol1
                                        , ref indexDat, ref indexst, ref indexscale, ref indexblock, ref indexpl, ref dvSub, ref po2dcolsub);
                                        ErrorDebugger(indexDat, indexst, indexscale, indexblock, indexpl, GlobalIndex, sta);
                                        createPolyLine(fltRedPol, po2dcol1);
                                        createPolyLine(fltsub, po2dcolsub);
                                        po2dcol1.Clear();
                                        po2dcolsub.Clear();
                                    }
                                    //---------------------
                                    if (stalist.IndexOf(sta) == stalist.LastIndexOf(sta))
                                    {
                                        if (chkEGFGOnly.Checked == false)
                                        {
                                            for (int k = 0; k < dv1R.Count; k++)
                                            {
                                                tbR.Rows.Add(sta, dv1R[k][0], dv1R[k][1]);
                                            }
                                            if (chkSub.Checked == true)
                                            {
                                                for (int k = 0; k < dvSub.Count; k++)
                                                {
                                                    tbsub.Rows.Add(sta, dvSub[k][0], dvSub[k][1]);
                                                }
                                            }
                                        }
                                        for (int j = 0; j < dv1G.Count; j++)
                                        {
                                            tbG.Rows.Add(sta, dv1G[j][0], dv1G[j][1]);
                                        }
                                    }
                                }
                            }

                        }
                    }

                    DataView dv3 = new DataView(tbG);
                    dv3.Sort = "STA ASC";
                    for (int i = 0; i < dv3.Count; i++)
                    {
                        sbG.Add(dv3[i][0].ToString() + "," + dv3[i][1].ToString() + "," + dv3[i][2].ToString());
                    }
                    exEG.Add("chainage" + dv3[0][0].ToString());
                    string station = dv3[0][0].ToString();
                    for (int i = 0; i < dv3.Count; i++)
                    {
                        if (dv3[i][0].ToString() == station)
                        {
                            exEG.Add(dv3[i][1].ToString() + "," + dv3[i][2].ToString());
                        }
                        else
                        {
                            exEG.Add("chainage" + dv3[i][0].ToString());
                            exEG.Add(dv3[i][1].ToString() + "," + dv3[i][2].ToString());
                            station = dv3[i][0].ToString();
                        }
                    }
                    if (chkEGFGOnly.Checked == false)
                    {
                        DataView dv4 = new DataView(tbR);
                        dv4.Sort = "STA ASC";
                        
                        for (int i = 0; i < dv4.Count; i++)
                        {
                            sbR.Add(dv4[i][0].ToString() + "," + dv4[i][1].ToString() + "," + dv4[i][2].ToString());
                        }
                        exFG.Add("chainage" + dv4[0][0].ToString());
                        station = dv4[0][0].ToString();
                        for (int i = 0; i < dv4.Count; i++)
                        {
                            if (dv4[i][0].ToString() == station)
                            {
                                exFG.Add(dv4[i][1].ToString() + "," + dv4[i][2].ToString());
                            }
                            else
                            {
                                exFG.Add("chainage" + dv4[i][0].ToString());
                                exFG.Add(dv4[i][1].ToString() + "," + dv4[i][2].ToString());
                                station = dv4[i][0].ToString();
                            }
                        }
                        if (chkSub.Checked == true)
                        {
                            DataView dv5 = new DataView(tbsub);
                            dv5.Sort = "STA ASC";
                            for (int i = 0; i < dv5.Count; i++)
                            {
                                sbsub.Add(dv5[i][0].ToString() + "," + dv5[i][1].ToString() + "," + dv5[i][2].ToString());
                            }
                            exsub.Add("chainage" + dv5[0][0].ToString());
                            station = dv5[0][0].ToString();
                            for (int i = 0; i < dv5.Count; i++)
                            {
                                if (dv5[i][0].ToString() == station)
                                {
                                    exsub.Add(dv5[i][1].ToString() + "," + dv5[i][2].ToString());
                                }
                                else
                                {
                                    exsub.Add("chainage" + dv5[i][0].ToString());
                                    exsub.Add(dv5[i][1].ToString() + "," + dv5[i][2].ToString());
                                    station = dv5[i][0].ToString();
                                }
                            }
                        }
                    }

                    checkBoxFile.Enabled = true;
                    checkBoxFile.CheckState = CheckState.Checked;
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.ToString());
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
                tr.Commit();
            }

            if (STRError != null)
                MessageBox.Show(STRError, "Error List", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public CSDPSectionExport()
        {
            InitializeComponent();
        }
        public void exportTable(double maxpolyX, double maxpolyY, double minpolyX, double minpolyY, TypedValue[] fltPol,
            ref DataView dv, ref double sta, ref Point2dCollection po2dcol, ref int indexDat, ref int indexst, ref int indexscale
            , ref int indexblock, ref int indexpl, ref DataView dvSub, ref Point2dCollection po2dcolsub)
        {

            indexDat = 0;
            indexst = 0;
            indexscale = 0;
            indexblock = 0;
            indexpl = 0;
            TypedValue[] tvTEX = new TypedValue[] { new TypedValue((int)DxfCode.Start, "TEXT") };
            TypedValue[] fltRedPol = { new TypedValue(0, "*POLY*"), new TypedValue((int)DxfCode.Color, 1) };
            TypedValue[] filter = { new TypedValue(0, "INSERT"), new TypedValue((int)DxfCode.BlockName, "ARROW") };
            po2dcol = new Point2dCollection();
            po2dcolsub = new Point2dCollection();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    System.Data.DataTable table = new System.Data.DataTable();
                    table.Columns.Add("OFF", typeof(double));
                    table.Columns.Add("H", typeof(double));
                    table.Columns.Add("Xdwg", typeof(double));
                    table.Columns.Add("Ydwg", typeof(double));
                    System.Data.DataTable tb2 = new System.Data.DataTable();
                    tb2.Columns.Add("OFF", typeof(double));
                    tb2.Columns.Add("H", typeof(double));
                    tb2.Columns.Add("Xdwg", typeof(double));
                    tb2.Columns.Add("Ydwg", typeof(double));
                    double aveX = (minpolyX + maxpolyX) / 2;
                    double aveY = (minpolyY + maxpolyY) / 2;                  
                    string km1 = "";
                    string km2 = "A";
                    string Stationstr = "";
                    string b = string.Empty;
                    double Station;
                    double Datum = 0;
                    sta = -1e10;
                    double blPosY = 0;
                    int ID = 0;
                    double x = 0;
                    double y = 0;
                    //----پیدا کردن تکست دیتوم و کیلومتراژ
                    PromptSelectionResult polsel;
                    polsel = ed.SelectCrossingWindow(new Point3d(minpolyX, minpolyY, 0), new Point3d(maxpolyX, maxpolyY, 0), new SelectionFilter(tvTEX));
                    if (polsel.Status == PromptStatus.Error) return;
                    SelectionSet DatTexset = polsel.Value;
                    ObjectId[] ids2 = DatTexset.GetObjectIds();
                    string[] str = { "C", "F", "S", "P", "G", "T", "*", "+", "Y", "X" };
                    double ScBox = 0;
                    if (ScaleBox.Text == null)
                    {
                        ScBox = 1000;
                    }
                    else
                    {
                        ScBox = Convert.ToDouble(ScaleBox.Text);
                    }

                    foreach (ObjectId entId2 in ids2)
                    {
                        DBText mtex1 = tr.GetObject(entId2, OpenMode.ForRead) as DBText;
                        if (mtex1.ColorIndex != 7 || mtex1.TextString.StartsWith("A") || mtex1.TextString.StartsWith("B") || mtex1.TextString.StartsWith("C")
                            || mtex1.TextString.StartsWith("D") || mtex1.TextString.StartsWith("E") || mtex1.TextString.StartsWith("F")
                            || mtex1.TextString.StartsWith("G") || mtex1.TextString.StartsWith("H") || mtex1.TextString.StartsWith("I")
                            || mtex1.TextString.StartsWith("T") || mtex1.TextString.StartsWith("S")
                            || mtex1.TextString.StartsWith("P") || mtex1.TextString.StartsWith("X")
                            || mtex1.TextString.StartsWith("Y") || mtex1.TextString.StartsWith("*") || mtex1.Rotation != 0
                            || mtex1.Position.Y <= minpolyY || mtex1.Position.Y >= maxpolyY)
                            continue;
                        if (mtex1 != null && mtex1.Rotation == 0 && Math.Abs(mtex1.Position.X - minpolyX - datX) <= 0.01 && Datum == 0)
                        {
                            Datum = Convert.ToDouble(mtex1.TextString);
                            indexDat = 1;
                            Point2d po = new Point2d(mtex1.Position.X, mtex1.Position.Y);
                            po2dcol.Add(po);
                            continue;
                        }
                        if (mtex1 != null && Math.Abs(aveX - mtex1.Position.X - AveXKMText1) <= ScBox && mtex1.Position.X <= aveX && (
                            Math.Abs(mtex1.Height - H_KMText) < .01 || mtex1.TextString.StartsWith("Km")
                            || mtex1.TextString.StartsWith("KM") || mtex1.TextString.StartsWith("kM") || mtex1.TextString.StartsWith("km")))
                        {
                            km1 = mtex1.TextString;
                            Point2d po = new Point2d(mtex1.Position.X, mtex1.Position.Y);
                            po2dcol.Add(po);
                        }
                        if (mtex1 != null && AveXKMText2 != -1e-10 && Math.Abs(mtex1.Position.X - aveX - AveXKMText2) <= ScBox &&
                            Math.Abs(mtex1.Height - H_KMText) < .01 && mtex1.Position.X >= aveX ||
                            mtex1.TextString.StartsWith("+") || mtex1.TextString.StartsWith("+ ") || mtex1.TextString.StartsWith(" +") ||
                            mtex1.TextString.StartsWith(" + "))
                        {
                            km2 = mtex1.TextString;
                            Point2d po = new Point2d(mtex1.Position.X, mtex1.Position.Y);
                            po2dcol.Add(po);
                        }
                        if (km1 != "" && km2 != "" && indexst == 0 && km2 != "A")
                        {
                            Stationstr = km1 + km2;
                            for (int i = 0; i < Stationstr.Length; i++)
                            {
                                if (Char.IsDigit(Stationstr[i]) || Stationstr[i] == '.')
                                    b += Stationstr[i];
                            }

                            if (b.Length > 0)
                            {
                                Station = double.Parse(b);
                                sta = Station;
                                indexst = 1;
                            }
                        }
                    }
                    //---------مقدار Scale----
                    polsel = ed.SelectCrossingWindow(new Point3d(minX, minY, 0), new Point3d(maxX, maxY, 0), new SelectionFilter(tvTEX));
                    if (polsel.Status == PromptStatus.Error) return;
                    SelectionSet DatTexset2 = polsel.Value;
                    ObjectId[] ids22 = DatTexset2.GetObjectIds();
                    double Verts = 1;
                    double Hors = 1;
                    foreach (ObjectId entId22 in ids22)
                    {
                        DBText mtex1 = tr.GetObject(entId22, OpenMode.ForRead) as DBText;
                        if (mtex1.TextString.StartsWith("Scale") && mtex1.Position.X >= minX && mtex1.Position.X <= maxX
                            && mtex1.Position.Y >= minY && mtex1.Position.Y <= maxY)
                        {
                            Hors = Convert.ToDouble(mtex1.TextString.Split(' ')[1].Split(')')[0].Split(':')[2]);
                            Verts = Convert.ToDouble(mtex1.TextString.Split(' ')[2].Split(')')[0].Split(':')[2]);
                            indexscale = 1;
                            //Point2d po = new Point2d(mtex1.Position.X, mtex1.Position.Y);
                            //po2dcol.Add(po);
                            break;
                        }
                    }
                    //--------پیدا کردن ایگرگ بلوک دیتوم
                    polsel = ed.SelectCrossingWindow(new Point3d(minpolyX, minpolyY, 0), new Point3d(maxpolyX, maxpolyY, 0),
                    new SelectionFilter(filter));
                    if (polsel.Status == PromptStatus.Error) return;
                    SelectionSet BLockAR = polsel.Value;
                    ObjectId[] ids3 = BLockAR.GetObjectIds();
                    foreach (ObjectId entId3 in ids3)
                    {
                        BlockReference bl = tr.GetObject(entId3, OpenMode.ForRead) as BlockReference;
                        if (bl != null && bl.Position.Y <= maxpolyY && bl.Position.Y >= minpolyY
                            && bl.Position.X <= maxpolyX && bl.Position.X >= minpolyX)
                        {
                            blPosY = bl.Position.Y;
                            indexblock = 1;
                            Point2d po = new Point2d(bl.Position.X, bl.Position.Y);
                            po2dcol.Add(po);
                        }
                    }
                    //-------تشکیل نقاط از پلی لاین
                    double scale = (ScBox / (Hors / Convert.ToDouble(DefaultScale.Text.ToString())));
                    polsel = ed.SelectCrossingWindow(new Point3d(minpolyX, minpolyY, 0), new Point3d(maxpolyX, maxpolyY, 0),
                        new SelectionFilter(fltPol));
                    if (polsel.Status == PromptStatus.Error) return;
                    SelectionSet PolGreen = polsel.Value;
                    ObjectId[] ids4 = PolGreen.GetObjectIds();
                    foreach (ObjectId entId4 in ids4)
                    {
                        Autodesk.AutoCAD.DatabaseServices.Polyline poly2 = tr.GetObject(entId4, OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.Polyline;
                        //if(chkSub.Checked==false)                       
                            if (poly2 != null && Math.Abs(poly2.StartPoint.X - aveX)/ scale > 1 && chkMultiple.Checked == true && chkTrim.Checked == false)
                                continue;
                        //Math.Abs(poly2.StartPoint.X - aveX) / scale < 1
                        if (poly2 != null && poly2.StartPoint.Y >= minpolyY && poly2.StartPoint.Y <= maxpolyY && poly2.Area != Area && poly2.ColorIndex== (int)fltPol[1].Value)
                        {
                            Point2d po2d = new Point2d();
                            for (int i = ID; i < poly2.NumberOfVertices; i++)
                            {
                                po2d = poly2.GetPoint2dAt(i);
                                x = Math.Round((po2d.X - aveX) / scale, 4);
                                y = Math.Round((Datum + ((-blPosY + po2d.Y) / scale)), 4);
                                if(fltPol[1].Value== checkBoxFG.Text.Split(' ')[checkBoxFG.Text.Split(' ').Length - 1])
                                    if (Math.Abs(x) <= 1 && Math.Abs(x) > 0.01 & chkEGFGOnly.Checked==false)//-----حذف نقاط اضافی خطوط زیر سطح نهایی
                                        continue;
                                table.Rows.Add(x, y, po2d.X, po2d.Y);
                                indexpl = 1;
                            }
                            ID = 1;
                        }
                    }

                    //-------تشکیل نقاط سابگرید   
                    if(chkSub.Checked==true&& fltPol[1].Value.ToString() == "1")
                    {
                        foreach (ObjectId entId4 in ids4)
                        {
                            Autodesk.AutoCAD.DatabaseServices.Polyline poly2 = tr.GetObject(entId4, OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.Polyline;
                            if (poly2 != null && poly2.StartPoint.Y >= minpolyY && poly2.StartPoint.Y <= maxpolyY && poly2.Area != Area && Math.Abs(poly2.StartPoint.X - aveX) > 0)
                            {
                                Point2d po2d = new Point2d();
                                for (int i = 0; i < poly2.NumberOfVertices; i++)
                                {
                                    po2d = poly2.GetPoint2dAt(i);
                                    x = Math.Round((po2d.X - aveX) / (ScBox / (Hors / Convert.ToDouble(DefaultScale.Text.ToString()))), 4);
                                    y = Math.Round((Datum + ((-blPosY + po2d.Y) / (ScBox / (Verts / Convert.ToDouble(DefaultScale.Text.ToString()))))), 4);
                                    tb2.Rows.Add(x, y, po2d.X, po2d.Y);
                                    indexpl = 1;
                                }
                            }
                        }
                        dvSub = new DataView(tb2);                        
                        if(dvSub.Count==0)
                        {
                            MessageBox.Show("In station : "+ sta.ToString()+" there is no subgrade line!");
                        }
                        else
                        {
                            double x1;
                            double x2;
                            double y1;
                            double y2;
                            for (int i = 0; i < po2dcol.Count; i++)
                            {
                                Point2d po = new Point2d(po2dcol[i].X, po2dcol[i].Y);
                                po2dcolsub.Add(po);
                            }
                            int ID2 = 0;
                            x1 = (double)dvSub[0][2];
                            x2 = (double)dvSub[1][2];
                            if (x1 > x2)
                            {
                                dvSub[0].Row.Delete();
                                //ID = 1;
                            }
                            ID = 0;
                            x1 = (double)dvSub[dvSub.Count - 1][2];
                            x2 = (double)dvSub[dvSub.Count - 2][2];
                            if (x1 < x2)
                            {
                                dvSub[dvSub.Count - 1].Row.Delete();
                                //ID2 = 1;
                            }
                            for (int i = 0; i < dvSub.Count; i++)
                            {                                               
                                Point2d po = new Point2d((double)dvSub[i][2], (double)dvSub[i][3]);
                                po2dcolsub.Add(po);
                            }
                        }                        
                    }
                    dv = new DataView(table);
                    dv.Sort = "OFF ASC";
                    if(dvSub != null)
                        dvSub.Sort = "OFF ASC";
                    //------حذف نقاط با فاصله کم از هم---------                    
                    double difx = 0;
                    double dify = 0;
                    if (dv.Count > 2)
                    {
                        for (int i = 0; i < dv.Count; i++)
                        {
                            Point2d po = new Point2d((double)dv[i][2], (double)dv[i][3]);

                            if (i == dv.Count - 1)
                            {
                                difx = Convert.ToDouble(dv[i][0].ToString()) - Convert.ToDouble(dv[i - 1][0].ToString());
                                dify = Convert.ToDouble(dv[i][1].ToString()) - Convert.ToDouble(dv[i - 1][1].ToString());
                            }
                            else
                            {
                                difx = Convert.ToDouble(dv[i + 1][0].ToString()) - Convert.ToDouble(dv[i][0].ToString());
                                dify = Convert.ToDouble(dv[i + 1][1].ToString()) - Convert.ToDouble(dv[i][1].ToString());
                            }

                            if (Math.Abs(difx) * ScBox <= .01 & Math.Abs(dify) * ScBox <= .01 & chkReduce.Checked == true)
                            {
                                dv[i].Row.Delete();
                                continue;
                            }
                            //dvlist.Add(dv[i][0].ToString()); 
                            po2dcol.Add(po);
                        }
                    }                   
                    
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
            }
        }

        private void saveTextFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (chkEGFGOnly.Checked == false)
            {
                SaveFiles(sbG, sbR, "EG-FG");
                if(chkSub.Checked == true)
                    SaveFiles(sbsub, null, "Sub");
            }
            else
                SaveFiles(sbG, null, "EG-FG");
        }
        private void saveGenericFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (chkEGFGOnly.Checked == false)
            {
                SaveFiles(exEG, exFG, "EG-FG");
                if (chkSub.Checked == true)
                    SaveFiles(exsub, null, "Sub");
            }
            else
                SaveFiles(exEG, null, "EG-FG");
            
        }
        private void PolyBTN_Click(object sender, EventArgs e)
        {
            //checkBoxObject.Text = "";
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    Area = 0;
                    PromptSelectionResult se = ed.GetSelection();
                    if (se.Status == PromptStatus.Error) return;
                    SelectionSet Set1 = se.Value;
                    ObjectId[] ids1 = Set1.GetObjectIds();
                    foreach (ObjectId entId1 in ids1)
                    {
                        Autodesk.AutoCAD.DatabaseServices.Polyline poly1 = tr.GetObject(entId1, OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.Polyline;
                        if (poly1 != null)
                        {
                            PolyLine = poly1;
                            Area = poly1.Area;
                            AveXKMText1 = Math.Abs(X_KMText1 - (poly1.Bounds.Value.MaxPoint.X + poly1.Bounds.Value.MinPoint.X) / 2);
                            if (X_KMText2 != -1e10)
                                AveXKMText2 = Math.Abs(X_KMText2 - (poly1.Bounds.Value.MaxPoint.X + poly1.Bounds.Value.MinPoint.X) / 2);
                            //AveYKMText1= Math.Abs(H_KMText - poly1.Bounds.Value.MinPoint.Y);
                            if (datX == 5)
                                datX = Math.Abs(MtextDatum - poly1.Bounds.Value.MinPoint.X);
                            checkBoxObject.Enabled = true;
                            checkBoxObject.CheckState = CheckState.Checked;
                            checkBoxFile.Enabled = false;
                            checkBoxFile.CheckState = CheckState.Unchecked;
                        }
                        else
                            MessageBox.Show("Please select only Polyline not 2Dpolyline or 3Dpolyline or Line...", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //select on screen                    
                    PromptSelectionResult selRes = ed.GetSelection(new SelectionFilter(tv));
                    if (selRes.Status == PromptStatus.Error) return;
                    selSet = selRes.Value;

                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.ToString());
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
                tr.Commit();
            }
        }
        private void EGBTN_Click(object sender, EventArgs e)
        {
            //checkBoxEG.Text = "";
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {

                    PromptSelectionResult se = ed.GetSelection();
                    if (se.Status == PromptStatus.Error) return;
                    SelectionSet Set1 = se.Value;
                    ObjectId[] ids1 = Set1.GetObjectIds();
                    indexG = 0;

                    foreach (ObjectId entId1 in ids1)
                    {
                        Autodesk.AutoCAD.DatabaseServices.Polyline poly1 = tr.GetObject(entId1, OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.Polyline;
                        if (poly1 != null)
                        {
                            indexG = poly1.ColorIndex;
                            checkBoxEG.Enabled = true;
                            checkBoxEG.CheckState = CheckState.Checked;
                            checkBoxEG.Text = "EG Selected : Color is " + indexG.ToString();
                        }
                        else
                            MessageBox.Show("Please select only Polyline not 2Dpolyline or 3Dpolyline or Line...", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.ToString());
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
                tr.Commit();

            }

        }
        private void FGBTN_Click(object sender, EventArgs e)
        {
            //checkBoxFG.Text = "";
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {

                    PromptSelectionResult se = ed.GetSelection();
                    if (se.Status == PromptStatus.Error) return;
                    SelectionSet Set1 = se.Value;
                    ObjectId[] ids1 = Set1.GetObjectIds();
                    indexR = 0;
                    foreach (ObjectId entId1 in ids1)
                    {
                        Autodesk.AutoCAD.DatabaseServices.Polyline poly1 = tr.GetObject(entId1, OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.Polyline;
                        if (poly1 != null)
                        {
                            indexR = poly1.ColorIndex;
                            checkBoxFG.Enabled = true;
                            checkBoxFG.CheckState = CheckState.Checked;
                            checkBoxFG.Text = "FG Selected : Color is " + indexR.ToString();
                        }
                        else
                            MessageBox.Show("Please select only Polyline not 2Dpolyline or 3Dpolyline or Line...", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.ToString());
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
                tr.Commit();

            }

        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sections.AboutBoxCSDPExport win1 = new Sections.AboutBoxCSDPExport();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModelessDialog(win1);
        }
        private void SelKMBTN_Click(object sender, EventArgs e)
        {
            //CHKBXKM.Text = "";
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    PromptSelectionResult se = ed.GetSelection();
                    if (se.Status == PromptStatus.Error) return;
                    SelectionSet Set1 = se.Value;
                    ObjectId[] ids1 = Set1.GetObjectIds();

                    int id = 0;
                    foreach (ObjectId entId1 in ids1)
                    {
                        DBText mtex1 = tr.GetObject(entId1, OpenMode.ForRead) as DBText;
                        if (mtex1 != null)
                        {
                            if (id == 0)
                            {
                                KMStlye = mtex1.TextString;
                                KMcolor = mtex1.ColorIndex;
                                KMrot = mtex1.Rotation;
                                X_KMText1 = mtex1.Position.X;
                                H_KMText = mtex1.Height;
                            }
                            if (id > 0)
                            {
                                X_KMText2 = mtex1.Position.X;
                            }
                            kmstr = kmstr + mtex1.TextString;
                        }
                        id++;
                    }
                    CHKBXKM.Enabled = true;
                    CHKBXKM.CheckState = CheckState.Checked;
                    CHKBXKM.Text = "Your Style is: " + kmstr;
                    //kmstr = "";

                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.ToString());
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
                tr.Commit();
            }
        }
        private void datBTN_Click(object sender, EventArgs e)
        {
            //checkBoxDat.Text = "";
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    PromptSelectionResult se = ed.GetSelection();
                    if (se.Status == PromptStatus.Error) return;
                    SelectionSet Set1 = se.Value;
                    ObjectId[] ids1 = Set1.GetObjectIds();
                    foreach (ObjectId entId1 in ids1)
                    {
                        DBText mtex1 = tr.GetObject(entId1, OpenMode.ForRead) as DBText;
                        if (mtex1 != null)
                        {
                            MtextDatum = Convert.ToDouble(mtex1.Position.X);
                            datumTex = mtex1.TextString;
                            checkBoxDat.Enabled = true;
                            checkBoxDat.CheckState = CheckState.Checked;
                            checkBoxDat.Text = "Datum Text is: " + mtex1.TextString;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.ToString());
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
                tr.Commit();
            }
        }
        private void saveSelectionToAFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> st = new List<string>();
            st.Add(X_KMText1.ToString());
            st.Add(X_KMText2.ToString());
            st.Add(H_KMText.ToString());
            st.Add(kmstr);
            st.Add(datumTex.ToString());
            st.Add(datumTex.ToString());
            st.Add(indexG.ToString());
            st.Add(indexR.ToString());
            st.Add(Area.ToString());
            st.Add(AveXKMText1.ToString());
            st.Add(AveXKMText2.ToString());
            st.Add(datX.ToString());
            st.Add(chkEGFGOnly.Checked.ToString());
            st.Add(ScaleBox.Text.ToString());
            st.Add(DefaultScale.Text);
            st.Add(chkMultiple.Checked.ToString());
            st.Add(chkTrim.Checked.ToString());
            st.Add(chkReduce.Checked.ToString());
            st.Add(chkEGFGOnly.Checked.ToString());
            st.Add(chkSub.Checked.ToString());
            SaveFiles(st, null, "Selection");           
        }
        private void openSelectionFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string filename;
                DialogResult result = new DialogResult();
                using (OpenFileDialog filechooser = new OpenFileDialog())
                {
                    filechooser.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    filechooser.FilterIndex = 1;
                    result = filechooser.ShowDialog();
                    filename = filechooser.FileName;
                }
                if (result == DialogResult.OK)
                {
                    try
                    {
                        FileStream input = new FileStream(filename, FileMode.Open, FileAccess.Read);
                        filereader = new StreamReader(input);
                        string whole_file = filereader.ReadToEnd();
                        string[] inputfields;
                        char[] delim = { '\n', ' ', ' ' };
                        inputfields = whole_file.Split(delim);
                        whole_file = whole_file.Replace('\n', '\r');
                        string[] lines = whole_file.Split(new char[] { '\r', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                        X_KMText1 = Convert.ToDouble(lines[0]);
                        X_KMText2 = Convert.ToDouble(lines[1]);
                        H_KMText = Convert.ToDouble(lines[2]);
                        kmstr = lines[3];
                        MtextDatum = Convert.ToDouble(lines[4]);
                        datumTex = lines[5];
                        indexG = Convert.ToInt32(lines[6]);
                        indexR = Convert.ToInt32(lines[7]);
                        Area = Convert.ToDouble(lines[8]);
                        AveXKMText1 = Convert.ToDouble(lines[9]);
                        AveXKMText2 = Convert.ToDouble(lines[10]);
                        datX = Convert.ToDouble(lines[11]);
                        chkEGFGOnly.Checked = Convert.ToBoolean(lines[12]);
                        ScaleBox.Text = lines[13];
                        DefaultScale.Text = lines[14];
                        chkMultiple.Checked = Convert.ToBoolean(lines[15]);
                        chkTrim.Checked = Convert.ToBoolean(lines[16]);
                        chkReduce.Checked = Convert.ToBoolean(lines[17]);
                        chkEGFGOnly.Checked = Convert.ToBoolean(lines[18]);
                        chkSub.Checked = Convert.ToBoolean(lines[19]);
                        CHKBXKM.Enabled = true;
                        CHKBXKM.CheckState = CheckState.Checked;
                        CHKBXKM.Text = "Your Style is: " + kmstr;
                        checkBoxDat.Enabled = true;
                        checkBoxDat.CheckState = CheckState.Checked;
                        checkBoxDat.Text = "Datum Text is: " + datumTex;
                        checkBoxEG.Enabled = true;
                        checkBoxEG.CheckState = CheckState.Checked;
                        checkBoxEG.Text = "EG Selected : Color is " + indexG.ToString();
                        checkBoxFG.Enabled = true;
                        checkBoxFG.CheckState = CheckState.Checked;
                        checkBoxFG.Text = "FG Selected : Color is " + indexR.ToString();
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Error reading from file", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    filereader.Close();
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(ex.Message);
            }

        }
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
        private void CreateBTN_Click(object sender, EventArgs e)
        {
            elaptime.Start();
            TimeElapseStripStatus.Text = "Elapsed Time: 00:00:00 ";
            ProgBar.Value = 0;
            percentLbl.Text = "Sec: 0";
            ErrorNOStripStatus.Text = "Errors: 0";
            //Percentlbl.Text = "0%";            
            CreateFiles();
            elaptime.Stop();
            TimeElapseStripStatus.Text = "Elapsed Time: " + elaptime.Elapsed.ToString().Remove(8);//.Hours+":"+elaptime.Elapsed.Minutes+":"+elaptime.Elapsed.Seconds;
            ErrorNOStripStatus.Text = "Errors: " + NOErr.ToString();
            elaptime.Reset();

        }
        private void CheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            if(chkEGFGOnly.Checked==true)
            {
                FGBTN.Enabled = false;
                checkBoxFG.Enabled = false;
            }
            else
            {
                FGBTN.Enabled = true;
            }
        }
        private void button1_Click (object sender, EventArgs e)
        {
            Sections.CreateSectionFromFile win = new Sections.CreateSectionFromFile();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win);
        }
    }
}
