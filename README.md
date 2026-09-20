# 🛒 eCommerceAppStore - System Documentation

`eCommerceAppStore` este o soluție software completă de tip Full-Stack destinată gestiunii magazinelor online. Proiectul combină un backend REST API robust creat în **.NET / ASP.NET Core** cu o interfață desktop modernă și intuitivă creată în **Windows Forms (WinForms)**.

---

## 📐 Arhitectura Proiectului

```text
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
```

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
* **`GET /api/products`**: Preluare produse cu căutare, filtrare (preț/stoc), sortare și paginare.
* **`GET /api/products/{id}`**: Preluare produs după ID.
* **`POST /api/products`**: Creare produs nou.
* **`PUT /api/products/{id}`**: Actualizare date produs existent.
* **`DELETE /api/products/{id}`**: Ștergere produs după ID.

#### 🛒 Comenzi Controller (`/api/orders`)
* **`GET /api/orders`**: Lista tuturor comenzilor din sistem.
* **`POST /api/orders`**: Creare comandă nouă (verificare stoc, scădere automată din stoc, calcul total).

### 3. Middleware & Tratarea Erorilor
* **`ExceptionMiddleware.cs`**: Tratează excepțiile neprevăzute și returnează răspuns standardizat JSON `500 Internal Server Error`.

---

## 🖥️ Detalii Implementare Frontend (`eCommerceAppStore.WinForms`)

### 1. Interfață & Design
* **Dark Mode Theme**: Interfață stilizată în nuanțe închise (`#1E1E2E`, `#181825`, `#28283C`).
* **Design Customizat DataGridView**: Tabele fără margini albe, header întunecat și rânduri alternante.
* **Design Responsive**: Navigare prin meniu lateral (`MainForm`).

### 2. Module Implementate
* **Gestiune Produse (`ProductsControl` & `ProductForm`)**:
  * Afișare tabelară din API, căutare în timp real și filtrare.
  * Pop-up modal pentru adăugare/editare cu limite stricte de input (max. 20 caractere).
  * Ștergere produs cu confirmare promptă.
* **Gestiune Comenzi (`OrdersControl`)**: Vizualizare comenzi preluate din baza de date.
* **Rapoarte & Simulări (`ReportsControl`)**: Grafice **GDI+** (vânzări/stoc) și carduri KPI.
* **Setări (`SettingsControl`)**: Modificare URL server API.

### 3. Modul Comunicare HTTP (`ApiService.cs`)
* Client HTTP static centralizat cu `async/await`.
* Suport pentru autorizare JWT Bearer Token (`SetJwtToken`).
* Returnează tuple de tip `(bool Success, string ErrorMessage)` pentru erori clare.

---

## 🧪 Teste Unitare (`eCommerceAppStore.Tests`)

* **`OrdersControllerTests`**: Testează crearea comenzii și scăderea stocului folosind `EF Core In-Memory`.

---

## 🚦 Instrucțiuni de Rulare

Ambele proiecte trebuie să ruleze simultan.

### Varianta A: Din Visual Studio (Recomandat)
1. Click dreapta pe Soluție -> **Set Startup Projects...**
2. Selectează **Multiple startup projects**.
3. Setează acțiunea **Start** pentru `eCommerceAppStore.Api` și `eCommerceAppStore.WinForms`.
4. Apasă **F5**.

### Varianta B: Din Terminal (dotnet CLI)

```bash
# Terminal 1 - Backend API
dotnet run --project eCommerceAppStore.Api

# Terminal 2 - Frontend WinForms
dotnet run --project eCommerceAppStore.WinForms
```
