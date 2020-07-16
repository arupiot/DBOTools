# EXCEL TO SHARED PARAMETER FILE
import sys
print(sys.version)
print(sys.path)
import clr
import System

from System.Collections.Generic import *

clr.AddReference("RevitAPI")
from Autodesk.Revit.DB import *

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

doc = __revit__.ActiveUIDocument.Document
app = doc.Application
shared_param_file = app.OpenSharedParameterFile()

if shared_param_file == None:
    print("No Shared Parameter File found. Please open or create a Shared Parameter File.")
else:
    # Select the excel file
    excel_file_path = select_file("Excel File (*.xlsx)|*.xlsx", multiple = False)
    if excel_file_path:
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

        all_param_types = System.Enum.GetValues(ParameterType)

        parameter_types = []
        for param_type_string in parameter_types_strings:
            for param_type in all_param_types:
                if param_type_string == str(param_type):
                    parameter_types.append(param_type)

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

        for parameter in shared_parameters:
            
            param_name = parameter[0]
            group_name = parameter[1]
            builtin_group = parameter[2]
            param_type = parameter[3]
            user_modifiable = parameter[4]
            description = parameter[5]

            if description == None:
                description = ""

            if param_name and group_name and builtin_group and param_type and user_modifiable and description:
                new_group = createnewgroup(shared_param_file,group_name)
                if new_group:
                    definition = create_definition(new_group, shared_param_file, param_name, param_type, user_modifiable, description)
                else:
                    print("Please check the parameter template has all information populated")
    else:
        print("Please specify an excel file")