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

    public class ExtEvent_PopulateExportAs : IExternalEventHandler
    {
        List<DBOFamily> SelectedFamilies;
        public ExtEvent_PopulateExportAs
            (List<DBOFamily> selectedFamilies)
        {
            SelectedFamilies = selectedFamilies;
        }


        public void Execute(UIApplication uiapp)
        {
            Document doc = uiapp.ActiveUIDocument.Document;
            
            StringBuilder stringBuilder = new StringBuilder();

            Transaction t = new Transaction(doc);
            stringBuilder.AppendLine("Starting transaction...");

            try
            {
                t.Start("Edit DBO Export As");

                foreach (DBOFamily dboFamily in SelectedFamilies)
                {
                    Element element = doc.GetElement(dboFamily.Id);

                    if ( element is Family)
                    {
                        Family family = element as Family;
                        //stringBuilder.AppendLine($"Editing family {dboFamily.FullName}");
                        stringBuilder.AppendLine($"Editing family {family.Name}");

                        try
                        {
                            string entityTypeAsString = dboFamily.ExportAs.Name;
                            stringBuilder.AppendLine($"Setting \"DBO Export As\" to: {entityTypeAsString}...");

                            /*
                            List<FamilySymbol> familySymbols = family
                                .GetFamilySymbolIds()
                                .Select(symbolId => doc.GetElement(symbolId))
                                .Cast<FamilySymbol>()
                                .ToList();
                            */

                            stringBuilder.AppendLine($"Family Types found in Family: {family.GetFamilySymbolIds().Count}");
                            foreach (ElementId elementId in family.GetFamilySymbolIds())
                            {
                                Element _element = doc.GetElement(elementId);

                                if (_element is FamilySymbol)
                                {
                                    FamilySymbol symbol = _element as FamilySymbol;

                                    stringBuilder.AppendLine($"\tEditing type " +
                                        $"{symbol.LookupParameter("Type Name").AsString()}...");
                                    Parameter p = symbol.LookupParameter(
                                            DBOParameters.dboExportAs.Name);
                                    if (p != null)
                                    {
                                        bool success = p.Set(entityTypeAsString);
                                        if (success)
                                        {
                                            stringBuilder.AppendLine("\tType successfully edited!");
                                        }
                                        else
                                        {
                                            stringBuilder.AppendLine("\tERROR: Type not edited.");
                                        }
                                    }
                                    else
                                    {
                                        stringBuilder.AppendLine("Parameter not found.");
                                    }
                                }
                            }
                            stringBuilder.AppendLine("\tFinished editing family.");
                        }
                        catch (Exception ee)
                        {
                            stringBuilder.AppendLine($"ERROR: {ee.Message}");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Error: DBO Family is not pointing at Revit Family. " +
                            $"Id: {dboFamily.Id.ToString()}");
                    }
                }

                t.Commit();
                stringBuilder.AppendLine("Transaction committed!");
            }
            catch (Exception e)
            {
                stringBuilder.AppendLine($"ERROR: {e.Message}");
            }



            TaskDialog logTaskDialog = new TaskDialog("DBO Parameters Removal")
            {
                Title = "Populate DBO Export As",
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
