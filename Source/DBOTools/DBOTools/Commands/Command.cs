#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

#endregion

namespace DBOTools
{
    [Transaction(TransactionMode.Manual)]
    public class DisplayInjectParameters : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;
            //Selection sel = uidoc.Selection;

            if (doc.IsFamilyDocument == true)
            {
                // Add another version for a family document
                MessageBox.Show("This command is not available in a Family Document.");
            }
            else
            {
                InjectParameters injectParameters = new InjectParameters(doc);
                try
                {
                    injectParameters.Show();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                
            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class DisplayPopulateDBOExportAs : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;
            //Selection sel = uidoc.Selection;

            if (doc.IsFamilyDocument == true)
            {
                MessageBox.Show("This command is not available in a Family Document.");
            }
            else
            {
                PopulateExportAsWindow populateExportAsWindow = new PopulateExportAsWindow(doc);
                try
                {
                    populateExportAsWindow.Show();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class PopulateDBOTypeParametersShowDialog : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;
            //Selection sel = uidoc.Selection;

            if (doc.IsFamilyDocument == true)
            {
                MessageBox.Show("This command is not available in a Family Document.");
            }
            else
            {
                PopulateTypeParametersWindow populateTypeParametersWindow =
                    new PopulateTypeParametersWindow(doc);
                try
                {
                    populateTypeParametersWindow.Show();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }

            return Result.Succeeded;
        }
    }

    /*
    public class ExtEventInjectParameters : IExternalEventHandler
    {
        List<ElementId> SelectedFamiliesIds;
        List<DBOParameter> ParametersToAdd;

        public ExtEventInjectParameters
            (List<ElementId> selectedFamiliesIds, 
            List<DBOParameter> parametersToAdd)
        {
            SelectedFamiliesIds = selectedFamiliesIds;
            ParametersToAdd = parametersToAdd;
        }


        public void Execute(UIApplication uiapp)
        {
            Document doc = uiapp.ActiveUIDocument.Document;
            DefinitionFile sharedParameterFile = doc.Application
                .OpenSharedParameterFile();

            List<string> errorMessages = new List<string>() { };

            List<string> injectionLog = new List<string>() { };

            StringBuilder stringBuilder = new StringBuilder();
            
            foreach (ElementId id in SelectedFamiliesIds)
            {
                Element family = doc.GetElement(id);

                if (family is Family)
                {
                    stringBuilder.AppendLine(family.Name + " " + family.Id.ToString());
                }

                
                
            }
            MessageBox.Show(stringBuilder.ToString());

            TaskDialog logTaskDialog = new TaskDialog("DBO Parameters Injection")
            {
                Title = "Parameters injection",
                MainInstruction = "Operation completed.",
                MainContent = stringBuilder.ToString(),
                MainIcon = TaskDialogIcon.TaskDialogIconNone
            };
            logTaskDialog.Show();

            return;
        }

        public string GetName()
        {
            return "External Event Inject Parameters";
        }
    }

    public class ExtEventRemoveParameters : IExternalEventHandler
    {
        List<Family> SelectedFamilies;
        List<DBOParameter> ParametersToRemove;

        public ExtEventRemoveParameters
            (List<Family> selectedFamilies,
            List<DBOParameter> parametersToRemove)
        {
            SelectedFamilies = selectedFamilies;
            ParametersToRemove = parametersToRemove;
        }


        public void Execute(UIApplication uiapp)
        {
            Document doc = uiapp.ActiveUIDocument.Document;
            DefinitionFile sharedParameterFile = doc.Application
                .OpenSharedParameterFile();

            List<string> errorMessages = new List<string>() { };

            List<string> injectionLog = new List<string>() { };

            foreach (Family family in SelectedFamilies)
            {
                try
                {
                    if (!family.IsEditable)
                    {
                        injectionLog.Add($"Family is not editable: " +
                                $"{family.FamilyCategory.Name} - {family.Name}");
                    }
                    else
                    {
                        Document famDoc = doc.EditFamily(family);
                        FamilyManager familyManager = famDoc.FamilyManager;
                        List<FamilyParameter> familyParameters = familyManager.
                            GetParameters().ToList();
                        List<Definition> definitionsPresent = familyParameters.
                            Select(x => x.Definition).ToList();


                        if (definitionsPresent.Any(_def => ParametersToRemove.
                            Select(x => x.GetDefinition(sharedParameterFile))
                            .Contains(_def)) == true)
                        {
                            try
                            {
                                Transaction famT = new Transaction(famDoc);
                                famT.Start("Remove DBO parameters");
                                foreach (DBOParameter parameterToRemove in ParametersToRemove)
                                {
                                    FamilyParameter p = familyManager.Parameters.Cast<FamilyParameter>()
                                        .Where(x => x.Definition.Name == parameterToRemove.Name).FirstOrDefault();

                                    if (p != null)
                                    {
                                        try
                                        {
                                            familyManager.RemoveParameter(p);
                                        }
                                        catch (Exception exception)
                                        {
                                            injectionLog.Add($"Exception thrown for family " +
                                                $"{family.FamilyCategory.Name} - {family.Name}, " +
                                                $"removing parameter {parameterToRemove.Name}: " +
                                                $"{exception.Message}");
                                        }
                                    }

                                }
                                famT.Commit();

                                famDoc.LoadFamily(doc, new FamilyLoad.FamilyLoadOptions());

                            }
                            catch (Exception exception)
                            {
                                injectionLog.Add($"Exception thrown for family " +
                                    $"{family.FamilyCategory.Name} - {family.Name}: " +
                                    $"{exception.Message}");
                            }
                        }
                    }
                }
                catch (Exception injectionException)
                {

                    TaskDialog.Show($"Error removing the parameter", injectionException.Message);
                }
            }


            TaskDialog logTaskDialog = new TaskDialog("BDNS Parameters Deletion")
            {
                Title = "Parameters Deletion",
                MainInstruction = "Operation completed.",
                MainContent = string.Join("\n", injectionLog),
                MainIcon = TaskDialogIcon.TaskDialogIconNone
            };
            logTaskDialog.Show();

            return;
        }

        public string GetName()
        {
            return "External Event Remove Parameters";
        }
    }
    */

    [Transaction(TransactionMode.Manual)]
    public class ExportEntitiesShowDialog : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;
            //Selection sel = uidoc.Selection;

            if (doc.IsFamilyDocument == true)
            {
                MessageBox.Show("This command is not available in a Family Document.");
            }
            else
            {
                ExportEntitiesWindow exportEntitiesWindow =
                    new ExportEntitiesWindow(doc);
                try
                {
                    exportEntitiesWindow.Show();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }

            }

            return Result.Succeeded;
        }
    }


    public class FamilyLoad
    {
        public class FamilyLoadOptions : IFamilyLoadOptions
        {
            public bool OnFamilyFound(
                bool familyInUse,
                out bool overwriteParameterValues)
            {
                overwriteParameterValues = true;
                return true;
            }

            public bool OnSharedFamilyFound(
                Family sharedFamily,
                bool familyInUse,
                out FamilySource source,
                out bool overwriteParameterValues)
            {
                source = FamilySource.Family;
                overwriteParameterValues = true;
                return true;
            }
        }
    }

}
