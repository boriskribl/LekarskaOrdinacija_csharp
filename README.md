# Lekarska ordinacija – Praktikum primenjenog programiranja

Web aplikacija za upravljanje lekarskom ordinacijom razvijena je korišćenjem ASP.NET Core MVC (.NET 8), Entity Framework Core, ASP.NET Core Identity i MS SQL Server baze podataka.

## Potrebno za pokretanje

- Visual Studio sa podrškom za ASP.NET Core
- .NET 8 SDK
- Microsoft SQL Server / SQL Server Express
- SQL Server Management Studio (SSMS)

## Baza podataka

SQL skripta:

`database/LekarskaOrdinacija.sql`

kreira i inicijalno popunjava bazu:

`LekarskaOrdinacijaDb`

Aplikacija je testirana korišćenjem SQL Server Express instance:

`.\SQLEXPRESS`

Ukoliko se na računaru na kojem se projekat pokreće koristi druga SQL Server instanca, potrebno je izmeniti vrednost `Server` u `DefaultConnection` unutar fajla `appsettings.json`.

Primer:

`Server=.\\SQLEXPRESS;Database=LekarskaOrdinacijaDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True`

## Pokretanje projekta

1. U SQL Server Management Studio-u povezati se na odgovarajuću SQL Server instancu.
2. Otvoriti i izvršiti `database/LekarskaOrdinacija.sql`.
3. Otvoriti `LekarskaOrdinacija.slnx` u Visual Studio-u.
4. Proveriti `DefaultConnection` u `appsettings.json` i po potrebi promeniti naziv SQL Server instance.
5. Izvršiti `Build → Rebuild Solution`.
6. Pokrenuti aplikaciju iz Visual Studio-a.

Ako `.slnx` format nije podržan u korišćenoj verziji Visual Studio-a, projekat se može otvoriti direktno preko:

`src/LekarskaOrdinacija/LekarskaOrdinacija.csproj`

## Test nalozi

### Administrator
Username: `admin`  
Password: `Admin123!`

### Lekar
Username: `lekar`  
Password: `Lekar123!`

### Medicinska sestra
Username: `sestra`  
Password: `Sestra123!`