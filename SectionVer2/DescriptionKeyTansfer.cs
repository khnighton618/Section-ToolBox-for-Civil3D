using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
namespace Sections
{
    public partial class DescriptionKeyTansfer : Form
    {
        public string[] lines;
        private StreamReader filereader;
        public List<string> polblst = new List<string>();
        public List<string> postyle = new List<string>();
        public List<string> codesfinal = new List<string>();
        Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
        Database db = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Database;
        CivilDocument civildoc = CivilApplication.ActiveDocument;       
        private StreamWriter filewriter;

        public void refreshlist()
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    PointDescriptionKeySetCollection pointDescKeySetColl = PointDescriptionKeySetCollection.GetPointDescriptionKeySets(db);
                    for (int i = 0; i < pointDescKeySetColl.Count; i++)
                    {
                        PointDescriptionKeySet pointDescKeySet = trans.GetObject(pointDescKeySetColl[i], OpenMode.ForWrite) as PointDescriptionKeySet;
                        listBox1.Items.Add(pointDescKeySet.Name);
                    }
                    listBox1.SelectedItem = 0;

                    trans.Commit();
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage(ex.Message);
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
            }
        }

        public DescriptionKeyTansfer()
        {
            InitializeComponent();
            exportstyle();
            refreshlist();
            listBox1.SelectedIndex = 0;

        }

        public void exportstyle()
        {                       
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    for (int i = 0; i < civildoc.Styles.PointStyles.Count; i++)
                    {
                        Autodesk.Civil.DatabaseServices.Styles.PointStyle po = trans.GetObject(civildoc.Styles.PointStyles[i], OpenMode.ForRead) as Autodesk.Civil.DatabaseServices.Styles.PointStyle;
                        postyle.Add(po.Name);
                    }
                    
                    for (int i = 0; i < civildoc.Styles.LabelStyles.PointLabelStyles.LabelStyles.Count; i++)
                    {

                        ObjectId obpoid = civildoc.Styles.LabelStyles.PointLabelStyles.LabelStyles[i];
                        Autodesk.Civil.DatabaseServices.Styles.LabelStyle oLabelStyle = trans.GetObject(obpoid, OpenMode.ForRead) as Autodesk.Civil.DatabaseServices.Styles.LabelStyle;
                        polblst.Add(oLabelStyle.Name);
                    }                   
                    
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage(ex.Message);
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
            }
        }

        private void importDescKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {

            List<string> CodesImported = new List<string>();
            List<string> PointStyleImported = new List<string>();
            List<string> PointlblStyleImported = new List<string>();
            dataGridView2.Rows.Clear();
            DescKeySetName.Text = "";
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
                        lines = whole_file.Split(new char[] { '\r', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);                       
                        for (int i = 0; i < lines.Length; i=i+3)
                        {
                            dataGridView2.Rows.Add(lines[i], lines[i+1], lines[i+2]);
                            CodesImported.Add(lines[i]);
                            PointStyleImported.Add(lines[i + 1]);
                            PointlblStyleImported.Add(lines[i + 2]);
                        }
                        for (int i = 0; i < PointStyleImported.Count; i++)
                        {
                            if (postyle.FindAll(x => x.Contains(PointStyleImported[i])).Count ==0)
                            {
                                postyle.Add(PointStyleImported[i]);
                                civildoc.Styles.PointStyles.Add(PointStyleImported[i]);
                            }

                            else
                                continue;                           
                        }
                        for (int i = 0; i < PointlblStyleImported.Count; i++)
                        {
                            if (polblst.FindAll(x => x.Contains(PointlblStyleImported[i])).Count == 0)
                            {
                                polblst.Add(PointlblStyleImported[i]);
                                civildoc.Styles.LabelStyles.PointLabelStyles.LabelStyles.Add(PointlblStyleImported[i]);
                            }
                            else
                                continue;
                        }
                        ComboBox CB = new ComboBox();
                        CB.Items.AddRange(postyle.ToArray());
                        ((DataGridViewComboBoxColumn)dataGridView2.Columns[1]).DataSource = CB.Items;
                        ComboBox CB2 = new ComboBox();
                        CB2.Items.AddRange(polblst.ToArray());
                        ((DataGridViewComboBoxColumn)dataGridView2.Columns[2]).DataSource = CB2.Items;
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
                Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
            }
        }                      

        private void BTNCreate_Click(object sender, EventArgs e)
        {
            string fileExp = "";
            string Codes = "";
            string PointStyle = "";
            string pointlabelstyle = "";
            List<string> descCodes = new List<string>();
            List<string> expo = new List<string>();
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    PointDescriptionKeySetCollection pointDescKeySetColl = PointDescriptionKeySetCollection.GetPointDescriptionKeySets(db);
                    PointDescriptionKeySet pointDescKeySet = trans.GetObject(pointDescKeySetColl[listBox1.SelectedIndex], OpenMode.ForWrite) as PointDescriptionKeySet;                    
                    ObjectIdCollection descOID2 = pointDescKeySet.GetPointDescriptionKeyIds();
                    foreach (ObjectId descOID in descOID2)
                    {
                        PointDescriptionKey po = trans.GetObject(descOID, OpenMode.ForWrite) as PointDescriptionKey;

                        if (po.Code.Contains("*"))
                            Codes = po.Code.Remove(po.Code.Length - 1);
                        else
                            Codes = po.Code;
                        Autodesk.Civil.DatabaseServices.Styles.LabelStyle lbl = trans.GetObject(po.LabelStyleId, OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Styles.LabelStyle;
                        pointlabelstyle = lbl.Name;
                        Autodesk.Civil.DatabaseServices.Styles.PointStyle post = trans.GetObject(po.StyleId, OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Styles.PointStyle;
                        PointStyle = post.Name;
                        fileExp = Codes + "," + PointStyle + "," + pointlabelstyle;
                        expo.Add(fileExp);
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage(ex.Message);
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
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
                    for (int i = 0; i < expo.Count; i++)
                    {
                        filewriter.WriteLine(expo[i]);
                    }
                    filewriter.Close();
                }
                catch (IOException)
                {
                    MessageBox.Show("Error writing to file", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            string Codes = "";
            string PointStyle = "";
            string pointlabelstyle = "";            
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    
                     PointDescriptionKeySetCollection pointDescKeySetColl = PointDescriptionKeySetCollection.GetPointDescriptionKeySets(db);
                    PointDescriptionKeySet pointDescKeySet = trans.GetObject(pointDescKeySetColl[listBox1.SelectedIndex], OpenMode.ForWrite) as PointDescriptionKeySet;

                    ObjectIdCollection descOID2 = pointDescKeySet.GetPointDescriptionKeyIds();
                    foreach (ObjectId descOID in descOID2)
                    {
                        PointDescriptionKey po = trans.GetObject(descOID, OpenMode.ForWrite) as PointDescriptionKey;
                        if (po.Code.Contains("*"))
                            Codes = po.Code.Remove(po.Code.Length - 1);
                        else
                            Codes = po.Code;
                        if (po.LabelStyleId.IsNull || po.StyleId.IsNull)
                        {
                            pointlabelstyle = "Default";
                            PointStyle = "Default";
                        }
                        else
                        {
                            Autodesk.Civil.DatabaseServices.Styles.LabelStyle lbl = trans.GetObject(po.LabelStyleId, OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Styles.LabelStyle;
                            pointlabelstyle = lbl.Name;

                            Autodesk.Civil.DatabaseServices.Styles.PointStyle post = trans.GetObject(po.StyleId, OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Styles.PointStyle;
                            PointStyle = post.Name;
                        }
                            
                        dataGridView1.Rows.Add(Codes, PointStyle, pointlabelstyle);
                    }
                    
                     
                    trans.Commit();
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage(ex.Message);
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }

            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sections.About_Description_Key win = new About_Description_Key();
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(win);
        }

        private void BTNPaste_Click(object sender, EventArgs e)
        {
            DataObject o = (DataObject)Clipboard.GetDataObject();
            if (o.GetDataPresent(DataFormats.Text))
            {
                if (dataGridView2.RowCount > 0)
                    dataGridView2.Rows.Clear();

                if (dataGridView2.ColumnCount > 0)
                    dataGridView2.Columns.Clear();

                bool columnsAdded = false;
                string[] pastedRows = System.Text.RegularExpressions.Regex.Split(o.GetData(DataFormats.Text).ToString().TrimEnd("\r\n".ToCharArray()), "\r\n");
                foreach (string pastedRow in pastedRows)
                {
                    string[] pastedRowCells = pastedRow.Split(new char[] { '\t' });

                    if (!columnsAdded)
                    {
                        for (int i = 0; i < pastedRowCells.Length; i++)
                            dataGridView2.Columns.Add("col" + i, pastedRowCells[i]);

                        columnsAdded = true;
                        continue;
                    }

                    dataGridView2.Rows.Add();
                    int myRowIndex = dataGridView2.Rows.Count - 1;

                    using (DataGridViewRow myDataGridViewRow = dataGridView2.Rows[myRowIndex])
                    {
                        for (int i = 0; i < pastedRowCells.Length; i++)
                            myDataGridViewRow.Cells[i].Value = pastedRowCells[i];
                    }
                }
            }
        }

        private void BTNCreateDesc_Click(object sender, EventArgs e)
        {
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    PointDescriptionKeySetCollection pointDescKeySetColl = PointDescriptionKeySetCollection.GetPointDescriptionKeySets(db);
                    PointDescriptionKeySet pointDescKeySet = trans.GetObject(pointDescKeySetColl[0], OpenMode.ForWrite) as PointDescriptionKeySet;
                    // Add a new Descp. Key Sets
                    if (DescKeySetName.Text != "")
                    {
                        ObjectId PointDescKeySetsId = pointDescKeySetColl.Add(DescKeySetName.Text);
                        pointDescKeySet = trans.GetObject(PointDescKeySetsId, OpenMode.ForWrite) as PointDescriptionKeySet;
                    }
                    List<string> descCodes = new List<string>();
                    ObjectIdCollection descOID2 = pointDescKeySet.GetPointDescriptionKeyIds();
                    foreach (ObjectId descOID in descOID2)
                    {
                        PointDescriptionKey po = trans.GetObject(descOID, OpenMode.ForWrite) as PointDescriptionKey;
                        if (po.Code.Contains("*"))
                            descCodes.Add(po.Code.Remove(po.Code.Length - 1));
                        else
                            descCodes.Add(po.Code);
                    }

                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        if (dataGridView2.Rows[i].Cells[0].Value == null) continue;
                        if (descCodes.FindAll(x => x.Contains(dataGridView2.Rows[i].Cells[0].Value.ToString())).Count >= 1) continue;
                        ObjectId pointDescKeyId = pointDescKeySet.Add(Convert.ToString(dataGridView2.Rows[i].Cells[0].Value.ToString()));
                        PointDescriptionKey pointDescKey = trans.GetObject(pointDescKeyId, OpenMode.ForWrite) as PointDescriptionKey;
                        pointDescKey.StyleId = civildoc.Styles.PointStyles[dataGridView2.Rows[i].Cells[1].Value.ToString()];
                        pointDescKey.ApplyStyleId = true;
                        pointDescKey.LabelStyleId = civildoc.Styles.LabelStyles.PointLabelStyles.LabelStyles[dataGridView2.Rows[i].Cells[2].Value.ToString()];
                        pointDescKey.ApplyLabelStyleId = true;
                    }
                    trans.Commit();
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage(ex.Message);
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
            }
            listBox1.Items.Clear();
            refreshlist();
            listBox1.SelectedIndex = 0;
        }
    }
}



//private void CopyClipboard()
//{
//    DataObject d = dataGridView2.GetClipboardContent();
//    Clipboard.SetDataObject(d);
//}

//private void pasteCtrlVToolStripMenuItem_Click(object sender, EventArgs e)
//{
//    PasteClipboard();
//}
//private void dgData_KeyDown(object sender, KeyEventArgs e)
//{
//    if ((e.Control && e.KeyCode == Keys.Delete) || (e.Shift && e.KeyCode == Keys.Delete))
//    {
//        CopyClipboard();
//    }
//    if ((e.Control && e.KeyCode == Keys.Insert) || (e.Shift && e.KeyCode == Keys.Insert))
//    {
//        PasteClipboard();
//    }

//}

////private void pasteCtrlVToolStripMenuItem_Click(object sender, EventArgs e)
////{
////    PasteClipboard();
////}

///// <summary>
///// This will be moved to the util class so it can service any paste into a DGV
///// </summary>
//private void PasteClipboard()
//{
//    try
//    {
//        string s = Clipboard.GetText();
//        string[] lines = s.Split('\n');
//        int iFail = 0, iRow = dataGridView2.CurrentCell.RowIndex;
//        int iCol = dataGridView2.CurrentCell.ColumnIndex;
//        DataGridViewCell oCell;
//        foreach (string line in lines)
//        {
//            if (iRow < dataGridView2.RowCount && line.Length > 0)
//            {
//                string[] sCells = line.Split('\t');
//                for (int i = 0; i < sCells.GetLength(0); ++i)
//                {
//                    if (iCol + i < this.dataGridView2.ColumnCount)
//                    {
//                        oCell = dataGridView2[iCol + i, iRow];
//                        if (!oCell.ReadOnly)
//                        {
//                            if (oCell.Value.ToString() != sCells[i])
//                            {
//                                oCell.Value = Convert.ChangeType(sCells[i], oCell.ValueType);
//                                oCell.Style.BackColor = Color.Tomato;
//                            }
//                            else
//                                iFail++;//only traps a fail if the data has changed and you are pasting into a read only cell
//                        }
//                    }
//                    else
//                    { break; }
//                }
//                iRow++;
//            }
//            else
//            { break; }
//            if (iFail > 0)
//                MessageBox.Show(string.Format("{0} updates failed due to read only column setting", iFail));
//        }
//    }
//    catch (FormatException)
//    {
//        MessageBox.Show("The data you pasted is in the wrong format for the cell");
//        return;
//    }
//}



//using (Transaction trans = db.TransactionManager.StartTransaction())
//{
//    try
//    {
//        PointDescriptionKeySetCollection pointDescKeySetColl = PointDescriptionKeySetCollection.GetPointDescriptionKeySets(db);
//        PointDescriptionKeySet pointDescKeySet = trans.GetObject(pointDescKeySetColl[0], OpenMode.ForWrite) as PointDescriptionKeySet;
//        // Add a new Descp. Key Sets
//        if (checkBox1.CheckState == CheckState.Checked)
//        {
//            ObjectId PointDescKeySetsId = pointDescKeySetColl.Add(DescKeySetName.Text);
//            pointDescKeySet = trans.GetObject(PointDescKeySetsId, OpenMode.ForWrite) as PointDescriptionKeySet;
//        }
//        List<string> descCodes = new List<string>();
//        ObjectIdCollection descOID2 = pointDescKeySet.GetPointDescriptionKeyIds();
//        foreach (ObjectId descOID in descOID2)
//        {
//            PointDescriptionKey po = trans.GetObject(descOID, OpenMode.ForWrite) as PointDescriptionKey;
//            if (po.Code.Contains("*"))
//                descCodes.Add(po.Code.Remove(po.Code.Length - 1));
//            else
//                descCodes.Add(po.Code);
//        }
//        for (int i = 0; i < lines.Length; i = i + 3)
//        {
//            if (descCodes.FindAll(x => x.Contains(lines[i])).Count >= 1) continue;
//            ObjectId pointDescKeyId = pointDescKeySet.Add(lines[i]);
//            PointDescriptionKey pointDescKey = trans.GetObject(pointDescKeyId, OpenMode.ForWrite) as PointDescriptionKey;
//            pointDescKey.StyleId = civildoc.Styles.PointStyles[lines[i + 1]];
//            pointDescKey.ApplyStyleId = true;
//            pointDescKey.LabelStyleId = civildoc.Styles.LabelStyles.PointLabelStyles.LabelStyles[lines[i + 2]];
//            pointDescKey.ApplyLabelStyleId = true;
//        }
//        trans.Commit();
//    }
//    catch (System.Exception ex)
//    {
//        ed.WriteMessage(ex.Message);
//        Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
//    }
//}