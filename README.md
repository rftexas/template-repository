# Template Repository

This is my extremely opinionated starter template for a Web API Solution. It uses Domain Design practices, Entity Framework Core, CQRS, [Vogen](https://github.com/SteveDunn/Vogen), and [Reqnroll](https://reqnroll.net/). 

## Using this template

Install this template by downloading this repository, navigating to the directory and typing `dotnet new install . --force`. That will add the template to your dotnet cli command. Now you can create a new solution by typing `dotnet new domainsolution --name Something`. That will generate this solution, replacing the Template text with Something.

## About this template

Many of the decisions that I have made here are to get users to the 90% mark quickly. The CQRS containers run much like they would if you were to use a popular closed source Mediator solution, just with a dedicated pipeline for Queries vs. Commands. Using Vogen Value Objects allows one to use type sensitive, compile time checks for data types, completely removing an entire class of bug. Reqnroll allows you, the developer, to define strong unit tests that use the language of the business. Theoretically, just about anyone should be able to read the feature file and find missing edge cases (my feature file has a few).
