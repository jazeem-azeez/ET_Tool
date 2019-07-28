# Sample ET_tool : Assignment

## Assignment task is as follows
 
from a data set containing.

Change,Country,Location,Name,NameWoDiacritics,Subdivision,Status,Function,Date,IATA,Coordinates,Remarks

and mappings for

CountryCode,CountryName

FunctionCode,FunctionDescription

#### where Country_Code == Country, country_name == CountryName == Country, Location_Code==Location,Location_Name=Name,Location_Type=FunctionDescription==Function, Longitude ==Extract(Coordinate), Longitude ==Extract(Coordinate)

produce in follwing format

#### Country_code | country_name | Location_code | Location_Name | Location_Type | Longitude | Latitude

#### AU Australia ALO Adelong Road Terminal -35.3 148.06666666666666
#### AU Australia ANB Agnes Bank Road Terminal -33.6 150.7

## Getting started with ET_Tool

~ Note: I have opted for developing a microservice

### System Requriements: 

1. Windows OS
2. .NetCore 2.1


Short video on how to use <[here](.\Doc\media\intro-vid.avi) >


<[Design Documentation](.\Doc\DesignDoc.md) >


## Approaches Dsicussion
1. SQl 
   1. Export Csv & Lookup information to Sql database 
      1. Perform Iterative Cleaning to Csv Data with basic tools
   2. Write queries to Create Projections 
   3. Export projections to TSV or expected output format
   4. Advantage 
      1. easy to work with data (for small data)
      2. easy to understand & general tool knowledge 
      3. time to achieve solution is minimum
   5. Downside 
      1. Not Modular - tight coupling caused by scripts
      2. Not a scalable, data size limits 
      3. Dependency on Sql 
      4. will Manual Intervention is needed to progress through steps
      5. Cannot be Packaged & deployed to environments easly
2. Use Python tools Like CSV-KIt & sql Alchemy
   1. Omitted due to personal preference & scarcity of available machine for installing tools.
3. Developing a Tool / Micro Service 
   1. Build a software tool to reading the incoming csv
   2. Take Runtime configuration
   3. Auto validate entries
      1. If faulty, Auto apply fixes - fixing data by inference
   4. Perform Data Cleansing
   5. Create Projections as mentioned in output confgi
      1. By using auto dependency discovery & preconfigurations
      2. Perform Final Checksum of data
   6. Advantages
      1. Extensibility - Can be made to Source and Sink from any to any 
      2. Modularity& Resusablilty - Source to Sink transformations can be reused for other datasets * services 
      3. Easly can be made as General service
      4. Performance & efficiency of resources
      5. No dependency on databases
   7. Disadvantages
      1. Huge Effort & time for building framework
      2. maintenance of tool & relate Knowledge about tool
      3. learning curve of new tool
