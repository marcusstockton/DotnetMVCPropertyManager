
Move the HttpClient creation into startup ala: services.AddHttpClient() - Update AddressController to use this.


Migrations:
Add-Migration [MIGRATION NAME] -OutputDir Data\Migrations


ToDo/Ideas:
Portfolio > Details to use a datatable or add in pagination/ordering to the existing page