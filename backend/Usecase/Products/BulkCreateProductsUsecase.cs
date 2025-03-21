﻿using backend.Context;
using backend.Model;
using backend.Repository;
using System.Transactions;

namespace backend.Usecase.Products {
    public class BulkCreateProductsUsecase : UsecaseBase<List<Product>, object> {
        public BulkCreateProductsUsecase(ILogger<object> logger, ApiDbContext ctx, List<Product> input) : base(logger, ctx, input) {
        }

        public override async Task<OpResponse<object>> Run() {
                try {
                var repo = new ProductRepository(_context);
                    foreach (var p in _input) {
                        await repo.New(p, true);
                    }
                    return new OpResponse<object> {
                        Status = 200,
                        Message = "Produtos criados com sucesso!",
                        Data = null
                    };  
                } catch (Exception ex) {
                    _logger.LogError(ex, "Erro criando produtos bulk");
                    return Utils.Responses.DefaultInternalServerError<object>();
                }
        }
    }
}
