﻿namespace Localisation.API.Dtos.MaintenanceDtos
{
    public class MaintenanceReadDto
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string? Description { get; set; }
        public int ProductId { get; set; }
    }
}