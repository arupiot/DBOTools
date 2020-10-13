#EXCEL TO YAML

import sys
#print(sys.version)
#print(sys.path)
import clr
import System
import yaml
import pprint

clr.AddReference('Microsoft.Office.Interop.Excel, Version=11.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c')
from Microsoft.Office.Interop import Excel
from System import Array
from System.Runtime.InteropServices import Marshal

from rpw.ui.forms import *

import array
from array import *

doc = __revit__.ActiveUIDocument.Document
app = doc.Application
pp = pprint.PrettyPrinter(indent=4)
shared_param_file = app.OpenSharedParameterFile()

excel_file_path = select_file("Excel File (*.xlsx)|*.xlsx", title = "Please select the \"Data Schedule and Responsibility Matrix\" Excel spreadsheet", multiple = False)

if excel_file_path:
    ex = Excel.ApplicationClass()

    ex.Visible = False
    ex.DisplayAlerts = False

    workbook = ex.Workbooks.Open(excel_file_path)
    print("Opened spreadsheet \"{}\"".format(workbook.Name))

    worksheets = []
    for s in workbook.worksheets:
        if s.Name not in ["Instructions","OmniClass","Project Information"]:
            worksheets.append(s)
    
    print("Groups detected in the spreadsheet:")
    for s in worksheets:
        print(s.Name)

    parameters_dictionary = {}

    for s in worksheets:
        sheet_name = s.Name
        print("Processing group {}...".format(sheet_name))
        ####
        name_range = None
        name_range = s.Range("C:C")
        name_array = name_range.Value2
        name_list = []

        for i in name_array:
            name_list.append(i)
        name_list = name_list[2:]
        for i in range(len(name_list)):
            if name_list[i] != None:
                counter = i+1
        name_list = name_list[:counter]
        ####
        prefix_range = None
        prefix_range = s.Range("B:B")
        prefix_array = prefix_range.Value2
        prefix_list = []
        
        for i in prefix_array:
            prefix_list.append(i)
        prefix_list = prefix_list[2:]
        prefix_list = prefix_list[:counter]
        ####
        type_range = None
        type_range = s.Range("D:D")
        type_array = type_range.Value2
        type_list = []

        for i in type_array:
            type_list.append(i)
        type_list = type_list[2:]
        type_list = type_list[:counter]
        ####
        instance_range = None
        instance_range = s.Range("F:F")
        instance_array = instance_range.Value2
        instance_list = []

        for i in instance_array:
            instance_list.append(i)
        instance_list = instance_list[2:]
        instance_list = instance_list[:counter]

        
        param_list = zip(prefix_list,name_list,type_list,instance_list)
        #pp.pprint(new_list)
        for param in param_list:
            if param[0] != None:
                
                param_name = param[0]+param[1]

                if param[2] == "TEXT":
                    param_type = "Text"
                elif param[2] == "YESNO":
                    param_type = "YesNo"
                elif param[2] == "NUMBER":
                    param_type = "Number"
                elif param[2] == "LENGTH":
                    param_type = "Length"
                else:
                    print("Parameter Type not recognised: Group:{}, Type:{}.".format(s.Name,param[2]))
                
                if param[3] == "Instance":
                    is_instance = True
                elif param[3] == "Type":
                    is_instance = False
                else:
                    print("Is Instance not recognised: Group:{}, Is Instance:{}.".format(s.Name,param[3]))

                parameters_dictionary[param_name] = {
                    "GroupName" : sheet_name,
                    "BuiltinGroup" : "PG_IDENTITY_DATA",
                    "ParameterType" : param_type,
                    "IsInstance" : is_instance,
                    "UserModifiable" : True,
                    "Description" : ""
                }
        
        prefix = list(dict.fromkeys(prefix_list))
        prefix.remove(None)
        if len(prefix) != 1:
            print("More than one prefix detected in group {}. Prefixes: {}".format(sheet_name, prefix_list))
            for prefix in prefix_list:
                print([type(prefix).__name__, prefix])
        else:
            prefix = prefix[0]
        
        Marshal.ReleaseComObject(s)
        print("Finished processing group {}.".format(sheet_name))

    ex.ActiveWorkbook.Close(False)
    
    Marshal.ReleaseComObject(workbook)
    Marshal.ReleaseComObject(ex)

    #pp.pprint(parameters_dictionary)

    output_filepath = excel_file_path.strip(".xlsx") + ".yaml"

    with open(output_filepath, "w") as output:
        yaml.safe_dump(parameters_dictionary, output, encoding='utf-8', default_flow_style = False)
    
    print("-------")
    print("Yaml file exported at {}".format(output_filepath))

else:
    print("Please specify an Excel file")