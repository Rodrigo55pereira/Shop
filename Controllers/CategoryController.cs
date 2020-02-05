using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<Category>> Post([FromBody]Category model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(model);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> Put(int id, [FromBody]Category model)
        {
            // verifica se o ID informado � o mesmo do modelo
            if (model.Id != id)
                return NotFound(new { message = "Categoria n�o encontrada" });

            // Verifica se os dados s�o v�lidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(model);
        }
        
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> Delete()
        {
            return Ok();
        }

        
    }
}