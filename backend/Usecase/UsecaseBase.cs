using backend.Model;
using backend.Repository;

namespace backend.Usecase {
    public interface UsecaseBase<R,I,O> where R: IRepository {
        OpResponse<O> Run(R rep, I input);
    }
}
