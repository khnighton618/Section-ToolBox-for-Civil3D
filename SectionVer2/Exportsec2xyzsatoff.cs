using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using System.Diagnostics;

using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;

namespace Sections
{
    public partial class Exportsec2xyzsatoff : Form
    {
        public Alignment align;
        int ss = 0;
        public PromptSelectionResult SelectedSections;
        public List<int> ListofSections;
        public SampleLineGroup slg = null;
        Stopwatch elaptime = new Stopwatch();
        Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
        CivilDocument civildoc = CivilApplication.ActiveDocument;
        Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
        private StreamWriter filewriter;
        public Exportsec2xyzsatoff()
        {
            
            // TODO: Complete member initialization
            InitializeComponent();
            intialization(0,0, true);
            ss++;
        }
        public void PFselID(ref int PFselID, ref int ALselID, ref PromptSelectionResult Selsec, ref List<int> chksel, ref List<int> AR)
        {
            PFselID = LSboxEG.SelectedIndex;
            ALselID = AlignmentsList.SelectedIndex;
            Selsec = SelectedSections;
            AR = ListofSections;
            for (int i = 0; i < CHLBoxSectionLists.CheckedIndices.Count; i++)
            {
                chksel.Add(ListofSections[CHLBoxSectionLists.CheckedIndices[i]]);
            }
        }
        public void intialization(int indexal, int indexslg, bool first)
        {
            LSboxEG.Items.Clear();
            if (first == true)
                AlignmentsList.Items.Clear();
            CHLBoxSectionLists.Items.Clear();
            LS_SLG.Items.Clear();
            LS_SVG.Items.Clear();
            SectionSources.Items.Clear();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    Alignment align2;
                    ObjectIdCollection alIDs = civildoc.GetAlignmentIds();
                    if (alIDs == null)
                    {
                        MessageBox.Show("You must have at least one alignment", "No Alignment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new System.Exception("There is no alignment");
                    }
                    int coalid = alIDs.Count;
                    List<string> alname = new List<string>();
                    if (first == true)
                    {
                        for (int i = 0; i < coalid; i++)
                        {

                            ObjectId alignID = alIDs[i];
                            align2 = trans.GetObject(alignID, OpenMode.ForRead) as Alignment;
                            alname.Add(align2.Name);
                            AlignmentsList.Items.Add(align2.Name);


                            if (i == indexal)
                                align = align2;
                        }
                        AlignmentsList.SelectedIndex = indexal;
                    }
                    else
                        align = trans.GetObject(alIDs[indexal], OpenMode.ForRead) as Alignment;

                    slg = trans.GetObject(align.GetSampleLineGroupIds()[indexal], OpenMode.ForWrite) as SampleLineGroup;
                    if (slg == null)
                    {
                        MessageBox.Show("Alignment must have a sample line group", "No sample line group", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new System.Exception("There is no sample line group");
                    }
                    foreach (ObjectId slgID in align.GetSampleLineGroupIds())
                    {
                        slg = trans.GetObject(slgID, OpenMode.ForWrite) as SampleLineGroup;
                        LS_SLG.Items.Add(slg.Name);
                    }
                    LS_SLG.SelectedIndex = indexslg;
                    for (int i = 0; i < slg.SectionViewGroups.Count; i++)
                    {
                        LS_SVG.Items.Add(slg.SectionViewGroups[i].Name);
                    }
                    LS_SVG.SelectedIndex = 0;

                    ObjectIdCollection alIDs2 = civildoc.GetSurfaceIds();
                    if (alIDs2 == null)
                    {
                        MessageBox.Show("You must have existing surface", "No Exiting Surface", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new System.Exception("There is no Exiting Surface");
                    }
                    foreach (ObjectId slgID in civildoc.GetSurfaceIds())
                    {
                        TinSurface tin = trans.GetObject(slgID, OpenMode.ForWrite) as TinSurface;
                        LSboxEG.Items.Add(tin.Name);
                    }
                    LSboxEG.SelectedIndex = 0;
                    foreach (var id in slg.GetSectionSources())
                    {
                        if (id.SourceType == SectionSourceType.TinSurface || id.SourceType == SectionSourceType.CorridorSurface)
                        {
                            TinSurface tinslg = trans.GetObject(id.SourceId, OpenMode.ForWrite) as TinSurface;
                            SectionSources.Items.Add(tinslg.Name);
                        }                        
                    }
                    SectionSources.SelectedIndex = 0;

                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                }
                trans.Commit();
            }
        }
        private void AlignmentsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ss!=0)
                intialization(AlignmentsList.SelectedIndex, 0, false);            
        }
        private void btnAll_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < CHLBoxSectionLists.Items.Count; x++)
            {
                CHLBoxSectionLists.SetItemChecked(x, true);
            }
        }
        private void btnSelSecs_Click(object sender, EventArgs e)
        {
            CHLBoxSectionLists.Items.Clear();
            PromptSelectionOptions selsec = new PromptSelectionOptions();
            selsec.AllowDuplicates = false;
            selsec.AllowSubSelections = true;
            PromptSelectionResult secres = ed.GetSelection(selsec);
            if (secres.Status != PromptStatus.OK) return;
            SelectedSections = secres;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    DataGridView Table;

                    Table = new DataGridView();
                    Table.Columns.Add("Index", "Index Station");
                    Table.Columns.Add("STATION", "Station");

                    for (int il = 0; il < secres.Value.Count; il++)
                    {
                        ObjectId secid = secres.Value[il].ObjectId;
                        Autodesk.Civil.DatabaseServices.Section secs = trans.GetObject(secid, OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Section;
                        if (secs != null)
                        {
                            Table.Rows.Add(il, secs.Station);
                        }
                    }
                    Table.Sort(Table.Columns[1], ListSortDirection.Ascending);

                    double[] sta = new double[secres.Value.Count];
                    int sum = 0;
                    int i = 0;
                    string header22 = "";
                    List<string> txt22 = new List<string>();
                    List<int> array3 = new List<int>();
                    for (int ik = 0; ik < Table.RowCount; ik++)
                    {
                        i = Convert.ToInt32(Table[0, ik].Value);
                        ObjectId secid = secres.Value[i].ObjectId;
                        Autodesk.Civil.DatabaseServices.Section secs = trans.GetObject(secid, OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Section;

                        if (secs != null)
                        {
                            sum++;
                            SectionPointCollection secpo = secs.SectionPoints;
                            sta[ik] = secs.Station;

                            double staform = sta[ik] - Math.Floor(sta[ik] / 1000) * 1000;
                            //header22 = Convert.ToString(sum) + "   STA =" + Convert.ToString(Math.Floor(sta[i] / 1000)) + "+" + staform.ToString("F3") + "-" + secs.Name.ToString();

                            header22 = "STA =" + Convert.ToString(sta[ik]) + "--" + secs.Name.ToString();
                            txt22.Add(Convert.ToString(header22));

                            array3.Add(i);
                        }
                        ListofSections = array3;
                    }
                    //txt22.Sort()
                    for (int ij = 0; ij < txt22.Count; ij++)
                    {
                        CHLBoxSectionLists.Items.Add(txt22[ij]);
                        //LSbox1.Items.Add(txt22[i]);
                    }
                    //CHLBox1.Sorted = true;
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                }
                trans.Commit();
            }
        }
        private void btnNone_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < CHLBoxSectionLists.Items.Count; x++)
            {
                CHLBoxSectionLists.SetItemChecked(x, false);
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            elaptime.Start();
            TimeElapseStripStatus.Text = "Elapsed Time: 00:00:00 ";
            ProgBar.Value = 0;
            ErrorNOStripStatus.Text = "Errors: 0";            
            createXYZSTA(false, false, true);
            elaptime.Stop();
            TimeElapseStripStatus.Text = "Elapsed Time: " + elaptime.Elapsed.ToString().Remove(8);//.Hours+":"+elaptime.Elapsed.Minutes+":"+elaptime.Elapsed.Seconds;
            ErrorNOStripStatus.Text = "Errors: ";// + NOErr.ToString();
            elaptime.Reset();
        }
        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            About_importsection about1 = new About_importsection();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(about1);
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LSboxEG.Items.Contains(false))
            {
                MessageBox.Show("You must select Profile", "Profile Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        public void createXYZSTA(bool chain, bool staoff, bool total)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    double sta = 0;
                    double x = 0;
                    double y = 0;
                    double xx = 0;
                    double yy = 0;
                    int sum = 0;
                    int sum2 = 0;
                    int ss = 0;
                    int progbarID = 0;
                    string header = "";
                    List<string> txt = new List<string>();
                    List<string> txtXYZ = new List<string>();
                    List<string> txtChainage = new List<string>();                    
                    List<double> arr = new List<double>();
                    foreach (var it in CHLBoxSectionLists.CheckedItems)
                    {
                        arr.Add(Convert.ToDouble(it.ToString().Split(',')[1].Split('=')[1]));
                    }
                    ProgBar.Maximum = arr.Count;
                    ProgBar.Step = 1;
                    ProgBar.Value = 0;
                    foreach (var station in arr)
                    {
                        progbarID++;
                        ProgBar.Value = progbarID;                        
                        Autodesk.Civil.DatabaseServices.Section secs = null;
                        foreach (ObjectId osamID in slg.GetSampleLineIds())
                        {
                            SampleLine osam = trans.GetObject(osamID, OpenMode.ForWrite) as SampleLine;
                            if (Math.Abs(station-osam.Station)<=0.001)
                            {
                                ObjectIdCollection origenes = osam.GetSectionIds();
                                foreach (ObjectId id in origenes)
                                {
                                    Autodesk.Civil.DatabaseServices.Entity ent = (Autodesk.Civil.DatabaseServices.Entity)id.GetObject(OpenMode.ForRead);
                                    string name = ent.Name;
                                    secs = trans.GetObject(id, OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Section;
                                    if (name.Contains(SectionSources.SelectedItem.ToString()))
                                    {
                                        break;
                                    }
                                }
                            }
                            //break;
                        }
                        SectionPointCollection secpo = secs.SectionPoints;

                        if (secs != null)
                        {                                                     
                            double dx = 0;
                            List<int> PdxList = new List<int>();
                            double ep = 0;
                            int countCSDP = secs.SectionPoints.Count;
                            int ll = 0;
                            if (countCSDP <= 49)
                            {
                                for (int il = 0; il < secs.SectionPoints.Count; il++)
                                {
                                    PdxList.Add(il);
                                }
                            }
                            while (countCSDP >= 49)
                            {
                                PdxList.Clear();
                                for (int ii = 0; ii < secs.SectionPoints.Count - 1; ii++)
                                {
                                    dx = Math.Pow((secs.SectionPoints[ll].Location.X - secs.SectionPoints[ii + 1].Location.X), 2) - Math.Pow((secs.SectionPoints[ll].Location.Y - secs.SectionPoints[ii + 1].Location.Y), 2);
                                    if (dx > Convert.ToDouble(Pdx.Text) + ep)
                                    {
                                        PdxList.Add(ll);
                                        ll = ii + 1;
                                    }
                                }
                                countCSDP = PdxList.Count;
                                ep = ep + .1;
                            }

                            double leftOff = secs.LeftOffset;
                            double rightoff = secs.RightOffset;
                            List<double> X = new List<double>();
                            List<double> Y = new List<double>();
                            int ik = 0;
                            for (int ijk = 0; ijk < PdxList.Count; ijk++)
                            {
                                ik = PdxList[ijk];
                                if ((secs.SectionPoints[ik].Location.X - rightoff > .1) || (leftOff - secs.SectionPoints[ik].Location.X > .1))
                                {
                                    continue;
                                }
                                X.Add(secs.SectionPoints[ik].Location.X);
                                Y.Add(secs.SectionPoints[ik].Location.Y);
                            }
                            sta = secs.Station;
                            double staform = sta - Math.Floor(sta / 1000) * 1000;
                            align.PointLocation(sta, 0, ref x, ref y);
                            ObjectIdCollection surfIDs = civildoc.GetSurfaceIds();
                            TinSurface surfEG = trans.GetObject(surfIDs[LSboxEG.SelectedIndex], OpenMode.ForWrite) as TinSurface;
                            double z = surfEG.FindElevationAtXY(x, y);
                            header = Convert.ToString(sum) + "," + Convert.ToString(x.ToString("F3")) + "," + Convert.ToString(y.ToString("F3")) + "," + Convert.ToString(z.ToString("F3")) + "," + Convert.ToString(Math.Floor(sta / 1000)) + "+" + staform.ToString("F3");
                            txt.Add(Convert.ToString(header));
                            if (chain==true || staoff == true)
                            {
                                txtChainage.Add("Chainage=" + Convert.ToString(sta));
                                double off = 0;
                                for (int j = 0; j < secpo.Count; j++)
                                {
                                    off = secpo[j].Location.X;
                                    z = secpo[j].Location.Y;
                                    align.PointLocation(sta, off, ref x, ref y);
                                    string OFFELEV = Convert.ToString(off) + "," + Convert.ToString(z);
                                    txtChainage.Add(OFFELEV);
                                    txtXYZ.Add(x.ToString() + "," + y.ToString() + "," + z.ToString() + "," + off.ToString() + "," + sta.ToString());
                                }
                            }
                            else
                            {
                                for (int j = 0; j < X.Count; j++)
                                {
                                    string side = "Axis";
                                    align.PointLocation(sta, X[j], ref xx, ref yy);
                                    double Xround = Math.Round(X[j], 4);
                                    if (Xround < -0.00001)
                                    {
                                        side = "L";
                                        sum++;
                                    }
                                    else if (Xround > 0.00001)
                                    {
                                        side = "R";
                                        sum++;
                                    }
                                    else if (Xround == 0)
                                        continue;
                                    string OFFELEV = Convert.ToString(sum) + "," + xx.ToString("F3") + "," + yy.ToString("F3") + "," + Y[j].ToString("F3") + "," + side;
                                    txt.Add(OFFELEV);
                                }
                            }                   
                        }
                        else
                            continue;
                        
                    }
                    if (chain == true)
                        saveTextFile(txtChainage);
                    if(staoff==true)
                        saveTextFile(txtXYZ);
                    if(total==true)
                        saveTextFile(txt);
                    //-------------------------------------------

                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                }

                trans.Commit();
            }

        }
        public void saveTextFile(List<string> txt)
        {
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
                    for (int i = 0; i < txt.ToArray().Length; i++)
                    {
                        filewriter.WriteLine(txt[i]);
                    }
                    filewriter.Close();
                }

                catch (IOException)
                {
                    MessageBox.Show("Error writing to file", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void saveGenericToolStripMenuItem_Click(object sender, EventArgs e)
        {
            createXYZSTA(true, false, false);
        }
        private void saveStaOffHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            createXYZSTA(false, true, false);
        }
        private void ClearBTN_Click(object sender, EventArgs e)
        {
            CHLBoxSectionLists.Items.Clear();
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void LS_SVG_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void label7_Click(object sender, EventArgs e)
        {

        }
        private void SectionSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            TinSurface tin = null;
            Corridor cors = null;
            CHLBoxSectionLists.Items.Clear();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    foreach (ObjectId slgID in align.GetSampleLineGroupIds())
                    {
                        slg = trans.GetObject(slgID, OpenMode.ForWrite) as SampleLineGroup;
                        if (slg.Name == LS_SLG.SelectedItems[0].ToString())
                            break;
                    }
                    if (slg == null)
                    {
                        MessageBox.Show("Alignment must have a sample line group", "No sample line group", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new System.Exception("There is no sample line group");
                    }
                    foreach (var id in slg.GetSectionSources())
                    {
                        if (id.SourceType == SectionSourceType.TinSurface || id.SourceType == SectionSourceType.CorridorSurface)
                        {
                            tin = trans.GetObject(id.SourceId, OpenMode.ForWrite) as TinSurface;
                            if (tin.Name == SectionSources.SelectedItems[0].ToString())
                                break;                            
                        }
                        //if (id.SourceType == SectionSourceType.Corridor)
                        //{
                        //    cors = trans.GetObject(id.GetSectionIds()[0], OpenMode.ForRead) as Corridor;
                        //    if (cors.Name == SectionSources.SelectedItems.ToString())
                        //        break;
                        //}
                    }
                    
                    
                    foreach (ObjectId osamID in slg.GetSampleLineIds())
                    {
                        
                        SampleLine osam = trans.GetObject(osamID, OpenMode.ForWrite) as SampleLine;
                        //Section sec = trans.GetObject(osam.get, OpenMode.ForWrite) as Section;
                        ObjectIdCollection origenes = osam.GetSectionIds();
                        foreach (ObjectId id in origenes)
                        {
                            
                            Autodesk.Civil.DatabaseServices.Entity ent = (Autodesk.Civil.DatabaseServices.Entity)id.GetObject(OpenMode.ForRead);
                            string name = ent.Name;
                            if (name.Contains(SectionSources.SelectedItem.ToString()))
                            {
                                CHLBoxSectionLists.Items.Add(name+ ", STA= "+osam.Station.ToString("F3"));
                            }
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
    }
}
//
//public void saveGeneric(ref List<string> txtChainage, ref List<string> txtXYZ)
//{
//    using (Transaction trans = db.TransactionManager.StartTransaction())
//    {
//        try
//        {
//            int alID = 0;
//            int PfID = 0;
//            List<int> chsel = new List<int>();
//            List<int> arr = new List<int>();
//            PromptSelectionResult secres = null;
//            PFselID(ref PfID, ref alID, ref secres, ref chsel, ref arr);
//            if (chsel.Count <= 0)
//            {
//                MessageBox.Show("You must select Section", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                throw new System.Exception("You must select Sections!!!");
//            }
//            ObjectId alignID = civildoc.GetAlignmentIds()[alID];
//            Alignment align = trans.GetObject(alignID, OpenMode.ForRead) as Alignment;
//            //-------------------------------------------
//            double sta = 0;
//            double off = 0;
//            double x = 0;
//            double y = 0;
//            double z = 0;
//            txtChainage = new List<string>();
//            foreach (int i in arr)
//            {
//                ObjectId secid = secres.Value[i].ObjectId;
//                Autodesk.Civil.DatabaseServices.Section secs = trans.GetObject(secid, OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Section;
//                if (secs != null)
//                {
//                    SectionPointCollection secpo = secs.SectionPoints;
//                    sta = secs.Station;
//                    txtChainage.Add("Chainage=" + Convert.ToString(sta));
//                    for (int j = 0; j < secpo.Count; j++)
//                    {
//                        off = secpo[j].Location.X;
//                        z = secpo[j].Location.Y;
//                        align.PointLocation(sta, off, ref x, ref y);
//                        string OFFELEV = Convert.ToString(off) + "," + Convert.ToString(z);
//                        txtChainage.Add(OFFELEV);
//                        txtXYZ.Add(x.ToString() + "," + y.ToString() + "," + z.ToString() + "," + off.ToString() + "," + sta.ToString());
//                    }
//                }
//                else
//                {
//                    continue;
//                }
//            }

//        }
//        catch (System.Exception ex)
//        {
//            ed.WriteMessage("\n" + ex.Message);
//            Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
//        }
//        trans.Commit();
//    }
//}
//private void btnselALG_Click(object sender, EventArgs e)
//{
//    //LSbox2.Items.Clear();
//    AlignmentsList.Items.Clear();
//    //------------------------------------------------
//    Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
//    CivilDocument civildoc = CivilApplication.ActiveDocument;
//    Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
//    //------------------------------------------------
//    PromptEntityOptions opt2 = new PromptEntityOptions("\nSelect an Alignment");
//    opt2.SetRejectMessage("\nObject must be an alignment.");
//    opt2.AddAllowedClass(typeof(Alignment), false);
//    ObjectId alignID2 = ed.GetEntity(opt2).ObjectId;
//    //------------------------------------------------
//    using (Transaction trans = db.TransactionManager.StartTransaction())
//    {
//        try
//        {
//            Alignment align2 = trans.GetObject(alignID2, OpenMode.ForRead) as Alignment;
//            //------------------------------------------------
//            AlignmentsList.Items.Clear();
//            AlignmentsList.Items.Add(align2.Name);
//            AlignmentsList.SelectedIndex = 0;
//            //------------------------------------------------
//            if (align2.GetProfileIds() == null) return;
//            ObjectIdCollection pfobj22 = align2.GetProfileIds();
//            List<string> pflist77 = new List<string>();
//            for (int i = 0; i < pfobj22.Count; i++)
//            {
//                Profile pf7 = trans.GetObject(align2.GetProfileIds()[i], OpenMode.ForWrite) as Profile;
//                pflist77.Add(pf7.Name);
//            }
//            //LSbox2.Items.Clear();
//            //LSbox2.Items.AddRange(pflist77.ToArray());
//        }
//        catch (System.Exception ex)
//        {
//            ed.WriteMessage("\n" + ex.Message);
//        }
//        trans.Commit();
//    }
//}