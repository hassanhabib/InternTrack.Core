// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

<<<<<<< HEAD
using System.Linq;
using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Models.Interns.Exceptions;
using InternTrack.Core.Api.Services.Foundations.Interns;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace InternTrack.Core.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InternController : RESTFulController
    {
        private readonly IInternService internService;

        public InternController(IInternService internService) =>
            this.internService = internService;

        [HttpGet]
        public ActionResult<IQueryable<Intern>> GetAllInterns()
        {
            try
            {
                IQueryable<Intern> storageInterns =
                    this.internService.RetrieveAllInternsAsync();

                return Ok(storageInterns);
            }
            catch (InternDependencyException interDependencyException)
            {
                return Problem(interDependencyException.Message);
            }
            catch (InternServiceException internServiceException)
            {
                return Problem(internServiceException.Message);
            }
        }

        [HttpPost]
        public async ValueTask<ActionResult<Intern>> PostInternAsync(Intern intern)
        {
            try
            {
                Intern createIntern =
                    await this.internService.AddInternAsync(intern);

                return Created(createIntern);
            }
            catch (InternValidationException internValidationException)
                when (internValidationException.InnerException is AlreadyExistsInternException)
            {
                return Conflict(internValidationException.InnerException);
            }
            catch (InternValidationException internValidationException)
            {
                return BadRequest(internValidationException.InnerException);
            }
            catch (InternDependencyException internDependencyException)
            {
                return InternalServerError(internDependencyException);
            }
            catch (InternServiceException internServiceException)
            {
                return InternalServerError(internServiceException);
            }
        }
    }
}
