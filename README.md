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

## Contributing

Contributions are welcome! If you'd like to contribute to this project, please follow the standard procedures for pull requests and issues.

## License

This project is licensed under the [MIT License](LICENSE).
