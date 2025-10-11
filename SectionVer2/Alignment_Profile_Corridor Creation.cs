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
using Autodesk.AutoCAD.Geometry;
using System.Security.Cryptography;
using Autodesk.Civil.DatabaseServices.Styles;
using ObjectIdCollection = Autodesk.Civil.DatabaseServices;

namespace SectionToolBox
{
    public partial class Alignment_Profile_Corridor_Creation : Form
    {
        Document doc = Application.DocumentManager.MdiActiveDocument;
        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        Database db = Application.DocumentManager.MdiActiveDocument.Database;
        CivilDocument cvdoc = CivilApplication.ActiveDocument;
        public Alignment_Profile_Corridor_Creation()
        {
            InitializeComponent();
            SurfListbox.Items.Clear();
            PF_StyleListbox.Items.Clear();
            PFV_Band_Listbox.Items.Clear();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                foreach (ObjectId surfid in cvdoc.GetSurfaceIds())
                {
                    TinSurface surface = tr.GetObject(surfid, OpenMode.ForRead) as TinSurface;
                    SurfListbox.Items.Add(surface.Name);
                }
                foreach (ObjectId pfid in cvdoc.Styles.ProfileStyles)
                {
                    ProfileStyle pfstyle = tr.GetObject(pfid, OpenMode.ForRead) as ProfileStyle;                    
                   PF_StyleListbox.Items.Add(pfstyle.Name);
                    ExistListbox.Items.Add(pfstyle.Name);
                }
                foreach (ObjectId pfvstyle in cvdoc.Styles.ProfileViewStyles)
                {
                    ProfileViewStyle pfvs = tr.GetObject(pfvstyle, OpenMode.ForRead) as ProfileViewStyle;
                    Pfviewstyleslist.Items.Add(pfvs.Name);
                }
                //ProfileViewBandItem pfband = cvdoc.Styles.BandStyles.ProfileViewProfileDataBandStyles;
                //ProfileViewBandSetItem pfband = cvdoc.Styles.ProfileViewBandSetStyles;
                ProfileViewBandSetStyleCollection bandStyles = cvdoc.Styles.ProfileViewBandSetStyles;
                
                foreach (ObjectId pfid in bandStyles)
                {
                    ProfileViewBandSetStyle band = tr.GetObject(pfid, OpenMode.ForRead) as ProfileViewBandSetStyle;
                    PFV_Band_Listbox.Items.Add(band.Name);
                    
                }
                tr.Commit();
            }
        }

        [Obsolete]
        private void button1_Click(object sender, EventArgs e)
        {
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {

                try
                {
                    ObjectId EGid = cvdoc.GetSurfaceIds()[0];
                    foreach (ObjectId surfid in cvdoc.GetSurfaceIds())
                    {
                        TinSurface surface = tr.GetObject(surfid, OpenMode.ForRead) as TinSurface;
                        if (surface.Name == SurfListbox.SelectedItem.ToString())
                        {
                            EGid = surfid;
                            break;
                        }                        
                    }
                    double startSTA;
                    double endSTA;
                    double startZ;
                    double endZ;
                    double maxX = 0;
                    double temp = 0;
                    int s = 0;
                    Point2d postart = new Point2d();
                    Point2d poend = new Point2d();
                    PromptPointOptions ppo = new PromptPointOptions("Select location");
                    PromptPointResult ppr = ed.GetPoint(ppo);
                    Point3d pinsert = new Point3d(ppr.Value.X, ppr.Value.Y, ppr.Value.Z);
                    double y = ppr.Value.Y;
                    double x = ppr.Value.X;
                    int indexexist = 0;
                    int indexproject = 0;
                    int indexband = 0;
                    int indexpfvstyle = 0;
                    int rem = 0;
                    double m = 0;
                    indexexist= findprofilestyle(cvdoc.Styles.ProfileStyles, tr, " ");
                    indexproject= findprofilestyle(cvdoc.Styles.ProfileStyles, tr, "project line");
                    indexband= findprofileviewbandstyle(cvdoc.Styles.ProfileViewBandSetStyles, tr);
                    indexpfvstyle= findpfvstyle(cvdoc.Styles.ProfileViewStyles, tr);
                    List<double> maxx = new List<double>();
                    foreach (ObjectId algid in cvdoc.GetAlignmentIds())
                    {
                        Alignment alg = tr.GetObject(algid, OpenMode.ForWrite) as Alignment;                        
                        ObjectId layerId = alg.LayerId;                        
                        ObjectId styleId = cvdoc.Styles.ProfileStyles[indexexist];
                        ObjectId labelSetId = cvdoc.Styles.LabelSetStyles.ProfileLabelSetStyles[0];
                        ObjectId surfaceProfileId = Profile.CreateFromSurface("EG_" + alg.Name+DateTime.Now.ToString(), algid, EGid, layerId, styleId, labelSetId);
                        Profile EGProfile = tr.GetObject(surfaceProfileId, OpenMode.ForWrite) as Profile;
                        startSTA = EGProfile.StartingStation;
                        endSTA = EGProfile.EndingStation;
                        startZ = EGProfile.ElevationAt(startSTA);
                        endZ = EGProfile.ElevationAt(endSTA);
                        postart = new Point2d(startSTA, startZ);
                        poend= new Point2d(endSTA, endZ);
                        styleId = cvdoc.Styles.ProfileStyles[indexproject];
                        ObjectId layoutid = Profile.CreateByLayout("L_" + alg.Name + DateTime.Now.ToString() + DateTime.Now.ToString(), algid, layerId, styleId, labelSetId);
                        Profile layoutProf = tr.GetObject(layoutid, OpenMode.ForWrite) as Profile;
                        layoutProf.Entities.AddFixedTangent(postart, poend);                
                        y = ppr.Value.Y - (s - m) * Convert.ToDouble(RowwidthText.Text);
                        x = ppr.Value.X + Math.Floor(s / Convert.ToDouble(NoperrowText.Text)) * Convert.ToDouble(ColwidthText.Text);
                        pinsert = new Point3d(x, y, ppr.Value.Z);
                        //xy.Add(x);
                        //xy.Add(y);
                        ObjectId profViewId = ProfileView.Create(alg.ObjectId, pinsert, "PFV_" + alg.Name + DateTime.Now.ToString(), cvdoc.Styles.ProfileViewBandSetStyles[indexband], cvdoc.Styles.ProfileViewStyles[indexpfvstyle]);
                        ProfileView profView = tr.GetObject(profViewId, OpenMode.ForWrite) as ProfileView;
                        maxx.Add(profView.StationEnd);
                        if(s> Convert.ToInt32(NoperrowText.Text)) maxX = maxx.Max();
                        s++;
                        m =  Convert.ToInt32(NoperrowText.Text)* Math.Floor(s / Convert.ToDouble(NoperrowText.Text));
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\n" + ex.Message);
                }
                tr.Commit();
            }
               
        }
        public int findprofileviewbandstyle(ProfileViewBandSetStyleCollection obidcol, Transaction tr)
        {
            int s = 0;
            foreach(ObjectId oid in obidcol)
            {
                ProfileViewBandSetStyle band = tr.GetObject(oid, OpenMode.ForRead) as ProfileViewBandSetStyle;
                if (band != null & band.Name == PFV_Band_Listbox.SelectedItem.ToString())
                    break;
                s++;
            }

            return s;
        }
        public int findpfvstyle(ProfileViewStyleCollection pfvst, Transaction transaction)
        {
            int s = 0;
            foreach(ObjectId oid in pfvst)
            {
                ProfileViewStyle pfvstyle = transaction.GetObject(oid, OpenMode.ForRead) as ProfileViewStyle;
                if(pfvstyle != null & pfvstyle.Name== Pfviewstyleslist.SelectedItem.ToString())
                    break;
                s++;
            }
            return s;
        } 
        public int findprofilestyle(ProfileStyleCollection obidcol, Transaction tr, string str)
        {
            int s = 0;
            if (str == "project line")
            {
                foreach (ObjectId oid in obidcol)
                {
                    ProfileStyle stylepf = tr.GetObject(oid, OpenMode.ForRead) as ProfileStyle;

                    if (stylepf != null & stylepf.Name == PF_StyleListbox.SelectedItem.ToString())
                        break;
                    s++;
                }
            }
            else
            {
                foreach (ObjectId oid in obidcol)
                {
                    ProfileStyle stylepf = tr.GetObject(oid, OpenMode.ForRead) as ProfileStyle;
                    if (stylepf != null & stylepf.Name == ExistListbox.SelectedItem.ToString())
                        break;
                    s++;
                }
            }
                

            return s;
        }
    }
}
