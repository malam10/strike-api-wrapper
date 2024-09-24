# Strike API Wrapper

A .NET Core API wrapper for interacting with Strike's API, allowing you to create invoices, retrieve account profiles, generate quotes, and manage invoices. This project is built with ASP.NET Core, and utilizes HttpClient and Newtonsoft.Json for API interactions.

## Features

- Create invoices with a predefined currency and amount.
- Retrieve account profiles by handle.
- Get specific invoice details by ID.
- Generate quotes for unpaid invoices.
- Fetch a list of unpaid invoices.

## Getting Started

### Prerequisites

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0) or higher
- [Strike API Key](https://strike.me/) (You'll need this to access the API)

### Setup

1. Clone the repository:

    ```bash
    git clone https://github.com/your-username/strike-api-wrapper.git
    cd strike-api-wrapper
    ```

2. Create a `appsettings.json` file in the root directory with your Strike API credentials:

    ```json
    {
      "StrikeApi": {
        "ApiKey": "your-strike-api-key",
        "Environment": "https://api.strike.me" 
      }
    }
    ```

3. Restore the dependencies:

    ```bash
    dotnet restore
    ```

4. Build the project:

    ```bash
    dotnet build
    ```

5. Run the project:

    ```bash
    dotnet run
    ```

### Usage

Once the project is running, you can access the API using the following routes:

- **Create an Invoice:**

    ```
    POST /api/strike/invoices
    ```

    Sample Request Body:

    ```json
    {
      "correlationId": "12345",
      "description": "Payment for services rendered",
      "currency": "USD",
      "amount": "50.00"
    }
    ```

- **Get Account Profile by Handle:**

    ```
    GET /api/strike/account/{handle}/profile
    ```

- **Get Invoice by ID:**

    ```
    GET /api/strike/invoices/{invoiceId}
    ```

- **Generate Quote for an Invoice:**

    ```
    POST /api/strike/invoices/{invoiceId}/quote
    ```

- **Get Unpaid Invoices:**

    ```
    GET /api/strike/invoices?skip=0&top=10
    ```

### Running Tests

This project includes unit tests built with [xUnit](https://xunit.net/) and [Moq](https://github.com/moq/moq4).

To run the tests, use the following command:

```bash
dotnet test
