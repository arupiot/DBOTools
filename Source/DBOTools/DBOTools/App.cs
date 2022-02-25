#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

#endregion

namespace DBOTools
{
    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
            #region Create Tab and Panels

            // Create the ribbon tab or get existing
            try
            {
                a.CreateRibbonTab(Constants.ribbonTabName);
            }
            catch (Exception) // In case the tab already exists
            {

            }

            // Create the ribbon panels or get existing (see the function from Helpers)
            RibbonPanel ribbonPanelDBOParameters = Helpers.CreateRibbonPanel(a, PanelNames.DBOParameters);
            RibbonPanel ribbonPanelDBOValidation = Helpers.CreateRibbonPanel(a, PanelNames.DBOValidation);
            RibbonPanel ribbonPanelDBOExport = Helpers.CreateRibbonPanel(a, PanelNames.DBOExport);
            RibbonPanel ribbonPanelDBOInfo = Helpers.CreateRibbonPanel(a, PanelNames.DBOInfo);

            #endregion

            #region Creating Buttons
            // Icons created in ButtonsPanelsIcons

            PushButton injectParametersButton = Helpers.CreatePushButton(
                Buttons._injParamButton,
                ribbonPanelDBOParameters,
                true);

            PushButton populateDBOExportAsButton = Helpers.CreatePushButton(
                Buttons._populateDBOExportAsButton,
                ribbonPanelDBOParameters,
                true);

            PushButton editDBOTypesButton = Helpers.CreatePushButton(
                Buttons._editDBOTypesButton,
                ribbonPanelDBOParameters,
                true);

            PushButton validationButton = Helpers.CreatePushButton(
                Buttons._DBOValidationButton,
                ribbonPanelDBOValidation,
                true);

            PushButton exportToCSVButton = Helpers.CreatePushButton(
                Buttons._exportToCSVButton,
                ribbonPanelDBOExport,
                true);

            PushButton exportToYAMLButton = Helpers.CreatePushButton(
                Buttons._exportToYAMLButton,
                ribbonPanelDBOExport,
                true);

            PushButton exportToRDFButton = Helpers.CreatePushButton(
                Buttons._exportToRDFButton,
                ribbonPanelDBOExport,
                true);

            PushButton infoButton = Helpers.CreatePushButton(
                Buttons._exportToRDFButton,
                ribbonPanelDBOInfo,
                true);

            #endregion

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
