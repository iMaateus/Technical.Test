using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Technical.Test.Business.Interfaces;
using Technical.Test.Business.Models;
using Technical.Test.Business.Untils;

namespace Technical.Test.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecyclerController : MainController
    {
        private readonly IRecyclerService _reciclerService;

        public RecyclerController(IRecyclerService reciclerService,
            INotifier notifier) : base(notifier)
        {
            _reciclerService = reciclerService;
        }

        [HttpGet("status")]
        public async Task<ActionResult<CustomMessage<List<Recycler>>>> GetAll()
        {
            return CustomResponse(await _reciclerService.GetAll());
        }

        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [HttpPost("process/{days:int}")]
        public async Task<ActionResult<CustomMessage<Recycler>>> Post(int days)
        {
            var recycler = await _reciclerService.Add(days);

            if (ValidOperation())
            {
                await _reciclerService.DeleteBinary(recycler);
            }

            return CustomResponse(HttpStatusCode.Accepted, recycler);
        }
    }
}
