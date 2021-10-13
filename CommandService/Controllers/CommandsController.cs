using System.Collections.Generic;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController:ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            System.Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");
            if(!_repository.PlatformExists(platformId)){
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(_repository.GetCommandsForPlatform(platformId)));
        }

        [HttpGet("{commandId}",Name ="GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId,int commandId){
            System.Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");
            if(!_repository.PlatformExists(platformId)){
                return NotFound();
            }
            var command = _repository.GetCommand(platformId,commandId);
            if(command is null){
                return NotFound();
            }
            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandforPlatform(int platformId,CommandCreateDto commandDto)
        {
            System.Console.WriteLine($"--> Hit CreateCommandforPlatform: {platformId}");
            if(!_repository.PlatformExists(platformId)){
                return NotFound();
            }
            var command = _mapper.Map<Command>(commandDto);

            _repository.CreateCommand(platformId, command);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform),
                new {platformId = platformId, commandId=commandReadDto.Id},commandReadDto);
        }
    }
}