using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ana.Web.Models;
using Ana.Contracts.Business;

namespace Ana.Web.Controllers
{
    [Authorize]
    public class CardController : Controller
    {
        private IUserManager _userManager;
        private ICardManager _cardManager;

        public CardController(IUserManager userManager, ICardManager cardManager)
        {
            _userManager = userManager;
            _cardManager = cardManager;
        }

        
        //
        // GET: /Cards/
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create(string board_id,string board_url_name)
        {
            return View(new CardCreateOrEditModel()
            {
                board_id = board_id
            });
        }

        [HttpPost]
        public ActionResult Create(CardCreateOrEditModel model, string board_id, string board_url_name)
        {
            if (ModelState.IsValid)
            {
                _cardManager.CreateNewCard(model);

                return RedirectToAction("Details", "Board", new { id = board_id });
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult Done(string id)
        {
            var state = _cardManager.SetDone(id);
            return Json(state);
        }

        public ActionResult Details(string id)
        {
            var card = _cardManager.GetCard(id);

            return View(card);
        }

        public ActionResult Delete(string id)
        {
            var card = _cardManager.GetCard(id);
            _cardManager.DeleteCard(id);

            return RedirectToAction("Details", "Board", new { id = card.board_id });
        }

        public ActionResult Edit(string id)
        {
            var card = _cardManager.GetCard(id);

            return View(new CardCreateOrEditModel()
            {
                id = card.id,
                name = card.name,
                text = card.text,
                board_id = card.board_id
            });
        }

        [HttpPost]
        public ActionResult Edit(CardCreateOrEditModel model)
        {
            if (ModelState.IsValid)
            {
                _cardManager.UpdateCard(model);
                return RedirectToAction("Details", "Board", new { id = model.board_id });
            }

            return View(model);
        }

    }
}
