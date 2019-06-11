using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HtmlToPDF.Controllers
{
    [Route("api/[controller]")]
    public class ConvertController : Controller
    {
        private IConverter _converter;

        public ConvertController(IConverter converter)
        {
            _converter = converter;
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get(string url)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    PaperSize = PaperKind.Letter,
                    Orientation = Orientation.Portrait,
                    ViewportSize = "1280x1024"
                }
                
            };

            if(!string.IsNullOrEmpty(url))
            {
                doc.Objects.Add(new ObjectSettings { Page = url,  });
            }

            byte[] pdf = _converter.Convert(doc);


            return new FileContentResult(pdf, "application/pdf");
        }

        [HttpPost]
        public IActionResult Post(string html, Boolean isBlueCard = false, Boolean isPortrait = true)
        {
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    PaperSize = PaperKind.Letter,
                    Orientation = isBlueCard || isPortrait ? Orientation.Portrait : Orientation.Landscape,
                    Outline = false
                }
            };

            if (isBlueCard)
            {
                doc.GlobalSettings.Margins = new MarginSettings { Left = 0, Right = 0, Top = 5 };
            }

            if (!string.IsNullOrEmpty(html))
            {
                doc.Objects.Add(new ObjectSettings {  
                    HtmlContent = html,
                    LoadSettings = {
                        JSDelay = 500,
                        StopSlowScript = false,
                        ZoomFactor = isBlueCard ? 0.99 : 1.0
                    }, 
                    WebSettings = { 
                        EnableIntelligentShrinking = false 
                    }
                });
            }

            byte[] pdf = _converter.Convert(doc);


            return new FileContentResult(pdf, "application/pdf");
        }
    }
}