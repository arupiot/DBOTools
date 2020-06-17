# create_revit_families
Create Revit families from yaml files

This is in 2 steps:

* execution of python script
* execution of [dynamo](https://dynamobim.org/download/) script

## Installation

Install python libraries requirements

```
pip install -r requirements.txt
```

Install dynamo libraries requirements
* `archi-lab.net` 2019.2.27
* `orchid` 0.0.2

## Usage

```
python yaml2excel.py
```

## Tools for Revit automation using RevitPythonShell

### Installation

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
