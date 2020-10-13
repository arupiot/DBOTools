# YAML TO GOOGLE PARAMETER
# ADD ALL PARAMETERS OF A GROUP NAME TO THE SELECTED FAMILY
# INPUT: YAML FILE -> SELECT GROUP NAME

# YAML TO FAMILY DOCUMENT PARAMETERS

import sys
print(sys.version)
print(sys.path)
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

import rpw

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

def builtinGroupFromName(builtin_group_name):
    b_i_groups = System.Enum.GetValues(BuiltInParameterGroup)
    builtin_group = None
    for g in b_i_groups:
        if g.ToString() == builtin_group_name:
            builtin_group = g
    
    if builtin_group != None:
        return builtin_group
    else:
        print("Built-in Group not valid: {}".format(builtin_group_name))
        return None

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
            print("Something went wrong. The group with name {} was not created. Please check Shared Parameter File is not read-only.".format(new_group_name))
            sys.exit("Script has ended")
    return newgroup

def create_definition (group, shared_parameter_file, param_name, param_type, usermodifiable, description):
    same_name = False
    new_definition = None

    for random_group in shared_parameter_file.Groups:
        for definition in random_group.Definitions:
            if definition.Name == param_name:
                print("Parameter with name {} already exists in group {}. External definition not created.".format(definition.Name, random_group.Name))
                same_name = True

    if same_name == False:
        ext_def_creation_options = ExternalDefinitionCreationOptions(param_name, param_type)
        ext_def_creation_options.UserModifiable = usermodifiable
        ext_def_creation_options.Description = description
        new_definition = group.Definitions.Create(ext_def_creation_options)
        print("Created external definition \"{}\" in group {}".format(new_definition.Name, group.Name))

doc = __revit__.ActiveUIDocument.Document
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
                parameters_yaml = yaml.safe_load(stream)
                parameter_groups_list = []
                for param in parameters_yaml.items():
                    group_name = param[1]["GroupName"]
                    parameter_groups_list.append(group_name)
                parameter_groups_list = list(dict.fromkeys(parameter_groups_list))

                form_title = "Select a parameter group:"
                group = rpw.ui.forms.SelectFromList(form_title,parameter_groups_list,description=None,sort=True,exit_on_close=True)

                # Select group from shared parameter file
                sh_param_group = None
                parameters_list = []
                warning = []
                created_params = []
                for sh_p_group in shared_param_file.Groups:
                    if sh_p_group.Name == group:
                        sh_param_group = sh_p_group
                if sh_param_group:
                    print("Group found in shared parameter file: {}".format(sh_param_group.Name))
                    print("Following parameters will be added to the family:")
                    for definition in sh_param_group.Definitions:
                        print(definition.Name)
                        parameters_list.append(definition)
                        
                    t = Transaction(doc, "Add parameters")
                    t.Start()
                    for definition in parameters_list:
                        param_name = definition.Name
                        print("Processing parameter \"{}\"...".format(param_name))
                        param_from_yaml = dict(filter(lambda elem : elem[0] == param_name, parameters_yaml.items()))

                        print("Parameter \"{}\" found in yaml file".format(param_from_yaml.keys()))
                        if param_from_yaml:
                            builtin_group = None
                            is_instance = None

                            builtin_group_name = param_from_yaml[param_name]["BuiltinGroup"]
                            builtin_group = builtinGroupFromName(builtin_group_name)
                            is_instance = param_from_yaml[param_name]["IsInstance"]

                            if builtin_group != None:
                                if is_instance != None:
                                    doc.FamilyManager.AddParameter(definition, builtin_group, is_instance)
                                    print("Parameter \"{}\" added to family".format(param_name))
                                    created_params.append([param_name, builtin_group, is_instance])
                                else:
                                    warning_message = "IsInstance missing in yaml file for parameter \"{}\". Parameter not created".format(param_name)
                                    warning.append([param_name, warning_message])
                                    print(warning_message)
                            else:
                                warning_message = "Built-in Group missing in yaml file for parameter \"{}\". Parameter not created".format(param_name)
                                warning.append([param_name, warning_message])
                                print(warning_message)
                        else:
                            warning_message = "Parameter \"{}\" is not in the Shared Parameter File".format(param_name)
                            warning.append([param_name, warning_message])
                            print(warning_message)
                    t.Commit()

                    print("Script has ended.")
                    if warning_message == []:
                        print("Warnings: None")
                    else:
                        print("Warnings:")
                        pp.pprint(warning_message)

                else:
                    print("Group not found in shared parameter file: {}".format(group))
           
        else:
            print("Please specify a yaml file")
else:
    print("Please run the script in a Family Document")