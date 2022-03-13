using System;
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
        public int ChatRoomId { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int StatusId { get; set; }
        public int? WinnerId { get; set; }
        public int? CurrentPlayerInTurnId { get; set; }
        public int? PreviousPlayerId { get; set; }
        public int LastRollResult { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }
        public virtual PlayersInGame CurrentPlayerInTurn { get; set; }
        public virtual User HostUser { get; set; }
        public virtual PlayersInGame PreviousPlayer { get; set; }
        public virtual GameStatus Status { get; set; }
        public virtual PlayersInGame Winner { get; set; }
        public virtual ICollection<PlayersInGame> PlayersInGames { get; set; }
        //
        public GameDTO() { }

        public MoversInGame[][] Board { get; set; }
       

        

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

        static T[,] ToMatrix<T>(T[][] jaggedArray)
        {
            int rows = jaggedArray.Length;
            int cols = jaggedArray[0].Length;

            T[,] twoDimensionalArray = new T[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; i++)
                {
                    twoDimensionalArray[i, j] = jaggedArray[i][j];
                }
            }

            return twoDimensionalArray;
        }
    }
}
