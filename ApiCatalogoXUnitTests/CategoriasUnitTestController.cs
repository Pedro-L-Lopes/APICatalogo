using ApiCatalogo.Repository;
using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoXUnitTests
{
    public class CategoriasUnitTestController
    {
        private IMapper mapper;
        private IUnityOfWork repository;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string conectionString = "server=localhost;port=3306;database=CatalogDB;user=root;password=";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(conectionString)
                .options;
        }

        public CategoriasUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            //DbUnitTestsMackInitialize db = new DbUnitTestsMackInitialize();
            //db.Seed(context);

            repository = new UnityOfWork(context);
        }

        //Testes unitários
    }
}
