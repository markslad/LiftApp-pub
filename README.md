# LiftApp
Část projektu tvořeného v rámci diplomové práce, který reprezentuje samotnou aplikaci.

## Spuštení
### Prerekvizity
- Nainstalovaný **PostgreSQL** (testováno na verzi **PostgreSQL 15.1, compiled by Visual C++ build 1914, 64-bit** - pro ověření slouží SQL dotaz `SELECT VERSION();`)
- Nainstalovaný **.NET 6**
- Nainstalovaný Microsoft Excel (testováno na verzi **Microsoft Excel pro Microsoft 365 MSO (Version 2302 Build 16.0.16130.20298) 64 bitů**)
- Provedena migrace datového modelu do PostgreSQL pomocí projektu **LiftApp-migrations** (návod v README.md v projektu **LiftApp-migrations**)

### Postup
1. Upravit connection string `ConnectionStrings.DefaultConnection` v konfiguračním souboru `src\LiftApp\appsettings.json`. **Connection string musí být totožný v obou projektech LiftApp-migrations i LiftApp !!!**
2. Následně je možné projekt sestavit a spustit aplikaci