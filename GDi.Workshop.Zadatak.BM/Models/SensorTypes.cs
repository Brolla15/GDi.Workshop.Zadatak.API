namespace GDi.Workshop.Zadatak.BM.Models
{
    public record GetSensorTypesResponse(
        List<SensorTypesModel> SensorTypes);
    public record SensorTypesModel(
        long Id,
        int FromInterval,
        int ToInterval,
        string Name);

    public record DropdownModel(
        long Value,
        string Label);
}
