# 🧪 TESTOVÁNÍ – Pojišťovna App

Tento dokument shrnuje implementované a plánované testy pro aplikaci Pojišťovna. Pokrývá unit testy, integrační testy, UI testy a CI/CD automatizaci.

Struktura testů dle typu je realizována formou samostatných C# projektů níže:
- PojistovnaFullAspPrzeczek.Tests.Unit.xUnit
- PojistovnaFullAspPrzeczek.Tests.Integration.xUnit
- PojistovnaFullAspPrzeczek.Tests.UI.Playwright
se závislostí na projektu aplikace PojistovnaFullAspPrzeczek.

Projekty jsou verzovány prostřednictvím repozitáře PojistovnaFullAspPrzeczek_repo.

---

## Unit testy

Unit testy ověřují jednotlivé metody ve **službách** (Service layer), bez závislosti na kontrolerech či databázi.

### Testováno:
- `InsuredPersonServiceTests`
  - `GetAllAsync()` – načítání všech pojištěnců

### Technologie:
- `xUnit` including runner.visualstudio
- .NET.Test.Sdk
- Moq (mockování závislostí)

---

## Integrační testy

Cílem je testovat reálné **toky přes API**, od controlleru přes service až po databázi.

### Testováno:
- `InsuredPersonsIntegrationTests`
  - `GetAsync("/InsuredPersons")` – načtení stránky se všemi pojištěnci
	- simulování přihlášení admina pro dosažení endpointu

### Technologie:
- `xUnit`
- AspNetCore.Mvc.Testing
- .NET.Test.Sdk
- InMemory context (pro kontrolu bez DB)

---

## UI testy – Playwright

UI testy simulují chování uživatele v prohlížeči.

### Testováno:
- `Admin_Can_Login()` – přihlášení administrátora

### Technologie:
- Microsoft.Playwright.MSTest
-  .NET.Test.Sdk

### Předpoklady:
- Nasazená aplikace (Railway, nebo localhost)
- Testovací uživatelé a data (seed nebo předskriptování)

---

## CI/CD – GitHub Actions 

### Automatizace buildů a testů
- Build .NET projektu
- Spuštění unit testů
- Spuštění integračních testů

- spuštění UI testů manuálně, není součástí CI/CD

### Nasazení
- Deployment na Railway po úspěšném buildu/testu

---

## Autor testovací strategie

Roman Przeczek  
[www.rero.cz](https://www.rero.cz)

