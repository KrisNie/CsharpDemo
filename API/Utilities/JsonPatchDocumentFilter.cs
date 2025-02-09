using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Utilities;

public class JsonPatchDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // Handle schemas
        var keysToRemove = swaggerDoc.Components.Schemas
            .Where(
                s => s.Key.StartsWith("SystemTextJsonPatch", StringComparison.OrdinalIgnoreCase) ||
                     s.Key.Equals("Void", StringComparison.OrdinalIgnoreCase))
            .Select(s => s.Key)
            .ToList();

        foreach (var key in keysToRemove) swaggerDoc.Components.Schemas.Remove(key);

        swaggerDoc.Components.Schemas.Add(
            "JsonPatchDocument",
            new OpenApiSchema
            {
                Type = "object",
                Description =
                    "Describes a single operation in a JSON Patch document. Includes the operation type, the target property path, and the value to be used.",
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    {
                        "op", new OpenApiSchema
                        {
                            Type = "string",
                            Description =
                                "The operation type. Allowed values: 'add', 'remove', 'replace', 'move', 'copy', 'test'.",
                        }
                    },
                    {
                        "path", new OpenApiSchema
                        {
                            Type = "string",
                            Description =
                                "The JSON Pointer path to the property in the target document where the operation is to be applied.",
                        }
                    },
                    {
                        "value", new OpenApiSchema
                        {
                            Type = "string",
                            Description =
                                "The value to apply for 'add', 'replace', or 'test' operations. Not required for 'remove', 'move', or 'copy'.",
                        }
                    },
                },
            });

        // Handle paths
        foreach (var path in swaggerDoc.Paths)
            if (path.Value.Operations.TryGetValue(OperationType.Patch, out var patchOperation) &&
                patchOperation.RequestBody != null)
            {
                foreach (var key in patchOperation.RequestBody.Content.Keys)
                    patchOperation.RequestBody.Content.Remove(key);

                if (patchOperation.OperationId.StartsWith(
                        "odata",
                        StringComparison.OrdinalIgnoreCase))
                    path.Value.Operations.Remove(OperationType.Patch);

                patchOperation.RequestBody.Content.Add(
                    "application/json-patch+json",
                    new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Reference = new OpenApiReference
                                { Type = ReferenceType.Schema, Id = "JsonPatchDocument" },
                        },
                    });
            }
    }
}