##Open excel file, create a list of the parameters and add them to the family as shared parameters
import sys
import clr
import System

from System.Collections.Generic import *

clr.AddReference("RevitAPI")
from Autodesk.Revit.DB import *

clr.AddReference("RevitServices")
from RevitServices.Persistence import DocumentManager
from RevitServices.Transactions import TransactionManager

clr.AddReference("RevitAPIUI")
from Autodesk.Revit.UI import *
from Autodesk.Revit import Creation

clr.AddReference('Microsoft.Office.Interop.Excel, Version=11.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c')
from Microsoft.Office.Interop import Excel
from System import Array
from System.Runtime.InteropServices import Marshal

from rpw.ui.forms import *

import array
from array import *

app = doc.Application

excel_file_path = select_file("Excel File (*.xlsx)|*.xlsx", multiple = False)
if excel_file_path:
    pass
else:
    sys.exit("Please specify an excel file")

ex = Excel.ApplicationClass()

ex.Visible = False
ex.DisplayAlerts = False

workbook = ex.Workbooks.Open(excel_file_path)
sheet = workbook.Worksheets("Parameters")
## Structure is:
## Parameter Name | Group Name | Parameter Type | User Modifiable | Description

non_empty_range = sheet.UsedRange

header = sheet.Range("A1","E1")
# Get parameter names and cut the list where the empty cells start
param_name_range = sheet.Range("A:A")
param_names_array = param_name_range.Value2
param_names_list = []
for param_name in param_names_array:
    param_names_list.append(param_name)

for i in range(0,len(param_names_list)):
    if param_names_list[i] != None:
        counter = i

param_names = param_names_list[1:counter+1]

# Get the Group Names
group_name_range = sheet.Range("B:B")
group_name_array = group_name_range.Value2

group_names = []
for i in range(1, counter+1):
    group_names.append(group_name_array[i,0])

# Get the Built-in Group
builtin_group_range = sheet.Range("C:C")
builtin_group_array = builtin_group_range.Value2

builtin_groups = []
for i in range(1, counter+1):
    builtin_groups.append(builtin_group_array[i,0])

# Get the Parameter Types
parameter_types_range = sheet.Range("D:D")
parameter_types_array = parameter_types_range.Value2

# for parameter_type in parameter_types_array:
#     print(parameter_type)
#     print(type(param_type).__name__)

parameter_types_strings = []
for i in range(1, counter+1):
    parameter_types_strings.append(parameter_types_array[i,0])

for param_type_string in parameter_types_strings:
    print(param_type_string)

all_param_types = System.Enum.GetValues(ParameterType)


parameter_types = []
for param_type_string in parameter_types_strings:
    for param_type in all_param_types:
        if param_type_string == str(param_type):
            parameter_types.append(param_type)

for pt in parameter_types:
    print(pt)
    print(type(pt).__name__)


# Get the User Modifiable
user_modifiable_range = sheet.Range("E:E")
user_modifiable_array = user_modifiable_range.Value2

user_modifiable_s = []
for i in range(1, counter+1):
    user_modifiable_s.append(user_modifiable_array[i,0])

# Get the Description
descriptions_range = sheet.Range("F:F")
descriptions_array = descriptions_range.Value2

descriptions = []
for i in range(1, counter+1):
    descriptions.append(descriptions_array[i,0])

# Zip values together

shared_parameters = zip(param_names, group_names, builtin_groups, parameter_types, user_modifiable_s,descriptions)

ex.ActiveWorkbook.Close(False)
Marshal.ReleaseComObject(sheet)
Marshal.ReleaseComObject(workbook)
Marshal.ReleaseComObject(ex)


t = Transaction(doc, "Add Parameters to Shared Parameter File")

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
            print("Something went wrong. The group with name {} was not created".format(new_group_name))
            sys.exit("Script has ended")
    return newgroup

def create_definition (group, shared_parameter_file, param_name, param_type, usermodifiable, description):
    same_name_same_group = False
    same_name_different_group = False
    new_definition = None

    ext_def_creation_options = ExternalDefinitionCreationOptions(param_name, param_type)
    ext_def_creation_options.UserModifiable = usermodifiable
    ext_def_creation_options.Description = description
    new_definition = group.Definitions.Create(ext_def_creation_options)
    

    # try:
    #     ext_def_creation_options = ExternalDefinitionCreationOptions(param_name, builtin_group)
    #     ext_def_creation_options.UserModifiable = usermodifiable
    #     ext_def_creation_options.Description = description
    #     new_definition = group.Definitions.Create(ext_def_creation_options)
    #     print("Success")
    # except:
    #     # Check all the definitions in the same group
    #     list_definition_names = []
    #     for definition_same_group in group.Definitions:
    #         list_definition_names.append(definition_same_group.Name)
    #     if param_name in list_definition_names:
    #         print("A parameter already exists with name {} in the same group. Using existing parameter.".format(param_name))
    #         new_definition = definition_same_group
    #         same_name_same_group = True
        
    #     # Check all the definitions in all groups to look for the definition with the same name
    #     if same_name_same_group == False:
    #         for random_group in shared_parameter_file.Groups:
    #             if random_group.Name != group.Name:
    #                 list_definition_names = []
    #                 for definition in random_group.Definitions:
    #                     if definition.Name in list_definition_names:
    #                         print("A parameter already exists with name {} in group {}.".format(param_name, random_group.Name))
    #                         sys.exit("Interrupting script. Please check the name of your parameter.")
    #                     else:
    #                         same_name_different_group = True

        #if same_name_same_group == True and same_name_different_group == True:
        #    new_definition = None
        #    sys.exit("Something went wrong. Parameter {} not created. Script has ended.".format(param_name))

    print(new_definition)
    
t = Transaction(doc, "New instance Binding")
t.Start()

for parameter in shared_parameters:
    
    param_name = parameter[0]
    group_name = parameter[1]
    builtin_group = parameter[2]
    param_type = parameter[3]
    user_modifiable = parameter[4]
    description = parameter[5]
    
    new_group = createnewgroup(shared_param_file,group_name)
    
    if new_group:
        definition = create_definition(new_group, shared_param_file, param_name, param_type, user_modifiable, description)

t.Commit()
#     fam_params = doc.FamilyManager.Parameters
#     fam_params_names = []
#     for fam_param in fam_params:
#         fam_params_names.append(fam_param.Definition.Name)
#    
#     category_set = app.Create.NewCategorySet()
#     for cat in doc.Settings.Categories:
#         if cat.Name == "Lighting Fixtures":
#             category = cat
#     category_set.Insert(category)

#     if param_name in fam_params_names:
#         print("Parameter \"{}\" already exists in the family".format(param_name))
#     else:
#         try:
#             #AddParameter(ExternalDefinition, BuiltInParameterGroup, IsInstance)
#             doc.FamilyManager.AddParameter(new_definition, param_group, True)
#             print("Parameter \"{}\" added to the family".format(param_name))
#         except:
#             print("Something went wrong. Parameter \"{}\" not added to the family".format(param_name))