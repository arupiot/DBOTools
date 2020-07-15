##Open excel file, create a list of the parameters and add them to the family as shared parameters
import sys
import clr
import System
import yaml
import pprint

from System.Collections.Generic import *

clr.AddReference("RevitAPI")
from Autodesk.Revit.DB import *

from rpw.ui.forms import *

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
                print("Parameter with name {} already exists in group {}. Parameter not created.".format(definition.Name, random_group.Name))
                same_name = True

    if same_name == False:
        ext_def_creation_options = ExternalDefinitionCreationOptions(param_name, param_type)
        ext_def_creation_options.UserModifiable = usermodifiable
        ext_def_creation_options.Description = description
        new_definition = group.Definitions.Create(ext_def_creation_options)
        print("Created external definition \"{}\" in group {}".format(new_definition.Name, group.Name))

def paramTypeName2ParamType (paramTypeName):
    all_param_types = System.Enum.GetValues(ParameterType)
    for param_type in all_param_types:
        if param_type.ToString() == paramTypeName:
            param_type_match = param_type
    if param_type_match:
        return param_type_match
    else:
        return None

app = doc.Application
pp = pprint.PrettyPrinter(indent=4)
shared_param_file = app.OpenSharedParameterFile()

if shared_param_file == None:
    print("No Shared Parameter File found. Please open or create a Shared Parameter File")
else:

    yaml_path = select_file("Yaml File (*.yaml)|*.yaml", "Select the yaml file with the parameters", multiple = False, restore_directory = True)
    if yaml_path:
        with open(yaml_path, "r") as stream:
            parameters_yaml = yaml.safe_load(stream)

        for param in parameters_yaml.items():
            param_name = param[0]
            print(param_name)
            group_name = param[1].get("GroupName")
            builtin_group = param[1].get("BuiltinGroup")
            param_type_name = param[1].get("ParameterType")
            param_type = paramTypeName2ParamType(param_type_name)
            user_modifiable = param[1].get("UserModifiable")
            description = param[1].get("Description")
            if param_name and group_name and builtin_group and param_type and user_modifiable and description:
                new_group = createnewgroup(shared_param_file,group_name)
                if new_group:
                    definition = create_definition(new_group, shared_param_file, param_name, param_type, user_modifiable, description)
            else:
                print("Please check the parameter template has all information populated")
    else:
        print("Please specify a yaml file")