using AutoMapper.Internal;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace InventariumAPI.Middleware;

public class FallibleResponse: FallibleResponse<object>;
public class FallibleResponse<T>
{
    public bool Success { get; set; }

    public T? Data { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Error { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Detail { get; set; }
    [DefaultValue(200)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int StatusCode { get; set; } = 0;

    public async Task WriteAsResponse(HttpContext context)
    {
        JsonObject respData = new([new("success", Success)]);
        var statusCode = StatusCodes.Status200OK;

        var resp = context.Response;
        resp.ContentType = "application/json";

        if (Success)
        {
            if (Data is not null)
                respData.Add("data", JsonSerializer.SerializeToNode(Data)
                    ?? throw new JsonException("Failed serialize data to node."));

            goto respond;
        }

        respData.Add("error", Error);
        respData.Add("detail", Detail!);
        respData.Add("statusCode", statusCode);

    respond:
        resp.StatusCode = statusCode;
        await resp.WriteAsync(
            respData.ToJsonString()
            );

    }

    public FallibleResponse(T? data)
    {
        Success = true;
        Data = data;
        Error = null;
    }

    public FallibleResponse(Exception ex)
    {
        Success = false;
        Data = default;
        Error = ex.Message;
        Detail = ex.InnerException?.Message;
        StatusCode = ex switch
        {
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    public FallibleResponse()
    {
        Success = true;
    }

    public static implicit operator FallibleResponse<T>(T? response)
        => new(response);

}


public class ErrorHandling(RequestDelegate next, ILogger<ErrorHandling> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (Exception e)
        {
            var fallible = new FallibleResponse<object>(e);

            logger.LogInformation(e, "Exception thrown in Endpoint, sending back simplified error info.");
            context.Response.StatusCode = fallible.StatusCode;
            await fallible.WriteAsResponse(context);

        }
    }

    

}



