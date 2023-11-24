using APICatalogo.Models;
using System.ComponentModel.DataAnnotations;
using System;

namespace ApiCatalogo.DTOs
{
    public class ProdutoDTO
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public string ImagemUrl { get; set; }
        public int CategoriaId { get; set; }
    }
}
