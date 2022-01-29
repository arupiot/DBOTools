using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DBOTools
{
    public class HVACTypes
    {

        public class AHUType : DBOType
        {
            public bool IsCanonical { get; set; }
            public List<string> Implements { get; set; }
            public List<string> Uses { get; set; }
            public EntityType EntityType = EntityType.AHU;

            public AHUType(
                string name,
                string id,
                bool isCanonical,
                List<string> implements, 
                List<string> uses)
            {
                Name = name;
                Id = id;
                IsCanonical = isCanonical;
                Implements = implements;
                Uses = uses;
                DBONamespace = DBONamespace.HVAC;
            }

            public AHUType(FamilySymbol ahuFamilyType)
            {
                try
                {
                    Name = ahuFamilyType.LookupParameter("Name").AsString();
                }
                catch (Exception)
                {
                    Name = "";
                }


                if (ahuFamilyType.LookupParameter("IfcGuid") != null)
                {
                    Id = ahuFamilyType.LookupParameter("IfcGuid").AsString();
                }
                else
                {
                    Id = "";
                }

                IsCanonical = true;

                if (ahuFamilyType.LookupParameter(DBOParameters.dboImplements.Name) != null)
                {
                    string implements = ahuFamilyType.LookupParameter(DBOParameters.dboImplements.Name).AsString();
                    Implements = implements.Split(' ').ToList().OrderBy(x=>x).ToList();
                }
                else
                {
                    Implements = new List<string>() { };
                }

                if (ahuFamilyType.LookupParameter(DBOParameters.dboUses.Name) != null)
                {
                    string implements = ahuFamilyType.LookupParameter(DBOParameters.dboUses.Name).AsString();
                    Uses = implements.Split(' ').ToList().OrderBy(x => x).ToList();
                }
                else
                {
                    Uses = new List<string>() { };
                }
            }
        }

        public class AHUInstance : DBOInstance
        {
            public DBOType DBOType { get; set; }
            public string Name { get; set; }
            public string Id { get; set; }
            public bool IsCanonical = false;

            public AHUInstance(DBOType dboType, string name, string id)
            {
                DBOType = dboType;
                Name = name;
                Id = id;
            }

            public AHUInstance(FamilyInstance familyInstance, List<DBOType> dboTypesList, string nameParameterName)
            {
                if (familyInstance.LookupParameter(nameParameterName) != null)
                {
                    Name = familyInstance.LookupParameter(nameParameterName).AsString();
                }
                else
                {
                    Name = "";
                }

                if (familyInstance.LookupParameter("IfcGuid") != null)
                {
                    Id = familyInstance.LookupParameter("IfcGuid").AsString();
                }
                else
                {
                    Id = "";
                }

                DBOType = null;
                if (familyInstance.Symbol.LookupParameter("Name") != null)
                {
                    string typeName = familyInstance.Symbol.LookupParameter("Name").AsString();
                    if (typeName != "")
                    {
                        DBOType = dboTypesList.Where(x => x.Name == typeName).FirstOrDefault();
                    }
                }

            }
        }
    }
}
