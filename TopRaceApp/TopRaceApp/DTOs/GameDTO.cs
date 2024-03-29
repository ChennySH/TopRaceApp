﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TopRaceApp.Models;

namespace TopRaceApp.DTOs
{
    public class GameDTO
    {
        public int Id { get; set; }
        public string GameName { get; set; }
        public bool IsPrivate { get; set; }
        public string PrivateKey { get; set; }
        public int HostUserId { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int StatusId { get; set; }
        public int UpdatesCounter { get; set; }
        public int MovesCounter { get; set; }
        public int? WinnerId { get; set; }
        public int? CurrentPlayerInTurnId { get; set; }
        public int? PreviousPlayerId { get; set; }
        public int LastRollResult { get; set; }
        public virtual PlayersInGame CurrentPlayerInTurn { get; set; }
        public virtual PlayersInGame PreviousPlayer { get; set; }
        public virtual GameStatus Status { get; set; }
        public virtual PlayersInGame Winner { get; set; }
        public virtual ICollection<PlayersInGame> PlayersInGames { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        //
        public GameDTO()
        {
            this.Messages = new List<Message>();
            this.PlayersInGames = new List<PlayersInGame>();
        }
        public Mover[][] Board { get; set; }
        public GameDTO(Game game)
        {
            Messages = game.Messages?.ToList();
            LastUpdateTime = game.LastUpdateTime;
            PlayersInGames = game.PlayersInGames?.ToList();
            Winner = game.Winner;
            CurrentPlayerInTurn = game.CurrentPlayerInTurn;
            Id = game.Id;
            PrivateKey = game.PrivateKey;
            IsPrivate = game.IsPrivate;
            UpdatesCounter = game.UpdatesCounter;
            MovesCounter = game.MovesCounter;
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                PropertyNameCaseInsensitive = true
            };

            Board = JsonSerializer.Deserialize<Mover[][]>(game.Board, options);
            StatusId = game.StatusId;
            Status = game.Status;
            WinnerId = game.WinnerId;
            CurrentPlayerInTurnId = game.CurrentPlayerInTurnId;
            PreviousPlayerId = game.PreviousPlayerId;
            PreviousPlayer = game.PreviousPlayer;
            LastRollResult = game.LastRollResult;
        }

        public Game ToGame()
        {

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                PropertyNameCaseInsensitive = true
            };
            return new Game()
            {
                Messages = this.Messages?.ToList(),
                LastUpdateTime = this.LastUpdateTime.ToUniversalTime(),
                GameName = this.GameName,
                PlayersInGames = this.PlayersInGames?.ToList(),
                Winner = this.Winner,
                CurrentPlayerInTurn = this.CurrentPlayerInTurn,
                Id = this.Id,
                PrivateKey = this.PrivateKey,
                IsPrivate = this.IsPrivate,
                Board = JsonSerializer.Serialize<Mover[][]>(Board, options),
                StatusId = this.StatusId,
                Status = this.Status,
                WinnerId = this.WinnerId,
                CurrentPlayerInTurnId = this.CurrentPlayerInTurnId,
                PreviousPlayerId = this.PreviousPlayerId,
                PreviousPlayer = this.PreviousPlayer,
                LastRollResult = this.LastRollResult,
                UpdatesCounter = this.UpdatesCounter,
                MovesCounter = this.MovesCounter,
            };
        }

        private static T[][] ToJaggedArray<T>(T[,] twoDimensionalArray)
        {
            int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
            int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
            int numberOfRows = rowsLastIndex - rowsFirstIndex + 1;

            int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
            int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
            int numberOfColumns = columnsLastIndex - columnsFirstIndex + 1;

            T[][] jaggedArray = new T[numberOfRows][];
            for (int i = 0; i < numberOfRows; i++)
            {
                jaggedArray[i] = new T[numberOfColumns];

                for (int j = 0; j < numberOfColumns; j++)
                {
                    jaggedArray[i][j] = twoDimensionalArray[i + rowsFirstIndex, j + columnsFirstIndex];
                }
            }
            return jaggedArray;
        }

        public static T[,] ToMatrix<T>(T[][] jaggedArray)
        {
            int rows = jaggedArray.Length;
            int cols = jaggedArray[0].Length;

            T[,] twoDimensionalArray = new T[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    twoDimensionalArray[i, j] = jaggedArray[i][j];
                }
            }

            return twoDimensionalArray;
        }
    }
}
