# ThisNetWorks.OrchardCore.OpenApi
Orchard Core Open Api Code Generation Tools based on [NSwag](https://github.com/RicoSuter/NSwag)

Provides a dynamic Open Api definition for Orchard Core Content Types and REST API Controllers.

This project is heavily based on [NSwag](https://github.com/RicoSuter/NSwag) and provides a wrapper and dynamic Content Type
functionality for Orchard Core.

## Features

- Dynamic Content Type JSON Schema generation.
- Client tool generation for Content Type Dto models.
- Client tool generation for Orchard Core REST API Controllers.
- Swagger UI for Orchard Core.

## Getting Started

Install the [ThisNetWorks.OrchardCore.OpenApi] module.

Refer the sample projects for example NSwag client.nswag and models.nswag configurations.

Install [NSwag Studio](https://github.com/RicoSuter/NSwag/releases).

Generate your API Client.

Install the [ThisNetWorks.OrchardCore.OpenApi.Abstractions] package for access to base classes
and extension methods, such as 
- `contentItem.ToDto<BlogPostItemDto>()`
- `contentItem.FromDto(dto)`

to convert back and forth between dto classes and content items.

This is useful if you are using these generated models within the context of an Orchard Core project
to create your own REST API Controllers.

## Samples

There are two sample projects included
- [Orchard Core Sample](https://github.com/ThisNetWorks/ThisNetWorks.OrchardCore.OpenApi/blob/master/src/samples/ThisNetWorks.OrchardCore.OpenApi.Sample)
  - Install using the Open Api recipe.
  - Includes example REST API Controller (/foo).
- [Console Client Sample](https://github.com/ThisNetWorks/ThisNetWorks.OrchardCore.OpenApi/blob/master/src/samples/ThisNetWorks.OrchardCore.OpenApi.ConsoleClient)
  - Console Client which connects to the `Orchard Core Sample` and mutates content items with the Content API Controller
  
