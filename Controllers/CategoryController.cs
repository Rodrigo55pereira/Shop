using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Controllers
{

    // Endpoint => url
    // https://localhost:5001/categories
    // http://localhost:5000/categories
    [Route("categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Get()
        {
            return new List<Category>();
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            return new Category();
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Category>> Post([FromBody]Category model, 
            [FromServices]DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível criar a categoria!"});
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> Put(int id, [FromBody]Category model, 
            [FromServices]DataContext context)
        {
            // verifica se o ID informado é o mesmo do modelo
            if (model.Id != id)
                return NotFound(new { message = "Categoria não encontrada" });

            // Verifica se os dados são válidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            // Tratativa para conflito de alteração, erro de concorrência.
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro já foi atualizado!" });
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível atualizar a categoria!" });
            }
        }
        
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> Delete(int id, 
            [FromServices]DataContext context)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(new { message = "Categoria não encontrada" });

            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new { message = "Categoria removida com sucesso" });
            }
            catch
            {
                return BadRequest(new { message = "Não foi possível remover a categoria!" });
            }
        }

        
    }
}