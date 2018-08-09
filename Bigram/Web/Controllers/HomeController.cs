using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Utilities;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private NGramHelper _nGramHelper;

        public HomeController()
        {
            _nGramHelper = new NGramHelper();
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ParseBigramsFromFile(IFormFile file)
        {
            string msg;

            try
            {
                if (file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var text = reader.ReadToEnd()?.Trim();

                        if (!string.IsNullOrEmpty(text))
                            return ParseBigrams(text);
                    }
                }

                msg = "The selected file is empty.";
            }
            catch (FileNotFoundException)
            {
                msg = "The selected file can not be found.";
            }
            catch
            {
                msg = "There was an issue reading the selected file.";
            }

            return Json(new { success = false, message = msg });
        }

        [HttpPost]
        public IActionResult ParseBigrams(string text)
        {
            string msg;

            try
            {
                var bigrams = _nGramHelper.GetNGrams(text, 2);
                return Json(new { success = true, content = bigrams.OrderByDescending(x => x.Value) });
            }
            catch (Exception e)
            {
                msg = e.Message;
            }

            return Json(new { success = false, message = msg });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
