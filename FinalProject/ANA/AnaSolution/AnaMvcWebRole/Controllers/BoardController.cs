using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ana.Business;
using Ana.Domain;
using Ana.Utils;
using Ana.Web.Models;
using Ana.Contracts.Business;
using System.Web.Security;

namespace Ana.Web.Controllers
{
    [Authorize]
    public class BoardController : Controller
    {
        private IUserManager _userManager;
        private IBoardManager _boardManager;

        public BoardController(IUserManager userManager, IBoardManager boardManager)
        {
            _userManager = userManager;
            _boardManager = boardManager;
        }

        public ActionResult Index()
        {
            return View(_boardManager.GetMyBoards());
        }

        public ActionResult Create()
        {
            return View(new BoardCreateOrEditModel());
        }

        [HttpPost]
        public ActionResult Create(BoardCreateOrEditModel model)
        {
            if (ModelState.IsValid)
            {
                var newBoard = _boardManager.CreateNewBoard(model);

                return RedirectToAction("Index");
            }

            return View(model);
        }


        public ActionResult Edit(string id)
        {
            var board = _boardManager.GetBoardForEdit(id);

            return View(board);
        }

        [HttpPost]
        public ActionResult Edit(BoardCreateOrEditModel model)
        {
            if (ModelState.IsValid)
            {
                _boardManager.UpdateBoard(model);

                return RedirectToAction("Details", new { id = model.id });
            }

            return View(model);
        }

        public ActionResult Details(string id)
        {
            return View(_boardManager.GetBoard(id));
        }

        

        [HttpPost]
        public ActionResult RemoveUserShare(string id, string user_id)
        {
            _boardManager.RemoveUserShare(id, user_id);

            return RedirectToAction("Share");
        }


        [HttpPost]
        public ActionResult Share(string id, string user_name)
        {
            var user = Membership.GetUser(user_name);
            _boardManager.ShareBoardWithUser(id, user.ProviderUserKey.ToString());

            return RedirectToAction("Share");
        }

        // Get
        public ActionResult Share(string id)
        {
            return View(_boardManager.GetBoardShares(id));
        }

        public ActionResult Delete(string id)
        {
            _boardManager.DeleteBoard(id);

            return RedirectToAction("Index");
        }


        
    }
}
