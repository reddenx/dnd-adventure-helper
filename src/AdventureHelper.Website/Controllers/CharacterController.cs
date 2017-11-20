using AdventureHelper.Website.Models;
using AdventureHelper.Website.Models.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdventureHelper.Website.Controllers
{
    [Route("character")]
    public class CharacterController : Controller
    {
        private SimpleFileBank<AttributeDto> AttributeRepo;

        public CharacterController()
        {
            var configuration = new Configuration();
            AttributeRepo = new SimpleFileBank<AttributeDto>(configuration.DocumentsPath);
        }

        [Route("")]
        public ViewResult Character()
        {
            return View();
        }
    }
}