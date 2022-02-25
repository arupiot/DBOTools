# DBO Tools
Generate [DBO ontologies](https://github.com/google/digitalbuildings) from a Revit model.
## Installation

1. Copy all files in folder:

    ```
    DBOTools\Source\DBO Tools\ADDIN FILES
    ```
    and paste them into the addin folder of your version of Revit:

    ```
    C:\Users\username\AppData\Roaming\Autodesk\Revit\Addins\2020
    ```
    _(The DBO Tools addin is currently available only for Revit 2020)_
2. Restart Revit and authorise the new addin. The DBO Tools will appear as a ribbon integrated into the standard Revit layout.

## Intended Workflow
To generate the desired ontology from the Revit model, the intended steps are:
1. Priming the model with the creation of shared parameters:
    - Loading of the DBO Shared Parameter File provided in this same repository
    - Batch editing of the families of interest to inject the DBO Parameters
    - _TBC_ Project specific design of Revit Spaces and Zones
    - _TBC_ Device naming workflow (suggested open standard [BDNS](https://github.com/theodi/BDNS))
2. Population of DBO parameters
    - Script assisted population of the DBO Type Parameter 'DBO Export As'
    - Script assisted generation of the DBO Entity Types and their assignment to the Revit Familiy Types
3. Export of relevant entities<br>
_Currently WIP, functionalities not available_
    - One click export into several formats:
        - yaml
        - rdf
        - gsheet
        - csv
    - Requires access to internet or to the [Digital Buildings Project](https://github.com/google/digitalbuildings) repository downloaded for offline use.
## Tools and Usage
The DBO Tools present themselves as a new ribbon in the Revit interface called "DBO Tools".<br>
The ribbon is divided in panels where the User can find the following buttons:
### Inject Parameters
Displays a window containing all families in model. The user can select via checkboxes which families should receive the DBO parameters. The window will also contain information on whether the families already have the DBO parameters.
Pressing Inject opens each selected family, adds the parameters them and loads them back into the model.
Pressing Remove opens each family to remove the DBO parameters, if the user wishes to clean up the model and exclude certain entities from the ontology.
Pressing Cancel closes the window.
A pop up message will appear at the end of the Injection/Removal process to report on the execution of the command.
### DBO Export As
Displays a window containing all families in the model. The User can edit the ones that have DBO parameters injected and populate the "DBO Export As" parameter to set the DBO General Type (e.g. Air Handling Units, Fan Coil Units, Pumps...) of that family type and of all relative instances. The families without DBO parameters will be greyed out and not editable.
Please note that the "DBO Export As" parameter is a type parameter but the User sees the process happening at a family level because all types of the same family are supposed to receive the same parameter value.
The selection of the DBO General Type with which the family types are populated happens via a combobox that lists the available Entity Types via their abbreviations (e.g "AHU","FCU","PMP"...) to prevent the invalid ones from being entered.
Pressing OK starts the population of the parameter. A pop up message will appear at the end of the process to report on the execution of the command.
Pressing Cancel closes the window.
_Is is recommended that the User only edits this and the other parameters via the tools, to avoid invalid values, inconsistencies, typos and other errors._
### Edit DBO Types
Displays a window containing all families in the model with DBO Parameters. When the User selects a family, a menu will expand on the side to show all family types belonging to said family. For each family type, the two type parameters "DBO Uses" and "DBO Implements" can be populated to build the DBO Entity Type.
_The window and its functionalities are work in progress__
### DBO Validation
Button not active
_Validation processes are to be discussed_
### Export CSV
Button not active
_Currently work in progress, will export all types and instances with relative properties into a csv spreadsheet in a format TBC_
### Export YAML
Button not active
_Currently work in progress, will export all types and instances with relative properties into a collection of yaml files. The format for both Entity Types and Instances is TBC._
### Export RDF
Button not active
_Not yet developed, will use the dotNetRDF library to export all relevant elements in Revit into an RDF ontology._
### Info
Displays a pop up message with info regarding the DBO Tools, ownership, credits, licence and useful links.
_The content of the pop up message is not populated_
## Licence
_TBC_
## Contacts
_TBC_
## References and Links
_TBC_