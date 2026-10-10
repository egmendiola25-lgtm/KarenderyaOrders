# Karenderya Orders

A simple ASP.NET Core MVC application for managing food items, orders, and inventory for a karenderya.

## How to Run

1. Install the .NET 10 SDK, Visual Studio, and SQL Server.
2. Open `KarenderyaOrders.slnx` or `KarenderyaOrders.csproj` in Visual Studio.
3. Update the `DefaultConnection` connection string in `appsettings.json` with your SQL Server instance name.
4. Open the Package Manager Console, select the project containing the EF Core migrations as the default project, and run `Update-Database`.
5. Build and run the application using Visual Studio.

## Technologies
- C#
- ASP.NET Core MVC (.NET 10)
- Entity Framework Core 10
- SQL Server
- Bootstrap

## Features
- Create, view, edit, and delete food items.
- Create orders with order numbers, quantities, and prices.
- Check available inventory before accepting an order.
- Automatically deduct stock when an order is created.
- Complete or cancel orders.
- Restore stock when an eligible order is cancelled.
- View order details and totals.
- Search orders by order number.
- Filter orders by status and paginate results.
- Dashboard showing total orders, today's completed sales, and low-stock items.

## How to Run
1. Install the .NET 10 SDK and SQL Server.
2. Open the solution in Visual Studio.
3. Update the `DefaultConnection` connection string in `appsettings.json` for your SQL Server instance.
4. Open the Package Manager Console and run `Update-Database` to apply the EF Core migrations.
5. Build and run the project using Visual Studio.

## Design Decisions
- Used ASP.NET Core MVC to separate models, controllers, and views.
- Used Entity Framework Core to access SQL Server.
- Calculated order totals from the selected food item's price and quantity.
- Used stock validation to prevent orders exceeding available inventory.
- Kept the implementation straightforward for a small application.

## Testing and Checks
Manually checked the main workflows:
- Creating and viewing food items and orders.
- Deducting stock when an order is created.
- Restoring stock when an eligible order is cancelled.
- Preventing duplicate order numbers and invalid quantities.
- Searching orders, filtering by status, and navigating pages.
- Checking dashboard sales and low-stock counts.

## Assumptions and Limitations
- Each order currently contains one food item.
- Low stock means a stock quantity of 5 or less.
- Today's sales include completed orders created today.
- The application requires a configured SQL Server database.
- Automated tests have not been added.



## AI Usage
ChatGPT was used to assist with implementation, debugging, and explanations of ASP.NET Core MVC, Entity Framework Core, SQL queries, and Razor views. The generated suggestions were applied and checked through manual testing. The submitted code remains my responsibility, and I should be able to explain its important parts.
