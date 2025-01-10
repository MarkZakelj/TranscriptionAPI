# Speech To Text API
This API serves as a speech transcription service, converting audio signal to text and detecting spoken language at the same time.

## Installation and Configuration
- Install .NET 8.x
- Create new file `appsettings.Development.json` and copy content from `appsettings_example.Development.json`
- Add the OPENAI_API_KEY key in the `appsettings.Development.json`
- Install NuGet packages with `dotnet restore`
- Run the app
- Access the swaggerUI on `http://localhost:5072/swagger`

The API accepts WAV files and returns the recognized language and raw text.