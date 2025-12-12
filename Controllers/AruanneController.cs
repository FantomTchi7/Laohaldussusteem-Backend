using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml;
using Newtonsoft.Json;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AruanneController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AruanneController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        private async Task<XDocument> GenerateXmlData()
        {
            var warehouses = await _context.Laod.ToListAsync();
            
            var invoices = await _context.Arved
                .Include(a => a.Tellija)
                .Include(a => a.Koostaja)
                .Include(a => a.Ettevõte)
                .Include(a => a.Tooted)
                .ToListAsync();

            XElement root = new XElement("laoseis");

            foreach (var ladu in warehouses)
            {
                XElement laduElem = new XElement("ladu",
                    new XAttribute("id", ladu.Id),
                    new XAttribute("aadress", ladu.Aadress)
                );

                var relevantInvoices = invoices
                    .Where(i => i.Tooted.Any(t => t.LaduId == ladu.Id))
                    .ToList();

                foreach (var inv in relevantInvoices)
                {
                    XElement arveElem = new XElement("arve",
                        new XAttribute("arve_nr", inv.Id),
                        new XAttribute("koostatud", inv.KoostatudKuupäev.ToString("yyyy-MM-dd")),
                        new XAttribute("maksetahtaeg", inv.Maksetähtaeg.ToString("yyyy-MM-dd")),
                        new XAttribute("tellija", inv.Tellija?.Nimi ?? "Tundmatu"),
                        new XAttribute("tellija_email", inv.Tellija?.Email ?? ""),
                        new XAttribute("koostaja", inv.Koostaja?.Nimi ?? "Tundmatu"),
                        new XAttribute("ettevote", inv.Ettevõte?.Nimi ?? "Määramata"),
                        new XAttribute("ettevote_reg", inv.Ettevõte?.Registrikood ?? ""),
                        new XAttribute("summa_neto", (inv.SummaKäibemaksuta / 100.0).ToString("F2")),
                        new XAttribute("km_maar", inv.Käibemaksumäär),
                        new XAttribute("summa_bruto", (inv.SummaKäibemaksuga / 100.0).ToString("F2"))
                    );

                    var productsInLadu = inv.Tooted.Where(t => t.LaduId == ladu.Id);

                    foreach (var prod in productsInLadu)
                    {
                        arveElem.Add(new XElement("toode",
                            new XElement("id", prod.Id),
                            new XElement("nimetus", prod.Nimetus),
                            new XElement("kogus", prod.Kogus),
                            new XElement("uhik", prod.Ühik),
                            new XElement("hind", (prod.Hind / 100.0).ToString("F2")),
                            new XElement("rida_summa", ((prod.Hind * prod.Kogus) / 100.0).ToString("F2"))
                        ));
                    }
                    laduElem.Add(arveElem);
                }
                root.Add(laduElem);
            }

            return new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);
        }

        [HttpGet("xml")]
        public async Task<IActionResult> GetXml()
        {
            var doc = await GenerateXmlData();
            return Content(doc.ToString(), "application/xml");
        }

        [HttpGet("html")]
        public async Task<IActionResult> GetHtml([FromQuery] string? search = null)
        {
            var xmlDoc = await GenerateXmlData();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var elementsToRemove = new List<XElement>();
                
                foreach (var ladu in xmlDoc.Descendants("ladu"))
                {
                    var nonMatchingInvoices = ladu.Elements("arve")
                        .Where(arve => 
                            !arve.Attribute("tellija")!.Value.Contains(search, StringComparison.OrdinalIgnoreCase) &&
                            !arve.Attribute("arve_nr")!.Value.Contains(search)
                        )
                        .ToList();

                    foreach (var inv in nonMatchingInvoices)
                    {
                        inv.Remove();
                    }
                }

                var emptyWarehouses = xmlDoc.Descendants("ladu")
                    .Where(l => !l.Elements("arve").Any())
                    .ToList();

                foreach (var w in emptyWarehouses) w.Remove();
            }

            string xsltPath = Path.Combine(_env.ContentRootPath, "Aruanne.xslt");
            
            if (!System.IO.File.Exists(xsltPath)) return Content(xmlDoc.ToString(), "application/xml");

            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(xsltPath);

            XsltArgumentList args = new XsltArgumentList();
            args.AddParam("searchTerm", "", search ?? "");

            using (StringWriter sw = new StringWriter())
            using (XmlWriter xwo = XmlWriter.Create(sw, transform.OutputSettings))
            {
                using (var reader = xmlDoc.CreateReader())
                {
                    transform.Transform(reader, args, xwo);
                }
                return Content(sw.ToString(), "text/html");
            }
        }

        [HttpGet("json")]
        public async Task<IActionResult> GetJsonFromXml()
        {
            var xDoc = await GenerateXmlData();

            string json;
            using (var reader = xDoc.CreateReader())
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(reader);
                json = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);
            }

            return Content(json, "application/json");
        }
    }
}