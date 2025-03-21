﻿using backend.Context;
using backend.Model;
using backend.Repository;

namespace backend.Usecase.Products {
    public class ResetProductTablesUsecase : UsecaseBase<object, object> {

        public ResetProductTablesUsecase(ILogger<object> logger, ApiDbContext ctx, object input) : base(logger, ctx, input) {
        }

        public override async Task<OpResponse<object>> Run() {
            try {
                await new ProductRepository(_context).Reset();
                return new OpResponse<object> {
                    Status = 200,
                    Message = "Tabelas de produtos resetadas com sucesso!"
                };
            } catch(Exception ex) {
                return Utils.Responses.DefaultInternalServerError<object>();
            }
        }
    }
}
