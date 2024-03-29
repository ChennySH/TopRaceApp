﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TopRaceApp.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using TopRaceApp.DTOs;
using System.Linq;


namespace TopRaceApp.Services
{
    public class TopRaceAPIProxy
    {
        private const string CLOUD_URL = "TBD"; //API url when going on the cloud
        private const string CLOUD_PHOTOS_URL = "TBD";
        private const string DEV_ANDROID_EMULATOR_URL = "http://10.0.2.2:20698/TopRaceAPI"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_URL = "http://192.168.1.106:20698/TopRaceAPI"; //API url when using physucal device on android
        private const string DEV_WINDOWS_URL = "http://localhost:20698/TopRaceAPI"; //API url when using windoes on development
        private const string DEV_ANDROID_EMULATOR_PHOTOS_URL = "http://10.0.2.2:20698/Images/"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_PHOTOS_URL = "http://192.168.1.106:20698/Images/"; //API url when using physucal device on android
        private const string DEV_WINDOWS_PHOTOS_URL = "http://localhost:20698/Images/"; //API url when using windoes on development

        private HttpClient client;
        private string baseUri;
        private string basePhotosUri;
        private static TopRaceAPIProxy proxy = null;

        public static TopRaceAPIProxy CreateProxy()
        {
            string baseUri;
            string basePhotosUri;
            if (App.IsDevEnv)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    if (DeviceInfo.DeviceType == DeviceType.Virtual)
                    {
                        baseUri = DEV_ANDROID_EMULATOR_URL;
                        basePhotosUri = DEV_ANDROID_EMULATOR_PHOTOS_URL;
                    }
                    else
                    {
                        baseUri = DEV_ANDROID_PHYSICAL_URL;
                        basePhotosUri = DEV_ANDROID_PHYSICAL_PHOTOS_URL;
                    }
                }
                else
                {
                    baseUri = DEV_WINDOWS_URL;
                    basePhotosUri = DEV_WINDOWS_PHOTOS_URL;
                }
            }
            else
            {
                baseUri = CLOUD_URL;
                basePhotosUri = CLOUD_PHOTOS_URL;
            }

            if (proxy == null)
                proxy = new TopRaceAPIProxy(baseUri, basePhotosUri);
            return proxy;
        }
        public static TopRaceAPIProxy CreateProxyForTester()
        {
            string baseUri;
            string basePhotosUri;
            if (App.IsDevEnv)
            {
                baseUri = DEV_WINDOWS_URL;
                basePhotosUri = DEV_WINDOWS_PHOTOS_URL;                
            }
            else
            {
                baseUri = CLOUD_URL;
                basePhotosUri = CLOUD_PHOTOS_URL;
            }

            if (proxy == null)
                proxy = new TopRaceAPIProxy(baseUri, basePhotosUri);
            return proxy;
        }

        private TopRaceAPIProxy(string baseUri, string basePhotosUri)
        {
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();

            //Create client with the handler!
            this.client = new HttpClient(handler, true);
            this.baseUri = baseUri;
            this.basePhotosUri = basePhotosUri;
        }

        public string GetBasePhotoUri() { return this.basePhotosUri; }
        //Login!
        public async Task<User> LoginAsync(string userNameOrEmail, string password)
        {
            try
            {
                LoginDTO loginDTO = new LoginDTO
                {
                    UserNameOrEmail = userNameOrEmail,
                    Password = password
                };
                string loginDTOJson = JsonSerializer.Serialize(loginDTO);
                StringContent loginDTOJsonContent = new StringContent(loginDTOJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/Login", loginDTOJsonContent);
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    User u = JsonSerializer.Deserialize<User>(content, options);
                    return u;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> SignUpAsync(User user)
        {
            try
            {
                string userJson = JsonSerializer.Serialize(user);
                StringContent userJsonContent = new StringContent(userJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/SignUp", userJsonContent);
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    bool signedInSuccesfuly = JsonSerializer.Deserialize<bool>(content, options);
                    return signedInSuccesfuly;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> IsUserNameExistAsync(string userName)
        {
            try
            {
                string userNameJson = JsonSerializer.Serialize(userName);
                StringContent userNameJsonContent = new StringContent(userNameJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/IsUserNameExist", userNameJsonContent);
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    }; ;
                    string content = await response.Content.ReadAsStringAsync();
                    bool isExist = JsonSerializer.Deserialize<bool>(content, options);
                    return isExist;
                }
                else
                    return true;
            }
            catch (Exception)
            {
                return true;
            }
        }
        public async Task<bool> IsEmailExistAsync(string email)
        {
            try
            {
                string emailJson = JsonSerializer.Serialize(email);
                StringContent emailJsonContent = new StringContent(emailJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/IsEmailExist", emailJsonContent);
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    bool isExist = JsonSerializer.Deserialize<bool>(content, options);
                    return isExist;
                }
                else
                    return true;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public async Task<GameDTO> HostGameAsync(GameDTO game)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                    PropertyNameCaseInsensitive = true
                };
                string gameJson = JsonSerializer.Serialize(game, options);
                StringContent gameJsonContent = new StringContent(gameJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/HostGame", gameJsonContent);
                if (response.IsSuccessStatusCode)
                {
                    
                    string content = await response.Content.ReadAsStringAsync();
                    GameDTO gameAdded = JsonSerializer.Deserialize<GameDTO>(content, options);

                    return gameAdded;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<GameDTO> GetGameAsync(int GameID)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetGame?GameID={GameID}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    GameDTO game = JsonSerializer.Deserialize<GameDTO>(content, options);
                    foreach (PlayersInGame p in game.PlayersInGames)
                    {
                        if (p.Color.PicLink.StartsWith(this.basePhotosUri) == false)
                        { 
                            p.Color.PicLink = this.basePhotosUri + p.Color.PicLink;
                        }
                    }
                    return game;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<bool> SendMessageAsync(Message message)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                    PropertyNameCaseInsensitive = true
                };
                string messageJson = JsonSerializer.Serialize(message, options);
                StringContent messageJsonContent = new StringContent(messageJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/SendMessage", messageJsonContent);
                if (response.IsSuccessStatusCode)
                {                   
                    string content = await response.Content.ReadAsStringAsync();
                    bool isSent = JsonSerializer.Deserialize<bool>(content, options);
                    return isSent;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public async Task<GameDTO> JoinGameWithPrivateCodeAsync(string privateKey)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                    PropertyNameCaseInsensitive = true
                };
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/JoinPrivateGame?privateKey={privateKey}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    GameDTO game = JsonSerializer.Deserialize<GameDTO>(content, options);
                    return game;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<List<Models.Color>> GetAllColorsAsync()
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                    PropertyNameCaseInsensitive = true
                };
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetAllColors");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<Models.Color> lst = JsonSerializer.Deserialize<List<Models.Color>>(content, options);
                    foreach(Models.Color c in lst)
                    {
                        c.PicLink = this.basePhotosUri + c.PicLink;
                    }
                    return lst;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<Position>> GetAllPositionssAsync()
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                    PropertyNameCaseInsensitive = true
                };
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetAllPositions");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<Models.Position> lst = JsonSerializer.Deserialize<List<Models.Position>>(content, options);
                    return lst;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<bool> UpdatePlayerAsync(PlayersInGame playersInGame)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                    PropertyNameCaseInsensitive = true
                };
                string playerJson = JsonSerializer.Serialize(playersInGame, options);
                StringContent playerJsonContent = new StringContent(playerJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/UpdatePlayer", playerJsonContent);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    bool isUpdated = JsonSerializer.Deserialize<bool>(content, options);
                    return isUpdated;
                }
                else
                {
                    return false;
                }

            }
            catch(Exception e)
            {
                return false;
            }
        }
        public async Task<bool> CloseGameAsync(int gameID)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                    PropertyNameCaseInsensitive = true
                };
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/CloseGame?gameID={gameID}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    bool isClosed = JsonSerializer.Deserialize<bool>(content, options);
                    return isClosed;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public async Task<bool> KickOutAsync(int gameID, int playerID)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                    PropertyNameCaseInsensitive = true
                };
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/KickOutPlayer?gameID={gameID}&playerInGameID={playerID}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    bool isKicked = JsonSerializer.Deserialize<bool>(content, options);
                    return isKicked;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<bool> LogOutAsync()
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                    PropertyNameCaseInsensitive = true
                };
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/LogOut");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    bool isLoggedOut = JsonSerializer.Deserialize<bool>(content, options);
                    return isLoggedOut;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<GameDTO> StartGameAsync(int gameID)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                    PropertyNameCaseInsensitive = true
                };
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/StartGame?gameID={gameID}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    GameDTO gameDTO = JsonSerializer.Deserialize<GameDTO>(content, options);
                    return gameDTO;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<GameDTO> PlayAsync(int gameID)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                    PropertyNameCaseInsensitive = true
                };
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/Play?gameID={gameID}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    GameDTO gameDTO = JsonSerializer.Deserialize<GameDTO>(content, options);
                    return gameDTO;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<Stream> GetCrewmateStream(string colorLink)
        {
            return await this.client.GetStreamAsync(colorLink);
        }
        public async Task<GameDTO> PlayTestAsync(int gameID, int wantedResult)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                    PropertyNameCaseInsensitive = true
                };
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/PlayTest?gameID={gameID}&wantedResult={wantedResult}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    GameDTO gameDTO = JsonSerializer.Deserialize<GameDTO>(content, options);
                    return gameDTO;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<GameDTO> ResetGameAsync(int gameID)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                    PropertyNameCaseInsensitive = true
                };
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/ResetGame?gameID={gameID}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    GameDTO gameDTO = JsonSerializer.Deserialize<GameDTO>(content, options);
                    return gameDTO;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }
        //Upload file to server (only images!)
        public async Task<bool> UploadImage(Models.FileInfo fileInfo, string targetFileName)
        {
            try
            {
                var multipartFormDataContent = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(fileInfo.Name));
                multipartFormDataContent.Add(fileContent, "file", targetFileName);
                HttpResponseMessage response = await client.PostAsync($"{this.baseUri}/UploadImage", multipartFormDataContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }


}
