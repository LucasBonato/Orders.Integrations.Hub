## Type of Change
<!-- Mark with [x] the relevant option(s) -->
- [ ] **Bug fix** (non-breaking, fixes existing functionality)
- [ ] **Feature** (non-breaking, adds new functionality)
- [ ] **Refactor** (code improvement, no behavior change)
- [ ] **Documentation** (docs/comments only)
- [ ]  **Architecture** (major structural change)
- [ ] **Performance** (optimization, speed improvement)
- [ ] **Security** (vulnerability fix, security hardening)
- [ ] **Breaking Change** (modifies public API, requires major version bump)

---

## Description

### What does this PR do?
<!-- Concise explanation of changes. Agent should understand intent instantly. -->


### Why is this change needed?
<!-- Business reason, bug description, performance issue, etc. -->


### Related Issues
<!-- Link to GitHub issues: Closes #123, Relates to #456 -->


---

## Testing

### Tests Added/Updated
<!-- List which test files changed. Include counts if significant. -->
- [ ] Unit tests added
- [ ] Integration tests added
- [ ] Architecture tests added
- [ ] E2E tests added

### Manual Testing Steps
<!-- If manual testing needed, describe how to verify locally. -->
1. 
2. 
3. 

### Coverage Impact
<!-- Does coverage increase, decrease, or stay same? -->
- Before: X%
- After: Y%

---

## Checklist

<!-- REQUIRED: All must be checked before merge. -->
- [ ] Tests pass locally (`dotnet test`)
- [ ] Architecture tests pass (`dotnet test Test/Orders.Integrations.Hub.ArchTests`)
- [ ] StyleCop warnings resolved or suppressed (no new style violations)
- [ ] No NullReference exceptions (Nullable ref types validated)
- [ ] PR description matches actual changes
- [ ] Commit messages follow Conventional Commits (`feat:`, `fix:`, `docs:`, etc.)
- [ ] No hard-coded values (credentials, URLs, API keys)

---

## Breaking Changes?
<!-- CRITICAL: Mark if API/behavior changes. Requires major version bump. -->
- [ ] No breaking changes
- [ ] **BREAKING:** Explain:


---

## Code Quality Notes

### Dependency Changes
<!-- List any new NuGet packages, versions bumped, or removed. -->
- [ ] No dependency changes
- Changes:
  - 


### Integration Points Affected
<!-- Which integrations (iFood, Rappi, 99Food, etc.) or services affected? -->


### Environment Variables Added/Changed
<!-- Any new .env keys or modified behavior? -->
- [ ] No env var changes
- New/Modified:
  - 


---

## Deployment Notes

### Database Changes
- [ ] No DB changes
- [ ] Migrations required: (describe)

### Configuration Changes
- [ ] No config changes
- [ ] Changes: (describe)

### Performance Impact
- [ ] No impact
- [ ] Positive (faster/less memory)
- [ ] Negative (slower/more memory): mitigated by:


---

## 👥 Review Checklist (for Reviewers)

- [ ] Changes align with described intent
- [ ] Tests cover new/changed code paths
- [ ] No security issues (no hardcoded secrets, unsafe patterns)
- [ ] Architecture rules respected (domain isolation, layering, etc.)
- [ ] Code style matches project (StyleCop clean)
- [ ] Documentation updated if needed
- [ ] Commit history is clean (squash trivial commits if needed)
