

namespace Ejercicio.WebApi.Controllers
{
    using Ejercicio.Business.Interfaces;
    using Ejercicio.Models.Api;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// CRUDController
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TKey"></typeparam>    
    [Authorize]
    [ValidateModel]
    public abstract class CRUDController<TModel, TKey> : BaseController where TModel : class, IModel<TKey>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly ICRUDBusiness<TModel, TKey> negocio;
        private bool bComprobarExiste = false;

        /// <summary>
        /// 
        /// </summary>
        public CRUDController()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="negocio"></param>
        /// <param name="ComprobarExiste"></param>
        public CRUDController(ICRUDBusiness<TModel, TKey> negocio, bool ComprobarExiste = false) : base()
        {
            this.negocio = negocio;
            this.bComprobarExiste = ComprobarExiste;
        }
        /// <summary>
        /// GetAll 
        /// </summary>
        /// <returns></returns>
        [Route]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            IEnumerable<TModel> templates = await negocio.GetAllAsync().ConfigureAwait(false);
            if (templates is null || !templates.Any())
                return NoContent();
            else
                return Ok(templates);
        }

        /// <summary>
        /// Get 
        /// </summary>
        /// <param name="id">guid id</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(TKey id)
        {
            TModel dato = await this.negocio.GetAsync(id).ConfigureAwait(false);
            if (dato != null)
                return Ok(dato);
            else
                return NotFound();
        }

        /// <summary>
        /// Método creación  
        /// </summary>
        /// <param name="value"></param>
        [Route]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]TModel value)
        {
            if (bComprobarExiste)
                if (await negocio.ExistAsync(value).ConfigureAwait(true)) return Conflict();
            if (!ModelState.IsValid || value is null) return Conflict();
            value.Id = default(TKey);
            value = await negocio.SaveAsync(value).ConfigureAwait(true);
            if (value != null)
                return Created(Request.RequestUri, value);
            else
                return NotFound();
        }

        /// <summary>
        /// Método modifiación 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(TKey id, [FromBody]TModel value)
        {
            if (!await negocio.ExistAsync(value).ConfigureAwait(true)) return Conflict();
            if (!ModelState.IsValid || value is null) return Conflict();
            value.Id = id;
            value = await negocio.SaveAsync(value).ConfigureAwait(false);
            if (value != null)
                return Created(Request.RequestUri, value);
            else
                return NotFound();
        }

        /// <summary>
        /// Método borrado  
        /// </summary>
        /// <param name="id"></param>
        // DELETE: api/Prueba/5
        [Route("{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(TKey id)
        {
            if (!await negocio.ExistAsync(id).ConfigureAwait(true)) return Conflict();
            bool resultado = await this.negocio.DeleteAsync(id).ConfigureAwait(false);
            if (resultado)
                return Ok();
            else
                return NotFound();
        }

    }
}