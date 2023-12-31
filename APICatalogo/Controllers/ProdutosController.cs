﻿using ApiCatalogo.DTOs;
using ApiCatalogo.Repository;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ApiCatalogo.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnityOfWork _uof;
        private readonly IMapper _mapper;
        public ProdutosController(IUnityOfWork contexto, IMapper mapper)
        {
            _uof = contexto;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPorPreco()
        {
            var produtos = await _uof.ProdutoRepository.GetProdutoPorPreco();
            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDTO;
        }

        // api/produtos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get([FromQuery] ProdutosParameters produtosParameters)
        {
            var produtos = await _uof.ProdutoRepository.GetProdutos(produtosParameters);

            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious

            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDTO;
        }

        // api/produtos/1
        [HttpGet("{id}", Name = "ObterProduto")]
        public async Task<ActionResult<ProdutoDTO>> Get(int id)
        {

            //throw new Exception("Exception ao retornar produto pelo id");
            //string[] teste = null;
            //if (teste.Length > 0)
            //{ }

            var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound();
            }

            var produtosDTO = _mapper.Map<ProdutoDTO>(produto);

            return produtosDTO;
        }

        //  api/produtos
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProdutoDTO produtoDto)
        {
            var produto = _mapper.Map<Produto>(produtoDto);

            _uof.ProdutoRepository.Add(produto);
            await _uof.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produtoDTO);
        }

        // api/produtos/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ProdutoDTO produtoDto)
        {

            if (id != produtoDto.ProdutoId)
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDto);

            _uof.ProdutoRepository.Update(produto);
            await _uof.Commit();
            return Ok();
        }

        //  api/produtos/1
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            //var produto = _uof.Produtos.Find(id);

            if (produto == null)
            {
                return NotFound();
            }

            _uof.ProdutoRepository.Delete(produto);
            await _uof.Commit();

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return produtoDto;
        }
    }
}