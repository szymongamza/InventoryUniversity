namespace Localisation.API.Dtos
{
    public class RoomReadDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Floor { get; set; }
        public int BuildingId { get; set; }
    }
}
