API

For manual deployment to azure add to the WebApp the following Environment variables:

In App settings
Name: TokenKey
Value: super secret key super secret key super secret key super secret key super secret key

In Connection strings
Name: DefaultConnection
Value: Server=tcp:dating-app-server-name.database.windows.net,1433;Initial Catalog=DatingApp;Persist Security Info=False;User ID=appuser;Password=Pa$$w0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
Type: SQLServer

Client

node version
nvm list -> 22.14.0

angular version
ng --version -> 19.2.6
