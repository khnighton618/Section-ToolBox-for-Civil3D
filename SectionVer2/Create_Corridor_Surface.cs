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

namespace SectionToolBox
{
    internal class Create_Corridor_Surface
    {
        Document doc = Application.DocumentManager.MdiActiveDocument;
        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        Database db = Application.DocumentManager.MdiActiveDocument.Database;
        CivilDocument cvdoc = CivilApplication.ActiveDocument;
        public void create()
        {
            int i = 0;
            var corcol = cvdoc.CorridorCollection;
            PromptStringOptions ps = new PromptStringOptions("Enter surface code:");
            ps.AllowSpaces = true;
            PromptResult result = ed.GetString(ps);
            string code = result.StringResult;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    foreach (var cor in cvdoc.CorridorCollection)
                    {
                        Corridor co = trans.GetObject(cor, OpenMode.ForWrite) as Corridor;                       
                        
                        if(co.CorridorSurfaces.Count > 2)                        
                            continue;
                        else
                        {
                            co.CorridorSurfaces.Add(co.Name + "-" + code+ " Surface");
                            co.CorridorSurfaces[co.CorridorSurfaces.Count-1].AddLinkCode(code, true);
                            if(code=="Datum")
                                co.CorridorSurfaces[co.CorridorSurfaces.Count - 1].OverhangCorrection = OverhangCorrectionType.BottomLinks;
                            else
                                co.CorridorSurfaces[co.CorridorSurfaces.Count - 1].OverhangCorrection = OverhangCorrectionType.TopLinks;
                            co.CorridorSurfaces[co.CorridorSurfaces.Count - 1].Boundaries.AddCorridorExtentsBoundary(co.Name +"_" + code+ "-Bo");
                            //co.CorridorSurfaces[i].Boundaries[0].BoundaryType = CorridorSurfaceBoundaryType.OutsideBoundary;                            
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
