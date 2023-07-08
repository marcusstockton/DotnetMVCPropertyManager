
Move the HttpClient creation into startup ala: services.AddHttpClient() - Update AddressController to use this.


Migrations:
Add-Migration [MIGRATION NAME] -OutputDir Data\Migrations