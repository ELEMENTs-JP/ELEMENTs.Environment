# the ELEMENTs Environment
The ELEMENTs Environment is a collection of controls and user interfaces as well as a test application. 
The library implements the CSS classes of Bootstrap and Tabler.IO. Only the CSS and JS links have to be added to the HTML file.

At the moment the Controls Library contains **90 different Microsoft Razor controls, components, page layouts and charts** that you can use in your projects. We are still working on providing you with more controls, components and user interfaces as well as an improved data framework.

![ELEMENTS](https://raw.githubusercontent.com/ELEMENTs-JP/the-ELEMENTs-Environment/master/ELEMENTs.Environment/Screenshots/ELEMENTS_TEASER.png)

## the Goal
The goal of this environment, this framework, this initial control library is to provide a comprehensive framework. With this framework, business applications for the web based on Microsoft Razor / Blazor technology and with Bootstrap and Tabler.IO should be buildable as fast as possible.
## Components
The framework consists (roughly) of the following components:
- Controls (e.g. Input, Date, etc.)
- User interfaces / Components (login screen, etc.)
- Repositories
- Data framework (e.g. API with Entity Framework for SQLite, SQL Server, etc.)

## Controls
We start with a set of standard controls. Among them, for example, are the following:
- TextBox, TextArea, IntegerBox, DecimalBox, MoneyBox, etc.
- DateBox, TimeBox, DateTimeBox, etc.
- FileInputBox, FileDragDropBox, etc.
- Badge, Display, Heading, Label, Lead, etc.
- Details Summary, Waiting Progress, Progress Bar, etc.
- Button, Toggle-Button, etc.
- Items List, List Items, etc.
- Template based Items Collection, etc.
- List (a simple list), Bullet Point List, Ordered List, etc.
- KPI List, KPI Item, KPI Panel, KPI Trend Indicator, etc.
- Hero Image, Avatar Image, Audio Player, Video Player, etc.
- Card, Control Container, Component Container, etc.
- Search Box, Search Filter, Search Result List, Search Drop Down, etc.
- Tab Control with TabItem, Accordion with Items, etc.
- Sidebar with 6 different orientations, etc.

## Charts
ChartJS has also found its way into the control library. We implement a set of Chart JS charts that you can use in your applications. However, we do not deliver the Chart JS library with it. You can get the [Chart JS Library](https://www.chartjs.org/) here. Here we have made a simple standard implementation with the Razor functionalities. You are welcome to customize these functionalities to your needs. Currently we support the following ChartJS charts as pre-implemented standard charts:
- Line Chart, Area Chart, Stepline Chart, etc.
- Vertical Bar Chart, Horizontal Bar Chart, etc.
- Doughnut Chart, Pie Chart, etc.
- Polar Area Chart, Radar Chart, etc.

![Analytics](https://raw.githubusercontent.com/ELEMENTs-JP/the-ELEMENTs-Environment/master/ELEMENTs.Environment/Screenshots/Analytics_Controls.png)

For the implementation of the ChartJS charts we used the following link to the script:
```
<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
```

## Edit UI
With the "Edit Interface" we want to create possibilities to quickly and easily generate user interfaces with which data sets can be edited. Maybe you know this. You have to bind every single control, textbox, textarea, etc. individually to the repository and apply conversions or any checks each time. With the "Edit Interface" we would like to introduce a simpler and much faster methodology here. We have learned a lot about data binding from past projects and how to implement this in Blazor. Currently, the "Edit Interface" is still in a development stage. But we will continue to work on it and are sure that within a short time a few repositories and different controls will be usable.

### What is included and what is not included?
The big goal is of course the one "button" with which everything is possible. But you know yourself that this is relatively difficult. Basically, let's start with text boxes, headings and descriptions for individual text fields. Then we go on. The real goal is that we manage to minimize the workload for developers by working with json declaration files but also different templates.

![Edit_UI_Interface](https://raw.githubusercontent.com/ELEMENTs-JP/the-ELEMENTs-Environment/master/ELEMENTs.Environment/Screenshots/Edit_Interface.png)

## Drag & drop user interfaces
With the drag & drop user interfaces you can develop your own applications like Backlog or Kanban tools. We use the external library Sortable.JS for the drag & drop user interfaces. We have made good experiences with it. However, you have to integrate the external library yourself. Of course you can also look at the code and map the corresponding functionalities. At the beginning you will not see so many features. Maybe there are still some bugs in it. Please report them to us and tell us where you found a bug. We basically start with a simple backlog and a Kanban drag & drop user interface. But we will extend these user interfaces significantly over time and also attach repositories. This will give you the possibility to use the user interfaces with the ELEMENTs GO data framework.

Currently the following drag & drop user interfaces are implemented:
- Backlog, Kanban, etc.
- Generic Board with priorization, etc.

![Backlog_Image](https://raw.githubusercontent.com/ELEMENTs-JP/the-ELEMENTs-Environment/master/ELEMENTs.Environment/Screenshots/Board_Backlog.png)

## PageLayouts
PageLayouts are used to design the user interface of a single page. Please do not confuse PageLayouts with AppLayouts. The AppLayouts design the entire application and are to be understood as MasterPage. PageLayouts design a single page by providing individual areas where individual components can be inserted. The following PageLayouts are currently available:
- Empty, App, Website, etc.
- Portfolio, Category, etc.
- PrintPreview, List, EditView, etc.
- Profile, Dashboard, etc.

![App Page PageLayout](https://raw.githubusercontent.com/ELEMENTs-JP/the-ELEMENTs-Environment/master/ELEMENTs.Environment/Screenshots/AppPagePageLayout.png)

## Components
Components are not yet finished user interfaces. In terms of size, they are somewhere between a small or larger control and a small user interface. In the vast majority of cases, a component can be used to implement a small use case. Here are a few examples:
- Security: Login, Register, Log off, Change Password, Permissions list etc.
- Security: Request Access, Request new Password, Set new password, etc.
- Search: Search Box, Search Filter, Search Scope, Search Result List, etc.
- Help: Help Search Box, Help Scope, Help Result Items, Help View, etc.
- Website: Teaser (left), Teaser (right), etc.
- Item: Add Item, etc.

## Repositories
We use repositories to establish a connection between the data framework and the user interfaces / controls. We use the "Dependency Injection" pattern to implement customizable and reusable access code and business logic using interfaces. You can adapt this code to your needs at any time.
- Terms of, etc.
- Navigation, etc.
- Add Item Repository, Items (Load / Search) Repository, etc.

## ELEMENTs GO - the integrated data framework
The framework additionally contains an integrated data framework with which database applications can be developed in a fast way.

![ELEMENTS GO Data Framework](https://raw.githubusercontent.com/ELEMENTs-JP/the-ELEMENTs-Environment/master/ELEMENTs.Environment/Screenshots/ELEMENTS_GO.png)

[ELEMENTs GO Data Framework on nuget](https://www.nuget.org/packages/ELEMENTS.GO.SQLite/)

> Nuget Command: ```Install-Package ELEMENTS.GO.SQLite```

The wiki will show you how to work with the Data Framework.
[Data Framework Wiki](https://github.com/ELEMENTs-JP/the-ELEMENTs-Environment/wiki/the-ELEMENTs-Data-Framework)

## Components of ELEMENTs GO
ELEMENTs GO is an interface based database framework. In the version available here the database SQLite is implemented as an example. You can perform the following operations:
- Delete database
- Create database
- Migrate database
- Query data
- Create data
- Query number of data
- Query parameterized data
- Set or delete columns / properties for a data set
- Set or delete settings for a data set

The ELEMENTs GO Data Framework for SQLite has the following dependencies:
- Entity Framework Core 5.0.13
- Entity Framework Core Design 5.0.13
- Entity Framework Core SQLite 5.0.13
- Entity Framework Core Tools 5.0.13
- ELEMENTs Infrastructure Library

## Lists and list elements
Lists and list items can be used both manually and with the Data Framework. Basically you have to work with the IDTO interface. The ListItem will not be displayed until the DTO object has been injected. The ItemsList can only be filled with IDTO interface.

## ELEMENTs Infrastructure Library
The ELEMENTs Infrastructure Library is a Razor class library with a set of helper methods, interfaces and enumerations that help to keep the ELEMENTs GO Data Framework functional. The library is required for the operation of the ELEMENTs GO Data Framework.

The Nuget Package for the Infrastructure Library can be found here: [Nuget Infrastructure Library Package](https://www.nuget.org/packages/ELEMENTS.Infrastructure/)

> Nuget Package ```Install-Package ELEMENTS.Infrastructure```

## Console Test Application
In order to understand the use of the ELEMENTs GO Data Framework or to replicate its implementation, a console test application has been added to the Solution. In the main method, all the commands that you can execute with the ELEMENTs GO Data Framework for SQLite have been implemented.

## Test environment
The test environment is a standard Microsoft Blazor Server application. It has been or will be adapted and extended over time with the controls and userob to show how the controls and user interfaces can be used.

![Environment](https://raw.githubusercontent.com/ELEMENTs-JP/the-ELEMENTs-Environment/master/ELEMENTs.Environment/Screenshots/Environment_Example_UI.png)

## Navigation
Navigation is often a difficult part within a software application. In some applications it is set manually, in some it results from different structures. In ELEMENTs we introduce a mixed mode. First of all you have 3 different navigation interfaces. A simple and mobile navigation bar on the left side. But you can also use a classic hierarchical website navigation for the header. And finally there is a hierarchical application navigation for more complex tasks in the left navigation area. You can organize all these navigation interfaces using the included "NavigationEditor". But you can also use the INavigationRepository and implement your own navigation provider or just use the NavigationEntry class for your own needs. 

![Navigation_Editor](https://raw.githubusercontent.com/ELEMENTs-JP/the-ELEMENTs-Environment/master/ELEMENTs.Environment/Screenshots/Navigation_Editor.png)

## Bootstrap
To make the controls and user interfaces work visually with Bootstrap, you need to include the Bootstrap CSS libraries in the HTML files. We used the following links in the test environment and created the screenshots based on them.
[Bootstrap js.delivr Documentation](https://getbootstrap.com/docs/5.1/getting-started/introduction/)

![Bootstrap Controls](https://raw.githubusercontent.com/ELEMENTs-JP/the-ELEMENTs-Environment/master/ELEMENTs.Environment/Screenshots/Bootstrap%20Controls.png)

For the implementation of Bootstrap we used the following reference:
``` 
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css"
              rel="stylesheet" integrity="sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3"
              crossorigin="anonymous"> 
```
## Tabler.IO
To make the controls and user interfaces work visually with Tabler.IO, you need to include the Tabler.IO libraries / script files in the HTML files. We used the following links in the test environment and created the screenshots based on them. Don't forget to note the @ symbol twice in the HTML code/link so that the Blazor engine doesn't throw errors.
[Tabler Documentation](https://preview.tabler.io/docs/download.html)

![Tabler.IO Controls](https://raw.githubusercontent.com/ELEMENTs-JP/the-ELEMENTs-Environment/master/ELEMENTs.Environment/Screenshots/Tabler%20IO%20Controls.png)

For the implementation of Tabler.IO we used the following links:
```
    // Don't forget to put @@ in quotes in the tabler URL.
    <script src="https://unpkg.com/@@tabler/core@1.0.0-beta9/dist/js/tabler.min.js"></script>
    <link rel="stylesheet" href="https://unpkg.com/@@tabler/core@1.0.0-beta9/dist/css/tabler.min.css">
```

## Bootstrap or Tabler.IO
Our recommendation for business use is: Use the Tabler.IO libraries. The spacing between the user interfaces is slightly smaller, and we believe this is more suitable for business applications. If you are more interested in building a website with the framework, you should use the Bootstrap framework. The spacing here is a bit larger and this is a bit easier for the user to handle. Furthermore, it is the case that the support of all functions and styles between Tabler.IO and Bootstrap cannot be guaranteed 100%. However, it is also the case that Tabler.IO offers a tiny bit more business styles than Bootstrap. We would like to leave the decision to you for which framework you choose.

## Fontawesome for Icons
For the display of icons, as for example in the "MoneyBox" we use Fontawesome in version 5. [Fontawesome](https://fontawesome.com/v5/search) Here we use exclusively the free assortment. You are not forced to use Fontawesome. Each control has a separate "string" property where you can insert an HTML icon. If you insert a HTML string here, this HTML string will be used instead of the Fontawesome icon.

For the implementation of Fontawesome we used the following link:
```
 <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css"
          integrity="sha512-1ycn6IcaQQ40/MKBW2W4Rhis/DbILU74C1vSrLJxCq57o941Ym01SwNsOMqvEBFlcgUa6xLiPY/NS5R+E6ztJQ=="
          crossorigin="anonymous" referrerpolicy="no-referrer" />
```

## Sortable.JS
We have included sortable.JS via jsdelivr. Please find out the current link of sortable.JS via jsdelivr or include a sortable.JS library yourself. We have included it here at the end of _Host.cshtml with async. We did this because we don't need sortable.js right away, but only when we need a backlog or a corresponding drag and drop interface. So we can load sortable.js calmly with a small time delay. This speeds up the basic loading process of the website a bit.

For the implementation of sortable.js we used the following link:
```
  @* Sortable *@
    <script src="https://cdn.jsdelivr.net/npm/sortablejs@1.15.0/Sortable.min.js" async></script>
```


## Change CSS classes
You can change the CSS classes of each control (i.e. the Bootstrap or Tabler.IO CSS classes) of each control. Each control implements different CSS class properties (e.g. ControlCSS, OuterCSS, InnerCSS, FrameCSS, etc.). This allows you to enrich the controls according to your wishes, e.g. with additional classes or change the size of an input box.

## Nuget Library
We have made the entire control library available for free on nuget. So you can easily integrate ELEMENTs controls into your Visual Studio solution at any time.
[Here you can find the Nuget Package for Visual Studio and .NET-Core 5](https://www.nuget.org/packages/ELEMENTS.Controls/)

> Nuget Command: ```Install-Package ELEMENTS.Controls```

## Use of external technologies
This library uses external technologies such as Microsoft ASP.NET Core 5.0, Microsoft Razor / Blazor, Bootstrap, Tabler.IO, Fontawesome, JavaScript or Chart.JS to provide controls, components and user interfaces to accelerate the development of mobile and web-based business applications. The dependent libraries are listed and would need to be integrated separately into the _Host.cshtml.

## Libraries and technologies used
- [Bootstrap](https://getbootstrap.com/)
- [Tabler.IO](https://preview.tabler.io/)
- [Fontawesome Version 5](https://fontawesome.com/v5/search?m=free)
- [ChartJS](https://www.chartjs.org/)
- [JavaScript](https://www.w3schools.com/js/)
- [Microsoft Blazor / Razor / ASP.NET Core 5.0](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0)
- [DropZone.JS](https://www.dropzone.dev/js/)
- [Sortable.JS](https://sortablejs.github.io/Sortable/)
- [Microsoft Entity Framework Core 5.13](https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-5.0/breaking-changes)
- [Microsoft Entity Framework Core for SQLite 5.13](https://docs.microsoft.com/en-us/ef/core/providers/sqlite/?tabs=dotnet-core-cli)
- [SQLite](https://www.sqlite.org/index.html)
- [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)

## Imports
To help you build the _Imports.razor file faster, we have listed the most important import statements in the following code section. Of course, there are a few more in total. But with these import lines you already have a good overview.
```
@using ELEMENTs
@using ELEMENTS
@using ELEMENTS.Controls
@using ELEMENTS.Infrastructure
```

## Notes and known problems
- The Waiting Progress Control works only with the Tabler.IO framework.
- The Fontawesome Library is required to display icons. But you can also use other libraries.
- To use the FileUpload functions, the IFileUploadService interface must be registered as a central service in Startup.cs.
```
services.AddSingleton<IFileDragDropService, FileDragDropUploadService>();
services.AddSingleton<IFileUploadService, FileUploadService>();
```
- The Color Picker control only works with the Tabler.IO CSS framework.
- Placeholder provides max. 5 placeholder elements.
- Text Devider only works with Tabler.IO CSS Framework.
- Only Tabler.IO supports the "page" css classes.

## License
ELEMENTs Controls and ELEMENTs Go for SQLite is available under the MIT license.
