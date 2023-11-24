using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using System.Collections.Generic;
using System.Linq;

namespace ApiCatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {
            return Get()
                    .OrderBy(on => on.Nome)
                    .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
                    .Take(produtosParameters.PageSize)
                    .ToList();
        }

        public IEnumerable<Produto> GetProdutoPorPreco()
        {
            return Get().OrderBy(c => c.Preco).ToList();
        }
    }
}
