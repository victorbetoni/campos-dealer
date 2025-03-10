using backend.Context;
using backend.Controllers;
using backend.Model;
using backend.Repository;
using Microsoft.AspNetCore.Mvc;

namespace backend.Usecase {
    public abstract class UsecaseBase<I,O> {

        protected readonly ILogger<object> _logger;
        protected readonly ApiDbContext _context;
        protected readonly I _input;

        public UsecaseBase(ILogger<object> logger, ApiDbContext context, I input) {
            _logger = logger;
            _context = context;
            _input = input;
        }

        public abstract Task<OpResponse<O>> Run();

    }
}
