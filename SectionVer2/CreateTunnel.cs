using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using System.Diagnostics;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;


using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using System.IO;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;

namespace Sections
{
    public partial class CreateTunnel : Form
    {
        Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
        CivilDocument civildoc = CivilApplication.ActiveDocument;
        Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
        Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
        Stopwatch elaptime = new Stopwatch();
        private StreamReader filereader;
        private StreamWriter filewriter;
        public DataView dvFinal;
        public string[] TextFile;
        public List<double> slgsta;
        public Alignment align2;
        public SampleLineGroup slg;
        public ObjectId alignID2;
        public double[] minoff2;
        public double[] maxoff2;
        private MenuStrip menuStrip1;
        public TinSurface surface;
        public double[] Station2;
        public Polyline polysample;
        public string ii { get; set; }
        public CreateTunnel()
        {            
            InitializeComponent();
            LS_SLG.Items.Clear();
            LS_Prof.Items.Clear();
            LS_Alg.Items.Clear();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    ObjectIdCollection alIDs = civildoc.GetAlignmentIds();
                    if (alIDs == null)
                    {
                        System.Windows.Forms.MessageBox.Show("You must have at least one alignment", "No Alignment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new System.Exception("There is no alignment");
                    }
                    int coalid = alIDs.Count;                    
                    Alignment align = null;
                    for (int i = 0; i < coalid; i++)
                    {
                        ObjectId alignID = alIDs[i];
                        align = trans.GetObject(alignID, OpenMode.ForRead) as Alignment;                        
                        LS_Alg.Items.Add(align.Name);
                    }                    
                    LS_Alg.SelectedIndex = 0;                    
                    
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                }
                trans.Commit();
            }
        }

        [Obsolete]
        private void CreateBTN_Click_1(object sender, EventArgs e)
        {
            elaptime.Start();
            TimeElapseStripStatus.Text = "Elapsed Time: 00:00:00 ";
            ProgBar.Value = 0;
            ErrorNOStripStatus.Text = "Errors: 0";
            DataView dvFinal = null;
            DataView dv = null;
            DataView dvTunnel = null;
            List<double> Station = null;
            ProgBar.Maximum = 100;
            ProgBar.Step = 1;
            ProgBar.Value = 0;
            if (chkwhitoutslg.Checked == true)
                Sort_PointsBYSTA(ref dvFinal, ref dv, ref Station, ref dvTunnel);
            else
                Sort_Points(ref dvFinal, ref dv, ref Station, ref dvTunnel);
            ProgBar.Value = 100;
            if (dv == null || dvTunnel == null || Station == null || dvFinal == null)
            {
                MessageBox.Show("Review Your Data and Input File!", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorNOStripStatus.Text = "Errors: 1";
            }
            else
                CreateSurf(dv, dvTunnel, Station);
            elaptime.Stop();
            TimeElapseStripStatus.Text = "Elapsed Time: " + elaptime.Elapsed.ToString().Remove(8);
            elaptime.Reset();
        }

        [Obsolete]
        public void CreateSurf(DataView dv, DataView dv4, List<double> Station)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                
                double maxOff = 0;
                dv.Sort = "OFF ASC";
                if (Math.Abs((double)(dv[0][4])) > (double)(dv[dv.Count - 1][4]))
                {
                    maxOff = Math.Abs((double)(dv[0][4]));
                }
                else
                {
                    maxOff = (double)(dv[dv.Count - 1][4]);
                }
                string styleName = "Tunnel Style";
                ObjectId tsId = ObjectId.Null;
                DBDictionary sd = (DBDictionary)trans.GetObject(db.TableStyleDictionaryId, OpenMode.ForRead);
                TableStyle ts = new TableStyle();
                ts.SetGridColor(Color.FromColorIndex(ColorMethod.ByAci, 1), (int)GridLineType.InnerGridLines, (int)(RowType.HeaderRow | RowType.TitleRow | RowType.DataRow));
                ts.SetGridColor(Color.FromColorIndex(ColorMethod.ByAci, 1), (int)GridLineType.HorizontalTop, (int)(RowType.DataRow));
                ts.SetGridColor(Color.FromColorIndex(ColorMethod.ByAci, 1), (int)GridLineType.HorizontalBottom, (int)(RowType.HeaderRow));
                ts.SetGridColor(Color.FromColorIndex(ColorMethod.ByAci, 1), (int)GridLineType.HorizontalTop, (int)(RowType.HeaderRow));
                ts.SetAlignment(CellAlignment.MiddleCenter, (int)(RowType.HeaderRow | RowType.TitleRow | RowType.DataRow));
                ts.SetGridColor(Color.FromColorIndex(ColorMethod.ByAci, 5), (int)GridLineType.HorizontalTop, (int)(RowType.TitleRow));
                ts.SetGridColor(Color.FromColorIndex(ColorMethod.ByAci, 5), (int)GridLineType.VerticalLeft, (int)(RowType.TitleRow));
                ts.SetGridColor(Color.FromColorIndex(ColorMethod.ByAci, 5), (int)GridLineType.VerticalRight, (int)(RowType.TitleRow));
                ts.SetGridColor(Color.FromColorIndex(ColorMethod.ByAci, 5), (int)GridLineType.VerticalLeft, (int)(RowType.HeaderRow));
                ts.SetGridColor(Color.FromColorIndex(ColorMethod.ByAci, 5), (int)GridLineType.VerticalRight, (int)(RowType.HeaderRow));
                ts.SetGridColor(Color.FromColorIndex(ColorMethod.ByAci, 5), (int)GridLineType.VerticalLeft, (int)(RowType.DataRow));
                ts.SetGridColor(Color.FromColorIndex(ColorMethod.ByAci, 5), (int)GridLineType.VerticalRight, (int)(RowType.DataRow));
                sd.UpgradeOpen();
                tsId = sd.SetAt(styleName, ts);
                trans.AddNewlyCreatedDBObject(ts, true);
                sd.DowngradeOpen();
                Point3dCollection po3d2 = new Point3dCollection();
                Point3dCollection po3dcol2 = new Point3dCollection();
                Point2dCollection po2d2 = new Point2dCollection();
                Point2dCollection posamplecol = new Point2dCollection();
                ObjectIdCollection pol3dobjcol = new ObjectIdCollection();
                DataView dv3 = null;
                dv3 = dv4;
                PromptPointResult se = ed.GetPoint("Select Insert Point");
                double X_insert = se.Value.X;
                double Y_insert = se.Value.Y;
                PromptPointResult se2 = ed.GetPoint("\n Select CenterLine Point:");
                double Xc = se2.Value.X;
                double Yc = se2.Value.Y;
                int id = 0;
                double elev = 0;
                double elevMin = 0;
                double height = polysample.Bounds.Value.MaxPoint.Y - polysample.Bounds.Value.MinPoint.Y;
                System.Data.DataTable table = new System.Data.DataTable();
                table.Columns.Add("A1", typeof(double));
                table.Columns.Add("A2", typeof(double));
                table.Columns.Add("A3", typeof(double));
                Point3dCollection pos = new Point3dCollection();
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                //--------Create 3D PolyLines and 2D PolyLines-----------
                try
                {
                    for (int i = 0; i < Station.Count; i++)
                    {
                        Profile prof = trans.GetObject(align2.GetProfileIds()[LS_Prof.SelectedIndices[0]], OpenMode.ForRead) as Profile;
                        elev = prof.ElevationAt(Station[i]);

                        elevMin = prof.ElevationMin;
                        for (int j = 0; j < dv3.Count; j++)
                        {
                            if ((double)(dv3[j][3]) == Station[i])
                            {

                                Point3d po3d = new Point3d((double)(dv3[j][0]), (double)(dv3[j][1]), (double)(dv3[j][2]));
                                Point2d po2d = new Point2d(X_insert + 2.5 * id * maxOff + (double)(dv3[j][4]), Y_insert + (double)(dv3[j][2]) - elevMin);
                                po3d2.Add(po3d);
                                po2d2.Add(po2d);
                            }
                        }
                        if (po2d2.Count == 0) continue;
                        if (check3dpoly.Checked == true)
                        {
                            Autodesk.AutoCAD.DatabaseServices.Polyline3d pol3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();
                            btr.AppendEntity(pol3d);
                            trans.AddNewlyCreatedDBObject(pol3d, true);
                            pol3d.SetDatabaseDefaults();
                            foreach (Point3d poo in po3d2)
                            {
                                PolylineVertex3d vex3d = new PolylineVertex3d(poo);
                                pol3d.AppendVertex(vex3d);
                                trans.AddNewlyCreatedDBObject(vex3d, true);
                            }
                            pol3d.Closed = true;
                            pol3dobjcol.Add(pol3d.ObjectId);
                            po3d2.Clear();
                        }
                        Autodesk.AutoCAD.DatabaseServices.Polyline pol2d = new Autodesk.AutoCAD.DatabaseServices.Polyline();
                        btr.AppendEntity(pol2d);
                        trans.AddNewlyCreatedDBObject(pol2d, true);
                        pol2d.SetDatabaseDefaults();
                        int v = 0;
                        foreach (Point2d poi in po2d2)
                        {
                            pol2d.AddVertexAt(v, poi, 0, 0, 0);
                            v++;
                        }
                        pol2d.Closed = true;
                        po2d2.Clear();
                        Polyline polysample2 = polysample.Clone() as Polyline;
                        Vector3d pom = new Vector3d(X_insert + 2.5 * id * maxOff - Xc, Y_insert + elev - elevMin - Yc, 0);
                        Matrix3d mat = Matrix3d.Displacement(pom);
                        polysample2.TransformBy(mat);
                        btr.AppendEntity(polysample2);
                        trans.AddNewlyCreatedDBObject(polysample2, true);                        
                        //------------Create Regions and Hatches and Section Views and Tables------------------------------------
                        if (checkBoxSection.Checked == true)
                        {
                            DBObjectCollection objs = new DBObjectCollection();
                            objs.Add(pol2d);
                            objs.Add(polysample2);
                            if (pol2d.NumberOfVertices < 3) continue;
                            DBObjectCollection myRegionColl = new DBObjectCollection();
                            myRegionColl = Autodesk.AutoCAD.DatabaseServices.Region.CreateFromCurves(objs);
                            Region reg1 = myRegionColl[0] as Region;
                            reg1.ColorIndex = 33;
                            btr.AppendEntity(reg1);
                            trans.AddNewlyCreatedDBObject(reg1, true);
                            Region reg3 = null;
                            Region reg4 = null;
                            Region reg2 = null;
                            Region reg5 = null;
                            Region reg6 = null;
                            if (myRegionColl.Count > 1)
                            {
                                reg2 = myRegionColl[1] as Region;
                                reg2.ColorIndex = 44;
                                btr.AppendEntity(reg2);
                                trans.AddNewlyCreatedDBObject(reg2, true);
                                reg3 = reg1.Clone() as Region;
                                reg5 = reg2.Clone() as Region;
                                reg3.BooleanOperation(BooleanOperationType.BoolSubtract, reg5);
                                reg3.ColorIndex = 55;
                                btr.AppendEntity(reg3);
                                trans.AddNewlyCreatedDBObject(reg3, true);
                                reg4 = reg2.Clone() as Region;
                                reg6 = reg1.Clone() as Region;
                                reg4.BooleanOperation(BooleanOperationType.BoolSubtract, reg6);
                                reg4.ColorIndex = 75;
                                btr.AppendEntity(reg4);
                                trans.AddNewlyCreatedDBObject(reg4, true);
                            }
                            //----------------Section Views Creation--------------------------------
                            Polyline BoundPoly = null;
                            BoundryPolygone(elev, pol2d, polysample2, ref BoundPoly);
                            //--------------Bottom Tables Creation Data----------------------------- 
                            Point3d potab = new Point3d(BoundPoly.Bounds.Value.MinPoint.X, BoundPoly.Bounds.Value.MinPoint.Y - 1, 0);
                            pos.Add(potab);
                            if (reg2 == null || reg3 == null)
                                table.Rows.Add(reg1.Area, 0, 0);
                            else if (reg1 == null)
                                table.Rows.Add(0, reg2.Area, reg3.Area);
                            else if (reg2 == null)
                                table.Rows.Add(reg1.Area, 0, reg3.Area);
                            else if (reg3 == null)
                                table.Rows.Add(reg1.Area, 0, 0);
                            else
                                table.Rows.Add(reg1.Area, reg2.Area, reg3.Area);
                            //------------------------------------------------
                            ObjectIdCollection ObjIds = new ObjectIdCollection();
                            //-----------------Regions and Hatches-------------------------------
                            if (checkBoxHatch.Checked == true)
                            {
                                if (reg3 != null && reg3.Area != 0)
                                {
                                    DBObjectCollection objs3 = null;
                                    RegionToPolyline(reg3, ref objs3);
                                    foreach (Autodesk.AutoCAD.DatabaseServices.DBObject dbo in objs3)
                                    {
                                        Polyline pp = dbo as Polyline;
                                        if (pp != null)
                                        {
                                            ObjIds.Add(pp.ObjectId);
                                            Hatch achatch3 = new Hatch();
                                            achatch3.ColorIndex = 1;
                                            achatch3.PatternScale = .05;
                                            achatch3.SetHatchPattern(HatchPatternType.PreDefined, "ANSI37");
                                            btr.AppendEntity(achatch3);
                                            trans.AddNewlyCreatedDBObject(achatch3, true);
                                            achatch3.AppendLoop(HatchLoopTypes.Default, ObjIds);
                                            achatch3.EvaluateHatch(true);
                                            ObjIds.Clear();
                                        }
                                    }
                                }
                                ObjIds.Clear();
                                if (reg4 != null && reg4.Area != 0)
                                {
                                    DBObjectCollection objs4 = null;
                                    RegionToPolyline(reg4, ref objs4);
                                    foreach (Autodesk.AutoCAD.DatabaseServices.DBObject dbo in objs4)
                                    {
                                        Polyline pp = dbo as Polyline;
                                        if (pp != null)
                                        {
                                            ObjIds.Add(pp.ObjectId);
                                            Hatch achatch3 = new Hatch();
                                            achatch3.ColorIndex = 5;
                                            achatch3.PatternScale = .05;
                                            achatch3.SetHatchPattern(HatchPatternType.PreDefined, "ANSI37");
                                            btr.AppendEntity(achatch3);
                                            trans.AddNewlyCreatedDBObject(achatch3, true);
                                            achatch3.AppendLoop(HatchLoopTypes.Default, ObjIds);
                                            achatch3.EvaluateHatch(true);
                                            ObjIds.Clear();
                                        }
                                    }
                                }
                                ObjIds.Clear();
                                if (reg3 != null && reg4 != null && reg4.Area != 0 && reg3.Area != 0)
                                {
                                    DBObjectCollection objs4 = null;
                                    RegionToPolyline(reg4, ref objs4);
                                    foreach (Autodesk.AutoCAD.DatabaseServices.DBObject dbo in objs4)
                                    {
                                        Polyline pp = dbo as Polyline;
                                        if (pp != null)
                                        {
                                            ObjIds.Add(pp.ObjectId);
                                            Hatch achatch4 = new Hatch();
                                            achatch4.ColorIndex = 5;
                                            achatch4.PatternScale = .05;
                                            achatch4.SetHatchPattern(HatchPatternType.PreDefined, "ANSI37");
                                            btr.AppendEntity(achatch4);
                                            trans.AddNewlyCreatedDBObject(achatch4, true);
                                            achatch4.AppendLoop(HatchLoopTypes.Default, ObjIds);
                                            achatch4.EvaluateHatch(true);
                                            ObjIds.Clear();
                                        }
                                    }
                                }
                                else if (reg3 != null && reg4 != null && reg4.Area == 0 && reg3.Area == 0)
                                {
                                    ObjIds.Add(pol2d.ObjectId);
                                    Hatch achatch5 = new Hatch();
                                    achatch5.ColorIndex = 5;
                                    achatch5.PatternScale = .05;
                                    achatch5.SetHatchPattern(HatchPatternType.PreDefined, "ANSI37");
                                    btr.AppendEntity(achatch5);
                                    trans.AddNewlyCreatedDBObject(achatch5, true);
                                    achatch5.AppendLoop(HatchLoopTypes.Default, ObjIds);
                                    ObjIds.Clear();
                                    ObjIds.Add(polysample2.ObjectId);
                                    achatch5.AppendLoop(HatchLoopTypes.Default, ObjIds);
                                    achatch5.EvaluateHatch(true);
                                    ObjIds.Clear();
                                }
                                else
                                {
                                    ObjIds.Add(pol2d.ObjectId);
                                    Hatch achatch5 = new Hatch();
                                    achatch5.ColorIndex = 5;
                                    achatch5.PatternScale = .05;
                                    achatch5.SetHatchPattern(HatchPatternType.PreDefined, "ANSI37");
                                    btr.AppendEntity(achatch5);
                                    trans.AddNewlyCreatedDBObject(achatch5, true);
                                    achatch5.AppendLoop(HatchLoopTypes.Default, ObjIds);
                                    ObjIds.Clear();
                                    ObjIds.Add(polysample2.ObjectId);
                                    achatch5.AppendLoop(HatchLoopTypes.Default, ObjIds);
                                    achatch5.EvaluateHatch(true);
                                    ObjIds.Clear();
                                    ObjIds.Add(polysample2.ObjectId);
                                    Hatch achatch4 = new Hatch();
                                    achatch4.ColorIndex = 13;
                                    achatch4.PatternScale = .05;
                                    achatch4.SetHatchPattern(HatchPatternType.PreDefined, "ANSI37");
                                    btr.AppendEntity(achatch4);
                                    trans.AddNewlyCreatedDBObject(achatch4, true);
                                    achatch4.AppendLoop(HatchLoopTypes.Default, ObjIds);
                                    achatch4.EvaluateHatch(true);
                                    ObjIds.Clear();
                                }
                            }
                            //------Text Distance Between Two Surface-----
                            ObjIds.Clear();
                            double dist = 0;
                            for (int k = 0; k < pol2d.NumberOfVertices; k++)
                            {
                                dist = polysample2.GetGeCurve().GetDistanceTo(pol2d.GetPoint3dAt(k));
                                Point3d popo = polysample2.GetGeCurve().EvaluatePoint(dist);
                                PointOnCurve3d po = polysample2.GetGeCurve().GetClosestPointTo(pol2d.GetPoint3dAt(k));
                                Point3d po3d = pol2d.GetPoint3dAt(k);
                                DBText Text = new DBText();

                                double rot = Math.Atan((pol2d.GetPoint3dAt(k).Y - po.Point.Y) / (pol2d.GetPoint3dAt(k).X - po.Point.X));
                                Text.Height = .1;                                

                                if (isinnside(polysample2, pol2d.GetPoint3dAt(k), po.Point, rot))
                                    Text.TextString = "-" + dist.ToString("F3");
                                else
                                    Text.TextString = "+" + dist.ToString("F3");

                                if (po.Point.X < (polysample2.Bounds.Value.MaxPoint.X + polysample2.Bounds.Value.MinPoint.X) / 2)
                                {
                                    Text.HorizontalMode = TextHorizontalMode.TextRight;
                                    Text.AlignmentPoint = new Point3d(pol2d.GetPoint3dAt(k).X, pol2d.GetPoint3dAt(k).Y, 0);                            
                                }
                                else
                                {
                                    Text.Position = new Point3d(pol2d.GetPoint3dAt(k).X, pol2d.GetPoint3dAt(k).Y, 0);
                                }
                                Text.Rotation = rot;
                                btr.AppendEntity(Text);
                                trans.AddNewlyCreatedDBObject(Text, true);
                            }
                            objs.Clear();
                            reg1.Erase();
                            reg2.Erase();
                            reg3.Erase();
                            reg4.Erase();
                        }
                        id = id + 5;
                    }
                    Create_Table(bt, btr, table, pos, Station, trans, tsId);
                    if (chkformat.Checked == true)
                    {
                        ObjectId SL2 = SampleLineGroup.Create("SL - " + align2.Name + DateTime.UtcNow.Second.ToString(), align2.ObjectId);

                        for (int i = 0; i < Station.Count; i++)
                        {
                            SampleLine.Create("SL-" + Station[i].ToString("F3"), SL2, Station[i]);
                        }
                    }
                    if (chkSurface.Checked == true)
                    {
                        //----------Point Group Creation--------------
                        CogoPointCollection cog = civildoc.CogoPoints;
                        Point3dCollection po3dcol = new Point3dCollection();
                        ObjectId postyle = civildoc.Styles.PointStyles[0];
                        ObjectId groupId = civildoc.PointGroups.Add(align2.Name.ToString() + DateTime.Today.ToShortDateString() + DateTime.Now.ToShortTimeString());
                        //
                        PointGroup group = groupId.GetObject(OpenMode.ForWrite) as PointGroup;
                        string Desc = "";
                        for (int i = 0; i < dv4.Count; i++)
                        {
                            Point3d po3d = new Point3d((double)(dv4[i][0]), (double)(dv4[i][1]), (double)(dv4[i][2]));
                            if (Math.Abs((double)(dv4[i][4])) <= 0.0001)
                                Desc = "STA = " + dv4[i][3].ToString();
                            else
                                Desc = "STA = " + dv4[i][3].ToString() + " Off= " + dv4[i][4].ToString();
                            ObjectId pointIds = cog.Add(po3d, Desc, true);

                        }
                        StandardPointGroupQuery query = new StandardPointGroupQuery();
                        query.IncludeRawDescriptions = "STA*";
                        PointGroup groupPO = groupId.GetObject(OpenMode.ForWrite) as PointGroup;
                        groupPO.SetQuery(query);
                        Autodesk.Civil.Settings.SettingsPoint pointSettings = civildoc.Settings.GetSettings<Autodesk.Civil.Settings.SettingsPoint>() as Autodesk.Civil.Settings.SettingsPoint;
                        ObjectId surfaceStyleId = civildoc.Styles.SurfaceStyles[0];
                        ObjectId surfaceId = TinSurface.Create("Surface - " + align2.Name.ToString(), surfaceStyleId);
                        surface = surfaceId.GetObject(OpenMode.ForWrite) as TinSurface;
                        surface.Rebuild();
                        surface.BreaklinesDefinition.AddStandardBreaklines(pol3dobjcol, 1, 1, 1, 1);
                        surface.Rebuild();
                        surface.Rebuild();
                    }
                    trans.Commit();
                }                
                catch (System.Exception ex)
                {
                    var st = new System.Diagnostics.StackTrace(ex, true);
                    var frame = st.GetFrame(0);
                    var line = frame.GetFileLineNumber();
                    ed.WriteMessage("\n" + ex.Message + "\n Line Number" + line + "\n You must remove or edit Point N.O:" + ii);
                }
            }
        }

        [Obsolete]
        public void Create_Table(BlockTable bt, BlockTableRecord btr, System.Data.DataTable table, Point3dCollection pos, 
            List<double> Station, Transaction trans, ObjectId tsId)
        {
            double OverEx = 0;
            double UnderExc = 0;
            double Exc = 0;
            double Exc_Vol = 0;
            double OverEx_Vol = 0;
            double UnderExc_Vol = 0;
            double SumExc_Vol = 0;
            double SumOverEx_Vol = 0;
            double SumUnderExc_Vol = 0;
            double Exc_Vol_P = 0;
            double OverEx_Vol_P = 0;
            double UnderExc_Vol_P = 0;
            double SumExc_Vol_P = 0;
            double SumOverEx_Vol_P = 0;
            double SumUnderExc_Vol_P = 0;
            DataView dv = new DataView(table);
            string sta0 = Station[0].ToString();
            for (int i = 0; i < dv.Count; i++)
            {
                if(i!=0& i != dv.Count - 1)
                {
                    OverEx_Vol = (Station[i] - Station[i - 1]) * ((double)dv[i][0] + (double)dv[i - 1][0]) / 2;
                    UnderExc_Vol = (Station[i] - Station[i - 1]) * ((double)dv[i][1] + (double)dv[i - 1][1]) / 2;
                    Exc_Vol = (Station[i] - Station[i - 1]) * ((double)dv[i][2] + (double)dv[i - 1][2]) / 2;
                    SumExc_Vol = SumExc_Vol + Exc_Vol;
                    SumOverEx_Vol = SumOverEx_Vol + OverEx_Vol;
                    SumUnderExc_Vol = SumUnderExc_Vol + UnderExc_Vol;
                    OverEx_Vol_P = (Station[i + 1] - Station[i - 1]) * ((double)dv[i + 1][0] + (double)dv[i - 1][0] + 4 * (double)dv[i][0]) / 6;
                    UnderExc_Vol_P = (Station[i + 1] - Station[i - 1]) * ((double)dv[i + 1][1] + (double)dv[i - 1][1] + 4 * (double)dv[i][1]) / 6;
                    Exc_Vol_P = (Station[i + 1] - Station[i - 1]) * ((double)dv[i + 1][2] + (double)dv[i - 1][2] + 4 * (double)dv[i][2]) / 6;
                    SumExc_Vol_P = SumExc_Vol_P + Exc_Vol_P;
                    SumOverEx_Vol_P = SumOverEx_Vol_P + OverEx_Vol_P;
                    SumUnderExc_Vol_P = SumUnderExc_Vol_P + UnderExc_Vol_P;
                }
                else if(i==dv.Count-1)
                {
                    OverEx_Vol_P = (Station[i] - Station[i - 1]) * ((double)dv[i - 1][0] + 4 * (double)dv[i][0]) / 6;
                    UnderExc_Vol_P = (Station[i] - Station[i - 1]) * ((double)dv[i - 1][1] + 4 * (double)dv[i][1]) / 6;
                    Exc_Vol_P = (Station[i] - Station[i - 1]) * ((double)dv[i - 1][2] + 4 * (double)dv[i][2]) / 6;
                    SumExc_Vol_P = SumExc_Vol_P + Exc_Vol_P;
                    SumOverEx_Vol_P = SumOverEx_Vol_P + OverEx_Vol_P;
                    SumUnderExc_Vol_P = SumUnderExc_Vol_P + UnderExc_Vol_P;
                }
                OverEx = (double)dv[i][0];
                UnderExc = (double)dv[i][1];
                Exc = (double)dv[i][2];                
                Autodesk.AutoCAD.DatabaseServices.Table tb = new Autodesk.AutoCAD.DatabaseServices.Table();
                tb.TableStyle = tsId;
                tb.SetSize(6, 7);
                tb.SetRowHeight(1.3);
                tb.SetColumnWidth(2.052);
                tb.Position = pos[i];
                tb.SetTextHeight(.2, (int)(RowType.DataRow | RowType.HeaderRow | RowType.TitleRow));               
                double staform = Station[i] - Math.Floor(Station[i] / 1000) * 1000;
                string staform2 = "";
                if (staform < 10)
                    staform2 = "00" + staform.ToString("F3");
                else if (staform < 100)
                    staform2 = "0" + staform.ToString("F3");
                else
                    staform2 = staform.ToString();
                if (i == 0) sta0 = Convert.ToString(Math.Floor(Station[i] / 1000)) + "+" + staform2;
                tb.Cells[0, 0].TextString = "Total Volume at Station : " + Convert.ToString(Math.Floor(Station[i] / 1000)) + "+" + staform2;
                tb.Cells[2, 0].TextString = "Average";
                tb.Cells[3, 0].TextString = "Total Volume from Station " + sta0 + " to Station " + Convert.ToString(Math.Floor(Station[i] / 1000)) + "+" + staform2;
                tb.Cells[4, 0].TextString = "Prismoidal";
                tb.Cells[5, 0].TextString = "Total Volume from Station " + sta0 + " to Station " + Convert.ToString(Math.Floor(Station[i] / 1000)) + "+" + staform2;
                tb.Cells[0, 1].TextString = Station[i].ToString();
                tb.Cells[1, 0].TextString = "Method";
                tb.Cells[1, 1].TextString = "SobrEX Area";
                tb.Cells[1, 2].TextString = "UnderEX Area";
                tb.Cells[1, 3].TextString = "Exc Area";
                tb.Cells[1, 4].TextString = "OverEX Vol";
                tb.Cells[1, 5].TextString = "UnderEX Vol";
                tb.Cells[1, 6].TextString = "Exc Vol";
                tb.Cells[2, 1].TextString = OverEx.ToString("F3");
                tb.Cells[2, 2].TextString = UnderExc.ToString("F3");
                tb.Cells[2, 3].TextString = Exc.ToString("F3");
                tb.Cells[2, 4].TextString = OverEx_Vol.ToString("F3");
                tb.Cells[2, 5].TextString = UnderExc_Vol.ToString("F3");
                tb.Cells[2, 6].TextString = Exc_Vol.ToString("F3");
                tb.Cells[3, 4].TextString = SumOverEx_Vol.ToString("F3");
                tb.Cells[3, 5].TextString = SumUnderExc_Vol.ToString("F3");
                tb.Cells[3, 6].TextString = SumExc_Vol.ToString("F3");
                tb.Cells[5, 4].TextString = SumOverEx_Vol_P.ToString("F3");
                tb.Cells[5, 5].TextString = SumUnderExc_Vol_P.ToString("F3");
                tb.Cells[5, 6].TextString = SumExc_Vol_P.ToString("F3");
                tb.MergeCells(CellRange.Create(tb, 3, 0, 3, 3));
                tb.MergeCells(CellRange.Create(tb, 5, 0, 5, 3));
                tb.MergeCells(CellRange.Create(tb, 4, 1, 4, 3));
                tb.Cells[0, 0].Alignment = CellAlignment.MiddleCenter;
                tb.Cells[1, 0].Alignment = CellAlignment.MiddleCenter;
                tb.Cells[2, 0].Alignment = CellAlignment.MiddleCenter;
                tb.Cells[3, 0].Alignment = CellAlignment.MiddleCenter;
                tb.Cells[4, 0].Alignment = CellAlignment.MiddleCenter;
                tb.Cells[0, 1].Alignment = CellAlignment.MiddleCenter;
                tb.Cells[1, 1].Alignment = CellAlignment.MiddleCenter;
                tb.Cells[2, 1].Alignment = CellAlignment.MiddleCenter;
                tb.Cells[3, 1].Alignment = CellAlignment.MiddleCenter;
                tb.Cells[4, 1].Alignment = CellAlignment.MiddleCenter;
                tb.Rows[0].ContentColor = Color.FromColorIndex(ColorMethod.ByAci, 140);
                tb.Rows[1].ContentColor = Color.FromColorIndex(ColorMethod.ByAci, 140);
                tb.Rows[2].ContentColor = Color.FromColorIndex(ColorMethod.ByAci, 140);
                tb.Rows[3].ContentColor = Color.FromColorIndex(ColorMethod.ByAci, 140);
                tb.Rows[4].ContentColor = Color.FromColorIndex(ColorMethod.ByAci, 140);
                tb.Rows[5].ContentColor = Color.FromColorIndex(ColorMethod.ByAci, 140);              
                tb.GenerateLayout();
                btr.AppendEntity(tb);
                trans.AddNewlyCreatedDBObject(tb, true);
            }            
        }

        public void Sort_Points(ref DataView dvFinal, ref DataView dv, ref List<double> Station,ref DataView dvTunnel)
        {
            try
            {
                Profile prof = null;
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    prof = trans.GetObject(align2.GetProfileIds()[LS_Prof.SelectedIndices[0]], OpenMode.ForRead) as Profile;
                    slgsta = new List<double>();
                    foreach (ObjectId id in align2.GetSampleLineGroupIds())
                    {
                        SampleLineGroup slg22 = trans.GetObject(id, OpenMode.ForWrite) as SampleLineGroup;
                        if (LS_SLG.SelectedItem.ToString() == slg22.Name)
                        {
                            slg = slg22;
                            foreach (ObjectId osamID in slg.GetSampleLineIds())
                            {
                                SampleLine osam = trans.GetObject(osamID, OpenMode.ForWrite) as SampleLine;
                                slgsta.Add(osam.Station);
                            }
                            break;
                        }
                    }
                    PromptSelectionResult se1 = ed.GetSelection();
                    if (se1.Status == PromptStatus.Error) return;
                    SelectionSet Set1 = se1.Value;
                    ObjectId[] ids1 = Set1.GetObjectIds();
                    polysample = trans.GetObject(ids1[0], OpenMode.ForRead) as Polyline;
                }
                System.Data.DataTable table = new System.Data.DataTable();
                table.Columns.Add("X", typeof(double));
                table.Columns.Add("Y", typeof(double));
                table.Columns.Add("Z", typeof(double));
                table.Columns.Add("STA", typeof(double));
                table.Columns.Add("OFF", typeof(double));
                double sta = 0;
                double off = 0;
                double x = 0;
                double y = 0;
                double z = 0;
                double slope = 0;
                double tol = 0;
                List<double> df = new List<double>();
                List<double> tt2 = new List<double>();
                List<double> tt3 = new List<double>();
                DataView dv3 = null;
                if (chkformat.Checked == true)
                {
                    slgsta.Clear();
                    chkConvex.Enabled = false;
                    for (int i = 0; i < TextFile.Length - 5; i = i + 5)
                    {
                        x = Convert.ToDouble(TextFile[i + 1]);
                        y = Convert.ToDouble(TextFile[i + 2]);
                        z = Convert.ToDouble(TextFile[i + 3]);
                        ii = TextFile[i + 0];
                        align2.StationOffset(x, y, ref sta, ref off);
                        sta = Convert.ToDouble(TextFile[i + 4]);
                        tt2.Add(x);
                        tt2.Add(y);
                        tt2.Add(z);
                        tt2.Add(sta);
                        tt2.Add(off);
                        table.Rows.Add(x, y, z, sta, off);
                        slgsta.Add(sta);
                    }
                    Station = slgsta;
                    dvFinal = new DataView(table);
                    dv = dvFinal;
                    Station = slgsta.Distinct().ToList();
                    boundry(dvFinal, Station, ref dvTunnel); //My own Boundry Creation for Tunne Section
                }
                else
                {
                    for (int i = 0; i < TextFile.Length - 4; i = i + 4)
                    {
                        x = Convert.ToDouble(TextFile[i + 1]);
                        y = Convert.ToDouble(TextFile[i + 2]);
                        z = Convert.ToDouble(TextFile[i + 3]);
                        ii = TextFile[i + 0];
                        align2.StationOffset(x, y, ref sta, ref off);
                        //slope = prof.GradeAt(sta);
                        tol = 0; //Math.Sin(Math.Atan(slope)) * (polysample.Bounds.Value.MaxPoint.Y - polysample.Bounds.Value.MinPoint.Y);
                        tt2.Add(x);
                        tt2.Add(y);
                        tt2.Add(z);
                        tt2.Add(Math.Abs(tol - sta));
                        tt2.Add(off);
                        table.Rows.Add(x, y, z, sta, off);
                    }
                    //--------------------------------- 
                    dv = new DataView(table);
                    dv.Sort = "STA ASC";
                    slgsta.Sort();
                    for (int i = 0; i < dv.Count; i++)
                    {
                        sta = (double)dv[i][3];
                        for (int j = 0; j < slgsta.Count; j++)
                        {
                            df.Add(Math.Abs(sta - slgsta[j]));
                        }
                        dv[i].Row.BeginEdit();
                        dv[i][3] = slgsta[df.IndexOf(df.Min())];
                        tt3.Add((double)dv[i][3]);
                        df.Clear();
                    }
                    //---------Create Table from points in format of : x,y,z,STA,Offset---------
                    System.Data.DataTable table2 = new System.Data.DataTable();
                    table2.Columns.Add("X", typeof(double));
                    table2.Columns.Add("Y", typeof(double));
                    table2.Columns.Add("Z", typeof(double));
                    table2.Columns.Add("STA", typeof(double));
                    table2.Columns.Add("OFF", typeof(double));
                    System.Data.DataTable tab2 = new System.Data.DataTable();
                    tab2.Columns.Add("X", typeof(double));
                    tab2.Columns.Add("Y", typeof(double));
                    tab2.Columns.Add("Z", typeof(double));
                    tab2.Columns.Add("STA", typeof(double));
                    tab2.Columns.Add("OFF", typeof(double));
                    DataView dv2;
                    for (int i = 0; i < dv.Count; i++)
                    {
                        x = 0;
                        y = 0;
                        align2.PointLocation((double)(dv[i][3]), (double)(dv[i][4]), ref x, ref y);
                        table2.Rows.Add(x, y, dv[i][2], dv[i][3], dv[i][4]);
                    }
                    dv2 = new DataView(table2);
                    dv2.Sort = "STA ASC";                   
                    dvFinal = dv2;
                    Station = slgsta;
                    dv3 = new DataView(table2);
                    dv3.Sort = "STA ASC";
                    if (chkConvex.Checked == true)
                    {
                        dvTunnel = Convex(dv3, Station);
                    }
                    else
                        boundry(dvFinal, Station, ref dvTunnel); //My own Boundry Creation for Tunne Section
                }                
            }
            catch (System.Exception ex)
            {
                var st = new System.Diagnostics.StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                ed.WriteMessage("\n" + ex.Message + "\n Line Number" + line + "\n You must remove or edit Point N.O:" + ii);
            }
        }

        public void Sort_PointsBYSTA(ref DataView dvFinal, ref DataView dv, ref List<double> Station, ref DataView dvTunnel)
        {
            try
            {
                Profile prof = null;
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    prof = trans.GetObject(align2.GetProfileIds()[LS_Prof.SelectedIndices[0]], OpenMode.ForRead) as Profile;
                    slgsta = new List<double>();
                    foreach (ObjectId id in align2.GetSampleLineGroupIds())
                    {
                        SampleLineGroup slg22 = trans.GetObject(id, OpenMode.ForWrite) as SampleLineGroup;
                        if (LS_SLG.SelectedItem.ToString() == slg22.Name)
                        {
                            slg = slg22;
                            foreach (ObjectId osamID in slg.GetSampleLineIds())
                            {
                                SampleLine osam = trans.GetObject(osamID, OpenMode.ForWrite) as SampleLine;
                                slgsta.Add(osam.Station);
                            }
                            break;
                        }
                    }
                    PromptSelectionResult se1 = ed.GetSelection();
                    if (se1.Status == PromptStatus.Error) return;
                    SelectionSet Set1 = se1.Value;
                    ObjectId[] ids1 = Set1.GetObjectIds();
                    polysample = trans.GetObject(ids1[0], OpenMode.ForRead) as Polyline;
                }
                double sta = 0;
                double off = 0;
                double x = 0;
                double y = 0;
                double z = 0;
                double x1 = 0;
                double y1 = 0;
                double x2 = 0;
                double y2 = 0;
                double slope = 0;
                double tol = 0;
                double d1 = 0;
                double d2 = 0;
                double m = 0;
                int s = 0;
                double dx1 = 0;
                double dy1 = 0;
                List<double> df = new List<double>();
                List<double> tt2 = new List<double>();
                List<int> index = new List<int>();
                List<double> sta1 = new List<double>();
                List<double> sta2 = new List<double>();
                List<double> sta3 = new List<double>();
                index.Add(0);
                System.Data.DataTable tb = new System.Data.DataTable();
                tb.Columns.Add("X", typeof(double));
                tb.Columns.Add("Y", typeof(double));
                tb.Columns.Add("Z", typeof(double));
                tb.Columns.Add("STA", typeof(double));
                tb.Columns.Add("OFF", typeof(double));
                for (int i = 0; i < TextFile.Length - 4; i = i + 4)
                {
                    double x0 = Convert.ToDouble(TextFile[i + 1]);
                    double y0 = Convert.ToDouble(TextFile[i + 2]);
                    double z0 = Convert.ToDouble(TextFile[i + 3]);
                    align2.StationOffset(x0, y0, ref sta, ref off);
                    m = Math.Atan(prof.GradeAt(sta));
                    if (z0 < prof.ElevationAt(sta))
                    {
                        x1 = sta + Math.Abs((prof.ElevationAt(sta) - z0) * Math.Sin(m) * Math.Cos(m));
                    }
                    else
                    {
                        x1 = sta - Math.Abs((prof.ElevationAt(sta) - z0) * Math.Sin(m) * Math.Cos(m));
                    }
                    sta1.Add(x1);
                    tb.Rows.Add(x0, y0, z0, x1, off);
                }
                DataView dv1 = new DataView(tb);
                dv1.Sort = "STA ASC";
                sta1.Sort();
                s = 0;
                m = 0;
                int indx = 0;
                int ind = 0;
                double m1 = 0;
                for (int i = 0; i < dv1.Count; i++)
                {
                    m1 = sta1[ind];                    
                    df.Add(Math.Abs(sta1[i] - m1));
                    if (Math.Abs(sta1[i]-m1)<= Convert.ToDouble(Tolbox.Text)&i<dv1.Count-1)
                    {                        
                        m = m + sta1[i];
                        s++;
                        indx++;
                    }
                    else
                    {
                        x1 = m / s;
                        if (m == 0 & s == 0) continue;
                        for(int j=0; j<s+1;j++)
                        {
                            sta2.Add(x1);
                        }
                        ind = df.Count;
                        m = 0;
                        s = 0;
                    }
                }
                m = 0;
                s = 0;
                df.Clear();
                for (int i = 1; i < sta2.Count-1; i++)
                {
                    m = sta2[s];
                    df.Add(Math.Abs(sta2[i] - m));
                    if (Math.Abs(sta2[i] - m) <= Convert.ToDouble(Tolbox.Text)& Math.Abs(sta2[i] - m) !=0)
                    {
                        sta3.Add(m);                        
                    }
                    else
                    {
                        sta3.Add(sta2[i]);
                        s++;
                    }                    
                }
                sta3.Add(sta2[sta2.Count-1]);
                sta3.Add(sta2[sta2.Count - 1]);
                for (int i = 0; i < sta3.Count; i++)
                {
                    dv1[i].Row.BeginEdit();
                    dv1[i][3] = sta3[i];
                }                
                Station = sta2.Distinct().ToList();
                dv = dv1;
                dvFinal = dv1;
                if (chkConvex.Checked == true)
                {
                    dvTunnel = Convex(dv1, Station);
                }
                else
                    boundry(dv1, Station, ref dvTunnel); //My own Boundry Creation for Tunne Section
            }
            catch (System.Exception ex)
            {
                var st = new System.Diagnostics.StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                ed.WriteMessage("\n" + ex.Message + "\n Line Number" + line + "\n You must remove or edit Point N.O:" + ii);
            }
        }

        public DataView Convex(DataView dv3, List<double> Station)
        {
            //----------Convex Hull--------- ---
            System.Data.DataTable tab2 = new System.Data.DataTable();
            tab2.Columns.Add("X", typeof(double));
            tab2.Columns.Add("Y", typeof(double));
            tab2.Columns.Add("Z", typeof(double));
            tab2.Columns.Add("STA", typeof(double));
            tab2.Columns.Add("OFF", typeof(double));
            double sta = 0;
            double x;
            double y;
            List<double> Winpo = new List<double>();
            for (int i = 0; i < Station.Count; i++)
            {
                for (int j = 0; j < dv3.Count; j++)
                {
                    sta = (double)dv3[j][3];
                    if (sta == Station[i])
                    {
                        Winpo.Add((double)dv3[j][2]);
                        Winpo.Add((double)dv3[j][4]);
                    }
                }
                ConvexHull convex = new ConvexHull(PO(Winpo));
                convex.CalcConvexHull();
                System.Windows.Point[] po = convex.GetResultsAsArrayOfPoint();
                for (int k = 0; k < po.Length; k++)
                {
                    x = 0;
                    y = 0;
                    align2.PointLocation(Station[i], po[k].Y, ref x, ref y);
                    tab2.Rows.Add(x, y, po[k].X, Station[i], po[k].Y);
                }
                Winpo.Clear();
            }
            DataView dvTunnel = new DataView(tab2);
            return dvTunnel;

        }

        public void boundry(DataView dv1, List<double> Station, ref DataView dvsort)
        {
            double sta = 0;
            System.Data.DataTable tab = new System.Data.DataTable();
            tab.Columns.Add("Z", typeof(double));
            tab.Columns.Add("off", typeof(double));
            tab.Columns.Add("id", typeof(double));
            tab.Columns.Add("X", typeof(double));
            tab.Columns.Add("Y", typeof(double));
            System.Data.DataTable tab2 = new System.Data.DataTable();
            tab2.Columns.Add("X", typeof(double));
            tab2.Columns.Add("Y", typeof(double));
            tab2.Columns.Add("Z", typeof(double));
            tab2.Columns.Add("STA", typeof(double));
            tab2.Columns.Add("OFF", typeof(double));
            DataView dv = null;
            int indexsum = 0;
            for (int i = 0; i < Station.Count; i++)
            {
                for (int j = 0; j < dv1.Count; j++)
                {
                    sta = (double)dv1[j][3];
                    if (sta == Station[i])
                    {
                        tab.Rows.Add((double)dv1[j][2], (double)dv1[j][4],0, (double)dv1[j][0], (double)dv1[j][1]);
                    }
                }
                dv = new DataView(tab);               
                double x;
                double y;                
                double xc = 0;
                double yc = 0;
                for (int j = 0; j < dv.Count; j++)
                {
                    xc = xc + (double)dv[j][0];
                    yc = yc + (double)dv[j][1];
                }
                xc = xc / (dv.Count);
                yc = yc / (dv.Count);
                List<double> al = new List<double>();
                for (int j = 0; j < dv.Count; j++)
                {
                    al.Add(Math.Atan2((yc - (double)dv[j][1]), (xc - (double)dv[j][0])));
                    dv[j][2] = Math.Atan2((yc - (double)dv[j][1]), (xc - (double)dv[j][0]));
                }
                dv.Sort = "id ASC";
                List<double> X1 = new List<double>();
                List<double> Y1 = new List<double>();
                for (int j = 0; j < dv.Count; j++)
                {
                    X1.Add((double)dv1[j][2]);
                    Y1.Add((double)dv1[j][4]);
                }
                for (int k = 0; k < dv.Count; k++)
                {
                    x = (double)dv[k][3];
                    y = (double)dv[k][4];                    
                    tab2.Rows.Add(x, y, (double)dv[k][0], Station[i], (double)dv[k][1]);
                }
                tab.Rows.Clear();
            }
            dvsort = new DataView(tab2);
        }     
        
        private void LS_Alg_SelectedIndexChanged(object sender, EventArgs e)
        {
            LS_SLG.Items.Clear();            
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    for (int i = 0; i < civildoc.GetAlignmentIds().Count; i++)
                    {
                        ObjectId alIDs = civildoc.GetAlignmentIds()[i];
                        Alignment Align = trans.GetObject(alIDs, OpenMode.ForRead) as Alignment;
                        if (Align.Name == LS_Alg.Items[i].ToString())
                        {
                            align2 = Align;
                            alignID2 = alIDs;
                        }                                      

                    }
                    SampleLineGroup slg2;
                    foreach (ObjectId slgID in align2.GetSampleLineGroupIds())
                    {
                        slg2 = trans.GetObject(slgID, OpenMode.ForWrite) as SampleLineGroup;
                        LS_SLG.Items.Add(slg2.Name);
                    }
                    LS_SLG.SetSelected(0, true);
                    foreach (ObjectId profID in align2.GetProfileIds())
                    {
                        Profile pr = trans.GetObject(profID, OpenMode.ForRead) as Profile;
                        LS_Prof.Items.Add(pr.Name);
                    }
                    LS_Prof.SetSelected(0, true);
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                }
                trans.Commit();
            }
        }

        public void RegionToPolyline(Region reg,ref DBObjectCollection objs)
        {            
            Transaction tr =  doc.TransactionManager.StartTransaction();            
            using (tr)
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr =  (BlockTableRecord)tr.GetObject( bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                if (reg != null)
                {
                    objs =  PolylineFromRegion(reg);
                    // Append our new entities to the database
                    btr.UpgradeOpen();
                    foreach (Autodesk.AutoCAD.DatabaseServices.Entity ent in objs)
                    {
                        btr.AppendEntity(ent);
                        tr.AddNewlyCreatedDBObject(ent, true);
                    }
                    // Finally we erase the original region
                    reg.UpgradeOpen();
                    reg.Erase();
                }                
                tr.Commit();
            }
        }

        private static double BulgeFromCurve( Curve cv , bool clockwise)
        {
            double bulge = 0.0;
            Arc a = cv as Arc;
            if (a != null)
            {
                double newStart;
                // The start angle is usually greater than the end,
                // as arcs are all counter-clockwise.
                // (If it isn't it's because the arc crosses the
                // 0-degree line, and we can subtract 2PI from the
                // start angle.)
                if (a.StartAngle > a.EndAngle)
                    newStart = a.StartAngle - 8 * Math.Atan(1);
                else
                    newStart = a.StartAngle;
                // Bulge is defined as the tan of
                // one fourth of the included angle
                bulge = Math.Tan((a.EndAngle - newStart) / 4);
                // If the curve is clockwise, we negate the bulge
                if (clockwise)
                    bulge = -bulge;
            }
            return bulge;
        }

        private static DBObjectCollection PolylineFromRegion(Region reg)
        {
            // We will return a collection of entities
            // (should include closed Polylines and other
            // closed curves, such as Circles)
            DBObjectCollection res =  new DBObjectCollection();
            // Explode Region -> collection of Curves / Regions
            DBObjectCollection cvs =  new DBObjectCollection();
            reg.Explode(cvs);
            // Create a plane to convert 3D coords
            // into Region coord system
            Plane pl =  new Plane(new Point3d(0, 0, 0), reg.Normal);
            using (pl)
            {
                bool finished = false;
                while (!finished && cvs.Count > 0)
                {
                    // Count the Curves and the non-Curves, and find
                    // the index of the first Curve in the collection
                    int cvCnt = 0, nonCvCnt = 0, fstCvIdx = -1;
                    for (int i = 0; i < cvs.Count; i++)
                    {
                        Curve tmpCv = cvs[i] as Curve;
                        if (tmpCv == null)
                            nonCvCnt++;
                        else
                        {
                            // Closed curves can go straight into the
                            // results collection, and aren't added
                            // to the Curve count
                            if (tmpCv.Closed)
                            {
                                res.Add(tmpCv);
                                cvs.Remove(tmpCv);
                                // Decrement, so we don't miss an item
                                i--;
                            }
                            else
                            {
                                cvCnt++;
                                if (fstCvIdx == -1)
                                    fstCvIdx = i;
                            }
                        }
                    }                    
                    if (fstCvIdx >= 0)
                    {
                        // For the initial segment take the first
                        // Curve in the collection
                        Curve fstCv = (Curve)cvs[fstCvIdx];
                        // The resulting Polyline
                       Polyline p = new Polyline();
                        // Set common entity properties from the Region
                        p.SetPropertiesFrom(reg);
                        // Add the first two vertices, but only set the
                        // bulge on the first (the second will be set
                        // retroactively from the second segment)
                        // We also assume the first segment is counter-
                        // clockwise (the default for arcs), as we're
                        // not swapping the order of the vertices to
                        // make them fit the Polyline's order
                        p.AddVertexAt( p.NumberOfVertices, fstCv.StartPoint.Convert2d(pl), BulgeFromCurve(fstCv, false), 0, 0);
                        p.AddVertexAt( p.NumberOfVertices, fstCv.EndPoint.Convert2d(pl),  0, 0, 0 );
                        cvs.Remove(fstCv);
                        // The next point to look for
                        Point3d nextPt = fstCv.EndPoint;
                        // Find the line that is connected to
                        // the next point
                        // If for some reason the lines returned were not
                        // connected, we could loop endlessly.
                        // So we store the previous curve count and assume
                        // that if this count has not been decreased by
                        // looping completely through the segments once,
                        // then we should not continue to loop.
                        // Hopefully this will never happen, as the curves
                        // should form a closed loop, but anyway...
                        // Set the previous count as artificially high,
                        // so that we loop once, at least.
                        int prevCnt = cvs.Count + 1;
                        while (cvs.Count > nonCvCnt && cvs.Count < prevCnt)
                        {
                            prevCnt = cvs.Count;
                            foreach (Autodesk.AutoCAD.DatabaseServices.DBObject obj in cvs)
                            {
                                Curve cv = obj as Curve;
                                if (cv != null)
                                {
                                    // If one end of the curve connects with the
                                    // point we're looking for...
                                    if (cv.StartPoint == nextPt || cv.EndPoint == nextPt)
                                    {
                                        // Calculate the bulge for the curve and
                                        // set it on the previous vertex
                                        double bulge =  BulgeFromCurve(cv, cv.EndPoint == nextPt);
                                        if (bulge != 0.0)
                                            p.SetBulgeAt(p.NumberOfVertices - 1, bulge);
                                        // Reverse the points, if needed
                                        if (cv.StartPoint == nextPt)
                                            nextPt = cv.EndPoint;
                                        else
                                            // cv.EndPoint == nextPt
                                            nextPt = cv.StartPoint;
                                        // Add out new vertex (bulge will be set next
                                        // time through, as needed)
                                        p.AddVertexAt( p.NumberOfVertices, nextPt.Convert2d(pl),  0, 0, 0 );
                                        // Remove our curve from the list, which
                                        // decrements the count, of course
                                        cvs.Remove(cv);
                                        break;
                                    }
                                }
                            }
                        }
                        // Once we have added all the Polyline's vertices,
                        // transform it to the original region's plane
                        p.TransformBy(Matrix3d.PlaneToWorld(pl));
                        res.Add(p);
                        if (cvs.Count == nonCvCnt)
                            finished = true;
                    }
                    // If there are any Regions in the collection,
                    // recurse to explode and add their geometry
                    if (nonCvCnt > 0 && cvs.Count > 0)
                    {

                        foreach (Autodesk.AutoCAD.DatabaseServices.DBObject obj in cvs)
                        {
                            Region subReg = obj as Region;
                            if (subReg != null)
                            {                                
                                DBObjectCollection subRes =  PolylineFromRegion(subReg);
                                foreach (Autodesk.AutoCAD.DatabaseServices.DBObject o in subRes)
                                    res.Add(o);
                                cvs.Remove(subReg);
                            }
                        }
                    }
                    if (cvs.Count == 0)
                        finished = true;
                }
            }
            return res;
        }

        public void BoundryPolygone(double elev, Polyline pol2d, Polyline polysample2, ref Polyline BoundPoly)
        {
            BoundPoly = new Polyline();
            int v = 0;
            Point2dCollection PointCol = new Point2dCollection();
            double max_X = pol2d.Bounds.Value.MaxPoint.X + 2;
            double max_Y = pol2d.Bounds.Value.MaxPoint.Y + 2;
            double min_X = pol2d.Bounds.Value.MinPoint.X - 2;
            double min_Y = pol2d.Bounds.Value.MinPoint.Y - 2;
            if (polysample2.Bounds.Value.MaxPoint.X > max_X) max_X = polysample2.Bounds.Value.MaxPoint.X + 2;
            if (polysample2.Bounds.Value.MaxPoint.Y > max_Y) max_Y = polysample2.Bounds.Value.MaxPoint.Y + 2;
            if (polysample2.Bounds.Value.MinPoint.X < min_X) min_X = polysample2.Bounds.Value.MinPoint.X - 2;
            if (polysample2.Bounds.Value.MinPoint.Y < min_Y) min_Y = polysample2.Bounds.Value.MinPoint.Y - 2;
            double ave_X = (polysample2.Bounds.Value.MaxPoint.X + polysample2.Bounds.Value.MinPoint.X) / 2;
            double ave_Y = polysample2.StartPoint.Y - elev + Math.Floor(elev);
            double datumZ =  Math.Floor(elev);
            double LxR = max_X - ave_X;
            double LxL = ave_X - min_X;
            //Lx = Math.Floor(Lx);
            double LyU = max_Y - ave_Y;
            double LyD = ave_Y - min_Y;
            //Ly = Math.Floor(Ly);
            double LxR_Count = Math.Floor(LxR / 1);
            double LxL_Count = Math.Floor(LxL / 1);
            double LxR_res = LxR - LxR_Count;
            double LxL_res = LxL - LxL_Count;
            System.Data.DataTable tabx = new System.Data.DataTable();
            tabx.Columns.Add("X", typeof(double));
            tabx.Columns.Add("Y", typeof(double));
            //tabx.Rows.Add(max_X , min_Y );
            //tabx.Rows.Add(max_X- LxR_res, min_Y);
            List<double> cord = new List<double>();
            for (int i=0;i<LxR_Count;i++)
            {
                if (ave_X + 2 * i > max_X) continue;
                //tabx.Rows.Add(ave_X + 2 * i, min_Y);
                tabx.Rows.Add(ave_X + 2 * i, min_Y  + .1);
                //tabx.Rows.Add(ave_X + 2 * i, min_Y );
                //cord.Add(ave_X + 2 * i);
                //cord.Add(min_Y);
                cord.Add(ave_X + 2 * i);
                cord.Add(min_Y + .1);
                //cord.Add(ave_X + 2 * i);
                //cord.Add(min_Y);
            }
            for (int i = 0; i < LxL_Count; i++)
            {
                if (ave_X - 2 * i < min_X) continue;
                //tabx.Rows.Add(ave_X - 2 * i, min_Y);
                tabx.Rows.Add(ave_X - 2 * i, min_Y + .1);
                //tabx.Rows.Add(ave_X - 2 * i, min_Y );
                //cord.Add(ave_X - 2 * i);
                //cord.Add(min_Y);
                cord.Add(ave_X - 2 * i);
                cord.Add(min_Y + .1);
                //cord.Add(ave_X - 2 * i);
                //cord.Add(min_Y);
            }
            DataView dvX = new DataView(tabx);
            dvX.Sort = "X DESC, Y ASC";
            double LyU_Count = Math.Floor(LyU / 1);
            double LyD_Count = Math.Floor(LyD / 1);
            System.Data.DataTable taby = new System.Data.DataTable();
            taby.Columns.Add("X", typeof(double));
            taby.Columns.Add("Y", typeof(double));
            //taby.Rows.Add(min_X , min_Y );
            cord.Clear();
            for (int i = 0; i < LyU_Count; i++)
            {
                if (ave_Y + 2 * i > max_Y) continue;
                //taby.Rows.Add(min_X, ave_Y + 2 * i);
                taby.Rows.Add(min_X + .1, ave_Y + 2 * i);
                cord.Add(min_X + .1);
                cord.Add(ave_Y + 2 * i);
                //taby.Rows.Add(min_X , ave_Y + 2 * i);
            }
            for (int i = 0; i < LyD_Count; i++)
            {
                if (ave_Y - 2 * i < min_Y) continue;
                //taby.Rows.Add(min_X, ave_Y - 2 * i);
                taby.Rows.Add(min_X + .1, ave_Y - 2 * i);
                cord.Add(min_X + .1);
                cord.Add(ave_Y - 2 * i);
                //taby.Rows.Add(min_X , ave_Y - 2 * i);
            }
            DataView dvY = new DataView(taby);
            dvY.Sort = "Y ASC, X DESC";
            for (int i = 0; i < dvX.Count; i++)
            {
                Point2d po = new Point2d((double)dvX[i][0], (double)dvX[i][1]);
                PointCol.Add(po);
            }
            for (int i = 0; i <  dvY.Count; i++)
            {
                Point2d po = new Point2d((double)dvY[i][0], (double)dvY[i][1]);
                PointCol.Add(po);
            }
            Point2dCollection POCollBound = new Point2dCollection();
            POCollBound.Add(new Point2d(max_X , max_Y ));
            POCollBound.Add(new Point2d(max_X , min_Y ));
            POCollBound.Add(new Point2d(min_X , min_Y ));
            POCollBound.Add(new Point2d(min_X , max_Y ));
            for (int j = 0; j < POCollBound.Count; j++)
            {
                BoundPoly.AddVertexAt(v, POCollBound[j], 0, 0, 0);
                v++;
            }
            BoundPoly.Closed = true;
            BoundPoly.ColorIndex = 3;
            double temp = 0;
            double temp2 = 0;
            for (int i = 0; i < PointCol.Count-1; i++)
            {
                temp2 = Math.Abs(PointCol[i].X - PointCol[i+1].X) + Math.Abs(PointCol[i].Y - PointCol[i+1].Y);
                if (temp2 == 0)
                    PointCol.RemoveAt(i);
                if (Math.Abs(PointCol[i].Y - PointCol[i + 1].Y) == 0)
                    temp++;
            }
            for (int i = 0; i < PointCol.Count; i++)
            {
                double dx = 0;
                double dy = 0;
                if (i <= temp)
                    dy = -0.1;
                else
                    dx = -0.1;
                Polyline pol = new Polyline();
                pol.AddVertexAt(0, PointCol[i], 0, 0, 0);
                Point2d po = new Point2d(PointCol[i].X + dx, PointCol[i].Y + dy);
                pol.AddVertexAt(1, po, 0, 0, 0);
                pol.ColorIndex = 3;
                MText acMText = new MText();
                acMText.Location = new Point3d(PointCol[i].X + dx - .1, PointCol[i].Y - .1 + dy, 0);
                acMText.Height = .1;
                double textX = PointCol[i].X - ave_X;
                double textY = PointCol[i].Y - ave_Y + datumZ;
                if (dx==0)
                    acMText.Contents = textX.ToString("F1");
                if(dy==0)
                {
                    acMText.Contents = textY.ToString("F0");
                    acMText.Location = new Point3d(PointCol[i].X + dx - .8, PointCol[i].Y , 0);
                }
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                    btr.AppendEntity(pol);
                    trans.AddNewlyCreatedDBObject(pol, true);
                    btr.AppendEntity(acMText);
                    trans.AddNewlyCreatedDBObject(acMText, true);
                    trans.Commit();
                }
            }
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                btr.AppendEntity(BoundPoly);
                trans.AddNewlyCreatedDBObject(BoundPoly, true);
                trans.Commit();               
            }
        }

        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DataView dvFinal = null;
            DataView dv = null;
            DataView dvTunnel = null;
            List<double> Station = null;
            Sort_Points(ref dvFinal, ref dv, ref Station, ref dvTunnel);
            dvFinal.Sort = "OFF ASC";
            List<string> txt = new List<string>();
            string ErrSLG = "";
            string header;
            int sum = 1;
            double x = 0;
            double y = 0;
            double z = 0;
            double sta;
            for (int i = 0; i < Station.Count; i++)
            {
                if (Station[i] > (double)dv[dv.Count - 1][3])
                    continue;
                align2.PointLocation(Station[i], 0, ref x, ref y);
                ii = Station[i].ToString();
                try
                {
                    z = surface.FindElevationAtXY(x, y);
                }
                catch
                {
                    ErrSLG = ErrSLG + "\n Remove STA = " + ii + " sample line";
                }
            }
            try
            {
                for (int i = 0; i < Station.Count; i++)
                {
                    if (Station[i] > (double)dv[dv.Count - 1][3]) continue;
                    align2.PointLocation(Station[i], 0, ref x, ref y);
                    ii = Station[i].ToString();
                    z = surface.FindElevationAtXY(x, y);
                    sta = Station[i];
                    string side = "";
                    string staform2 = "";
                    double staform = sta - Math.Floor(sta / 1000) * 1000;
                    if (staform < 100)
                        staform2 = "0" + staform.ToString();
                    else
                        staform2 = staform.ToString();
                    header = Convert.ToString(sum) + "," + Convert.ToString(x.ToString("F3")) + "," + Convert.ToString(y.ToString("F3")) + "," + Convert.ToString(z.ToString("F3")) + "," + Convert.ToString(Math.Floor(sta / 1000)) + "+" + staform2;
                    txt.Add(Convert.ToString(header));
                    for (int j = 0; j < dvFinal.Count; j++)
                    {
                        if ((double)(dvFinal[j][3]) == Station[i])
                        {
                            if ((double)(dvFinal[j][4]) < 0)
                                side = "L";
                            else if ((double)(dvFinal[j][4]) > 0)
                                side = "R";
                            sum++;
                            x = (double)(dvFinal[j][0]);
                            y = (double)(dvFinal[j][1]);
                            z = (double)(dvFinal[j][2]);
                            string OFFELEV = Convert.ToString(sum) + "," + x.ToString("F3") + "," + y.ToString("F3") + "," + z.ToString("F3") + "," + side;
                            txt.Add(OFFELEV);
                        }
                    }
                }
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
                        for (int i = 0; i < txt.Count; i++)
                        {
                            filewriter.WriteLine(txt[i]);
                        }
                        filewriter.Close();
                    }
                    catch (IOException)
                    {
                        System.Windows.Forms.MessageBox.Show("Error writing to file", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("You must remove sample lines which are outside of surface boundries!" + "\n" + ErrSLG, "Outside sample lines", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }    
        
        private void About_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sections.AboutCreateSectionFromXYZ win1 = new Sections.AboutCreateSectionFromXYZ();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win1);
        }

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SectionFromFile_TxtBox.Text = "";
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
                        TextFile = whole_file.Split(new char[] { '\r', ',' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < 100; i++)
                        {
                            SectionFromFile_TxtBox.Text += "\r\n" + TextFile[i].ToString();
                        }
                    }
                    catch (IOException)
                    {
                        System.Windows.Forms.MessageBox.Show("Error reading from file", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    filereader.Close();
                }
            }
            catch (System.Exception ex)
            {
                var st = new System.Diagnostics.StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                ed.WriteMessage("\n" + ex.Message + "\n Line Number" + line);
            }
        }

        public System.Windows.Point[] PO(List<double> points)
        {
            var Po = new System.Windows.Point[points.Count() / 2];
            int s = 0;
            for (int j = 0; j < points.Count(); j = j + 2)
            {
                Po[s] = new System.Windows.Point(points[j], points[j + 1]);
                s++;
            }
            return Po;
        }  
        
        public bool isinnside(Polyline pol,Point3d Point1,Point3d Point2,double rot)
        {
            Point2d CentPoint = new Point2d((pol.Bounds.Value.MaxPoint.X + pol.Bounds.Value.MinPoint.X) / 2,
                (pol.Bounds.Value.MaxPoint.Y + pol.Bounds.Value.MinPoint.Y) / 2);
            bool t = false;
            int quad = 1;
            if (CentPoint.X <= Point1.X & CentPoint.Y >= Point1.Y)
                quad = 2;
            else if (CentPoint.X >= Point1.X & CentPoint.Y >= Point1.Y)
                quad = 3;
            else if (CentPoint.X >= Point1.X & CentPoint.Y <= Point1.Y)
                quad = 4;
            if (quad == 1 & Point1.X <= Point2.X & Point1.Y <= Point2.Y)
                t = true;
            else if (quad == 2 & Point1.X <= Point2.X & Point1.Y >= Point2.Y)
                t = true;
            else if (quad == 3 & Point1.X >= Point2.X & Point1.Y >= Point2.Y)
                t = true;
            else if (quad == 4 & Point1.X >= Point2.X & Point1.Y <= Point2.Y)
                t = true;
            return t;
        }

        private void chksampleline_CheckedChanged(object sender, EventArgs e)
        {
            if(chksampleline.CheckState == CheckState.Checked)
            {
                chkformat.Enabled = false;
                Tolbox.Enabled = false;
                chkwhitoutslg.Enabled = false;
            }
            else
            {
                chkformat.Enabled = true;
                Tolbox.Enabled = true;
                chkwhitoutslg.Enabled = true;
            }
        }

        private void chkwhitoutslg_CheckedChanged(object sender, EventArgs e)
        {
            if (chkwhitoutslg.CheckState == CheckState.Checked)
            {
                chkformat.Enabled = false;
                chksampleline.Enabled = false;
                chkformat.Checked = false;
                chksampleline.Checked = false;
                chkSurface.Checked = false;
                chkSurface.Enabled = false;
                chkConvex.Checked = false;
                chkConvex.Enabled = false;
            }
            else
            {
                chkformat.Enabled = true;
                chksampleline.Enabled = true;
                chkSurface.Enabled = true;
                chkConvex.Enabled = true;
            }
        }

        private void chkformat_CheckedChanged(object sender, EventArgs e)
        {
            if (chkformat.CheckState == CheckState.Checked)
            {
                chkwhitoutslg.Enabled = false;
                chksampleline.Enabled = false;
                Tolbox.Enabled = false;
            }
            else
            {
                chkwhitoutslg.Enabled = true;
                chksampleline.Enabled = true;
                Tolbox.Enabled = true;
            }
        }
    }
}