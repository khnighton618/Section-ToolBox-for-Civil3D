using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;

using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;

namespace Sections
{
    public partial class ExportSection2Chainage : Form
    {
        public PromptSelectionResult SectionSelectionStes;
        public List<int> SelectionArray;
        private StreamWriter filewriter;
        Stopwatch elaptime = new Stopwatch();
        Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
        CivilDocument civildoc = CivilApplication.ActiveDocument;
        Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
        
        public ExportSection2Chainage()
        {
            
            InitializeComponent();
            comboBox1.Items.Clear();
            CHLBox1.Items.Clear();            
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
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
                trans.Commit();
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < CHLBox1.Items.Count; x++)
            {
                CHLBox1.SetItemChecked(x, true);
            }
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < CHLBox1.Items.Count; x++)
            {
                CHLBox1.SetItemChecked(x, false);
            }
        }

        private void btnSelSecs_Click(object sender, EventArgs e)
        {
            CHLBox1.Items.Clear();            
            PromptSelectionOptions selsec = new PromptSelectionOptions();
            selsec.AllowDuplicates = false;
            selsec.AllowSubSelections = true;
            PromptSelectionResult secres = ed.GetSelection(selsec);
            if (secres.Status != PromptStatus.OK) return;
            SectionSelectionStes = secres;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    System.Data.DataTable Table = new System.Data.DataTable();
                    Table.Columns.Add("Index", typeof(double));
                    Table.Columns.Add("IndexStation", typeof(double));
                    //Table.Columns.Add("Index", "Index Station");
                    //Table.Columns.Add("STATION", "Station");

                    for (int il = 0; il < secres.Value.Count; il++)
                    {
                        ObjectId secid = secres.Value[il].ObjectId;
                        Autodesk.Civil.DatabaseServices.Section secs = trans.GetObject(secid, OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Section;
                        if (secs != null)
                        {
                            Table.Rows.Add(il, secs.Station);
                            
                        }
                        
                    }
                    DataView dv = new DataView(Table);
                    dv.Sort = "IndexStation ASC";
                    //Table.Sort(Table.Columns[1], ListSortDirection.Ascending);


                    double[] sta = new double[secres.Value.Count];
                    int sum = 0;
                    
                    string header22 = "";
                    List<string> txt = new List<string>();
                    List<int> array3 = new List<int>();
                    
                    for (int ik = 0; ik < dv.Count; ik++)
                    {
                        
                        ObjectId secid = secres.Value[Convert.ToInt32(dv[ik][0])].ObjectId;
                        Autodesk.Civil.DatabaseServices.Section secs = trans.GetObject(secid, OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Section;

                        if (secs != null)
                        {
                            sum++;
                            SectionPointCollection secpo = secs.SectionPoints;
                            sta[ik] = secs.Station;

                            double staform = sta[ik] - Math.Floor(sta[ik] / 1000) * 1000;
                            //header22 = Convert.ToString(sum) + "   STA =" + Convert.ToString(Math.Floor(sta[i] / 1000)) + "+" + staform.ToString("F3") + "-" + secs.Name.ToString();

                            header22 = "STA =" + Convert.ToString(sta[ik]) + "--" + secs.Name.ToString();
                            txt.Add(Convert.ToString(header22));                           
                            array3.Add(ik);
                        }
                        
                    }
                    SelectionArray = array3;
                    //txt22.Sort()
                    for (int ij = 0; ij < txt.Count; ij++)
                    {
                        CHLBox1.Items.Add(txt[ij]);
                        //LSbox1.Items.Add(txt22[i]);
                    }
                    //CHLBox1.Sorted = true;
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
                trans.Commit();
            }
        }
        public void PFselID(ref int ALselID, ref PromptSelectionResult secress2, ref List<int> chksel, ref List<int> AR)
        {
            //PFselID = LSbox2.SelectedIndex;
            ALselID = comboBox1.SelectedIndex;
            secress2 = SectionSelectionStes;
            AR = SelectionArray;
            for (int i = 0; i < CHLBox1.CheckedIndices.Count; i++)
            {
                chksel.Add(SelectionArray[CHLBox1.CheckedIndices[i]]);
            }
        }
        public void saveText(ref List<string> txtChainage, ref List<string> txtXYZ)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    int alID = 0;
                    List<int> chsel = new List<int>();
                    List<int> arr = new List<int>();
                    PromptSelectionResult secres = null;
                    PFselID(ref alID, ref secres, ref chsel, ref arr);
                    if (chsel.Count <= 0)
                    {
                        MessageBox.Show("You must select Section", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new System.Exception("You must select Sections!!!");
                    }
                    ObjectId alignID = civildoc.GetAlignmentIds()[alID];
                    Alignment align = trans.GetObject(alignID, OpenMode.ForRead) as Alignment;
                    //-------------------------------------------
                    double sta = 0;
                    double off = 0;
                    double x = 0;
                    double y = 0;
                    double z = 0;


                    txtChainage = new List<string>();
                    foreach (int i in arr)
                    {
                        ObjectId secid = secres.Value[i].ObjectId;
                        Autodesk.Civil.DatabaseServices.Section secs = trans.GetObject(secid, OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Section;
                        if (secs != null)
                        {
                            SectionPointCollection secpo = secs.SectionPoints;
                            sta = secs.Station;
                            txtChainage.Add("Chainage=" + Convert.ToString(sta));
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
                            continue;
                        }

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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            elaptime.Start();
            TimeElapseStripStatus.Text = "Elapsed Time: 00:00:00 ";
            ProgBar.Value = 0;
            ErrorNOStripStatus.Text = "Errors: 0";
            List<string> txtChainage = new List<string>();
            List<string> txtXYZ = new List<string>();
            saveText(ref txtChainage, ref txtXYZ);
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
                    int progbarID = 0;
                    ProgBar.Maximum = txtChainage.ToArray().Length;
                    ProgBar.Step = 1;// / ids.Count();
                    ProgBar.Value = 0;
                    for (int i = 0; i < txtChainage.ToArray().Length; i++)
                    {
                        progbarID++;
                        ProgBar.Value = progbarID;// / ids.Count() * 1000;
                        filewriter.WriteLine(txtChainage[i]);
                    }
                    filewriter.Close();
                }

                catch (IOException)
                {
                    MessageBox.Show("Error writing to file", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            elaptime.Stop();
            TimeElapseStripStatus.Text = "Elapsed Time: " + elaptime.Elapsed.ToString().Remove(8);//.Hours+":"+elaptime.Elapsed.Minutes+":"+elaptime.Elapsed.Seconds;
            ErrorNOStripStatus.Text = "Errors: ";// + NOErr.ToString();
            elaptime.Reset();

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

     

        private void saveXYZSTAOFFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            elaptime.Start();
            TimeElapseStripStatus.Text = "Elapsed Time: 00:00:00.0000000 ";
            ProgBar.Value = 0;
            ErrorNOStripStatus.Text = "Errors: 0";
            List<string> txtChainage = new List<string>();
            List<string> txtXYZ = new List<string>();
            saveText(ref txtChainage, ref txtXYZ);
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
                    for (int i = 0; i < txtXYZ.ToArray().Length; i++)
                    {
                        filewriter.WriteLine(txtXYZ[i]);
                    }
                    filewriter.Close();
                }

                catch (IOException)
                {
                    MessageBox.Show("Error writing to file", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            elaptime.Stop();
            TimeElapseStripStatus.Text = "Elapsed Time: " + elaptime.Elapsed;//.Hours+":"+elaptime.Elapsed.Minutes+":"+elaptime.Elapsed.Seconds;
            ErrorNOStripStatus.Text = "Errors: ";// + NOErr.ToString();
            elaptime.Reset();

        }
    }
}
