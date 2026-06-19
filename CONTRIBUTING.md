# Contributing to Picnic In The Park

## GitOps Workflow

This repository follows a **GitOps** development model:

```
Feature branch → Pull Request → CI passes → Review → Merge to main → CD auto-deploys
```

### Branch Rules

- **No direct pushes to `main`** — all changes go through pull requests.
- **CI must pass** before a PR can be merged.
- **At least one review** is required.
- Feature branches should be short-lived and focused on a single change.

### What Happens on Merge

When a PR is merged to `main`, the CD workflow automatically:

1. Authenticates to Azure via OIDC (no stored secrets)
2. Provisions/updates infrastructure with `azd provision`
3. Deploys the application with `azd deploy`

## Setting Up Azure Deployment

### Prerequisites

- [Azure CLI](https://learn.microsoft.com/cli/azure/install-azure-cli) (`az`)
- [GitHub CLI](https://cli.github.com/) (`gh`)
- An Azure subscription with permissions to create app registrations and assign roles
- `jq` (for the Bash script)

### One-Time OIDC Setup

Run the setup script to create the federated identity between GitHub Actions and Azure:

**Bash (macOS/Linux):**

```bash
./infra/scripts/setup-oidc.sh <github-org/repo> <subscription-id> <location>

# Example:
./infra/scripts/setup-oidc.sh skyarkitekten/picnic-in-the-park xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx eastus2
```

**PowerShell (Windows):**

```powershell
./infra/scripts/setup-oidc.ps1 -Repo "skyarkitekten/picnic-in-the-park" -SubscriptionId "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" -Location "eastus2"
```

The script:

1. Creates an Entra ID app registration (`picnic-in-the-park-deploy`)
2. Creates federated credentials for the `main` branch and pull requests
3. Assigns `Contributor` and `User Access Administrator` roles on the subscription
4. Sets the required GitHub repository variables via `gh`

The script is **idempotent** — safe to run multiple times.

### Required GitHub Repository Variables

These are set automatically by the OIDC setup script:

| Variable                 | Description                          |
| ------------------------ | ------------------------------------ |
| `AZURE_CLIENT_ID`        | App registration client ID           |
| `AZURE_TENANT_ID`        | Entra ID tenant ID                   |
| `AZURE_SUBSCRIPTION_ID`  | Target Azure subscription            |
| `AZURE_ENV_NAME`         | AZD environment name (e.g., `production`) |
| `AZURE_LOCATION`         | Azure region (e.g., `eastus2`)       |

### Manual Deployment

To deploy from your local machine:

```bash
# Login to Azure
az login
azd auth login

# Provision and deploy
azd up
```

## Local Development

```bash
# Build everything
dotnet build ./src/PicnicPlanner.slnx
npm run build --prefix ./src/frontend

# Run the full distributed app
aspire run
```

## Code Style

- **C#**: See `.github/instructions/csharp.instructions.md`
- **TypeScript**: See `.github/instructions/typescript.instructions.md`
- **Frontend linting**: `npm run lint --prefix ./src/frontend`
- **Frontend formatting**: `npm run format:check --prefix ./src/frontend`
