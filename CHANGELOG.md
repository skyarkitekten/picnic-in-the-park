# Changelog

All notable changes to the Picnic Planner project are documented in this file.

## [Unreleased]

### September 6–12, 2026

#### 🎨 Frontend Redesign: Golden Hour Visual System
- **#26**: Completely redesigned the Picnic Planner interface to reflect the warm, social nature of the product
  - Replaced cold dark monitoring-dashboard aesthetic with golden-hour visual system
  - Full-bleed hero photograph with scrim overlay guaranteeing text contrast
  - Warm color palette throughout (field `#fdf4e7`, terracotta `#b03a0b` accents)
  - **WCAG 2.2 AA contrast compliance by design** — resolves 5 P1 audit findings:
    - Badge contrast
    - Hairline borders below 3:1
    - Invisible light-mode table dividers
    - Generic page title
    - White-on-accent chrome
  - Serif typeface for human voice, sans-serif for agent output
  - CSS-based animations and elevation via shadows instead of borders
  - Refactored WeatherCard and BudgetCard from inline styles to CSS classes
  - Added `PRODUCT.md` and `DESIGN.md` as durable design authority
  - Machine-readable `.impeccable/design.json` sidecar for design validation

#### 🚀 Infrastructure & Deployment Fixes
- **#24**: Fixed CD deployment failures — removed redundant cache password parameter
  - Aspire's `AddRedis` already creates password parameter with generated default
  - Explicit redundant parameter was preventing `azd deploy --no-prompt` from resolving values
  - Resolved: `error calling securedParameter: parameter cache_password not found`
  
- **#20**: Added production OIDC federated credentials for GitHub → Azure auth
  - Created federated identity credential for production environment
  - Enables secure OIDC token flow without stored secrets
  
- **#14**: Enhanced CD workflow to fall back to repository secrets
  - CD now reads Azure config from organization variables with secrets fallback
  - Improves flexibility across different deployment contexts

#### 🔧 Security & Maintenance
- **#23**: Investigated and documented cache container ingress security finding
  - Verified external ingress is disabled on deployed cache resource
  - Added defensive code comment in AppHost to prevent accidental exposure
  - Updated Aspire packages: 13.3.0 → 13.4.6 (latest stable)
    - `Aspire.AppHost.Sdk`
    - `Aspire.Hosting.JavaScript`
    - `Aspire.Hosting.Redis`
  
- **#21**: Updated GitHub Actions to Node.js 24 runtime
  - Bumped actions/checkout, actions/setup-node, actions/setup-dotnet from v4 → v5
  - Resolves Node.js 20 deprecation warnings in CI/CD
  
- **#16**: Prefixed Azure resource group name for consistency
  - Resource group now: `rg-picnic-planner-*`
  - Improves resource organization and naming standards

#### ✅ System Validation
- **#19**: Validated full picnic planner state end-to-end
  - Added CI format validation
  - Fixed local Aspire agent wiring
  - Enabled hosted agent provisioning and invocation in Azure
  - Confirmed system works from local dev to production deployment

---

## Summary

**Week of September 6–12, 2026:** Major visual refresh paired with critical infrastructure stabilization. The product now visually matches its warm, social domain. CD pipeline is fully operational with OIDC authentication, secrets fallback, and validated agent deployment end-to-end.

**Stats:**
- 7 PRs merged
- 5 P1 accessibility findings resolved
- 2 major deployment blockers fixed
- 1 complete visual system redesign
- 100% end-to-end validation passing
