# ShoeStore

A full-stack e-commerce application for buying shoes online, built with modern web technologies and clean architecture principles.

## 🎯 Features

- **Product Browsing** - Browse a diverse collection of shoes with filtering and sorting options
- **Shopping Cart** - Add items to cart, update quantities, and manage your shopping experience
- **User Authentication** - Secure user profiles with email-based authentication
- **Order Management** - Complete checkout process with order summary
- **Responsive Design** - Mobile-friendly interface that works on all devices
- **Search Functionality** - Find shoes quickly with the search feature

## 🛠 Tech Stack

### Frontend
- **Angular** - Modern web framework for the UI
- **TypeScript** - Type-safe JavaScript development
- **HTML/CSS** - Semantic markup and styling

### Backend
- **C#** - Backend logic and API development
- **.NET** - Enterprise-grade framework

### Database
- **SQL Server** - Data persistence

## 📦 Project Structure

```
ShoeStore/
├── frontend/          # Angular application
├── backend/           # .NET API
└── README.md
```

## 🚀 Getting Started

### Prerequisites
- Node.js 16+ and npm
- .NET SDK 8.0
- Visual Studio or VS Code

### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   ng serve --open
   ```

The application will open at `https://localhost:4200`

### Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd backend
   ```

2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

3. Run the API:
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:7200`

## 🛒 How to Use

1. **Browse Products** - Explore the shoe collection on the main page
2. **Filter & Sort** - Use filters to narrow down options or sort by price/name
3. **Add to Cart** - Click "Add to cart" to add items to your shopping cart
4. **Manage Cart** - Update quantities or remove items as needed
5. **Checkout** - Review your order summary and proceed to payment
6. **Create Profile** - Sign up or log in to save your information

## 📊 User Profile

Users can create and manage their profiles with:
- Personal information (Name, Email, Phone)
- Delivery address management
- Order history
- Saved preferences

## 🔧 Configuration

Create a `appsettings.json` file in the backend with:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your_connection_string"
  },
  "CloudinarySettings": {
    "CloudName": "your_cloud_name",
    "ApiKey": "your_api_key",
    "ApiSecret": "your_api_secret"
  },
}
```

## 📝 Development Branches

- `main` - Production-ready code
- `dev` - Development branch with latest features
