using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public PublishersController(PublishersService publishersService)
        {
            _publishersService = publishersService;
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
        public IActionResult GetPublisherById(int publisherId)
        {
            //throw new Exception("This is an exception that will be handled by middleware "); //for testing the middleware exceptions
            var _response = _publishersService.GetPublisherById(publisherId);
            if (_response != null)
            {
                return Ok(_response);

                //var _responseObj = new CustomActionResultVM() 
                //{
                //   Publisher = _response 
                //};

                //return new CustomActionResult(_responseObj);

                //return _response;
            }
            else
            {
                //var _responseObj = new CustomActionResultVM()
                //{
                //    Exception = new Exception("This is coming from publishers controller")
                //};

                //return new CustomActionResult(_responseObj);

                return NotFound();
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
