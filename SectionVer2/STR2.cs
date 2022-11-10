using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;

namespace Sections
{
    public partial class STR2 : Form
    {
        Stopwatch elaptime = new Stopwatch();
        public ObjectId corid;
        public Alignment align;
        public Corridor Corridor;
        public ObjectId CorID;
        public List<string> STRlength = new List<string>();
        private StreamWriter filewriter;
        Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
        CivilDocument civildoc = CivilApplication.ActiveDocument;
        Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
        public STR2()
        {
            InitializeComponent();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    ObjectIdCollection alIDs = civildoc.GetAlignmentIds();
                    if (alIDs == null)
                    {
                        MessageBox.Show("You must have at least one alignment", "No Alignment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new System.Exception("There is no alignment");
                    }
                    int coalid = alIDs.Count;
                    List<string> alno = new List<string>();
                    for (int i = 0; i < coalid; i++)
                    {
                        ObjectId alignID = alIDs[i];
                        Alignment align = trans.GetObject(alignID, OpenMode.ForRead) as Alignment;
                        alno.Add(align.Name);
                        LS_Align.Items.Add(align.Name);
                    }
                    LS_Align.SelectedIndex = 0;
                    ObjectIdCollection SurfIDs = civildoc.GetSurfaceIds();
                    for (int i = 0; i < SurfIDs.Count; i++)
                    {
                        TinSurface tin = trans.GetObject(SurfIDs[i], OpenMode.ForRead) as TinSurface;
                        LS_EG.Items.Add(tin.Name);
                        LS_Final.Items.Add(tin.Name);
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                }
                trans.Commit();
            }
            
        }
        public void SlopeLength(int idx, ref double ccl, ref double cfl, ref double cflEG, ref double cclEG)
        {            
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    
                    ObjectIdCollection surfIDs = civildoc.GetSurfaceIds();
                    TinSurface surfEG = trans.GetObject(surfIDs[LS_EG.SelectedIndex], OpenMode.ForWrite) as TinSurface;
                    TinSurface corSurf = trans.GetObject(surfIDs[LS_Final.SelectedIndex], OpenMode.ForWrite) as TinSurface;
                    Alignment align = trans.GetObject(civildoc.GetAlignmentIds()[LS_Align.SelectedIndex], OpenMode.ForWrite) as Alignment;
                    SampleLineGroup slg = null;
                    foreach (ObjectId id in align.GetSampleLineGroupIds())
                    {
                        SampleLineGroup slg2 = trans.GetObject(id, OpenMode.ForWrite) as SampleLineGroup;
                        if (LS_SLG.SelectedItem.ToString() == slg2.Name)
                        {
                            slg = slg2;
                            break;
                        }
                    }
                    SectionView secvg = null;
                    Autodesk.Civil.DatabaseServices.Section seccor = null;
                    Autodesk.Civil.DatabaseServices.Section secEG = null;
                    int progbarID = 0;
                    ProgBar.Maximum = slg.GetSampleLineIds().Count;
                    ProgBar.Step = 1;// / ids.Count();
                    ProgBar.Value = 0;
                    foreach (ObjectId osamID in slg.GetSampleLineIds())
                    {
                        progbarID++;
                        ProgBar.Value = progbarID;// / ids.Count() * 1000;
                        int index22 = 0;
                        int index33 = 0;
                        double a1 = 0;
                        double a2 = 0;
                        double a3 = 0;
                        SampleLine osam = trans.GetObject(osamID, OpenMode.ForWrite) as SampleLine;
                        foreach (ObjectId sectionId in osam.GetSectionViewIds())
                        {
                            secvg = trans.GetObject(sectionId, OpenMode.ForWrite) as SectionView;
                        }
                        a1 = secvg.Location.X - secvg.OffsetRight;
                        a2 = secvg.Location.Y + secvg.ElevationMax - secvg.ElevationMin;
                        a3 = secvg.Location.Z;

                        //-------------------------پیدا کردن ایندکس سطح ها در لیست سطوح سکشن-------------------
                        ObjectId id = ObjectId.Null;
                        ObjectIdCollection origenes = osam.GetSectionIds();
                        //SectionSourceCollection origenes = osam.GetSectionIds();
                        int index5 = -1;
                        int index6 = -1;
                        foreach (ObjectId source in origenes)
                        {
                            Autodesk.Civil.DatabaseServices.Entity ent = (Autodesk.Civil.DatabaseServices.Entity)source.GetObject(OpenMode.ForRead);
                            string name = ent.Name;
                            index5++;
                            if (name.Contains(LS_EG.SelectedItem.ToString()))
                            {
                                break;
                            }

                        }
                        foreach (ObjectId source in origenes)
                        {
                            Autodesk.Civil.DatabaseServices.Entity ent = (Autodesk.Civil.DatabaseServices.Entity)source.GetObject(OpenMode.ForRead);
                            string name = ent.Name;
                            index6++;
                            if (name.Contains(LS_Final.SelectedItem.ToString()))
                            {
                                break;
                            }

                        }
                        //--------------------------نقاط سطح کوریدور--------------------------------------
                        seccor = trans.GetObject(osam.GetSectionIds()[index6], OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Section;
                        //SectionPointCollection secpoCOR = seccor.SectionPoints;
                        Point3dCollection secpoCOR = new Point3dCollection();
                        double index3;
                        double index4;
                        try
                        {
                            index3 = seccor.LeftOffset;
                        }
                        catch
                        {
                            continue;
                        }
                        try
                        {
                            index4 = seccor.RightOffset;
                        }
                        catch
                        {
                            continue;
                        }
                        for (int i = 0; i < seccor.SectionPoints.Count; i++)
                        {
                            if (seccor.SectionPoints[i].Location.X >= seccor.LeftOffset && seccor.SectionPoints[i].Location.X <= seccor.RightOffset)
                            {
                                Point3d popo2d = new Point3d(seccor.SectionPoints[i].Location.X, seccor.SectionPoints[i].Location.Y, 1);
                                secpoCOR.Add(popo2d);
                            }
                        }
                        Point3dCollection CorSectionpoint2 = new Point3dCollection(secpoCOR.Cast<Point3d>().OrderBy(point => point.X).ToArray());
                        //-------------تولید پلی لاین از نقاط سطح کوریدور------------------------------
                        Autodesk.AutoCAD.DatabaseServices.Polyline plineCO = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                        int v = 0;
                        BlockTable blockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord blockTableRec = trans.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                        plineCO.SetDatabaseDefaults();
                        for (int i = 0; i < CorSectionpoint2.Count; i++)
                        {
                            Point2d copo2d = new Point2d(CorSectionpoint2[i].X, CorSectionpoint2[i].Y);
                            plineCO.AddVertexAt(v, copo2d, 0, 0, 0);
                            v++;
                        }
                        plineCO.Closed = false;
                        blockTableRec.AppendEntity(plineCO);
                        trans.AddNewlyCreatedDBObject(plineCO, true);

                        //-------------------------------------نقاط سطح زمین اولیه----------------------


                        secEG = trans.GetObject(osam.GetSectionIds()[index5], OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Section;
                        //SectionPointCollection secPOEG = secEG.SectionPoints;
                        Point3dCollection secPOEG = new Point3dCollection();
                        for (int i = 0; i < secEG.SectionPoints.Count; i++)
                        {
                            if (secEG.SectionPoints[i].Location.X >= seccor.LeftOffset && secEG.SectionPoints[i].Location.X <= seccor.RightOffset)
                            {
                                Point3d popo2d = new Point3d(secEG.SectionPoints[i].Location.X, secEG.SectionPoints[i].Location.Y, 1);
                                secPOEG.Add(popo2d);
                            }
                        }
                        if (Math.Abs(secPOEG[0].X - seccor.LeftOffset) > 0.001)
                        {
                            Point3d popo2d = new Point3d(seccor.LeftOffset, secpoCOR[0].Y, 1);
                            secPOEG.Add(popo2d);
                        }
                        if (Math.Abs(secPOEG[secPOEG.Count - 1].X - seccor.RightOffset) > 0.001)
                        {
                            Point3d popo2d = new Point3d(seccor.RightOffset, secpoCOR[secpoCOR.Count - 1].Y, 1);
                            secPOEG.Add(popo2d);
                        }
                        Point3dCollection EGSectionpoint2 = new Point3dCollection(secPOEG.Cast<Point3d>().OrderBy(point => point.X).ToArray());

                        v = 0;
                        Autodesk.AutoCAD.DatabaseServices.Polyline plineEG = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                        //------------تولید پلی لاین از نقاط سطح زمین-----------------------------------
                        BlockTable blockTableEG = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord blockTableRecEG = trans.GetObject(blockTableEG[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                        plineEG.SetDatabaseDefaults();
                        for (int i = 0; i < EGSectionpoint2.Count; i++)
                        {
                            Point2d EGpo2d = new Point2d(EGSectionpoint2[i].X, EGSectionpoint2[i].Y);
                            plineEG.AddVertexAt(v, EGpo2d, 0, 0, 0);
                            v++;
                        }
                        plineEG.Closed = false;
                        blockTableRecEG.AppendEntity(plineEG);
                        trans.AddNewlyCreatedDBObject(plineEG, true);


                        //--------------------نقطه برخورد دو پلی لاین حاصل از کوریدور و سطح زمین-----


                        Point3dCollection interpoS = new Point3dCollection();
                        plineEG.IntersectWith(plineCO, Intersect.OnBothOperands, interpoS, IntPtr.Zero, IntPtr.Zero);

                        for (int i = 0; i < interpoS.Count; i++)
                        {
                            Point3d popo2d = new Point3d(interpoS[i].X, interpoS[i].Y, 0);
                            EGSectionpoint2.Add(popo2d);
                            CorSectionpoint2.Add(popo2d);
                        }
                        Point3dCollection EGSectionpoint = new Point3dCollection(EGSectionpoint2.Cast<Point3d>().OrderBy(point => point.X).ToArray());
                        Point3dCollection CorSectionpoint = new Point3dCollection(CorSectionpoint2.Cast<Point3d>().OrderBy(point => point.X).ToArray());
                        if (Math.Abs(EGSectionpoint[EGSectionpoint.Count - 1].X - EGSectionpoint[EGSectionpoint.Count - 2].X) < 0.0001)
                        {
                            EGSectionpoint.RemoveAt(EGSectionpoint.Count - 1);
                        }
                        if (Math.Abs(EGSectionpoint[0].X - EGSectionpoint[1].X) < 0.0001)
                        {
                            EGSectionpoint.RemoveAt(0);
                        }
                        if (Math.Abs(CorSectionpoint[CorSectionpoint.Count - 1].X - CorSectionpoint[CorSectionpoint.Count - 2].X) < 0.0001)
                        {
                            CorSectionpoint.RemoveAt(CorSectionpoint.Count - 1);
                        }
                        if (Math.Abs(CorSectionpoint[0].X - CorSectionpoint[1].X) < 0.0001)
                        {
                            CorSectionpoint.RemoveAt(0);
                        }
                        //------------------------------------------------------------------ 
                        double CCL = 0;
                        double zEG = 0;
                        double zCo = 0;
                        double xx = 0;
                        double yy = 0;
                        double x1 = 0;
                        double y1 = 0;
                        double x2 = 0;
                        double y2 = 0;
                        double temp = 0;
                        double CFL = 0;
                        double wr = 0;
                        double wl = -1 * Convert.ToDouble(LeftETWWidth.Text);
                        if (checkBox1.Checked == true)
                        {
                            wr = Convert.ToDouble(LeftETWWidth.Text);
                        }
                        else wr = Convert.ToDouble(RightETWWidth.Text);


                        for (int i = 0; i < EGSectionpoint.Count - 1; i++)
                        {
                            if (CorSectionpoint[0].X <= EGSectionpoint[i].X &&
                                EGSectionpoint[i].X < CorSectionpoint[CorSectionpoint.Count - 1].X)
                            {
                                align.PointLocation(secEG.Station, (EGSectionpoint[i].X + EGSectionpoint[i + 1].X) / 2, ref xx, ref yy);
                                try
                                {
                                    zCo = corSurf.FindElevationAtXY(xx, yy);
                                }
                                catch
                                {
                                    index22 = -1;
                                    break;
                                }
                                try
                                {
                                    zEG = surfEG.FindElevationAtXY(xx, yy);
                                }
                                catch
                                {
                                    index33 = -1;
                                    break;
                                }
                            }
                            else
                            {
                                continue;
                            }
                            if (zCo > zEG)
                            {
                                x1 = EGSectionpoint[i].X;
                                y1 = EGSectionpoint[i].Y;
                                x2 = EGSectionpoint[i + 1].X;
                                y2 = EGSectionpoint[i + 1].Y;
                                temp = Math.Pow(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)), .5);
                                CFL = temp + CFL;
                            }
                        }
                        x1 = 0;
                        y1 = 0;
                        x2 = 0;
                        y2 = 0;
                        temp = 0;
                        //double xx2 = 0;
                        double T1 = 0;
                        for (int i = 0; i < CorSectionpoint.Count - 1; i++)
                        {
                            if (i < CorSectionpoint.Count - 1)
                            {
                                align.PointLocation(secEG.Station, (CorSectionpoint[i].X + CorSectionpoint[i + 1].X) / 2, ref xx, ref yy);
                                try
                                {
                                    zCo = corSurf.FindElevationAtXY(xx, yy);
                                }
                                catch
                                {
                                    index33 = -1;
                                    break;
                                }
                                try
                                {
                                    zEG = surfEG.FindElevationAtXY(xx, yy);
                                }
                                catch
                                {
                                    index33 = -1;
                                    break;
                                }

                            }
                            else
                            {
                                align.PointLocation(secEG.Station, (CorSectionpoint[i].X + CorSectionpoint[i - 1].X) / 2, ref xx, ref yy);
                                zCo = CorSectionpoint[i].Y;
                                try
                                {
                                    zEG = surfEG.FindElevationAtXY(xx, yy);
                                }
                                catch
                                {
                                    index33 = -1;
                                    break;
                                }
                            }
                            if (zCo < zEG)
                            {
                                x1 = CorSectionpoint[i].X;
                                y1 = CorSectionpoint[i].Y;
                                x2 = CorSectionpoint[i + 1].X;
                                y2 = CorSectionpoint[i + 1].Y;
                                temp = Math.Pow(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)), .5);
                                CCL = temp + CCL;
                                if (Math.Round(x2 - x1, 4) <= 0.001)
                                    continue;
                                if (x2 < 0 & x2 - wl < -0.001)
                                    T1 = T1 + temp;
                                else if (x2 < 0 & Math.Abs(x2 - wl) <= .05)
                                    T1 = T1 + temp;
                                else if (x1 >= 0 & x1 - wr > 0.001)
                                    T1 = T1 + temp;
                                else if (x1 >= 0 & Math.Abs(x1 - wr) <= .05)
                                    T1 = T1 + temp;
                                else
                                    continue;
                            }
                        }
                        string lblstr2;
                        string lblstr;
                        Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                        Database acCurDb = acDoc.Database;
                        if (index22 == -1 || index33 == -1)
                        {
                            lblstr2 = "No Intersection Between \n EG and FG Surface";
                            lblstr = "No Intersection Between \n EG and FG Surface";
                            if (idx == 0) secvg.Description = lblstr;
                            STRlength.Add(lblstr2);
                            plineEG.Erase();
                            plineCO.Erase();
                            continue;
                        }

                        double cc = 0;

                        cc = CCL - T1;
                        Point3d mtLoc = new Point3d(a1, a2, a3);
                        double staform = secEG.Station - Math.Floor(secEG.Station / 1000) * 1000;
                        //string lblstr = "STA=" + Convert.ToString(Math.Floor(secEG.Station / 1000)) + "+" + staform.ToString("F3") + "\nCC=" + Convert.ToString(cc.ToString("F3")) + "\nCF=" + Convert.ToString(CFL.ToString("F3")) + "\nT=" + Convert.ToString(CCL.ToString("F3")) + "\nT1=" + Convert.ToString(T1.ToString("F3"));
                        lblstr2 = secEG.Station.ToString("F3") + "," + Convert.ToString(cc.ToString("F3")) + "," + Convert.ToString(CFL.ToString("F3")) + "," + Convert.ToString(T1.ToString("F3"));
                        lblstr = "CC=" + Convert.ToString(cc.ToString("F3")) + "\nCF=" + Convert.ToString(CFL.ToString("F3")) + "\nT=" + Convert.ToString(T1.ToString("F3"));
                        if (idx == 0) secvg.Description = lblstr;
                        STRlength.Add(lblstr2);
                        plineEG.Erase();
                        plineCO.Erase();
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
                trans.Commit();
            }
        }
        private void BTN_OK_Click(object sender, EventArgs e)
        {
            elaptime.Start();
            TimeElapseStripStatus.Text = "Elapsed Time: 00:00:00.0000000 ";
            ProgBar.Value = 0;
            ErrorNOStripStatus.Text = "Errors: 0";
            double ccl = 0;
            double cfl = 0;
            double ccleg = 0;
            double cfleg = 0;
            SlopeLength(0, ref ccl, ref cfl, ref ccleg, ref cfleg);
            elaptime.Stop();
            TimeElapseStripStatus.Text = "Elapsed Time: " + elaptime.Elapsed;//.Hours+":"+elaptime.Elapsed.Minutes+":"+elaptime.Elapsed.Seconds;
            ErrorNOStripStatus.Text = "Errors: ";// + NOErr.ToString();
            elaptime.Reset();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double ccl = 0;
            double cfl = 0;
            double ccleg = 0;
            double cfleg = 0;
            STRlength.Add("STA,CC,CF,T");
            SlopeLength(1, ref ccl, ref cfl, ref ccleg, ref cfleg);
            string filename;
            DialogResult result = new DialogResult();
            using (SaveFileDialog filechooser = new SaveFileDialog())
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
                    FileStream input = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
                    filewriter = new StreamWriter(input);
                    for (int i = 0; i < STRlength.Count; i++)
                    {
                        filewriter.WriteLine(STRlength[i]);
                    }
                    filewriter.Close();
                }

                catch (IOException)
                {
                    MessageBox.Show("Error writing to file", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sections.About_Stripping_Length win1 = new About_Stripping_Length();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win1);
        }
        private void LS_Align_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    ObjectIdCollection alIDs = civildoc.GetAlignmentIds();
                    if (alIDs == null)
                    {
                        MessageBox.Show("You must have at least one alignment", "No Alignment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new System.Exception("There is no alignment");
                    }
                    int coalid = alIDs.Count;
                    List<string> alno = new List<string>();
                    for (int i = 0; i < coalid; i++)
                    {
                        ObjectId alignID = alIDs[i];
                        Alignment align = trans.GetObject(alignID, OpenMode.ForRead) as Alignment;
                        alno.Add(align.Name);
                        LS_Align.Items.Add(align.Name);
                    }
                    Alignment align2 = trans.GetObject(alIDs[LS_Align.SelectedIndex], OpenMode.ForRead) as Alignment;
                    ObjectIdCollection smalpeIDs = align2.GetSampleLineGroupIds();
                    for (int i = 0; i < smalpeIDs.Count; i++)
                    {
                        SampleLineGroup slg = trans.GetObject(smalpeIDs[i], OpenMode.ForRead) as SampleLineGroup;
                        LS_SLG.Items.Add(slg.Name);
                    }
                    
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                }
                trans.Commit();
            }

        }
        private void LeftETWWidth_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                RightETWWidth.Enabled = true;
                RightETWWidth.Text = LeftETWWidth.Text;
            }
            else RightETWWidth.Text = LeftETWWidth.Text;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                RightETWWidth.Enabled = true;
            }
            else RightETWWidth.Enabled = false;
        }
        private void BTN_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
