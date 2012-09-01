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
    public class CardController : ApiController
    {
        private ICardManager _cardManager;
        private IBoardManager _boardManager;

        public CardController()
        {
            _cardManager = ObjectFactory.GetInstance<ICardManager>();
            _boardManager = ObjectFactory.GetInstance<IBoardManager>();
        }

        //[GET] api/card/
        public IEnumerable<CardModel> Get()
        {
            var cards = _boardManager.GetMyBoards().SelectMany(b=>b.cards);
            return cards;
        }

        //[GET] api/card/{id}
        public CardModel Get(string id)
        {
            return _cardManager.GetCard(id);
        }


        //[POST] api/card/
        public HttpResponseMessage Post(CardCreateOrEditModel model)
        {
            if (ModelState.IsValid)
            {
                var card = _cardManager.CreateNewCard(model);

                var response = new HttpResponseMessage(HttpStatusCode.Created);

                string uri = Url.Route(null, new { id = card.id });
                response.Headers.Location = new Uri(Request.RequestUri, uri);

                return response;
            }
            throw new HttpResponseException(HttpStatusCode.BadRequest);
        }


        //[PUT] /api/card/{id}
        public HttpResponseMessage Put(string id, CardCreateOrEditModel model)
        {
            if (ModelState.IsValid)
            {
                _cardManager.UpdateCard(model);

                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        //[Patch] /api/card/{id}/done
        [HttpPatch]
        public HttpResponseMessage Done(string id)
        {
            _cardManager.SetDone(id);

            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }


        //[DELETE] /api/card/{id}
        public void Delete(string id)
        {
            _cardManager.DeleteCard(id);

            //throw new HttpResponseException(HttpStatusCode.NotFound);
        }



    }
}
