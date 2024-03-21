# Superhero Quoter
A simple application that displays a random superhero quote every 10 seconds.

## Setup

### Creating the database
Run the following command in the Package Manager Console to create a local database based on the existing migrations:
```sh
Update-Database
```

If you encounter any issues with the database creation or migrations, you can try running the following commands:
```sh
Add-Migration InitialCreate
Update-Database
```

### Data
Copy and paste the contents of [quotes.json](quotes.json) into the Swagger POST `/api/Quotes/Multiple` API endpoint.

### Run the project
Run the server and client projects.