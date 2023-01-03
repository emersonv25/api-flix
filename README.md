# MyFlixApi
series/anime streaming web api.
Organized into Categories, Series, Season and Episodes
Storing only the images, the videos we store the url hosted elsewhere.
It has an authentication system, but so far it has no practical use, in the future I will use the login for comments and favorites

# Technologies Used
* Asp .Net Core 6 + Entity Framework Core 6
* Microsoft SQL Serve
* Visual Studios 2022

# Starting
1. Configure database ConnectionStrings in **appsettings.js**
3. Create the database migration `dotnet ef migrations add InitialCreate`
4. `dotnet ef database update` to create the database
5. run

----
Front-end: https://github.com/emersonv25/myflix-app-nextjs
