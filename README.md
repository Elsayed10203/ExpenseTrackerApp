# Expense Tracker - .NET MAUI

## 🧾 Overview
The **Expense Tracker** is a cross-platform mobile application built using **.NET MAUI**.  
It helps users efficiently record, categorize, and analyze their expenses with a clean Material Design interface.  
The app demonstrates best practices in **MVVM architecture**, **dependency injection**, and **service abstraction** for real-world scalability.

---

## ✅ Features Implemented
- Add, edit, and delete expenses
- View categorized expense summaries
- Display total amount and currency formatting
- Bar and Pie chart visualization of expenses
- Category management (create/edit categories)
- Local data persistence (mock or API-backed)
- Responsive design across Android, iOS, and Windows
- Theming and Material Design UI

---

## 🏗️ Architecture
The application follows the **MVVM (Model–View–ViewModel)** pattern using **CommunityToolkit.MVVM** **Fresh MVVM**   to ensure clean separation of concerns and testability.

### Key Layers
- **UI Layer (Views & XAML Pages)**  
  Defines the visual presentation using MAUI controls and Material Design styling.

- **ViewModels (Logic Layer)**  
  Handles UI state and commands using `ObservableObject`, `RelayCommand`, and `INotifyPropertyChanged`.

- **Services Layer**  
  Contains interfaces and implementations for data access (`IHttpClientService`, `IExpenseService`), abstracting away API or mock data operations.

- **Models**  
  Defines the app’s domain entities like `Expense`, `Category`, and `User`.

---

## ⚙️ Technologies Used
- **.NET 8 / .NET MAUI**
- **CommunityToolkit.MVVM**
- **FreshMvvm** (for navigation & dependency injection)
- **System.Text.Json** (for JSON serialization)
- **CsvHelper** (for CSV export)
- **SkiaSharp / DevExpress MAUI Charts** (for chart visualizations)
- **Preferences / Mock Data Source** (for offline mode)
- **Material Design Styling**

---

## 🚀 Setup Instructions

### 1. Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio 2022 (with MAUI workload)
- Android/iOS emulator or physical device
- Git

### 2. Clone Repository
```bash
git clone https://github.com/YourUsername/ExpenseTrackerApp.git
cd ExpenseTrackerApp
