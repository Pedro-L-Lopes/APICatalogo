using ApiCatalogo.Controllers;
using ApiCatalogo.DTOs;
using ApiCatalogo.Repository;
using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ApiCatalogoXUnitTests
{
    public class CategoriasUnitTestController
    {
        private IMapper mapper;
        private IUnityOfWork repository;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "server=localhost;port=3306;database=CatalogDB;user=root;password=";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, new MySqlServerVersion(new Version()))
             .Options;

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

        // Testes unitários
        [Fact]
        public async void GetCategorias_Return_OkResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);
            CategoriasParameters parameters = new()
            {
                PageNumber = 1,
                PageSize = 15
            };

            //Act
            var data = await controller.Get(parameters);

            //Assert
            Assert.IsType<OkObjectResult>(data.Result);
        }
    }
}