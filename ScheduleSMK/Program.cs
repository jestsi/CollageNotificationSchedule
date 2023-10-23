using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using QuickType;

public class Api
{
    public static List<Schedule> GetDataFromApi(string Group)
    {
        var web = new WebClient();
        var httpClient = new HttpClient();
        var request = 
            new HttpRequestMessage(HttpMethod.Get, 
                $"https://api.stavmk.ru/widget/schedule/get?group={Uri.EscapeDataString(Group.ToUpper())}");
        var productValue = new ProductInfoHeaderValue("ScraperBot", "1.0");
        var commentValue = new ProductInfoHeaderValue("(+http://www.API.com/ScraperBot.html)");
        
        request.Headers.UserAgent.Add(productValue);
        request.Headers.UserAgent.Add(commentValue);

        var resp = httpClient.SendAsync(request).Result;
        resp.EnsureSuccessStatusCode();
        var data = resp.Content.ReadAsStringAsync().Result;
        return Schedule.FromJson(data);
    }
}

internal class Program
{
    
    private static void Main(string[] args)
    {
        Task.Run(async () =>
        {
            var x = Api.GetDataFromApi("КИС-2229");
        });
    }
}
