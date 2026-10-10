# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### No Activity

No merged pull requests this week (October 3–10, 2026). Repository is stable.

---

## [1.0.0] - 2026-08-28

### 🎨 Changed

- **Frontend Redesign** (#26)
  - Complete visual redesign with warm "Golden Hour" aesthetic
  - Updated color palette and typography system
  - Enhanced user experience and visual appeal
  - Improved accessibility compliance (WCAG 2.2 AA)

### 🔧 Fixed

- **CD Deployment** (#24)
  - Fixed CD deployment failures by resolving OIDC federation and cache-password issues
  - Added missing production federated identity credential (Azure-side fix)
  - Removed explicit cache password parameter, allowing AddRedis auto-generation
  - Improved deployment reliability and automation

- **CI/CD Runtime** (#21)
  - Bumped GitHub Actions to Node.js 24 runtime
  - Maintained compatibility with latest Node.js LTS
  - Resolved runtime compatibility issues

### 🔐 Security

- **Cache Container Ingress Security** (#23)
  - Investigated SecOps report of publicly exposed cache container
  - Verified current code produces internal-only ingress for cache (not externally exposed)
  - Upgraded Aspire packages to 13.4.6 for security hygiene
  - Added defensive code comment documenting cache must never have external ingress
  - Confirmed live environment matches code-level security posture

- **Production Authentication** (#20)
  - Added GitHub Environment federated credential for production CD environment
  - Fixed OIDC setup scripts to create proper `github-production` subject
  - Documented production-environment authentication requirements

### 📚 Documentation

- **Security Assessment** (#23, resolving #22)
  - Investigated and documented cache container ingress security finding
  - Created detailed analysis confirming security posture is correct
  - Added recommendations for environment hardening

### 📦 Infrastructure

- **Azure Resource Naming** (#15)
  - Prefixed Azure resource group with `rg-picnic-planner-`
  - Standardized naming convention for better resource organization
  - Improved resource management and discoverability

- **CI/CD Gitops Infrastructure** (#16)
  - Scaffolded CI/CD pipeline with Gitops approach
  - Established infrastructure-as-code patterns with Bicep
  - Improved deployment automation

- **CD Workflow Flexibility** (#14)
  - Allow CD workflow to read Azure configuration from vars or secrets
  - Added fallback mechanism for flexible deployment contexts
  - Improved deployment robustness

### ✅ Validation

- **System State Validation** (#19, #18)
  - Validated current picnic planner state
  - End-to-end functionality verification
  - System integrity confirmation

---

## Previous Releases

### [0.1.0] - 2026-06-23

Initial release with core agent framework, CI/CD pipeline, and Azure infrastructure scaffolding.

- Introduced Microsoft Agent Framework agents (coordinator, invitation, weather)
- Scaffolded CI/CD pipeline with GitHub Actions
- Set up Azure infrastructure with Bicep templates
- Implemented basic park and weather services
- Established initial frontend with Vite + React + TypeScript

---

## Summary Statistics

### Current Week (Oct 3–10, 2026)
- **Merged PRs**: 0
- **Open PRs**: 1 (Release notes documentation)
- **Repository Status**: Stable

### Recent Activity (Past 6 Weeks)
- **Total Merged PRs**: 7
- **Major Features**: 1 (Frontend redesign)
- **Bug Fixes**: 2 (CD deployment, CI runtime)
- **Security Enhancements**: 2 (OIDC, cache ingress investigation)
- **Infrastructure Improvements**: 2 (Resource naming, CD flexibility)
- **Validation & Documentation**: 2 (System validation, security docs)

---

## Notes

- The repository last received significant updates on August 28, 2026 with PR #26 (frontend redesign)
- All recent changes have been focused on stabilizing deployment, enhancing security, and improving user experience
- The system is currently stable and awaiting next sprint planning
