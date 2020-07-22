# create_revit_families
Create shared parameters in families from yaml files

## Installation

Find the path of your pyRevit libraries:
Run any script and check the first lines of the output. You should have something like:

```
2.7.7 (IronPython 2.7.7 (2.7.7.0) on .NET 4.0.30319.42000 (64-bit))

['C:\\Users\\username\\Documents\\GitHub\\create_revit_families\\RevitPythonShell_Scripts\\GoogleTools.extension\\GoogleTools.tab\\GoogleTools.Panel\\Excel2SharedParamFile.pushbutton', 'C:\\Users\\username\\AppData\\Roaming\\pyRevit-Master\\pyrevitlib', 'C:\\Users\\username\\AppData\\Roaming\\pyRevit-Master\\site-packages']
```
Copy the address of the pyrevitlib folder and use it as target when installing the requirements

Install python libraries requirements

```
pip install --target 'C:\\Users\\username\\AppData\\Roaming\\pyRevit-Master\\pyrevitlib' -r requirements.txt
```
## Tools for Revit automation using RevitPythonShell

### Installation for Revit Python Shell

* Install [RevitPythonShell](https://github.com/architecture-building-systems/revitpythonshell). RevitPythoShell uses IronPython to expose Revit methods to Python. 
* Install [Miniconda](https://docs.conda.io/en/latest/miniconda.html) for Windows version 2.7. If another version of Anaconda Python or Miniconda Python is installed, create a virtual environment using python version 2.7.7:
   `conda env create --name=py277 python=2.7.7` 
   and switch to it: 
   `activate py277` 
   from the anaconda shell or Windows command prompt (it doesn't work on Windows PowerShell).
* From the Windows command prompt of anaconda shell, intall the libraries requirements
    ```
    pip install -r requirements.txt
    ```
* Inside Revit, go to Add-ins -> Interactive Python Shell -> Configure -> Search Paths and add the following library paths:
    ```
    C:\Users\username\AppData\Local\conda\conda\envs\py277\Lib
    ```
    and
    ```
    C:\Users\username\AppData\Local\conda\conda\envs\py277\Lib\site-packages
    ```
    where `username` is your user name.
* Restart Revit
* Go to Add-ins -> Interactive Python Shell
* You should now be able to import the library requirements and execute the create_revit_families script

### Installation for pyRevit

<img src="https://images.squarespace-cdn.com/content/v1/5605a932e4b0055d57211846/1579016738840-S4HNYZPL5U05TTOGFZSR/ke17ZwdGBToddI8pDm48kGUB6bvAQyL_fjdXd3nTTDBZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZUJFbgE-7XRK3dMEBRBhUpy1qOPYMCUmUox1BUDmVYF_KfvPNJdunqX1yE5UASPGwIGTVuQfUTrbnSl6yicKsPc/image-asset.png?format=750w" data-canonical-src="https://images.squarespace-cdn.com/content/v1/5605a932e4b0055d57211846/1579016738840-S4HNYZPL5U05TTOGFZSR/ke17ZwdGBToddI8pDm48kGUB6bvAQyL_fjdXd3nTTDBZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZUJFbgE-7XRK3dMEBRBhUpy1qOPYMCUmUox1BUDmVYF_KfvPNJdunqX1yE5UASPGwIGTVuQfUTrbnSl6yicKsPc/image-asset.png?format=750w" width="200" height="130" />

* Install [pyRevit](https://github.com/eirannejad/pyRevit/releases). pyRevit uses IronPython to expose Revit methods to Python.

* Add the GoogleTools tab to the Revit ribbon:
    From Revit go to the pyRevit tab and click on pyRevit -> Settings
    In Custom Extension Directories, click Add folder and browse till the RevitPythonShell_Scripts folder
    Click on `Save Settings and Reload`

* Find the libraries path used by pyRevit:
    Click on the `?` button in the GoogleTools tab
    The output will be something similar to:
    ```
    2.7.7 (IronPython 2.7.7 (2.7.7.0) on .NET 4.0.30319.42000 (64-bit))
    ['C:\\Users\\username\\Documents\\GitHub\\create_revit_families\\RevitPythonShell_Scripts\\GoogleTools.extension\\GoogleTools.tab\\GoogleTools.Panel\\Excel2SharedParamFile.pushbutton', 'C:\\Users\\username\\AppData\\Roaming\\pyRevit-Master\\pyrevitlib', 'C:\\Users\\username\\AppData\\Roaming\\pyRevit-Master\\site-packages']
    ```
    where `username` is your user name.
    Take note of the `pyrevitlib` folder and use it as target when installing the requirements

* Install [Miniconda](https://docs.conda.io/en/latest/miniconda.html) for Windows version 2.7. If another version of Anaconda Python or Miniconda Python is installed, create a virtual environment using python version 2.7.7:
   `conda env create --name=py277 python=2.7.7` 
   and switch to it: 
   `activate py277` 
   from the anaconda shell or Windows command prompt (it doesn't work on Windows PowerShell).
* From the Windows command prompt of anaconda shell, intall the libraries requirements
    ```
    pip install --target 'C:\\Users\\username\\AppData\\Roaming\\pyRevit-Master\\pyrevitlib' -r requirements.txt
    ```