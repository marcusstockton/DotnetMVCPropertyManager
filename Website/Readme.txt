
Move the HttpClient creation into startup ala: services.AddHttpClient() - Update AddressController to use this.


Migrations:
Add-Migration [MIGRATION NAME] -OutputDir Data\Migrations


ToDo/Ideas:
Portfolio > Details to use a datatable or add in pagination/ordering to the existing page
Property Tenants > make a table instead of the card desk
Update from bootstrap 4 > 5




### Docker Commands

``docker build -t propertymanager .``\
``docker run -it -p 5000:5000 propertymanager``\
``docker exec -it <container name> bash`` # to load up the docker image to navigate in linux\
``docker compose build`` # builds all images\
``docker compose build postgresdb`` # Builds specific image\
``docker compose up -d`` # runs all images in detached mode\
``docker compose up -d --build``
``docker compose up -d postgresdb`` # runs one particular image in detached mode