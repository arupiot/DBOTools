# Create a yaml file with the content of the active revit model following the ODI standards

import sys
import clr
import System
import rpw
import yaml
import pprint
import json

from System.Collections.Generic import *

clr.AddReference("RevitAPI")
from Autodesk.Revit.DB import *

from rpw.ui.forms import *
from Autodesk.Revit.UI.Selection import ObjectType

doc = __revit__.ActiveUIDocument.Document
uidoc = __revit__.ActiveUIDocument
app = doc.Application
pp = pprint.PrettyPrinter(indent=1)
shared_param_file = app.OpenSharedParameterFile()

