using FileExplorer.DataModel;
using FileExplorer.Service;
using Microsoft.AspNetCore.Mvc;

namespace FileExplorer.API
{
    public class BaseController<TEntity>: ControllerBase where TEntity : class
    {
        private readonly IService<TEntity> _baseService;

        public BaseController(IService<TEntity> service)
        {
            _baseService = service;
        }

    }
}
