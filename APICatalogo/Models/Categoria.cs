using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models;

// Já definiu o mapeamento no arquivo de contexto, o código abaixo é uma redundancia
[Table("Categorias")]
public class Categoria
{

    public Categoria()
    {
        Produtos = new Collection<Produto>(); // É uma boa pratica inicializar a coleção
    }

    [Key]// redundante
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }

    // Cada categoria pode ter uma coleção de produtos
    public ICollection<Produto>? Produtos { get; set; }

}

