using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Entities.Extensions;
using Newtonsoft.Json;

namespace AccountOwnerServer.Controllers
{
    [Route("api/owners/{ownerId}/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
		private ILoggerManager _logger;
		private IRepositoryWrapper _repository;

		public AccountController(ILoggerManager logger,
			IRepositoryWrapper repository)
		{
			_logger = logger;
			_repository = repository;
		}

		[HttpGet]
		public IActionResult GetAccountsForOwner(Guid ownerId, [FromQuery] AccountParameter parameters)
		{
			var accounts = _repository.Account.GetAccountsByOwner(ownerId, parameters);

			var metadata = new
			{
				accounts.TotalCount,
				accounts.PageSize,
				accounts.CurrentPage,
				accounts.TotalPages,
				accounts.HasNext,
				accounts.HasPrevious
			};

			Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

			_logger.LogInfo($"Returned {accounts.TotalCount} owners from database.");

			return Ok(accounts);
		}

		//[HttpGet("{id}")]
		//public IActionResult GetAccountForOwner(Guid ownerId, Guid id)
		//{
  //          var account = _repository.Account.GetAccountsByOwner(ownerId, id);

  //          if (account.IsEmptyObject())
  //          {
  //              _logger.LogError($"Owner with id: {id}, hasn't been found in db.");
  //              return NotFound();
  //          }

  //          return Ok(account);
		//}

	}
}
