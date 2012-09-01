using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Ana.Business.Managers;
using Ana.Contracts.Business;
using Ana.Domain;
using Ana.Web.Authentication;
using Ana.Web.Models;
using StructureMap;
using System.Net.Http;
using System.Net;

namespace Ana.Web.ControllersApi
{
    [OAuth2]
    public class BoardController : ApiController
    {
        private IBoardManager _boardManager;

        public BoardController()
        {
            _boardManager = ObjectFactory.GetInstance<IBoardManager>();
        }

        //[GET] api/board
        public IEnumerable<BoardModel> Get()
        {
            return _boardManager.GetMyBoards();
        }

        //[GET] api/board/{id}
        public BoardModel Get(string id)
        {
            return _boardManager.GetBoard(id);
        }


        //[POST] api/board/
        public HttpResponseMessage Post(BoardCreateOrEditModel model)
        {
            if (ModelState.IsValid)
            {
                var board = _boardManager.CreateNewBoard(model);
                
                var response = new HttpResponseMessage(HttpStatusCode.Created);

                string uri = Url.Route(null, new { id = board.id });
                response.Headers.Location = new Uri(Request.RequestUri, uri);

                return response;
            }
            throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        //[PUT] /api/board/{id}
        public HttpResponseMessage Put(string id, BoardCreateOrEditModel model)
        {
            if (ModelState.IsValid)
            {
                _boardManager.UpdateBoard(model);
                
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            throw new HttpResponseException(HttpStatusCode.BadRequest);
        }


        //[DELETE] /api/board/{id}
        public void Delete(string id)
        {
            _boardManager.DeleteBoard(id);
            
                //throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        

    }
}
