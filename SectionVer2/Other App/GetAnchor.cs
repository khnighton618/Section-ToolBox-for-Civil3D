using System;
using System.Collections.Generic;
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
    class GetAnchor1
    {
        static public List<DBObject> FullExplode(Entity ent)

        {

            // final result

            List<DBObject> fullList = new List<DBObject>();



            // explode the entity

            DBObjectCollection explodedObjects = new DBObjectCollection();

            ent.Explode(explodedObjects);

            foreach (Entity explodedObj in explodedObjects)

            {

                // if the exploded entity is a blockref or mtext

                // then explode again

                if (explodedObj.GetType() == typeof(BlockReference) ||

                    explodedObj.GetType() == typeof(MText))

                {

                    fullList.AddRange(FullExplode(explodedObj));

                }

                else

                    fullList.Add(explodedObj);

            }

            return fullList;

        }
        static public void labelprop2()
        {
            Autodesk.Civil.ApplicationServices.CivilDocument civildoc = Autodesk.Civil.ApplicationServices.CivilApplication.ActiveDocument;
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {

                    PromptSelectionResult se = ed.GetSelection();
                    if (se.Status == PromptStatus.Error) return;
                    SelectionSet Set1 = se.Value;
                    ObjectId[] ids1 = Set1.GetObjectIds();
                    List<string> txt = new List<string>();
                    foreach (ObjectId entId1 in ids1)
                    {
                        Autodesk.Civil.DatabaseServices.Label lbl = tr.GetObject(entId1, OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Label;
                        if (lbl != null)
                        {
                            string entityText = "";
                            using (Transaction trans = db.TransactionManager.StartTransaction())
                            {
                                // open the entity
                                Entity point = trans.GetObject(entId1, OpenMode.ForRead) as Entity;
                                // do a full explode (considering explode again
                                // all BlockReferences and MText)
                                List<DBObject> objs = FullExplode(point);
                                foreach (Entity ent in objs)
                                {
                                    // now get the text of each DBText
                                    if (ent.GetType() == typeof(DBText))
                                    {
                                        DBText text = ent as DBText;
                                        entityText = entityText + text.TextString;
                                    }
                                }
                                trans.Commit();
                            }
                            Autodesk.Civil.DatabaseServices.AnchorInfo anc = lbl.AnchorInfo;
                            Autodesk.Aec.DatabaseServices.Override over = new Autodesk.Aec.DatabaseServices.Override();


                            Point3d po = new Point3d(anc.Location.X, anc.Location.Y, anc.Location.Z);
                            Point3dCollection pocol = new Point3dCollection();
                            IntegerCollection intcol = new IntegerCollection();
                            IntegerCollection intcol2 = new IntegerCollection();
                            intcol.Add(1);

                            //lbl.GetGripPoints(pocol, intcol, intcol2);
                            txt.Add(entityText.ToString());
                            txt.Add("Anchor Location: " + po.X.ToString() + "," + po.Y.ToString() + "," + po.Z.ToString());
                            Document acDoc = Application.DocumentManager.MdiActiveDocument;
                            Database acCurDb = acDoc.Database;

                            // Start a transaction
                            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                            {
                                // Open the Block table for read
                                BlockTable acBlkTbl;
                                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                             OpenMode.ForRead) as BlockTable;

                                // Open the Block table record Model space for write
                                BlockTableRecord acBlkTblRec;
                                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                                OpenMode.ForWrite) as BlockTableRecord;

                                // Create a point at (4, 3, 0) in Model space
                                DBPoint acPoint = new DBPoint(po);

                                acPoint.SetDatabaseDefaults();

                                // Add the new object to the block table record and the transaction
                                acBlkTblRec.AppendEntity(acPoint);
                                acTrans.AddNewlyCreatedDBObject(acPoint, true);

                                // Set the style for all point objects in the drawing


                                // Save the new object to the database
                                acTrans.Commit();
                            }

                        }
                    }

                    System.IO.StreamWriter filewriter;
                    string filename;
                    try
                    {
                        string f1;
                        f1 = "C://anchor.txt";
                        System.IO.FileStream inputEG = new System.IO.FileStream(f1, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                        filewriter = new System.IO.StreamWriter(inputEG);
                        for (int i = 0; i < txt.Count; i++)
                        {
                            filewriter.WriteLine(txt[i]);
                        }
                        filewriter.Close();
                    }
                    catch (System.IO.IOException)
                    {
                        System.Windows.Forms.MessageBox.Show("Error writing to file", "File error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }


                    PromptResult exp = ed.GetString("Override Text:");
                    foreach (ObjectId entId1 in ids1)
                    {
                        Autodesk.Civil.DatabaseServices.Label lbl = tr.GetObject(entId1, OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Label;
                        string exp1 = "";
                        string exp2 = "";
                        int exprID = 0;
                        foreach (ObjectId oid in lbl.GetTextComponentIds())
                        {
                            exp1 = lbl.GetTextComponentOverride(oid);
                            if (exp1.Contains("Length"))
                                break;
                            exprID++;
                        }
                        exp2 = exp1 + exp.StringResult;
                        lbl.SetTextComponentOverride(lbl.GetTextComponentIds()[exprID], exp2);
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
        static public void labelprop1()
        {
            Autodesk.Civil.ApplicationServices.CivilDocument civildoc = Autodesk.Civil.ApplicationServices.CivilApplication.ActiveDocument;
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                try
                {

                    PromptSelectionResult se = ed.GetSelection();
                    if (se.Status == PromptStatus.Error) return;
                    SelectionSet Set1 = se.Value;
                    ObjectId[] ids1 = Set1.GetObjectIds();
                    List<string> txt = new List<string>();
                    foreach (ObjectId entId1 in ids1)
                    {
                        Autodesk.Civil.DatabaseServices.Label lbl = tr.GetObject(entId1, OpenMode.ForWrite) as Autodesk.Civil.DatabaseServices.Label;
                        if (lbl != null)
                        {
                            string entityText = "";
                            using (Transaction trans = db.TransactionManager.StartTransaction())
                            {
                                // open the entity
                                Entity point = trans.GetObject(entId1, OpenMode.ForRead) as Entity;
                                // do a full explode (considering explode again
                                // all BlockReferences and MText)
                                List<DBObject> objs = FullExplode(point);
                                foreach (Entity ent in objs)
                                {
                                    // now get the text of each DBText
                                    if (ent.GetType() == typeof(DBText))
                                    {
                                        DBText text = ent as DBText;
                                        entityText = entityText + text.TextString;
                                    }
                                }
                                trans.Commit();
                            }
                            Autodesk.Civil.DatabaseServices.AnchorInfo anc = lbl.AnchorInfo;
                            Autodesk.Aec.DatabaseServices.Override over = new Autodesk.Aec.DatabaseServices.Override();


                            Point3d po = new Point3d(anc.Location.X, anc.Location.Y, anc.Location.Z);
                            Point3dCollection pocol = new Point3dCollection();
                            IntegerCollection intcol = new IntegerCollection();
                            IntegerCollection intcol2 = new IntegerCollection();
                            intcol.Add(1);

                            //lbl.GetGripPoints(pocol, intcol, intcol2);
                            txt.Add(entityText.ToString());
                            txt.Add("Anchor Location: " + po.X.ToString() + "," + po.Y.ToString() + "," + po.Z.ToString());
                            Document acDoc = Application.DocumentManager.MdiActiveDocument;
                            Database acCurDb = acDoc.Database;

                            // Start a transaction
                            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                            {
                                // Open the Block table for read
                                BlockTable acBlkTbl;
                                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                             OpenMode.ForRead) as BlockTable;

                                // Open the Block table record Model space for write
                                BlockTableRecord acBlkTblRec;
                                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                                OpenMode.ForWrite) as BlockTableRecord;

                                // Create a point at (4, 3, 0) in Model space
                                DBPoint acPoint = new DBPoint(po);

                                acPoint.SetDatabaseDefaults();

                                // Add the new object to the block table record and the transaction
                                acBlkTblRec.AppendEntity(acPoint);
                                acTrans.AddNewlyCreatedDBObject(acPoint, true);

                                // Set the style for all point objects in the drawing


                                // Save the new object to the database
                                acTrans.Commit();
                            }

                        }
                    }

                    System.IO.StreamWriter filewriter;
                    string filename;
                    try
                    {
                        string f1;
                        f1 = "C://anchor.txt";
                        System.IO.FileStream inputEG = new System.IO.FileStream(f1, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                        filewriter = new System.IO.StreamWriter(inputEG);
                        for (int i = 0; i < txt.Count; i++)
                        {
                            filewriter.WriteLine(txt[i]);
                        }
                        filewriter.Close();
                    }
                    catch (System.IO.IOException)
                    {
                        System.Windows.Forms.MessageBox.Show("Error writing to file", "File error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
