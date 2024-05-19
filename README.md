# Fund Raising Web Server
This is our End semester project of second year for Databse Systems.
This project is a Fund Raising web server built using .NET 8.0 SDK and Microsoft SQL Server. It follows a DB First Approach and includes authentication (using JWT tokens) with plans for role-based authorization.

## Project Structure

The project follows a clean architecture approach with the following structure:

- **Controllers**: Contains all the controllers.
- **Models**: Contains the DB Context and DB tables.
  - **DTOs**: Contains Data Transfer Objects (Improving on their readability).
- **Repositories**: Contains interfaces of the services.
- **Services**: Involves the services used, such as CaseService for the case table.
- **Configurations**: Holds JWT-Config class.
- **Utilities**: Includes utilities.
  - **PasswordHashing**: Contains Argon2Hasher class and IArgon2Hasher interface for password hashing, and RandomSaltGenerator class to generate a random salt.

## Features

- **Authentication**: Secure authentication using JWT.
- **Role-based Authorization**: Currently in progress.
- **API Endpoints**: For managing cases and causes and users.
- **Password Hashing**: Uses Argon2 for secure password hashing with random salt generation.

## Technologies Used

- **.NET 8.0.2 SDK**
- **MS SQL Server**
- **Entity Framework (Database First Approach)**
- **JWT for authentication**
- **Argon2 for password hashing**

## Work in Progress

- The web API is currently under development.
- Efforts are being made to enhance readability and generate additional endpoints as per frontend requirements.

## Getting Started

To get started with this project, follow these steps:

1. Clone the repository.
  To check the project:
   ```bash
   git clone https://github.com/Muhammad-Shah-zaib/FundRaisingServer
   ```
2. Ensure you have .NET 8.0 SDK installed.
3. Set up your Microsoft SQL Server instance.
  Then run the following commands:
    ```bash
    cd <folder_name>
    dotnet restore
    dotnet ef dbcontext scaffold "<ConnectionString>" <Provider> <Options>
    dotnet run
    ```

## Contributors
- [Muhammad Shahzaib](https://github.com/Muhammad-Shah-zaib)
- [Azka Ahmad](https://github.com/AzkaAhmad754)
- [Hassam Ali](https://github.com/Hassam-01)

