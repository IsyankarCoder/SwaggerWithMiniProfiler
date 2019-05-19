using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Profiling;
using System.Threading;
using System.Web.Http;

namespace WebApplication7.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DemoProfilingController : ControllerBase
    {
         
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string url1 = string.Empty;
            string url2 = string.Empty;

            using (MiniProfiler.Current.Step("Get Method"))
            {
                using (MiniProfiler.Current.CustomTiming("SQL", "SELECT * FROM Config"))
                {
                    Thread.Sleep(500);
                    url1 = "https://google.com/";
                    url2 = "https://stackoverflow.com/";

                }
            }
            using (MiniProfiler.Current.Step("Use data for http call"))
            {
                using (MiniProfiler.Current.CustomTiming("HTTP", "GET" + url1))
                {
                    var client = new System.Net.WebClient();
                    var reply = client.DownloadString(url1);
                }
                using (MiniProfiler.Current.CustomTiming("HTTP", "GET" + url2))
                {
                    var client = new System.Net.WebClient();
                    var reply = client.DownloadString(url1);
                }

            }

            return new string[] { "value1", "value2" };
        }

        public void GetFive()
        {
            int a = 10;
        }
    }
}