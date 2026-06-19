#!/usr/bin/env bash
set -euo pipefail

# ============================================================================
# setup-oidc.sh
#
# Creates an Entra ID app registration with GitHub OIDC federated credentials
# for the picnic-in-the-park repository. Assigns required Azure roles and
# configures GitHub repository variables.
#
# Prerequisites:
#   - Azure CLI (az) authenticated with sufficient permissions
#   - GitHub CLI (gh) authenticated with repo access
#   - jq
#
# Usage:
#   ./setup-oidc.sh <github-org/repo> <subscription-id> <location>
#
# Example:
#   ./setup-oidc.sh skyarkitekten/picnic-in-the-park xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx eastus2
# ============================================================================

REPO="${1:?Usage: $0 <github-org/repo> <subscription-id> <location>}"
SUBSCRIPTION_ID="${2:?Usage: $0 <github-org/repo> <subscription-id> <location>}"
LOCATION="${3:?Usage: $0 <github-org/repo> <subscription-id> <location>}"

APP_NAME="picnic-in-the-park-deploy"
AZURE_ENV_NAME="production"

echo "==> Setting active subscription to ${SUBSCRIPTION_ID}"
az account set --subscription "${SUBSCRIPTION_ID}"

TENANT_ID=$(az account show --query tenantId -o tsv)
echo "==> Tenant ID: ${TENANT_ID}"

# Create or retrieve the app registration
echo "==> Creating/retrieving app registration: ${APP_NAME}"
APP_ID=$(az ad app list --display-name "${APP_NAME}" --query "[0].appId" -o tsv)

if [ -z "${APP_ID}" ] || [ "${APP_ID}" == "None" ]; then
  APP_ID=$(az ad app create --display-name "${APP_NAME}" --query appId -o tsv)
  echo "    Created app registration: ${APP_ID}"
else
  echo "    Found existing app registration: ${APP_ID}"
fi

# Create or retrieve the service principal
echo "==> Creating/retrieving service principal"
SP_ID=$(az ad sp list --filter "appId eq '${APP_ID}'" --query "[0].id" -o tsv)

if [ -z "${SP_ID}" ] || [ "${SP_ID}" == "None" ]; then
  SP_ID=$(az ad sp create --id "${APP_ID}" --query id -o tsv)
  echo "    Created service principal: ${SP_ID}"
else
  echo "    Found existing service principal: ${SP_ID}"
fi

# Create federated credential for main branch
echo "==> Creating federated credential for main branch"
MAIN_CRED_NAME="github-main"
EXISTING_CRED=$(az ad app federated-credential list --id "${APP_ID}" --query "[?name=='${MAIN_CRED_NAME}'].name" -o tsv)

if [ -z "${EXISTING_CRED}" ]; then
  az ad app federated-credential create --id "${APP_ID}" --parameters "{
    \"name\": \"${MAIN_CRED_NAME}\",
    \"issuer\": \"https://token.actions.githubusercontent.com\",
    \"subject\": \"repo:${REPO}:ref:refs/heads/main\",
    \"audiences\": [\"api://AzureADTokenExchange\"]
  }"
  echo "    Created federated credential for main branch"
else
  echo "    Federated credential for main branch already exists"
fi

# Create federated credential for pull requests
echo "==> Creating federated credential for pull requests"
PR_CRED_NAME="github-pr"
EXISTING_PR_CRED=$(az ad app federated-credential list --id "${APP_ID}" --query "[?name=='${PR_CRED_NAME}'].name" -o tsv)

if [ -z "${EXISTING_PR_CRED}" ]; then
  az ad app federated-credential create --id "${APP_ID}" --parameters "{
    \"name\": \"${PR_CRED_NAME}\",
    \"issuer\": \"https://token.actions.githubusercontent.com\",
    \"subject\": \"repo:${REPO}:pull_request\",
    \"audiences\": [\"api://AzureADTokenExchange\"]
  }"
  echo "    Created federated credential for pull requests"
else
  echo "    Federated credential for pull requests already exists"
fi

# Assign Contributor role on the subscription
echo "==> Assigning Contributor role"
az role assignment create \
  --assignee "${SP_ID}" \
  --role "Contributor" \
  --scope "/subscriptions/${SUBSCRIPTION_ID}" \
  --only-show-errors 2>/dev/null || echo "    Role assignment already exists or was created"

# Assign User Access Administrator role (needed for managed identity role assignments)
echo "==> Assigning User Access Administrator role"
az role assignment create \
  --assignee "${SP_ID}" \
  --role "User Access Administrator" \
  --scope "/subscriptions/${SUBSCRIPTION_ID}" \
  --only-show-errors 2>/dev/null || echo "    Role assignment already exists or was created"

# Set GitHub repository variables
echo "==> Setting GitHub repository variables"
gh variable set AZURE_CLIENT_ID --body "${APP_ID}" --repo "${REPO}"
gh variable set AZURE_TENANT_ID --body "${TENANT_ID}" --repo "${REPO}"
gh variable set AZURE_SUBSCRIPTION_ID --body "${SUBSCRIPTION_ID}" --repo "${REPO}"
gh variable set AZURE_ENV_NAME --body "${AZURE_ENV_NAME}" --repo "${REPO}"
gh variable set AZURE_LOCATION --body "${LOCATION}" --repo "${REPO}"

echo ""
echo "============================================"
echo "OIDC federation setup complete!"
echo "============================================"
echo ""
echo "  App Registration:    ${APP_NAME}"
echo "  Client ID:           ${APP_ID}"
echo "  Tenant ID:           ${TENANT_ID}"
echo "  Subscription ID:     ${SUBSCRIPTION_ID}"
echo "  Location:            ${LOCATION}"
echo ""
echo "GitHub repository variables have been set on ${REPO}."
echo "The CD workflow can now authenticate via OIDC."
echo ""
