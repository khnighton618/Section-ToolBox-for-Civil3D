using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.Civil.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Xaml.Schema;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Region = Autodesk.AutoCAD.DatabaseServices.Region;

namespace SectionToolBox
{
    public partial class Subdividing : Form
    {
        Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
        CivilDocument civildoc = CivilApplication.ActiveDocument;
        Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
        Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
        public Polyline poly;
        public Polyline Line_Sub;
        public double angle = 0;
        double minx = 0;
        double maxx = 0;
        double miny = 0;
        double maxy = 0;
        double bmin = 0;
        double bmax = 0;
        double desirearea = 0;
        string Elapsed_Time = "";
        Stopwatch elaptime = new Stopwatch();

        public Subdividing()
        {
            InitializeComponent();
        }

        private void SelPolyBTN_Click(object sender, EventArgs e)
        {
            try
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    PromptSelectionOptions op = new PromptSelectionOptions();
                    op.MessageForAdding = "\nSelect Polyline:";
                    PromptSelectionResult se1 = ed.GetSelection(op);
                    if (se1.Status == PromptStatus.Error) return;
                    SelectionSet Set1 = se1.Value;
                    ObjectId[] ids1 = Set1.GetObjectIds();
                    poly = trans.GetObject(ids1[0], OpenMode.ForRead) as Polyline;                   

                    minx = poly.Bounds.Value.MinPoint.X;
                    maxx = poly.Bounds.Value.MaxPoint.X;
                    miny = poly.Bounds.Value.MinPoint.Y;
                    maxy = poly.Bounds.Value.MaxPoint.Y;

                    labelArea.Text = "Polyline Area: " + poly.Area.ToString("F3") + "m2";

                    trans.Commit();
                }
            }
            catch (System.Exception ex)
            {
                var st = new System.Diagnostics.StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                ed.WriteMessage("\n" + ex.Message + "\n Line Number" + line + "\n You must remove or edit Point N.O:");
            }
        }

        private void AngleBTN_Click(object sender, EventArgs e)
        {
            PromptAngleOptions op = new PromptAngleOptions("\nSelect two points:");
            PromptDoubleResult result = ed.GetAngle(op);
            if (result.Status == PromptStatus.Error) return;
            angle = result.Value;            
            labelangle.Text = "Angle: " + (angle * 180 / Math.PI).ToString("F4") + "°";            
        }

        public Polyline createLine_sub(Polyline polyfirst, double angle)
        {
            minx = polyfirst.Bounds.Value.MinPoint.X;
            maxx = polyfirst.Bounds.Value.MaxPoint.X;
            miny = polyfirst.Bounds.Value.MinPoint.Y;
            maxy = polyfirst.Bounds.Value.MaxPoint.Y;

            double yminvertice = 0;
            double ymaxvertice = 0;
            Point2d po = new Point2d();
            for (int i = 0; i < polyfirst.NumberOfVertices; i++)
            {
                po = polyfirst.GetPoint2dAt(i);
                if (po.X == minx) yminvertice = po.Y;
                if (po.X == maxx) ymaxvertice = po.Y;
            }
            if (Math.Abs(Math.Cos(angle)) < 1e-5)
            {
                Point2d po1 = new Point2d(minx, miny);
                Point2d po2 = new Point2d(minx, maxy);
                Line_Sub = new Polyline();
                Line_Sub.AddVertexAt(0, po1, 0, 0, 0);
                Line_Sub.AddVertexAt(1, po2, 0, 0, 0);
            }
            else
            {
                bmin = yminvertice - Math.Tan(angle) * minx;
                Point2d po1 = new Point2d(minx - 1e2, bmin + Math.Tan(angle) * (minx - 1e2));
                Point2d po2 = new Point2d(maxx + 1e2, bmin + Math.Tan(angle) * (maxx + 1e2));
                Line_Sub = new Polyline();
                Line_Sub.AddVertexAt(0, po1, 0, 0, 0);
                Line_Sub.AddVertexAt(1, po2, 0, 0, 0);
            }
            return Line_Sub;
        }

        public double createRegion(Polyline firstpoly, double x1, double y1, double x2, double y2, bool createpoly, ref Polyline residualPolyline)
        {
            Region reg3 = null;
            Polyline pl5 = null;
            residualPolyline = null;
            double area = 0;
            try
            {                
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                    Polyline poly2 = new Polyline();
                    Region reg4 = null;
                    Region reg5 = null;
                    Region reg6 = null;
                    Point2d po1 = new Point2d(x1, y1);
                    Point2d po2 = new Point2d(x2, y2);
                    Point2d po3 = new Point2d(x2 - 1e2, y2);
                    Point2d po4 = new Point2d(x1 - 2e2, y1 - 1e2);
                    poly2.AddVertexAt(0, po1, 0, 0, 0);
                    poly2.AddVertexAt(1, po2, 0, 0, 0);
                    poly2.AddVertexAt(2, po3, 0, 0, 0);
                    poly2.AddVertexAt(3, po4, 0, 0, 0);
                    poly2.Closed = true;
                    DBObjectCollection objs = new DBObjectCollection();
                    objs.Add(poly2);
                    objs.Add(firstpoly);
                    
                    DBObjectCollection myRegionColl = new DBObjectCollection();
                    myRegionColl = Autodesk.AutoCAD.DatabaseServices.Region.CreateFromCurves(objs);
                    Region reg1 = myRegionColl[0] as Region;
                    Region reg2 = myRegionColl[1] as Region;
                    
                    reg1.ColorIndex = 33;
                    reg2.ColorIndex = 22;  

                    reg3 = reg1.Clone() as Region;                    
                    reg4 = reg1.Clone() as Region;

                    reg3.BooleanOperation(BooleanOperationType.BoolSubtract, reg2);

                    reg5 = reg3.Clone() as Region;
                    reg5.ColorIndex = 44;

                    reg4.BooleanOperation(BooleanOperationType.BoolSubtract, reg3);

                    reg6 = reg4.Clone() as Region;
                    reg6.ColorIndex = 55;

                    if (createpoly==true)
                    {                        
                        pl5= RegionToPolyline(reg6);
                        pl5.ColorIndex = poly.ColorIndex;
                        btr.AppendEntity(pl5);
                        trans.AddNewlyCreatedDBObject(pl5, true);
                        residualPolyline = RegionToPolyline(reg5);                       
                    }
                    area = reg6.Area;                    
                    trans.Commit();
                }                
            }
            catch (System.Exception ex)
            {
                var st = new System.Diagnostics.StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                ed.WriteMessage("\n" + ex.Message);// + "\n Line Number" + line + "\n You must remove or edit Point N.O:");
            }     
            
            return area;
        }

        public Polyline whileloop(Polyline polyfirst)
        {
            double area = 1;
            double x1 = 0;
            double y1 = 0;
            double x2 = 0;
            double y2 = 0;
            double offset = 0;
            double s = 1;
            int s2 = 0;
            double s3 = 1500;
            double d1 = 0;
            double d2 = 0;
            double d3 = 0;
            int s4 = 1;
            double d_offset = 0;           
            Polyline residualPolyline = null;
            try
            {                
                Line_Sub = createLine_sub(polyfirst, angle);
                while (true)
                {                    
                    if(s2 == 0) { d_offset = .01; }
                    else
                        d_offset = s4 * Convert.ToDouble(AreaRes.Text) * (s/( desirearea/poly.Area))*s2;
                    offset = offset + d_offset;
                    if (offset > Math.Abs(maxx - minx)) offset = offset / Math.Abs(maxx - minx);
                    Polyline pol;
                    using (Transaction trans = db.TransactionManager.StartTransaction())
                    {
                        DBObjectCollection objcol = Line_Sub.GetOffsetCurves(offset);
                        pol = (Polyline)objcol[0];
                        trans.Commit();
                    }
                    x1 = pol.StartPoint.X;
                    y1 = pol.StartPoint.Y;
                    x2 = pol.EndPoint.X;
                    y2 = pol.EndPoint.Y;

                    area = createRegion(polyfirst, x1, y1, x2, y2, false, ref residualPolyline);
                    d1 = area;
                    if (s2 == 0) d2 = area;
                    if (d1 - d2 <= 0 & s2 > 0) s4 = -1;
                    d2 = d1;
                    d3 = d3 + area;
                    s = desirearea - area;
                    s2++;
                    //toolStripStatusLabel1.Text = "Finished, S=" + s.ToString() + ", S2= " + s2.ToString() + " ,Area= " + area.ToString();
                    if (s < Convert.ToDouble(AreaRes.Text) | s2 > s3)
                        break;
                    if (d3 < 1e-5 & s2 > 10)
                    {
                        ed.WriteMessage("\nNo Solution avaialbe!");
                        break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                var st = new System.Diagnostics.StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                ed.WriteMessage("\n" + ex.Message);// + "\n Line Number" + line + "\n You must remove or edit Point N.O:");
            }
            area = createRegion(polyfirst, x1, y1, x2, y2, true, ref residualPolyline);
             return residualPolyline;
            
        }

        public Polyline RegionToPolyline(Region reg)
        {
            Polyline p = null;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                if (reg != null)

                {
                    // Explode Region -> collection of Curves
                    DBObjectCollection cvs = new DBObjectCollection();
                    reg.Explode(cvs);
                    // Create a plane to convert 3D coords
                    // into Region coord system
                    Plane pl = new Plane(new Point3d(0, 0, 0), reg.Normal);
                    // The resulting Polyline
                    p = new Polyline();
                    // Set common entity properties from the Region
                    p.SetPropertiesFrom(reg);
                    // For initial Curve take the first in the list
                    Curve cv1 = cvs[0] as Curve;
                    p.AddVertexAt(p.NumberOfVertices, cv1.StartPoint.Convert2d(pl), BulgeFromCurve(cv1, false), 0, 0);
                    p.AddVertexAt(p.NumberOfVertices, cv1.EndPoint.Convert2d(pl), 0, 0, 0);
                    cvs.Remove(cv1);
                    // The next point to look for
                    Point3d nextPt = cv1.EndPoint;
                    // Find the line that is connected to
                    // the next point
                    // If for some reason the lines returned were not
                    // connected, we could loop endlessly.
                    // So we store the previous curve count and assume
                    // that if this count has not been decreased by
                    // looping completely through the segments once,
                    // then we should not continue to loop.
                    // Hopefully this will never happen, as the curves
                    // should form a closed loop, but anyway...
                    // Set the previous count as artificially high,
                    // so that we loop once, at least.

                    int prevCnt = cvs.Count + 1;
                    while (cvs.Count > 0 && cvs.Count < prevCnt)
                    {
                        prevCnt = cvs.Count;
                        foreach (Curve cv in cvs)
                        {
                            // If one end of the curve connects with the
                            // point we're looking for...
                            if (cv.StartPoint == nextPt || cv.EndPoint == nextPt)
                            {
                                // Calculate the bulge for the curve and
                                // set it on the previous vertex
                                double bulge = BulgeFromCurve(cv, cv.EndPoint == nextPt);
                                p.SetBulgeAt(p.NumberOfVertices - 1, bulge);
                                // Reverse the points, if needed
                                if (cv.StartPoint == nextPt)
                                    nextPt = cv.EndPoint;
                                else
                                    // cv.EndPoint == nextPt
                                    nextPt = cv.StartPoint;
                                // Add out new vertex (bulge will be set next
                                // time through, as needed)
                                p.AddVertexAt(p.NumberOfVertices, nextPt.Convert2d(pl), 0, 0, 0);
                                // Remove our curve from the list, which
                                // decrements the count, of course
                                cvs.Remove(cv);
                                break;
                            }
                        }
                    }

                    if (cvs.Count >= prevCnt)
                    {
                        p.Dispose();
                        ed.WriteMessage("\nError connecting segments.");
                    }
                    else
                    {
                        // Once we have added all the Polyline's vertices,
                        // transform it to the original region's plane
                        p.TransformBy(Matrix3d.PlaneToWorld(pl));
                        // Append our new Polyline to the database

                        // btr.UpgradeOpen();
                        // btr.AppendEntity(p);
                        // tr.AddNewlyCreatedDBObject(p, true);

                        // Finally we erase the original region
                        //reg.UpgradeOpen();
                        //reg.Erase();
                    }
                    
                }

                tr.Commit();
            }
            return p;
        }

        // Helper function to calculate the bulge for arcs
        private static double BulgeFromCurve(Curve cv, bool clockwise)
        {
            double bulge = 0.0;
            Arc a = cv as Arc;
            if (a != null)
            {
                double newStart;
                // The start angle is usually greater than the end,
                // as arcs are all counter-clockwise.
                // (If it isn't it's because the arc crosses the
                // 0-degree line, and we can subtract 2PI from the
                // start angle.)
                if (a.StartAngle > a.EndAngle)
                    newStart = a.StartAngle - 8 * Math.Atan(1);
                else

                    newStart = a.StartAngle;
                // Bulge is defined as the tan of
                // one fourth of the included angle
                bulge = Math.Tan((a.EndAngle - newStart) / 4);
                // If the curve is clockwise, we negate the bulge
                if (clockwise)
                    bulge = -bulge;
            }
            return bulge;
        }

        private void CreateBTN_Click(object sender, EventArgs e)
        {
            elaptime.Start();
            if (poly == null) MessageBox.Show("You must select a polyline first!");
            else if (angle==null) MessageBox.Show("You must select a angle!");
            else
            {
                if (checkBox1.Checked) desirearea = poly.Area / Convert.ToInt32(NObtn.Text);
                else desirearea = Convert.ToDouble(DesireArea.Text);
                //label7.Text = "Elapsed Time: 00:00:00 ";
                double area = 0;
                Polyline pl = null;
                for (int i = 0; i < Convert.ToInt32(NObtn.Text); i++)
                {
                    if (i == 0) pl = poly;
                    pl = whileloop(pl);
                    if (pl == null) break;
                    if (pl != null & pl.Area < desirearea) break;

                }
                elaptime.Stop();
                Elapsed_Time = "Elapsed Time: " + elaptime.Elapsed.ToString().Remove(11);
                elaptime.Reset();
                label7.Text = Elapsed_Time;
            }            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked) { DesireArea.Enabled = false; }
            if(checkBox1.Checked== false) { DesireArea.Enabled = true; }
        }

        private void Subdividing_MouseHover(object sender, EventArgs e)
        {
            //label7.Text = Elapsed_Time;
        }
    }
}
