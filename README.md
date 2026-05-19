# Picnic In The Park

Picnic In The Park is a simple reference implementation of a multi-agent system built using the [Microsoft Agent Framework](https://learn.microsoft.com/en-us/agent-framework/overview/?pivots=programming-language-csharp) framework. It is designed to demonstrate how to create and manage multiple agents that interact with each other in a shared environment.

## Prerequisites

- .NET 10.0 SDK or later
- Python 3.14 or later
- UV 0.8.0 or later
- Aspire 13.3.0 or later
- Azure CLI
- Azure Developer CLI
- Docker or Podman
- An Azure subscription (for deploying to Azure)

## Getting Started

1. Clone the repository:

   ```bash
    git clone
    cd picnic-in-the-park
   ```

2. Build the projects:

   ```bash
    # Build the backend and services
    dotnet build ./src/PicnicPlanner.slnx.

    # Build the frontend application
    npm run build --prefix ./src/frontend
   ```

3. Run the application:

   ```bash
   aspire run
   ```

## Contributing

## License
