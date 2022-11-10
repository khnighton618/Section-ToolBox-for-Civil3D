using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.GraphicsSystem;
using Autodesk.AutoCAD.Geometry;

using Autodesk.Civil.ApplicationServices;
using Autodesk.Civil.DatabaseServices;
using Autodesk.Civil.DatabaseServices.Styles;

namespace Sections
{
    public class Profiles
    {        
        static public void profilefrompolyline()
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            CivilDocument civildoc = CivilApplication.ActiveDocument;
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    //----------------------------------------
                    PromptEntityOptions peo = new PromptEntityOptions("\n Select a polyline : ");
                    peo.SetRejectMessage("\n Not a polyline");
                    peo.AddAllowedClass(typeof(Autodesk.AutoCAD.DatabaseServices.Polyline), true);
                    PromptEntityResult per = ed.GetEntity(peo);
                    if (per.Status != PromptStatus.OK) return;
                    ObjectId plineId = per.ObjectId;
                    //-----------------------------------------
                    PromptEntityOptions peo2 = new PromptEntityOptions("\n Select a ProfileView: ");
                    peo.SetRejectMessage("\n Not a ProfileView");
                    peo.AddAllowedClass(typeof(ProfileView), true);
                    PromptEntityResult per2 = ed.GetEntity(peo2);
                    if (per2.Status != PromptStatus.OK) return;
                    ProfileView pv = trans.GetObject(per2.ObjectId, OpenMode.ForWrite) as ProfileView;
                    double x0 = 0;
                    double y0 = 0;
                    double Sta0 = pv.StationStart;
                    if (pv.ElevationRangeMode == ElevationRangeType.Automatic)
                    {
                        pv.ElevationRangeMode = ElevationRangeType.UserSpecified;
                        pv.FindXYAtStationAndElevation(pv.StationStart, pv.ElevationMin, ref x0, ref y0);
                    }
                    else
                    {
                        pv.FindXYAtStationAndElevation(pv.StationStart, pv.ElevationMin, ref x0, ref y0);
                    }
                    Autodesk.Civil.DatabaseServices.Styles.ProfileViewStyle PVstyle = trans.GetObject(pv.StyleId, OpenMode.ForRead) as Autodesk.Civil.DatabaseServices.Styles.ProfileViewStyle;

                    //-------------------------------------
                    Alignment oAlignment = trans.GetObject(pv.AlignmentId, OpenMode.ForRead) as Alignment;
                    ObjectId layerId = oAlignment.LayerId;
                    ObjectId styleId = civildoc.Styles.ProfileStyles[0];

                    ObjectId labelSetId = civildoc.Styles.LabelSetStyles.ProfileLabelSetStyles[0];
                    ObjectId oProfileId = Profile.CreateByLayout(oAlignment.Name + "-" + DateTime.Now.ToShortTimeString(), pv.AlignmentId, layerId, styleId, labelSetId);
                    Profile oProfile = trans.GetObject(oProfileId, OpenMode.ForWrite) as Profile;
                    //-----------------------------------------------------------
                    BlockTableRecord btr = trans.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;
                    Autodesk.AutoCAD.DatabaseServices.Polyline pline = trans.GetObject(plineId, OpenMode.ForRead, false) as Autodesk.AutoCAD.DatabaseServices.Polyline;
                    if (pline != null)
                    {
                        int segCount = pline.NumberOfVertices - 1;
                        double xs = 0;
                        double ys = 0;
                        double xe = 0;
                        double ye = 0;
                        for (int cnt = 0; cnt < segCount; cnt++)
                        {
                            SegmentType type = pline.GetSegmentType(cnt);
                            switch (type)
                            {
                                case SegmentType.Line:
                                    {
                                        LineSegment2d Liseg2d = pline.GetLineSegment2dAt(cnt);
                                        xs = Liseg2d.StartPoint.X;
                                        ys = Liseg2d.StartPoint.Y;
                                        double sta = xs - x0 + Sta0;
                                        double dh = (ys - y0) / (PVstyle.GraphStyle.VerticalExaggeration) + pv.ElevationMin;
                                        Point2d startpo = new Point2d(sta, dh);
                                        xe = Liseg2d.EndPoint.X;
                                        ye = Liseg2d.EndPoint.Y;
                                        double sta2 = xe - x0 +Sta0;
                                        double dh2 = (ye - y0) / (PVstyle.GraphStyle.VerticalExaggeration) + pv.ElevationMin;
                                        Point2d endpo = new Point2d(sta2, dh2);
                                        ProfileTangent oTangent1 = oProfile.Entities.AddFixedTangent(startpo, endpo);
                                        break;
                                    }
                                case SegmentType.Arc:
                                    {
                                        CircularArc2d arcseg = pline.GetArcSegment2dAt(cnt);
                                        xs = arcseg.StartPoint.X;
                                        ys = arcseg.StartPoint.Y;
                                        xe = arcseg.EndPoint.X;
                                        ye = arcseg.EndPoint.Y;
                                        double sta = xs - x0 + Sta0;
                                        double dh = (ys - y0) / (PVstyle.GraphStyle.VerticalExaggeration) + pv.ElevationMin;
                                        double sta2 = xe - x0 + Sta0;
                                        double dh2 = (ye - y0) / (PVstyle.GraphStyle.VerticalExaggeration) + pv.ElevationMin;
                                        Point2d po = arcseg.GetSamplePoints(11)[5];
                                        double sta3 = po.X - x0 + Sta0;
                                        double dh3 = (po.Y - y0) / (PVstyle.GraphStyle.VerticalExaggeration) + pv.ElevationMin;
                                        Point2d meanpo = new Point2d(sta3, dh3);
                                        Point2d endpo = new Point2d(sta2, dh2);
                                        Point2d startpo = new Point2d(sta, dh);
                                        ProfileParabolaSymmetric oCurve = oProfile.Entities.AddFixedSymmetricParabolaByThreePoints(startpo, meanpo, endpo);
                                        break;
                                    }
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
