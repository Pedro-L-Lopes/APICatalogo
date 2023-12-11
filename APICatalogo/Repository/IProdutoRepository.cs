using APICatalogo.Models;
using APICatalogo.Pagination;
using System.Collections.Generic;

namespace ApiCatalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters);
        Task<IEnumerable<Produto>> GetProdutoPorPreco();  
    }
}
