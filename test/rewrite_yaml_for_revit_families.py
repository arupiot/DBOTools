import yaml
import io
import argparse
import pprint
import sys

yaml_file = "FCU.yaml"
yaml_template = "FCU_Template.yaml"
#yaml_file = "G:\Shared drives\KGX1 BIM Working Group Drive\Google Linked Data Model\Ontology\yaml\HVAC\entity_types\AHU.yaml"


f_destination = "FCU_revit_family.yaml"

shared_parameter_input = "KXC-A-001-Z-BTN-XX-TM001_Google_Master_Shared_Parameters.txt"
shared_parameter_output = "Shared_parameter_output.txt"

pp = pprint.PrettyPrinter(indent=4)

def write_types():

    with open(yaml_template, "r") as stream_template:
        data_loaded_template = yaml.safe_load(stream_template)
        family_type_template = data_loaded_template.get("types").get("Generic")
        #print(type(family_type_template).__name__)
        #print(family_type_template)

    with open(yaml_file, "r") as stream:
        data_loaded = yaml.safe_load(stream)
    
    implements_all = []
    uses_all = []

    for family_type in data_loaded.items():
        family_type_name = family_type[0]
        all_type_parameters = family_type[1]
        #print(type(family_type_name).__name__)
        #print(type(all_parameters).__name__)
        model_id = all_type_parameters.get("id", "")
        implements = all_type_parameters.get("implements", None)
        uses = all_type_parameters.get("uses", None)
        description = all_type_parameters.get("description", "")
        
        #build list of all "implements" values without duplicates
        implements_all.extend(implements)
        implements_all = list(set(implements_all))

        #build list of all "uses" values without duplicates
        
        try:
            uses_all.extend(uses)
            uses_all = list(set(uses_all))
        except(TypeError):
            pass

           
    implements_dict_all = {}
    uses_dict_all = {}

    #Updates the dictionary with the new "implements" parameters, setting them all to False
    for implements_value in implements_all:
        implements_dict = {
            "Implements_{}".format(implements_value) : False
        }
        implements_dict_all.update(implements_dict)
    
    #Updates the dictionary with the new "uses" parameters
    for uses_value in uses_all:
        uses_dict = {
            "Uses_{}".format(uses_value) : False
        }
        uses_dict_all.update(uses_dict)
    
    types_dict = {
        "types": {}
    }

    family_type_set_values = {}
     
    
    for family_type in data_loaded.items():
        family_type_name = family_type[0]
        all_type_parameters = family_type[1]
        model_id = all_type_parameters.get("id")
        implements = all_type_parameters.get("implements")
        uses = all_type_parameters.get("uses")
        description = all_type_parameters.get("description")
        
        family_type_set_values.clear

        family_type_set_values = {
            "Model_id" : model_id,
            "Description" : description
        }

        family_type_final_values = {}
                
        family_type_final_values.update(family_type_template)
        family_type_final_values.update(uses_dict_all)
        family_type_final_values.update(implements_dict_all)
        family_type_final_values.update(family_type_set_values)

        for implement_true in implements:
            implements_true = {
                "Implements_{}".format(implement_true) : True
            }
            family_type_final_values.update(implements_true)

        try:
        
            for use_true in uses:
                uses_true = {
                    "Uses_{}".format(use_true) : True
                }
                family_type_final_values.update(uses_true)
        except(TypeError):
            pass

        for family_type_final_values_key, family_type_final_values_value in family_type_final_values.items():
            if type(family_type_final_values_key) == str:
                family_type_final_values_key = family_type_final_values_key.replace(" ", "_")
            if type(family_type_final_values_value) == str:
                family_type_final_values_value = family_type_final_values_value.replace(" ", "_")
                
        family_type_final_dict = {
            family_type_name : {}
         }

        family_type_final_dict[family_type_name] = family_type_final_values

        types_dict["types"].update(family_type_final_dict)

        family_type_final_values.clear
        family_type_final_dict.clear

    
    
    return data_loaded_template, implements_dict_all, uses_dict_all, types_dict
    
def write_parameters():
    
    data_loaded_template, implements_dict_all, uses_dict_all, types_dict = write_types()
    
    parameters_dict = {
        "parameters" : {}
    }
    
    all_parameters = data_loaded_template.get("parameters")
    for parameter in data_loaded_template.get("parameters").items():
         #print(type(parameter).__name__) #returns a tuple
         #print(parameter)
         par_name = parameter[0]
         par_content = parameter[1]
         #print(type(par_content).__name__)
         existing_parameter = {
            par_name : par_content
         }
         parameters_dict["parameters"].update(existing_parameter)

    print(type(parameters_dict["parameters"]).__name__)

    for parameter_key, parameter_value in parameters_dict["parameters"].items():
        parameter_value["formula"] = ""
        #if parameter_value["formula"] == None:
        #    parameter_value["formula"] = ""
        
        #else:
        #    print(type(parameter_value["formula"]).__name__)
        #    print(parameter_value["formula"])

    for implement_parameter_key, implement_parameter_value in implements_dict_all.items():
        new_implement_parameter = {
            implement_parameter_key : {
                "formula" : "",
                "instance": True,
                "group": "INVALID",
                "reporting": False,
                "type": "YesNo",
                "default": False
            }
        }
        parameters_dict["parameters"].update(new_implement_parameter)


    
    for use_parameter_key, use_parameter_value in uses_dict_all.items():
        new_use_parameter = {
            use_parameter_key : {
                "formula" : "",
                "instance": True,
                "group": "INVALID",
                "reporting": False,
                "type": "YesNo",
                "default": False
            }
        }
        parameters_dict["parameters"].update(new_use_parameter)

    final_dictionary = {}
    final_dictionary.update(types_dict)
    final_dictionary.update(parameters_dict)
    
    return final_dictionary

def write_yaml():

    final_dictionary = write_parameters()    


    with io.open(f_destination, "w", encoding="utf8") as outfile:
        noalias_dumper = yaml.dumper.SafeDumper
        noalias_dumper.ignore_aliases = lambda self, final_dictionary: True
        yaml.dump(final_dictionary, outfile, default_flow_style = False, allow_unicode = True, Dumper=noalias_dumper)

def write_shared_parameters():

    string_shared_parameters = ""
    final_dictionary = write_parameters()
    for param_name, param_value in final_dictionary["parameters"].items():
        if "Implements" in param_name or "Uses" in param_name:
            new_line = "{}, Other, YesNo, INVALID, true, Air Terminals".format(param_name)
            if string_shared_parameters == "":
                string_shared_parameters = string_shared_parameters + new_line
            else:
                string_shared_parameters = string_shared_parameters + "\n" + new_line
        
    text_file_shared_parameters = open(shared_parameter_output, "w")
    text_file_shared_parameters.write(string_shared_parameters)
   
if __name__ == "__main__":
    write_types()
    write_parameters()
    write_yaml()
    write_shared_parameters()