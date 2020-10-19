# PIN - Pirate Intelligence Network

The Accord may think their Shared Intelligence Network is unique and impenetrable,
but not everyone agrees with their restricted access and constant surveillance.
That's why PIN, the Pirate Intelligence Network, has been created.

*Fight the Accord - Kill the Chosen*

## Usage

1. Install Firefall via Steam
2. Edit the `firefall.ini` located in `steamapps\common\Firefall`
3. Add content from below
4. Add "127.0.0.1 tmw.local" to your hosts file (C:\Windows\System32\drivers\etc\hosts must be opened as admin)
5. Recursive clone the repository `git clone --recurse-submodules https://github.com/themeldingwars/PIN.git`
6. Build the solution
7. Run WebHostManager\create-cert.ps1 as an admin
8. Start multiple targets at once
	- Visual Studio: Create a `Multiple Startup Projects` target that start WebHostManager, MyGameServer and MyMatrixServer
	- ReShaper: Create a `Compound` target that starts WebHostManager, MyGameServer and MyMatrixServer
9. Start Firefall

```ini
[Config]
OperatorHost = "tmw.local:4400"

[FilePaths]
AssetStreamPath = "http://tmw.local:4401/AssetStream/%ENVMNEMONIC%-%BUILDNUM%/"
VTRemotePath = "http://tmw.local:4401/vtex/%ENVMNEMONIC%-%BUILDNUM%/static.vtex"

[UI]
PlayIntroMovie = false
```

## Development

### Web Hosts

| Host       | HTTP | HTTPS |
|------------|------|-------|
| Operator   | 4400 | 44300 |
| WebAsset   | 4401 | 44301 |
| ClientApi  | 4402 | 44302 |
| InGame     | 4403 | 44303 |
| Store      | 4406 | 44306 |
| Chat       | 4407 | 44307 |
| Replay     | 4408 | 44308 |
| Market     | 4410 | 44310 |
| Catchall   | 4499 | 44399 |

### Game Server

| Host          | UDP   |
|---------------|-------|
| Matrix Server | 25000 |
| Game Server   | 25001 |