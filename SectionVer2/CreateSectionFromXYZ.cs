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

namespace Sections
{
    public partial class CreateSectionFromXYZ : Form
    {
        Stopwatch elaptime = new Stopwatch();
        private StreamReader filereader;
        private StreamWriter filewriter;
        public DataView dvFinal;
        public string[] TextFile;
        public string[,] Left2;
        public string[,] Left3;
        public List<double> slgsta;
        public Alignment align2;
        public SampleLineGroup slg;
        public ObjectId alignID2;
        public double[] minoff2;
        public double[] maxoff2;
        private MenuStrip menuStrip1;
        public TinSurface surface;        
        public double[] Station2;
        public CreateSectionFromXYZ()
        {
            InitializeComponent();            
            LS_SLG.Items.Clear();
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            CivilDocument civildoc = CivilApplication.ActiveDocument;
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
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
                    List<string> alno = new List<string>();
                    Alignment align = null;
                    for (int i = 0; i < coalid; i++)
                    {
                        ObjectId alignID = alIDs[i];
                        align = trans.GetObject(alignID, OpenMode.ForRead) as Alignment;
                        alno.Add(align.Name);
                        comboBox1.Items.Add(align.Name);
                    }
                    align2 = align;
                    comboBox1.SelectedIndex = 0;
                    LS_SLG.SetSelected(0, true);
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                }
                trans.Commit();
            }
        }
        private void Select_Alignment_BTN_Click(object sender, EventArgs e)
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            CivilDocument civildoc = CivilApplication.ActiveDocument;
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    PromptEntityOptions opt = new PromptEntityOptions("\nSelect an Alignment");
                    opt.SetRejectMessage("\nObject must be an alignment.");
                    opt.AddAllowedClass(typeof(Alignment), false);
                    ObjectId alignID = ed.GetEntity(opt).ObjectId;
                    Alignment align = trans.GetObject(alignID, OpenMode.ForRead) as Alignment;
                    foreach (ObjectId id in align.GetSampleLineGroupIds())
                    {
                        SampleLineGroup slg2 = trans.GetObject(id, OpenMode.ForWrite) as SampleLineGroup;
                        if (LS_SLG.SelectedItem.ToString() == slg2.Name)
                        {
                            slg = slg2;
                            break;
                        }
                    }

                    align2 = align;
                    alignID2 = alignID;
                    
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add(align.Name);
                    comboBox1.SelectedIndex = 0;
                    Alignment align3 = null;
                    ObjectIdCollection alIDs = civildoc.GetAlignmentIds();
                    foreach (ObjectId alignID2 in alIDs)
                    {

                        align3 = trans.GetObject(alignID2, OpenMode.ForRead) as Alignment;
                        if (align3.Name == align.Name) continue;
                        comboBox1.Items.Add(align.Name);
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage(ex.Message);
                }
                trans.Commit();
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LS_SLG.Items.Clear();
            int index = comboBox1.SelectedIndex;
            string alname = comboBox1.SelectedItem.ToString();
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            CivilDocument civildoc = CivilApplication.ActiveDocument;
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    for (int i = 0; i < civildoc.GetAlignmentIds().Count; i++)
                    {
                        ObjectId alIDs = civildoc.GetAlignmentIds()[index];
                        Alignment Align = trans.GetObject(alIDs, OpenMode.ForRead) as Alignment;
                        if (Align.Name == alname)
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

                    //comboBox1.Select(index, 1);
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                }
                trans.Commit();
            }
        }              
        public void CreateSurf(DataView dvFinal, List<double> Station, CivilDocument civildoc, Database db)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                double maxOff = 0;
                double x = 0;
                double y = 0;
                //----------تولید گروه نقاط--------------
                CogoPointCollection cog = civildoc.CogoPoints;
                Point3dCollection po3dcol = new Point3dCollection();
                ObjectId postyle = civildoc.Styles.PointStyles[0];
                ObjectId groupId = civildoc.PointGroups.Add(align2.Name.ToString() + DateTime.Today.ToShortDateString() + DateTime.Now.ToShortTimeString());              
                PointGroup group = groupId.GetObject(OpenMode.ForWrite) as PointGroup;
                Point3dCollection po3d2 = new Point3dCollection();
                ObjectIdCollection pol3dobjcol = new ObjectIdCollection();                
                string Desc = "";                
                for (int i = 0; i < dvFinal.Count; i++)
                {
                    align2.PointLocation((double)(dvFinal[i][3]), (double)(dvFinal[i][4]), ref x, ref y);
                    
                    Point3d po3d = new Point3d(x, y, (double)(dvFinal[i][2]));
                    if (Math.Abs((double)(dvFinal[i][4])) <= 0.0001)
                        Desc = "D = " + dvFinal[i][5].ToString(); //"STA = " + dvFinal[i][3].ToString();
                    else
                        Desc = "D = " + dvFinal[i][5].ToString(); //"STA = " + dvFinal[i][3].ToString() + " Off= " + dvFinal[i][4].ToString();
                    ObjectId pointIds = cog.Add(po3d, Desc, true);
                    if (maxOff < Math.Abs((double)(dvFinal[i][4]))& Math.Abs((double)(dvFinal[i][4]))<100)
                        maxOff = Math.Abs((double)(dvFinal[i][4]));
                    //if ((double)(dvFinal[i][2]) == 1264.731)
                    //    continue;
                }
                dvFinal.Sort = "OFF ASC";                
                for (int i = 0; i < Station.Count; i++)
                {
                    for (int j = 0; j < dvFinal.Count; j++)
                    {
                        if ((double)(dvFinal[j][3]) == Station[i])
                        {
                            align2.PointLocation((double)(dvFinal[j][3]), (double)(dvFinal[j][4]), ref x, ref y);
                            Point3d po3d = new Point3d(x, y, (double)(dvFinal[j][2]));
                            po3d2.Add(po3d);
                        }
                    }                 
                    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                    Autodesk.AutoCAD.DatabaseServices.Polyline3d pol3d = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();
                    btr.AppendEntity(pol3d);
                    trans.AddNewlyCreatedDBObject(pol3d, true);
                    pol3d.SetDatabaseDefaults();
                    foreach (Point3d po in po3d2)
                    {
                        PolylineVertex3d vex3d = new PolylineVertex3d(po);
                        pol3d.AppendVertex(vex3d);
                        trans.AddNewlyCreatedDBObject(vex3d, true);
                    }
                    pol3d.Closed = false;
                    pol3dobjcol.Add(pol3d.ObjectId);
                    po3d2.Clear();
                }
                StandardPointGroupQuery query = new StandardPointGroupQuery();
                query.IncludeRawDescriptions = "D = *";
                PointGroup groupPO = groupId.GetObject(OpenMode.ForWrite) as PointGroup;
                groupPO.SetQuery(query);
                Autodesk.Civil.Settings.SettingsPoint pointSettings = civildoc.Settings.GetSettings<Autodesk.Civil.Settings.SettingsPoint>() as Autodesk.Civil.Settings.SettingsPoint;
                ObjectId surfaceStyleId = civildoc.Styles.SurfaceStyles[0];
                ObjectId surfaceId = surfaceStyleId;
                if ( chksurf.CheckState == CheckState.Checked )
                {
                    createsurf(dvFinal, Station, db, pol3dobjcol, surfaceStyleId, ref surfaceId);               
                }                    

                if (checkBox1.CheckState == CheckState.Checked)
                {
                    double OFFSET = 0;
                    double x1 = 0;
                    double y1 = 0;
                    double x2 = 0;
                    double y2 = 0;
                    ObjectId slgId = SampleLineGroup.Create("Section SampleLine" + DateTime.Today.ToShortDateString() + DateTime.Now.ToShortTimeString(), alignID2);
                    SampleLineGroup slg = trans.GetObject(slgId, OpenMode.ForWrite) as SampleLineGroup;
                    ObjectId slatStationId;
                    Point2dCollection secpo = new Point2dCollection();
                    double sta = 0;

                    List<string> str = new List<string>();
                    for (int i = 0; i < Station.Count; i++)
                    {
                        sta = Station[i];
                        OFFSET = Math.Floor(maxOff);
                        align2.PointLocation(sta, OFFSET, ref x1, ref y1);
                        align2.PointLocation(sta, -1 * OFFSET, ref x2, ref y2);
                        Point2d samVert1 = new Point2d(x1, y1);
                        secpo.Add(samVert1);
                        Point2d samVert2 = new Point2d(x2, y2);
                        secpo.Add(samVert2);

                        string v = "SampleLineByPoints-" + Station[i].ToString() + "-" + DateTime.Now.ToShortTimeString();
                        str.Add(v);
                        slatStationId = SampleLine.Create(v, slgId, secpo);
                        SampleLine sampleLine = trans.GetObject(slatStationId, OpenMode.ForWrite) as SampleLine;
                        sampleLine.StyleId = civildoc.Styles.SampleLineStyles[0];
                        secpo.Clear();
                    }
                    createsurf(dvFinal, Station, db, pol3dobjcol, surfaceStyleId, ref surfaceId);
                    SectionSourceCollection sectionSources = slg.GetSectionSources();
                    foreach (SectionSource sectionSource in sectionSources)
                    {
                        if (sectionSource.SourceId.Equals(surfaceId))
                        {
                            surfaceId = sectionSource.SourceId;
                            TinSurface sourceSurface = trans.GetObject(sectionSource.SourceId, OpenMode.ForRead) as TinSurface;
                            sectionSource.IsSampled = true;
                        }
                    }
                }
                
                trans.Commit();
            }
        }
        public void createsurf(DataView dvFinal, List<double> Station, Database db, ObjectIdCollection pol3dobjcol, ObjectId surfaceStyleId, ref ObjectId surfaceId)
        {
            surfaceId = TinSurface.Create("Surface - " + align2.Name.ToString(), surfaceStyleId);           
            surface = surfaceId.GetObject(OpenMode.ForWrite) as TinSurface;
            surface.Rebuild();
            surface.BreaklinesDefinition.AddStandardBreaklines(pol3dobjcol, 1, 1, 1, 1);
            Autodesk.AutoCAD.DatabaseServices.Polyline3d poly = null;
            boundrypol(dvFinal, Station, db, ref poly);
            //poly.Layer = db.Clayer;
            ObjectIdCollection boundaryEntities = new ObjectIdCollection();
            boundaryEntities.Add(poly.ObjectId);
            surface.BoundariesDefinition.AddBoundaries(boundaryEntities, .1, SurfaceBoundaryType.Outer, true);
            surface.Rebuild();
        }
        public void boundrypol(DataView dvFinal, List<double> Station, Database db, ref Polyline3d poly)
        {
            Point3dCollection po3dcolLeft = new Point3dCollection();
            Point3dCollection po3dcolRight = new Point3dCollection();
            Point3d po;
            double x = 0;
            double y = 0;
            dvFinal.Sort = "STA ASC";
            double maxoff = 0;
            double minoff = 0;
            List<double> LeftOff = new List<double>();
            List<double> RightOff = new List<double>();
            for (int i = 0; i < Station.Count; i++)
            {
                for (int j = 0; j < dvFinal.Count; j++)
                {
                    if ((double)(dvFinal[j][3]) == Station[i])
                    {
                        if ((double)(dvFinal[j][4]) <= minoff)
                            minoff = (double)(dvFinal[j][4]);
                        if ((double)(dvFinal[j][4]) >= maxoff)
                            maxoff = (double)(dvFinal[j][4]);
                    }
                }
                LeftOff.Add(minoff);
                align2.PointLocation(Station[i], minoff, ref x, ref y);
                po = new Point3d(x, y, 0);
                po3dcolLeft.Add(po);
                RightOff.Add(maxoff);
                align2.PointLocation(Station[i], maxoff, ref x, ref y);
                po = new Point3d(x, y, 0);
                po3dcolRight.Add(po);
                maxoff = 0;
                minoff = 0;
            }
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                poly = new Autodesk.AutoCAD.DatabaseServices.Polyline3d();
                btr.AppendEntity(poly);
                trans.AddNewlyCreatedDBObject(poly, true);
                poly.SetDatabaseDefaults();
                foreach (Point3d po2 in po3dcolLeft)
                {
                    PolylineVertex3d vex3d = new PolylineVertex3d(po2);
                    poly.AppendVertex(vex3d);
                    trans.AddNewlyCreatedDBObject(vex3d, true);
                }               
                for (int i = po3dcolRight.Count-1; i >-1 ; i--)
                {
                    PolylineVertex3d vex3d = new PolylineVertex3d(po3dcolRight[i]);
                    poly.AppendVertex(vex3d);
                    trans.AddNewlyCreatedDBObject(vex3d, true);
                }
                PolylineVertex3d vex3d2 = new PolylineVertex3d(po3dcolLeft[0]);
                poly.AppendVertex(vex3d2);
                trans.AddNewlyCreatedDBObject(vex3d2, true);
                poly.Closed = true;
                //LayerTable ltb = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                //if (!ltb.Has("Boundry"))
                //{
                //    ltb.UpgradeOpen();
                //    LayerTableRecord newLayer = new LayerTableRecord();
                //    newLayer.Name = "Boundry";
                //    newLayer.LineWeight = LineWeight.LineWeight005;
                //    newLayer.IsHidden = true;
                //    newLayer.Color = Autodesk.AutoCAD.Colors.Color.FromRgb(0, 0, 0);
                //    ltb.Add(newLayer);
                //    trans.AddNewlyCreatedDBObject(newLayer, true);
                //}                
                trans.Commit();         
            }       
        }
        public void datv2(Editor ed, CivilDocument civildoc, Database db,
            ref DataView dvFinal, ref List<double> Station)
        {
            try
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
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
                }
                double sta = 0;
                double off = 0;
                double x = 0;
                double y = 0;
                double z = 0;
                string d = "";
                List<double> df = new List<double>();
                List<double> tt2 = new List<double>();
                System.Data.DataTable table = new System.Data.DataTable();
                table.Columns.Add("X", typeof(double));
                table.Columns.Add("Y", typeof(double));
                table.Columns.Add("Z", typeof(double));
                table.Columns.Add("STA", typeof(double));
                table.Columns.Add("OFF", typeof(double));
                table.Columns.Add("Desc", typeof(string));
                for (int i = 0; i < TextFile.Length - 5; i = i + 5)
                {
                    x = Convert.ToDouble(TextFile[i + 1]);
                    y = Convert.ToDouble(TextFile[i + 2]);
                    z = Convert.ToDouble(TextFile[i + 3]);
                    d = Convert.ToString(TextFile[i + 4]);
                    ii = TextFile[i + 0];
                    align2.StationOffset(x, y, ref sta, ref off);
                    table.Rows.Add(x, y, z, sta, off, d);
                }
                //---------------------------------

                DataView dv = new DataView(table);
                dv.Sort = "STA ASC";
                slgsta.Sort();
                for (int i = 0; i < dv.Count; i++)
                {
                    sta = Convert.ToDouble(dv[i][3].ToString());
                    for (int j = 0; j < slgsta.Count; j++)
                    {
                        df.Add(Math.Abs(sta - slgsta[j]));
                    }
                    dv[i].Row.BeginEdit();
                    dv[i][3] = slgsta[df.IndexOf(df.Min())];
                    df.Clear();
                }
                //---------درست کردن جدول نهایی از نقاط بصورت xyz,STA,Offset---------
                System.Data.DataTable table2 = new System.Data.DataTable();
                table2.Columns.Add("X", typeof(double));
                table2.Columns.Add("Y", typeof(double));
                table2.Columns.Add("Z", typeof(double));
                table2.Columns.Add("STA", typeof(double));
                table2.Columns.Add("OFF", typeof(double));
                table2.Columns.Add("Desc", typeof(string));
                DataView dv2;
                for (int i = 0; i < dv.Count; i++)
                {
                    x = 0;
                    y = 0;
                    align2.PointLocation(Convert.ToDouble(dv[i][3]), Convert.ToDouble(dv[i][4]), ref x, ref y);
                    table2.Rows.Add(x, y, dv[i][2], dv[i][3], dv[i][4], dv[i][5]);
                }
                dv2 = new DataView(table2);
                dv2.Sort = "OFF ASC";
                dvFinal = dv2;
                Station = slgsta;
            }
            catch (System.Exception ex)
            {
                var st = new System.Diagnostics.StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                ed.WriteMessage("\n" + ex.Message + "\n Line Number" + line + "\n You must remove or edit Point N.O:" + ii);
            }
        }
        public void datv3(Editor ed, CivilDocument civildoc, Database db,
            ref DataView dvFinal, ref List<double> Station)
        {
            try
            {
                if(Tol.Text==null)
                    MessageBox.Show("Tolerance must have a value!", "Tolerance Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Station = null;
                double sta = 0;
                double off = 0;
                double x = 0;
                double y = 0;
                double z = 0;
                string d = "";
                List<double> df = new List<double>();
                List<double> st = new List<double>();
                List<double> st2 = new List<double>();
                List<double> st3 = new List<double>();
                System.Data.DataTable table = new System.Data.DataTable();
                table.Columns.Add("X", typeof(double));
                table.Columns.Add("Y", typeof(double));
                table.Columns.Add("Z", typeof(double));
                table.Columns.Add("STA", typeof(double));
                table.Columns.Add("OFF", typeof(double));
                table.Columns.Add("Desc", typeof(string));
                for (int i = 0; i < TextFile.Length - 5; i = i + 5)
                {
                    x = Convert.ToDouble(TextFile[i + 1]);
                    y = Convert.ToDouble(TextFile[i + 2]);
                    z = Convert.ToDouble(TextFile[i + 3]);
                    d = Convert.ToString(TextFile[i + 4]);
                    ii = TextFile[i + 0];
                    align2.StationOffset(x, y,.1, ref sta, ref off);
                    table.Rows.Add(x, y, z, sta, off, d);
                    st2.Add(sta);
                }
                //---------------------------------
                DataView dv = new DataView(table);
                dv.Sort = "STA ASC";
                st2.Sort();
                st2.Add(st2.Max() + 1e10);
                sta = (double)dv[0][3];
                double s = 0;
                int ind = 1;
                
                for (int i = 1; i <= dv.Count; i++)
                {
                    
                    if(Math.Abs(sta- st2[i])<= Convert.ToDouble(Tol.Text))
                    {
                        s = s + st2[i];
                        ind++;
                    }
                    else
                    {
                        if (ind < 3)

                        {
                            sta = st2[i];
                            st.Add(-1);
                            continue;
                        }
                        else
                        {
                            s = s + sta;
                            s = Math.Floor(s / ind);
                            st3.Add(s);
                            for (int j = 0; j < ind; j++)
                            {
                                st.Add(s);
                            }
                            ind = 1;
                            s = 0;
                        }
                        
                    }
                    sta = st2[i];
                }
                for (int i = 0; i < dv.Count; i++)
                {
                    dv[i].Row.BeginEdit();
                    if (st[i] == -1)
                    {
                        dv[i].Row.Delete();
                        st.RemoveAt(i);
                        i--;
                    }                        
                    else
                        dv[i][3] = st[i];           
                }
                dvFinal = dv;
                Station = st3;
            }
            catch (System.Exception ex)
            {
                var st = new System.Diagnostics.StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                ed.WriteMessage("\n" + ex.Message + "\n Line Number" + line + "\n You must remove or edit Point N.O:" + ii);
            }
        }
        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            SectionFromFile_TxtBox.Text = "";
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;// Application.DocumentManager.MdiActiveDocument.Editor;
            CivilDocument civildoc = CivilApplication.ActiveDocument;
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;

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
                        if(chkCHAIN.CheckState == CheckState.Checked)
                        {
                            string[] inputfields;
                            char[] delim = { '\n', ' ', ' ' };
                            inputfields = whole_file.Split(delim);

                            whole_file = whole_file.Replace('\n', '\r');
                            string[] lines = whole_file.Split(new char[] { '\r', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                            // See how many rows and columns there are.
                            int num_rows = lines.Length;
                            if (lines[0].Contains("chainage") == false)
                            {
                                int sid = 0;
                                string[,] Left = new string[(lines.Length) / 3, 3];
                                for (int i = 0; i < lines.Length / 3; i++)
                                {
                                    Left[i, 0] = lines[sid];
                                    Left[i, 1] = lines[sid + 1];
                                    Left[i, 2] = lines[sid + 2];
                                    sid = sid + 3;
                                }                                
                                for (int i = 0; i < 30; i++)
                                {
                                    SectionFromFile_TxtBox.Text += "\r\n" + lines[i].ToString();

                                }
                            }
                            else
                            {
                                // Allocate the data array.
                                List<int> myInts = new List<int>();
                                for (int i = 0; i < num_rows; i++)
                                {
                                    if (lines[i].IndexOf("chainage") != -1)
                                    {
                                        myInts.Add(i);

                                    }
                                }
                                int[] index = myInts.ToArray();
                                double[] Station = new double[index.Length];
                                double[] minoff = new double[index.Length];
                                minoff[0] = Convert.ToDouble(lines[1]);

                                double[] maxoff = new double[index.Length];
                                maxoff[0] = Convert.ToDouble(lines[1]);

                                int sum1 = 0;
                                int sum2 = 0;
                                int sum3 = 0;
                                int jj = 0;
                                string[,] Left = new string[(lines.Length - index.Length) / 2, 3];
                                for (int i = index[sum1]; i < lines.Length; i = index[sum1])
                                {
                                    if (sum1 >= index.Length - 1)
                                    {
                                        jj = lines.Length - 1;
                                    }
                                    else
                                    {
                                        jj = (index[sum1 + 1] - index[sum1] - 1) + i;
                                    }
                                    if (i == index[sum1])
                                    {

                                        for (int j = i; j < jj; j = j + 2)
                                        {
                                            Station[sum3] = Convert.ToDouble(lines[i].Remove(0, 8));
                                            Left[sum2, 0] = lines[i].Remove(0, 8);
                                            Left[sum2, 1] = lines[j + 1];
                                            Left[sum2, 2] = lines[j + 2];
                                            if (Convert.ToDouble(Left[sum2, 1]) <= minoff[sum3])
                                            {
                                                minoff[sum3] = (Convert.ToDouble(Left[sum2, 1]));
                                            }
                                            if (Convert.ToDouble(Left[sum2, 1]) >= maxoff[sum3])
                                            {
                                                maxoff[sum3] = (Convert.ToDouble(Left[sum2, 1]));

                                            }
                                            sum2++;
                                        }
                                        sum1++;
                                        if (sum1 == index.Length)
                                        {
                                            break;
                                        }
                                    }
                                    sum3++;
                                }
                                Left2 = Left;
                                maxoff2 = maxoff;
                                minoff2 = minoff;
                                Station2 = Station;
                                double x = 0;
                                double y = 0;
                                string Desc = "";
                                int p = 1;
                                List<string> tex = new List<string>();
                                for (int i = 0; i < Left2.Length / 3; i++)
                                {
                                    double sta = Convert.ToDouble(Left2[i, 0]);
                                    double off = Convert.ToDouble(Left2[i, 1]);
                                    double z = Convert.ToDouble(Left2[i, 2]);                                    
                                    if (sta > align2.EndingStation)
                                    {
                                        MessageBox.Show("Your Text File ending Station is greater than Alignment length", "Text File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        throw new System.Exception("Your Text File ending Station is greater than Alignment length");
                                    }
                                    align2.PointLocation(sta, off, ref x, ref y);
                                    tex.Add(p.ToString());
                                    tex.Add(x.ToString());
                                    tex.Add(y.ToString());
                                    tex.Add(z.ToString());
                                    p++;                                  
                                }
                                TextFile = tex.ToArray();
                                for (int i = 0; i < 30; i++)
                                {
                                    SectionFromFile_TxtBox.Text += "\r\n" + tex[i];

                                }

                            }
                        }
                        else
                        {
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
        private void CreateBTN_Click_1(object sender, EventArgs e)
        {
            elaptime.Start();
            TimeElapseStripStatus.Text = "Elapsed Time: 00:00:00 ";
            ProgBar.Value = 0;
            ErrorNOStripStatus.Text = "Errors: 0";
            DataView dvFinal = null;
            DataView dv = null;
            List<double> Station = null;
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            CivilDocument civildoc = CivilApplication.ActiveDocument;
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;       
            ProgBar.Maximum = 100;
            ProgBar.Step = 1;
            ProgBar.Value = 0;
            if (checkBox1.CheckState == CheckState.Checked)
                datv3(ed, civildoc, db, ref dvFinal, ref Station);
            else
                datv2(ed, civildoc, db, ref dvFinal, ref Station);
            ProgBar.Value = 100;
            if (dvFinal == null)
            {
                MessageBox.Show("You must remove or edit Point N.O:" + ii, "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorNOStripStatus.Text = "Errors: 1";
            }
            else
                CreateSurf(dvFinal, Station, civildoc, db);            
            elaptime.Stop();
            TimeElapseStripStatus.Text = "Elapsed Time: " + elaptime.Elapsed.ToString().Remove(8);
            elaptime.Reset();
        }
        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DataView dvFinal = null;
            DataView dv = null;
            List<double> Station = null;          
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            CivilDocument civildoc = CivilApplication.ActiveDocument;
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
            
            if(surface==null)
                MessageBox.Show("You must import points and create surface!");
            else
            {
                if (checkBox1.CheckState == CheckState.Checked)
                    datv3(ed, civildoc, db, ref dvFinal, ref Station);
                else
                    datv2(ed, civildoc, db, ref dvFinal, ref Station);
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
                            if (Convert.ToDouble(dvFinal[j][3]) == Station[i])
                            {
                                if (Convert.ToDouble(dvFinal[j][4]) < 0)
                                    side = "L";
                                else if (Convert.ToDouble(dvFinal[j][4]) > 0)
                                    side = "R";
                                else if ((double)dvFinal[j][4] == 0)
                                    continue;
                                sum++;
                                x = Convert.ToDouble(dvFinal[j][0]);
                                y = Convert.ToDouble(dvFinal[j][1]);
                                z = Convert.ToDouble(dvFinal[j][2]);
                                string OFFELEV = Convert.ToString(sum) + "," + x.ToString("F3") + "," + y.ToString("F3") + "," + z.ToString("F3") + "," + side;
                                txt.Add(OFFELEV);
                            }
                        }
                    }
                    //List<string> STR2 = new List<string>();
                    //STR2.Add("#X,Y,Z,STA,OFF");
                    //for (int i = 0;i < dvFinal.Count; i++)
                    //{
                    //    STR2.Add(dvFinal[i][0].ToString() + "," + dvFinal[i][1].ToString() + "," + dvFinal[i][2].ToString() + "," + dvFinal[i][3].ToString() + "," + dvFinal[i][4].ToString());
                    //}
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
                       
        }
        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        private void راهنماToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sections.AboutCreateSectionFromXYZ win1 = new AboutCreateSectionFromXYZ();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win1);
        }
        public string ii { get; set; }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
                LS_SLG.Enabled = false;
            if (checkBox1.CheckState == CheckState.Unchecked)
                LS_SLG.Enabled = true;
        }
    }
}