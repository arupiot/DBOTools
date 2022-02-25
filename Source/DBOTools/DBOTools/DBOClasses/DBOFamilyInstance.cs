using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace DBOTools
{
    public class DBOFamilyInstance
    {
        public Family Family { get; }
        public FamilyInstance FamilyInstance { get; }
        public DBOFamilyType DBOFamilyType { get; }
        public ElementId Id { get; }

        public string Name { get; set; }

        public bool IsSelected { get; set; }
        public string TempString { get; set; }
        public bool TempBool { get; set; }
        public bool TempBoolInvert { get; set; }
        
        public Document Document { get; }
        XYZ LocationPoint { get; }
        
        public DBOFamilyInstance (FamilyInstance familyInstance, DBOFamilyType dboFamilyType)
        {
            DBOFamilyType = dboFamilyType;
            FamilyInstance = familyInstance;
            Family = DBOFamilyType.Family;
            Document = FamilyInstance.Document;
            Id = FamilyInstance.Id;
            IsSelected = false;

            Name = "NO_NAME";

            LocationPoint = (FamilyInstance.Location as LocationPoint).Point;
        }

        public string SetNameFromParameter(string parameterName)
        {
            Parameter p = FamilyInstance.LookupParameter(parameterName);
            if (p != null)
            {
                try
                {
                    return p.AsString();
                }
                catch (Exception)
                {
                    return "ERROR_INVALID_PARAMETER";
                }
                
            }
            else
            {
                return "PARAMETER_NOT_FOUND";
            }
            
        }

        public void SetNameFromId()
        {
            string entityTypeName = DBOFamilyType.ExportAs.Name;
            Name = $"{entityTypeName}-{FamilyInstance.Id.ToString()}";
        }

        public Dictionary<string,Dictionary<string,object>> ToDictionary()
        {
            Dictionary<string, Dictionary<string, object>> dictionary = 
                new Dictionary<string, Dictionary<string, object>>()
            {
                {
                    Name,
                        new Dictionary<string, object>()
                        {
                            {"id", Id.ToString() },
                            {"description", FamilyInstance.LookupParameter("Comments")
                                .AsString() },
                            {"general_type", DBOFamilyType.ExportAs.Name},
                            {"entity_type",DBOFamilyType.Name },
                            {"namespace",DBOFamilyType.ExportAs.DBONamespace.ToString() },
                            {"location", new Dictionary<string,string>()
                                {
                                    { "x_loc", $"{LocationPoint.X.ToString("F3")}"},
                                    { "y_loc", $"{LocationPoint.Y.ToString("F3")}"},
                                    { "z_loc", $"{LocationPoint.Z.ToString("F3")}"},
                                }
                            }
                        }
                }
            };
            return dictionary;
        }
    }
}
