using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ana.Contracts.Repository;
using Ana.Domain;
using Ana.Repository;
using Ana.Contracts.Business;
using Ana.Web.Models;
using Ana.Utils;
using Ana.Domain.Azure.Storage;

namespace Ana.Business.Managers
{
    public class BoardManager : BaseManager, IBoardManager
    {
        private IBoardRepository _boardRepository;
        private ICardManager _cardManager;
        private IUserManager _userManager;
        private IBoardUserShareRepository _boardUserShareRepository;
        private IUserBoardShareRepository _userBoardShareRepository;
        private INotificationManager _notificationManager;

        public BoardManager(
            IBoardRepository boardRepository, 
            ICardManager cardManager,
            IUserBoardShareRepository userBoardShareRepository, 
            IBoardUserShareRepository boardUserShareRepository, 
            IUserManager userManager,
            INotificationManager notificationManager)
        {
            _cardManager = cardManager;
            _userManager = userManager;
            _notificationManager = notificationManager;
            _boardRepository = boardRepository;
            _boardRepository.CreateIfNotExist();
            _boardUserShareRepository = boardUserShareRepository;
            _boardUserShareRepository.CreateIfNotExist();

            _userBoardShareRepository = userBoardShareRepository;
            _userBoardShareRepository.CreateIfNotExist();
        }


        public BoardModel GetBoard(string id)
        {
            var boardModel = GetBaseBoard(id);

            var myBoards = GetMyBoards();
            
            boardModel.cards = _cardManager.GetBoardCards(id);
            boardModel.boards = myBoards.Select(b => new BoardResumeModel()
            {
               id = b.id,
               name = b.name,
               url_name = b.url_name
            });

            boardModel.shared_boards = GetMySharedBoards();

            // is shared if exists on shared_boards
            boardModel.is_shared = !boardModel.shared_boards.All(b => b.id != id);

            return boardModel;
        }

        

        public BoardCreateOrEditModel GetBoardForEdit(string id)
        {
            var boardModel = GetBaseBoard(id);

            return new BoardCreateOrEditModel()
            {
                id = boardModel.id,
                name = boardModel.name,
                description = boardModel.description,
            };
        }


        public BoardModel CreateNewBoard(BoardCreateOrEditModel newBoard)
        {
            var currUser = base.CurrentUser();

            var boardToCreate = new Board()
            {
                PartitionKey = currUser.RowKey,
                RowKey = GetNewShortGuid(),
                Name = newBoard.name,
                UrlName = newBoard.name.Slugify(),
                Description = newBoard.description,
                CreatedBy = currUser.UserName,
                CreatedAt = DateTime.Now,
                UpdatedBy = currUser.UserName,
                UpdatedAt = DateTime.Now,
            };

            _boardRepository.AddOrUpdateEntity(boardToCreate);


            return new BoardModel()
            {
                id = boardToCreate.RowKey
            };
        }

        public void UpdateBoard(BoardCreateOrEditModel board)
        {
            var user = CurrentUser();

            var storageBoard = _boardRepository.Query
                .Where(b => b.PartitionKey == user.RowKey && b.RowKey == board.id).FirstOrDefault();

            storageBoard.Name = board.name;
            storageBoard.UrlName = board.name.Slugify();
            storageBoard.Description = board.description;
            storageBoard.UpdatedBy = user.UserName;
            storageBoard.UpdatedAt = DateTime.Now;

            //send notifications
            _notificationManager.AlertBoardUpdate(storageBoard, user);

            _boardRepository.AddOrUpdateEntity(storageBoard);

        }

        public void DeleteBoard(string id)
        {
            var storageBoard = _boardRepository.Query
                .Where(b => b.PartitionKey == CurrentUserId() && b.RowKey == id).FirstOrDefault();

            _boardRepository.DeleteEntity(storageBoard);

            var userShares = _userBoardShareRepository.Query
                .Where(u => u.RowKey == id).ToList();

            var boardShares = _boardUserShareRepository.Query
                .Where(u => u.PartitionKey == id).ToList();

            userShares.ForEach((u)=> _userBoardShareRepository.DeleteEntity(u));
            boardShares.ForEach((u) => _boardUserShareRepository.DeleteEntity(u));

        }


        public void ShareBoardWithUser(string id, string userId)
        {
            var board = GetBaseBoard(id);
            var user = _userManager.GetUser(userId);

            var newUserShare = new UserBoardShare()
            {
                PartitionKey = user.id,
                RowKey = board.id,
                BoardName = board.name,
                UserName = board.created_by,
                BoardUrlName = board.url_name
            };

            var newShare = new BoardUserShare()
            {
                PartitionKey = board.id,
                RowKey = user.id,
                UserName = user.name,
            };

            _boardUserShareRepository.AddOrUpdateEntity(newShare);
            _userBoardShareRepository.AddOrUpdateEntity(newUserShare);
            
        }

        public void RemoveUserShare(string id, string userId)
        {
            var share =_boardUserShareRepository.Query.Where(usb => usb.PartitionKey == id && usb.RowKey == userId).FirstOrDefault();
            var userShare = _userBoardShareRepository.Query.Where(usb => usb.PartitionKey == userId && usb.RowKey == id).FirstOrDefault();

            _boardUserShareRepository.DeleteEntity(share);
            _userBoardShareRepository.DeleteEntity(userShare);
        }


        public ShareBoardModel GetBoardShares(string id)
        {
            var board = GetBoard(id);

            var shares = _boardUserShareRepository.Query.Where(bus => bus.PartitionKey == id).ToList();
            return new ShareBoardModel()
            {
                id = board.id,
                name = board.name,
                url_name = board.url_name,
                shared_users = shares.Select(bus => new UserModel()
                            {
                                id = bus.RowKey,
                                name = bus.UserName
                            })
            };
        }


        public IEnumerable<BoardModel> GetMyBoards()
        {
            var boards = _boardRepository.Query
                .Where(b => b.PartitionKey == this.CurrentUserId()).ToList();

            var ret =  boards.Select(b => new BoardModel()
            {
                id = b.RowKey,
                name = b.Name,
                url_name = b.UrlName,
                description = b.Description,
                created_by = b.CreatedBy,
                created_at = b.CreatedAt,
                updated_by = b.UpdatedBy,
                updated_at = b.UpdatedAt,
                boards = null,
                shared_boards = null,
                cards = _cardManager.GetBoardCards(b.RowKey),
            }).ToList();


            return ret;
        }



        public IEnumerable<BoardResumeModel> GetMySharedBoards()
        {
            var user = CurrentUser();
            var userShares = _userBoardShareRepository.Query
                .Where(ubs => ubs.PartitionKey == user.RowKey)
                .ToList();

            return userShares.Select(us => new BoardResumeModel()
                                               {
                                                   id = us.RowKey,
                                                   name = us.BoardName,
                                                   url_name = us.BoardUrlName,
                                                   user_name = us.UserName
                                               });
        }



        private BoardModel GetBaseBoard(string id)
        {
            var storageBoard = _boardRepository.Query
                .Where(b => b.RowKey == id).FirstOrDefault();

            var boardModel = new BoardModel()
            {
                id = storageBoard.RowKey,
                name = storageBoard.Name,
                url_name = storageBoard.UrlName,
                description = storageBoard.Description,
                created_by = storageBoard.CreatedBy,
                created_at = storageBoard.CreatedAt,
                updated_by = storageBoard.UpdatedBy,
                updated_at = storageBoard.UpdatedAt,
                boards = null,
                shared_boards = null,
                cards = _cardManager.GetBoardCards(storageBoard.RowKey)

            };
            return boardModel;
        }
    }
}
