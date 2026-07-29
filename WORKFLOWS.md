# xyToolz — Release-Workflows

## TL;DR — was du ausführen musst

```powershell
# im Monorepo-Root, einmalig:
pwsh build/install-hooks.ps1

# Trockenlauf, ändert nichts:
pwsh .githooks/xy-sync-components.ps1 -DryRun
```

Danach ist der Alltag: im Monorepo committen mit `feat:` / `fix:` /
`BREAKING CHANGE`, dann **normal `git push`** — `.githooks/pre-push` ruft
`xy-sync-components.ps1` automatisch auf, für alle Commits seit dem letzten
Push (nicht nur den letzten). Kein manueller Aufruf mehr nötig. Jede
Komponente, deren Quellen sich geändert haben, wird gepusht, gebaut, getestet,
gepackt, veröffentlicht und getaggt. Ein fehlgeschlagener Build blockiert den
`git push` selbst (wie jeder andere pre-push-Hook auch) — Umgehen mit
`git push --no-verify`, aber dann synct nichts.

Manueller Aufruf von `xy-sync-components.ps1` bleibt weiterhin möglich (z. B.
für `-DryRun`, `-Only <component>`, oder um sofort zu synchronisieren ohne
extra Commit).

## NuGet-API-Key: drei Wege, den Hook damit zu versorgen

Der Key selbst landet **nie** im Repo, in `xy-release.conf` oder im Log — in
keinem der drei Wege.

### 1. Umgebungsvariable (einfachster Weg, ein Key für alle Komponenten)

```powershell
[Environment]::SetEnvironmentVariable("XY_NUGET_KEY", "<key>", "User")
pwsh build/install-hooks.ps1 -NuGetSource https://api.nuget.org/v3/index.json -ApiKeyEnv XY_NUGET_KEY
```

`xy-release.conf` bekommt nur `NUGET_API_KEY_ENV="XY_NUGET_KEY"` — den *Namen*,
nicht den Wert. `post-receive` liest den echten Wert erst zur Laufzeit per
`eval "KEY=\${$NUGET_API_KEY_ENV}"`.

### 2. DPAPI-verschlüsselt via xySecurity (`xyDataProtector`)

Für einen einzelnen Windows-Entwickler-Rechner, ohne den Key als Klartext in
irgendeiner Umgebungsvariable stehen zu haben:

```powershell
dotnet build xyToolz_Exec -c Release
# Key wird von STDIN gelesen, NIE als Kommandozeilen-Argument (landet sonst in
# der Shell-History und der Prozessliste):
echo "<key>" | dotnet xyToolz_Exec/bin/Release/net8.0/xyToolz_Exec.dll encrypt build/nuget-api-key.protected

pwsh build/install-hooks.ps1 -NuGetSource https://api.nuget.org/v3/index.json `
    -ApiKeyProtectedFile build/nuget-api-key.protected `
    -KeyToolPath xyToolz_Exec/bin/Release/net8.0/xyToolz_Exec.dll
```

`xy-release.conf` bekommt `NUGET_API_KEY_PROTECTED_FILE` (Pfad zur
verschlüsselten Datei) und `NUGET_KEY_TOOL` (Pfad zum Entschlüssler).
`post-receive` ruft den Tool-Aufruf `decrypt` auf und extrahiert den Klartext
nur zwischen zwei eindeutigen Markern aus stdout — so kann kein zusätzliches
Logging der Bibliothek versehentlich als Key interpretiert werden.

**Wichtig:** DPAPI mit `DataProtectionScope.CurrentUser` entschlüsselt nur
unter demselben Windows-Konto, das `encrypt` ausgeführt hat. Für eine geteilte
Build-Maschine oder mehrere Entwickler ist Weg 1 (Umgebungsvariable) oder Weg 3
(GitHub Actions Secret) besser geeignet. `build/nuget-api-key.protected`
gehört trotzdem nicht ins Git — auch wenn es ohne den richtigen Windows-Account
nutzlos ist, ist es kein Artefakt, das man leichtfertig committet.

### 3. GitHub Actions Secret (für den CI-Pfad, unabhängig von 1 und 2)

Siehe `generate-workflow.sh` — der Key kommt dort als Repository-Secret unter
Settings → Secrets and variables → Actions, referenziert per `secrets.<NAME>`
im generierten Workflow. Hat nichts mit den Umgebungsvariablen-Namen aus Weg 1
oder der `.protected`-Datei aus Weg 2 zu tun, auch wenn es derselbe nuget.org-Key
ist.

Logs pro Komponente: `VersionControl/<id>.git/xy-release.log`

---

## Warum nichts funktioniert hat

### 1. Das Sync-Skript existierte nicht

`xyExtensions.csproj` verweist im Header auf `.githooks/xy-sync-components.ps1`.
Diese Datei war **nirgends im Baum**. Damit fehlte das Bindeglied zwischen
Monorepo und den Komponenten-Repos — und der Zustand sah entsprechend aus:

| Repo | Refs | Hook |
|---|---|---|
| `xyQOL.git` | v1.0.1 – v1.0.3 | ja |
| die anderen **9** Komponenten | **komplett leer** | ja |
| `xyToolz.git` | v1.0.37 – v1.3.2, aktiv | **keiner** |

`xyQOL` lief nur durch, weil es als einzige Komponente **keine**
`ProjectReference` hat. Alles andere wäre ohnehin gescheitert.

### 2. Die Komponenten-csproj können in einem Standalone-Repo nicht bauen

```xml
<Compile Include="..\..\xyToolz\Security\**\*.cs" />   <!-- Quelle liegt woanders -->
<ProjectReference Include="..\xyQOL\xyQOL.csproj" />   <!-- Nachbarordner -->
<PackageOutputPath>..\..\NuGets</PackageOutputPath>    <!-- verlässt das Repo-Root -->
<EnableDefaultCompileItems>false</...>                 <!-- unterdrückt lokale .cs -->
```

Keine dieser vier Zeilen überlebt den Umzug in ein eigenes Repo. Das Sync-Skript
schreibt sie jetzt um: Quellen flach kopieren, `ProjectReference` →
`PackageReference` mit gepinnter Version aus dem Tag des Dependency-Repos.
`build/components.json` hält den Abhängigkeitsgraph und damit die Reihenfolge:

```
xyChrono  xyEnumerables  xyExtensions  xyFonts  xyPdf  xyQOL     (keine Deps)
    └─> xyFilesystem  xySerialization  xyMaths                   (brauchen obige)
            └─> xySecurity                                       (braucht 4 davon)
```

### 3. Bugs in den post-receive-Hooks

Zehnmal dasselbe Skript per Copy-Paste, also jeder Bug zehnmal:

- **`BUILD_DIR` wurde einmal *vor* der Schleife angelegt und *in* der Schleife
  mit `rm -rf` gelöscht.** Push mit mehr als einem Ref ⇒ zweiter Durchlauf
  arbeitet in einem Verzeichnis, das es nicht mehr gibt.
- **`GIT_DIR`, `GIT_QUARANTINE_PATH` usw. wurden nicht entfernt.** Git exportiert
  die in den Hook; sie überleben bis in `dotnet build` und verwirren SourceLink
  und jeden `git`-Aufruf im Temp-Worktree. Klassiker für „lokal geht's, beim Push
  stirbt es".
- **`dotnet test "$test_csproj" --no-build`** — gebaut wurde aber nur
  `$PKG_ID.csproj`. Sobald ein Testprojekt existiert, schlägt das garantiert fehl.
- **Löschen von `master` wurde nicht abgefangen** (`newrev` = lauter Nullen), der
  Hook versuchte einen gelöschten Ref zu bauen.
- **`set -e` + `curl`** — jeder Netzwerkhänger killt den Hook mitten im Push.
- **Der NuGet-Existenzcheck war kaputt**: bei `curl`-Fehler stand `exists` auf
  `000` und es wurde **trotzdem publiziert**.
- **Tag wurde ohne Vorabprüfung gesetzt**; existierte er schon, starb der Hook
  *nach* dem Publish. Nicht reparierbarer Zwischenzustand.
- **`GeneratePackageOnBuild=true`** ⇒ `dotnet build` packte bereits nach
  `..\..\NuGets` (relativ zum Temp-Dir, also ins Nirgendwo), danach packte
  `dotnet pack` nochmal.
- **Nur `PackageVersion` gesetzt, nicht `Version`** ⇒ die Assembly blieb ewig
  auf 1.0.0, während die Paketnummer hochlief.
- **Kein Log, kein Lock.**

Ersetzt durch **eine** Datei `build/post-receive` + je zwei Zeilen
`hooks/xy-release.conf` pro Repo. Ein Fix wirkt ab jetzt überall.

### 4. `ci-template-xyProjects.yml` konnte niemals veröffentlichen

```bash
exists=$(curl -sL https://api.nuget.org/v3-flatcontainer/<id>/$version/...)
if [ -n "$exists" ]; then skip_publish=true
```

Ein 404 vom Flat-Container liefert **trotzdem einen nicht-leeren Body**. `$exists`
war also nie leer, `skip_publish` immer `true` — der Workflow hat kein einziges
Paket gepusht. Jetzt wird der HTTP-Status geprüft.

Dazu:

- Version wurde aus der `.csproj` gelesen, per `sed` neu gesetzt und **nie
  committet**. Der nächste Lauf las denselben alten Wert. Die Version konnte
  strukturell nicht steigen. Quelle ist jetzt der Git-Tag.
- `dotnet test <ProjectName>.csproj` gegen eine Klassenbibliothek ⇒ immer rot.
- Kein `permissions: contents: write` ⇒ `action-gh-release` bekommt 403.
- `fetch-depth` fehlte ⇒ keine Tags im Checkout.
- Kein `concurrency` ⇒ zwei Pushes publizieren dieselbe Version.
- `softprops/action-gh-release@v1` ist veraltet (v2).

### 5. CRLF — der teuerste unsichtbare Fehler

`.gitattributes` hatte nur `* text=auto`. Unter Windows landen `.sh` und `.yml`
damit mit CRLF im Working Tree:

- `generate-workflow.sh` begann mit `#!/bin/bash\r` → `bad interpreter: /bin/bash^M`.
  Das Skript war **nicht ausführbar**.
- Jeder `run: |`-Block im generierten YAML trug ein `\r` am Zeilenende. Damit
  schrieb `echo "version=$new_version" >> $GITHUB_ENV` den Wert `1.2.3\r`, und
  jeder Folgeschritt rechnete mit einem Carriage Return in der Versionsnummer.

Beides sieht man in keinem Editor. Fix in `gitattributes-append.txt`.

### 6. Fehlende Infrastruktur

Kein `nuget.config`, kein `Directory.Build.props`, kein `global.json`. Der Feed
`nuget-local` existierte nur in einer maschinenlokalen NuGet-Config — auf jedem
anderen Rechner scheitert `restore` sofort. Der Hook baut außerdem in `/tmp`,
wo NuGets Config-Suche nach oben ins Leere läuft; er übergibt die Feeds jetzt
explizit per `--source`.

---

## Was ich bewusst **nicht** angefasst habe

Das sind Entscheidungen, die dir gehören — nicht mir um 5 Uhr morgens:

1. **`VersionControl.zip` (2,9 MB) und `NuGets/*.nupkg` liegen im Arbeitsbaum.**
   Beide Ordner stehen in `.gitignore`, sind also vermutlich nur lokal. Falls doch
   mal eingecheckt: `git rm -r --cached`.
2. **`xyToolz.sln` enthält nur `xyToolz.csproj`.** Die zehn Komponenten und
   `xyToolz_Exec` fehlen — deshalb baut ein Solution-Build sie nie mit und du
   merkst Breakages erst beim Push.
3. **`file.txt` im Root** (14 Bytes) sieht nach Versehen aus.
4. **Doppelte Commits im xyQOL-Verlauf** (`sync: QOL @ ddaef18` steht zweimal
   drin, einmal mit kurzer, einmal mit langer SHA). Historie, tut nicht weh.
5. **`xyToolz.csproj` hat `PackageVersion 0.0.0-local`**, während `xyToolz.git`
   schon bei `v1.3.2` steht. Der Hook nimmt jetzt den Tag, insofern egal — aber
   verwirrend beim lokalen Bauen.
6. **`xyToolz_Exec` referenziert `xyToolz` gar nicht** und ist in keinem Repo.
7. **Kein einziges Testprojekt existiert.** Hook und Workflow überspringen Tests
   sauber, aber „Run tests" ist damit aktuell eine leere Geste.

---

## Dateien in diesem Fix

| Datei | Was |
|---|---|
| `build/post-receive` | **Neu.** Ein Hook für alle Repos, ersetzt 10 Kopien |
| `build/components.json` | **Neu.** Komponenten + Abhängigkeitsgraph + Reihenfolge |
| `build/install-hooks.ps1` | **Neu.** Verteilt Hook + Config in alle Bare-Repos |
| `.githooks/xy-sync-components.ps1` | **Neu.** Das fehlende Sync-Skript |
| `ci-template-xyProjects.yml` | Ersetzt. Publiziert jetzt tatsächlich |
| `generate-workflow.sh` | Ersetzt. LF, `set -euo pipefail`, Validierung |
| `nuget.config` | **Neu.** Lokaler Feed reist mit dem Repo |
| `Directory.Build.props` | **Neu.** Metadaten an einer Stelle statt 10× dupliziert |
| `global.json` | **Neu.** SDK-Band auf .NET 8 festgenagelt |
| `gitattributes-append.txt` | An `.gitattributes` anhängen — behebt CRLF |

## Was getestet ist

Der Hook lief gegen echte Bare-Repos mit echten `git push`-Aufrufen
(`dotnet` als Stub, da im Container kein SDK):

- Versionssprünge: erstes Release → 1.0.0, `fix:` → Patch, `feat:` → Minor,
  `BREAKING CHANGE` → Major, `release: x.y.z` → exakt, ohne Keyword → kein Release
- Tag-Sortierung `v1.0.10 > v1.0.9`
- Multi-Ref-Push (der Fall, der den alten Hook zerlegt hat)
- `master` gelöscht → sauberer Skip
- Build-Fehler → kein Publish, kein Tag, Compilerfehler steht im Log
- Test-Fehler → Abbruch vor dem Publish
- Bereits existierender Tag → Abbruch **vor** dem Publish
- Lock gegen Parallelläufe

Das generierte YAML parst, und die Versionslogik wurde isoliert gegen alle
Commit-Typen geprüft.

Die beiden PowerShell-Skripte konnte ich hier nicht ausführen (kein `pwsh` im
Container). Die csproj-Umschreibung habe ich stattdessen gegen die echten
Dateien nachgestellt und verifiziert — inklusive `xySecurity` mit seinen vier
`ProjectReference`. Lass sie trotzdem einmal mit `-DryRun` laufen, bevor du
sie scharf schaltest.
