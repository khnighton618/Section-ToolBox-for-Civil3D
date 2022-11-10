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
using Autodesk.AutoCAD.GraphicsInterface;
using Polyline = Autodesk.AutoCAD.DatabaseServices.Polyline;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;

namespace Sections
{
    
    public partial class STR : Form
    {
        public int NOErr;
        public ObjectId corid;
        public Alignment align;
        public Corridor Corridor;
        public ObjectId CorID;
        public List<string> STRlength =new List<string>();
        public DataView dv;
        private StreamWriter filewriter;
        Stopwatch elaptime = new Stopwatch();
        Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
        CivilDocument civildoc = CivilApplication.ActiveDocument;
        Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
        public double ii;
        public void CreatePolyLine(int index, Point2dCollection po2dcol1, Point3dCollection interpo,ref Polyline poly,ref Polyline poly2, double cx)
        {
            if(chkDraw.CheckState==CheckState.Checked & po2dcol1.Count != 0)
            {
                int v1 = 0;
                int v2 = 0;
                //if (index == 150)
                //{
                //    for (int i = 0; i < interpo.Count; i++)
                //    {
                //        Point2d po = new Point2d(interpo[i].X, interpo[i].Y);
                //        po2dcol1.Add(po);
                //    }
                //}

                Point2dCollection po2dcol = new Point2dCollection(po2dcol1.Cast<Point2d>().OrderBy(point => point.X).ToArray());                
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    try
                    {                                                                     
                        BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                        if(index==150)
                        {
                            PolylineCollection pol = new PolylineCollection();
                            
                            poly = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                            poly2 = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                            for (int i = 0; i < po2dcol.Count; i++)
                            {
                                if(po2dcol[i].X<cx)
                                {
                                    
                                    poly.SetDatabaseDefaults();
                                    poly.AddVertexAt(v1, po2dcol[i], 0, 0, 0);
                                    v1++;
                                }
                                else
                                {

                                    poly2.SetDatabaseDefaults();
                                    poly2.AddVertexAt(v2, po2dcol[i], 0, 0, 0);
                                    v2++;
                                }                     
                            }
                            poly.Closed = false;
                            poly.ColorIndex = index;
                            btr.AppendEntity(poly);
                            tr.AddNewlyCreatedDBObject(poly, true);
                            poly2.Closed = false;
                            poly2.ColorIndex = index;
                            btr.AppendEntity(poly2);
                            tr.AddNewlyCreatedDBObject(poly2, true);                            
                        }
                        else
                        {
                            Point3dCollection pointersect = new Point3dCollection();
                            DistinctFunction(po2dcol, ref pointersect);
                            poly = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                            for (int i = 0; i < po2dcol.Count; i++)
                            {
                                poly.SetDatabaseDefaults();
                                poly.AddVertexAt(i, po2dcol[i], 0, 0, 0);
                            }
                            if(pointersect!=null)
                            {
                                if (pointersect.Count != 0)
                                {
                                    for (int i = 0; i < pointersect.Count; i = i + 2)
                                    {

                                        Polyline poly1 = (Polyline)poly.GetSplitCurves(pointersect)[i];
                                        poly1.Closed = false;
                                        poly1.ColorIndex = index;
                                        btr.AppendEntity(poly1);
                                        tr.AddNewlyCreatedDBObject(poly1, true);
                                        poly2 = poly1;
                                    }
                                }
                                else
                                {
                                    poly.Closed = false;
                                    poly.ColorIndex = index;
                                    btr.AppendEntity(poly);
                                    tr.AddNewlyCreatedDBObject(poly, true);
                                }
                            }
                            
                        }                        
                        tr.Commit();
                    }
                    catch (Autodesk.AutoCAD.Runtime.Exception ex)
                    {
                        Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                    }
                }                
            }
            else
                poly = null;

        }
        
        public void DistinctFunction(Point2dCollection po, ref Point3dCollection po3)
        {
            if ( po.Count <2 )
            {
                po3 = null;
                return;
            }
            try
            {
                System.Data.DataTable tb = new System.Data.DataTable();
                tb.Columns.Add("x", typeof(double));
                tb.Columns.Add("y", typeof(double));
                tb.Columns.Add("ind", typeof(string));
                foreach (Point2d p in po)
                {
                    tb.Rows.Add(p.X, p.Y, "S");
                }
                DataView dv = new DataView(tb);
                if ((double)dv[0][0] == (double)dv[1][0] & (double)dv[0][1] == (double)dv[1][1])
                {
                    dv[0].Row.BeginEdit();
                    dv[0].Row.Delete();
                }
                if ((double)dv[dv.Count - 1][0] == (double)dv[dv.Count - 2][0] & (double)dv[dv.Count - 1][1] == (double)dv[dv.Count - 2][1])
                    dv[dv.Count - 1].Row.Delete();
                for (int i = 1; i < dv.Count - 1; i = i + 2)
                {
                    if ((double)dv[i][0] == (double)dv[i + 1][0] & (double)dv[i][1] == (double)dv[i + 1][1])
                    {
                        dv[i].Row.BeginEdit();
                        dv[i][2] = "S";
                    }
                    else
                    {
                        dv[i].Row.BeginEdit();
                        dv[i][2] = "E";
                        dv[i + 1][2] = "E";
                    }
                }
                po3 = new Point3dCollection();
                for (int i = 1; i < dv.Count; i++)
                {
                    if ((string)dv[i][2] == "E")
                        po3.Add(new Point3d((double)dv[i][0], (double)dv[i][1], 0));
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
            }            
        }        
        private bool IsPointOnPolyline(Polyline pl1, Polyline pl2, Point3d pt, Point2dCollection po3d, double cx)
        {
            bool isOn = false;
            Polyline pl = null;
            if (pl1 == null & pl2 != null)
                pl = pl1;
            else if(pl1 != null & pl2 == null)
                pl = pl2;
            else
            {
                if (pt.X < cx)
                    pl = pl1;
                else
                    pl = pl2;
            }  
            foreach (Point2d po in po3d)
            {
                if (Math.Abs(pt.X - po.X)<1e-4 &&Math.Abs( pt.Y - po.Y)<1e-4)
                {
                    isOn = false;
                    return isOn;
                }
            }            
            for (int i = 0; i < pl.NumberOfVertices; i++)
            {
                Curve3d seg = null;
                SegmentType segType = pl.GetSegmentType(i);
                if (segType == SegmentType.Arc)
                    seg = pl.GetArcSegmentAt(i);
                else if (segType == SegmentType.Line)
                    seg = pl.GetLineSegmentAt(i);
                if (seg != null)
                {
                    isOn = seg.IsOn(pt);                    
                    if (isOn)
                        break;                                        
                }
            }
            return isOn;
        }
        public void SlopeLength(ObjectId CorID, int idx, ref double ccl, ref double cfl, ref double cflEG, ref double cclEG)
        {                        
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    System.Data.DataTable tb = new System.Data.DataTable();
                    tb.Columns.Add("sta", typeof(double));
                    tb.Columns.Add("cc", typeof(double));
                    tb.Columns.Add("cf", typeof(double));
                    tb.Columns.Add("t", typeof(double));
                    tb.Columns.Add("ccv", typeof(double));
                    tb.Columns.Add("cfv", typeof(double));
                    tb.Columns.Add("tv", typeof(double));
                    Corridor cor = trans.GetObject(CorID, OpenMode.ForWrite) as Corridor;
                    string[] pocode = cor.GetPointCodes();
                    ObjectIdCollection surfIDs = civildoc.GetSurfaceIds();
                    TinSurface surfEG = trans.GetObject(surfIDs[LS_EG.SelectedIndex], OpenMode.ForWrite) as TinSurface;                    
                    CorridorSurface corSurf = cor.CorridorSurfaces[LS_Final.SelectedIndex]; 
                    Assembly ass = trans.GetObject(civildoc.AssemblyCollection[0], OpenMode.ForRead) as Assembly;
                    OffsetAssemblyCollection OffsetAssemblies = ass.OffsetAssemblies;
                    Subassembly sub = trans.GetObject(civildoc.SubassemblyCollection[0], OpenMode.ForRead) as Subassembly;                    
                    Alignment align = trans.GetObject(cor.Baselines[LS_Align.SelectedIndex].AlignmentId, OpenMode.ForWrite) as Alignment;
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
                    ProgBar.Step = 1;
                    ProgBar.Value = 0;
                    foreach (ObjectId osamID in slg.GetSampleLineIds())
                    {
                        
                        progbarID++;
                        ProgBar.Value = progbarID;
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
                        if (osam.GetSectionViewIds().Count == 0)
                        {
                            dv = new DataView(tb);
                            ProgBar.Value = ProgBar.Maximum;
                            break;
                        }
                        ii = osam.Station;
                        a1 = secvg.Location.X - secvg.OffsetRight;
                        a2 = secvg.Location.Y + secvg.ElevationMax - secvg.ElevationMin;
                        a3 = secvg.Location.Z;
                        //-----------------------پیدا کردن ایندکس سطح ها در لیست سطوح سکشن-------------------
                        ObjectId id = ObjectId.Null;
                        ObjectIdCollection origenes = osam.GetSectionIds();
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
                        #region Points nd PolyLines Creation
                        //--------------------------نقاط سطح کوریدور--------------------------------------
                        seccor = trans.GetObject(osam.GetSectionIds()[index6], OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Section;
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
                            Point2d copo2d = new Point2d(CorSectionpoint2[i].X + secvg.Location.X, CorSectionpoint2[i].Y - secvg.ElevationMin + secvg.Location.Y);
                            plineCO.AddVertexAt(v, copo2d, 0, 0, 0);
                            v++;
                        }
                        if (chkEGFG.CheckState == CheckState.Checked)
                        {
                            plineCO.Closed = false;
                            plineCO.ColorIndex = 1;
                            blockTableRec.AppendEntity(plineCO);
                            trans.AddNewlyCreatedDBObject(plineCO, true);
                        }

                        //-------------------------------------نقاط سطح زمین اولیه----------------------
                        secEG = trans.GetObject(osam.GetSectionIds()[index5], OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Section;
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
                            Point3d popo2d = new Point3d(seccor.Station, seccor.LeftOffset, secpoCOR[0].Y);
                            Point3d popo3d = cor.Baselines[0].StationOffsetElevationToXYZ(popo2d);
                            double z = surfEG.FindElevationAtXY(popo3d.X, popo3d.Y);
                            Point3d popo4d = new Point3d(seccor.LeftOffset, z, 1);
                            secPOEG.Add(popo4d);
                        }
                        if (Math.Abs(secPOEG[secPOEG.Count - 1].X - seccor.RightOffset) > 0.001)
                        {
                            Point3d popo2d = new Point3d(seccor.Station, seccor.RightOffset, secpoCOR[secpoCOR.Count - 1].Y);
                            Point3d popo3d = cor.Baselines[0].StationOffsetElevationToXYZ(popo2d);
                            double z = surfEG.FindElevationAtXY(popo3d.X, popo3d.Y);
                            Point3d popo4d = new Point3d(seccor.RightOffset, z, 1);
                            secPOEG.Add(popo4d);
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
                            Point2d EGpo2d = new Point2d(EGSectionpoint2[i].X + secvg.Location.X, EGSectionpoint2[i].Y - secvg.ElevationMin + secvg.Location.Y);
                            plineEG.AddVertexAt(v, EGpo2d, 0, 0, 0);
                            v++;
                        }
                        if (chkEGFG.CheckState == CheckState.Checked)
                        {
                            plineEG.Closed = false;
                            plineEG.ColorIndex = 106;
                            blockTableRecEG.AppendEntity(plineEG);
                            trans.AddNewlyCreatedDBObject(plineEG, true);
                        }
                        //--------------------نقطه برخورد دو پلی لاین حاصل از کوریدور و سطح زمین-----
                        Point3dCollection interpoS = new Point3dCollection();
                        Point3dCollection interpo2 = new Point3dCollection();
                        plineEG.IntersectWith(plineCO, Intersect.OnBothOperands, interpoS, IntPtr.Zero, IntPtr.Zero);
                        for (int i = 0; i < interpoS.Count; i++)
                        {
                            Point3d popo2d = new Point3d(interpoS[i].X - secvg.Location.X, interpoS[i].Y + secvg.ElevationMin - secvg.Location.Y, 0);
                            interpo2.Add(popo2d);
                            EGSectionpoint2.Add(popo2d);
                            CorSectionpoint2.Add(popo2d);
                        }
                        Point3dCollection interpo = new Point3dCollection(interpo2.Cast<Point3d>().OrderBy(point => point.X).ToArray());
                        Point3dCollection EGSectionpoint = new Point3dCollection(EGSectionpoint2.Cast<Point3d>().OrderBy(point => point.X).ToArray());
                        Point3dCollection CorSectionpoint = new Point3dCollection(CorSectionpoint2.Cast<Point3d>().OrderBy(point => point.X).ToArray());
                        //------حذف نقاط تکراری--------------
                        for (int i = 0; i < EGSectionpoint.Count - 1; i++)
                        {
                            if (EGSectionpoint[i].X < index3)
                            {
                                EGSectionpoint.RemoveAt(i);
                                continue;
                            }
                            if (EGSectionpoint[i].X > index4)
                            {
                                EGSectionpoint.RemoveAt(i);
                                continue;
                            }
                            if ((Math.Abs(EGSectionpoint[i].X - EGSectionpoint[i + 1].X) < 0.0001))
                            {
                                EGSectionpoint.RemoveAt(i);
                                i--;
                            }

                        }
                        for (int i = 0; i < CorSectionpoint.Count - 1; i++)
                        {
                            if (CorSectionpoint[i].X < index3)
                            {
                                CorSectionpoint.RemoveAt(i);
                                continue;
                            }
                            if (CorSectionpoint[i].X > index4)
                            {
                                CorSectionpoint.RemoveAt(i);
                                continue;
                            }
                            if ((Math.Abs(CorSectionpoint[i].X - CorSectionpoint[i + 1].X) < 0.0001))
                            {
                                CorSectionpoint.RemoveAt(i);
                                i--;
                            }

                        }
                        #endregion
                        Polyline poly = null;
                        Polyline poly2 = null;
                        #region T1-Calculation
                        //------------------------------------------------------------------
                        double CCL = 0;
                        double zEG = 0;
                        double zCo = 0;
                        double xx = 0;
                        double yy = 0;
                        double xx1 = 0;
                        double yy1 = 0;
                        double xx2 = 0;
                        double yy2 = 0;
                        double x1 = 0;
                        double y1 = 0;
                        double x2 = 0;
                        double y2 = 0;
                        double temp = 0;
                        double CFL = 0;
                        List<double> wr = new List<double>();
                        List<double> wl = new List<double>();
                        //double wr = 0;
                        //double wl = 0;
                        double T = 0;
                        double T1 = 0;
                        double s1 = 0;
                        AppliedAssembly ass3 = null;
                        try
                        {
                            ass3 = cor.Baselines[LS_Cor.SelectedIndex].GetAppliedAssemblyAtStation(seccor.Station);
                        }
                        catch
                        {
                            MessageBox.Show("Add Frequency to your Corridor at Station :" + seccor.Station.ToString());
                        }
                        double zc = 0;
                        Profile prof = trans.GetObject(cor.Baselines[LS_Cor.SelectedIndex].ProfileId, OpenMode.ForRead) as Profile;
                        try
                        {
                            zc = prof.ElevationAt(seccor.Station);
                        }
                        catch
                        {
                            index22 = -1;
                            break;
                        }
                        //-------------------
                        Point3dCollection interp = new Point3dCollection();
                        for (int i = 0; i < interpo.Count; i++)
                        {
                            Point3d p1 = new Point3d(interpo[i].X + secvg.Location.X, interpo[i].Y - secvg.ElevationMin + secvg.Location.Y , 0);
                            interp.Add(p1);
                        }
                        //interpo.Clear();
                        //interpo = interp;
                        //-------------
                        CalculatedPointCollection CPC = ass3.GetPointsByCode(listBox_Codes.SelectedItem.ToString());
                        Point2dCollection colT1po2d = new Point2dCollection();
                        Point2dCollection colccpo2d = new Point2dCollection();
                        Point2dCollection colcfpo2d = new Point2dCollection();
                        for (int j = 0; j < listBox_LinkCodes.SelectedItems.Count; j++)
                        {
                            CalculatedLinkCollection callink = ass3.GetLinksByCode(listBox_LinkCodes.SelectedItems[j].ToString());
                            for (int i = 0; i < callink.Count; i++)
                            {
                                xx1 = callink[i].CalculatedPoints[0].StationOffsetElevationToBaseline.Y;
                                yy1 = callink[i].CalculatedPoints[0].StationOffsetElevationToBaseline.Z;
                                xx2 = callink[i].CalculatedPoints[1].StationOffsetElevationToBaseline.Y;
                                yy2 = callink[i].CalculatedPoints[1].StationOffsetElevationToBaseline.Z;
                                T1 = T1 + Math.Pow(((xx1 - xx2) * (xx1 - xx2) + (yy1 - yy2) * (yy1 - yy2)), .5);
                                Point2d T1PO1 = new Point2d(xx1 + secvg.Location.X, yy1 - secvg.ElevationMin + secvg.Location.Y + zc);
                                Point2d T1PO2 = new Point2d(xx2 + secvg.Location.X, yy2 - secvg.ElevationMin + secvg.Location.Y + zc);
                                colT1po2d.Add(T1PO1);
                                colT1po2d.Add(T1PO2);
                                //createPolyLine(150, colT1po2d, secvg.Location.X, interpo);
                            }

                            //colT1po2d.Clear();
                        }
                        Point2dCollection colT1po2d1 = new Point2dCollection(colT1po2d.Cast<Point2d>().OrderBy(point => point.X).ToArray());
                        Polyline polyT = null;
                        Polyline polyT2 = null;
                        CreatePolyLine(150, colT1po2d, interp, ref polyT, ref polyT2, secvg.Location.X);
                        #endregion

                        //-------------------------------------------------
                        double offbase = 0;
                        Point2dCollection poColDaylight = new Point2dCollection();
                        for (int i = 0; i < CPC.Count; i++)
                        {
                            offbase = CPC[i].StationOffsetElevationToBaseline.Y;
                            x1 = CPC[i].StationOffsetElevationToBaseline.Y + secvg.Location.X;
                            y1 = CPC[i].StationOffsetElevationToBaseline.Z - secvg.ElevationMin + secvg.Location.Y + zc;
                            Point2d po2 = new Point2d(x1, y1);
                            poColDaylight.Add(po2);
                            if (offbase < 0)
                                wl.Add(offbase);
                            else
                                wr.Add(offbase);
                        }
                        wl.Sort();
                        wr.Sort();
                        #region CF-Claculation
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
                                Point2d CFPO1 = new Point2d(x1 + secvg.Location.X, y1 - secvg.ElevationMin + secvg.Location.Y);
                                Point2d CFPO2 = new Point2d(x2 + secvg.Location.X, y2 - secvg.ElevationMin + secvg.Location.Y);
                                colcfpo2d.Add(CFPO1);
                                colcfpo2d.Add(CFPO2);
                            }
                        }
                        CreatePolyLine(5, colcfpo2d, interp, ref poly, ref poly2, secvg.Location.X);
                        #endregion
                        #region T-Calculation
                        //---------------------------------------------
                        x1 = 0;
                        y1 = 0;
                        x2 = 0;
                        y2 = 0;
                        temp = 0;
                        for (int i = 0; i < CorSectionpoint.Count - 1; i++)
                        {
                            if (i < CorSectionpoint.Count - 1)
                            {
                                align.PointLocation(secEG.Station, (CorSectionpoint[i].X + CorSectionpoint[i + 1].X) / 2, ref xx, ref yy);
                                try
                                {
                                    zCo = (CorSectionpoint[i].Y + CorSectionpoint[i + 1].Y) / 2;// corSurf.FindElevationAtXY(xx, yy);
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
                                Point2d CLPO1 = new Point2d(x1 + secvg.Location.X, y1 - secvg.ElevationMin + secvg.Location.Y);
                                Point2d CLPO2 = new Point2d(x2 + secvg.Location.X, y2 - secvg.ElevationMin + secvg.Location.Y);
                                Point3d po1 = new Point3d(CLPO1.X, CLPO1.Y, 0);
                                Point3d po2 = new Point3d(CLPO2.X, CLPO2.Y, 0);
                                if (chkDraw.CheckState == CheckState.Checked)
                                {
                                    if(polyT==null & polyT2==null)
                                    {
                                        colccpo2d.Add(CLPO1);
                                        colccpo2d.Add(CLPO2);
                                    }
                                    else
                                    {
                                        if (IsPointOnPolyline(polyT, polyT2, po1, poColDaylight, secvg.Location.X) == false)
                                            colccpo2d.Add(CLPO1);
                                        if (IsPointOnPolyline(polyT, polyT2, po2, poColDaylight, secvg.Location.X) == false)
                                            colccpo2d.Add(CLPO2);
                                    }                                                                 
                                }                                    
                            }
                        }                        
                        CreatePolyLine(6, colccpo2d, interp, ref poly, ref poly2, secvg.Location.X);
                        #endregion
                        //--------------------------------
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
                            continue;
                        }                        
                        double cc = 0;
                        if ( polyT != null & polyT2 != null )
                            if ( polyT.NumberOfVertices != 0 & polyT2.NumberOfVertices == 0 )
                                cc = CCL - polyT.Length;// CCL - T1;        
                            else if ( polyT.NumberOfVertices == 0 & polyT2.NumberOfVertices != 0 )
                                cc = CCL - polyT2.Length;
                            else if ( polyT.NumberOfVertices != 0 & polyT2.NumberOfVertices != 0 )
                                cc = CCL - polyT2.Length - polyT.Length;
                        T = CCL;
                        cc = CCL - T1;
                        lblstr2 =  secEG.Station.ToString("F3") + "," + Convert.ToString(cc.ToString("F3")) 
                            + "," + Convert.ToString(CFL.ToString("F3")) + "," + Convert.ToString(T.ToString("F3"));
                        lblstr = "CC=" + Convert.ToString(cc.ToString("F3")) + "\nCF=" + Convert.ToString(CFL.ToString("F3")) 
                            + "\nT=" + Convert.ToString(T.ToString("F3"));
                        if(idx==0) secvg.Description = lblstr;                        
                        STRlength.Add(lblstr2);
                        tb.Rows.Add(secEG.Station, cc, CFL, T, 0, 0, 0);
                        CorSectionpoint2.Clear();
                        secpoCOR.Clear();
                        secPOEG.Clear();
                        EGSectionpoint.Clear();
                        CorSectionpoint.Clear();
                        interpoS.Clear();
                        colccpo2d.Clear();
                        colcfpo2d.Clear();
                        colT1po2d.Clear();
                    }
                    dv = new DataView(tb);
                }
                catch (System.Exception ex)
                {
                    NOErr++;
                    
                    ed.WriteMessage("\n" + ex.ToString());                    
                    ed.WriteMessage("\n" + ex.StackTrace);
                    ed.WriteMessage("\n Error on station: " + ii.ToString());

                }
                trans.Commit();
            }
        }        
        public STR()
        {
            InitializeComponent();
            LS_Cor.Items.Clear();
            LS_SLG.Items.Clear();
            LS_EG.Items.Clear();
            LS_Align.Items.Clear();
            LS_Final.Items.Clear();
            if (checkBox1.Checked == true)
            {
                RightETWWidth.Enabled = false;                
            }
            //
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    CorridorCollection corcolec = civildoc.CorridorCollection;
                    if (corcolec == null)
                    {
                        MessageBox.Show("You must have at least one Corridor", "No Corridor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new System.Exception("There is no Corridor");
                    }
                    //Corridor cor = null;
                    foreach (ObjectId objId in civildoc.CorridorCollection)
                    {
                        Corridor cors = trans.GetObject(objId, OpenMode.ForRead) as Corridor;
                        //cor = cors;
                        LS_Cor.Items.Add(cors.Name);
                    }
                    LS_Cor.SelectedIndex = 0;
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                    NOErr++;
                }
                trans.Commit();
            }
        }
        private void LS_Cor_SelectedIndexChanged(object sender, EventArgs e)
        {        
            LS_SLG.Items.Clear();
            LS_EG.Items.Clear();
            LS_Align.Items.Clear();
            LS_Final.Items.Clear();
            
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    string texcor = LS_Cor.SelectedItem.ToString();
                    foreach (ObjectId objId in civildoc.CorridorCollection)
                    {
                        Corridor cors = trans.GetObject(objId, OpenMode.ForRead) as Corridor;
                        if (cors.Name == texcor) corid = objId;
                    }
                    Corridor cor = trans.GetObject(corid, OpenMode.ForRead) as Corridor;
                    Corridor = cor;
                    CorID = cor.ObjectId;
                    List<string> PointCodes = cor.GetPointCodes().ToList();
                    PointCodes.Sort();
                    foreach (string codestr in PointCodes)
                    {
                        listBox_Codes.Items.Add(codestr);
                    }
                    List<string> linkCodes = cor.GetLinkCodes().ToList();
                    linkCodes.Sort();
                    listBox_Codes.SelectedIndex = 0;
                    foreach (string codestr in linkCodes)
                    {
                        listBox_LinkCodes.Items.Add(codestr);
                    }
                    listBox_LinkCodes.SelectedIndex = 0;
                    foreach (Baseline oBaseline in cor.Baselines)
                    {
                        Alignment align2 = trans.GetObject(oBaseline.AlignmentId, OpenMode.ForRead) as Alignment;
                        LS_Align.Items.Add(align2.Name);
                    }
                    LS_Align.SelectedIndex = 0;
                    foreach (CorridorSurface corsurf in cor.CorridorSurfaces)
                    {
                        LS_Final.Items.Add(corsurf.Name);
                    }
                    LS_Final.SelectedIndex = 0;
                    ObjectIdCollection id = civildoc.GetSurfaceIds();
                    foreach (ObjectId ID in id)
                    {
                        Autodesk.Civil.DatabaseServices.Surface surf = trans.GetObject(ID, OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Surface;
                        LS_EG.Items.Add(surf.Name);
                    }
                    LS_EG.SelectedIndex = 0;
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                    NOErr++;
                }
                trans.Commit();
            }
        }
        private void LS_Align_SelectedIndexChanged(object sender, EventArgs e)
        {
            object selalg = LS_Align.SelectedItem;
            LS_SLG.Items.Clear();      
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    foreach (ObjectId algid in civildoc.GetAlignmentIds())
                    {
                        Alignment alg = trans.GetObject(algid, OpenMode.ForWrite) as Alignment;
                        if (alg.Name == selalg.ToString()) align = alg;
                    }
                    foreach (ObjectId slgID in align.GetSampleLineGroupIds())
                    {
                        SampleLineGroup slg = trans.GetObject(slgID, OpenMode.ForWrite) as SampleLineGroup;
                        LS_SLG.Items.Add(slg.Name);
                    }
                    LS_SLG.SelectedIndex = 0;

                }
                catch (System.Exception ex)
                {
                    NOErr++;
                    ed.WriteMessage("\n" + ex.Message);
                }
                trans.Commit();
            }
        }
        private void LS_SLG_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void BTN_OK_Click(object sender, EventArgs e)
        {

            elaptime.Start();
            TimeElapseStripStatus.Text = "Elapsed Time: 00:00:00 ";
            ProgBar.Value = 0;
            ErrorNOStripStatus.Text = "Errors: 0";
            double ccl = 0;
            double cfl = 0;
            double ccleg = 0;
            double cfleg = 0;
            SlopeLength(corid,0, ref ccl, ref cfl, ref ccleg, ref cfleg);
            //this.Close();
            elaptime.Stop();
            TimeElapseStripStatus.Text = "Elapsed Time: " + elaptime.Elapsed.ToString().Remove(8);//.Hours+":"+elaptime.Elapsed.Minutes+":"+elaptime.Elapsed.Seconds;
            ErrorNOStripStatus.Text = "Errors: "+NOErr.ToString();
            elaptime.Reset();
            NOErr = 0;
        }
        private void BTN_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                RightETWWidth.Enabled = true;
            }
            else RightETWWidth.Enabled = false;
        }
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double ccl = 0;
            double cfl = 0;
            double ccleg = 0;
            double cfleg = 0;
            double ccv = 0;
            double cfv = 0;
            double tv = 0;
            STRlength.Clear();
            STRlength.Add("STA,CC,CF,T,CCA,CFA,TA");
            STRlength.Add(dv[0][0] + "," + dv[0][1] + "," + dv[0][2] + "," + dv[0][3] + "," + dv[0][4] + "," + dv[0][5] + "," + dv[0][6]);

            for (int i=1 ; i < dv.Count ; i++)
            {
                ccv = ccv + ((double)dv[i][0] - (double)dv[i - 1][0]) / 2 * ((double)dv[i][1] + (double)dv[i - 1][1]);
                cfv = cfv + ((double)dv[i][0] - (double)dv[i - 1][0]) / 2 * ((double)dv[i][2] + (double)dv[i - 1][2]);
                tv = tv + ((double)dv[i][0] - (double)dv[i - 1][0]) / 2 * ((double)dv[i][3] + (double)dv[i - 1][3]);
                dv[i].Row.BeginEdit();
                dv[i][4] = ccv;
                dv[i][5] = cfv;
                dv[i][6] = tv;
                STRlength.Add(dv[i][0] + "," + dv[i][1] + "," + dv[i][2] + "," + dv[i][3] + "," + dv[i][4] + "," + dv[i][5] + "," + dv[i][6]);
            }
            //SlopeLength(corid,1, ref ccl, ref cfl, ref ccleg, ref cfleg);
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
                    NOErr++;
                    MessageBox.Show("Error writing to file", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sections.About_Stripping_Length win1 = new About_Stripping_Length();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModelessDialog(win1);
        }
        private void ListBox_Codes_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    AppliedAssembly ass3 = Corridor.Baselines[0].GetAppliedAssemblyAtStation(Corridor.Baselines[0].StartStation);
                    CalculatedPointCollection CPC = ass3.GetPointsByCode(listBox_Codes.SelectedItem.ToString());
                    double offbase = 0;
                    RightETWWidth.Text = "";
                    LeftETWWidth.Text = "";
                    
                    for (int i = 0; i < CPC.Count; i++)
                    {
                        offbase = CPC[i].StationOffsetElevationToBaseline.Y;
                        if (offbase < 0)
                            LeftETWWidth.Text = (-1*offbase).ToString();
                        else
                            RightETWWidth.Text = offbase.ToString();
                    }
                }
                catch (System.Exception ex)
                {
                    NOErr++;
                    ed.WriteMessage("\n" + ex.Message);
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
                trans.Commit();
            }

            //CalculatedPointCollection CPC = ass.get.GetPointsByCode(listBox_Codes.SelectedItem.ToString());
            //double offbase = 0;

            //for (int i = 0; i < CPC.Count; i++)
            //{
            //    offbase = CPC[i].StationOffsetElevationToBaseline.Y;
            //    if (offbase < 0)
            //        wl = offbase;
            //    else
            //        wr = offbase;
            //}
        }
        private void saveAsXLSToolStripMenuItem_Click (object sender, EventArgs e)
        {
            double ccl = 0;
            double cfl = 0;
            double ccleg = 0;
            double cfleg = 0;
            double ccv = 0;
            double cfv = 0;
            double tv = 0;
            STRlength.Clear();
            STRlength.Add("STA,CC,CF,T,CCA,CFA,TA");
            STRlength.Add(dv[0][0] + "," + dv[0][1] + "," + dv[0][2] + "," + dv[0][3] + "," + dv[0][4] + "," + dv[0][5] + "," + dv[0][6]);

            for (int i=1 ; i < dv.Count ; i++)
            {
                ccv = ccv + ((double)dv[i][0] - (double)dv[i - 1][0]) / 2 * ((double)dv[i][1] + (double)dv[i - 1][1]);
                cfv = cfv + ((double)dv[i][0] - (double)dv[i - 1][0]) / 2 * ((double)dv[i][2] + (double)dv[i - 1][2]);
                tv = tv + ((double)dv[i][0] - (double)dv[i - 1][0]) / 2 * ((double)dv[i][3] + (double)dv[i - 1][3]);
                dv[i].Row.BeginEdit();
                dv[i][4] = ccv;
                dv[i][5] = cfv;
                dv[i][6] = tv;
                STRlength.Add(dv[i][0] + "," + dv[i][1] + "," + dv[i][2] + "," + dv[i][3] + "," + dv[i][4] + "," + dv[i][5] + "," + dv[i][6]);
            }
            
            



        }
    }
}