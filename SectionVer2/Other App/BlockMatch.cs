using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace Sections
{
    class BlockMatch
    {
        static public void DynamicBlocksmatch()
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Database;
            string name;
            System.Data.DataTable tb = new System.Data.DataTable();
            tb.Columns.Add("name", typeof(string));
            tb.Columns.Add("value", typeof(string));
            List<string> str = new List<string>();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    PromptEntityOptions op = new PromptEntityOptions("Select First Block");
                    PromptEntityResult se = ed.GetEntity(op);
                    if (se.Status == PromptStatus.Error) return;
                    BlockReference bl1 = tr.GetObject(se.ObjectId, OpenMode.ForRead) as BlockReference;
                    if (bl1 != null)
                    {
                        foreach (DynamicBlockReferenceProperty dy in bl1.DynamicBlockReferencePropertyCollection)
                        {
                            if (dy.PropertyName == "Origin") continue;
                            tb.Rows.Add(dy.PropertyName, dy.Value);
                            str.Add(dy.PropertyName);
                            str.Add(dy.Value.ToString());
                        }
                    }
                    DataView dv = new DataView(tb);
                    //select on screen  
                    PromptSelectionOptions op2 = new PromptSelectionOptions();
                    op2.MessageForAdding = "Select Other Blocks";
                    PromptSelectionResult se2 = ed.GetSelection(op2);
                    if (se2.Status == PromptStatus.Error) return;
                    SelectionSet Set2 = se2.Value;
                    ObjectId[] entId2 = Set2.GetObjectIds();
                    int s = 0;
                    double d = 0;
                    foreach (ObjectId entId1 in entId2)
                    {
                        s = 0;
                        BlockReference bl = tr.GetObject(entId1, OpenMode.ForWrite) as BlockReference;
                        if (bl != null)
                        {
                            foreach (DynamicBlockReferenceProperty dy in bl.DynamicBlockReferencePropertyCollection)
                            {
                                if (dy.PropertyName == "Origin")
                                {
                                    s++;
                                    continue;
                                }
                                for (int i = 0; i < dv.Count; i++)
                                {
                                    if (dy.PropertyName.ToString() == dv[i][0].ToString())
                                    {
                                        if (Double.TryParse(dv[i][1].ToString(), out d))
                                        {
                                            bl.DynamicBlockReferencePropertyCollection[s].Value = Convert.ToDouble(dv[i][1]);
                                        }
                                        else
                                            bl.DynamicBlockReferencePropertyCollection[s].Value = Convert.ToString(dv[i][1]);
                                    }
                                }
                                s++;
                            }
                        }
                    }

                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.ToString());
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ex.StackTrace);
                }
                tr.Commit();
            }
        }
    }
}
