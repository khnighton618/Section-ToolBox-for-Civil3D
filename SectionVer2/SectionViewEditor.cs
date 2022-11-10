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
using System.Drawing;

namespace Sections
{
    public partial class SectionViewEditor : Form
    {
        Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
        CivilDocument civildoc = CivilApplication.ActiveDocument;
        Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
        public Alignment align;
        System.Data.DataTable tb1 = new System.Data.DataTable();
        public SampleLineGroup slg = null;
        public int idsvg = 0;
        Stopwatch elaptime = new Stopwatch();

        public SectionViewEditor()
        {
            InitializeComponent();
            LS_SLG.Items.Clear();
            LS_Align.Items.Clear();
            tb1.Columns.Add("name", typeof(string));
            tb1.Columns.Add("sta", typeof(double));
            tb1.Columns.Add("zmax", typeof(double));
            tb1.Columns.Add("zmin", typeof(double));
            tb1.Columns.Add("L", typeof(double));
            tb1.Columns.Add("R", typeof(double));
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    
                    foreach (ObjectId objId in civildoc.GetAlignmentIds())
                    {

                        Alignment al = trans.GetObject(objId, OpenMode.ForWrite) as Alignment;                        
                        LS_Align.Items.Add(al.Name);
                    }
                    LS_Align.SelectedIndex = 0;


                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                    ErrorNOStripStatus.Text = "Errors: " + ex.Message;
                }
                trans.Commit();
            }
        }

        private void LS_Align_SelectedIndexChanged(object sender, EventArgs e)
        {
            object selalg = LS_Align.SelectedItem;
            LS_SLG.Items.Clear();
            LS_SVG.Items.Clear();
            tb1.Clear();
            
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
                         slg = trans.GetObject(slgID, OpenMode.ForWrite) as SampleLineGroup;
                        LS_SLG.Items.Add(slg.Name);                        
                    }
                    LS_SLG.SelectedIndex = 0;
                    
                    for (int i=0;i< slg.SectionViewGroups.Count;i++)
                    {                       
                        LS_SVG.Items.Add(slg.SectionViewGroups[i].Name);
                    }
                    LS_SVG.SelectedIndex = 0;
                    



                }
                catch (System.Exception ex)
                {                    
                    ed.WriteMessage("\n" + ex.Message);
                    ErrorNOStripStatus.Text = "Errors: " + ex.Message;
                }
                trans.Commit();
            }
        }

        private void ApplyBTN_Click(object sender, EventArgs e)
        {
            string s = Clipboard.GetText();

            string[] lines = s.Replace("\n", "").Split('\r');
            DVG1.Rows.Clear();
            DVG1.Rows.Add(lines.Length - 1);
            string[] fields;
            int row = 0;
            int col = 0;

            foreach (string item in lines)
            {
                fields = item.Split('\t');
                foreach (string f in fields)
                {
                    Console.WriteLine(f);
                    DVG1[col, row].Value = f;
                    col++;
                }
                row++;
                col = 0;
            }

        }       

        private void DVG1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataView dv1 = new DataView(tb1);
            SectionView sec = null;
            if(DVG1.SelectedCells.Count!=0)
            {
                int col = DVG1.CurrentCell.ColumnIndex;
                int row = DVG1.CurrentCell.RowIndex;
                //int row = ((DataGrid)sender).ItemContainerGenerator.IndexFromContainer(row1);
                //int col= col1.DisplayIndex;

                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    try
                    {
                        SampleLine osam = trans.GetObject(slg.GetSampleLineIds()[row], OpenMode.ForWrite) as SampleLine;
                        
                        sec = trans.GetObject(osam.GetSectionViewIds()[0], OpenMode.ForWrite) as SectionView;
                        sec.IsOffsetRangeAutomatic = false;
                        sec.IsElevationRangeAutomatic = false;
                        if (col == 2)
                            sec.ElevationMin = Convert.ToDouble(DVG1.CurrentCell.Value.ToString());
                        else if (col == 3)
                            sec.ElevationMax = Convert.ToDouble(DVG1.CurrentCell.Value.ToString());
                        else if (col == 4)
                            sec.OffsetLeft = Convert.ToDouble(DVG1.CurrentCell.Value.ToString());
                        else if (col == 5)
                            sec.OffsetRight = Convert.ToDouble(DVG1.CurrentCell.Value.ToString());

                    }
                    catch (System.Exception ex)
                    {
                        ed.WriteMessage("\n" + ex.Message);
                        ErrorNOStripStatus.Text = "Errors: " + ex.Message;
                    }
                    trans.Commit();
                }

            }
            
        }

        private void DrawBTN_Click(object sender, EventArgs e)
        {
            int col = Convert.ToInt32(TXTCol.Text);
            int row = Convert.ToInt32(textRow.Text);
            double sp = Convert.ToDouble(TXTSpace.Text);
            int sr = 0;
            int sc = 0;
            int s = 0;
            double a = Convert.ToDouble(TXTA.Text);
            double b = Convert.ToDouble(TXTB.Text);
            double c = Convert.ToDouble(TXTC.Text);
            double d = Convert.ToDouble(TXTD.Text);
            double h = Convert.ToDouble(TXTE.Text);
            double x;
            double y;
            double xf;
            double yf;
            double x0 = 0;
            double y0 = 0;
            List<double> maxX = new List<double>();
            List<double> maxY = new List<double>();
            System.Data.DataTable tb = new System.Data.DataTable();
            tb.Columns.Add("x", typeof(double));
            tb.Columns.Add("y", typeof(double));
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    SectionView sv = null;
                    SampleLine osam0 = trans.GetObject(slg.GetSampleLineIds()[0], OpenMode.ForWrite) as SampleLine;
                    sv = trans.GetObject(osam0.GetSectionViewIds()[idsvg], OpenMode.ForWrite) as SectionView;
                    SectionOverride overrideObj0 = sv.GraphOverrides[0];
                    overrideObj0.Draw = true;
                    x0 = sv.Location.X;
                    y0 = sv.Location.Y;
                    int NO = 0;
                    for (int i = 0; i < slg.GetSampleLineIds().Count-1; i++)
                    {
                        if (NO >= slg.GetSampleLineIds().Count) break;
                        for (int j = 0; j < row; j++)
                        {
                            if (NO >= slg.GetSampleLineIds().Count) break;
                            osam0 = trans.GetObject(slg.GetSampleLineIds()[NO], OpenMode.ForWrite) as SampleLine;
                            try
                            {
                                sv = trans.GetObject(osam0.GetSectionViewIds()[idsvg], OpenMode.ForWrite) as SectionView;

                            }
                            catch
                            {
                                continue;
                            }
                            
                            //maxX.Add(x0 + i * (sv.OffsetRight - sv.OffsetLeft + b + d));
                            //maxY.Add(y0 + j * (sv.ElevationMax - sv.ElevationMin + a + c + h));
                            //if (tb.Rows.Count < (row * col))
                            //    sp = 0;
                            //else
                            //{
                            //    sp = Convert.ToDouble(TXTSpace.Text);
                            //}
                            if(chkbox.CheckState == CheckState.Unchecked)
                            {
                                x = x0 + i * (sv.OffsetRight - sv.OffsetLeft + b + d) + sp* (tb.Rows.Count / (row * col));
                                y = y0 + j * (sv.ElevationMax - sv.ElevationMin + a + c + h);
                            }
                            else
                            {
                                y = x0 + i * (sv.OffsetRight - sv.OffsetLeft + b + d) + sp;
                                x = y0 + j * (sv.ElevationMax - sv.ElevationMin + a + c + h);

                            }
                            maxX.Add(x);
                            maxY.Add(y);
                            tb.Rows.Add(x, y);
                            NO++;
                        }
                        
                    }
                    DataView dv = new DataView(tb);
                    double z = 0;
                    s = 0;
                    foreach (ObjectId osamID in slg.GetSampleLineIds())
                    {


                        SampleLine osam = trans.GetObject(osamID, OpenMode.ForWrite) as SampleLine;
                        sv = trans.GetObject(osam.GetSectionViewIds()[idsvg], OpenMode.ForWrite) as SectionView;
                        SectionOverride overrideObj = sv.GraphOverrides[0];
                        overrideObj.Draw = true;
                        x = (double)dv[s][0]-sv.Location.X;
                        y = (double)dv[s][1]-sv.Location.Y;
                        Vector3d v = new Vector3d(x, y,0);
                        Matrix3d m3d = Matrix3d.Displacement(v);
                        //m3d.
                        sv.TransformBy(m3d);
                        //sv.Location.Add(v);
                        s++;
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                    ErrorNOStripStatus.Text = "Errors: " + ex.Message;
                }
                trans.Commit();
            }
        }

        private void LS_SLG_SelectedIndexChanged(object sender, EventArgs e)
        {
            LS_SVG.Items.Clear();
            tb1.Clear();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    int index = 0;
                    foreach(int i in LS_SLG.Items)
                    {
                        if (slg.Name == LS_SLG.Items[i].ToString()) break;
                        index++;
                    }
                    //tb1.Columns.Add("name", typeof(string));
                    //tb1.Columns.Add("sta", typeof(double));
                    //tb1.Columns.Add("zmax", typeof(double));
                    //tb1.Columns.Add("zmin", typeof(double));
                    //tb1.Columns.Add("L", typeof(double));
                    //tb1.Columns.Add("R", typeof(double));
                    SectionView secvg = null;
                    foreach (ObjectId osamID in slg.GetSampleLineIds())
                    {
                        SampleLine osam = trans.GetObject(osamID, OpenMode.ForWrite) as SampleLine;

                        foreach (ObjectId sectionId in osam.GetSectionViewIds())
                        {
                            secvg = trans.GetObject(sectionId, OpenMode.ForWrite) as SectionView;

                        }
                        DVG1.Rows.Add(secvg.Name, osam.Station, secvg.ElevationMin, secvg.ElevationMax, secvg.OffsetLeft, secvg.OffsetRight);
                        tb1.Rows.Add(secvg.Name, osam.Station, secvg.ElevationMin, secvg.ElevationMax, secvg.OffsetLeft, secvg.OffsetRight);
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                    ErrorNOStripStatus.Text = "Errors: " + ex.Message;
                }
                trans.Commit();
            }
        }

        private void LS_SVG_SelectedIndexChanged(object sender, EventArgs e)
        {
            elaptime.Start();
            TimeElapseStripStatus.Text = "Elapsed Time: 00:00:00 ";
            ErrorNOStripStatus.Text = "Errors: 0";
            idsvg = 0;
            tb1.Clear();
            DVG1.Rows.Clear();
            int progbarID = 0;
            ProgBar.Maximum = slg.GetSampleLineIds().Count;
            ProgBar.Step = 1;// / ids.Count();
            ProgBar.Value = 0;
            object selsvg = LS_SVG.SelectedItem;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    for (int i = 0; i < slg.SectionViewGroups.Count; i++)
                    {
                        if (slg.SectionViewGroups[i].Name == selsvg.ToString())
                            break;
                        else
                            idsvg++;
                    }
                    SectionView secvg = null;
                    List<double> minz = new List<double>();
                    List<double> maxz = new List<double>();
                    foreach (ObjectId osamID in slg.GetSampleLineIds())
                    {
                        SampleLine osam = trans.GetObject(osamID, OpenMode.ForWrite) as SampleLine;
                        
                        try
                        {
                            secvg = trans.GetObject(osam.GetSectionViewIds()[idsvg], OpenMode.ForWrite) as SectionView;
                        }
                        catch
                        {
                            ed.WriteMessage("\n There is no Section View at Station: " + osam.Station.ToString());
                            continue;
                        }
                        Autodesk.Civil.DatabaseServices.Section sec2 = trans.GetObject(osam.GetSectionViewIds()[idsvg], OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Section;
                        DVG1.Rows.Add(secvg.Name, osam.Station, secvg.ElevationMin, secvg.ElevationMax, secvg.OffsetLeft, secvg.OffsetRight);
                        tb1.Rows.Add(secvg.Name, osam.Station, secvg.ElevationMin, secvg.ElevationMax, secvg.OffsetLeft, secvg.OffsetRight);
                        progbarID++;
                        ProgBar.Value = progbarID;
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                    ErrorNOStripStatus.Text = "Errors: " + ex.Message;
                }
                trans.Commit();
            }
            elaptime.Stop();
            TimeElapseStripStatus.Text = "Elapsed Time: " + elaptime.Elapsed.ToString().Remove(8);
            elaptime.Reset();

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sections.About_Section_Editor win1 = new Sections.About_Section_Editor();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModelessDialog(win1);
        }

        private void MinMaxBTN_Click(object sender, EventArgs e)
        {
            elaptime.Start();
            TimeElapseStripStatus.Text = "Elapsed Time: 00:00:00.0000000 ";
            ErrorNOStripStatus.Text = "Errors: 0";
            idsvg = 0;
            tb1.Clear();
            DVG1.Rows.Clear();
            int progbarID = 0;
            ProgBar.Maximum = slg.GetSampleLineIds().Count;
            ProgBar.Step = 1;// / ids.Count();
            ProgBar.Value = 0;
            object selsvg = LS_SVG.SelectedItem;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    for (int i = 0; i < slg.SectionViewGroups.Count; i++)
                    {
                        if (slg.SectionViewGroups[i].Name == selsvg.ToString())
                            break;
                        else
                            idsvg++;
                    }


                    SectionView secvg = null;

                    List<double> minz = new List<double>();
                    List<double> maxz = new List<double>();
                    foreach (ObjectId osamID in slg.GetSampleLineIds())
                    {
                        SampleLine osam = trans.GetObject(osamID, OpenMode.ForWrite) as SampleLine;
                        foreach (ObjectId osid in osam.GetSectionIds())
                        {
                            try
                            {
                                Autodesk.Civil.DatabaseServices.Section sec = trans.GetObject(osid, OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Section;
                                minz.Add(sec.MinmumElevation);
                                maxz.Add(sec.MaximumElevation);
                            }
                            catch
                            {
                                continue;
                            }
                        }
                        minz.Sort();
                        maxz.Sort();
                        try
                        {
                            secvg = trans.GetObject(osam.GetSectionViewIds()[idsvg], OpenMode.ForWrite) as SectionView;
                        }
                        catch
                        {
                            ed.WriteMessage("\n There is no Section View at Station: " + osam.Station.ToString());
                            continue;
                        }
                        double minztex = Convert.ToDouble(MinZBox.Text);
                        double maxztex = Convert.ToDouble(MaxZBox.Text);
                        SectionOverride overrideObj = secvg.GraphOverrides[0];
                        overrideObj.Draw = true;
                        DVG1.Rows.Add(secvg.Name, osam.Station, Math.Round(minz[0]) - minztex, Math.Round(maxz[maxz.Count - 1]) + maxztex, secvg.OffsetLeft, secvg.OffsetRight);
                        tb1.Rows.Add(secvg.Name, osam.Station, Math.Round(minz[0]) - minztex, Math.Round(maxz[maxz.Count - 1]) + maxztex, secvg.OffsetLeft, secvg.OffsetRight);
                        if (secvg.IsElevationRangeAutomatic)
                            secvg.IsElevationRangeAutomatic = false;
                        secvg.ElevationMin = Math.Round(minz[0]) - minztex;
                        secvg.ElevationMax = Math.Round(maxz[maxz.Count - 1]) + maxztex;
                        minz.Clear();
                        maxz.Clear();
                        progbarID++;
                        ProgBar.Value = progbarID;
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                    ErrorNOStripStatus.Text = "Errors: " + ex.Message;
                }
                trans.Commit();
            }
            elaptime.Stop();
            TimeElapseStripStatus.Text = "Elapsed Time: " + elaptime.Elapsed;
            elaptime.Reset();
        }

        private void PasteClipboard()
        {
            try
            {
                string s = Clipboard.GetText();
                char[] delim = { '\n',',','\t','\r' };
                string[] lines = s.Split(delim);
                int ss = 0;
                for (int i= 0; i < DVG1.RowCount; i++)
                {
                    DVG1[2, i].Value = lines[ss];
                    DVG1[3, i].Value = lines[ss + 1];
                    DVG1[4, i].Value = lines[ss + 2];
                    DVG1[5, i].Value = lines[ss + 3];
                    ss = ss + 5;
                }
                
                SectionView sec = null;
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    try
                    {

                        for (int i = 0; i < DVG1.RowCount; i++)
                        {
                            SampleLine osam = trans.GetObject(slg.GetSampleLineIds()[i], OpenMode.ForWrite) as SampleLine;
                            sec = trans.GetObject(osam.GetSectionViewIds()[0], OpenMode.ForWrite) as SectionView;
                            sec.IsOffsetRangeAutomatic = false;
                            sec.IsElevationRangeAutomatic = false;
                            sec.ElevationMin = Convert.ToDouble(DVG1[2, i].Value.ToString());
                            sec.ElevationMax = Convert.ToDouble(DVG1[3, i].Value.ToString());
                            sec.OffsetLeft = Convert.ToDouble(DVG1[4, i].Value.ToString());
                            sec.OffsetRight = Convert.ToDouble(DVG1[5, i].Value.ToString());
                        }
                    }
                    catch (System.Exception ex)
                    {
                        ed.WriteMessage("\n" + ex.Message);
                        ErrorNOStripStatus.Text = "Errors: " + ex.Message;
                    }
                    trans.Commit();
                }                
            }
            catch (FormatException)
            {
                MessageBox.Show("The data you pasted is in the wrong format for the cell");
                return;
            }

        }

        private void CopyClipboard()
        {
            DataObject d = DVG1.GetClipboardContent();
            Clipboard.SetDataObject(d);
        }

        private void DVG1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control && e.KeyCode == Keys.Delete) || (e.Shift && e.KeyCode == Keys.Delete))
            {
                CopyClipboard();
            }
            if ((e.Control && e.KeyCode == Keys.Insert) || (e.Shift && e.KeyCode == Keys.Insert))
            {
                PasteClipboard();
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteClipboard();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyClipboard();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void DVG1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int selcell = DVG1.SelectedRows[0].Index;
            double sta = Convert.ToDouble(DVG1[1, selcell].Value);
            //
            SampleLine osam = null;
            idsvg = 0;
            //tb1.Clear();
            //DVG1.Rows.Clear();            
            object selsvg = LS_SVG.SelectedItem;
            SectionView secvg = null;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    for (int i = 0; i < slg.SectionViewGroups.Count; i++)
                    {
                        if (slg.SectionViewGroups[i].Name == selsvg.ToString())
                            break;
                        else
                            idsvg++;
                    }                                       
                    foreach (ObjectId osamID in slg.GetSampleLineIds())
                    {                       
                        osam = trans.GetObject(osamID, OpenMode.ForWrite) as SampleLine;
                        if (osam.Station==sta)
                        {
                            try
                            {
                                secvg = trans.GetObject(osam.GetSectionViewIds()[idsvg], OpenMode.ForWrite) as SectionView;
                                propertyGrid1.SelectedObject = secvg;
                                
                            }
                            catch
                            {
                                ed.WriteMessage("\n There is no Section View at Station: " + osam.Station.ToString());
                                continue;
                            }
                        }              
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                    ErrorNOStripStatus.Text = "Errors: " + ex.Message;
                }
                trans.Commit();
            }
            
        }
    }
}
