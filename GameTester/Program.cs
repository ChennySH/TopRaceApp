using System;
using TopRaceApp.Models;
using TopRaceApp.Services;
using TopRaceApp.DTOs;

namespace GameTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
        public static void PrintGame(Game game)
        {
            GameDTO gameDTO = new GameDTO(game);
            MoversInGame[,] board = gameDTO.board;
            for (int i = board.GetLength(1) -1 ; i > -1; i++)//y
            {
                for (int j = 0; j < board.GetLength(0); j++)//x
                {
                    PlayersInGame pl = IsPlayerOnMover(game, board[j, i]);
                    if (board[j, i].IsLadder)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"[->{board[j, i].EndPos.Id}] ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    if (board[j, i].IsSnake)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"[->{board[j, i].EndPos.Id}] ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    if(pl != null)
                    {

                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($"[O] ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write($"[{board[j, i].Id}] ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Rolled: {game.LastRollResult}");
        }
        public static PlayersInGame IsPlayerOnMover(Game game, MoversInGame mover)
        {
            foreach(PlayersInGame pl in game.PlayersInGames)
            {
                if(pl.CurrentPosId == mover.StartPosId && pl.IsInGame)
                {
                    return pl;
                }
            }
            return null;
        }
    }
}
