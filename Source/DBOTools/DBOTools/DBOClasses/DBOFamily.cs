using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace DBOTools
{
    public class DBOFamily
    {
        public Family Family { get; }
                
        public bool HasExportAs { get; }
        public bool HasImplements { get; }
        public bool HasUses { get; }

        public string FullName { get; }

        public bool IsSelected { get; set; }

        public EntityType ExportAs { get; set; }
        public List<string> Implements { get; set; }
        public List<string> Uses { get; set; }
        public DBONamespace DBONamespace { get; set; }

        public Document Document { get; }
        public IList<FamilySymbol> Symbols { get; }
        public List<DBOFamilyType> DBOFamilyTypes { get; set; }
        public List<EntityType> AvailableTypes { get; } = EntityType.EntityTypes;
        public ElementId Id { get; }
        

        /*
        Implements, Uses and other attributes will change asynchronously 
        from the Revit Family parameter values because they need a transaction.

        The methods below can set them to existing if needed, and also restore
        them in the DBO families if the changes have not been committed to the
        Revit Families.
        */
        
        public DBOFamily(Family family)
        {
            Family = family;
            Document = Family.Document;
            Symbols = GetFamilySymbols();

            FullName = $"{Family.FamilyCategory.Name} - {Family.Name}";

            HasExportAs = _HasExportAs();
            HasImplements = _HasImplements();
            HasUses = _HasUses();

            Uses = new List<string>() { };
            Implements = new List<string>() { };

            DBOFamilyTypes = GetDBOFamilyTypes();
            Id = Family.Id;
        }

        private IList<FamilySymbol> GetFamilySymbols()
        {
            IList<FamilySymbol> symbols = new FilteredElementCollector(Document).
                OfClass(typeof(FamilySymbol)).WhereElementIsElementType().
                Cast<FamilySymbol>().Where(x => x.Family.Id == Family.Id).ToList();

            FilteredElementCollector symbolsFEC = new FilteredElementCollector(Document).
            OfClass(typeof(FamilySymbol)).WhereElementIsElementType();

            return symbols;
        }

        private bool _HasExportAs()
        {
            if (Symbols.Count() > 0)
            {
                return Symbols.First().
                    LookupParameter(DBOParameters.dboExportAs.Name) != null;
            }
            else
            {
                return false;
            }
        }

        private bool _HasImplements()
        {

            if (Symbols.Count() > 0)
            {
                return Symbols.First().
                    LookupParameter(DBOParameters.dboImplements.Name) != null;
            }
            else
            {
                return false;
            }
        }

        private bool _HasUses()
        {
            if (Symbols.Count() > 0)
            {
                return Symbols.First().
                    LookupParameter(DBOParameters.dboUses.Name) != null;
            }
            else
            {
                return false;
            }
        }

        public List<string> GetExistingImplements()
        {
            if (HasImplements == false)
            {
                return null;
            }
            else
            {
                FamilySymbol symbol = Symbols.First();
                if (symbol.LookupParameter(DBOParameters.dboImplements.Name)
                    .HasValue == true)
                {
                    string paramValue = symbol
                        .LookupParameter(DBOParameters.dboImplements.Name)
                        .AsString();

                    return paramValue.Split(' ').ToList();
                }
                else
                {
                    return new List<string>() { };
                }
            }
        }

        public List<string> GetExistingUses()
        {
            if (HasUses == false)
            {
                return null;
            }
            else
            {
                FamilySymbol symbol = Symbols.First();
                if (symbol.LookupParameter(DBOParameters.dboUses.Name)
                    .HasValue == true)
                {
                    string paramValue = symbol
                        .LookupParameter(DBOParameters.dboUses.Name)
                        .AsString();

                    return paramValue.Split(' ').ToList();
                }
                else
                {
                    return new List<string>() { };
                }
            }
        }

        public EntityType GetExistingDBOExportAs()
        {
            if (HasExportAs == false)
            {
                return null;
            }
            else
            {
                FamilySymbol symbol = Symbols.First();
                if (symbol.LookupParameter(DBOParameters.dboExportAs.Name)
                    .HasValue == true)
                {
                    string paramValue = symbol
                        .LookupParameter(DBOParameters.dboExportAs.Name)
                        .AsString();

                    return EntityType.EntityTypes.
                        Where(x => x.Name == paramValue).
                        FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
        }

        public void SetAttributesToExisting()
        {
            ExportAs = GetExistingDBOExportAs();
            Implements = GetExistingImplements();
            Uses = GetExistingUses();

            if (ExportAs != null)
            {
                DBONamespace = ExportAs.DBONamespace;
            }
        }

        private List<DBOFamilyType> GetDBOFamilyTypes()
        {
            List<DBOFamilyType> dboTypes = new List<DBOFamilyType>() { };

            if (Symbols.Count() != 0)
            {
                foreach(FamilySymbol familySymbol in Symbols)
                {
                    DBOFamilyType dboFamilyType = new DBOFamilyType(this, familySymbol);
                    dboTypes.Add(dboFamilyType);
                }
            }

            return dboTypes;
        }


    }
}
