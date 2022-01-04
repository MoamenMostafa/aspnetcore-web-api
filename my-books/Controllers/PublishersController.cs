using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using my_books.ActionResults;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModels;
using my_books.Exceptions;
using System;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private PublishersService _publishersService;
        private readonly ILogger<PublishersController> _logger;

        public PublishersController(PublishersService publishersService,ILogger<PublishersController> logger)
        {
            _publishersService = publishersService;
            _logger = logger;
        }


        [HttpGet("get-all-publishers")]
        public IActionResult GetAllPublishers(string sortBy,string searchString, int pageNumber)
        {
            //throw new Exception("This is an Exception thrown from Publisher Controller ");

            try
            {
                _logger.LogInformation("This is just a log in GetAllPublishers() ");
                var _result = _publishersService.GetAllPublishers(sortBy, searchString,pageNumber);
                return Ok(_result);
            }
            catch (Exception ex)
            {
                return BadRequest("Sorry , we could not load the publishers");
            }
        }


        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] PublisherVM publisher)
        {
            try 
            {
                var newPublisher = _publishersService.AddPublisher(publisher);
                return Created(nameof(AddPublisher), newPublisher);
            }
            catch (PublisherNameException ex)
            {
                return BadRequest($"{ex.Message}, Publisher name : {ex.PublisherName}");
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-publisher-books-with-authors/{publishId}")]
        public IActionResult GetPublisherData(int publishId)
        {
            var publisher = _publishersService.GetPublisherData(publishId);
            return Ok(publisher);
        }

        [HttpGet("get-publisher-by-id/{publisherId}")]
        public CustomActionResult GetPublisherById(int publisherId)
        {
            //throw new Exception("This is an exception that will be handled by middleware "); //for testing the middleware exceptions
            var _response = _publishersService.GetPublisherById(publisherId);
            if (_response != null)
            {
                //return Ok(_response);

                var _responseObj = new CustomActionResultVM()
                {
                    Publisher = _response
                };

                return new CustomActionResult(_responseObj);

            }
            else
            {
                var _responseObj = new CustomActionResultVM()
                {
                    Exception = new Exception("This is coming from publishers controller")
                };

                return new CustomActionResult(_responseObj);

                //return NotFound();
            }
        }


        [HttpDelete("delete-publisher-by-id/{publisherId}")]
        public IActionResult DeletePublisherById(int publisherId)
        {
            try
            {
                _publishersService.DeletePublisherById(publisherId);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
