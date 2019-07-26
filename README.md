Sample ET_tool

from a data set containing 

Change,Country,Location,Name,NameWoDiacritics,Subdivision,Status,Function,Date,IATA,Coordinates,Remarks

and mappings 

CountryCode,CountryName
FunctionCode,FunctionDescription

where Country_Code == Country, country_name == CountryName == Country, Location_Code==Location,Location_Name=Name,Location_Type=FunctionDescription==Function, Longitude ==Extract(Coordinate), Longitude ==Extract(Coordinate)

produce in follwing format

Country_code | country_name | Location_code | Location_Name | Location_Type | Longitude | Latitude
AU Australia ALO Adelong Road Terminal -35.3 148.06666666666666
AU Australia ANB Agnes Bank Road Terminal -33.6 150.7