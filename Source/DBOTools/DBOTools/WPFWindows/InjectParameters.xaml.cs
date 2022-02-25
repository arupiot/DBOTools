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
    /// Interaction logic for InjectParameters.xaml
    /// </summary>
    public partial class InjectParameters : Window
    {
        public Document Document { get; set; }
        public DefinitionFile SharedParameterFile { get; }

        public List<DBOFamily> DBOFamilies { get; set; }
        public List<ElementId> SelectedFamiliesIds { get; set; }
        public List<DBOParameter> ParametersToAdd { get; set; }

        public Definition ExportAsDefinition { get; set; }
        public Definition ImplementsDefinition { get; set; }
        public Definition UsesDefinition { get; set; }

        ExtEvent_InjectParameters handlerInjectParameters;
        ExternalEvent externalEventInjectParameters;

        ExtEvent_RemoveParameters handlerRemoveParameters;
        ExternalEvent externalEventRemoveParameters;

        public InjectParameters(Document document)
        {

            Document = document;
            SharedParameterFile = Document.Application.OpenSharedParameterFile();

            if (SharedParameterFile == null)
            {
                MessageBox.Show("No shared parameter file loaded");
            }
            else
            {
                ExportAsDefinition = Helpers.DefinitionFromName
                    (SharedParameterFile, DBOParameters.dboExportAs.Name);
                ImplementsDefinition = Helpers.DefinitionFromName
                    (SharedParameterFile, DBOParameters.dboImplements.Name);
                UsesDefinition = Helpers.DefinitionFromName
                    (SharedParameterFile, DBOParameters.dboUses.Name);
            }

            ParametersToAdd = new List<DBOParameter>() { };

            List<Family> familiesList = new FilteredElementCollector(Document).
                OfClass(typeof(Family)).
                WhereElementIsNotElementType().
                Cast<Family>().
                Where(x => 
                    Constants.IncludedCategories.Contains(x.FamilyCategory.Name) == true
                    && x.GetFamilySymbolIds().Count >0).
                ToList().
                OrderBy(family => family.FamilyCategory.Name).
                ThenBy(family => family.Name).ToList();

            DBOFamilies = new List<DBOFamily>() { };

            foreach (Family family in familiesList)
            {
                try
                {
                    DBOFamily dboFamily = new DBOFamily(family);
                    DBOFamilies.Add(dboFamily);
                }
                catch (Exception)
                {
                    throw;
                }
                
            }

            DBOFamilies = DBOFamilies.OrderBy(x => x.FullName).ToList();

            SelectedFamiliesIds = new List<ElementId>() { };

            DataContext = this;

            InitializeComponent();

            handlerInjectParameters = new ExtEvent_InjectParameters
                (SelectedFamiliesIds, ParametersToAdd);
            externalEventInjectParameters = ExternalEvent.Create(handlerInjectParameters);

            handlerRemoveParameters = new ExtEvent_RemoveParameters
                (SelectedFamiliesIds, ParametersToAdd);
            externalEventRemoveParameters = ExternalEvent.Create(handlerRemoveParameters);

            dboExportAsCheckBox.IsChecked = ExportAsDefinition != null;
            dboImplementsCheckBox.IsChecked = ImplementsDefinition != null;
            dboUsesCheckBox.IsChecked = UsesDefinition != null;

        }

        private void injectParametersButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

            if (dboExportAsCheckBox.IsChecked == true)
            {
                ParametersToAdd.Add(DBOParameters.dboExportAs);
            }
            if (dboImplementsCheckBox.IsChecked == true)
            {
                ParametersToAdd.Add(DBOParameters.dboImplements);
            }
            if (dboUsesCheckBox.IsChecked == true)
            {
                ParametersToAdd.Add(DBOParameters.dboUses);
            }

            if (ParametersToAdd.Count() == 0)
            {
                MessageBox.Show("No parameters selected");
            }
            if (SelectedFamiliesIds.Count() == 0)
            {
                MessageBox.Show("No families selected");
            }
            if (ParametersToAdd.Count() != 0 && SelectedFamiliesIds.Count() != 0)
            {
                try
                {
                    externalEventInjectParameters.Raise();
                }
                catch (Exception eRaise)
                {
                    MessageBox.Show(eRaise.Message);
                }
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            
            if (dboExportAsCheckBox.IsChecked == true)
            {
                ParametersToAdd.Add(DBOParameters.dboExportAs);
            }
            if (dboImplementsCheckBox.IsChecked == true)
            {
                ParametersToAdd.Add(DBOParameters.dboImplements);
            }
            if (dboUsesCheckBox.IsChecked == true)
            {
                ParametersToAdd.Add(DBOParameters.dboUses);
            }

            if (ParametersToAdd.Count() == 0)
            {
                MessageBox.Show("No parameters selected");
            }
            if (SelectedFamiliesIds.Count() == 0)
            {
                MessageBox.Show("No families selected");
            }
            if (ParametersToAdd.Count() != 0 && SelectedFamiliesIds.Count() != 0)
            {
                try
                {
                    externalEventRemoveParameters.Raise();
                }
                catch (Exception eRaise)
                {
                    MessageBox.Show(eRaise.Message);
                }
            }

        }



        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FamilyChecked_Click(object sender, RoutedEventArgs e)
        {
            // Add the Id of the family to the list of selected families
            // Which is ugly but works

            CheckBox checkBox = sender as CheckBox;
            DBOFamily dboFamily = checkBox.DataContext as DBOFamily;
            if (dboFamily != null)
            {
                if (SelectedFamiliesIds.Contains(dboFamily.Id)
                    == false)
                {
                    SelectedFamiliesIds.Add(dboFamily.Id);
                }
            }
            else
            {
                MessageBox.Show($"Can't find DBOFamily " +
                    $"{dboFamily.FullName} in DataGrid source");
            }
        }
        private void FamilyUnchecked_Click(object sender, RoutedEventArgs e)
        {
            // Removes the Id of the family from the list of selected families
            // Which is ugly but works

            CheckBox checkBox = sender as CheckBox;
            DBOFamily dboFamily = checkBox.DataContext as DBOFamily;
            if (dboFamily != null)
            {
                if (SelectedFamiliesIds.Contains(dboFamily.Id)
                    == true)
                {
                    SelectedFamiliesIds.Remove(dboFamily.Id);
                }
            }
            else
            {
                MessageBox.Show($"Can't find DBOFamily " +
                    $"{dboFamily.FullName} in DataGrid source");
            }
        }
    }
}
