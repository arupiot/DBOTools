using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Caliburn.Micro;

namespace DBOTools
{
    /// <summary>
    /// Interaction logic for PopulateExportAs.xaml
    /// </summary>
    public partial class PopulateExportAsWindow : Window
    {
        public string SourceParameterName { set; get; }
        public List<string> AvailableTypeParameters { set; get; }
        public BindableCollection<DBOFamily> DBOFamilies { set; get; }
        public List<DBOFamily> SelectedFamilies { set; get; }

        ExtEvent_PopulateExportAs handler_PopulateExportAs;
        ExternalEvent externalEvent_PopulateExportAs;

        public PopulateExportAsWindow(Document document)
        {
            // Create empty list for the selected families
            SelectedFamilies = new List<DBOFamily>() { };

            // Get all type parameters in project

            AvailableTypeParameters = Helpers.GetAllTypeParameterNames(document);

            // Build the list of DBO Families to edit

            List<Family> familiesList = new FilteredElementCollector(document)
                .OfClass(typeof(Family))
                .WhereElementIsNotElementType()
                .Cast<Family>()
                .Where(x => Constants.IncludedCategories.Contains(x.FamilyCategory.Name)
                    && x.GetFamilySymbolIds().Count > 0)
                .ToList();
            
            DBOFamilies = new BindableCollection<DBOFamily>() { };

            foreach (Family family in familiesList)
            {
                {
                    DBOFamily dboFamily = new DBOFamily(family);
                    dboFamily.SetAttributesToExisting();
                    dboFamily.IsSelected = false;
                    
                    DBOFamilies.Add(dboFamily);
                }
            }

            DBOFamilies.OrderBy(x => x.FullName);



            DataContext = this;

            InitializeComponent();

            handler_PopulateExportAs = new ExtEvent_PopulateExportAs(SelectedFamilies);
            externalEvent_PopulateExportAs = ExternalEvent.Create(handler_PopulateExportAs);

        }

        private void editFamilyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            DBOFamily dboFamily = checkBox.DataContext as DBOFamily;
            SelectedFamilies.Add(dboFamily);
        }

        private void editFamilyCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            DBOFamily dboFamily = checkBox.DataContext as DBOFamily;
            SelectedFamilies.Remove(dboFamily);
        }

        private void typeParameters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // On parameter changed, update source parameter name 
            // for auto-populating DBO Export As

            try
            {
                System.Windows.Controls.ComboBox comboBox = 
                    sender as System.Windows.Controls.ComboBox;
                SourceParameterName = comboBox.SelectedItem as string;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedFamilies.Count == 0)
            {
                MessageBox.Show("No families selected");
            }
            else
            {
                try
                {
                    externalEvent_PopulateExportAs.Raise();
                }
                catch (Exception eOk)
                {
                    MessageBox.Show(eOk.Message);
                }

                this.Close();
            }
        }

        private void selectExportAsComboBox_SelectionChanged
            (object sender, SelectionChangedEventArgs e)
        {
            // On selection changed, change the DBO Export As of DBO families
            // DO NOT CHANGE THE REVIT FAMILIES JUST YET

            try
            {
                System.Windows.Controls.ComboBox comboBox = 
                    sender as System.Windows.Controls.ComboBox;
                DBOFamily dboFamily = comboBox.DataContext as DBOFamily;
                EntityType selectedEntityType = comboBox.SelectedItem as EntityType;

                dboFamily.ExportAs = selectedEntityType;
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.Message);
            }
        }

        private void tryPopulateFromParameterButton_Click(object sender, RoutedEventArgs e)
        {
            /*
                For all families OR selected families only:
                Try to read the source parameter if it exists for the symbol,
                Try to associate the string value to an entity type,
                If entity type found, populate DBO Export As with it.
            */

            SourceParameterName = typeParameters.SelectedItem as string;
            

            if (SourceParameterName != null && SourceParameterName != "")
            {
            
                foreach (DBOFamily dboFamily in DBOFamilies)
                {

                        //MessageBox.Show(dboFamily.Family.Name);
                        Parameter parameter = dboFamily.Symbols[0].LookupParameter(SourceParameterName);
                        if (parameter != null)
                        {
                            string parameterValue = parameter.AsString();

                            //MessageBox.Show(parameterValue);

                            EntityType matchingType = EntityType.EntityTypes
                                .Where(x => x.Name == parameterValue)
                                .FirstOrDefault();

                            if (matchingType != null)
                            {
                                dboFamily.ExportAs = matchingType;
                                //MessageBox.Show(dboFamily.ExportAs.Name);
                            }

                        }

                }

                DBOFamilies.Refresh();
                dboFamiliesDataGrid.ItemsSource = DBOFamilies;
            }
            
        }

        private void SetComboBoxToExistingEntityType()
        {
            StringBuilder stringBuilder = new StringBuilder();
            int index = dboFamiliesDataGrid.Columns.IndexOf(ComboBoxColumn);
            foreach (var item in dboFamiliesDataGrid.Items)
            {
                //var another_item = dboFamiliesDataGrid.Columns[index].GetCellContent(item);

                int item_index = dboFamiliesDataGrid.Items.IndexOf(item);

                stringBuilder.AppendLine(item.ToString() + " - " + item_index.ToString());

                var another_item = ComboBoxColumn.GetCellContent(item);
                if (another_item != null)
                {
                    stringBuilder.AppendLine(another_item.GetType().ToString());
                }
                else
                {
                    stringBuilder.AppendLine("NOT FOUND");
                }
                
            }
            MessageBox.Show(stringBuilder.ToString());
        }

        private void editFamilyCheckBox_Checked_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
