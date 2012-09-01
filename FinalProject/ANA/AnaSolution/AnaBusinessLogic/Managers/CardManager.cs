using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Domain;
using Ana.Repository;
using Ana.Contracts.Business;
using Ana.Web.Models;
using Ana.Contracts.Repository;
using Ana.Utils;

namespace Ana.Business.Managers
{
    public class CardManager  : BaseManager, ICardManager
    {

        private ICardRepository _cardRepository;

        public CardManager(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
            _cardRepository.CreateIfNotExist();
        }

        public CardModel GetCard(string id)
        {
            var storageCards = _cardRepository.Query
                .Where(c =>  c.RowKey == id)
                .FirstOrDefault();

            return new CardModel
            {
                id = storageCards.RowKey,
                board_id = storageCards.PartitionKey,
                name = storageCards.Name,
                url_name = storageCards.UrlName,
                text = storageCards.Text,
                is_done = storageCards.IsDone,
                created_by = storageCards.CreatedBy,
                created_at = storageCards.CreatedAt,
                updated_by = storageCards.UpdatedBy,
                updated_at = storageCards.UpdatedAt,
            };
        }

        public CardModel GetCard(string id, string boardId)
        {
            var storageCards = _cardRepository.Query
                .Where(c => c.PartitionKey == boardId && c.RowKey == id)
                .FirstOrDefault();

            return new CardModel
                       {
                           id = storageCards.RowKey,
                           board_id = storageCards.PartitionKey,
                           name = storageCards.Name,
                           url_name = storageCards.UrlName,
                           text = storageCards.Text,
                           is_done = storageCards.IsDone,
                           created_by = storageCards.CreatedBy,
                           created_at = storageCards.CreatedAt,
                           updated_by = storageCards.UpdatedBy,
                           updated_at = storageCards.UpdatedAt,
                       };
        }

        public IEnumerable<CardModel> GetBoardCards(string boardId)
        {
            var storageCards =  _cardRepository.Query
                .Where(c => c.PartitionKey == boardId).ToList();

            return storageCards.Select(c => new CardModel
            {
                id = c.RowKey,
                board_id = c.PartitionKey,
                name = c.Name,
                url_name = c.UrlName,
                text = c.Text,
                is_done = c.IsDone,
                created_by = c.CreatedBy,
                created_at = c.CreatedAt,
                updated_by = c.UpdatedBy,
                updated_at = c.UpdatedAt,
            }).ToList();
        }

        public CardModel CreateNewCard(CardCreateOrEditModel card)
        {
            var currUser = base.CurrentUser();

            var cardToCreate = new Card()
            {
                PartitionKey = card.board_id,
                RowKey = GetNewShortGuid(),
                Name = card.name,
                UrlName = card.name.Slugify(),
                Text = card.text,
                IsDone = false,
                CreatedBy = currUser.UserName,
                CreatedAt = DateTime.Now,
                UpdatedBy = currUser.UserName,
                UpdatedAt = DateTime.Now,
            };

            _cardRepository.AddOrUpdateEntity(cardToCreate);


            return new CardModel()
            {
                id = cardToCreate.RowKey,
                board_id = cardToCreate.PartitionKey
            };
        }

        public void UpdateCard(CardCreateOrEditModel card)
        {
            var cardUpdate = _cardRepository.Query
                 .Where(c => c.RowKey == card.id && c.PartitionKey == card.board_id)
                 .FirstOrDefault();

            cardUpdate.Name = card.name;
            cardUpdate.Text = card.text;
            cardUpdate.UpdatedBy = CurrentUser().UserName;
            cardUpdate.UpdatedAt = DateTime.Now;

            _cardRepository.AddOrUpdateEntity(cardUpdate);

        }

        public void ShareCardWithUser(CardModel cardToShare, UserModel userToShare)
        {
            throw new NotImplementedException();
        }

        public void DeleteCard(string id)
        {
            var card = _cardRepository.Query
                 .Where(c => c.RowKey == id)
                 .FirstOrDefault();

            _cardRepository.DeleteEntity(card);
        }

        public IEnumerable<CardModel> GetMySharedCards()
        {
            throw new NotImplementedException();
        }


        public bool SetDone(string id)
        {
            var card = _cardRepository.Query
                .Where(c => c.RowKey == id)
                .FirstOrDefault();

            card.IsDone = !card.IsDone;
            _cardRepository.AddOrUpdateEntity(card);

            return card.IsDone;
        }
    }
}
