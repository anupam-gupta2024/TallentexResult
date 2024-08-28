# Tallentex Result .NET Core Project
### National Rank & Performance Analysis
All the students participating in TALLENTEX 2025 will get their National Rank in their respective classes. Also, a detailed analysis of overall & subject wise performance will be provided in report format
### Competitive Success Index (CSI)
All the students participating in TALLENTEX will be provided with their Competitive Success Index (CSI). Based on CSI, a student can evaluate their potential for Success in respective forthcoming higher Competitive Exams including JEE (Main), JEE (Advanced), NEET (UG), International Olympiads.

### Key Functions 
1. A detailed analysis of overall & subject wise performance.
2. A service that allows cash prize winners to upload documents in AWS.
3. OTP based Authentication
4. Showing Scholarship & Special Fee Benefit on taking admission in ALLEN's


## Getting Started
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites
- Visual Studio 2022 or higher 
- .NET 8.x SDK  
- MS SQL Server

### Installing
1. Download and copy to \My Documents\Visual Studio 2017\Templates\ProjectTemplates folder.

### Using the Template for a New Project
1. Launch Visual Studio 2019 and create a new project. Use the "ASP.NET Core Web Application" template (C#). Use a unique name for the new project.
2. In the "Create a new ASP.NET Core web application" window, select ".NET Core" and "ASP.NET Core 3.1" in the dropdown menus. Choose the "Empty" project type and Check the "Configure for HTTPS" box. Click "Create".
3. Once the project has been created, close Visual Studio.
4. Clone this repository to a new folder.
5. Remove *all* of the files and folders from within your project's web root. This includes the following:
    - *bin/*
    - *obj/*
    - *Properties/*
    - *appsettings.Development.json*
    - *appsettings.json*
    - *Program.cs*
    - *Startup.cs*
6. Copy *all* files and folders from this repository's **2nd** *ASP Core MVC Template* subdirectory and paste them into the same location within your own project. You're replacing all of the files you removed in the previous step.
7. Rename the *ASP Core MVC Template.csproj* file to your own project's name. Edit it with an app like *Notepad* and make the following changes:
    - Replace the *\<RootNamespace\>* value with your project's namespace.
8. Rename the *ASP Core MVC Template.csproj.user* file to your own project's name.
9. Rename the *ASP_CORE_MVC_Template_appsettings_Example.json* file to *\<YOURPROJECT\>_appsettings.json*.
10. Copy *\<YOURPROJECT\>_appsettings.json* to the folder you assigned to the *APPSETTINGS_DIRECTORY* environment variable (as described earlier in this readme).
11. Edit *AssemblyInfo.cs* as follows:
    - Replace the *RootNamespace* value with your project's namespace.
12. Edit *Program.cs* as follows:
    - Change *namespace ASP_Core_MVC_Template* to *namespace \<YOURPROJECTNAMESPACE\>*
    - Change *config.AddJsonFile("ASP_Core_MVC_Template_appsettings.json")* to *config.AddJsonFile("\<YOURPROJECT\>_appsettings.json")*
13. Edit *Startup.cs* as follows:
    - Change *namespace ASP_Core_MVC_Template* to *namespace \<YOURPROJECTNAMESPACE\>*
    - Change *options.Cookie.Name = ".Core-Web-Template.Session";* to *options.Cookie.Name = ".\<YOURPROJECT\>.Session";*
    - Change *loggerFactory.AddFile(logDirectory + "/Core-Web-Template-{Date}.log");* to *loggerFactory.AddFile(logDirectory + "/\<YOURPROJECT\>-{Date}.log");*
    - Change *app.UsePathBase("/core-web");* to *app.UsePathBase("/\<yourproject\>");* (make sure you **preserve all lowercase** for this change!)
14. Remove all *bin\\Debug\\netcoreapp3.1\\ASP Core MVC Template.\** files.
15. Edit *Controllers\\HomeControler.cs* as follows:
    - Change *using ASP_Core_MVC_Template.Models;* to *using \<YOURPROJECT\>.Models;*
    - Change *namespace ASP_Core_MVC_Template.Controllers* to *namespace \<YOURPROJECTNAMESPACE\>.Controllers*
    - If using *AppStatus* functionality from *CAAM*, change *_configService.GetAppClosed("CORE_TEMPLATE");* to *_configService.GetAppClosed("\<YOURPROJECT\>");*
    - If using *AppStatus* functionality from *CAAM*, change *_configService.GetAppWarning("CORE_TEMPLATE");* to *_configService.GetAppWarning("\<YOURPROJECT\>");*
    - If using roles from *CAAM*, edit the *\[Authorize(Roles = "COREADMIN,COREUSER")\]* line to use roles that are applicable to your project.
16. Edit *Controllers\\LoginControler.cs* as follows:
    - Change *namespace ASP_Core_MVC_Template.Controllers* to *namespace \<YOURPROJECTNAMESPACE\>.Controllers*
    - If using roles from *CAAM*, edit *_dataAPIService.GetCAAMRoles("Core Template"* to *_dataAPIService.GetCAAMRoles("\<YOURPROJECT\>"*
17. Edit *Models\\ErrorViewModel.cs* as follows:
    - Change *namespace ASP_Core_MVC_Template.Models* to *namespace \<YOURPROJECTNAMESPACE\>.Models*
18. If using roles from *CAAM*, edit *Views\\Home\\LoggedIn.cshtml* as follows:
    - Change *Context.User.IsInRole("COREADMIN")* to use a role applicable to your project.
19. Edit *Views\\Home\\Warning.cshtml* as follows:
    - Change *\<title\>Warning - CAAM Web Template\</title\>* to *\<title\>Warning - \<YOURPROJECT\>\</title\>*
20. Edit *Views\\Shared\\\_Layout.cshtml* as follows:
    - Change *@ViewData\["Title"\] - Core Web Template* to *@ViewData["Title"] - \<YOURPROJECT\>*
    - Change *aria-label="Home"\>Core Web Template* to *aria-label="Home"\>\<YOURPROJECT\>*
21. Edit *Views\\\_ViewImports.cshtml* as follows:
    - Change both instances of *@using ASP_Core_MVC_Template* to *@using \<YOURPROJECT\>*

### AWS CLI install and Profile Creation instructions
1. Download the AWS CLI and install (https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html)
2. To Check it install correctly open command prompt and type aws --version
    ```cmd
    $ aws --version
    aws-cli/2.17.24 Python/3.11.9 Windows/10 exe/AMD64
    ```
3. To create a profile & configure credential
  - Type the following command: aws configure --profile {profilename}
  - When prompted, enter the access key ID and secret access key for the first profile.
  - When prompted for the default region name and output format, enter the desired values for the first profile.
    ```cmd    
    $ aws configure --profile default
    AWS Access Key ID [None]: XXXXXX
    AWS Secret Access Key [None]: XXXXXX
    Default region name [None]: ap-south-1
    Default output format [None]: json
    ```
3. It will create two file 
  - C:\Users\{username}\.aws\credentials
  ```
    [default]   <-- 'profile' in front of 'profile_name' (not for default)!!
    aws_access_key_id={{aws_access_key_id}}
    aws_secret_access_key={{aws_secret_access_key}}
  ```
  - C:\Users\{username}\.aws\config
  ```
    [default]   <-- 'profile' in front of 'profile_name' (not for default)!!
    region={{region}}
    output={{output:"json||text"}}
  ```