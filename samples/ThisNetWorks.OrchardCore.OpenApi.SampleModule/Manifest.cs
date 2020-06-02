using OrchardCore.Modules.Manifest;

[assembly: Module(
    Id = "ThisNetWorks.OrchardCore.OpenApi.SampleModule",
    Name = "OpenAPI Sample Module",
    Author = "ThisNetWorks",
    Website = "https://github.com/ThisNetWorks",
    Version = "1.0.0",
    Description = "OpenAPI Sample Module",
    Dependencies = new[] { "OrchardCore.ContentTypes" },
    Category = "Content Management"
)]
