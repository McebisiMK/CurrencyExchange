[![Continuous Integration and deployment](https://github.com/McebisiMK/CurrencyExchange/actions/workflows/dotnet.yml/badge.svg)](https://github.com/McebisiMK/CurrencyExchange/actions/workflows/dotnet.yml)
# CurrencyExchange
Interacts with public currency exchange API and Save the Data to **MySQL** and **Redis Cache**.


PROJECT SETUP
-
- Download and Install MySQL using MySQL Installer
    ```
    https://dev.mysql.com/downloads/installer/
    ```
- Setup Redis Server using the following Docker commands
    ```
    // Docker container called redis-cache mapped to host machine port 90 running in detached mode.
    Command: docker run --name redis-cache -p 90:6379 -d redis

    // Launch the shell
    command: docker exec -it redis-cache sh
    ```
- Create the **Database** using **Migrations** (Open the solutions in Visual Studio). [Entity Framework Core Migrations](https://www.learnentityframeworkcore.com/migrations)
    ```
    // On Package Manager Console (Select Default Project as CurrencyExchange.Domain)
    Command: update-database

    // On Command Line
    Command: dotnet ef database update
    ```
- Create **API Key** to integrate with [FastForex (Exchange Rate API)](https://www.fastforex.io/). 
- Update **appsettings.json**:
    - Update **Api Key**
        ```
        "CurrencyExchangeAPIOptions": {
            "ApiKey": "...",
            "BaseUrl": "https://api.fastforex.io"
        }
        ```
    - **MySQL** and **Redis** Connection string
        ```
        "ConnectionStrings": {
            "CurrencyExchangeDatabase": "...",
            "RedisConnectionString": "localhost:90"
        }
        ```
USEFUL COMMANDS
-
- Restore solution
    ```
    dotnet restore
    ```
- Build solution
    ```
    dotnet build
    ```
- Run all tests
    ```
    dotnet test
    ```