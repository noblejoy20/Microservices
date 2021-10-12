using System;
using System.Collections.Generic;
using System.Linq;
using CommandService.Models;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context=context;
        }
        public void CreateCommand(int platformId, Command command)
        {
            if(command==null){
                throw new ArgumentNullException(nameof(command));               
            }
            command.PlatformId=platformId;
            _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform plat)
        {
            if(plat==null){
                throw new ArgumentNullException(nameof(plat));
            }
            if(!_context.Platforms.Any(x=>x.Id==plat.Id)){
                _context.Platforms.Add(plat);
            }
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _context.Commands.FirstOrDefault(x=>x.PlatformId==platformId && x.Id==commandId);
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands.Where(x=>x.PlatformId==platformId).OrderBy(c=>c.Platform.Name);
        }

        public bool PlatformExists(int platformId)
        {
            return _context.Platforms.Any(p=>p.Id==platformId);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges()>0;
        }
    }
}