using APICatalogo.Models;
using APICatalogo.Pagination;
using System.Collections;
using System.Collections.Generic;

namespace ApiCatalogo.Repository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetCategorias(CategoriasParameters categoriasParameters);
        Task<IEnumerable<Categoria>> GetCategoriasProdutos();
    }
}
