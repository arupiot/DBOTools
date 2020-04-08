import yaml
import pprint
import sys
import xlsxwriter

yaml_source = "test/FCU.yaml"
family_types_excel = xlsxwriter.Workbook("test/FCU_family_types.xlsx")

pp = pprint.PrettyPrinter(indent=4)

def open_yaml():
# Open yaml with family types

    with open(yaml_source, "r") as stream:
        family_types_from_yaml = yaml.safe_load(stream)

    return family_types_from_yaml

def create_list_of_family_types(): # Parses the yaml file from Google and return a list of family types
    
    family_types_from_yaml = open_yaml()
    
    list_of_family_types = []

    for family_type in family_types_from_yaml.items():
        list_of_family_types.append(family_type[0])
    
    if len(list_of_family_types) == len(set(list_of_family_types)):
        print("No duplicates found in the list of family types")
    else:
        print("WARNING: Duplicates found in the list of family types")
    
    return list_of_family_types

def create_list_of_shared_parameters():

    # Structure:
    # Parameter Name - "formula" - "instance" - "group" - "reporting" - "type" - "default"
    family_types_from_yaml = open_yaml()

    list_of_shared_parameters = []

    device_type_id = ["Device_type_id", "", False, "Google", True, "Text", ""]
    device_type_description = ["Device_type_description", "", False, "Google", True, "Text", ""]
    device_type_is_canonical = ["Device_type_is_canonical", "", False, "Google", True, "YesNo", False]

    list_of_shared_parameters.append(device_type_id)
    list_of_shared_parameters.append(device_type_description)
    list_of_shared_parameters.append(device_type_is_canonical)

    # Add implements parameters

    list_of_implements_name = []

    for family_type in family_types_from_yaml.items():

        implements_name = family_type[1].get("implements")
        
        # If implements names are in a string, splits them and puts them in a list

        for item in implements_name:
            if "_" in item:
                new_implements_list = item.split("_")
                for new_item in new_implements_list:
                    list_of_implements_name.append(new_item)
            else:
                list_of_implements_name.append(item)
    
    #print(len(list_of_implements_name))

    list_of_implements_name = list(dict.fromkeys(list_of_implements_name))
    
    for implements_name in list_of_implements_name:
        new_implement_name = "Device_type_implements_{}".format(implements_name)
        new_implement_parameter = [new_implement_name, "", False, "Google", True, "YesNo", False]
        list_of_shared_parameters.append(new_implement_parameter)
    
    # Add uses parameters

    list_of_uses_name = []

    for family_type in family_types_from_yaml.items():

        uses_name = family_type[1].get("uses")
        
        if uses_name is not None:
            for item in uses_name:
                list_of_uses_name.append(item)

    list_of_uses_name = list(dict.fromkeys(list_of_uses_name))
    
    # print(list_of_uses_name)
    
    for uses_name in list_of_uses_name:
        new_uses_name = "Device_type_uses_{}".format(uses_name)
        new_uses_parameter = [new_uses_name, "", False, "Google", True, "YesNo", False]
        list_of_shared_parameters.append(new_uses_parameter)

    # print(list_of_shared_parameters)

    return list_of_shared_parameters

def create_list_of_parameter_names_and_values():
    family_types_from_yaml = open_yaml()
    
    # Structure:
    # Family_type - Parameter name - Parameter value

    list_of_parameter_names_and_values = []

    for family_type in family_types_from_yaml.items():
        family_type_name = family_type[0]

        #print(family_type[0])
        #print(family_type[1])

        #Create list of implements
        implements_raw = family_type[1].get("implements")
        list_of_implements_name = []
        #print(implements_raw)
        for item in implements_raw:
            if "_" in item:
                new_implements_list = item.split("_")
                for new_item in new_implements_list:
                    list_of_implements_name.append(new_item)
            else:
                list_of_implements_name.append(item)
    
        list_of_implements_name = list(dict.fromkeys(list_of_implements_name))
    
        for implements_name in list_of_implements_name:
         new_implement_name = "Device_type_implements_{}".format(implements_name)
         new_implement_value = True
         parameter_to_append = [family_type_name, new_implement_name, new_implement_value]
         list_of_parameter_names_and_values.append(parameter_to_append)

        #Create list of uses
        list_of_uses_name = []

        uses_name = family_type[1].get("uses")
        
        if uses_name is not None:
            for item in uses_name:
                list_of_uses_name.append(item)

        list_of_uses_name = list(dict.fromkeys(list_of_uses_name))
        
        for uses_name in list_of_uses_name:
            new_uses_name = "Device_type_uses_{}".format(uses_name)
            new_uses_parameter = [family_type_name, new_uses_name, True]
            list_of_parameter_names_and_values.append(new_uses_parameter)

        #Create description and id
        device_type_id = family_type[1].get("id")
        device_type_description = family_type[1].get("description")

        id_to_append =  [family_type_name, "Device_type_id", device_type_id]
        description_to_append = [family_type_name, "Device_type_description", device_type_description]

        list_of_parameter_names_and_values.append(id_to_append)
        list_of_parameter_names_and_values.append(description_to_append)

        #Create is_canonical

        device_type_is_canonical = family_type[1].get("is_canonical")
        if device_type_is_canonical == True:
            pass
        else:
            device_type_is_canonical = False
        is_canonical_to_append = [family_type_name, "Device_type_is_canonical", device_type_is_canonical]
        list_of_parameter_names_and_values.append(is_canonical_to_append)

    #print(list_of_parameter_names_and_values)
    #pp.pprint(list_of_parameter_names_and_values)
    return list_of_parameter_names_and_values



def print_data_to_excel():
    list_of_family_types = create_list_of_family_types()
    list_of_shared_parameters = create_list_of_shared_parameters()
    list_of_parameter_names_and_values = create_list_of_parameter_names_and_values()
    
    # Print list of family types:
    worksheet_family_types = family_types_excel.add_worksheet("Family_types")
    worksheet_family_types.write(0,0,"Family type name")
    row = 1
    for item in list_of_family_types:
        worksheet_family_types.write(row, 0, item)
        row += 1
    
    # Print list of shared parameters
    worksheet_shared_parameters = family_types_excel.add_worksheet("Shared_parameters")
    worksheet_shared_parameters.write(0,0,"Parameter Name")
    worksheet_shared_parameters.write(0,1,"formula")
    worksheet_shared_parameters.write(0,2,"instance")
    worksheet_shared_parameters.write(0,3,"group")
    worksheet_shared_parameters.write(0,4,"reporting")
    worksheet_shared_parameters.write(0,5,"type")
    worksheet_shared_parameters.write(0,6,"default")

    row = 1
    for item in list_of_shared_parameters:
        for col in range(0,7):
            worksheet_shared_parameters.write(row,col,item[col])
        row += 1

    # Print parameters names and values
    worksheet_paramters_names_and_values = family_types_excel.add_worksheet("Parameters_names_and_values")
    worksheet_paramters_names_and_values.write(0,0,"Family type")
    worksheet_paramters_names_and_values.write(0,1,"Parameter name")
    worksheet_paramters_names_and_values.write(0,2,"Parameter value")

    row =1

    for item in list_of_parameter_names_and_values:
        for col in range(0,3):
            worksheet_paramters_names_and_values.write(row, col, item[col])
        row += 1
    
    #Close and write the excel file
    family_types_excel.close()

if __name__ == "__main__":
    open_yaml()
    create_list_of_family_types()
    create_list_of_shared_parameters()
    create_list_of_parameter_names_and_values()
    print_data_to_excel()