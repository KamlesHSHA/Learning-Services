# Persistence (Cosmos DB) - learnova.LearningService.Persistence

This folder contains the Cosmos DB persistence implementation for the Learning Service. It is intentionally minimal and expects a valid Cosmos DB configuration at startup.

Important notes
- Do NOT put real Cosmos credentials into source control.
- The application fails fast on startup if required Cosmos configuration is missing or incomplete. This is by design for local development and production environments.

Required configuration keys
The following configuration values must be provided at runtime (appsettings, environment variables or user secrets):

- Cosmos:Endpoint          (e.g. https://your-account.documents.azure.com:443/)
- Cosmos:Key               (your primary or secondary key)
- Cosmos:DatabaseName      (database name used by this service)

Using .NET user secrets (recommended for local development)

1. From the project directory (API project), enable user secrets if not already enabled:

   dotnet user-secrets init

2. Set the required keys:

   dotnet user-secrets set "Cosmos:Endpoint" "https://your-account.documents.azure.com:443/"
   dotnet user-secrets set "Cosmos:Key" "<your-key>"
   dotnet user-secrets set "Cosmos:DatabaseName" "LearnovaDatabase"

Using environment variables

You can also set these as environment variables. For example (PowerShell):

   $env:Cosmos__Endpoint = 'https://your-account.documents.azure.com:443/'
   $env:Cosmos__Key = '<your-key>'
   $env:Cosmos__DatabaseName = 'LearnovaDatabase'

Note: use double underscore (`__`) to represent a configuration section separator in environment variable names.

Expected Cosmos database & containers

On startup the service will attempt to create the database and the following containers if they do not already exist:

- Courses          -> partition key: /id
- Subjects         -> partition key: /courseId
- Units            -> partition key: /subjectId
- Topics           -> partition key: /unitId
- LearningResources-> partition key: /topicId
- Progress         -> partition key: /userId

This behavior is executed during DI setup and is fail-fast: if Cosmos configuration is missing, startup throws an InvalidOperationException with a clear message.

What happens when configuration is missing

- If any of Cosmos:Endpoint, Cosmos:Key, or Cosmos:DatabaseName is not provided or empty, the application will throw an InvalidOperationException during service registration with the message:

  "Cosmos configuration is missing or incomplete. Please configure 'Cosmos:Endpoint', 'Cosmos:Key' and 'Cosmos:DatabaseName'."

- This is intentional to prevent the service from starting in an invalid state. Supply configuration via user secrets or environment variables to run locally.

Security

- Never commit keys or credentials to source control. Use user secrets for local development and environment variables or a secure secrets store in CI/CD and production.
