<a id="readme-top"></a>
<div align="center">
  <h1 align="center">Personal Finance Console App</h1>
  
  <p align="center">
    A C# learning project by Jeffrey Jordan.
  </p>
</div>

## Table of Contents
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

## About The Project

The **Personal Finance Console App** is a simple finance management application built in C#. It enables users to track and categorize income and expenses, providing a basic overview of their personal finances. The project persists data using JSON files, making it useful for learning file I/O and basic data persistence in C#.

### Features:
- Add, edit, and remove transactions (income and expenses).
- Categorize transactions (e.g., Food, Rent, Entertainment).
- User can modify categories during runtime.
- Data persistence using local JSON files.
- Reports for monthly and yearly spending.

### Why This Project?
- Reinforce C# fundamentals like OOP, collections, and LINQ.
- Explore CRUD operations and file handling.
- Build a simple but practical application to practice coding skills.

Of course, this project is in no means complex, nor could it be used as a viable finance tracking solution in the real world.
However, this was never the intention of the project. Instead, it has served a great purpose of supporting my learning of C#. 
<br />
You may also suggest changes by forking this repo and creating a pull request or opening an issue.

### Built With
- **C#**
- **.NET Core**
- **JSON for Data Persistence**





## Getting Started
To get a local copy up and running follow these simple example steps.

### Prerequisites
- Install .NET SDK (version 6.0 or later):
  ```sh
  https://dotnet.microsoft.com/en-us/download
  ```
### Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/jeffjordan97/PersonalFinanceConsoleApp.git
   ```
2. Build the project using the .NET CLI:
   ```sh
   dotnet build
   ```
3. Run the application:
   ```js
   dotnet run
   ```



<!-- USAGE EXAMPLES -->
## Usage
This app allows users to manage their personal finances by adding, modifying, and deleting transactions, as well as categorizing them.

**Basic Commands**
- Add Transaction: Input description, amount, type (income or expense), and category.
- Modify/Delete Transaction: Select existing transactions by ID and edit/delete them.
- Add Category: Add a new category for transactions.
- View Reports: View categorized spending for a particular month or year.




<!-- ROADMAP -->
## Roadmap

- [x] Add transaction and category management.
- [x] Implement data persistence using JSON.
- [x] Add filtering by date range.
- [ ] Add graphical reports.
- [ ] Implement unit testing for transaction logic.




<!-- CONTRIBUTING -->
## Contributing
If you want to contribute to this project, you can:

1. Fork the repository.
2. Create a new feature branch (git checkout -b feature/NewFeature).
3. Commit your changes (git commit -m 'Add new feature').
4. Push to the branch (git push origin feature/NewFeature).
5. Open a Pull Request.




<!-- CONTACT -->
## Contact

X - [@JeffreyJordan97](https://x.com/JeffreyJordan97) <br />
LinkedIn - [@Jeffrey-Jordan1997](https://www.linkedin.com/in/jeffrey-jordan1997/)

Project Link: [https://github.com/jeffjordan97/PersonalFinanceConsoleApp](https://github.com/jeffjordan97/PersonalFinanceConsoleApp)




<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

Resources and tools used during the development of this project:
- [Microsoft C# Documentation](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [JSON.NET](https://www.newtonsoft.com/json)
