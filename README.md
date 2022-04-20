# the ELEMENTs Environment
The ELEMENTs Environment is a collection of controls and user interfaces as well as a test application. 
The library implements the CSS classes of Bootstrap and Tabler.IO. Only the CSS and JS links have to be added to the HTML file.

![ELEMENTS](https://raw.githubusercontent.com/ELEMENTs-JP/the-ELEMENTs-Environment/master/ELEMENTs.Environment/Screenshots/ELEMENTS_TEASER.png)

## the Goal
The goal of this environment, this framework, this initial control library is to provide a comprehensive framework. With this framework, business applications for the web based on Microsoft Razor / Blazor technology and with Bootstrap and Tabler.IO should be buildable as fast as possible.
## Components
The framework consists (roughly) of the following components:
- Controls (e.g. Input, Date, etc.)
- User interfaces / Components (login screen, etc.)
- Repositories
- Data framework (e.g. API with Entity Framework for SQLite, SQL Server, etc.)

### Controls
We start with a set of standard controls. Among them, for example, are the following:
- TextBox, TextArea, IntegerBox, DecimalBox, MoneyBox, etc.
- DateBox, TimeBox, DateTimeBox, etc.
- FileInputBox, FileDragDropBox, etc.
- Badge, Display, Heading, Label, Lead, etc.
- Details Summary, Waiting Progress, Progress Bar, etc.

### Components
### Repositories
We use repositories to establish a connection between the data framework and the user interfaces / controls. We use the "Dependency Injection" pattern to implement customizable and reusable access code and business logic using interfaces. You can adapt this code to your needs at any time.

### ELEMENTs GO - the integrated data framework
The framework additionally contains an integrated data framework with which database applications can be developed in a fast way.

![ELEMENTS GO Data Framework](https://raw.githubusercontent.com/ELEMENTs-JP/the-ELEMENTs-Environment/master/ELEMENTs.Environment/Screenshots/ELEMENTS_GO.png)

## Test environment
The test environment is a standard Microsoft Blazor Server application. It has been or will be adapted and extended over time with the controls and userob to show how the controls and user interfaces can be used.

## Bootstrap
To make the controls and user interfaces work visually with Bootstrap, you need to include the Bootstrap CSS libraries in the HTML files. We used the following links in the test environment and created the screenshots based on them.
[Bootstrap js.delivr Documentation](https://getbootstrap.com/docs/5.1/getting-started/introduction/)

![Bootstrap Controls](https://raw.githubusercontent.com/ELEMENTs-JP/the-ELEMENTs-Environment/master/ELEMENTs.Environment/Screenshots/Bootstrap%20Controls.png)

## Tabler.IO
To make the controls and user interfaces work visually with Tabler.IO, you need to include the Tabler.IO libraries / script files in the HTML files. We used the following links in the test environment and created the screenshots based on them. Don't forget to note the @ symbol twice in the HTML code/link so that the Blazor engine doesn't throw errors.
[Tabler Documentation](https://preview.tabler.io/docs/download.html)

![Tabler.IO Controls](https://raw.githubusercontent.com/ELEMENTs-JP/the-ELEMENTs-Environment/master/ELEMENTs.Environment/Screenshots/Tabler%20IO%20Controls.png)

### Bootstrap or Tabler.IO
Our recommendation for business use is: Use the Tabler.IO libraries. The spacing between the user interfaces is slightly smaller, and we believe this is more suitable for business applications. If you are more interested in building a website with the framework, you should use the Bootstrap framework. The spacing here is a bit larger and this is a bit easier for the user to handle. Furthermore, it is the case that the support of all functions and styles between Tabler.IO and Bootstrap cannot be guaranteed 100%. However, it is also the case that Tabler.IO offers a tiny bit more business styles than Bootstrap. We would like to leave the decision to you for which framework you choose.

## Fontawesome for Icons
For the display of icons, as for example in the "MoneyBox" we use Fontawesome in version 5. [Fontawesome](https://fontawesome.com/v5/search) Here we use exclusively the free assortment. You are not forced to use Fontawesome. Each control has a separate "string" property where you can insert an HTML icon. If you insert a HTML string here, this HTML string will be used instead of the Fontawesome icon.

### Change CSS classes
You can change the CSS classes of each control (i.e. the Bootstrap or Tabler.IO CSS classes) of each control. Each control implements different CSS class properties (e.g. ControlCSS, OuterCSS, InnerCSS, FrameCSS, etc.). This allows you to enrich the controls according to your wishes, e.g. with additional classes or change the size of an input box.

## Nuget Library
We have made the entire control library available for free on nuget. So you can easily integrate ELEMENTs controls into your Visual Studio solution at any time.
[Here you can find the Nuget Package for Visual Studio and .NET-Core 5](https://www.nuget.org/packages/ELEMENTS.Controls/)

## Use of external technologies
This library uses external technologies such as Microsoft ASP.NET Core 5.0, Microsoft Razor / Blazor, Bootstrap, Tabler.IO, Fontawesome, JavaScript or Chart.JS to provide controls, components and user interfaces to accelerate the development of mobile and web-based business applications. The dependent libraries are listed and would need to be integrated separately into the _Host.cshtml.

## Libraries and technologies used
- [Bootstrap](https://getbootstrap.com/)
- [Tabler.IO](https://preview.tabler.io/)
- [Fontawesome Version 5](https://fontawesome.com/v5/search?m=free)
- [JavaScript](https://www.w3schools.com/js/)
- [Microsoft Blazor / Razor / ASP.NET Core 5.0](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0)
- [DropZone.JS](https://www.dropzone.dev/js/)
- Microsoft Entity Framework Core 5.13
- Microsoft Entity Framework Core for SQLite 5.13
- C#

## Notes and known problems
- The Waiting Progress Control works only with the Tabler.IO framework.
- The Fontawesome Library is required to display icons. But you can also use other libraries.
- To use the FileUpload functions, the IFileUploadService interface must be registered as a central service in Startup.cs.
- services.AddSingleton<IFileDragDropService, FileDragDropUploadService>();
- services.AddSingleton<IFileUploadService, FileUploadService>();