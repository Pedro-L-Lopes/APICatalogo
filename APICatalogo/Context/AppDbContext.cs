using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Context
{
    public class AppDbContext : DbContext
    {                          // confgurar contexto no ef             //Passando pra classe base (Dbcontext)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        // Mapeamento das entidades
        public DbSet<Categoria>? Categorias { get; set; }
        public DbSet<Produto>? Produtos { get; set; } 
    }
}
