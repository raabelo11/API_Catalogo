using Microsoft.AspNetCore.Mvc;
using API_Catalogo.Context;
using API_Catalogo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace API_Catalogo.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _contexto;
        public CategoriasController(AppDbContext contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        [Route("GetCategorias")]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _contexto.Categorias.AsNoTracking().ToList();
        }

        [HttpGet("GetCategorias/{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _contexto.Categorias.AsNoTracking()
                .FirstOrDefault(p => p.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }

        [HttpPost]
        [Route("IncluirCategoria")]
        public ActionResult Post([FromBody] Categoria categoria)
        {
            _contexto.Categorias.Add(categoria);
            _contexto.SaveChanges();
            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoria);  //retorna o que foi criado
        }

        [HttpPut("AlterarCategoria/{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _contexto.Entry(categoria).State = EntityState.Modified;
            _contexto.SaveChanges();
            return Ok();
        }

        [HttpDelete("ExcluiCategoria/{id}")]
        public ActionResult<Categoria> Delete(int id)
        {
            var categoria = _contexto.Categorias.FirstOrDefault(c => c.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound();
            }
            _contexto.Categorias.Remove(categoria);
            _contexto.SaveChanges();
            return categoria;
        }
    }
}
