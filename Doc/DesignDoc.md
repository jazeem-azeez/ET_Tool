# Design of how this et_tool works
## Main
Critera kept on watch : 
1. Should be able to support large csv.
2. Should be able to support difference  sources of data
### `Key Players`
1. cleanerConfig.json - defines how to clean data
2. mappingRules.json - defines specific rules to be observe while mapping between source and destination
3. runtimeConfig.Json - runtime paramters & settings
4. outConfig.json - template definging how the data should look like
