using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Domain;
using Ana.Web.Models;

namespace Ana.Contracts.Business
{
    public interface ICardManager : IManager
    {
        CardModel GetCard(string id);
        CardModel GetCard(string id, string board_id);
        IEnumerable<CardModel> GetBoardCards(string board_id);

        CardModel CreateNewCard(CardCreateOrEditModel card);
        bool SetDone(string id);

        void UpdateCard(CardCreateOrEditModel card);

        void ShareCardWithUser(CardModel cardToShare, UserModel userToShare);
        void DeleteCard(string id);

        IEnumerable<CardModel> GetMySharedCards();
    }
}
