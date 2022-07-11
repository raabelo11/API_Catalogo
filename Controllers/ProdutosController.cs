using API_Catalogo.Context;
using API_Catalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

using System.Linq;

namespace API_Catalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _contexto;
        public ProdutosController(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        [Route("GetProdutos")]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            return _contexto.Produtos.AsNoTracking().ToList();
        }

        [HttpGet("GetProdutos/{id}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _contexto.Produtos.AsNoTracking()
                .FirstOrDefault(p => p.ProdutoId == id);

            if(produto == null)
            {
                return NotFound();
            }
            return Ok(produto);
        }

        [HttpPost]
        public ActionResult Post([FromBody]Produto produto)
        {
            _contexto.Produtos.Add(produto);
            _contexto.SaveChanges();
            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);  //retorna o que foi criado
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody]Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _contexto.Entry(produto).State = EntityState.Modified;
            _contexto.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _contexto.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto == null)
            {
                return NotFound();
            }
            _contexto.Produtos.Remove(produto);
            _contexto.SaveChanges();
            return produto;
        }
        

    }
}
