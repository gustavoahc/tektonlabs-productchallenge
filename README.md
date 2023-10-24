# tektonlabs-productchallenge
Repositorio de código para prueba técnica en .NET de Tekton Labs

# Repositorio
https://github.com/gustavoahc/tektonlabs-productchallenge

# Arquitectura Cloud
![CloudArchitecture.png](/.attachments/CloudArchitecture-1eff3f17-fcae-4fce-af83-20e04d4d25b9.png)
##Flujo de ejecución
1. El usuario desde la aplicación móvil ingresa sus credenciales (email y password)
2. La Function App encargada del login recibe la solicitud y se conecta al Azure Active Directory (AAD) para intentar autenticar al usuario
3. Si el login es exitoso, AAD devuelve un token
4. La aplicación móvil obtiene el token generado por AAD
5. Para solicitar información contenida en la base de datos, la aplicación móvil debe incluir el token como un header Authorization Bearer
6. Antes de dar el paso hacia la Function App que accede a la base de datos CosmosDB, el API Management verifica que el token enviado por la aplicación móvil sea válido mediante una configuración interna
7. Si el token es válido, se permite la consulta y retorno de información de la base de datos
8. La información es visualizada en la aplicación móvil
# Arquitectura de Software (Clean Architecture)
![CleanArchitecture.png](/.attachments/CleanArchitecture-3cf5afc6-2229-43f9-a817-da397e15212a.png)

El desarrollo fue realizado en **NET 6** mediante **Azure functions** utilizando una 
base de datos **Azure Cosmos DB**
##Estructura del proyecto
La solución se encuentra dividida en 3 carpetas lógicas (Core, Infraestructura y Presentacion)

![EstructuraProyectos.png](/.attachments/EstructuraProyectos-af722ada-e223-48cd-9fc4-d182e6141e03.png)

* La carpeta **Core** se encuentra conformada por dos proyectos: **Requerimientos.Core.Aplicacion** (contiene las interfaces a ser implementadas) y **Requerimientos.Core.Dominio** (contiene las entidades)
* En la carpeta **Infraestructura** se encuentra el proyecto **Requerimientos.Infraestructura.AccesoDatos** el cual permite conectar a la base de datos mediante los repositorios. Este proyecto al ser específico en tecnologías incorpora la librería necesaria para acceder a CosmosDB. En este caso utiliza la librería **Microsoft.EntityFrameworkCore.Cosmos**
* En la carpeta **Presentacion** se encuentran los proyecto de tipo **Azure Function App**. El proyecto **Requerimientos.Functions** contiene las funcionalidades para acceder a la base de datos CosmosDB y el proyecto **Requerimientos.Login** se encarga de autenticar al usuario en AAD. En cada uno de estos dos proyectos se encuentra el archivo **local.settings.json** con la configuración de los parámetros necesarios así como los modelos de request y response utilizados para mover los datos entre front y la lógica de negocio. Al igual que en el proyecto de la capa de **Infraestructura**, estos dos proyectos contienen librerías específicas, en este caso, **librerías de Azure Functions** así como consultar los servicios de la capa de **Aplicación** por medio de las interfaces haciendo uso de la **Inyección de Dependencias**. De esta manera se desarrolla **Clean Architecture** y la capa **Core** se mantiene desacoplada de componentes externos y específicos como la base de datos, tecnología de frontend, acceso a recursos externos, etc