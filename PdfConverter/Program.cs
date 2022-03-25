using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Office.Interop.Word;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var word = new Application();
builder.Services.AddSingleton<Application>(word); 
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Lifetime.ApplicationStopping.Register(() =>
{
    word.Quit();
});

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features
        .Get<IExceptionHandlerPathFeature>()
        .Error;
    var response = new
    {
        type = exception.GetType().Name,
        message = exception.Message,
        innerException = exception.InnerException,
        stackTrace = exception.StackTrace
    };
    await context.Response.WriteAsJsonAsync(response);
}));

app.UseRouting();

app.UseCors(builder =>
    builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
);

app.UseAuthorization();

app.MapControllers();

app.Run();
