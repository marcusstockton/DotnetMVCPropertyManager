Need to implement an ImageType object and update PropertyImages to use this instead of Document type.


Change address lookup stuff to add postcode, and search on that....would be better


https://developer.here.com/documentation/geocoding-search-api/dev_guide/topics-api/code-geocode-address.html

https://stackoverflow.com/questions/16534969/get-latitude-and-longitudefrom-city-name-using-html-js-in-bing-map
https://docs.microsoft.com/en-us/bingmaps/articles/create-a-custom-map-url
https://www.bing.com/maps?cp=50.7218017578125~-3.5336170196533205&lvl=15&style=r


Move the HttpClient creation into startup ala: services.AddHttpClient() - Update AddressController to use this.


Migrations:
Add-Migration [MIGRATION NAME] -OutputDir Data\Migrations