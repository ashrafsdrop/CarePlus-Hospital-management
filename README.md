# CarePlus Hospital Management System

A state-of-the-art Hospital Management System built with **ASP.NET Core MVC** and **Tailwind CSS**. It features a modern, high-performance UI utilizing glassmorphism and 3D visual effects.

## Tech Stack
- **Framework:** ASP.NET Core MVC (net10.0)
- **Styling:** Tailwind CSS (via CDN for Rapid UI Prototyping)
- **Database:** Entity Framework Core (SQLite)

---

## 🚀 Absolute Beginner Setup Guide

Don't worry if you've never coded in C# before. Follow these exact steps to get the project running on your machine.

### 1. Prerequisites (For everyone)
- Go to the **[.NET 10 SDK Download Page](https://dotnet.microsoft.com/download)** and install it for your computer. This is the engine that runs the code.

### 2. Operating System Instructions

#### 🍎 For macOS Users (Mac)
1. **Open your Terminal:** Press `Command + Space`, type `Terminal`, and hit Enter.
2. **Go to a folder:** Type this and hit Enter to go to your Desktop.
   ```bash
   cd ~/Desktop
   ```
3. **Download the code:** Type this and hit Enter.
   ```bash
   git clone https://github.com/ashrafsdrop/CarePlus-Hospital-management.git
   cd CarePlus-Hospital-management
   ```
4. **Install the project files:** Type this and hit Enter. It will download everything the project needs.
   ```bash
   dotnet restore
   ```
5. **Install Database Tools:** Type this and hit Enter.
   ```bash
   dotnet tool install --global dotnet-ef
   ```
   *(Note: If Mac gives you a "command not found" error later, type exactly this and press Enter: `export PATH="$PATH:$HOME/.dotnet/tools"`)*

#### 🪟 For Windows Users
1. **Open your Terminal:** Press the `Windows Key`, type `PowerShell`, and hit Enter.
2. **Go to a folder:** Type this and hit Enter to go to your Desktop.
   ```powershell
   cd ~\Desktop
   ```
3. **Download the code:** Type this and hit Enter.
   ```powershell
   git clone https://github.com/ashrafsdrop/CarePlus-Hospital-management.git
   cd CarePlus-Hospital-management
   ```
4. **Install the project files:** Type this and hit Enter. It will download everything the project needs.
   ```powershell
   dotnet restore
   ```
5. **Install Database Tools:** Type this and hit Enter.
   ```powershell
   dotnet tool install --global dotnet-ef
   ```

---

## 🗄️ Step 3: Setting up the Database

You only need to do this the **very first time** you set up the project. We use a local SQLite file, so you don't need to install any heavy database software!

1. In your terminal (make sure you are inside the `HospitalManagementSystem` folder), type this and press Enter. It translates the C# code into database tables.
   ```bash
   dotnet ef migrations add InitialCreate
   ```

2. Next, type this and press Enter. This actually creates the `hospital.db` file on your computer.
   ```bash
   dotnet ef database update
   ```

*(Advanced: If you ever change the C# Models in the future, just run `dotnet ef migrations add UpdateName` and then `dotnet ef database update` again!)*

---

## 🏃‍♂️ Step 4: Running the Website!

You are ready to go! To start the website, type this into your terminal and press Enter:

```bash
dotnet watch
```

**What does this do?**
`dotnet watch` is an amazing tool. It turns on your server and **automatically refreshes your web browser** whenever you save a change to the code!

**How to see it:**
Look at your terminal output. It will say something like:
`Now listening on: https://localhost:7123`
Hold `Ctrl` (or `Cmd` on Mac) and click that link, or copy and paste it into Chrome/Safari.

---

## 🌍 How to Deploy to Production (For Servers)
If you want to host this website on the internet (like on Azure, AWS, or DigitalOcean) for the world to see:

1. Open your terminal in the project folder.
2. Run the publish command:
   ```bash
   dotnet publish -c Release -o ./publish
   ```
3. This will create a `publish` folder containing the optimized, compiled version of your website.
4. Upload the contents of the `publish` folder to your web host (e.g., via FTP to an IIS Server, or via a Docker container).

---

## 📁 Where is everything located?
- `Controllers/` - Where the backend logic lives.
- `Models/` - Where the Database tables are defined (e.g., `Patient.cs`).
- `Views/` - Where the HTML/CSS lives.
  - `Home/Index.cshtml` - The main 3D landing page.
  - `Patient/` - The Login, Signup, and Dashboard pages.
  - `Shared/_Layout.cshtml` - The master template with the transparent navbar.
