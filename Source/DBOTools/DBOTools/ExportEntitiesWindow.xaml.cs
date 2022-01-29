using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace DBOTools
{
    /// <summary>
    /// Interaction logic for ExportEntitiesWindow.xaml
    /// </summary>
    public partial class ExportEntitiesWindow : Window
    {
        public List<DBOFamily> DBOFamiliesFound { get; set; }
        public List<DBOFamilyType> DBOFamilyTypes { get; set; }
        public List<DBOFamilyInstance> DBOFamilyInstances { get; set; }

        public List<string> AbbreviationsFound { get; set; }
        public List<string> AbbreviationsToExport { get; set; }
        public List<string> InstanceParameterNames { get; set; }

        public Document Document { get; set; }

        public bool NamesFromParameter { get; set; }
        public string InstanceNamingParameterName { get; set; }

        public ExportEntitiesWindow(Document document)
        {
            /*
             * Get all the family types
             * If they have DBOExport populated
             * Create a DBOType
             * For each DBOType:
             * Write the export as yaml
             * Get all instances and export them as yaml
            */

            Document = document;

            NamesFromParameter = false;
            InstanceParameterNames = Helpers.GetAllInstanceParameterNames(Document);

            List<FamilySymbol> familySymbols = new FilteredElementCollector(document).
                OfClass(typeof(FamilySymbol)).WhereElementIsElementType().
                Cast<FamilySymbol>()
                .ToList();

            DBOFamilyTypes = new List<DBOFamilyType>() { };
            DBOFamilyInstances = new List<DBOFamilyInstance>() { };
            AbbreviationsToExport = new List<string>() { };

            foreach (FamilySymbol familySymbol in familySymbols)
            {
                DBOFamilyType dboFamilyType = new DBOFamilyType(familySymbol);
                if (dboFamilyType.HasAllParameters() == true)
                {
                    dboFamilyType.SetAttributesToExisting();
                    if (dboFamilyType.ExportAs != null)
                    {
                        DBOFamilyTypes.Add(dboFamilyType);
                    }
                }
            }

            AbbreviationsFound = DBOFamilyTypes.Select(x => x.ExportAs.Name)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            DataContext = this;
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void injectParametersButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MessageBox.Show(InstanceNamingParameterName);

            // Create a yaml document to contain the types
            // Create a yaml document to contain the instances

            List<FamilyInstance> familyInstances = new FilteredElementCollector(Document).
                       OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType().
                       Cast<FamilyInstance>().ToList();

            List<Dictionary<string, Dictionary<string, object>>> typeDictionaries = 
                new List<Dictionary<string, Dictionary<string, object>>>();
            List<Dictionary<string, Dictionary<string, object>>> instanceDictionaries = 
                new List<Dictionary<string, Dictionary<string, object>>>();

            foreach (DBOFamilyType dboFamilyType in DBOFamilyTypes)
            {
                if (AbbreviationsToExport.Contains(dboFamilyType.ExportAs.Name))
                {
                    // Add the dictionary of the symbol to the yaml document for the types
                    typeDictionaries.Add(dboFamilyType.ToDictionary());

                    // Find the instances and create the dictionaries to export
                    List<FamilyInstance> instancesOfType = familyInstances
                        .Where(x => x.Symbol.Id == dboFamilyType.Id).ToList();

                    foreach (FamilyInstance familyInstance in instancesOfType) 
                    {
                        DBOFamilyInstance dboFamilyInstance = 
                            new DBOFamilyInstance(familyInstance, dboFamilyType);

                        // Need to check that if NamesFromId is false, something is selected
                        // as the name parameter
                        if (NamesFromParameter == false)
                        {
                            dboFamilyInstance.SetNameFromId();
                        }
                        else
                        {
                            // Temporary
                            dboFamilyInstance.SetNameFromParameter("BDNS Asset Name");
                        }
                        DBOFamilyInstances.Add(dboFamilyInstance);
                        instanceDictionaries.Add(dboFamilyInstance.ToDictionary());
                    }

                }
            }
            MessageBox.Show($"Family Instances: {familyInstances.Count}\n" +
                $"DBO Family Types: {DBOFamilyTypes.Count}\n" +
                $"DBO Family Instances: {DBOFamilyInstances.Count}");
            //MessageBox.Show(string.Join("\n",typeDictionaries));
            //MessageBox.Show(string.Join("\n", instanceDictionaries));
        }

        private void NamesFromIdRB_Click(object sender, RoutedEventArgs e)
        {
            NamesFromParameter = false;
            MessageBox.Show(NamesFromParameter.ToString());
            instanceParametersCB.IsEnabled = false;
        }

        private void NameFromParamRB_Click(object sender, RoutedEventArgs e)
        {
            NamesFromParameter = true;
            MessageBox.Show(NamesFromParameter.ToString());
            instanceParametersCB.IsEnabled = true;
        }

        private void instanceParametersCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InstanceNamingParameterName = instanceParametersCB.SelectedItem as string;
        }
    }
}
