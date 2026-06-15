# Contributing to Orders.Integrations.Hub

Welcome! This guide explains the development workflow, automated checks, and quality standards for contributing to this project.

## 🚀 Quick Start

1. **Fork & Clone**
   ```bash
   git clone https://github.com/YOUR-USERNAME/Orders.Integrations.Hub.git
   cd Orders.Integrations.Hub
   ```

2. **Setup Environment**
   ```bash
   cp .env.example .env
   dotnet restore Orders.Integrations.Hub.slnx
   ```

3. **Create Feature Branch**
   ```bash
   git switch -c feat/my-feature
   # or: git switch -c fix/issue-123
   ```

4. **Make Changes & Test Locally**
   ```bash
   dotnet test  # Run all tests
   dotnet build -c Release  # Build Release
   ```

5. **Commit & Push**
   ```bash
   git commit -m "feat: add new caching mode"
   git push origin feat/my-feature
   ```

6. **Open PR** (PR template auto-fills)

---

## 📋 Commit Convention

Follow **Conventional Commits**:

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types
- `feat:` — New feature
- `fix:` — Bug fix
- `docs:` — Documentation
- `style:` — Code style (StyleCop, formatting)
- `refactor:` — Code refactor (no behavior change)
- `perf:` — Performance improvement
- `test:` — Test additions/updates
- `chore:` — Build, CI/CD, dependencies
- `ci:` — CI/CD workflow changes

### Examples
```bash
git commit -m "feat(cache): add hybrid L1/L2 caching mode"
git commit -m "fix(ifood): handle webhook timeout correctly"
git commit -m "test(arch): add domain isolation rules"
git commit -m "chore(deps): update MassTransit to 8.3.0"
```

---

## ✅ Automated Checks

When you open a PR, **5 workflows run automatically**. All must pass before merge.

### 1️⃣ **PR Validation** (`pr-validation.yml`)
**Runs on:** PR open/sync/reopen  
**Checks:**
- ✅ StyleCop — no code style violations
- ✅ Nullable refs — no null-safety issues
- ✅ Secrets — no hardcoded credentials
- ✅ Quick unit tests — fails fast

**Time:** ~5-10 min  
**Fail = PR blocked.** Fix style/null errors, push again.

### 2️⃣ **Test Coverage** (`test-coverage.yml`)
**Runs on:** PR + merge to main/develop  
**Checks:**
- ✅ Unit tests (70%+ coverage required)
- ✅ Integration tests (with Redis in-memory)
- ✅ Architecture tests (NetArchTest rules)

**Time:** ~15-20 min  
**Fail = PR blocked.** Debug locally, retest.

### 3️⃣ **Code Quality & Security** (`code-quality.yml`)
**Runs on:** PR + push  
**Checks:**
- ✅ SAST scan (Semgrep) — security vulnerabilities
- ✅ Dependency scan — outdated/vulnerable packages
- ✅ Complexity analysis — high cyclomatic complexity
- ✅ Duplication — copy-paste code
- ✅ License compliance — permitted licenses only

**Time:** ~10-15 min  
**Warn = PR allowed.** Fix high-risk issues.

### 4️⃣ **Architecture Tests** (`architecture-tests.yml`)
**Runs on:** Changes to `Src/**` or tests  
**Checks:**
- ✅ Hexagonal architecture — layers isolated
- ✅ Domain independence — no infra refs in domain
- ✅ Integration modularity — no cross-integration deps
- ✅ Adapter patterns — no business logic in adapters

**Time:** ~10 min  
**Fail = PR blocked.** Refactor to respect architecture.

### 5️⃣ **Build** (`build.yml`)
**Runs on:** Push to main/develop  
**Checks:**
- ✅ Full build Release config
- ✅ All test suites
- ✅ Publish artifact
- ✅ Docker image build (main only)

**Time:** ~15-20 min  
**Fail = Push rejected.** Must pass before merge to main.

---

## 🧪 Local Testing (Before Push)

**Always run locally first:**

```bash
# Fast path (5 min) — catch most issues early
dotnet build Orders.Integrations.Hub.slnx -c Release
dotnet test Test/Orders.Integrations.Hub.UnitTests -c Release

# Full path (20 min) — matches CI exactly
dotnet test Orders.Integrations.Hub.slnx -c Release
```

**StyleCop violations?**
```bash
dotnet build Orders.Integrations.Hub.slnx -c Release \
  -p:EnforceCodeStyleInBuild=true \
  -p:TreatWarningsAsErrors=true
```

**Run architecture tests:**
```bash
dotnet test Test/Orders.Integrations.Hub.ArchTests -c Release
```

---

## 📝 PR Template Sections

When you open a PR, the template guides you:

| Section | Required? | Example |
|---------|-----------|---------|
| **Type** | ✅ Yes | `[x] Bug fix` |
| **Description** | ✅ Yes | "Fixes token expiry check in iFood webhook handler" |
| **Testing** | ✅ Yes | "Unit tests added for edge cases" |
| **Breaking Changes** | ⚠️ If applicable | "API endpoint URL changed (minor version bump)" |
| **Checklist** | ✅ Yes | Check all boxes (StyleCop clean, tests pass, etc.) |
| **Deployment Notes** | ⚠️ If applicable | "New env var: `CACHE__MODE`" |

**Agent-friendly sections:**
- Machines auto-parse the PR template
- Fill all required fields — templates enforce minimal completeness
- Link related issues: `Closes #123`
- Be explicit about test coverage

---

## 🔄 Dependabot Auto-PRs

Dependabot creates PRs for:
- **NuGet packages** — daily, manual review required
- **GitHub Actions** — weekly
- **Docker images** — weekly

**Review checklist:**
- ✅ No breaking changes in major versions
- ✅ Changelog reviewed (if available)
- ✅ Tests pass (auto-checked)
- ✅ Safe to merge

**If tests fail:** Click "Close" + new version waits next scan cycle.

---

## 🚨 Merge Requirements

PR must satisfy **ALL** before merge:

```
✅ PR Validation passed
✅ Test Coverage ≥70%
✅ Architecture Tests passed
✅ Security scan clean (no high-risk)
✅ Build successful
✅ At least 1 approval (human or auto)
```

**Blockers:**
- ❌ StyleCop violations unresolved
- ❌ Unit tests failing
- ❌ Architecture rules broken
- ❌ SAST high-risk findings

---

## 🏗️ Architecture Rules (Enforced)

Keep these **inviolable**:

### Core Layers
```
Domain (contracts, entities, values)
  ↓ (depends on)
Application (use cases, ports)
  ↓ (depends on)
Infrastructure (HTTP, DB, cache)
```

**Rule:** Domain must **never** reference Application or Infrastructure.

### Integrations
- Each integration (iFood, Rappi, 99Food) is **isolated**
- No cross-integration imports
- All use `Core` contracts (adapters, ports)

### Adapters
- Implement `Ports` from Application
- **Zero business logic** (translation only)

---

## 🔐 Security

### Don't Commit:
- ❌ Secrets (API keys, passwords, tokens)
- ❌ Hardcoded URLs in prod code
- ❌ Database connection strings
- ❌ AWS credentials

### Use Instead:
- ✅ Environment variables (`.env`, secret management)
- ✅ Configuration providers (AppEnv.cs)
- ✅ GitHub Secrets (for CI/CD)

**Automatic checks:**
- Trufflehog scans for leaked secrets
- PR blocked if found

---

## 📊 Test Guidelines

### Unit Tests
- Location: `Test/Orders.Integrations.Hub.UnitTests/`
- Coverage: ≥70%
- Naming: `[ClassName]Tests.cs`
- Example: `MemoryCacheServiceTests.cs`

### Integration Tests
- Location: `Test/Orders.Integrations.Hub.IntegrationTests/`
- Use MassTransit TestFramework
- Mock external services or use in-memory alternates
- Example: Command dispatch + handler verification

### Architecture Tests
- Location: `Test/Orders.Integrations.Hub.ArchTests/`
- Use NetArchTest.Rules
- Enforce layering, naming, dependencies
- Example: `Domain must not reference Infrastructure`

**Coverage badge:** Visible in README after first successful PR.

---

## 🚀 Deployment

### Production (main branch)
- Requires manual approval in GitHub Environments
- Build must pass all checks
- Docker image built + pushed to GHCR

---

## ❓ Questions?

- **Workflow issues?** Check `.github/workflows/*.yml`
- **Architecture questions?** See README § Architecture Diagram
- **Test failures?** Run locally first, then check workflow logs
- **Security concerns?** Create private security advisory

---

## 🎯 Summary

**For Humans:**
1. Branch off `dev` (or `main` for hotfixes)
2. Code → test locally → commit (conventional) → push
3. Open PR, fill template, wait for checks
4. Fix failures → push again (auto re-runs)
5. Merge when all green ✅

**For Agents/Bots:**
1. Parse PR template for structured data
2. Await all workflow status checks
3. Validate test coverage ≥70%
4. Approve if all gates pass + no manual review needed
5. Merge via GitHub API