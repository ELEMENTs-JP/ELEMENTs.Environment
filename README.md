# the ELEMENTs Environment
The ELEMENTs Environment is a collection of controls and user interfaces as well as a test application. 
The library implements the CSS classes of Bootstrap and Tabler.IO. Only the CSS and JS links have to be added to the HTML file.
## the Goal
The goal of this environment, this framework, this initial control library is to provide a comprehensive framework. With this framework, business applications for the web based on Microsoft Razor / Blazor technology and with Bootstrap and Tabler.IO should be buildable as fast as possible.
## Components
The framework consists (roughly) of the following components:
- Controls (e.g. Input, Date, etc.)
- User interfaces / Components (login screen, etc.)
- Repositories
- Data framework (e.g. API with Entity Framework for SQLite, SQL Server, etc.)

### Controls
### Components
### Repositories
### Integrated data framework
The framework additionally contains an integrated data framework with which database applications can be developed in a fast way.

## Test environment
The test environment is a standard Microsoft Blazor Server application. It has been or will be adapted and extended over time with the controls and userob to show how the controls and user interfaces can be used.

## Bootstrap
To make the controls and user interfaces work visually with Bootstrap, you need to include the Bootstrap CSS libraries in the HTML files. We used the following links in the test environment and created the screenshots based on them.
[Bootstrap js.delivr Documentation](https://getbootstrap.com/docs/5.1/getting-started/introduction/)

## Tabler.IO
To make the controls and user interfaces work visually with Tabler.IO, you need to include the Tabler.IO libraries / script files in the HTML files. We used the following links in the test environment and created the screenshots based on them. Don't forget to note the @ symbol twice in the HTML code/link so that the Blazor engine doesn't throw errors.
[Tabler Documentation](https://preview.tabler.io/docs/download.html)

## Nuget Library
We have made the entire control library available for free on nuget. So you can easily integrate ELEMENTs controls into your Visual Studio solution at any time.
[Here you can find the Nuget Package for Visual Studio and .NET-Core 5](https://www.nuget.org/packages/ELEMENTS.Controls/)
