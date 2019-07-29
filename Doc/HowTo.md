# Getting Started 
## How to use

First lets go through set of important files.

### Important files
 

### 1. CleanerConfig.json
    This defines how to perform cleaning operations on data
```javascript
{

  "[inputfilename]-header": {
    "[columnkey]": {
      "Key": "[operation]",
      "Value": "[inputvalue]"
    },
  "[input-filename]-row": {
    "[column]": {
      "Key": "[operation]",
      "Value": "[inputvalue]"
    }
  }
}
// sample
{

  "code-list.csv-header": {
    "Country": {
      "Key": "rename",
      "Value": "Country_code"
    }
  },
  "code-list.csv-row": {
    "Function": {
      "Key": "filter-for-symbol",
      "Value": "([0-8][B][-])\\w+"
    }
  }
}
```


### 2. MappingRules.json
    defines specific rules to be observe while mapping between source and destination
```javascript
sub rule to be applied for each cell when mapping happens between 
source & destination

{
  "[source]:[columnkey]=>[destination]:[cellkey]": [ 
    "[operation-steps]"
  ]

}
//sample
{
  "Source:FunctionCode=>function_list.txt:Location_Type": [ 
    "split-by-letters"
  ]
/*
description : for value (FunctionCode )in source file  apply operation (split-by-letters) and proceed with mappers to destination is [Location_Type]
*/ 
}
```
### 3. RuntimeConfig.json
    runtime parameters & settings
```json
    {
    "AutoBuild": true, /*changes behaviour to auto build & clean*/
    "LookUpFilePattern": "*.txt", /*look up file pattern file */
    "SourceDataFolder": "E:\\ET_Tool\\Data\\geo_unlocode\\", /*directory path where scanning has to be done*/
    "DataSourceFileName": "E:\\ET_Tool\\Data\\geo_unlocode\\code-list.csv", /*input file path*/
    "DataSinkFileName": "out.csv", /*out put file path*/
    "OutConfigFileName": "outConfig.json", /*template for output */
    "DegreeToDecimalLatLongMapperSettings": { /*store specific information for a data mapper */
        "ColumnKey": "Coordinates", /*source column*/
        "LatitudeKey": "Latitude", /* output column name*/
        "LongitudeKey": "Longitude" /* output column name*/
    },
    "DefaultCleanerConfig": "cleanerConfig.json", /* configuration file for cleaner*/
    "MappingRulesSourcePath": "mappingRules.json" /* mapping rules file to describe about input to output mapping rules*/
}
```
### 4. OutConfig.json
    template defining how the data should look like
```javascript
{
  "Headers": "[headers in csv format & in order]",
  "HeaderDelimiterSeparator": "|",
  "RowDelimiterSeparator": "\t",
  "AutoMapping": true
}
//sample
{
  "Headers": "Country_code,Country_name,Location_code,Location_Name,Location_Type,Longitude,Latitude",
  "HeaderDelimiterSeparator": "|",
  "RowDelimiterSeparator": "\t",
  "AutoMapping": true
}
```

## Starting the program

console program should started by double clicking or via command prompt after configurations files are placed properly 

Note: paths need to be changed to reflect your filesystem
