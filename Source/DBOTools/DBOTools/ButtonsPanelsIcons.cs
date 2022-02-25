using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DBOTools.Properties;


namespace DBOTools
{
    public static class PanelNames
    {
        public const string DBOParameters = "DBO Parameters Tools";
        public const string DBOValidation = "Validation Tools";
        public const string DBOExport = "Export to file";
        public const string DBOInfo = "Info";
    }

    public static class Icons
    {
        //public static ImageSource placeholderIconSmall = new BitmapImage(new Uri(
        //    "pack://application:,,,/DBOTools;component/Icons/PlaceholderIcon16x16.ico"));

        public static ImageSource placeholderIconSmall = System.Windows.Interop.Imaging.
            CreateBitmapSourceFromHBitmap(
                Resources.PlaceholderIcon16x16.ToBitmap().GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(16,16));

        public static ImageSource placeholderIconLarge = System.Windows.Interop.Imaging.
            CreateBitmapSourceFromHBitmap(
            Resources.PlaceholderIcon32x32.ToBitmap().GetHbitmap(),
            IntPtr.Zero,
            System.Windows.Int32Rect.Empty,
            BitmapSizeOptions.FromWidthAndHeight(32,32));

        //public static ImageSource placeholderIconLarge = new BitmapImage(new Uri(
        //    "pack://application:,,,/DBOTools;component/Icons/PlaceholderIcon32x32.ico"));
    }

    public class _Button
    {
        public string Name { get; }
        public string Text { get; }
        public string AssociatedCommand { get; }
        public string ToolTip { get; }
        public string LongDescription { get; }
        public ImageSource IconSmall { get; }
        public ImageSource IconLarge { get; }

        public _Button(
            string name,
            string text,
            string associatedCommand,
            string tooltip,
            string longDescription,
            ImageSource iconSmall,
            ImageSource iconLarge)
        {
            Name = name;
            Text = text;
            AssociatedCommand = associatedCommand;
            ToolTip = tooltip;
            LongDescription = longDescription;
            IconSmall = iconSmall;
            IconLarge = iconLarge;
        }
    }

    public class Buttons
    {
        public static readonly _Button _injParamButton = new _Button(
            "Inject Parameters",
            "Inject Parameters",
            "DBOTools.DisplayInjectParameters",
            "Inject DBO Parameters",
            "Inject DBO Parameters into families to prime the model" +
                " for the creation of the ontologies. " +
                "Please have the DBO Shared Parameter File loaded. " +
                "If you don't have it available, " +
                "download it from the DBO Tools repository " +
                "on GitHub. Click on the Info button for the link.",
            Icons.placeholderIconSmall,
            Icons.placeholderIconLarge
            );

        public static readonly _Button _populateDBOExportAsButton = new _Button(
            "Populate DBO Export As",
            "Populate DBO Export As",
            "DBOTools.DisplayPopulateDBOExportAs",
            "Populate the type parameter DBO Export As",
            "Open the dialog to populate the type parameter DBO Export As "+
                "the families whose instances and types will form the ontology. " +
                "Please populate DBO Export As only via this tool and do not " +
                "manually edit the values. For more information please refer " +
                "to the Digital Building Project, please click on the Info " +
                "button for the link.",
            Icons.placeholderIconSmall,
            Icons.placeholderIconLarge
            );

        public static readonly _Button _editDBOTypesButton = new _Button(
            "Edit DBO Types",
            "Edit DBO Types",
            "DBOTools.DisplayDBOEditTypes",
            "Populate the DBO Type Parameters",
            "Edit DBO Export As, DBO Uses and DBO Implements type parameters",
            Icons.placeholderIconSmall,
            Icons.placeholderIconLarge
            );

        public static readonly _Button _DBOValidationButton = new _Button(
            "DBO Validation",
            "DBO Validation",
            "DBOTools.WIP",
            "Validation tools TBC",
            "Validation tools TBC",
            Icons.placeholderIconSmall,
            Icons.placeholderIconLarge
            );

        public static readonly _Button _exportToCSVButton = new _Button(
            "Export CSV",
            "Export CSV",
            "DBOTools.WIP",
            "Export the ontology entities to CSV",
            "Export the ontology entities to CSV",
            Icons.placeholderIconSmall,
            Icons.placeholderIconLarge
            );

        public static readonly _Button _exportToYAMLButton = new _Button(
            "Export YAML",
            "Export YAML",
            "DBOTools.WIP",
            "Export the ontology entities to YAML",
            "Export the ontology entities to YAML",
            Icons.placeholderIconSmall,
            Icons.placeholderIconLarge
            );

        public static readonly _Button _exportToRDFButton = new _Button(
            "Export RDF",
            "Export RDF",
            "DBOTools.WIP",
            "Export the ontology entities to RDF",
            "Export the ontology entities to RDF",
            Icons.placeholderIconSmall,
            Icons.placeholderIconLarge
            );

        public static readonly _Button _infoButton = new _Button(
           "Info",
           "Info",
           "DBOTools.Info",
           "Display information, credits and useful links",
           "Display information, credits and useful links",
           Icons.placeholderIconSmall,
           Icons.placeholderIconLarge
           );
    }
}
