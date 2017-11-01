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
        private readonly SimpleFileBank<JournalEntryDto> JournalRepository;
        private readonly SimpleFileBank<JournalLinksDto> LinkRepository;

        public JournalController()
        {
            var configuration = new Configuration();
            JournalRepository = new SimpleFileBank<JournalEntryDto>(configuration.JournalFilePath);
            LinkRepository = new SimpleFileBank<JournalLinksDto>(configuration.LinkFilePath);
        }

        [Route("")]
        public ViewResult Journal()
        {
            return View();
        }

        [Route("api/entries/{characterName}")]
        [HttpGet]
        public ActionResult Entries(string characterName)
        {
            if (string.IsNullOrWhiteSpace(characterName))
                return HttpNotFound();

            var entries = JournalRepository.Get()
                .Where(entry => entry.CharacterOwner?.Equals(characterName, StringComparison.CurrentCultureIgnoreCase) == true)
                .ToArray();
            
            return Json(entries, JsonRequestBehavior.AllowGet);
        }

        [Route("api/entries")]
        [HttpPost]
        public void SaveEntry(JournalEntryDto data)
        {
            JournalRepository.Save(data);
        }

        [Route("api/links/{characterName}")]
        [HttpGet]
        public ActionResult Links(string characterName)
        {
            if (string.IsNullOrWhiteSpace(characterName))
                return HttpNotFound();

            var links = LinkRepository.Get()
                .Where(link => link.CharacterOwner?.Equals(characterName, StringComparison.CurrentCultureIgnoreCase) == true
                    || link.Shared)
                .ToArray();

            return Json(links, JsonRequestBehavior.AllowGet);
        }

        [Route("api/links")]
        [HttpPost]
        public void SaveLink(JournalLinksDto data)
        {
            LinkRepository.Save(data);
        }

        public PartialViewResult JournalViewComponent()
        {
            return PartialView();
        }
    }
}