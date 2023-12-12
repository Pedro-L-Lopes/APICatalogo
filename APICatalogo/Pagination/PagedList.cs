using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Pagination;
public class PagedList<T> : List<T>
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    // Itens a serem paginados // total de itens // número da página // tamanho da página
    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        AddRange(items);
    }

    // Fonte de dados // número da página // tamanho da página
    public async static Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int PageSize)
    {
        // Quantos itens tem no total para fazer a paginação
        var count = source.Count();
        //Paginando
        var items = await source.Skip((pageNumber - 1) * PageSize).Take(PageSize).ToListAsync();

        return new PagedList<T>(items, count, pageNumber, PageSize);
    }
}
