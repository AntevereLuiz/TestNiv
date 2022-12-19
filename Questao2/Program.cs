using Questao1.Extensions;
using Questao2;
using Questao2.Enums;
using System.Net.Http.Json;

public class Program
{
    const string URL_BASE = "https://jsonmock.hackerrank.com/api/football_matches";
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await GetTotalScoredGoalsAsync(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await GetTotalScoredGoalsAsync(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> GetTotalScoredGoalsAsync(string team, int year)
    {
        HttpClient clientHttp = new HttpClient();
        int goals = 0;
  
        var hostResult = await GetMatches(clientHttp, team, year, 1, CommandEnum.Host);
        goals += SumGoalsHost(hostResult);

        if (hostResult.Total_pages > 1)
            for (int j = 2; j <= hostResult.Total_pages; j++)
            {
                var hostResultPages = await GetMatches(clientHttp, team, year, j, CommandEnum.Host);
                goals += SumGoalsHost(hostResultPages);
            }

        var visitorResult = await GetMatches(clientHttp, team, year, 1, CommandEnum.Visitor);
        goals += SumGoalsVisitor(visitorResult);

        if (visitorResult.Total_pages > 1)
            for (int j = 2; j <= visitorResult.Total_pages; j++)
            {
                MatchPagined? visitorResultPages = await GetMatches(clientHttp, team, year, j, CommandEnum.Visitor);
                goals += SumGoalsVisitor(visitorResultPages);
            }

        return goals;
    }

    private static int SumGoalsHost(MatchPagined hostResultPages)
    {
        return hostResultPages.Data.Select(x => x.Team1goals.ToInt()).Sum();
    }

    private static int SumGoalsVisitor(MatchPagined visitorResultPages)
    {
        return visitorResultPages.Data.Select(x => x.Team2goals.ToInt()).Sum();
    }

    private static async Task<MatchPagined> GetMatches(HttpClient clientHttp, string team, int year, int page, CommandEnum command)
    {
        var match = await clientHttp.GetFromJsonAsync<MatchPagined>($"{URL_BASE}?year={year}&team{(int)command}={team}&page={page}");
        if (match == null)
        {
            throw new NullReferenceException("Match not found!");
        }
        return match;
    }
}