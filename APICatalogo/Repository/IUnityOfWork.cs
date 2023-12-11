namespace ApiCatalogo.Repository
{
    public interface IUnityOfWork
    {
        IProdutoRepository ProdutoRepository { get; }
        ICategoriaRepository CategoriaRepository { get; }

        Task Commit();
    }
}
