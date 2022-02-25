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
public class ExtEvent_PopulateTypeParameters : IExternalEventHandler
    {
        List<DBOFamilyType> SelectedFamilyTypes;

        public ExtEvent_PopulateTypeParameters
            (List<DBOFamilyType> selectedFamilyTypes)
        {
            SelectedFamilyTypes = selectedFamilyTypes;
        }

        public void Execute(UIApplication uiapp)
        {
            Document doc = uiapp.ActiveUIDocument.Document;

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Starting edit of DBO Implements and DBO Uses");
            //stringBuilder.AppendLine($"Selected {SelectedFamilyTypes.Count.ToString()}" +
            //    $" DBOFamilyTypes");

            stringBuilder.AppendLine("Starting transaction...");
            Transaction t = new Transaction(doc);

            try
            {
                t.Start("Edit DBO Uses and Implements");
                stringBuilder.AppendLine("Transaction started.");
                foreach (DBOFamilyType dboFamilyType in SelectedFamilyTypes)
                {
                    stringBuilder.AppendLine($"Editing {dboFamilyType.FullName}");

                    Element element = doc.GetElement(dboFamilyType.Id);
                    if (element is FamilySymbol)
                    {
                        FamilySymbol familySymbol = element as FamilySymbol;

                            // Edit the Implements:
                            Parameter implementsParam = familySymbol.LookupParameter
                                (DBOParameters.dboImplements.Name);

                            if (implementsParam != null)
                            {
                                implementsParam.Set(string.Join(" ", dboFamilyType.Implements));
                            }


                            // Edit the Uses:
                            Parameter usesParam = familySymbol.LookupParameter
                                (DBOParameters.dboUses.Name);

                            if (usesParam != null)
                            {
                                usesParam.Set(string.Join(" ", dboFamilyType.Uses));
                            }
                    }
                    else
                    {
                        stringBuilder.AppendLine($"Error: The DBOFamilyType is not " +
                            $"pointing at a Revit Family Symbol. Id: " +
                            $"{dboFamilyType.Id.ToString()}");
                    }
                }
                t.Commit();
                stringBuilder.AppendLine("\tTransaction committed.");

            }
            catch (Exception eT)
            {
                stringBuilder.AppendLine($"Error: {eT.Message}");
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
