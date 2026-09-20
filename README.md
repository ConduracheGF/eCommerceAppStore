# 🛒 eCommerceAppStore - System Documentation

`eCommerceAppStore` este o soluție software completă de tip Full-Stack destinată gestiunii magazinelor online. Proiectul combină un backend REST API robust creat în **.NET / ASP.NET Core** cu o interfață desktop modernă și intuitivă creată în **Windows Forms (WinForms)**.

---

## 📐 Arhitectura Proiectului

Soluția este structurată pe 3 proiecte principale:

eCommerceAppStore/
├── eCommerceAppStore.Api/          # Backend REST API (ASP.NET Core & EF Core)
│   ├── Controllers/               # Controller-e REST (Products, Orders)
│   ├── Data/                      # DbContext (EF Core) și Entitățile SQL
│   ├── DataTransferObject/        # Obiecte DTO pentru validarea cererilor
│   ├── Middleware/                # Middleware global pentru tratarea erorilor
│   └── Properties/                # Setări de lansare (launchSettings.json)
│
├── eCommerceAppStore.WinForms/     # Frontend Desktop Client (.NET WinForms)
│   ├── ApiService.cs              # Client HTTP static pentru apelarea rutei REST
│   ├── MainForm.cs                # Fereastra principală și meniul de navigare
│   ├── ProductForm.cs             # Fereastră modală pentru adăugare/editare produs
│   ├── ProductsControl.cs         # Interfață gestiune produse (CRUD & Filtrare)
│   ├── OrdersControl.cs           # Interfață vizualizare comenzi
│   ├── ReportsControl.cs          # Grafice GDI+ și simulări de stoc/vânzări
│   └── SettingsControl.cs         # Configurare conexiune la server API
│
└── eCommerceAppStore.Tests/        # Suită de teste unitare (xUnit)
    └── OrdersControllerTests.cs   # Testare logică de stoc și creare comenzi

---

## 🚀 Tehnologii Utilizate

* **Backend:** C# | .NET | ASP.NET Core Web API | Entity Framework Core (SQL Server / In-Memory DB) | Swagger (OpenAPI)
* **Frontend:** C# WinForms | GDI+ Custom Drawing | `HttpClient` (`System.Net.Http.Json`)
* **Securitate & Configurare:** JWT Bearer Authentication | CORS Policy ("AllowAll")
* **Testing:** xUnit | Entity Framework Core In-Memory

---

## 🛠️ Detalii Implementare Backend (`eCommerceAppStore.Api`)

### 1. Baza de Date și Entități (`AppDbContext.cs`)
* **`Product`**:
  * `Id` (int, Primary Key)
  * `Name` (string, obligatoriu, max. 20 caractere)
  * `Price` (decimal, `decimal(18,2)`, [0.01 - 100,000])
  * `Stock` (int, [0 - 10,000])
* **`Order`**:
  * `Id` (int, Primary Key)
  * `CustomerEmail` (string)
  * `TotalAmount` (decimal)
  * `CreatedAt` (DateTime, UTC)

### 2. Endpoints REST API

#### 📦 Produs Controller (`/api/products`)
* **`GET /api/products`**: Preluare produse cu suport pentru:
  * Căutare după nume (`search`)
  * Filtrare după preț minim/maxim (`minPrice`, `maxPrice`)
  * Filtrare după disponibilitate în stoc (`inStock`)
  * Sortare (`sortBy`: `price_asc`, `price_desc`, `name_desc`, implicit `name_asc`)
  * Paginare (`pageNumber`, `pageSize`)
* **`GET /api/products/{id}`**: Preluare produs după ID.
* **`POST /api/products`**: Creare produs nou.
* **`PUT /api/products/{id}`**: Actualizare date produs existent.
* **`DELETE /api/products/{id}`**: Ștergere produs după ID.

#### 🛒 Comenzi Controller (`/api/orders`)
* **`GET /api/orders`**: Lista tuturor comenzilor din sistem.
* **`POST /api/orders`**: Creare comandă nouă folosind `CreateOrderDto`:
  * Verifică existența produsului.
  * Verifică stocul disponibil.
  * Scade automat stocul produsului cu cantitatea comandată.
  * Calculează valoarea totală (`Price * Quantity`).

### 3. Middleware & Tratarea Erorilor
* **`ExceptionMiddleware.cs`**: Prinde orice eroare neprevăzută pe server și returnează un răspuns standardizat JSON cu statutul `500 Internal Server Error`, jurnalizând detaliile excepției.

---

## 🖥️ Detalii Implementare Frontend (`eCommerceAppStore.WinForms`)

### 1. Interfață & Design
* **Dark Mode Theme**: Interfață stilizată cu nuanțe închise (`#1E1E2E`, `#181825`, `#28283C`).
* **Design Customizat DataGridView**: Tabele stilizate fără margini albe, cu header întunecat și rânduri alternante.
* **Design Responsive**: Navigare prin meniuri laterale (`MainForm`) cu re-randare dinamică a controalelor.

### 2. Module Implementate
* **Gestiune Produse (`ProductsControl` & `ProductForm`)**:
  * Afișare tabelară reală direct din baza de date prin API.
  * Căutare în timp real și filtrare.
  * Adăugare/Editare prin pop-up modal cu limite stricte de input (ex: limitare la 20 caractere pentru nume).
  * Ștergere produs cu confirmare promptă.
* **Gestiune Comenzi (`OrdersControl`)**:
  * Vizualizare comenzi preluate din baza de date.
* **Rapoarte & Simulări (`ReportsControl`)**:
  * Grafic de evoluție a vânzărilor desenat prin **GDI+** cu diferențiere vizuală între *Istoric Real* și *Simulare Proiecție*.
  * Carduri KPI și indicatori de continuitate stoc.
* **Setări (`SettingsControl`)**:
  * Modificare URL server API.

### 3. Modul Comunicare HTTP (`ApiService.cs`)
* Client HTTP static centralizat cu metode asincrone (`async/await`).
* Suport pentru autorizare prin **JWT Bearer Token** (`SetJwtToken`).
* Returnează tuple de tip `(bool Success, string ErrorMessage)` pentru a afișa erori clare utilizatorului în caz de eșec HTTP.

---

## 🧪 Teste Unitare (`eCommerceAppStore.Tests`)

Suita de teste este scrisă în **xUnit** și folosește o bază de date în memorie (`EF Core In-Memory`):
* **`OrdersControllerTests`**: Testează crearea unei comenzi, scăderea stocului în mod corect din baza de date și returnarea statusului HTTP `201 Created`.

---

## 🚦 Instrucțiuni de Rulare

Pentru ca aplicația să funcționeze corect, **ambele proiecte** (API-ul și Clientul WinForms) trebuie să ruleze simultan.

### Varianta A: Din Visual Studio (Recomandat)
1. Deschide soluția `eCommerceAppStore.sln` în Visual Studio.
2. Click dreapta pe Soluție -> **Set Startup Projects...**
3. Selectează **Multiple startup projects**.
4. Setează acțiunea **Start** pentru `eCommerceAppStore.Api` și `eCommerceAppStore.WinForms`.
5. Apasă **F5** pentru a le porni pe ambele.

### Varianta B: Din Terminal (dotnet CLI)
Deschide două ferestre de terminal în folderul rădăcină al proiectului:

* **Terminal 1 (Backend API):**
  dotnet run --project eCommerceAppStore.Api
  (Serverul va porni la http://localhost:5113)

* **Terminal 2 (Frontend Desktop):**
  dotnet run --project eCommerceAppStore.WinForms
