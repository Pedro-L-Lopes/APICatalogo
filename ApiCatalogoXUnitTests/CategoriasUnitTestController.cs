using ApiCatalogo.Controllers;
using ApiCatalogo.DTOs;
using ApiCatalogo.Repository;
using APICatalogo.Context;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Pagination;
using AutoMapper;
using FluentAssertions;
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

        [Fact]
        public async void GetCategorias_Return_BadRequest()
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
            Assert.IsType<BadRequestObjectResult>(data.Result);
        }

        [Fact]
        public async void GetCategorias_MatchResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);
            CategoriasParameters parameters = new()
            {
                PageNumber = 1,
                PageSize = 15
            };

            //Act
            var result = await controller.Get(parameters);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);

            okObjectResult.Value.Should().NotBeNull(); // Adicionando esta verificação

            var cat = okObjectResult.Value.Should().BeAssignableTo<List<CategoriaDTO>>().Subject;

            Assert.Equal("Bebidas", cat[0].Nome);
            Assert.Equal("bebidas.jpg", cat[0].ImagemUrl);

            Assert.Equal("Sobremesas", cat[2].Nome);
            Assert.Equal("sobremesas.jpg", cat[2].ImagemUrl);
        }

        [Fact]
        public async void GetCategoriaById_Return_OkResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);
            var catId = 2;

            //Act
            var data = await controller.Get(catId);

            //Assert
            Assert.IsType<CategoriaDTO>(data.Value);
        }

        [Fact]
        public async void GetCategoriaById_Return_NotFound()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);
            var catId = 1000;

            //Act
            var data = await controller.Get(catId);

            //Assert
            Assert.IsType<NotFoundResult>(data.Result);
        }

        [Fact]
        public async void Post_Return_CreatedResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);

            var cat = new CategoriaDTO()
            { Nome = "teste unitario 1", ImagemUrl = "testecat.jpg" };

            //Act
            var data = await controller.Post(cat);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(data);
        }

        [Fact]
        public async void Put_Categoria_Update_Return_OkResult()
        {
            // Arrange
            var controller = new CategoriasController(repository, mapper);
            var cartId = 2;

            // Act
            var existingPost = await controller.Get(cartId);
            var result = existingPost.Value.Should().BeAssignableTo<CategoriaDTO>().Subject;

            var cartDto = new CategoriaDTO();
            cartDto.CategoriaId = cartId;
            cartDto.Nome = "Categoria atualizada teste 3 Final";
            cartDto.ImagemUrl = result.ImagemUrl;

            var updateData = await controller.Put(cartId, cartDto);

            // Assert
            Assert.IsType<OkResult>(updateData);
        }

        [Fact]
        public async void Delete_categoria_Return_OKResult()
        {
            // Arrange
            var controller = new CategoriasController(repository, mapper);
            var cartId = 2;

            // Act
            var data = await controller.Delete(cartId);

            // Assert 
            Assert.IsType<CategoriaDTO>(data.Value);
        }
    }
}