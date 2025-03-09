using backend.Model;
using Microsoft.IdentityModel.Tokens;

namespace backend {
    public class Utils{

        public class Responses {
            public static OpResponse<T> DefaultInternalServerError<T>(params string[] errors) {
                return new OpResponse<T> {
                    Status = 500,
                    Message = "Algo deu errado. Tente novamente.",
                    Errors = errors
                };
            }

            public static OpResponse<T> DefaultFillAllFields<T>(params string[] errors) {
                return new OpResponse<T> {
                    Status = 400,
                    Message = "Preencha todos os campos corretamente.",
                    Errors = errors
                };
            }
        }

        public static bool AllFilled(params string[] value) {
            foreach (var val in value) {
                if (string.IsNullOrWhiteSpace(val)) {
                    return false;
                }
            }
            return true;
        }

        public static int QueryOrDefaultInt(HttpContext ctx, string key, int def) {
            if(string.IsNullOrWhiteSpace(ctx.Request.Query[key])) {
                return def;
            }
            try {
                return int.Parse(ctx.Request.Query[key]);
            } catch (Exception ex) {
                return def;
            }
        }

    }
}
