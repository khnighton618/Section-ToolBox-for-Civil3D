//using System.Collections.Generic;
//using Autodesk.AutoCAD.ApplicationServices;
//using Autodesk.AutoCAD.EditorInput;
//using Autodesk.AutoCAD.Runtime;
//using Autodesk.AutoCAD.DataExtraction;


//namespace Sections

//{
//    public class xmlcommands
//    {
//        //const string path =   @"c:\Program Files\Autodesk\AutoCAD 2009\Sample\";

//        //const string fileName =   "Visualization - Aerial.dwg";

//        //const string outputXmlFile =     @"c:\temp\data-extract.xml";

//        [CommandMethod("extd")]
//        public void extractData(string path, string fileName, string outputXmlFile)
//        {
//            if (!System.IO.File.Exists(path + fileName))
//            {
//                Document doc =  Application.DocumentManager.MdiActiveDocument;
//                Editor ed =   doc.Editor;
//                ed.WriteMessage("\nFile does not exist.");
//                return;
//            }
//            // Create some settings for the extraction
//            IDxExtractionSettings es =  new DxExtractionSettings();
//            IDxDrawingDataExtractor de =  es.DrawingDataExtractor;
//            de.Settings.ExtractFlags =  ExtractFlags.ModelSpaceOnly | ExtractFlags.XrefDependent | ExtractFlags.Nested;

//            // Add a single file to the settings
//            IDxFileReference fr =  new DxFileReference(path, path + fileName);
//            de.Settings.DrawingList.AddFile(fr);

//            // Scan the drawing for object types & their properties
//            de.DiscoverTypesAndProperties(path);
//            List<IDxTypeDescriptor> types =  de.DiscoveredTypesAndProperties;

//            // Select all the types and properties for extraction
//            // by adding them one-by-one to these two lists
//            List<string> selTypes = new List<string>();
//            List<string> selProps = new List<string>();
//            foreach (IDxTypeDescriptor type in types)
//            {
//                selTypes.Add(type.GlobalName);
//                foreach (IDxPropertyDescriptor pr in type.Properties)
//                {
//                    if (!selProps.Contains(pr.GlobalName))
//                        selProps.Add(pr.GlobalName);
//                }
//            }

//            // Pass this information to the extractor
//            de.Settings.SetSelectedTypesAndProperties(types, selTypes, selProps);

//            // Now perform the extraction itself
//            de.ExtractData(path);

//            // Get the results of the extraction
//            System.Data.DataTable dataTable =   de.ExtractedData;

//            // Output the extracted data to an XML file
//            if (dataTable.Rows.Count > 0)
//            {
//                dataTable.TableName = "My_Data_Extract";
//                dataTable.WriteXml(outputXmlFile);
//            }
//        }
//    }
//}