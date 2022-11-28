﻿using AutoMapper;
using Localisation.API.Data;
using Localisation.API.Dtos;
using Localisation.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Localisation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomRepo _roomRepo;
        private readonly IBuildingRepo _buildingRepo;
        private readonly IMapper _mapper;

        public RoomsController(IRoomRepo roomRepo,IBuildingRepo buildingRepo, IMapper mapper)
        {
            _roomRepo = roomRepo;
            _buildingRepo = buildingRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _roomRepo.GetAllRooms();
            return Ok(_mapper.Map<IEnumerable<RoomReadDto>>(rooms));
        }

        [HttpGet("{id}", Name = "GetRoomById")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var room = await _roomRepo.GetRoomById(id);
            if (room == null)
                return NotFound();
            return Ok(_mapper.Map<RoomReadDto>(room));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody]RoomCreateDto room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var building = await _buildingRepo.GetBuildingById(room.BuildingId);

            if (building == null)
                return NotFound($"There is no building with ID: {room.BuildingId}");
            if(!await FloorInRange(building, room.Floor))
                return BadRequest("Floor is out of building floors range.");

            var roomModel = _mapper.Map<Room>(room);
            await _roomRepo.CreateRoom(roomModel);
            var roomReadDto = _mapper.Map<RoomReadDto>(roomModel);
            return CreatedAtRoute(nameof(GetRoomById), new { Id = roomReadDto.Id }, roomReadDto);
        }






        public Task<bool> FloorInRange(Building building,  int floor)
        {
            if (floor >= building.MinFloor && floor <= building.MaxFloor)
            {
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }
    }
}
