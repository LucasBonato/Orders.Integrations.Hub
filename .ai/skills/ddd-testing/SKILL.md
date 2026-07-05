# ddd-testing

**When:** Testing domain entities, value objects, enums, or domain behavior (zero-dependency layer).

## Patterns

- **Record/entity instantiation** via primary constructor — test creation, value equality, and `with` expressions
- **Value equality** — records have structural equality by default; test that two instances with same values are `Equal`
- **Enum mapping** — test that domain enums map to correct underlying values and round-trip through serialization
- **Factory methods** — test `IntegrationKey.From()`, `IntegrationKey.Nothing()`, and validation
- **Reflection-based validation** — test that all integration key types have `[IntegrationKeyDefinition]` and a `Value` field (see `IntegrationKeyValidationTests`)

## Examples

| Test Class | File Path | What It Tests |
|---|---|---|
| `IntegrationKeyValidatorTests` | `Test/.../UnitTests/Integration/IntegrationKeyValidatorTests.cs` | null, whitespace, casing rules |
| `IntegrationKeyValidationTests` | `Test/.../UnitTests/Integrations/IntegrationKeyValidationTests.cs` | reflection: attributes, fields, normalization |
| `IntegrationKeyJsonConverterTests` | `Test/.../UnitTests/Serialization/IntegrationKeyJsonConverterTests.cs` | round-trip, normalization, null, wrapper objects |
| `EnumBasedCancellationReasonUseCaseTests` | `Test/.../UnitTests/Integration/EnumBasedCancellationReasonUseCaseTests.cs` | enum → DTO mapping, empty enum |

## Conventions

- **Domain layer has zero dependencies** — no mocking needed, no NSubstitute, no DI containers
- **Test domain objects directly** via constructor or factory — `IntegrationKey.From("IFOOD")`, `new Order(...)`
- **Use `record` types** for value objects — test structural equality rather than reference equality
- **Use `Theory` + `InlineData`** for enum/value validation tests
- **Test invariants** — validation rules, factory constraints, value normalization
- **Test `with` expressions** to verify immutable updates produce new instances with correct values

## Anti-Patterns

- ❌ **Mocking domain objects:** Domain is pure — test with real instances, never substitutes
- ❌ **Testing infrastructure in domain tests:** Serialization side effects, DB concerns, HTTP concerns don't belong here
- ❌ **Complex fixture setup for domain objects:** Use record constructors directly, not `Fakers` — Fakers are for Application/Adapter test data
- ❌ **Testing implementation details of auto-properties:** Test behavior (validation, equality, normalization), not that getters return what was set
