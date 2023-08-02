# Yandex.API.GeoSuggest

Simple wrapper for Yandex GeoSuggest API.
Usage:

```c#
var result=await new GeoSuggestClient(apiKeyOption).GeoSuggest(
    new GeoSuggest.GeoSuggestRequest(textOption)
    {
        SearchCenter = (36,1M,55.6M)
    });
```