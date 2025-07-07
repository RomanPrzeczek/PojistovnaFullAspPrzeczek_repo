# 🛡️ Pojišťovna – ASP.NET Core MVC aplikace

Webová aplikace pro správu pojištěnců, pojištění a jejich vztahu. Postavena na ASP.NET Core MVC s podporou více jazyků (CZ/EN), ASP.NET Identity a čistou architekturou.

---

## Hlavní funkce

- Správa pojištěnců a pojištění
- Přidávání pojištění pojištěnci 
- ASP.NET Identity – přihlášení, role: `Admin`, `InsuredPerson`
- Multijazyčná podpora (CZ/EN) přes `.resx`
- Nasazení na Railway
- Automatizované* testy (unit*, integrační*, UI)

---

## Use Case scénář

```
[Admin]
   │
   └─➔ Přihlásí se
           │
           └─➔ Vyhledá pojištěnce
                       │
                       └─➔ Přidá pojištění ze seznamu
```

---

## Architektura (zjednodušeně)

```
[Controller]
   ↓
[Service Layer]
   ↓
[Repository Layer]
   ↓
[EF Core DbContext]
```

S využitím AutoMapper, DTO a Interface-based design.

---

## Datový model (zjednodušeně)

```
InsuredPerson
     │ 1
     │         
     ▼ n
PersonInsurance
     │ n
     │         
     ▼ 1
Insurance


InsuredPerson má n záznamů v PersonInsurance
PersonInsurance se vztahuje k 1 Insurance
```

---

## Lokalizace

- Použito `IViewLocalizer` a `.resx` soubory
- Výběr jazyka v layoutu (`cz` / `en`)
- Sdílené texty přes `SharedResources.cs`

---

## Autorizace

- ASP.NET Identity s cookie autentizací
- Role:
  - `Admin`: správa entit
  - `InsuredPerson`: přístup k vlastnímu profilu
- Uživatelská data mapována přes email na pojištěnce

---

## Testování

Testy popsány v TESTS.md:

- ✅ Unit testy (`InsuredPersonService`)
- 🔁 Integrační testy (plánované)
- 🎭 UI testy pomocí Playwright (`Admin login`, `user delete`)

---

## Nasazení

- Railway (PostgreSQL databáze)
- Příprava CI/CD pipeline pomocí GitHub Actions

---

## Struktura projektu (vybrané části)

```
📁 Controllers
📁 Services
📁 DTOs
📁 Data (ApplicationDbContext)
📁 Models
📁 Views
📁 Resources (.resx)
📁 Tests
```

---

## Technologie

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core (PostgreSQL provider)
- AutoMapper
- Identity
- I18n (.resx)
- Playwright
- xUnit

---

## Autor

Roman Przeczek\
[www.rero.cz](https://www.rero.cz)