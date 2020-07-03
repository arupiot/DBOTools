import sys
import clr
import System
import yaml
import pprint

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

def parameterName2ExternalDefinition(sharedParamFile, definitionName):
    """
    Given the name of a parameter, return the definition from the shared parameter file
    """
    externalDefinition = None
    for group in sharedParamFile.Groups:
        for definition in group.Definitions:
            if definition.Name == definitionName:
                externalDefinition = definition
    return externalDefinition

def createnewgroup(shared_parameter_file, new_group_name):
    try:
        newgroup = shared_parameter_file.Groups.Create(new_group_name)
        print("Group successfully created with name: {}".format(new_group_name))
    except:
        all_groups = []
        for group in shared_parameter_file.Groups:
            all_groups.append(group.Name)
        if new_group_name in all_groups:
            print("A group already exists with the following name: {}".format(new_group_name))
            for group in shared_parameter_file.Groups:
                if group.Name == new_group_name:
                    newgroup = group
        else:
            print("Something went wrong. The group with name {} was not created".format(new_group_name))
            sys.exit("Script has ended")
    return newgroup

def create_definition (group, shared_parameter_file, param_name, param_type, usermodifiable, description):
    same_name = False
    new_definition = None

    for random_group in shared_parameter_file.Groups:
        for definition in random_group.Definitions:
            if definition.Name == param_name:
                print("Parameter with name {} already exists in group {} a. External definition not created.".format(definition.Name, random_group.Name))
                same_name = True

    if same_name == False:
        ext_def_creation_options = ExternalDefinitionCreationOptions(param_name, param_type)
        ext_def_creation_options.UserModifiable = usermodifiable
        ext_def_creation_options.Description = description
        new_definition = group.Definitions.Create(ext_def_creation_options)
        print("Created external definition \"{}\" in group {}".format(new_definition.Name, group.Name))

app = doc.Application
pp = pprint.PrettyPrinter(indent=4)
shared_param_file = app.OpenSharedParameterFile()

if doc.IsFamilyDocument:
    if shared_param_file == None:
        print("No Shared Parameter File found. Please open or create a Shared Parameter File")
    else:
        yaml_path = select_file("Yaml File (*.yaml)|*.yaml", "Select the yaml file with the Revit Family", multiple = False, restore_directory = True)
        if yaml_path:
            with open(yaml_path, "r") as stream:
                family_yaml = yaml.safe_load(stream)
                parameter_names_list = []
                for family in family_yaml.items():
                    if family[1].get("is_canonical") == True:
                        for implement in family[1].get("implements"):
                            parameter_names_list.append(implement)

                parameter_names_list = list(dict.fromkeys(parameter_names_list))

                implements_group = createnewgroup(shared_param_file, "Implements")
                
                for param_name in parameter_names_list:
                    
                    if implements_group:
                        definition = create_definition(implements_group, shared_param_file, param_name, ParameterType.YesNo, True, "")
                
                parameters_in_family = doc.FamilyManager.Parameters
                parameters_in_family_names = [param.Definition.Name for param in parameters_in_family]

                t = Transaction(doc, "Add parameters")
                t.Start()
                for param_name in parameter_names_list:
                    if param_name in parameters_in_family_names:
                        print("Parameter \"{}\" already in family".format(param_name))
                    else:
                        ext_def = parameterName2ExternalDefinition(shared_param_file, param_name)
                        if ext_def != None:
                            doc.FamilyManager.AddParameter(ext_def, BuiltInParameterGroup.PG_IDENTITY_DATA, False)
                            print("Parameter \"{}\" added to family".format(param_name))
                        else:
                            print("Parameter \"{}\" is not in the Shared Parameter File".format(param_name))
                t.Commit()
        else:
            print("Please specify a yaml file")
else:
    print("Please run the script in a Family Document")