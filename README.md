# DotNetApisForAngularProjects app 
This RESTful web service was developed for PapaJohnsClone and HomeCuisine apps
* See [PapaJohnsClone](https://github.com/Tiamatt/PapaJohnsClone) repo
* See [HomeCuisine](https://github.com/Tiamatt/HomeCuisine) repo

## Demo
Web service was deployed to SmarterAsp.Net (moved from Microsoft Azure Cloud) <br />
<br />
Example ([link](http://tiamatt.com/api/ppjc/specials)): <br />

```json
[
    {
        "id": 1,
        "name": "Create Your Own",
        "description": "25% off Regular Menu Price Online Orders",
        "itemCategoryId": 4,
        "itemCategoryName": "Specials",
        "price": 10.99
    },
    {
        "id": 3,
        "name": "Cinnamon Pull Aparts",
        "description": "Bite-sized sweet roll dough covered in cinnamon and sugar, topped with cinnamon crumbles then baked and drizzled with cream cheese icing",
        "itemCategoryId": 1,
        "itemCategoryName": "Specials",
        "price": 6.29
    },
    {
        "id": 5,
        "name": "Double Chocolate Chip Brownie",
        "description": "Filled with chocolate chips, cut into 9 squares and served warm",
        "itemCategoryId": 1,
        "itemCategoryName": "Specials",
        "price": 3.19
    }
]
```

## Features
* ASP.NET Core 2.1 Web API
* Async/Await approach
* CORS configuration

## Built With
* .NET Core 2.1 Framework
* Entity Framework Core
* C#
* Visual Studio Code

## Getting Started
Install: <br/>
1. .NET Core CDK <br/> 
2. Visual Studio Code IDE <br/> 
3. '.NET Core Extension Pack' extension <br/> 
In order to start the project:
```bash
# download the source code
$ git clone https://github.com/Tiamatt/PapaJohnsCloneApi
# open the project with Visual Studio Code
# restore packages
$ dotnet restore
# run application
$ dotnet run
# open in browser
$ https://localhost:5001/api/values
```