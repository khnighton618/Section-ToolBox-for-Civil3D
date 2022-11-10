using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System.Diagnostics;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using System.Data;

namespace Sections
{
    public partial class CreateSectionFromFile : Form
    {
        Stopwatch elaptime = new Stopwatch();
        private StreamReader filereader;       
        public string[,] Left2;
        public string[,] Left3;
        public Alignment align2;
        public ObjectId alignID2;
        public double[] minoff2;
        public double[] maxoff2;
        public double[] Station2;
        public CreateSectionFromFile()
        {
            InitializeComponent();
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
                        comboBox1.Items.Add(align.Name);
                    }
                    comboBox1.SelectedIndex = 0;
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                }
                trans.Commit();
            }
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
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
                        string[] lines = whole_file.Split(new char[] { '\r', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                        // See how many rows and columns there are.
                        int num_rows = lines.Length;
                        if(lines[0].Contains("chainage")==false)
                        {
                            int sid = 0;
                            string[,] Left = new string[(lines.Length ) / 3, 3];
                            for (int i=0; i<lines.Length/3;i++)
                            {
                                Left[i, 0] = lines[sid];
                                Left[i, 1] = lines[sid+1];
                                Left[i, 2] = lines[sid+2];
                                sid = sid + 3;
                            }
                            Left3 = Left;
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

                            for (int i = 0; i < 30; i++)
                            {
                                SectionFromFile_TxtBox.Text += "\r\n" + lines[i].ToString();
                            }
                        }               
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

        private void selalgbtn_Click(object sender, EventArgs e)
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
                    align2 = align;
                    alignID2 = alignID;
                    comboBox1.Items.Clear();   
                    comboBox1.Items.Add(align.Name);
                    comboBox1.SelectedIndex = 0;
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage(ex.Message);
                }
                trans.Commit();
            }  
        }
        private void createsectionBtn_Click(object sender, EventArgs e)
        {
            elaptime.Start();
            TimeElapseStripStatus.Text = "Elapsed Time: 00:00:00 ";
            ProgBar.Value = 0;
            ErrorNOStripStatus.Text = "Errors: 0";            
            ProgBar.Maximum = 100;
            ProgBar.Step = 1;
            ProgBar.Value = 0;
            if (Point_Group_TxtBox.Text.Equals(""))
            {
                MessageBox.Show("You must enter a typical name!", "Typical Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new System.Exception("You must enter a typical name!");
            }
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;// Application.DocumentManager.MdiActiveDocument.Editor;
            CivilDocument civildoc = CivilApplication.ActiveDocument;
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;            
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    if (align2 == null)
                    {
                        MessageBox.Show("You must Select an Alignment", "Alignment Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new System.Exception("You must Select an Alignment");
                    }
                    System.Data.DataTable tb = new System.Data.DataTable();
                    tb.Columns.Add("X", typeof(double));
                    tb.Columns.Add("Y", typeof(double));
                    tb.Columns.Add("Z", typeof(double));
                    tb.Columns.Add("STA", typeof(double));
                    tb.Columns.Add("OFF", typeof(double));
                    
                    CogoPointCollection cog = civildoc.CogoPoints;                    
                    ObjectId postyle = civildoc.Styles.PointStyles[0];
                    int errtext = 0;
                    foreach(ObjectId id in civildoc.PointGroups)
                    {
                        PointGroup pogrp = (PointGroup)trans.GetObject(id, OpenMode.ForRead);
                        if (pogrp.Name == Point_Group_TxtBox.Text.ToString() + "-" + align2.Name.ToString())
                        {
                            MessageBox.Show("Enter another name for Name Style", "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            errtext = 1;
                        }
                    }
                    ObjectId groupId ;
                    if (errtext==0)
                    {
                        
                            groupId = civildoc.PointGroups.Add(Point_Group_TxtBox.Text.ToString() + "-" + align2.Name.ToString());
                            PointGroup group = groupId.GetObject(OpenMode.ForWrite) as PointGroup;
                        
                        
                        if (Left2 == null) Left2 = Left3;
                        double x = 0;
                        double y = 0;
                        string Desc = "";
                        List<double> STA2 = new List<double>();
                        for (int i = 0; i < Left2.Length / 3; i++)
                        {
                            double sta = Convert.ToDouble(Left2[i, 0]);
                            double off = Convert.ToDouble(Left2[i, 1]);
                            double z = Convert.ToDouble(Left2[i, 2]);
                            STA2.Add(sta);
                            if (sta > align2.EndingStation)
                            {
                                MessageBox.Show("Your Text File ending Station is greater than Alignment length", "Text File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                throw new System.Exception("Your Text File ending Station is greater than Alignment length");
                            }
                            align2.PointLocation(sta, off, ref x, ref y);
                            tb.Rows.Add(x, y, z, sta, off);
                            Point3d po3d = new Point3d(x, y, z);
                            if (Math.Abs(off) < 0.001)
                                Desc = "AXE-STA = " + sta.ToString() + Point_Group_TxtBox.Text.ToString();
                            else
                                Desc = "STA = " + sta.ToString() + " Off= " + off.ToString() + Point_Group_TxtBox.Text.ToString();
                            if (chkpo.Checked == true)
                            {
                                ObjectId pointIds = cog.Add(po3d, Desc, true);
                            }
                        }
                        DataView dv = new DataView(tb);
                        
                        if (chkpo.Checked == true)
                        {
                            StandardPointGroupQuery query = new StandardPointGroupQuery();
                            query.IncludeRawDescriptions = "*" + Point_Group_TxtBox.Text.ToString() + "*";
                            PointGroup groupPO = groupId.GetObject(OpenMode.ForWrite) as PointGroup;
                            groupPO.SetQuery(query);
                            //Autodesk.Civil.Settings.SettingsPoint pointSettings = civildoc.Settings.GetSettings<Autodesk.Civil.Settings.SettingsPoint>() as Autodesk.Civil.Settings.SettingsPoint;
                        }

                        ObjectId surfaceStyleId = civildoc.Styles.SurfaceStyles[0];
                        ObjectId surfaceId = TinSurface.Create(Point_Group_TxtBox.Text.ToString() + "-" + align2.Name.ToString(), surfaceStyleId);
                        TinSurface surface = surfaceId.GetObject(OpenMode.ForWrite) as TinSurface;
                        ProgBar.Value = 50;
                        surface.Rebuild();
                        x = 0;
                        y = 0;
                        int v = 0;
                        List<double> STA = STA2.Distinct().ToList();
                        Point3dCollection po3d2 = new Point3dCollection();
                        ObjectIdCollection pol3dobjcol = new ObjectIdCollection();
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
                            Point3d popo2d = new Point3d(x, y, z);
                            if (STA[v] == sta)
                            {
                                po3d2.Add(popo2d);
                                if (i == Left2.Length / 3 - 1)
                                {
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
                                    v++;
                                    po3d2.Clear();
                                }
                                continue;
                            }
                            else
                            {
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
                                v++;
                                po3d2.Clear();
                                i--;
                            }
                        }
                        surface.BreaklinesDefinition.AddStandardBreaklines(pol3dobjcol, 0.001, .5, .5, .5);
                        surface.Rebuild();
                        Autodesk.AutoCAD.DatabaseServices.Polyline3d poly = null;                        
                        boundrypol(dv, STA, db, ref poly);
                        //poly.Layer = db.Clayer;
                        ObjectIdCollection boundaryEntities = new ObjectIdCollection();
                        boundaryEntities.Add(poly.ObjectId);
                        surface.BoundariesDefinition.AddBoundaries(boundaryEntities, .1, Autodesk.Civil.SurfaceBoundaryType.Outer, true);
                        surface.Rebuild();
                        if(chkSLG.Checked==true)
                        {
                            ObjectId slgId = SampleLineGroup.Create("Section SampleLine" + "-" + Point_Group_TxtBox.Text.ToString(), alignID2);
                            SampleLineGroup slg = trans.GetObject(slgId, OpenMode.ForWrite) as SampleLineGroup;
                            ObjectId slatStationId;
                            Point2dCollection secpo = new Point2dCollection();
                            double sta2 = 0;
                            double x1 = 0;
                            double y1 = 0;
                            double x2 = 0;
                            double y2 = 0;
                            for (int i = 0; i <= Station2.Length - 1; i++)
                            {
                                sta2 = Station2[i];

                                align2.PointLocation(sta2, 40, ref x1, ref y1);
                                align2.PointLocation(sta2, -40, ref x2, ref y2);
                                Point2d samVert1 = new Point2d(x1, y1);
                                secpo.Add(samVert1);
                                Point2d samVert2 = new Point2d(x2, y2);
                                secpo.Add(samVert2);
                                slatStationId = SampleLine.Create("SampleLineByPoints-" + Station2[i].ToString() + "-" + Point_Group_TxtBox.Text.ToString(), slgId, secpo);
                                SampleLine sampleLine = trans.GetObject(slatStationId, OpenMode.ForWrite) as SampleLine;
                                sampleLine.StyleId = civildoc.Styles.SampleLineStyles[0];
                                secpo.Clear();
                            }
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
                        
                        ProgBar.Value = 100;
                        surface.Rebuild();
                    }                       
                }
                catch (System.Exception ex)
                {
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                    ed.WriteMessage("\n" + ex.Message);
                }
                trans.Commit();
            }
            elaptime.Stop();
            TimeElapseStripStatus.Text = "Elapsed Time: " + elaptime.Elapsed.ToString().Remove(8);//.Hours+":"+elaptime.Elapsed.Minutes+":"+elaptime.Elapsed.Seconds;
            ErrorNOStripStatus.Text = "Errors: ";// + NOErr.ToString();
            elaptime.Reset();
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
                for (int i = po3dcolRight.Count - 1; i > -1; i--)
                {
                    PolylineVertex3d vex3d = new PolylineVertex3d(po3dcolRight[i]);
                    poly.AppendVertex(vex3d);
                    trans.AddNewlyCreatedDBObject(vex3d, true);
                }
                PolylineVertex3d vex3d2 = new PolylineVertex3d(po3dcolLeft[0]);
                poly.AppendVertex(vex3d2);
                trans.AddNewlyCreatedDBObject(vex3d2, true);
                poly.Closed = true;
                             
                trans.Commit();
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            string alname = comboBox1.SelectedItem.ToString();
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            CivilDocument civildoc = CivilApplication.ActiveDocument;
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    for (int i=0;i<civildoc.GetAlignmentIds().Count;i++)
                    {
                        ObjectId alIDs = civildoc.GetAlignmentIds()[index];
                        Alignment Align = trans.GetObject(alIDs, OpenMode.ForRead) as Alignment;
                        if (Align.Name==alname)
                        {
                            align2 = Align;
                            alignID2 = alIDs;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                }
                trans.Commit();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About_importsection about1 = new About_importsection();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(about1);
        }

        private void CreateSectionFromFile_Load(object sender, EventArgs e)
        {

        }
    }
}