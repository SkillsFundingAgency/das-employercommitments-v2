# SFA.DAS.EmployerCommitmentsV2

## Getting Started

* Clone das-employercommitmentsv2 repo
* Obtain cloud config
* Run Storage Emulator
* Start (run under kestrel)

## Requirements

1. Install [.NET Core].
2. Install [Docker].

### Windows

Run the following PowerShell commands in the `tools` directory, [Choclatey] will also be installed:

```powershell
> iex ((new-object net.webclient).DownloadString('https://chocolatey.org/install.ps1'))
> choco install dotnetcore-sdk
> choco install docker-desktop
> & "$Env:PROGRAMFILES\Docker\Docker\Docker for Windows.exe"
> docker-compose up -d
```

### macOS

Run the following Bash commands in the `tools` directory, [Homebrew] will also be installed:

```bash
$ /usr/bin/ruby -e "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install)"
$ brew tap caskroom/cask
$ brew cask install dotnet-sdk
$ brew cask install docker
$ open -a docker
$ docker-compose up -d
```

## Setup

### Add configuration

* Clone the [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config) repository.
* Clone the [das-employer-config-updater](https://github.com/SkillsFundingAgency/das-employer-config-updater) repository.
* Run in the `das-employer-config-updater` directory:

```powershell
> dotnet run
```

* Follow the instructions to import the config from the directory that you cloned the `das-employer-config` repository to.

> The two repositories above are private. If the links appear to be dead, make sure that you're logged into GitHub with an account that has access to these i.e. that you are part of the Skills Funding Agency Team organization.

### Add certificates

```powershell
> dotnet dev-certs https --trust
```

## Run

Run in the `src/SFA.DAS.EmployerCommitmentsV2.Web` directory:

```powershell
> dotnet run
```

Alternatively:

* Open `SFA.DAS.EmployerCommitmentsV2.sln` in your IDE e.g. [Rider], [Visual Studio], [Visual Studio Code] etc.
* Start debugging the `SFA.DAS.EmployerCommitmentsV2.Web` project.

## Test

Run in the `src` directory:

```powershell
> dotnet test
```

[.NET Core]: https://dotnet.microsoft.com/download
[Azure Storage Explorer]: http://storageexplorer.com
[Azurite]: https://github.com/azure/azurite
[Choclatey]: https://chocolatey.org
[Docker]: https://www.docker.com
[Git]: https://git-scm.com
[Gulp]: http://gulpjs.com
[Homebrew]: https://brew.sh
[Node]: http://nodejs.org
[Npm]: https://www.npmjs.com/package/npm
[Rider]: https://www.jetbrains.com/rider
[SQL Server]: https://www.microsoft.com/en-us/sql-server/sql-server-2017
[Visual Studio]: https://www.visualstudio.com
[Visual Studio Code]: https://code.visualstudio.com
