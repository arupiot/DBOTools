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
    public class ExtEvent_InjectParameters : IExternalEventHandler
    {
        List<ElementId> SelectedFamiliesIds;
        List<DBOParameter> ParametersToAdd;

        public ExtEvent_InjectParameters
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
                            famT.Start("Inject DBO Parameters");
                            stringBuilder.AppendLine("\tTransaction started...");
                            foreach (DBOParameter dboParameter in ParametersToAdd)
                            {
                                // Check if parameter is already present
                                if (parameterNames.Contains(dboParameter.Name))
                                {
                                    stringBuilder.AppendLine($"\tFamily already contains" +
                                        $" {dboParameter.Name}");
                                }
                                else
                                {
                                    try
                                    {
                                        ExternalDefinition extDef = dboParameter.GetDefinition(sharedParameterFile)
                                            as ExternalDefinition;

                                        if (extDef != null)
                                        {
                                            FamilyParameter p = familyManager.AddParameter
                                            (
                                            extDef,
                                            dboParameter.BuiltInParameterGroup,
                                            dboParameter.IsInstance
                                            );

                                            if (p != null)
                                            {
                                                stringBuilder.AppendLine($"\tSuccessfully added {dboParameter.Name} to family");
                                            }
                                            else
                                            {
                                                stringBuilder.AppendLine($"\tFailed adding {dboParameter.Name} to family");
                                            }
                                        }
                                        else
                                        {
                                            stringBuilder.AppendLine($"\tCannot find {dboParameter.Name} " +
                                                $"in shared parameter file");
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
}
