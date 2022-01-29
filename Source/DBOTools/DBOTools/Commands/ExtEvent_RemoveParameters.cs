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

namespace DBOTools
{
    public class ExtEvent_RemoveParameters : IExternalEventHandler
    {
        List<ElementId> SelectedFamiliesIds;
        List<DBOParameter> ParametersToRemove;

        public ExtEvent_RemoveParameters
            (List<ElementId> selectedFamiliesIds,
            List<DBOParameter> parametersToRemove)
        {
            SelectedFamiliesIds = selectedFamiliesIds;
            ParametersToRemove = parametersToRemove;
        }


        public void Execute(UIApplication uiapp)
        {
            Document doc = uiapp.ActiveUIDocument.Document;
            DefinitionFile sharedParameterFile = doc.Application
                .OpenSharedParameterFile();

            StringBuilder stringBuilder = new StringBuilder();

            foreach (ElementId id in SelectedFamiliesIds)
            {
                Element element = doc.GetElement(id);

                if (element is Family)
                {
                    Family family = element as Family;
                    stringBuilder.AppendLine($"Processing family: " +
                        $"{family.FamilyCategory.Name} - {family.Name} Id: {id.ToString()}...");

                    if (!family.IsEditable)
                    {
                        stringBuilder.AppendLine($"\tFamily not editable. Skipped.");
                    }
                    else
                    {
                        try
                        {
                            Document famDoc = doc.EditFamily(family);
                            FamilyManager familyManager = famDoc.FamilyManager;
                            List<string> parameterNames = familyManager.GetParameters()
                                .Select(x => x.Definition.Name)
                                .Distinct()
                                .ToList();

                            // Start editing the family
                            Transaction famT = new Transaction(famDoc);
                            famT.Start("Remove DBO Parameters");
                            stringBuilder.AppendLine("\tTransaction started...");
                            foreach (DBOParameter dboParameter in ParametersToRemove)
                            {
                                // Check if parameter is present
                                if (parameterNames.Contains(dboParameter.Name) == false)
                                {
                                    stringBuilder.AppendLine($"\tFamily does not contain" +
                                        $" {dboParameter.Name}");
                                }
                                else
                                {
                                    try
                                    {
                                        FamilyParameter famP = familyManager.Parameters
                                            .Cast<FamilyParameter>()
                                            .Where(x => x.Definition.Name == dboParameter.Name)
                                            .FirstOrDefault();

                                        if (famP != null)
                                        {
                                            familyManager.RemoveParameter(famP);

                                            // Check if it's still there
                                            if (familyManager.GetParameters()
                                                .Cast<FamilyParameter>()
                                                .Where(x => x.Definition.Name == dboParameter.Name)
                                                .FirstOrDefault() == null)
                                            {
                                                stringBuilder.AppendLine($"\tSuccessfully removed {dboParameter.Name} from family");
                                            }
                                            else
                                            {
                                                stringBuilder.AppendLine($"\tFailed removing {dboParameter.Name} from family");
                                            }
                                        }
                                        else
                                        {
                                            stringBuilder.AppendLine($"\tCannot fetch {dboParameter.Name} " +
                                                $"from family parameters. Parameter skipped.");
                                        }

                                    }
                                    catch (Exception e)
                                    {
                                        stringBuilder.AppendLine("\t" + e.Message);
                                    }
                                }

                            }
                            famT.Commit();
                            stringBuilder.AppendLine("\tTransaction committed.");
                            stringBuilder.AppendLine("\tLoading family back...");
                            famDoc.LoadFamily(doc, new FamilyLoad.FamilyLoadOptions());
                            stringBuilder.AppendLine("\tFamily loaded!");
                        }
                        catch (Exception e)
                        {
                            stringBuilder.AppendLine("\t" + e.Message);
                            stringBuilder.AppendLine("Family skipped");
                        }
                    }

                }
                else
                {
                    stringBuilder.AppendLine($"INVALID ID: {id.ToString()}");
                }
            }

            TaskDialog logTaskDialog = new TaskDialog("DBO Parameters Removal")
            {
                Title = "Parameters removal",
                MainInstruction = "Operation completed.",
                MainContent = stringBuilder.ToString(),
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
}
