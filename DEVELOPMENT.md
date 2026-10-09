# Development notes

## Layout
.NET 8 monorepo. `xyToolz` bundles utility modules that also ship as standalone NuGets from
`xyComponents/*` (xyChrono, xyEnumerables, xyExtensions, xyFilesystem, xyFonts, xyMaths, xyPdf,
xyQOL, xySecurity, xySerialization). `xyToolz_Exec` is a small CLI (used e.g. to DPAPI-encrypt
the NuGet API key). `xyToolz.Tests` covers Filesystem, Maths, Security and Serialization.

The interfaces `IxyFiles`, `IxyJson` and `IxyDataProtector` only exist for test doubles of the
static classes; they are not public API.

## Rules
- Stay on .NET 8. `global.json` pins the SDK band (`8.0.100`, rollForward `latestFeature`).
- Never bypass the git hooks (`--no-verify`). `.githooks/pre-push` runs
  `.githooks/xy-sync-components.sh` on every push, which syncs every `xyComponents/*` whose
  sources changed into its bare repo under `VersionControl/`; the bare repo's post-receive
  hook then builds, tests, packs, publishes and tags it. Skipping the hook lets components
  drift out of sync.
- NuGet key handling is described in `WORKFLOWS.md`. The key never goes into the repo,
  `xy-release.conf`, shell history or a log.
- A commit subject with `fix:`, `feat:` or `BREAKING CHANGE` triggers a release on push.

## Conventions
- Nullable enabled, implicit usings on.
- `IDE1006` (naming), `CS1591` (missing XML doc) and `CA1416` (platform compat) are silenced
  on purpose in `.editorconfig`.
- Static helper classes are prefixed `xy` (`xyFiles`, `xyHasher`, `xyJson`, ...).

## Build and test
Always from the repository root, not from inside a project folder:

```bash
dotnet build xyToolz.sln
dotnet test xyToolz.Tests
```
