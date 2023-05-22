# Introduction 
The purpose of this project is to get started with Nhibernate REALLY fast!
I made up a db to hold your Recipes - just to have some tables as an example. The content in the tables are in Danish - sorry, but I was lazy. :)

# Getting Started

1.	Create a database on your local MS SQL-server called RecipeDb
2.	Run the SQL script from the DbCreation folder
3.	Set the username & password by calling dotnet user-secrets init / dotnet user-secrets set "SQLUSERNAME" "your-user-name"

# ToDo's
Within the next few weeks I will implement Serilog, JWT - and maybe some other stuff that will come to my head.
The important thing is at the implementations is implementet as-good-as-it-gets and no non-useful stuff. Stripped for EF and other thinks that I do not like.

# Project by Brian Staal @ www.wisesoft.dk
I made this for my self - but you can use it as well. It's ok to comment if there are better solutions.