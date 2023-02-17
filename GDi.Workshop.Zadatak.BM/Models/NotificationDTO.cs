namespace GDi.Workshop.Zadatak.BM.Models
{
    public record NotificationModel(
        long Id,
        DateTime Date,
        long Value,
        long SensorId);
}
