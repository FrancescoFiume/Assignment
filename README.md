Instructions:
NB: You need to have a valid dotnet sdk 8.0 and dotnet ef tool installed and working
Run in docker:
1) Clone project - `git clone git@github.com:FrancescoFiume/Assignment.git`
2) navigate to folder - `cd Assignment/Assignment
3) Start Docker containers - `docker-compose up --build -d`
4) Now we have to prepare the DB.
5) Check ip of the postgre container - `docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' assignment-db`
6) Modify appSettingFacSimile.Development.json to appSettings.Development.json
7) Change the localhost to the ip that you got from the docker inspect (should look like 172.18.0.2)
8) Now we have to update the db - `dotnet ef database update
9) go to `http://localhost:8080/swagger/index.html`


Run from system:
NB: To run from system, you need to have already set up and working postgre
1) Clone project - `git clone git@github.com:FrancescoFiume/Assignment.git`
2) `cd Assignmen/Assignment`
3) rename appSettingFacSimile.Development.json to appSettings.Development.json
4) change the connection string with your postgre host, port datbase name, user and password
5) dotnet ef database update
6) dotnet run
