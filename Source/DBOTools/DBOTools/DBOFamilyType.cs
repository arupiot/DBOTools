using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using YamlDotNet;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.RepresentationModel;
namespace DBOTools
{
    public class DBOFamilyType
    {
        public Family Family { get; }
        public FamilySymbol FamilySymbol { get; }
        public DBOFamily DBOFamily { get; }
        public ElementId Id { get; }
        public string Name { get; }

        public bool IsValid { get; }

        public bool HasExportAs { get; }
        public bool HasImplements { get; }
        public bool HasUses { get; }

        public bool IsSelected { get; set; }

        public EntityType ExportAs { get; set; }
        public List<string> Implements { get; set; }
        public List<string> Uses { get; set; }

        public Document Document { get; }
        public IList<FamilySymbol> Symbols { get; }
        public List<EntityType> AvailableTypes { get; } = EntityType.EntityTypes;

        public string ErrorMessage {get;}
        private string _ErrorMessage { get; set; }

        public DBOFamilyType(DBOFamily dboFamily, FamilySymbol familySymbol)
        {
            FamilySymbol = familySymbol;
            Name = FamilySymbol.LookupParameter("Type Name").AsString();
            Family = FamilySymbol.Family;
            Id = FamilySymbol.Id;

            DBOFamily = dboFamily;

            IsValid = GetIsValid();
            ErrorMessage = GetErrorMessage();

            HasExportAs = DBOFamily.HasExportAs;
            HasImplements = DBOFamily.HasImplements;
            HasUses = DBOFamily.HasUses;

            ExportAs = DBOFamily.ExportAs;
            Implements = new List<string>() { };
            Uses = new List<string>() { };

            IsSelected = false;
        }

        public DBOFamilyType(FamilySymbol familySymbol)
        {
            FamilySymbol = familySymbol;
            Name = FamilySymbol.LookupParameter("Type Name").AsString();
            Family = FamilySymbol.Family;
            Id = FamilySymbol.Id;

            DBOFamily = null;

            IsValid = GetIsValid();
            ErrorMessage = GetErrorMessage();

            HasExportAs = GetHasExportAs();
            HasImplements = GetHasImplements();
            HasUses = GetHasUses();

            ExportAs = null;
            Implements = new List<string>() { };
            Uses = new List<string>() { };

            IsSelected = false;
        }

        private bool GetHasExportAs()
        {
            if (DBOFamily != null)
            {
                return false;
            }
            else if (FamilySymbol.LookupParameter(DBOParameters.dboExportAs.Name) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool GetHasImplements()
        {
            if (DBOFamily != null)
            {
                return DBOFamily.HasImplements;
            }
            else if (FamilySymbol.LookupParameter(DBOParameters.dboImplements.Name) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool GetHasUses()
        {
            if (DBOFamily != null)
            {
                return DBOFamily.HasUses;
            }
            else if (FamilySymbol.LookupParameter(DBOParameters.dboUses.Name) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool GetIsValid()
        {
            if (DBOFamily == null)
            {
                return false;
            }
            else
            {
                if (DBOFamily.Family.Id != Family.Id)
                {
                    _ErrorMessage = "DBOFamily and Family Symbol Family mismatch. Error during creation of DBO Family";
                    return false;
                }
                if (FamilySymbol == null)
                {
                    _ErrorMessage = "DBOFamilySymbol is null. Error during creation of DBO Family";
                    return false;
                }

                return true;
            }

        }

        public bool HasAllParameters()
        {
            if (HasExportAs == HasImplements == HasUses == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private EntityType GetExportAs()
        {
            if (DBOFamily != null)
            {
                return DBOFamily.ExportAs;
            }
            else if (HasExportAs == true)
            {
                try
                {
                    return Helpers.GetEntityTypeByName
                        (FamilySymbol.LookupParameter
                        (DBOParameters.dboExportAs.Name).AsString());

                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        private string GetErrorMessage()
        {
            GetIsValid();
            return _ErrorMessage;
        }

        public void SetAttributesToExisting()
        {
            if (HasExportAs == true)
            {
                EntityType _exportAs = GetExportAs();

                if (DBOFamily != null)
                {
                    DBOFamily.ExportAs = _exportAs;
                }
            }
            else
            {
                ExportAs = null;
            }

            if (HasImplements == true)
            {
                string ImplementsFromParameter = FamilySymbol
                    .LookupParameter(DBOParameters.dboImplements.Name)
                    .AsString();
                List<string> implements = new List<string>() { };
                if (ImplementsFromParameter != null && ImplementsFromParameter != "")
                {
                    implements = ImplementsFromParameter.Split(' ').OrderBy(x => x).ToList() ;
                }
                Implements = implements;
            }
            else
            {
                Implements = null;
            }

            if (HasUses == true)
            {
                string UsesFromParameter = FamilySymbol
                    .LookupParameter(DBOParameters.dboImplements.Name)
                    .AsString();
                List<string> uses = new List<string>() { };
                if (UsesFromParameter != null && UsesFromParameter != "")
                {
                    uses = UsesFromParameter.Split(' ').OrderBy(x => x).ToList();
                }
                Uses = uses;
            }
            else
            {
                Uses = null;
            }
        }

        public Dictionary<string, Dictionary<string, object>> ToDictionary()
        {
            Dictionary<string, Dictionary<string, object>> dictionary = new Dictionary<string, Dictionary<string, object>>()
            {
                {
                    string.Join("_", Implements),
                        new Dictionary<string, object>()
                        {
                            { "id", Id.ToString() },
                            { "description", FamilySymbol.LookupParameter("Type Description")
                                .AsString() },
                            { "is_canonical", true },
                            { "implements", Implements },
                            { "uses", Uses }
                        }
                }
            };


            return dictionary;

        }

    }
}
