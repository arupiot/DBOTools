# Open family
# Open yaml
# Select family
# Add parameters to shared parameter file
# Add BOS_Entity_Type parameter
# Add parameters to family
# Close, load back

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
pp = pprint.PrettyPrinter(indent=0,stream=None)
shared_param_file = app.OpenSharedParameterFile()

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

def NameAndGroup2ExternalDefinition(sharedParamFile, definitionName, groupName):
    external_definition = None
    group_found = False
    group_matches = None
    for group in sharedParamFile.Groups:
        if group.Name == groupName:
            group_found = True
            group_matches = group
    
    if group_found == True:
        for definition in group_matches.Definitions:
            if definition.Name == definitionName:
                external_definition = definition
    else:
        print("Group not found with name: {}".format(groupName))
    
    return external_definition

def create_definition (group_name, shared_parameter_file, param_name, param_type, usermodifiable, description):

    new_definition = None
    group_matches = False
    group = None
    definition_matches = False

    for existing_group in shared_parameter_file.Groups:
        if existing_group.Name == group_name:
            group_matches = True
            group = existing_group
    
    if group_matches == True:
        for existing_definition in group.Definitions:
            if existing_definition.Name == param_name:
                definition_matches = True
    
        if definition_matches == False:
            ext_def_creation_options = ExternalDefinitionCreationOptions(param_name, param_type)
            ext_def_creation_options.UserModifiable = usermodifiable
            ext_def_creation_options.Description = description
            new_definition = group.Definitions.Create(ext_def_creation_options)
            print("Created external definition \"{}\" in group \"{}\"".format(new_definition.Name, group.Name))    
        else:
            print("Extenal definition already exists with name \"{}\" in group \"{}\"".format(param_name, group.Name))
    else:
        print("Couldn't find a group with name {} in shared parameter file".format(group_name))

if doc.IsFamilyDocument == True:
    
    yaml_path = select_file("Yaml File (*.yaml)|*.yaml", "Select the yaml file with the parameters", multiple = False, restore_directory = True)
    if yaml_path:
        with open(yaml_path, "r") as stream:
            ontology_yaml = yaml.safe_load(stream)

        file_name_split = yaml_path.split("\\")
        file_name_with_ext = file_name_split[-1]

        file_name_with_ext_split = file_name_with_ext.split(".")
        group_name = file_name_with_ext_split[0]

        canonical_types = dict(filter(lambda elem : elem[1].get("is_canonical") == True, ontology_yaml.items()))
        parameter_names = []

        for canonical_type in canonical_types.items():
            implements_params = canonical_type[1]["implements"]
            for implement_param in implements_params:
                parameter_names.append(implement_param)
        
        parameter_names = list(dict.fromkeys(parameter_names))

        print("The following parameters will be added to the shared parameter file (if not present) and to the family:")
        for p in parameter_names:
            print(p)
        
        warnings = []
        t = Transaction(doc, "Add parameters")
        t.Start()
        # Creation of "Entity_Type" parameter
        # Adding entity_type to shared parameter file
        entity_type_param_name = "Entity_Type"
        common_parameters_group = createnewgroup(shared_param_file, "Common_Parameters")
        create_definition("Common_Parameters", shared_param_file, entity_type_param_name, ParameterType.Text, True, "")

        # Adding entity_type to family
        parameters_in_family = doc.FamilyManager.Parameters
        parameters_in_family_names = [param.Definition.Name for param in parameters_in_family]

        if entity_type_param_name in parameters_in_family_names:
            message = "Parameter \"{}\" already in family".format(entity_type_param_name)
            print(message)
            warnings.append(message)
        else:
            ext_def = NameAndGroup2ExternalDefinition(shared_param_file, entity_type_param_name, "Common_Parameters")
            doc.FamilyManager.AddParameter(ext_def, BuiltInParameterGroup.PG_IDENTITY_DATA, True)
            print("Parameter \"{}\" added to family as an Instance Parameter".format(entity_type_param_name))

        # Add all other parameters from yaml
        category_group = createnewgroup(shared_param_file, group_name)
        for p_n in parameter_names:
            param_name = "Implements_"+p_n
            if param_name in parameters_in_family_names:
                message = "Parameter \"{}\" already in family".format(param_name)
                print("Parameter \"{}\" already in family".format(param_name))
                warnings.append(message)
            else:
                create_definition(category_group.Name, shared_param_file, param_name, ParameterType.YesNo, True, "")
                ext_def = NameAndGroup2ExternalDefinition(shared_param_file, param_name, group_name)
                if ext_def != None:
                    doc.FamilyManager.AddParameter(ext_def, BuiltInParameterGroup.PG_IDENTITY_DATA, True)
                    print("Parameter \"{}\" added to family as an Instance Parameter".format(param_name))
                else:
                    message = "Parameter \"{}\" is not in the Shared Parameter File".format(param_name)
                    print("Parameter \"{}\" is not in the Shared Parameter File".format(param_name))
                    warnings.append(message)
        t.Commit()
        print("Script has ended.")
        if warnings == []:
            print("Warnings: None")
        else:
            print("Warnings:")
            for w in warnings:
                print(w)

    else:
        print("Please specify a yaml file")

else:
    print("Please run the script in a Family Document.")