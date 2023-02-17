using GDi.Workshop.Zadatak.Core.Entities;

namespace GDi.Workshop.Zadatak.BM.Models
{
    public record GetSensorsResponse(
        List<SensorModel> Sensors);
    public record SensorModel(
        long Id,
        string SerialNumber,
        long Value,
        long SensorTypeId);
}
