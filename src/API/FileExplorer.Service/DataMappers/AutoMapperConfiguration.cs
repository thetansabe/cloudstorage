using AutoMapper;
using FileExplorer.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Service
{
    public class AutoMapperConfiguration
    {
        public static IMapper Configure()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<FileEntryDTO, FileModel>();
            });

            return mapperConfig.CreateMapper();
        }
    }
}
