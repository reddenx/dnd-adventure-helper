using AdventureHelper.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdventureHelper.Website.Controllers
{
    [RoutePrefix("journal")]
    public class JournalController : Controller
    {
        private readonly DocumentRepository DocumentRepository;

        public JournalController()
        {
            var configuration = new Configuration();
            DocumentRepository = new DocumentRepository(configuration.JournalFilePath);
        }

        [Route("")]
        public ViewResult Journal()
        {
            return View();
        }

        //[Route("json/documents")]
        //[HttpGet]
        //public JsonResult GetDocuments() //DocumentDto[]
        //{
        //    var documents = DocumentRepository.GetAllDocuments();
        //    var dtos = documents.Select(d => d.ToDto());

        //    return Json(dtos, JsonRequestBehavior.AllowGet);
        //}

        //[Route("json/documents")]
        //[HttpPost]
        //public JsonResult SaveDocument(DocumentDto document)
        //{
        //    DocumentRepository.UpdateDocument(document);
        //    return Json("derp", JsonRequestBehavior.AllowGet);
        //}

        public PartialViewResult JournalViewComponent()
        {
            return PartialView();
        }
    }
}