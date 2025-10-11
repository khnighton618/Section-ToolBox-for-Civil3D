using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System.Windows.Forms.VisualStyles;
using Autodesk.AutoCAD.ApplicationServices;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Aec.DatabaseServices;
using ObjectId = Autodesk.AutoCAD.DatabaseServices.ObjectId;
using ObjectIdCollection = Autodesk.AutoCAD.DatabaseServices.ObjectIdCollection;
using BlockReference = Autodesk.AutoCAD.DatabaseServices.BlockReference;
using Autodesk.AutoCAD.Geometry;
using Microsoft.Office.Interop.Excel;

namespace SectionToolBox
{
    public partial class Flow_Path : Form
    {
        Document doc = Application.DocumentManager.MdiActiveDocument;
        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        Database db = Application.DocumentManager.MdiActiveDocument.Database;
        CivilDocument cvdoc = CivilApplication.ActiveDocument;
        string blockname = "";
        public Alignment align;
        BlockReference bl;
        Profile pf;
        public Flow_Path()
        {
            InitializeComponent();
            if (AllalgCHK.Checked)
            {
                comboBox1.Enabled = false;
                Select_Alignment_BTN.Enabled = false;
                LSboxProf.Enabled = false;
            }
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    ObjectIdCollection alIDs = cvdoc.GetAlignmentIds();
                    if (alIDs == null)
                    {
                        System.Windows.Forms.MessageBox.Show("You must have at least one alignment", "No Alignment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new System.Exception("There is no alignment");
                    }
                    int coalid = alIDs.Count;
                    List<string> alno = new List<string>();
                    align = null;
                    for (int i = 0; i < coalid; i++)
                    {
                        ObjectId alignID = alIDs[i];
                        align = trans.GetObject(alignID, OpenMode.ForRead) as Alignment;
                        if (align.AlignmentType == AlignmentType.Centerline)
                        {
                            alno.Add(align.Name);
                            comboBox1.Items.Add(align.Name);
                        }
                    }
                    comboBox1.SelectedIndex = 0;
                    LSboxProf.SetSelected(0, true);
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                }
                trans.Commit();
            }
        }

        private void BlockSelect_BTN_Click(object sender, EventArgs e)
        {
            PromptSelectionResult polsel = ed.GetSelection();
            double scale = 1;
            if (polsel.Status == PromptStatus.Error) return;
            SelectionSet BLockAR = polsel.Value;
            if (BLockAR != null)
            {
                ObjectId[] id = BLockAR.GetObjectIds();
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    bl = tr.GetObject(id[0], OpenMode.ForRead) as BlockReference;
                    if (bl != null)
                    {
                        blockname = bl.Name;
                    }
                    tr.Commit();
                }

                labelBlkName.Text = blockname;
            }
            else
                MessageBox.Show("Please Select Block Sample!");
        }

        private void Select_Alignment_BTN_Click(object sender, EventArgs e)
        {

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    PromptEntityOptions opt = new PromptEntityOptions("\nSelect an Alignment");
                    opt.SetRejectMessage("\nObject must be an alignment.");
                    opt.AddAllowedClass(typeof(Alignment), false);
                    ObjectId alignID = ed.GetEntity(opt).ObjectId;
                    align = trans.GetObject(alignID, OpenMode.ForRead) as Alignment;
                    comboBox1.SelectedText = align.Name;
                    LSboxProf.Items.Clear();
                    if (align.AlignmentType == AlignmentType.Centerline)
                    {
                        comboBox1.SelectedIndex = comboBox1.Items.IndexOf(align.Name);
                        //foreach (ObjectId id in align.GetProfileIds())
                        //{
                        //    pf = trans.GetObject(id, OpenMode.ForWrite) as Profile;
                        //    LSboxProf.Items.Add(pf.Name);
                        //}
                    }
                    else
                        MessageBox.Show("Select only CenterLine!");
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage(ex.Message);
                }
                trans.Commit();
            }
        }

        private void BTN_Create_Click(object sender, EventArgs e)
        {


            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    BlockTable acBlkTbl = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord acBlkTblRec = trans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    if (bl == null)
                        MessageBox.Show("Select block first!");
                    else
                    {
                        double x = 0;
                        double y = 0;
                        double z1 = 0;
                        double z2 = 0;
                        double ang = 0;
                        double bearing = 0;
                        double offset = Convert.ToDouble(TextOff.Text.ToString());
                        double sta = Convert.ToDouble(IntervalTXT.Text.ToString());
                        ObjectIdCollection alIDs = cvdoc.GetAlignmentIds();
                        if (AllalgCHK.Checked)
                        {
                            foreach (ObjectId alignID in alIDs)
                            {
                                align = trans.GetObject(alignID, OpenMode.ForRead) as Alignment;
                                if (align.AlignmentType == AlignmentType.Centerline)
                                {
                                    foreach (ObjectId id in align.GetProfileIds())
                                    {
                                        pf = trans.GetObject(id, OpenMode.ForWrite) as Profile;
                                        if (pf.Name.StartsWith("L"))
                                        {
                                            for (int i = (int)sta; i < pf.EndingStation; i = i + (int)sta)
                                            {
                                                BlockReference bl2 = bl.Clone() as BlockReference;
                                                align.PointLocation(i, offset, 0.1, ref x, ref y, ref bearing);
                                                z1 = pf.ElevationAt(i);
                                                z2 = pf.ElevationAt(i + .1);
                                                if (z1 < z2)
                                                    ang = 3 * Math.PI / 2 - bearing;
                                                else
                                                    ang = Math.PI / 2 - bearing;
                                                bl2.Position = new Point3d(x, y, 0);
                                                bl2.Rotation = ang;
                                                acBlkTblRec.AppendEntity(bl2);
                                                trans.AddNewlyCreatedDBObject(bl2, true);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (align != null & pf != null)
                            {
                                for (int i = (int)sta; i < pf.EndingStation; i = i + (int)sta)
                                {
                                    BlockReference bl2 = bl.Clone() as BlockReference;
                                    align.PointLocation(i, offset, 0.1, ref x, ref y, ref bearing);
                                    z1 = pf.ElevationAt(i);
                                    z2 = pf.ElevationAt(i + .1);
                                    if (z1 < z2)
                                        ang = 3 * Math.PI / 2 - bearing;
                                    else
                                        ang = Math.PI / 2 - bearing;
                                    bl2.Position = new Point3d(x, y, 0);
                                    bl2.Rotation = ang;
                                    acBlkTblRec.AppendEntity(bl2);
                                    trans.AddNewlyCreatedDBObject(bl2, true);
                                }
                            }
                        }
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
            LSboxProf.Items.Clear();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    Alignment al;
                    foreach (ObjectId ids in cvdoc.GetAlignmentIds())
                    {
                        al = trans.GetObject(ids, OpenMode.ForRead) as Alignment;
                        if (al.Name == comboBox1.SelectedItem.ToString())
                            align = al;
                    }
                    foreach (ObjectId id in align.GetProfileIds())
                    {
                        pf = trans.GetObject(id, OpenMode.ForWrite) as Profile;
                        LSboxProf.Items.Add(pf.Name);
                    }
                    LSboxProf.SetSelected(0, true);
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage(ex.Message);
                }
                trans.Commit();
            }
        }

        private void AllalgCHK_CheckedChanged(object sender, EventArgs e)
        {
            if (AllalgCHK.CheckState == CheckState.Unchecked)
            {
                comboBox1.Enabled = true;
                Select_Alignment_BTN.Enabled = true;
                LSboxProf.Enabled = true;
            }
        }

        private void LSboxProf_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
