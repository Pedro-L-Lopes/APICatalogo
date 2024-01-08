using APICatalogo.Context;
using APICatalogo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoXUnitTests
{
    public class DbUnitTestsMackInitialize
    {
        public DbUnitTestsMackInitialize() { }

        public void Seed(AppDbContext context)
        {
            context.Categorias.Add
            (new Categoria { CategoriaId = 2, Nome = "Sucos", ImagemUrl = "sucos1.jpg" });
        }
    }
}
