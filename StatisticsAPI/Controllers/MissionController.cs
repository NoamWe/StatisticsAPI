using System;
using Microsoft.AspNetCore.Mvc;
using StatisticsAPI.Model;
using StatisticsAPI.Storage;

namespace StatisticsAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class MissionController : ControllerBase
    {
        private readonly GeocodingAPI _geocodingApi;
        private readonly IMissionService _missionService;

        public MissionController(IMissionService missionService, GeocodingAPI geocodingApi)
        {
            _geocodingApi = geocodingApi;
            _missionService = missionService;
        }

        [HttpPost]
        [Route("/mission")]
        public IActionResult Post(Mission newMission)
        {
            try
            {
                _missionService.Insert(newMission);
                return Ok("Mission created successfully");
            }
            catch (Exception e)
            {
                // will log e.message
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("/countries-by-isolation")]
        public IActionResult GetCountryByIsolation()
        {
            try
            {
                var country = _missionService.GetCountryByIsolation();
                return Ok(country);
            }
            catch (Exception e)
            {
                // will log e.message
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("/find-closest")]
        public IActionResult PostFIndClosest(ClosestAddressInput addressInput)
        {
            try
            {
                var coordinates = _geocodingApi.GetCoordinates(addressInput.TargetLocation);

                if (coordinates is null) return NotFound("could not find given address");

                var mission = _missionService.FIndNearestMission(coordinates);
                return Ok(mission);
            }
            catch (Exception e)
            {
                // will log e.message
                return StatusCode(500);
            }
        }
    }
}