using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Controllers
{

    // Endpoint => url
    // https://localhost:5001/v1/categories
    // http://localhost:5000/v1/categories
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<List<Category>>> Get([FromServices]DataContext context)
        {
            // AsNoTracking -> recupera a categoria de modo mais r�pido poss�vel (S� leitura). 
            //ToListAsync - Executa a consulta no banco de dados de fato. 
            var categories = await context.Categories.AsNoTracking().ToListAsync();
            return categories;
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context)
        {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return category;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
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
                return BadRequest(new { message = "N�o foi poss�vel criar a categoria!"});
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Category>> Put(int id, [FromBody]Category model, 
            [FromServices]DataContext context)
        {
            // verifica se o ID informado � o mesmo do modelo
            if (model.Id != id)
                return NotFound(new { message = "Categoria n�o encontrada" });

            // Verifica se os dados s�o v�lidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            // Tratativa para conflito de altera��o, erro de concorr�ncia.
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Este registro j� foi atualizado!" });
            }
            catch
            {
                return BadRequest(new { message = "N�o foi poss�vel atualizar a categoria!" });
            }
        }
        
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Category>> Delete(int id, 
            [FromServices]DataContext context)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(new { message = "Categoria n�o encontrada" });

            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new { message = "Categoria removida com sucesso" });
            }
            catch
            {
                return BadRequest(new { message = "N�o foi poss�vel remover a categoria!" });
            }
        }

        
    }
}