using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using System.Diagnostics;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
//using System.Linq;

using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using System.IO;
//using ConvexHull.Ouellet;
using Sections;
namespace SectionVer2
{
    public partial class DescriptionKeyTransfer : Form
    {
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
        public DescriptionKeyTransfer()
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
                    SampleLineGroup slg2;
                    foreach (ObjectId slgID in align.GetSampleLineGroupIds())
                    {
                        slg2 = trans.GetObject(slgID, OpenMode.ForWrite) as SampleLineGroup;
                        LS_SLG.Items.Add(slg2.Name);
                    }
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
                    foreach(ObjectId alignID2 in alIDs)                    
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
        public void CreateSurf(DataView dv, DataView dv4, List<double> Station, CivilDocument civildoc, Database db)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                double maxOff = 0;
                dv.Sort = "OFF ASC";
                if (Math.Abs(Convert.ToDouble(dv[0][4])) > Convert.ToDouble(dv[dv.Count - 1][4]))
                {
                    maxOff = Math.Abs(Convert.ToDouble(dv[0][4]));
                }
                else
                {
                    maxOff = Convert.ToDouble(dv[dv.Count - 1][4]);
                }

                //----------تولید گروه نقاط--------------
                CogoPointCollection cog = civildoc.CogoPoints;
                Point3dCollection po3dcol = new Point3dCollection();
                ObjectId postyle = civildoc.Styles.PointStyles[0];
                ObjectId groupId = civildoc.PointGroups.Add(align2.Name.ToString() + DateTime.Today.ToShortDateString() + DateTime.Now.ToShortTimeString());
                //
                PointGroup group = groupId.GetObject(OpenMode.ForWrite) as PointGroup;
                Point3dCollection po3d2 = new Point3dCollection();
                ObjectIdCollection pol3dobjcol = new ObjectIdCollection();
                //double x1 = 0;
                //double y1 = 0;
                //double x2 = 0;
                //double y2 = 0;
                DataView dv3 = null;
                //double OFFSET = 0;
                //dv4 = dv3;
                dv3 = dv4;
                string Desc = "";
                //Convex(Station,dv4, ref dv3);
                for (int i = 0; i < dv4.Count; i++)
                {
                    Point3d po3d = new Point3d(Convert.ToDouble(dv4[i][0]), Convert.ToDouble(dv4[i][1]), Convert.ToDouble(dv4[i][2]));
                    if (Convert.ToDouble(dv4[i][4]) <= 0.0001)
                        Desc = "STA = " + dv4[i][3].ToString();
                    else
                        Desc = "STA = " + dv4[i][3].ToString() + " Off= " + dv4[i][4].ToString();
                    ObjectId pointIds = cog.Add(po3d, Desc, true);

                }
                //int s = 0;
                //Point3d po3d22 = new Point3d();
                for (int i = 0; i < Station.Count; i++)
                {
                    //s = 0;
                    for (int j = 0; j < dv3.Count; j++)
                    {
                        if (Convert.ToDouble(dv3[j][3]) == Station[i])
                        {
                            //if(s==0)
                            //{
                            //    po3d22 = new Point3d(Convert.ToDouble(dv3[j][0]), Convert.ToDouble(dv3[j][1]), Convert.ToDouble(dv3[j][2]));
                            //    s = 1;
                            //}
                            Point3d po3d = new Point3d(Convert.ToDouble(dv3[j][0]), Convert.ToDouble(dv3[j][1]), Convert.ToDouble(dv3[j][2]));
                            po3d2.Add(po3d);
                        }
                    }
                    //po3d2.Add(po3d22);
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

                //ObjectId slgId = SampleLineGroup.Create("Section SampleLine" + DateTime.Today.ToShortDateString() + DateTime.Now.ToShortTimeString(), alignID2);
                //SampleLineGroup slg = trans.GetObject(slgId, OpenMode.ForWrite) as SampleLineGroup;
                //ObjectId slatStationId;
                //Point2dCollection secpo = new Point2dCollection();
                //double sta = 0;

                //List<string> str = new List<string>();
                //for (int i = 0; i < Station.Count; i++)
                //{
                //    sta = Station[i];
                //    OFFSET = Math.Floor(maxOff);
                //    align2.PointLocation(sta, OFFSET, ref x1, ref y1);
                //    align2.PointLocation(sta, -1 * OFFSET, ref x2, ref y2);
                //    Point2d samVert1 = new Point2d(x1, y1);
                //    secpo.Add(samVert1);
                //    Point2d samVert2 = new Point2d(x2, y2);
                //    secpo.Add(samVert2);

                //    string v = "SampleLineByPoints-" + Station[i].ToString() + "-" + DateTime.Now.ToShortTimeString();
                //    str.Add(v);
                //    slatStationId = SampleLine.Create(v, slgId, secpo);
                //    SampleLine sampleLine = trans.GetObject(slatStationId, OpenMode.ForWrite) as SampleLine;
                //    sampleLine.StyleId = civildoc.Styles.SampleLineStyles[0];
                //    secpo.Clear();
                //}

                //SectionSourceCollection sectionSources = slg.GetSectionSources();
                //foreach (SectionSource sectionSource in sectionSources)
                //{
                //    if (sectionSource.SourceId.Equals(surfaceId))
                //    {
                //        surfaceId = sectionSource.SourceId;
                //        TinSurface sourceSurface = trans.GetObject(sectionSource.SourceId, OpenMode.ForRead) as TinSurface;
                //        sectionSource.IsSampled = true;
                //    }
                //}
                surface.Rebuild();
                trans.Commit();
            }
        }
        public void datv2(Editor ed, CivilDocument civildoc, Database db,
            ref DataView dvFinal, ref DataView dv, ref List<double> Station,ref DataView dvTunnel)
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
                List<double> df = new List<double>();
                List<double> tt2 = new List<double>();
                for (int i = 0; i < TextFile.Length - 4; i = i + 4)
                {
                    x = Convert.ToDouble(TextFile[i + 1]);
                    y = Convert.ToDouble(TextFile[i + 2]);
                    z = Convert.ToDouble(TextFile[i + 3]);
                    ii = TextFile[i + 0];
                    align2.StationOffset(x, y, ref sta, ref off);
                    tt2.Add(x);
                    tt2.Add(y);
                    tt2.Add(z);
                    tt2.Add(sta);
                    tt2.Add(off);
                }
                //---------------------------------
                System.Data.DataTable table = new System.Data.DataTable();
                table.Columns.Add("X", typeof(double));
                table.Columns.Add("Y", typeof(double));
                table.Columns.Add("Z", typeof(double));
                table.Columns.Add("STA", typeof(double));
                table.Columns.Add("OFF", typeof(double));
                for (int i = 0; i < tt2.Count; i = i + 5)
                {
                    table.Rows.Add(tt2[i], tt2[i + 1], tt2[i + 2], tt2[i + 3], tt2[i + 4]);
                }
                dv = new DataView(table);
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
                    align2.PointLocation(Convert.ToDouble(dv[i][3]), Convert.ToDouble(dv[i][4]), ref x, ref y);
                    table2.Rows.Add(x, y, dv[i][2], dv[i][3], dv[i][4]);
                }
                dv2 = new DataView(table2);
                dv2.Sort = "OFF ASC";
                dvFinal = dv2;
                Station = slgsta;
                DataView dv3 = new DataView(table2);
                dv3.Sort = "STA ASC";
                //----------Conve Hull------------
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
                dvTunnel = new DataView(tab2);
                
                
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
        private void CreateBTN_Click_1(object sender, EventArgs e)
        {
            elaptime.Start();
            TimeElapseStripStatus.Text = "Elapsed Time: 00:00:00.0000000 ";
            ProgBar.Value = 0;
            ErrorNOStripStatus.Text = "Errors: 0";
            DataView dvFinal = null;
            DataView dv = null;
            DataView dvTunnel = null;
            List<double> Station = null;
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            CivilDocument civildoc = CivilApplication.ActiveDocument;
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;

            ProgBar.Maximum = 100;
            ProgBar.Step = 1;
            ProgBar.Value = 0;
            datv2(ed, civildoc, db, ref dvFinal, ref dv, ref Station,ref dvTunnel);
            ProgBar.Value = 100;
            if (dv == null)
            {
                MessageBox.Show("You must remove or edit Point N.O:" + ii, "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ErrorNOStripStatus.Text = "Errors: 1";
            }
            else
                CreateSurf(dv, dvTunnel, Station, civildoc, db);
            elaptime.Stop();
            TimeElapseStripStatus.Text = "Elapsed Time: " + elaptime.Elapsed;
            elaptime.Reset();
        }
        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            DataView dvFinal = null;
            DataView dv = null;
            DataView dvTunnel = null;
            List<double> Station = null;
            //Transaction trans = null;
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            CivilDocument civildoc = CivilApplication.ActiveDocument;
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
            datv2(ed, civildoc, db, ref dvFinal, ref dv, ref Station,ref dvTunnel);

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
        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        private void راهنماToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Sections.AboutCreateSectionFromXYZ win1 = new Sections.AboutCreateSectionFromXYZ();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win1);
        }
        public string ii { get; set; }
    }
}
