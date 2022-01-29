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
using Caliburn.Micro;

namespace DBOTools
{
    /// <summary>
    /// Interaction logic for PopulateFamily.xaml
    /// </summary>
    public partial class PopulateDBOFamilyParameterWindow : Window
    {
        public List<string> AvailableTypeParameters { set; get; }
        public List<Family> FamiliesList { get; set; }
        public ObservableCollection<DBOFamily> DBOFamiliesList { get; set; }
        public DBOFamily SelectedDBOFamily { get; set; }
        //public BindableCollection<DBOFamily> DBOFamilies { set; get; }
        //public ObservableCollection<DBOFamily> DBOFamiliesToEdit { set; get; }

        //public List<string> MyRandomStrings = new List<string>() { "Hello", "Hi" };

        public PopulateDBOFamilyParameterWindow(Document document)
        {
            FamiliesList = new FilteredElementCollector(document)
                .OfClass(typeof(Family))
                .WhereElementIsNotElementType()
                .Cast<Family>()
                .Where(x => Variables.IncludedCategories.Contains(x.FamilyCategory.Name)
                    && x.GetFamilySymbolIds().Count > 0)
                .ToList();

            DBOFamiliesList = new ObservableCollection<DBOFamily>() { };

            foreach (Family family in FamiliesList)
            {
                if (family.GetFamilySymbolIds().Count() != 0)
                {
                    DBOFamily dboFamily = new DBOFamily(family);
                    foreach (DBOFamilyType dboFamilyType in dboFamily.DBOFamilyTypes)
                    {
                        dboFamilyType.SetAttributesToExisting();
                    }
                    DBOFamiliesList.Add(dboFamily);
                }
            }

            DBOFamiliesList.OrderBy(x=>x.FullName);

            DataContext = this;

            InitializeComponent();
            entityTypeComboBox.ItemsSource = EntityType.EntityTypes;

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedDBOFamily != null)
            {
                MessageBox.Show(SelectedDBOFamily.FullName);
            }
            else
            {
                MessageBox.Show("Family not found");
            }
            
            this.Close();
        }

        private void FamiliesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Bring selected family to a public variable for ease of use
            SelectedDBOFamily = (sender as ListView).SelectedItem as DBOFamily;

            // Populate table of family types
            // Yes I know, I shouldn't be doing it from the code behind but I'm in a rush

            typesStackPanel.Children.Clear();

            typesTitle.Text = $"Selected Family: {SelectedDBOFamily.FullName}";

            bool familyIsDBOReady;
            if (SelectedDBOFamily.HasExportAs ==
                SelectedDBOFamily.HasImplements ==
                SelectedDBOFamily.HasUses == true)
            {
                familyIsDBOReady = true;
            }
            else
            {
                familyIsDBOReady = false;
            }

            if (familyIsDBOReady == false)
            {
                entityTypeComboBox.SelectedItem = null;
                entityTypeComboBox.IsEnabled = false;
            }
            else
            {
                try
                {
                    entityTypeComboBox.SelectedItem = SelectedDBOFamily.ExportAs;
                }
                catch (Exception)
                {
                    entityTypeComboBox.SelectedItem = null;
                }
            }

            foreach(DBOFamilyType dboFamilyType in SelectedDBOFamily.DBOFamilyTypes)
            {
                StackPanel typeStackPanel = new StackPanel();
                typeStackPanel.Orientation = Orientation.Vertical;

                Label typeNameLabel = new Label()
                {
                    Content = dboFamilyType.Name,
                };
                typeStackPanel.Children.Add(typeNameLabel);

                StackPanel typePropertiesStackPanel = new StackPanel();
                typePropertiesStackPanel.Orientation = Orientation.Horizontal;

                Label implementsLabel = new Label()
                {
                    Content = "Implements: ",
                    Width = 100,
                };
                typePropertiesStackPanel.Children.Add(implementsLabel);

                TextBox implementsTextBox = new TextBox();
                if (dboFamilyType.HasImplements == true)
                {
                    implementsTextBox.IsEnabled = true;
                    implementsTextBox.Text = string.Join(" ", dboFamilyType.Implements);
                    implementsTextBox.TextWrapping = TextWrapping.Wrap;
                    implementsTextBox.Width = 200;
                }
                else
                {
                    implementsTextBox.IsEnabled = false;
                    implementsTextBox.Text = "";
                    implementsTextBox.TextWrapping = TextWrapping.Wrap;
                    implementsTextBox.Width = 200;
                }                
                typePropertiesStackPanel.Children.Add(implementsTextBox);

                Label usesLabel = new Label()
                {
                    Content = "Uses: ",
                    Width = 100,
                };
                typePropertiesStackPanel.Children.Add(usesLabel);

                TextBox usesTextBox = new TextBox();
                if (dboFamilyType.HasUses == true)
                {
                    usesTextBox.IsEnabled = true;
                    usesTextBox.Text = string.Join(" ", dboFamilyType.Uses);
                    usesTextBox.TextWrapping = TextWrapping.Wrap;
                    usesTextBox.Width = 200;
                }
                else
                {
                    usesTextBox.IsEnabled = false;
                    usesTextBox.Text = "";
                    usesTextBox.TextWrapping = TextWrapping.Wrap;
                    usesTextBox.Width = 200;
                }
                typePropertiesStackPanel.Children.Add(usesTextBox);

                typeStackPanel.Children.Add(typePropertiesStackPanel);
                typesStackPanel.Children.Add(typeStackPanel);
            }






        }
    }
}