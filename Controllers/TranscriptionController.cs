using TextToSpeech.Models;
using TextToSpeech.Services;

namespace TextToSpeech.Controllers;

public static class TranscriptionEndpoints
{
    public static async Task<IResult> Transcribe(
        HttpRequest request,
        ITranscriptionService transcriptionService)
    {
        if (!request.HasFormContentType)
            return Results.BadRequest(new ErrorMessage
            {
                Message = "Request should have a Form Content Type"
            });
        var form = await request.ReadFormAsync();
        var file = form.Files.GetFile("audio");
        if (file == null)
            return Results.BadRequest(new ErrorMessage
            {
                Message = "The file is empty"
            });
        var response = await transcriptionService.TranscribeAudioAsync(file);

        return response.Success ? Results.Ok(response) : Results.BadRequest(response);
    }
}