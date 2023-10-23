using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using QuickType;
using TelegramEventer;

namespace ScheduleBotSide.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BotScheduleApiController : ControllerBase
{
    [HttpPost]
    [Route("add")]
    public IActionResult AddNewDailySchedule (string group, string time, string chatId)
    {
        RecurringJob.RemoveIfExists(chatId);
        
        var time2 = DateTime.Parse(time);
        RecurringJob.AddOrUpdate(chatId, 
            () => TaskAddScheduleOnTomorrow(Convert.ToInt64(chatId), group), 
            $"{time2.Minute} {time2.Hour} * * *",
            TimeZoneInfo.Local);
        return Ok();
    }

    public void TaskAddScheduleOnTomorrow(long chatId, string group)
    {
        if (DateTime.Now.DayOfWeek is DayOfWeek.Sunday)
        {
            // отправка сообщения и добавления job на следующее воскресенье
            return;
        }
        var schedule = Api.GetDataFromApi(group).First();
        BotTelegram.SendMessage(chatId, FormatSchedule(schedule));
        
    }

    private static string FormatSchedule(Schedule schedule)
    {
        var readyTable = new StringBuilder();
        readyTable.AppendLine(schedule.Date.ToString("yyyy/M/d dddd"));
        foreach (var para in schedule.List)
        {
            readyTable.AppendLine($"{para.Number} - {para.Disciplines} {para.Auditorium} {para.Types}");
            readyTable.AppendLine($"нач. {para.TimeStart} конец {para.TimeEnd}");
            readyTable.AppendLine($"учитель - {para.Teachers}\n");
            Enumerable.Range(0, readyTable.Length/2)
                .ToList()
                .ForEach(x => readyTable.Append('-'));
        }

        return readyTable.ToString();
    }
    [HttpPost]
    [Route("check")]
    public IActionResult CheckFreeSchedule (int chatId)
    {
        
        
        return Ok();
    }
}