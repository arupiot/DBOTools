# Select an element
# Open yaml file with entity types
# If parameters are already present, set values according to yaml input

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

selection = [doc.GetElement(element_Id) for element_Id in uidoc.Selection.GetElementIds()]

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
            print("Group_matches: {}".format(group_matches))
    
    if group_matches == True:
        for existing_definition in group.Definitions:
            if existing_definition.Name == param_name:
                definition_matches = True
                print("Definition matches:".format(definition_matches))
    
        if definition_matches == False:
            ext_def_creation_options = ExternalDefinitionCreationOptions(param_name, param_type)
            ext_def_creation_options.UserModifiable = usermodifiable
            ext_def_creation_options.Description = description
            new_definition = group.Definitions.Create(ext_def_creation_options)
            print("Created external definition \"{}\" in group \"{}\"".format(new_definition.Name, group.Name))    
        else:
            print("Extenal definition already exists with name \"{}\" in group \"{}\"".format(param_name, group.Name))
    else:
        print("Group doesn't match")

family_instances = []
not_family_instances = []

print("Selected {} items".format(len(selection)))

for item in selection:
    if type(item).__name__ == "FamilyInstance":
        family_instances.append(item)
    else:
        not_family_instances.append(item)

print("The following elements are family instances and will receive the parameter values from the ontology:")
if family_instances == []:
    print("None")
else:
    print([item.Id.ToString() for item in family_instances])

print("The following elements are not family instances and will be dropped from the selection:")
if not_family_instances == []:
    print("None")
else:
    print([item.Id.ToString() for item in not_family_instances])

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
    param_names_with_prefix = []

    for pn in parameter_names:
        param_name_with_prefix = "Implements_" + pn
        param_names_with_prefix.append(param_name_with_prefix)
    param_names_with_prefix.append("Entity_Type")

    #print(param_names_with_prefix)

    # Check if item has the parameters:
    print("Checking if family instances have the required parameters...")
    for family_instance in family_instances:
        all_params = family_instance.Parameters
        all_params_names = [param.Definition.Name for param in all_params]
        #pp.pprint(all_params_names)

        missing_params = []
        for param_name in param_names_with_prefix:
            if param_name in all_params_names:
                pass
            else:
                missing_params.append(param_name)
        
        if missing_params == []:
            print("Family instance {} has all required parameters.".format(family_instance.Id.ToString()))
        else:
            print("Family instance {} is missing the following parameters".format(family_instance.Id))
            pp.pprint(missing_params)
            family_instances.remove(family_instance)
            print("Family instance {} removed from the list of objects to modify")

    # ADD SELECTION OF TYPE THROUGH MENU
    print("Please select an entity type from the yaml ontology...")
    form_title = "Select an entity type:"
    canonical_types = dict(filter(lambda elem : elem[1].get("is_canonical") == True, ontology_yaml.items()))
    options = canonical_types.keys()
    entity_type_name = rpw.ui.forms.SelectFromList(form_title,options,description=None,sort=True,exit_on_close=True)

    entity_type_dict = (dict(filter(lambda elem: elem [0] == entity_type_name, canonical_types.items())))
    # print("Printing selected entity type:")
    # pp.pprint(entity_type_dict)

    print("NO INDENT")
    # pprint.pprint(entity_type_dict, width=1, sort_keys = True, compact=False)

    print(json.dumps(entity_type_dict, indent = 4))

    implements = entity_type_dict[entity_type_name]["implements"]
    params_to_edit_names = []
    for i in implements:
        params_to_edit_names.append("Implements_"+i)

    print(params_to_edit_names)

    print("The following instances will be modified according to Entity Type: {}".format(entity_type_name))
    pp.pprint(family_instances)
        
    warnings = []
    t = Transaction(doc, "Populate BOS parameters")
    t.Start()
    for family_instance in family_instances:
        print("Editing family instance {}...".format(family_instance.Id.ToString()))
        # MODIFY ENTITY TYPE
        try:
            p_entity_type = family_instance.LookupParameter("Entity_Type")
            p_entity_type.Set(entity_type_name)
            print("Entity_Type parameter successfully edited for family instance {}.".format(family_instance.Id.ToString()))
        except:
            message = "Couldn't edit parameter Entity_Type for family instance {}.".format(family_instance.Id.ToString())
            warnings.append(message)
        # MODIFY YESNO PARAMETERS
        all_implements_params = []
        for p in family_instance.Parameters:
            if "Implements_" in p.Definition.Name:
                all_implements_params.append(p)
        for p in all_implements_params:
            try:
                if p.Definition.Name in params_to_edit_names:
                    p.Set(True)
                else:
                    p.Set(False)
                print("{} parameter successfully edited for family instance {}.".format(p.Definition.Name, family_instance.Id.ToString()))
            except:
                message = "Couldn't edit parameter {} for family instance {}.".format(p.Definition.Name, family_instance.Id.ToString())
                warnings.append(message)

    t.Commit()
        
    print("Script has ended")
    if warnings == []:
        print("Warnings: None")
    else:
        print("Warnings:")
        for w in warnings:
            print(w)
    