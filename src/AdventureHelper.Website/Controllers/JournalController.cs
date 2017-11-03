using AdventureHelper.Website.Models;
using AdventureHelper.Website.Models.Documents;
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
        private readonly DndJournalManager JournalMan;

        public JournalController()
        {
            var configuration = new Configuration();
            JournalMan = new DndJournalManager(new SimpleFileBank<Document>(configuration.DocumentsPath));
        }

        [Route("")]
        public ViewResult Journal()
        {
            return View();
        }

        //todos.....
        //- get real auth on characters instead of guid passing and name dropping.
        //- json file repo is a joke that's gone too far

        [Route("api/characters/{characterName}")]
        [HttpGet]
        public ActionResult Characters(string characterName)
        {
            var character = JournalMan.GetOrCreateCharacter(characterName);
            return Json(character, JsonRequestBehavior.AllowGet);
        }

        [Route("api/entries/{userId:guid}")]
        [HttpGet]
        public ActionResult Entries(Guid userId)
        {
            var entries = JournalMan.GetJournalEntries(userId);
            return Json(entries, JsonRequestBehavior.AllowGet);
        }

        [Route("api/entries/{userId:guid}")]
        [HttpPost]
        public JsonResult SaveEntry(Guid userId, JournalEntryDto data)
        {
            var newEntry = JournalMan.SaveJournalEntry(data, userId);
            return Json(newEntry);
        }

        [Route("api/links/{userId:guid}")]
        [HttpGet]
        public ActionResult Links(Guid userId)
        {
            var links = JournalMan.GetJournalLinks(userId);
            return Json(links, JsonRequestBehavior.AllowGet);
        }

        [Route("api/links/{userId:guid}")]
        [HttpPost]
        public JsonResult SaveLink(Guid userId, JournalLinksDto data)
        {
            var newLink = JournalMan.SaveLink(data, userId);
            return Json(newLink);
        }


        [Route("documents")]
        [HttpGet]
        public ViewResult Documents()
        {
            var repo = new SimpleFileBank<Document>(new Configuration().DocumentsPath);
            var docs = repo.Get();
            return View(docs);
        }
    }
}