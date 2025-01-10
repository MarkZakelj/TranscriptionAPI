using Microsoft.AspNetCore.Http.Features;
using TextToSpeech.Controllers;
using TextToSpeech.EndpointFilters;
using TextToSpeech.Models;
using TextToSpeech.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<FormOptions>(options => { options.MultipartBodyLengthLimit = 25_000_000; });
builder.Services.AddScoped<ITranscriptionService, WhisperTranscriptionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/transcribe", TranscriptionEndpoints.Transcribe)
    .AddEndpointFilter<ApiKeyEndpointFilter>()
    .WithName("TranscribeAudio")
    .WithDescription("Transcribes audio file uploaded as form-data with key 'audio'")
    .Accepts<IFormFile>("multipart/form-data")
    .Produces<TranscriptionResponse>(200)
    .Produces<ErrorMessage>(400)
    .Produces(401);

await app.RunAsync("http://localhost:5072");