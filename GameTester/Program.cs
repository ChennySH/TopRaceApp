using System;
using System.Collections.Generic;
using System.Text;
using TopRaceApp.Models;
using TopRaceApp.Services;
using TopRaceApp.DTOs;
using System.IO;
using System.Threading.Tasks;

namespace GameTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Start();
            Console.ReadKey();
        }
        public static void Start()
        {
            TopRaceAPIProxy proxy = TopRaceAPIProxy.CreateProxyForTester();
            Task<User> task = proxy.LoginAsync("t@g", "12345678");
            User u = task.Result;
            task.Wait();
            GameDTO newGame = new GameDTO
            {
                GameName = $"User1's Game",
                IsPrivate = true,
                LastUpdateTime = DateTime.Now
            };
            Task<GameDTO> gameDTOTask = proxy.HostGameAsync(newGame);
            GameDTO gameDTO = gameDTOTask.Result;
            gameDTOTask.Wait();
            Task<GameDTO> startTask = proxy.StartGameAsync(gameDTO.Id);
            gameDTO = startTask.Result;
            startTask.Wait();
            PrintGame(gameDTO);
            while(gameDTO.Winner == null)
            {
                Console.ReadKey();
                Console.Clear();
                Task<GameDTO> taskGame = proxy.PlayAsync(gameDTO.Id);
                gameDTO = taskGame.Result;
                task.Wait();
                PrintGame(gameDTO);
            }
            Console.WriteLine($"{gameDTO.Winner.UserName} is the winner!");
        }
        public static void PrintGame(GameDTO gameDTO)
        {
            Mover[,] board = ToMatrix<Mover>(gameDTO.Board);
            for (int i = board.GetLength(1) -1 ; i > -1; i--)//y
            {
                for (int j = 0; j < board.GetLength(0); j++)//x
                {
                    PlayersInGame pl = IsPlayerOnMover(gameDTO, board[j, i]);
                    if (pl != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($"[X] ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (board[j, i].IsLadder)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($"[{board[j,i].StartPosId}->{board[j, i].EndPos.Id}] ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (board[j, i].IsSnake)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"[{board[j, i].StartPosId}->{board[j, i].EndPos.Id}] ");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write($"[{board[j, i].StartPos.Id}] ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine($"Rolled: {gameDTO.LastRollResult}");
        }
        public static PlayersInGame IsPlayerOnMover(GameDTO game, Mover mover)
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
                for (int j = 0; j < cols; j++)
                {
                    twoDimensionalArray[i, j] = jaggedArray[i][j];
                }
 
            }

            return twoDimensionalArray;
        }
        static void SetBoard()
        {
            List<int> freePositions = new List<int>();
            List<int> moversPosition = new List<int>();
            List<int> group1 = new List<int>();
            List<int> group2 = new List<int>();
            List<int> group3 = new List<int>();
            List<int> group4 = new List<int>();
            List<Mover> snakes = new List<Mover>();
            List<Mover> ladders = new List<Mover>();
            // deviding the board to 4 different groups
            for (int i = 2; i < 100; i++)
            {
                int decedes = i / 10;
                int digits = i % 10;
                if (i <= 50)
                {
                    if ((decedes % 2 == 0 && digits <= 5)||((decedes % 2 == 1 && digits > 5)))
                    {
                        group1.Add(i);
                    }
                    else
                    {
                        group2.Add(i);
                    }
                }
                else
                {
                    if ((decedes % 2 == 0 && digits <= 5) || ((decedes % 2 == 1 && digits > 5)))
                    {
                        group4.Add(i);
                    }
                    else
                    {
                        group3.Add(i);
                    }
                }
            }
            Random rnd = new Random();
            for (int i = 0; i < 16; i++)//getting 16 random free positions on group 1
            {
                int num = group1[rnd.Next(0, group1.Count)];
                freePositions.Add(num);
                group1.Remove(num);
            }
            for (int i = 0; i < 17; i++)//getting 17 random free positions on group 2
            {
                int num = group2[rnd.Next(0, group2.Count)];
                freePositions.Add(num);
                group2.Remove(num);
            }
            for (int i = 0; i < 17; i++)//getting 17 random free positions on group 3
            {
                int num = group3[rnd.Next(0, group3.Count)];
                freePositions.Add(num);
                group3.Remove(num);
            }
            int mustSnakePos = rnd.Next(97, 100);// setting a position between 97 - 99 where there must to be a snake
            for (int i = 0; i < 16; i++)//getting 16 random free positions on group 4
            {
                int num = group4[rnd.Next(0, group4.Count)];
                if(num == mustSnakePos)
                {
                    while(num == mustSnakePos)// if the snake pos is chosen you pick another one instead
                    {
                        num = group4[rnd.Next(0, group4.Count)];
                    }
                }
                freePositions.Add(num);
                group4.Remove(num);
            }
            foreach(int n in group1)
            {
                moversPosition.Add(n);
            }
            foreach (int n in group2)
            {
                moversPosition.Add(n);
            }
            foreach (int n in group3)
            {
                moversPosition.Add(n);
            }
            foreach (int n in group4)
            {
                moversPosition.Add(n);
            }
            moversPosition.Sort();
            //setting the snakes

            //setting the top snake
            int topIndex = moversPosition.IndexOf(mustSnakePos);//getting the index
            int topSnakeEndPos = moversPosition[rnd.Next(0,topIndex)];//setting the endPos by getting an index loser than the start index
            Mover topSnake = new Mover
            {
                StartPosId = mustSnakePos,
                EndPosId = topSnakeEndPos,
                IsLadder = false,
                IsSnake = true
            };

            // adding the snake
            snakes.Add(topSnake);
            moversPosition.Remove(mustSnakePos);
            moversPosition.Remove(topSnakeEndPos);
            for (int i = 0; i < 7; i++)
            {
                int startIndex = rnd.Next(2, moversPosition.Count);// setting a start index which can't be the first advalible pos idnex
                int endIndex = rnd.Next(0, startIndex);// seting a end index lower than the start index
                // getting the pos ids
                int startPosID = moversPosition[startIndex];
                int endPosId = moversPosition[endIndex];
                // creating the snake
                Mover snake = new Mover
                {
                    StartPosId = startPosID,
                    EndPosId = endPosId,
                    IsLadder = false,
                    IsSnake = true,
                };
                // adding the snake
                snakes.Add(snake);
                // removing the taken positions
                moversPosition.Remove(startPosID);
                moversPosition.Remove(endPosId);
            }
            // setting the ladders
            for (int i = 0; i < 8; i++)
            {
                int startIndex = rnd.Next(0, moversPosition.Count - 1);// setting a start index which can't be the last advalible pos idnex
                int endIndex = rnd.Next(startIndex + 1, moversPosition.Count);// seting a end index higher than the start index
                // getting the pos ids
                int startPosID = moversPosition[startIndex];
                int endPosId = moversPosition[endIndex];
                // creating the ladder
                Mover ladder = new Mover
                {
                    StartPosId = startPosID,
                    EndPosId = endPosId,
                    IsLadder = true,
                    IsSnake = false,
                };
                // adding the ladder
                ladders.Add(ladder);
                // removing the taken positions
                moversPosition.Remove(startPosID);
                moversPosition.Remove(endPosId);
            }
        }
        
    }
}
