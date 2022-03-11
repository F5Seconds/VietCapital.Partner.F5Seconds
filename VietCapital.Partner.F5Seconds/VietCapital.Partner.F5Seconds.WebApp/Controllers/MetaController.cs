﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace VietCapital.Partner.F5Seconds.WebApp.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MetaController : BaseApiController
    {
        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Startup).Assembly;

            var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {lastUpdate}");
        }
    }
}
