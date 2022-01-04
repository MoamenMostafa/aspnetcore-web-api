using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.Services;
using System;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private LogsService _logsService;

        public LogsController(LogsService logsService)
        {
            _logsService = logsService;
        }

        [HttpGet("get-all-logs-from-db")]
        public IActionResult GellAllLogsFromDb()
        {
            try
            {
                var logs = _logsService.GettAllLogsFromDb();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return BadRequest("Could not load logs from DB ");
            }
        }
    }
}
