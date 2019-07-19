# Text To Speech API

[![Build Status](https://dev.azure.com/aloji/Aloji/_apis/build/status/aloji.TextToSpeech?branchName=master)](https://dev.azure.com/aloji/Aloji/_build/latest?definitionId=5&branchName=master)


API to generate audio file from text using the Microsoft Text to Speech technology and Azure Storage to save the files.

Use of acronyms and regular expressions to make the speak more human

## Configuration

Modify the appsettings.json with your settings.

example:

```json
"AllowedHosts": "*",
"CognitiveApiKey": "64c5...",
"CognitiveTokenUrl": "https://northeurope.api.cognitive.microsoft.com/sts/v1.0/issuetoken",
"CognitiveUrl": "https://northeurope.tts.speech.microsoft.com/cognitiveservices/v1",
"User-Agent": "aloji",
"StorageOptions": {
  "ContainerName": "texttospeech",
  "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=..."
},
"RegexOptions": [
  {
    "Pattern": "(\\d+)\\.(\\d{2})(\\d{2})?",
    "Replacement": "$1 point $2 $3"
  }
],
"AcronymsOptions": [
  {
    "Key": "/",
    "Value": " "
  },
  {
    "Key": "EUR",
    "Value": "euro"
  },
  {
    "Key": "USD",
    "Value": "dollar"
  }
]
 ```
