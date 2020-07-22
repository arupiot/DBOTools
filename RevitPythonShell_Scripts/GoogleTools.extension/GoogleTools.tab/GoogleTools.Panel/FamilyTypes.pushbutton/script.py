# PANEL WITH LIST OF FAMILY TYPES FROM YAML

import sys
import clr
import System
import rpw
import yaml
import pprint

from System.Collections.Generic import *

clr.AddReference("RevitAPI")
from Autodesk.Revit.DB import *

from rpw.ui.forms import *

doc = __revit__.ActiveUIDocument.Document
app = doc.Application
#pp = pprint.PrettyPrinter(indent=10)
shared_param_file = app.OpenSharedParameterFile()

yaml_path = select_file("Yaml File (*.yaml)|*.yaml", "Select the yaml file with the parameters", multiple = False, restore_directory = True)
if yaml_path:
    with open(yaml_path, "r") as stream:
        ontology_yaml = yaml.safe_load(stream)

    form_title = "Select a family type:"
    canonical_types = dict(filter(lambda elem : elem[1].get("is_canonical") == True, ontology_yaml.items()))
    options = canonical_types.keys()
    mytype = rpw.ui.forms.SelectFromList(form_title,options,description=None,sort=True,exit_on_close=True)

    pprint.pprint(dict(filter(lambda elem: elem [0] == mytype, canonical_types.items())))

else:
    print("Please specify a yaml file")