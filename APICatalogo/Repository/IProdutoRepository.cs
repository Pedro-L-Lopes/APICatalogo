using APICatalogo.Models;
using APICatalogo.Pagination;
using System.Collections.Generic;

namespace ApiCatalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters);
        IEnumerable<Produto> GetProdutoPorPreco();  
    }
}
