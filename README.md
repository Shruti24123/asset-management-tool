# Asset Management System

A comprehensive web-based Asset Management Application built with Blazor Server, Entity Framework Core, and Dapper.

## 🚀 Features

### 👥 Employee Management
- CRUD operations for employee records
- Employee profiles with department, email, phone, and designation
- Active/Inactive status management
- Search and filter capabilities

### 💻 Asset Management
- Complete asset lifecycle management
- Asset categorization by type, condition, and status
- Serial number tracking and warranty management
- Spare asset designation
- Asset search and filtering by multiple criteria

### 📋 Asset Assignment System
- Assign assets to employees
- Track assignment history
- Asset return management with notes
- Real-time availability status updates

### 📊 Dashboard & Reports
- Comprehensive dashboard with key metrics
- Asset distribution charts by type and status
- Recent assignment activity tracking
- Warranty expiration alerts
- CSV export functionality for all data

### 🔐 Authentication
- Secure login system with ASP.NET Identity
- Session-based authentication
- Default admin account creation

## 🛠 Technology Stack

- **Frontend & Backend**: Blazor Server (.NET 8)
- **Database**: SQLite (cross-platform compatible)
- **ORM**: Entity Framework Core (CRUD operations)
- **Query Optimization**: Dapper (dashboard reports)
- **Authentication**: ASP.NET Identity
- **UI Framework**: Bootstrap 5 with Bootstrap Icons
- **Export**: CsvHelper for data export

## 📁 Architecture

The application follows a clean layered architecture:

```
├── Models/                 # Data models (Employee, Asset, AssetAssignment)
├── Data/
│   ├── ApplicationDbContext.cs    # EF Core DbContext
│   └── Repositories/             # Data access layer
├── Services/                     # Business logic layer
├── Components/
│   ├── Pages/               # Blazor pages/components
│   └── Layout/              # Application layout components
└── Program.cs               # Application configuration
```

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Visual Studio Code or Visual Studio 2022

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd asset-management-application1
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Create the database**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the application**
   - Open your browser and navigate to `http://localhost:5119`
   - Click "Create Default Admin Account" to set up the admin user
   - Login with: `admin@assetmanagement.com` / `Admin123!`

## 📖 Usage

### First-Time Setup
1. Create the default admin account from the login page
2. Add employees through the Employees section
3. Add assets through the Assets section
4. Start assigning assets to employees

### Key Operations

#### Managing Employees
- Navigate to **Employees** to add, edit, or deactivate employee records
- Use the search function to quickly find specific employees
- View employee assignment history

#### Managing Assets
- Navigate to **Assets** to manage your asset inventory
- Filter assets by status (Available, Assigned, Under Repair, Retired)
- Track warranty expiration dates
- Mark assets as spare inventory

#### Asset Assignments
- Navigate to **Assignments** to assign available assets to employees
- Track all active assignments in real-time
- Process asset returns with optional notes

#### Reports & Analytics
- View the **Dashboard** for key metrics and insights
- Navigate to **Reports** for detailed analytics and data export
- Export data to CSV for external reporting

### Dashboard Widgets
- **Total Assets**: Complete asset inventory count
- **Available Assets**: Assets ready for assignment
- **Assigned Assets**: Currently deployed assets
- **Active Employees**: Current workforce count
- **Under Repair**: Assets requiring maintenance
- **Retired Assets**: End-of-life asset count
- **Spare Assets**: Reserve inventory count
- **Warranty Alerts**: Assets with expiring warranties

## 🔧 Configuration

### Database Connection
The application uses SQLite by default. To change to SQL Server, update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AssetManagementDB;Trusted_Connection=true"
  }
}
```

And update `Program.cs` to use SQL Server:
```csharp
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
```

### Identity Configuration
Authentication settings can be modified in `Program.cs`:
```csharp
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => 
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    // ... other options
})
```

## 📊 Database Schema

### Core Tables
- **Employees**: Employee information and status
- **Assets**: Asset inventory with specifications
- **AssetAssignments**: Assignment history and tracking
- **AspNetUsers**: Authentication and user management

### Key Relationships
- Assets ↔ AssetAssignments (One-to-Many)
- Employees ↔ AssetAssignments (One-to-Many)
- Unique constraints on Email (Employees) and SerialNumber (Assets)

## 🔄 API Endpoints

The application uses Blazor Server, so no REST API endpoints are exposed. All operations are handled through SignalR connections between client and server.

## 🚀 Deployment

### Development
```bash
dotnet run --environment Development
```

### Production
```bash
dotnet publish -c Release
# Deploy the published files to your web server
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

For support, please create an issue in the repository or contact the development team.

## 🎯 Future Enhancements

- [ ] Multi-tenant support for multiple organizations
- [ ] Advanced reporting with charts and graphs
- [ ] Mobile responsive design improvements
- [ ] Barcode/QR code scanning for assets
- [ ] Email notifications for warranty expiration
- [ ] Asset maintenance scheduling
- [ ] Advanced user roles and permissions
- [ ] Asset depreciation tracking
- [ ] Integration with external asset management systems

---

Built with ❤️ using Blazor Server and .NET 8