using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DBOTools
{
    public class DBOParameter
    {
        public string Name { get; set; }
        public ParameterType ParameterType { get; set; }
        public BuiltInParameterGroup BuiltInParameterGroup { get; set; }
        public bool IsInstance { get; set; }
        public string GUID { get; set; } //Hardcoded guid to check the correct shared parameter file is used

        public Definition GetDefinition(DefinitionFile sharedParameterFile)
        {
            Definition definition = Helpers.DefinitionFromName(sharedParameterFile, Name);
            return definition;
        }

        public DBOParameter(string name,
                       ParameterType parameterType,
                       BuiltInParameterGroup builtInParameterGroup,
                       bool isInstance,
                       string guid)
        {
            Name = name;
            ParameterType = parameterType;
            BuiltInParameterGroup = builtInParameterGroup;
            IsInstance = isInstance;
            GUID = guid;
        }
    }

    public static class DBOParameters
    {
        // Commenting this out because it can be extracted from the implements
        //public static DBOParameter dboTypeName = new DBOParameter
        //    (
        //    "DBO Type Name",
        //    ParameterType.Text,
        //    BuiltInParameterGroup.PG_IDENTITY_DATA,
        //    false,
        //    ""
        //    );

        // Commenting this out because it can be taken from a user-specified parameter
        //public static DBOParameter dboAssetName = new DBOParameter
        //    (
        //    "DBO Asset Name",
        //    ParameterType.Text,
        //    BuiltInParameterGroup.PG_IDENTITY_DATA,
        //    true,
        //    ""
        //    );

        // Commenting this out because if DBO Export As has value, then this can be taken a true
        //public static DBOParameter dboExport = new DBOParameter
        //    (
        //    "DBO Export",
        //    ParameterType.YesNo,
        //    BuiltInParameterGroup.PG_IDENTITY_DATA,
        //    false,
        //    ""
        //    );

        public static DBOParameter dboExportAs = new DBOParameter
            (
            "DBO Export As",
            ParameterType.Text,
            BuiltInParameterGroup.PG_IDENTITY_DATA,
            false,
            "24eb6624-f359-4530-ace4-d0f0da0fc20f"
            );

        public static DBOParameter dboImplements = new DBOParameter
            (
            "DBO Implements",
            ParameterType.Text,
            BuiltInParameterGroup.PG_IDENTITY_DATA,
            false,
            "62035b5a-4f64-438a-8052-af03ceb5fbfb"
            );

        public static DBOParameter dboUses = new DBOParameter
            (
            "DBO Uses",
            ParameterType.Text,
            BuiltInParameterGroup.PG_IDENTITY_DATA,
            false,
            "e177f55b-32e1-4a40-8537-8c427f8bd90f"
            );

    }
}
