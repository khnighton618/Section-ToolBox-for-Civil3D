using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;

namespace Sections
{
    public partial class AutocadSection2Civil : Form
    {
        Sections.CSDPSectionExport csdp = new CSDPSectionExport();       
        Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;        
        Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
        Database db = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Database;
        Autodesk.AutoCAD.DatabaseServices.Polyline PolyLine = null;
        private StreamWriter filewriter;
        private StreamReader filereader;
        string KMStlye;
        double Y_KMText;
        double H_KMText;
        double X_KMText;
        string kmstr;
        int KMcolor;
        double KMrot;
        double X_Axe;
        double LeftOffTxt_X;
        double LeftOffTxt_Y;
        string datumTex;
        string offsetTex;
        double MtextDatum_X;
        double MtextDatum_Y;
        int indexG = 0;
        int indexR = -10;
        int indexPoly = 0;
        double OffLength;
        double elevLength;
        double KMlength;        
        int NOErr = 0;
        int indexAXE = 0;
        int indexst = 0;
        int indexOff = 0;        
        int indexpl = 0;
        int indexDat = 0;
        string STRError;
        string sLayerName1 = "PolyEG";
        string sLayerName2 = "PolyFG";
        Stopwatch elaptime = new Stopwatch();
        List<string> sbG = new List<string>();
        List<string> sbR = new List<string>();
        List<string> exEG = new List<string>();
        List<string> exFG = new List<string>();
        List<double> liny = new List<double>();
        SelectionSet selSet;
        TypedValue[] tv = new TypedValue[] { new TypedValue((int)DxfCode.Start, "*POLY*") };
        TypedValue[] tvline = new TypedValue[] { new TypedValue((int)DxfCode.Start, "LINE") };
        



        public AutocadSection2Civil()
        {
            InitializeComponent();
            
        }

        private void SelKMBTN_Click(object sender, EventArgs e)
        {
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    PromptSelectionResult se = ed.GetSelection();
                    if (se.Status == PromptStatus.Error) return;
                    SelectionSet Set1 = se.Value;
                    ObjectId[] ids1 = Set1.GetObjectIds();
                    string kmtxt = "";
                    

                    foreach (ObjectId entId1 in ids1)
                    {
                        MText mtex1 = tr.GetObject(entId1, OpenMode.ForRead) as MText;
                        if(mtex1!=null)
                        {
                            if (mtex1.Contents.Contains("{"))
                            {
                                kmtxt = mtex1.Contents.Split(';')[1].Split('}')[0];
                            }
                            else
                                kmtxt = mtex1.Contents;
                            
                            if (mtex1 != null)
                            {
                                KMStlye = kmtxt;
                                KMcolor = mtex1.ColorIndex;
                                KMrot = mtex1.Rotation;
                                X_KMText = mtex1.Location.X;
                                Y_KMText = mtex1.Location.Y;

                                H_KMText = mtex1.Height;
                                kmstr = kmstr + kmtxt;
                            }
                        }
                        else
                        {
                            
                            DBText tex1 = tr.GetObject(entId1, OpenMode.ForRead) as DBText;
                            if (tex1.TextString.Contains("{"))
                            {
                                kmtxt = tex1.TextString.Split(';')[1].Split('}')[0];
                            }
                            else
                                kmtxt = tex1.TextString;
                            
                            if (tex1 != null)
                            {
                                KMStlye = kmtxt;
                                KMcolor = tex1.ColorIndex;
                                KMrot = tex1.Rotation;
                                X_KMText = tex1.Position.X;
                                Y_KMText = tex1.Position.Y;

                                H_KMText = tex1.Height;
                                kmstr = kmstr + kmtxt;
                            }
                        }
                                               
                    }
                    CHKBXKM.Enabled = true;
                    CHKBXKM.CheckState = CheckState.Checked;
                    CHKBXKM.Text = "STA Text: " + kmstr;
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
                        MText mtex1 = tr.GetObject(entId1, OpenMode.ForRead) as MText;
                        if (mtex1 != null)
                        {
                            MtextDatum_X = Convert.ToDouble(mtex1.Location.X);
                            MtextDatum_Y = Convert.ToDouble(mtex1.Location.Y);

                            datumTex = mtex1.Contents;
                            checkBoxDat.Enabled = true;
                            checkBoxDat.CheckState = CheckState.Checked;
                            checkBoxDat.Text = "Datum Elevation is: " + mtex1.Contents;
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

        private void OffsetBTN_Click(object sender, EventArgs e)
        {
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    PromptSelectionResult se = ed.GetSelection();
                    if (se.Status == PromptStatus.Error) return;
                    SelectionSet Set1 = se.Value;
                    ObjectId[] ids1 = Set1.GetObjectIds();
                    string tex1 = "";
                    foreach (ObjectId entId1 in ids1)
                    {
                        MText mtex1 = tr.GetObject(entId1, OpenMode.ForRead) as MText;
                        if (mtex1.Contents.Contains("{"))                        
                            tex1 = mtex1.Contents.Split(';')[1].Split('}')[0];                        
                        else
                            tex1 = mtex1.Contents;
                        if (mtex1 != null)
                        {
                            LeftOffTxt_X = Convert.ToDouble(mtex1.Location.X);
                            LeftOffTxt_Y = Convert.ToDouble(mtex1.Location.Y);
                            offsetTex = tex1;
                            checkBoxOff.Enabled = true;
                            checkBoxOff.CheckState = CheckState.Checked;
                            checkBoxOff.Text = "Left Offset is: " + tex1;
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

        private void PolyBTN_Click(object sender, EventArgs e)
        {
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
                        Autodesk.AutoCAD.DatabaseServices.Polyline poly1 = tr.GetObject(entId1, OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.Polyline;
                        if (poly1 != null)
                        {
                            OffLength = Math.Abs(poly1.Bounds.Value.MinPoint.Y - LeftOffTxt_Y);
                            elevLength = Math.Abs(poly1.Bounds.Value.MinPoint.X - MtextDatum_X);
                            KMlength = Math.Abs(poly1.Bounds.Value.MaxPoint.Y - Y_KMText);
                            indexPoly = poly1.ColorIndex;
                            //AveXKMText1 = Math.Abs(X_KMText1 - (poly1.Bounds.Value.MaxPoint.X + poly1.Bounds.Value.MinPoint.X) / 2);
                            //if (X_KMText2 != -1e10)
                            //    AveXKMText2 = Math.Abs(X_KMText2 - (poly1.Bounds.Value.MaxPoint.X + poly1.Bounds.Value.MinPoint.X) / 2);
                            ////AveYKMText1= Math.Abs(H_KMText - poly1.Bounds.Value.MinPoint.Y);
                            //if (datX == 5)
                            //    datX = Math.Abs(MtextDatum - poly1.Bounds.Value.MinPoint.X);
                            checkBoxObject.Enabled = true;
                            checkBoxObject.CheckState = CheckState.Checked;
                            checkBoxFile.Enabled = false;
                            checkBoxFile.CheckState = CheckState.Unchecked;
                            PolyLine = poly1;
                        }
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
                        Autodesk.AutoCAD.DatabaseServices.Polyline poly1 = tr.GetObject(entId1, OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.Polyline;
                        if (poly1 != null)
                        {
                            indexR = poly1.ColorIndex;
                            checkBoxFG.Enabled = true;
                            checkBoxFG.CheckState = CheckState.Checked;
                            checkBoxFG.Text = "FG Selected : Color is " + indexR.ToString();
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

        private void CreateBTN_Click(object sender, EventArgs e)
        {
            elaptime.Start();
            TimeElapseStripStatus.Text = "Elapsed Time: 00:00:00 ";
            ProgBar.Value = 0;
            ErrorNOStripStatus.Text = "Errors: 0";
            //Percentlbl.Text = "0%";
            //csdp.CreateFiles();
            
            CreateFiles();
            elaptime.Stop();
            TimeElapseStripStatus.Text = "Elapsed Time: " + elaptime.Elapsed.ToString().Remove(8);//.Hours+":"+elaptime.Elapsed.Minutes+":"+elaptime.Elapsed.Seconds;
            ErrorNOStripStatus.Text = "Errors: " + NOErr.ToString();
            elaptime.Reset();
        }

        public void ErrorDebugger(int indexDat, int indexst, int indexOff, int indexAXE, int indexpl, double sta)
        {
            int v = 0;
            if (indexst == 0 || indexDat == 0 || indexOff == 0 || indexAXE == 0 || indexpl == 0)
            {
                v++;
                if (indexst == 0)
                    STRError = STRError + "\n You must edit Station Text or Select another One at Station: " + sta.ToString();
                //MessageBox.Show("You must edit Station Text or Select another One!", "Station Text Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (indexDat == 0)
                    STRError = STRError + "\n You must edit Datum Text or Select another One at Station: " + sta.ToString();
                //MessageBox.Show("You must edit Datum Text or Select another One!", "Datum Text Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (indexOff == 0)
                    STRError = STRError + "\n You must edit bottom left offset On at Station: " + sta.ToString();
                //MessageBox.Show("You must edit Scale Text at the bottom Left or Select another One!", "Scale Text Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (indexAXE == 0)
                    STRError = STRError + "\n Search zero offset section view at Station: " + sta.ToString();
                //MessageBox.Show("You must edit Datum Block Text or Select another One!", "Datum Block Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (indexpl == 0)
                    STRError = STRError + "\n You must edit PolyLine or Select another One at Station: " + sta.ToString();
                //MessageBox.Show("You must edit PolyLine or Select another One!", "PolyLine Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
                return;
            NOErr = NOErr + v;
           
        }

        public void CreateFiles()
        {
            TypedValue[] fltGreenPol = new TypedValue[] { new TypedValue(0, "*POLY*"), new TypedValue((int)DxfCode.Color, indexG) };
            TypedValue[] fltRedPol = null ;
            if (indexR != -10)
            {
                fltRedPol = new TypedValue[] { new TypedValue(0, "*POLY*"), new TypedValue((int)DxfCode.Color, indexR) };
            }
            sbG.Clear();
            sbR.Clear();
            exEG.Clear();
            exFG.Clear();
            sbG.Add("STA,OFFSET,Z");
            sbR.Add("STA,OFFSET,Z");
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    //---------ساخت لایه های پلی لاینها
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
                    List<double> stalist = new List<double>();                    
                    double maxpolyX = 0;
                    double maxpolyY = 0;
                    double minpolyX = 0;
                    double minpolyY = 0;                    
                    double sta = 0;
                    int progbarID = 0;                    
                    System.Data.DataTable tbG = new System.Data.DataTable();
                    tbG.Columns.Add("STA", typeof(double));
                    tbG.Columns.Add("OFF", typeof(double));
                    tbG.Columns.Add("H", typeof(double));
                    System.Data.DataTable tbR = new System.Data.DataTable();
                    tbR.Columns.Add("STA", typeof(double));
                    tbR.Columns.Add("OFF", typeof(double));
                    tbR.Columns.Add("H", typeof(double));
                    if(selSet==null)
                    {
                        ed.WriteMessage("Select All Section Views");
                        PromptSelectionResult selRes = ed.GetSelection(new SelectionFilter(tv));
                        if (selRes.Status == PromptStatus.Error) return;
                        selSet = selRes.Value;
                    }
                    ObjectId[] ids = selSet.GetObjectIds();                    
                    ProgBar.Maximum = ids.Count();
                    ProgBar.Step = 1;
                    ProgBar.Value = 0;
                    foreach (ObjectId entId in ids)
                    {
                        progbarID++;
                        ProgBar.Value = progbarID;
                        Autodesk.AutoCAD.DatabaseServices.Polyline poly = tr.GetObject(entId, OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.Polyline;
                        if (poly != null && poly.ColorIndex.Equals(indexPoly))
                        {
                            maxpolyX = poly.Bounds.Value.MaxPoint.X;
                            maxpolyY = poly.Bounds.Value.MaxPoint.Y;
                            minpolyX = poly.Bounds.Value.MinPoint.X;
                            minpolyY = poly.Bounds.Value.MinPoint.Y;
                            
                            
                            Point2dCollection po2dcol1 = new Point2dCollection();
                            //--- زمین طبیعی----
                            DataView dv1G = null;
                            exportTable(maxpolyX, maxpolyY, minpolyX, minpolyY, fltGreenPol, ref dv1G, ref sta, ref po2dcol1
                                , ref indexDat, ref indexst, ref indexOff , ref  indexAXE, ref  indexpl);
                            ErrorDebugger(indexDat, indexst, indexOff,  indexAXE,  indexpl,  sta);
                            createPolyLine(fltGreenPol, po2dcol1);
                            po2dcol1.Clear();
                            stalist.Add(sta);
                            //---سطح نهایی----
                            DataView dv1R = null;
                            if (indexR != -10)
                            {
                                exportTable(maxpolyX, maxpolyY, minpolyX, minpolyY, fltRedPol, ref dv1R, ref sta, ref po2dcol1
                                , ref indexDat, ref indexst, ref indexOff, ref indexAXE, ref indexpl);
                                ErrorDebugger(indexDat, indexst, indexOff, indexAXE, indexpl, sta);
                                createPolyLine(fltRedPol, po2dcol1);
                                po2dcol1.Clear();
                            }
                            //---------------------
                            if (stalist.IndexOf(sta) == stalist.LastIndexOf(sta))
                            {
                                if (indexR != -10)
                                {
                                    for (int k = 0; k < dv1R.Count; k++)
                                    {
                                        tbR.Rows.Add(sta, dv1R[k][0], dv1R[k][1]);
                                    }
                                }
                                for (int j = 0; j < dv1G.Count; j++)
                                {
                                    tbG.Rows.Add(sta, dv1G[j][0], dv1G[j][1]);
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
        
        public void exportTable(double maxpolyX, double maxpolyY, double minpolyX, double minpolyY, TypedValue[] fltPol,
            ref DataView dv, ref double sta, ref Point2dCollection po2dcol, ref int indexDat, ref int indexst, ref int indexOff
            , ref int indexAXE, ref int indexpl)
        {
            indexDat = 0;
            indexst = 0;
            indexOff = 0;
            indexAXE = 0;
            indexpl = 0;
            indexDat = 0;
            TypedValue[] tvTEX = new TypedValue[] { new TypedValue((int)DxfCode.Start, "MTEXT") };            
            po2dcol = new Point2dCollection();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    System.Data.DataTable table = new System.Data.DataTable();
                    table.Columns.Add("OFF", typeof(double));
                    table.Columns.Add("H", typeof(double));
                    table.Columns.Add("Xdwg", typeof(double));
                    table.Columns.Add("Ydwg", typeof(double));
                    double aveX = (minpolyX + maxpolyX) / 2;
                    double aveY = (minpolyY + maxpolyY) / 2;                    
                    string km1 = "";
                    string km2 = "A";                    
                    string b = string.Empty;                    
                    double Datum = 0;
                    sta = -1e10;                    
                    int ID = 0;
                    double x = 0;
                    double y = 0;
                    string Xaxe = "";
                    string kmtxt = "";
                    //----پیدا کردن تکست کیلومتراژ----------------------------------------
                    PromptSelectionResult polsel;
                    polsel = ed.SelectCrossingWindow(new Point3d(minpolyX, maxpolyY, 0), new Point3d(maxpolyX, maxpolyY+KMlength+1, 0), new SelectionFilter(tvTEX));
                    if (polsel.Status == PromptStatus.Error) return;
                    SelectionSet kmTexset = polsel.Value;
                    ObjectId[] ids2 = kmTexset.GetObjectIds();
                    foreach (ObjectId entId2 in ids2)
                    {
                        MText mtex1 = tr.GetObject(entId2, OpenMode.ForRead) as MText;
                        if (mtex1 == null) continue;
                        if (mtex1.Contents.Contains("{"))
                        {
                            kmtxt = mtex1.Contents.Split(';')[1].Split('}')[0];
                        }
                        else
                            kmtxt = mtex1.Contents;

                        if (kmtxt.StartsWith("A") || kmtxt.StartsWith("B") || kmtxt.StartsWith("C")
                            || kmtxt.StartsWith("D") || kmtxt.StartsWith("E") || kmtxt.StartsWith("F")
                            || kmtxt.StartsWith("G") || kmtxt.StartsWith("H") || kmtxt.StartsWith("I")
                            || kmtxt.StartsWith("T") || kmtxt.StartsWith("S")
                            || kmtxt.StartsWith("P") || kmtxt.StartsWith("X")
                            || kmtxt.StartsWith("Y") || kmtxt.StartsWith("*")
                            || Math.Abs(mtex1.Location.Y - KMlength - maxpolyY) >= 1)
                            continue;
                        
                        if (( kmtxt.StartsWith("Km") || kmtxt.StartsWith("KM") || 
                            kmtxt.StartsWith("kM") || kmtxt.StartsWith("km")))
                        {
                            sta = Convert.ToDouble(kmtxt);
                            Point2d po = new Point2d(mtex1.Location.X, mtex1.Location.Y);
                            po2dcol.Add(po);
                            indexst = 1;
                        }
                        if ( kmtxt.Contains("+") || kmtxt.Contains("+ ") || kmtxt.Contains(" +") ||
                            kmtxt.Contains(" + "))
                        {
                            km2 = kmtxt.Split('+')[0];
                            km1 = kmtxt.Split('+')[1];
                            sta = Convert.ToDouble(km2+km1);
                            Point2d po = new Point2d(mtex1.Location.X, mtex1.Location.Y);
                            po2dcol.Add(po);
                            indexst = 1;
                        }                        
                    }
                    
                    //-------------- پیدا کردن ارتفاع دیتوم     -----------------------------------              
                    polsel = ed.SelectCrossingWindow(new Point3d(minpolyX - elevLength - 1, minpolyY- elevLength - 1, 0), new Point3d(minpolyX + 1, minpolyY + 1, 0), new SelectionFilter(tvTEX));
                    if (polsel.Status == PromptStatus.Error) return;
                    SelectionSet DatTexset = polsel.Value;
                    ObjectId[] ids3 = DatTexset.GetObjectIds();
                    foreach (ObjectId entId2 in ids3)
                    {
                        MText mtex1 = tr.GetObject(entId2, OpenMode.ForRead) as MText;
                        
                        if (mtex1 != null && Math.Abs(mtex1.Location.X - minpolyX + elevLength) <= 0.01)
                        {
                            if (mtex1.Contents.Contains("{"))
                            {
                                Datum = Convert.ToDouble(mtex1.Contents.Split(';')[1].Split('}')[0]);
                            }
                            Datum = Convert.ToDouble(mtex1.Contents);
                            indexDat = 1;
                            indexOff = 1;
                            Point2d po = new Point2d(mtex1.Location.X, mtex1.Location.Y);
                            po2dcol.Add(po);
                            continue;
                        }
                    }

                    //----------------پیدا کردن مختصات آکس مسیر  ----------------------------------                  
                    polsel = ed.SelectCrossingWindow(new Point3d(minpolyX-2*OffLength-1, minpolyY-2*OffLength-1, 0), new Point3d(maxpolyX+1, minpolyY, 0), new SelectionFilter(tvTEX));
                    if (polsel.Status == PromptStatus.Error) return;
                    SelectionSet AXEset = polsel.Value;
                    ObjectId[] ids4 = AXEset.GetObjectIds();
                    
                    foreach (ObjectId entId2 in ids4)
                    {
                        MText mtex1 = tr.GetObject(entId2, OpenMode.ForRead) as MText;
                        if (mtex1.Contents.Contains("{"))
                        {
                            Xaxe = mtex1.Contents.Split(';')[1].Split('}')[0];
                        }
                        else
                            Xaxe = mtex1.Contents;
                        if (mtex1 != null && Xaxe == "0")
                        {
                            X_Axe = mtex1.Location.X;
                            indexAXE = 1;
                            Point2d po = new Point2d(mtex1.Location.X, mtex1.Location.Y);
                            po2dcol.Add(po);
                            continue;
                        }
                    }

                    //-----------تشکیل مختصات پلی لاین سطح زمین طبیعی------------------------------
                    polsel = ed.SelectCrossingWindow(new Point3d(minpolyX, minpolyY, 0), new Point3d(maxpolyX, maxpolyY, 0),
                        new SelectionFilter(fltPol));
                    if (polsel.Status == PromptStatus.Error) return;
                    SelectionSet PolGreen = polsel.Value;
                    ObjectId[] ids5 = PolGreen.GetObjectIds();
                    int indexpoly = 0;
                    foreach (ObjectId entId4 in ids5)
                    {
                        Autodesk.AutoCAD.DatabaseServices.Polyline poly2 = tr.GetObject(entId4, OpenMode.ForRead) as Autodesk.AutoCAD.DatabaseServices.Polyline;
                        
                        if (poly2 != null & indexpoly==0)
                        {
                            Point2d po2d = new Point2d();
                            for (int i = ID; i < poly2.NumberOfVertices; i++)
                            {
                                po2d = poly2.GetPoint2dAt(i);
                                x = Math.Round((po2d.X - X_Axe), 4);
                                y = Math.Round(Datum +  po2d.Y-minpolyY, 4);                                
                                table.Rows.Add(x, y, po2d.X, po2d.Y);
                                indexpl = 1;
                            }
                            ID = 1;
                            indexpoly++;
                        }
                    }
                    dv = new DataView(table);
                    dv.Sort = "OFF ASC";

                    //------حذف نقاط با فاصله کم از هم-----------------------------------------------
                    //List<string> dvlist = new List<string>();
                    double difx = 0;
                    double dify = 0;
                    for (int i = 0; i < dv.Count; i++)
                    {
                        Point2d po = new Point2d((double)dv[i][2], (double)dv[i][3]);
                        po2dcol.Add(po);
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
                        if (Math.Abs(difx) <= .01)
                        {
                            dv[i].Row.Delete();
                            continue;
                        }                                              
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
            }
        }

        public void SaveFiles(List<string> inputTXTEG, List<string> inputTXTFG, int otherfiles)
        {
            string filename;
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
                    if (otherfiles == 0)
                    {
                        string f1;
                        string f2;
                        f1 = filename.Trim(".txt".ToCharArray()) + "-EG.txt";
                        FileStream inputEG = new FileStream(f1, FileMode.OpenOrCreate, FileAccess.Write);
                        filewriter = new StreamWriter(inputEG);
                        for (int i = 0; i < inputTXTEG.Count; i++)
                        {
                            filewriter.WriteLine(inputTXTEG[i]);
                        }
                        filewriter.Close();
                        if (inputTXTFG != null && indexR != -10)
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
                        string f1;
                        f1 = filename;
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

        private void saveGenericToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFiles(exEG, exFG, 0);
        }

        private void saveSTAForatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFiles(sbG, sbR, 0);
        }

        private void openSelectionToolStripMenuItem_Click(object sender, EventArgs e)
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
                        List<string> key = new List<string>();
                        KMStlye = Convert.ToString(lines[0]);
                        Y_KMText = Convert.ToDouble(lines[1]);
                        H_KMText = Convert.ToDouble(lines[2]);
                        X_KMText = Convert.ToDouble(lines[3]);
                        kmstr = Convert.ToString(lines[4]);
                        KMcolor = Convert.ToInt32(lines[5]);
                        KMrot = Convert.ToDouble(lines[6]);
                        X_Axe = Convert.ToDouble(lines[7]);
                        LeftOffTxt_X = Convert.ToDouble(lines[8]);
                        LeftOffTxt_Y = Convert.ToDouble(lines[9]);
                        datumTex = Convert.ToString(lines[10]);
                        offsetTex= Convert.ToString(lines[11]);
                        MtextDatum_X = Convert.ToDouble(lines[12]);
                        MtextDatum_Y = Convert.ToDouble(lines[13]);
                        indexG = Convert.ToInt32(lines[14]);
                        indexR = Convert.ToInt32(lines[15]);
                        indexPoly = Convert.ToInt32(lines[16]);
                        OffLength = Convert.ToDouble(lines[17]);
                        elevLength = Convert.ToDouble(lines[18]);
                        KMlength = Convert.ToDouble(lines[19]);
                        NOErr = Convert.ToInt32(lines[20]);
                        indexAXE = Convert.ToInt32(lines[21]);
                        indexst = Convert.ToInt32(lines[22]);
                        indexOff = Convert.ToInt32(lines[23]);
                        indexpl = Convert.ToInt32(lines[24]);
                        indexDat = Convert.ToInt32(lines[25]);
                        using (Transaction tr = db.TransactionManager.StartTransaction())
                        {
                            int v = 0;
                            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                            Autodesk.AutoCAD.DatabaseServices.Polyline pol2d = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                            pol2d.SetDatabaseDefaults();
                            for (int i = 26; i < 34; i=i+2)
                            {
                                Point2d po = new Point2d(Convert.ToDouble(lines[i]), Convert.ToDouble(lines[i + 1]));
                                pol2d.AddVertexAt(v, po, 0, 0, 0);
                                v++;
                            }
                            pol2d.Closed = false;
                            PolyLine = pol2d;
                            tr.Commit();
                        }
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
                        checkBoxOff.Enabled = true;
                        checkBoxOff.CheckState = CheckState.Checked;
                        checkBoxOff.Text = "Left Offset is: " + offsetTex;
                        checkBoxObject.Enabled = true;
                        checkBoxObject.CheckState = CheckState.Checked;
                        //checkBoxObject.Enabled = true;
                        //checkBoxObject.CheckState = CheckState.Checked;
                        //checkBoxFile.Enabled = false; 
                        //checkBoxFile.CheckState = CheckState.Unchecked;
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

        private void saveSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> st = new List<string>();
            st.Add(KMStlye.ToString());
            st.Add(Y_KMText.ToString());
            st.Add(H_KMText.ToString());
            st.Add(X_KMText.ToString());
            st.Add(kmstr.ToString());
            st.Add(KMcolor.ToString());
            st.Add(KMrot.ToString());
            st.Add(X_Axe.ToString());
            st.Add(LeftOffTxt_X.ToString());
            st.Add(LeftOffTxt_Y.ToString());
            st.Add(datumTex);
            st.Add(offsetTex);
            st.Add(MtextDatum_X.ToString());
            st.Add(MtextDatum_Y.ToString());
            st.Add(indexG.ToString());
            st.Add(indexR.ToString());
            st.Add(indexPoly.ToString());
            st.Add(OffLength.ToString());
            st.Add(elevLength.ToString());
            st.Add(KMlength.ToString());
            st.Add(NOErr.ToString());
            st.Add(indexAXE.ToString());
            st.Add(indexst.ToString());
            st.Add(indexOff.ToString());
            st.Add(indexpl.ToString());
            st.Add(indexDat.ToString());
            //st.Add(PolyLine.ObjectId.ToString());
            for(int i=0; i< PolyLine.NumberOfVertices; i++)
            {
                st.Add(PolyLine.GetPoint2dAt(i).X.ToString());
                st.Add(PolyLine.GetPoint2dAt(i).Y.ToString());
            }
            SaveFiles(st, null, 1);

            //using (var tr = db.TransactionManager.StartTransaction())
            //{
            //    // open the model space block table record
            //    var ms = (BlockTableRecord)tr.GetObject(
            //        SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForRead);

            //    // iterate through the model space to count all objectIds grouped by their ObjectARX (C++) name
            //    foreach (var item in ms
            //        .Cast<ObjectId>()
            //        .ToLookup(id => id.ObjectClass.Name))
            //    {
            //        ObjectId oid = item.ToList()[0];
                    
            //        Polyline pol = tr.GetObject(oid, OpenMode.ForRead) as Polyline;
            //        if(pol!=null&& pol.GetPoint2dAt(0).X==)
            //        {
            //            PolyLine = pol;
            //        }
            //    }
            //    tr.Commit();
            //}



            //DialogResult result = new DialogResult();
            //using (SaveFileDialog filechooser = new SaveFileDialog())
            //{
            //    filechooser.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            //    filechooser.FilterIndex = 1;
            //    result = filechooser.ShowDialog();
            //    filechooser.DefaultExt = "EG.txt";
            //    filename = filechooser.FileName;
            //}
            //if (result == DialogResult.OK)
            //{
            //    try
            //    {
            //        Stream SaveFileStream = File.Create(filename);
            //        //BinaryFormatter serializer2 = new BinaryFormatter();
            //        //serializer2.Serialize(SaveFileStream, PolyLine.GetType());
            //        SaveFileStream.Close();
            //        //System.Xml.XmlDocument xmlDocument = new XmlDocument();
            //        //XmlSerializer serializer = new XmlSerializer(PolyLine.GetType());
            //        //using (MemoryStream stream = new MemoryStream())
            //        //{
            //        //    serializer.Serialize(stream, PolyLine);
            //        //    stream.Position = 0;
            //        //    //xmlDocument.Load(stream);
            //        //    //xmlDocument.Save(filename);
            //        //    //stream.Close();
            //        //    //System.Xml.Linq.XElement booksFromFile = System.Xml.Linq.XElement.Load(filename);
            //        //}
            //    }
            //    catch (IOException)
            //    {
            //        MessageBox.Show("Error writing to file", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Sections.AutocadSection2Civil win = new AutocadSection2Civil();
            //Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win);
        }
    }
}
