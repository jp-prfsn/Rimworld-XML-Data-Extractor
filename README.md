### Data Exporter for Rimworld Mod files

Small console tool to navigate folders and get XML data from all your Rimworld mods and export it as delimited .txt file.

Useful if you have a lot of mods and want to get them into a spreadsheet.

Settings for 
- import/export locations
- edit delimiter
- exclude folders / files by name

#### USAGE

##### 1 RUN

Run the main function of the tool. You should supply:
- folder path for the files: Top level folder containing all your RIMWORLD files. The search will run through all subfolders, so if in doubt, go up to the hightest level.
- export path for output
- tag name: the program will look for instances of the tag, there are often multiple examples in one file. The child tags are defined in the next step.
- property names: child tags of the above, things like label, cost, description.

##### 2 SETTINGS

Under settings you can change:
- the delimiter of the output (default is ;)
- add/remove terms to exclude from search (the program won't check a file if its filepath contains one of these terms)