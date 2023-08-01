using Newtonsoft.Json;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Globalization;
using Yandex.API;

var apiKeyOption = new Option<string>(
    aliases: new string[] { "--apikey", "-a" }, description: "Api key")
{
    IsRequired = true,
};

var textOption = new Option<string>(
    aliases: new string[] { "--text", "-t" }, description: "Search text.")
{
    IsRequired = true,
};

var centerOption = new Option<(decimal,decimal)>(
    aliases: new string[] { "--center", "-c" }, description: "Center coordinate to start search with.", parseArgument: (arg) =>
{
    if (arg == null || !arg.Tokens.Any())
        throw new ArgumentNullException(nameof(arg));
    var token = arg.Tokens.Single().Value;
    var coords=token.Split(",");
    var lon = decimal.Parse(coords[0], CultureInfo.InvariantCulture);
    var lat = decimal.Parse(coords[1], CultureInfo.InvariantCulture);
    return (lon, lat);
})
{
    IsRequired = false,
};
RootCommand rootCommand = new(description: "GeoSuggest with Yandex.")
{
    apiKeyOption,
    textOption,
    centerOption
};
rootCommand.SetHandler(async (apiKeyOption, textOption, centerOption) =>
{
    var result=await new GeoSuggestClient(apiKeyOption).GeoSuggest(new GeoSuggest.GeoSuggestRequest(textOption)
    {
        SearchCenter = centerOption
    });
    Console.WriteLine(JsonConvert.SerializeObject(result));
}, apiKeyOption, textOption, centerOption);
try
{
    await rootCommand.InvokeAsync(args);
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}