using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Windows.Controls;


namespace DBOTools
{
    public class Helpers : IExternalCommand
    {
        public static Definition DefinitionFromName(DefinitionFile sharedParameterFile, string parameterName)
        {
            Definition definition = null;

            foreach (DefinitionGroup group in sharedParameterFile.Groups)
            {
                foreach (Definition _d in group.Definitions)
                {
                    if (_d.Name == parameterName)
                    {
                        definition = _d;
                    }
                }
            }

            return definition;
        }

        public static List<string> GetAllInstanceParameterNames(Document doc)
        {
            List<string> instanceParameterNames = new List<string>() { };

            List<FamilyInstance> familyInstances = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType()
                .Cast<FamilyInstance>().ToList();

            familyInstances = familyInstances
                .GroupBy(x => x.Symbol.Family.Id.ToString())
                .Select(x => x.FirstOrDefault())
                .ToList();

            foreach (FamilyInstance familyInstance in familyInstances)
            {
                instanceParameterNames.AddRange(
                    familyInstance.Parameters.Cast<Parameter>().ToList()
                        .Select(x => x.Definition.Name)
                    );
            }

            instanceParameterNames = instanceParameterNames.Distinct().ToList();
            instanceParameterNames.Sort();

            return instanceParameterNames;
        }

        public static List<string> GetAllTypeParameterNames(Document doc)
        {
            List<string> typeParameterNames = new List<string>() { };

            List<FamilySymbol> familySymbols = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol)).WhereElementIsElementType()
                .Cast<FamilySymbol>().ToList();

            familySymbols = familySymbols
                .GroupBy(x => x.Family.Id.ToString())
                .Select(x => x.FirstOrDefault())
                .ToList();

            foreach (FamilySymbol familySymbol in familySymbols)
            {
                typeParameterNames.AddRange(
                    familySymbol.Parameters.Cast<Parameter>().ToList()
                        .Select(x => x.Definition.Name)
                    );
            }

            typeParameterNames = typeParameterNames.Distinct().ToList();
            typeParameterNames.Sort();

            return typeParameterNames;
        }

        public static EntityType GetEntityTypeByName(string name)
        {
            return EntityType.EntityTypes.Where(x => x.Name == name).FirstOrDefault();
        }

        public static PushButton CreatePushButton(
            _Button _button, 
            RibbonPanel ribbonPanel, 
            bool enableButton)
        {
            // To create new buttons check the _Button class

            PushButtonData pushButtonData = new PushButtonData(
                _button.Name,
                _button.Text,
                Assembly.GetExecutingAssembly().Location,
                _button.AssociatedCommand)
            {
                ToolTip = _button.ToolTip,
                LongDescription = _button.LongDescription,
                Image = _button.IconSmall,
                LargeImage = _button.IconLarge
            };

            PushButton pushButton = ribbonPanel.AddItem(pushButtonData)
                as PushButton;

            pushButton.Enabled = enableButton;

            return pushButton;
        }

        public static RibbonPanel CreateRibbonPanel(
            UIControlledApplication a,
            string panelName)
        {
            RibbonPanel newPanel = a.GetRibbonPanels(
                Constants.ribbonTabName)
                .Where(x => x.Name == panelName)
                .FirstOrDefault();

            if (newPanel == null)
            {
                newPanel = a.CreateRibbonPanel(Constants.ribbonTabName, panelName);
            }

            return newPanel;
        }

        /*
        public static BitmapSource GetBitmapSource(Image img)
        {
            BitmapImage bmp = new BitmapImage();

            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                ms.Position = 0;

                bmp.BeginInit();

                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = null;
                bmp.StreamSource = ms;

                bmp.EndInit();

            }

            return bmp;
        }
        */


        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            throw new NotImplementedException();
        }
    }
}
