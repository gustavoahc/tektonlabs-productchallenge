# tektonlabs-productchallenge
Repositorio de código de REST API para prueba técnica en .NET de Tekton Labs

## Repositorio
https://github.com/gustavoahc/tektonlabs-productchallenge

## Pasos para ejecutar el proyecto
1. Tener instalado .NET 7
2. Verificar que el proyecto de inicio sea **TektonLabs.Presentation.Api**
3. Como almacenamiento de los datos de utiliza una base de datos SQL Server local. Modificar la ruta de acceso a ella en el archivo **appsettings.json** del proyecto **TektonLabs.Presentation.Api**

## Arquitectura (Clean Architecture)
![TektonLabsApi-CleanArchitecture](https://github.com/gustavoahc/tektonlabs-productchallenge/assets/82127183/e39f9fed-e9d2-4012-884e-cbf25dddb8a3)

Dentro de los patrones y buenas prácticas implementadas en el desarrollo de la prueba se aplicaron los patrones **Repository** y **Options**, así como buenas prácticas como **Dependency Injection**, **Separation of Concerns**, **Guard Clauses**, **Single Responsibility**, estándar de nombres, uso correcto de los códigos de respuesta HTTP, uso de generics y en general los principios **SOLID** para tener un código limpio. También se implementó la captura global de errores por medio de un action filter (**ExceptionFilterAttribute**)

## Estructura de proyectos

![TektonLabsProjectStructure](https://github.com/gustavoahc/tektonlabs-productchallenge/assets/82127183/1a50262d-9ecf-4c1a-80d5-b3ad6e4347ac)

* En la carpeta **Testing** se encuentra el proyecto de unit tests
* La carpeta **TektonLabs** contiene el desarrollo de la solución. Se divide en 3 subcarpetas: Core, Infrastructure y Presentation
  * **Core** contiene las entidades (proyecto **TektonLabs.Core.Domain**) y la lógica de la aplicación (proyecto **TektonLabs.Core.Application**) en donde se definen las interfaces que implementarán las otras capas
  * **Infrastructure** es para el acceso a la base de datos (proyecto **TektonLabs.Infrastructure.DataAccess**)
  * **Presentation** es el web Api (proyecto **TektonLabs.Presentation.Api**)

## Tecnologias y frameworks utilizados
* **.NET 7**
* **Entity Framework Core** para el acceso a base de datos
* **Swagger** para documentar el Api
* **LazyCache** para tener en caché la data de estados del producto
* **NLog** para guardado del archivo de logs
* **AutoMapper** para el mapeo automático entre entidades y DTOs
* **FluentValidation** para la validación del objeto request recibido
* **SQL Server** (local)
* **NUnit** y **Moq** para los unit tests

###
El api utilizado para obtener los descuentos de los productos es https://653182fb4d4c2e3f333d17ac.mockapi.io/api/v1/discounts
