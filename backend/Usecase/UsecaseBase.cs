using backend.Controllers;
using backend.Model;
using backend.Repository;
using Microsoft.AspNetCore.Mvc;

namespace backend.Usecase {
    public abstract class UsecaseBase<R,I,O> where R: IRepository {

        protected readonly ILogger<object> _logger;
        protected readonly R _repository;
        protected readonly I _input;

        public UsecaseBase(ILogger<object> logger, R repository, I input) {
            _logger = logger;
            _repository = repository;
            _input = input;
        }

        public abstract OpResponse<O> Run();
    }
}
