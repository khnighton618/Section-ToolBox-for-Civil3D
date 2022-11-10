using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Permissions;
using Autodesk.AutoCAD.Runtime;
using acApp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

[assembly: CommandClass(typeof(MyClassSerializer.Commands))]



namespace Sections
{
    // We need it to help with deserialization
    public sealed class MyBinder : SerializationBinder
    {
        public override System.Type BindToType(string assemblyName, string typeName)
        {
            return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
        }
    }
    // Helper class to write to and from ResultBuffer
    public class MyUtil
    {
        const int kMaxChunkSize = 127;
        public static ResultBuffer StreamToResBuf(MemoryStream ms, string appName)
        {
            ResultBuffer resBuf = new ResultBuffer(new TypedValue((int)DxfCode.ExtendedDataRegAppName, appName));
            for (int i = 0; i < ms.Length; i += kMaxChunkSize)
            {
                int length = (int)Math.Min(ms.Length - i, kMaxChunkSize);
                byte[] datachunk = new byte[length];
                ms.Read(datachunk, 0, length);
                resBuf.Add(new TypedValue((int)DxfCode.ExtendedDataBinaryChunk, datachunk));
            }
            return resBuf;
        }
        public static MemoryStream ResBufToStream(ResultBuffer resBuf)
        {
            MemoryStream ms = new MemoryStream();
            TypedValue[] values = resBuf.AsArray();
            // Start from 1 to skip application name
            for (int i = 1; i < values.Length; i++)
            {
                byte[] datachunk = (byte[])values[i].Value;
                ms.Write(datachunk, 0, datachunk.Length);
            }
            ms.Position = 0;
            return ms;
        }
    }
    [Serializable]
    public abstract class MyBaseClass : ISerializable
    {
        public const string appName = "MyApp";
        public MyBaseClass()
        {
        }
        public static object NewFromResBuf(ResultBuffer resBuf)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Binder = new MyBinder();
            MemoryStream ms = MyUtil.ResBufToStream(resBuf);
            MyBaseClass mbc = (MyBaseClass)bf.Deserialize(ms);
            return mbc;
        }
        public static object NewFromEntity(Entity ent)
        {
            using (ResultBuffer resBuf = ent.GetXDataForApplication(appName))
            {
                return NewFromResBuf(resBuf);
            }
        }
        public ResultBuffer SaveToResBuf()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, this);
            ms.Position = 0;
            ResultBuffer resBuf = MyUtil.StreamToResBuf(ms, appName);
            return resBuf;
        }
        public void SaveToEntity(Entity ent)
        {
            // Make sure application name is registered
            // If we were to save the ResultBuffer to an Xrecord.Data,
            // then we would not need to have a registered application name
            Transaction tr = ent.Database.TransactionManager.TopTransaction;
            RegAppTable regTable = (RegAppTable)tr.GetObject(ent.Database.RegAppTableId, OpenMode.ForWrite);
            if (!regTable.Has(MyClass.appName))
            {
                RegAppTableRecord app = new RegAppTableRecord();
                app.Name = MyClass.appName;
                regTable.Add(app);
                tr.AddNewlyCreatedDBObject(app, true);
            }
            using (ResultBuffer resBuf = SaveToResBuf())
            {
                ent.XData = resBuf;
            }
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public abstract void GetObjectData(SerializationInfo info, StreamingContext context);
    }
    [Serializable]
    public class MyClass : MyBaseClass
    {
        public string myString;
        public double myDouble;
        public MyClass()
        {
        }
        protected MyClass(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            myString = (string)info.GetValue("MyString", typeof(string));
            myDouble = (double)info.GetValue("MyDouble", typeof(double));
        }
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("MyString", myString);
            info.AddValue("MyDouble", myDouble);
        }
        // Just for testing purposes
        public override string ToString()
        {
            return base.ToString() + "," + myString + "," + myDouble.ToString();
        }
    }
    public class Commands
    {
        [CommandMethod("SaveClassToEntityXData")]
        static public void SaveClassToEntityXData()
        {
            Database db = acApp.DocumentManager.MdiActiveDocument.Database;
            Editor ed = acApp.DocumentManager.MdiActiveDocument.Editor;
            PromptEntityResult per = ed.GetEntity("Select entity to save class to:\n");
            if (per.Status != PromptStatus.OK)
                return;
            // Create an object
            MyClass mc = new MyClass();
            mc.myDouble = 1.2345;
            mc.myString = "Some text";
            // Save it to the document
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                Entity ent = (Entity)tr.GetObject(per.ObjectId, OpenMode.ForWrite);
                mc.SaveToEntity(ent);
                tr.Commit();
            }
            // Write some info about the results
            ed.WriteMessage("Content of MyClass we serialized:\n {0} \n", mc.ToString());
        }
        [CommandMethod("GetClassFromEntityXData")]
        static public void GetClassFromEntityXData()
        {
            Database db = acApp.DocumentManager.MdiActiveDocument.Database;
            Editor ed = acApp.DocumentManager.MdiActiveDocument.Editor;
            PromptEntityResult per = ed.GetEntity("Select entity to get class from:\n");
            if (per.Status != PromptStatus.OK)
                return;
            // Get back the class
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                Entity ent = (Entity)tr.GetObject(per.ObjectId, OpenMode.ForRead);
                MyClass mc = (MyClass)MyClass.NewFromEntity(ent);
                // Write some info about the results
                ed.WriteMessage("Content of MyClass we deserialized:\n {0} \n", mc.ToString());
                tr.Commit();
            }
        }
    }
}
