using LoLLauncher;
using LoLLauncher.RiotObjects.Platform.Catalog.Champion;
using LoLLauncher.RiotObjects.Platform.Clientfacade.Domain;
using LoLLauncher.RiotObjects.Platform.Game;
using LoLLauncher.RiotObjects.Platform.Game.Message;
using LoLLauncher.RiotObjects.Platform.Matchmaking;
using LoLLauncher.RiotObjects.Platform.Statistics;
using LoLLauncher.RiotObjects;
using LoLLauncher.RiotObjects.Leagues.Pojo;
using LoLLauncher.RiotObjects.Platform.Game.Practice;
using LoLLauncher.RiotObjects.Platform.Harassment;
using LoLLauncher.RiotObjects.Platform.Leagues.Client.Dto;
using LoLLauncher.RiotObjects.Platform.Login;
using LoLLauncher.RiotObjects.Platform.Reroll.Pojo;
using LoLLauncher.RiotObjects.Platform.Statistics.Team;
using LoLLauncher.RiotObjects.Platform.Summoner;
using LoLLauncher.RiotObjects.Platform.Summoner.Boost;
using LoLLauncher.RiotObjects.Platform.Summoner.Masterybook;
using LoLLauncher.RiotObjects.Platform.Summoner.Runes;
using LoLLauncher.RiotObjects.Platform.Summoner.Spellbook;
using LoLLauncher.RiotObjects.Platform.Game.Map;
using LoLLauncher.RiotObjects.Team;
using LoLLauncher.RiotObjects.Team.Dto;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace JAutoQueue
{
    internal class Program
    {
        public Process exeProcess;
        public GameDTO currentGame = new GameDTO();
        public ChampionDTO[] availableChampsArray;
        public LoginDataPacket loginPacket = new LoginDataPacket();
        public LoLConnection connection = new LoLConnection();
        public List<ChampionDTO> availableChamps = new List<ChampionDTO>();

        public bool firstTimeInLobby = true;
        public bool firstTimeInQueuePop = true;
        public bool firstTimeInCustom = true;
        public bool HasLaunchedGame = false;

        public string Accountname;
        public string Password;
        public string ipath;
        public string regiona;

        public int threadID;
        public double sumLevel { get; set; }
        public double archiveSumLevel { get; set; }
        public double rpBalance { get; set; }

        public QueueTypes queueType { get; set; }
        public QueueTypes actualQueueType { get; set; }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        public Program(string username, string password, string region, string path, int threadid, QueueTypes QueueType)
        {
            ipath = path; Accountname = username; Password = password; threadID = threadid; queueType = QueueType;
            regiona = region;
            connection.OnConnect += new LoLConnection.OnConnectHandler(connection_OnConnect);
            connection.OnDisconnect += new LoLConnection.OnDisconnectHandler(connection_OnDisconnect);
            connection.OnError += new LoLConnection.OnErrorHandler(connection_OnError);
            connection.OnLogin += new LoLConnection.OnLoginHandler(connection_OnLogin);
            connection.OnLoginQueueUpdate += new LoLConnection.OnLoginQueueUpdateHandler(connection_OnLoginQueueUpdate);
            connection.OnMessageReceived += new LoLConnection.OnMessageReceivedHandler(connection_OnMessageReceived);
            switch (region)
            {
                case "EUW":
                    connection.Connect(username, password, Region.EUW, "4.21.14_12_08_11_36");
                    break;
                case "EUNE":
                    connection.Connect(username, password, Region.EUN, "4.21.14_12_08_11_36");
                    break;
                case "BR":
                    connection.Connect(username, password, Region.BR, "4.21.14_12_08_11_36");
                    break;
                case "KR":
                    connection.Connect(username, password, Region.KR, "4.21.14_12_08_11_36");
                    break;
                case "OCE":
                    connection.Connect(username, password, Region.OCE, "4.21.14_12_08_11_36");
                    break;
                case "NA":
                    connection.Connect(username, password, Region.NA, "4.21.14_12_08_11_36");
                    break;
                case "TR":
                    connection.Connect(username, password, Region.TR, "4.21.14_12_08_11_36");
                    break;
                case "TW":
                    connection.Connect(username, password, Region.TW, "4.21.14_12_08_11_36");
                    break;
                case "RU":
                    connection.Connect(username, password, Region.RU, "4.21.14_12_08_11_36");
                    break;
                case "LAN":
                    connection.Connect(username, password, Region.LAN, "4.21.14_12_08_11_36");
                    break;
                case "LAS":
                    connection.Connect(username, password, Region.LAS, "4.21.14_12_08_11_36");
                    break;
            }
        }
        public async void connection_OnMessageReceived(object sender, object message)
        {
            if (message is GameDTO)
            {
                GameDTO game = message as GameDTO;
                switch (game.GameState)
                {
                    case "CHAMP_SELECT":
                        firstTimeInCustom = true;
                        firstTimeInQueuePop = true;
                        if (firstTimeInLobby)
                        {
                            firstTimeInLobby = false;
                            updateStatus("In Champion Select", Accountname);
                            await connection.SetClientReceivedGameMessage(game.Id, "CHAMP_SELECT_CLIENT");
                            if (queueType != QueueTypes.ARAM)
                            {
                                if (Core.championId != "")
                                {
                                    await connection.SelectChampion(Enums.championToId(Core.championId));
                                    await connection.ChampionSelectCompleted();
                                }
                                else
                                {
                                    await connection.SelectChampion(availableChampsArray.First(champ => champ.Owned || champ.FreeToPlay).ChampionId);
                                    await connection.ChampionSelectCompleted();
                                }
                            }
                            break;
                        }
                        else
                            break;
                    case "POST_CHAMP_SELECT":
                        firstTimeInLobby = false;
                        updateStatus("(Post Champ Select)", Accountname);
                        break;
                    case "PRE_CHAMP_SELECT":
                        updateStatus("(Pre Champ Select)", Accountname);
                        break;
                    case "GAME_START_CLIENT":
                        updateStatus("Game client ran", Accountname);
                        break;
                    case "GameClientConnectedToServer":
                        updateStatus("Client connected to the server", Accountname);
                        break;
                    case "IN_QUEUE":
                        updateStatus("In Queue", Accountname);
                        break;
                    case "TERMINATED":
                        updateStatus("Re-entering queue", Accountname);
                        firstTimeInQueuePop = true;
                        break;
                    case "JOINING_CHAMP_SELECT":
                        if (firstTimeInQueuePop)
                        {
                            updateStatus("Queue popped", Accountname);
                            if (game.StatusOfParticipants.Contains("1"))
                            {
                                updateStatus("Accepted Queue", Accountname);
                                firstTimeInQueuePop = false;
                                firstTimeInLobby = true;
                                await connection.AcceptPoppedGame(true);
                            }
                        }
                        break;
                }
            }
            else if (message is PlayerCredentialsDto)
            {
                PlayerCredentialsDto dto = message as PlayerCredentialsDto;
                if (!HasLaunchedGame)
                {
                    HasLaunchedGame = true;
                    new Thread((ThreadStart)(() =>
                    {
                        LaunchGame(dto);
                        Thread.Sleep(3000);
                    })).Start();
                }
            }
            else if (!(message is GameNotification) && !(message is SearchingForMatchNotification))
            {
                if (message is EndOfGameStats)
                {
                    MatchMakerParams matchParams = new MatchMakerParams();
                    //Set BotParams
                    if (queueType == QueueTypes.INTRO_BOT)
                    {
                        matchParams.BotDifficulty = "INTRO";
                    }
                    else if (queueType == QueueTypes.BEGINNER_BOT)
                    {
                        matchParams.BotDifficulty = "EASY";
                    }
                    else if (queueType == QueueTypes.MEDIUM_BOT)
                    {
                        matchParams.BotDifficulty = "MEDIUM";
                    }
                    //Check if is available to join queue.
                    if (sumLevel == 3 && actualQueueType == QueueTypes.NORMAL_5x5)
                    {
                        queueType = actualQueueType;
                    }
                    else if (sumLevel == 6 && actualQueueType == QueueTypes.ARAM)
                    {
                        queueType = actualQueueType;
                    }
                    else if (sumLevel == 7 && actualQueueType == QueueTypes.NORMAL_3x3)
                    {
                        queueType = actualQueueType;
                    }
                    matchParams.QueueIds = new Int32[1] { (int)queueType };
                    SearchingForMatchNotification m = await connection.AttachToQueue(matchParams);
                    if (m.PlayerJoinFailures == null)
                    {
                        updateStatus("In Queue: " + queueType.ToString(), Accountname);
                    }
                    else
                    {
                        updateStatus("Couldn't enter Queue! >.<", Accountname);
                    }
                }
                else
                {
                    if (message.ToString().Contains("EndOfGameStats"))
                    {
                        EndOfGameStats eog = new EndOfGameStats();
                        connection_OnMessageReceived(sender, eog);
                        exeProcess.Kill();
                        loginPacket = await this.connection.GetLoginDataPacketForUser();
                        archiveSumLevel = sumLevel;
                        sumLevel = loginPacket.AllSummonerData.SummonerLevel.Level;
                        if (sumLevel != archiveSumLevel)
                        {
                            levelUp();
                        }
                    }
                }
            }
        }
        public void LaunchGame(PlayerCredentialsDto CurrentGame)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.WorkingDirectory = FindLoLExe();
            startInfo.FileName = "League of Legends.exe";
            startInfo.Arguments = "\"8394\" \"LoLLauncher.exe\" \"\" \"" + CurrentGame.ServerIp + " " +
                CurrentGame.ServerPort + " " + CurrentGame.EncryptionKey + " " + CurrentGame.SummonerId + "\"";
            updateStatus("Playing League of Legends", Accountname);
            new Thread(() =>
            {
                exeProcess = Process.Start(startInfo);
                while (exeProcess.MainWindowHandle == IntPtr.Zero) { }
                Thread.Sleep(1000);
            }).Start();
        }
        private void connection_OnLoginQueueUpdate(object sender, int positionInLine)
        {
            if (positionInLine <= 0)
                return;
            updateStatus("Position to login: " + (object)positionInLine, Accountname);
        }
        private void connection_OnLogin(object sender, string username, string ipAddress)
        {
            new Thread((ThreadStart)(async () =>
            {
                updateStatus("Connecting...", Accountname);
                loginPacket = await connection.GetLoginDataPacketForUser();
                await connection.Subscribe("bc", loginPacket.AllSummonerData.Summoner.AcctId);
                await connection.Subscribe("cn", loginPacket.AllSummonerData.Summoner.AcctId);
                await connection.Subscribe("gn", loginPacket.AllSummonerData.Summoner.AcctId);
                if (loginPacket.AllSummonerData == null)
                {
                    Random rnd = new Random();
                    String summonerName = Accountname;
                    if (summonerName.Length > 16)
                        summonerName = summonerName.Substring(0, 12) + new Random().Next(1000, 9999).ToString();
                    await connection.CreateDefaultSummoner(summonerName);
                    updateStatus("Created Summoner: " + summonerName, Accountname);
                }
                sumLevel = loginPacket.AllSummonerData.SummonerLevel.Level;
                string sumName = loginPacket.AllSummonerData.Summoner.Name;
                double sumId = loginPacket.AllSummonerData.Summoner.SumId;
                rpBalance = loginPacket.RpBalance;
                if (sumLevel > Core.maxLevel || sumLevel == Core.maxLevel)
                {
                    connection.Disconnect();
                    updateStatus("Summoner: " + sumName + " is already max level.", Accountname);
                    updateStatus("Log into new account.", Accountname);
                    Core.lognNewAccount();
                    return;
                }
                if (rpBalance == 400.0 && Core.buyBoost)
                {
                    updateStatus("Buying XP Boost", Accountname);
                    try
                    {
                        Task t = new Task(buyBoost);
                        t.Start();
                    }
                    catch (Exception exception)
                    {
                        updateStatus("Couldn't buy RP Boost.\n" + exception, Accountname);
                    }
                }
                if (sumLevel < 3.0 && queueType == QueueTypes.NORMAL_5x5)
                {
                    this.updateStatus("Need to be Level 3 before NORMAL_5x5 queue.", Accountname);
                    this.updateStatus("Joins Co-Op vs AI (Beginner) queue until 3", Accountname);
                    queueType = QueueTypes.BEGINNER_BOT;
                    actualQueueType = QueueTypes.NORMAL_5x5;
                }
                else if (sumLevel < 6.0 && queueType == QueueTypes.ARAM)
                {
                    this.updateStatus("Need to be Level 6 before ARAM queue.", Accountname);
                    this.updateStatus("Joins Co-Op vs AI (Beginner) queue until 6", Accountname);
                    queueType = QueueTypes.BEGINNER_BOT;
                    actualQueueType = QueueTypes.ARAM;
                }
                else if (sumLevel < 7.0 && queueType == QueueTypes.NORMAL_3x3)
                {
                    this.updateStatus("Need to be Level 7 before NORMAL_3x3 queue.", Accountname);
                    this.updateStatus("Joins Co-Op vs AI (Beginner) queue until 7", Accountname);
                    queueType = QueueTypes.BEGINNER_BOT;
                    actualQueueType = QueueTypes.NORMAL_3x3;
                }
                if (loginPacket.AllSummonerData.Summoner.ProfileIconId == -1 || loginPacket.AllSummonerData.Summoner.ProfileIconId == 1)
                {
                    double[] ids = new double[Convert.ToInt32(sumId)];
                    string icons = await connection.GetSummonerIcons(ids);
                    List<int> availableIcons = new List<int> { };
                    var random = new Random();
                    for (int i = 0; i < 29; i++)
                    {
                        availableIcons.Add(i);
                    }
                    foreach (var id in icons)
                    {
                        availableIcons.Add(Convert.ToInt32(id));
                    }
                    int index = random.Next(availableIcons.Count);
                    int randomIcon = availableIcons[index];
                    await connection.UpdateProfileIconId(randomIcon);
                }
                updateStatus("Logged in as " + loginPacket.AllSummonerData.Summoner.Name + " @ level " + loginPacket.AllSummonerData.SummonerLevel.Level, Accountname);
                availableChampsArray = await connection.GetAvailableChampions();
                PlayerDTO player = await connection.CreatePlayer();
                if (loginPacket.ReconnectInfo != null && loginPacket.ReconnectInfo.Game != null)
                {
                    connection_OnMessageReceived(sender, (object)loginPacket.ReconnectInfo.PlayerCredentials);
                }
                else
                    connection_OnMessageReceived(sender, (object)new EndOfGameStats());
            })).Start();
        }
        private void connection_OnError(object sender, LoLLauncher.Error error)
        {
            if (error.Message.Contains("is not owned by summoner"))
            {
                return;
            }
            else if (error.Message.Contains("Your summoner level is too low to select the spell"))
            {
                var random = new Random();
                var spellList = new List<int> { 13, 6, 7, 10, 1, 11, 21, 12, 3, 14, 2, 4 };

                int index = random.Next(spellList.Count);
                int index2 = random.Next(spellList.Count);

                int randomSpell1 = spellList[index];
                int randomSpell2 = spellList[index2];

                if (randomSpell1 == randomSpell2)
                {
                    int index3 = random.Next(spellList.Count);
                    randomSpell2 = spellList[index3];
                }

                int Spell1 = Convert.ToInt32(randomSpell1);
                int Spell2 = Convert.ToInt32(randomSpell2);
                return;
            }
            updateStatus("error received:\n" + error.Message, Accountname);
        }
        private void connection_OnDisconnect(object sender, EventArgs e)
        {
            Core.connectedAccs -= 1;
            Console.Title = " Current Connected: " + Core.connectedAccs;
            updateStatus("Disconnected", Accountname);
        }
        private void connection_OnConnect(object sender, EventArgs e)
        {
            Core.connectedAccs += 1;
            Console.Title = " Current Connected: " + Core.connectedAccs;
        }
        private void updateStatus(string status, string accname)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(string.Concat(new object[3]
              {
                (object) "[",
                (object) DateTime.Now,
                (object) "] "
              }));
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(string.Concat(new object[3]
              {
                (object) "[",
                (object) accname,
                (object) "] "
              }));
            Console.Write(status + "\n");
            
        }
        private void levelUp()
        {
            updateStatus("Level Up: " + sumLevel, Accountname);
            rpBalance = loginPacket.RpBalance;
            if (sumLevel >= Core.maxLevel)
            {
                connection.Disconnect();
                if (!connection.IsConnected())
                {
                    Core.lognNewAccount();
                }
            }
            if (rpBalance == 400.0 && Core.buyBoost)
            {
                updateStatus("Buying XP Boost", Accountname);
                try
                {
                    Task t = new Task(buyBoost);
                    t.Start();
                }
                catch (Exception exception)
                {
                    updateStatus("Couldn't buy RP Boost.\n" + exception, Accountname);
                }
            }
        }
        private async void buyBoost()
        {
            try
            {
                string url = await connection.GetStoreUrl();
                HttpClient httpClient = new HttpClient();
                Console.WriteLine(url);
                await httpClient.GetStringAsync(url);

                string storeURL = "https://store." + "EUW" + "1.lol.riotgames.com/store/tabs/view/boosts/1";
                await httpClient.GetStringAsync(storeURL);

                string purchaseURL = "https://store." + "EUW" + "1.lol.riotgames.com/store/purchase/item";

                List<KeyValuePair<string, string>> storeItemList = new List<KeyValuePair<string, string>>();
                storeItemList.Add(new KeyValuePair<string, string>("item_id", "boosts_2"));
                storeItemList.Add(new KeyValuePair<string, string>("currency_type", "rp"));
                storeItemList.Add(new KeyValuePair<string, string>("quantity", "1"));
                storeItemList.Add(new KeyValuePair<string, string>("rp", "260"));
                storeItemList.Add(new KeyValuePair<string, string>("ip", "null"));
                storeItemList.Add(new KeyValuePair<string, string>("duration_type", "PURCHASED"));
                storeItemList.Add(new KeyValuePair<string, string>("duration", "3"));
                HttpContent httpContent = new FormUrlEncodedContent(storeItemList);
                await httpClient.PostAsync(purchaseURL, httpContent);

                updateStatus("Bought 'XP Boost: 3 Days'!", Accountname);
                httpClient.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        private String FindLoLExe()
        {
            String installPath = ipath;
            if (installPath.Contains("notfound"))
                return installPath;
            installPath += @"RADS\solutions\lol_game_client_sln\releases\";
            installPath = Directory.EnumerateDirectories(installPath).OrderBy(f => new DirectoryInfo(f).CreationTime).Last();
            installPath += @"\deploy\";
            return installPath;
        }

        public Thread InjectThread { get; set; }
    }
}