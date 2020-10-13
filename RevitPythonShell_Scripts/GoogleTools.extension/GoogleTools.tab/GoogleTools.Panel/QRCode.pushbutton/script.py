import sys
import clr
import System
import pprint
import csv
import urllib

from System.Collections.Generic import *

clr.AddReference("RevitAPI")
from Autodesk.Revit.DB import *

clr.AddReference("RevitServices")
from RevitServices.Persistence import DocumentManager
from RevitServices.Transactions import TransactionManager

clr.AddReference("RevitAPIUI")
from Autodesk.Revit.UI import *
from Autodesk.Revit import Creation

from rpw.ui.forms import *

from Autodesk.Revit.UI.Selection import ObjectType

# Generate QR code using the Google Chart REST API:
# https://developers.google.com/chart/infographics/docs/qr_codes
root_url = 'https://chart.googleapis.com/chart?'
def generate_qr_code(message):
    query = dict(cht='qr', chs='320x320', chl=message)
    url = root_url + urllib.urlencode(query)
    url_open = urllib.urlopen(url)
    image = url_open.read()
    url_open.close()
    return image


def save_qr_code_image(image, file_path):
    file_name = '%s' % (file_path)
    with open (file_name, 'wb') as image_file:
        image_file.write(image)

doc = __revit__.ActiveUIDocument.Document
app = doc.Application
pp = pprint.PrettyPrinter(indent=4)
shared_param_file = app.OpenSharedParameterFile()

class CreateWindow(Form):
    def __init__(self): 
    
        # Create the Form
        self.Name = "Input Parameters Names"
        self.Text = "Input Parameters Names"
        self.Size = Size(500, 130)        
        self.CenterToScreen()

        self.values = []

        # Create Label for asset.name Name
        asset_name_input = Label(Text = "Parameter name for \"asset.name\"")
        asset_name_input.Parent = self
        asset_name_input.Location = Point(30, 20)
        asset_name_input.Width = 180
        
        # Create TextBox for asset.name Name
        self.textboxassetname = TextBox()
        self.textboxassetname.Parent = self
        self.textboxassetname.Text = "asset.name"
        self.textboxassetname.Location = Point(220, 20)
        self.textboxassetname.Width = 150

        # Create Button = button
        button = Button()
        button.Parent = self
        button.Text = "Ok"
        button.Location = Point(400, 50)
        # Register event
        button.Click += self.ButtonClicked
        
        # Create button event
        def ButtonClicked(self, sender, args):
            if sender.Click:
                self.values.append(self.textboxassetname.Text)
                self.values.append(self.textboxguidname.Text)
                self.Close()            

form = CreateWindow()

print("   ___   ___    ___          _         ___                             _             ")
print("  / _ \ | _ \  / __| ___  __| | ___   / __| ___  _ _   ___  _ _  __ _ | |_  ___  _ _ ")
print(" | (_) ||   / | (__ / _ \/ _` |/ -_) | (_ |/ -_)| ' \ / -_)| '_|/ _` ||  _|/ _ \| '_|")
print("  \__\_\|_|_\  \___|\___/\__,_|\___|  \___|\___||_||_|\___||_|  \__,_| \__|\___/|_|  ")

if doc.IsFamilyDocument:
    print("The document is a Family document. Please run the script in a Project document.")

else:
    selection = [doc.GetElement(element_Id) for element_Id in uidoc.Selection.GetElementIds()]

    ## Add option to scan the whole document
    if selection == []:
        print("Selection is empty.") 

        ## Select folder where to write the qr codes
    else:
        print("{} elements found in selection".format(len(selection)))
        print("Please select the output folder for the QR Codes...")
        output_folder = select_folder()

        if output_folder:
            print("Output folder: {}".format(output_folder))
            ## Print info about selection
            ## number of instances, kind of instances


            ## Run form to ask for parameter name
            Application.Run(form)
	
            form_output = form.values
            asset_name_param_name = form_output[0]

            ## Loop through items in selection, if they are family instances produce qr code otherwise skip
            for item in selection:
                print("Processing item {}...".format(item.Id.ToString()))
                if type(item).__name__ == "FamilyInstance":
                    asset_name_param = item.LookupParameter(asset_name_param_name)
                    asset_name = asset_name_param.AsString()
                    qr_code = generate_qr_code(asset_name)
                    save_qr_code_image(qr_code, "{}\\{}.png".format(output_folder, item.Id.ToString()))
                    print("QR Code printed")
                else:
                    print("Item skipped. Not a Family Instance.")

        else:
            print("Please select a folder")

print("Script has ended.")