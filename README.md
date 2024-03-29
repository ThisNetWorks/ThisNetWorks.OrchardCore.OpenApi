# ThisNetWorks.OrchardCore.OpenApi
Orchard Core OpenAPI Code Generation Tools based on [NSwag](https://github.com/RicoSuter/NSwag)

Provides a dynamic OpenAPI definition for Orchard Core Content Types and REST API Controllers.

This project is heavily based on [NSwag](https://github.com/RicoSuter/NSwag) and provides a wrapper and dynamic Content Type
functionality for Orchard Core.

## Features

- Dynamic Content Type JSON Schema generation.
- Client tool generation for Content Type Dto models.
- Client tool generation for Orchard Core REST API Controllers.
- Swagger UI for Orchard Core.

## Getting Started

Install the [ThisNetWorks.OrchardCore.OpenApi](https://www.nuget.org/packages/ThisNetWorks.OrchardCore.OpenApi) module to your Orchard Core Host project.

Enable the `Orchard Core OpenAPI feature`.

Refer the sample projects for example NSwag client.nswag and models.nswag configurations.

Install [NSwag Studio](https://github.com/RicoSuter/NSwag/releases).

Generate your API Client.

Install the [ThisNetWorks.OrchardCore.OpenApi.Abstractions](https://www.nuget.org/packages/ThisNetWorks.OrchardCore.OpenApi.Abstractions) package for access to base classes
and extension methods, such as 
- `contentItem.ToDto<BlogPostItemDto>()`
- `contentItem.FromDto(dto)`

to convert back and forth between dto classes and content items.

This is useful if you are using these generated models within the context of an Orchard Core project
to create your own REST API Controllers.

## Samples

There are two sample projects included
- [Orchard Core Sample](https://github.com/ThisNetWorks/ThisNetWorks.OrchardCore.OpenApi/blob/master/samples/ThisNetWorks.OrchardCore.OpenApi.Sample)
  - Install using the Open Api recipe.
  - Includes example REST API Controller (/foo).
- [Console Client Sample](https://github.com/ThisNetWorks/ThisNetWorks.OrchardCore.OpenApi/blob/master/samples/ThisNetWorks.OrchardCore.OpenApi.ConsoleClient)
  - Console Client which connects to the `Orchard Core Sample` and mutates content items with the Content API Controller


## Versions

Version tags and pre release suffixes are based of the version of Orchard Core referenced.

For version 1.0 of Orchard Core use `1.0.0`, which will use the [Orchard Core NuGet Feed](https://www.nuget.org/packages/OrchardCore/).

Prerelease versions are suffixed with the CloudSmith build of Orchard Core referenced, 
and will required a configured CloudSmith NuGet feed. Refer [Configuring a preview package source](https://docs.orchardcore.net/en/latest/docs/getting-started/preview-package-source/)

e.g. `1.1.0-preview-16439` refers to the CloudSmith Orchard Core prerelease build `v1.1.0-preview-16439`

  
