import sys
import clr
import System
import yaml

from System.Collections.Generic import *

clr.AddReference("RevitAPI")
from Autodesk.Revit.DB import *

clr.AddReference("RevitServices")
from RevitServices.Persistence import DocumentManager
from RevitServices.Transactions import TransactionManager

clr.AddReference("RevitAPIUI")
from Autodesk.Revit.UI import *
from Autodesk.Revit import Creation

from rpw.ui.forms import *

app = doc.Application

#family_templates_path = app.FamilyTemplatePath

family_template = select_file("Revit Family Template (*.rft)|*.rft", "Select the Revit Family Template file",multiple = False)
if family_template:
    pass
else:
    sys.exit("Please specify a Revit Family Template file")

famdoc = app.NewFamilyDocument(family_template)

new_type_name = "New_Type_Name"

famt = Transaction(famdoc, "Creation of new family type)")

famt.Start()
newtype = famdoc.FamilyManager.NewType(new_type_name)
famt.Commit()

types = famdoc.FamilyManager.Types

for fam_type in types:
    print(fam_type.Name)
    print([param.Definition.Name for param in famdoc.FamilyManager.Parameters])

famt = Transaction(famdoc, "Change current type")

famt.Start()
famdoc.FamilyManager.CurrentType = newtype

