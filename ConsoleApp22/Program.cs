using System;
using System.ComponentModel.Design;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp22
{
    public enum GameScene
    {
        StartScene,
        PlayerStatScene,
        InventoryScene,
        ClothScene,
        ShopScene,
        PurchaseScene
    }
    internal class Program
    {
        static GameScene gameState = GameScene.StartScene;
        static Player player = new Player(1, "소연", "전사", 17, 10, 100, 1500);


        static void Main(string[] args)
        {
            while (true)
            {
                switch (gameState)
                {
                    case GameScene.StartScene:
                        ShowStartScene();
                        break;

                    case GameScene.PlayerStatScene:
                        ShowPlayerStat();
                        break;

                    case GameScene.InventoryScene:
                        ShowInventoryScene();
                        break;

                    case GameScene.ClothScene:
                        ShowClothScene();
                        break;

                    case GameScene.ShopScene:
                        ShowShopScene();
                        break;

                        //case GameScene.PurchaseScene:
                        //   ShowPurchaseScene();
                        //  break;
                }
            }
        }


        static void ShowStartScene()
        {

            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");

            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 휴식하기");


            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1": gameState = GameScene.PlayerStatScene; break;
                case "2": gameState = GameScene.InventoryScene; break;
                case "3": gameState = GameScene.ShopScene; break;
                case "4": gameState = GameScene.PlayerStatScene; break; // 도전기능
                case "5": gameState = GameScene.PlayerStatScene; break; // 도전기능

                default:
                    Console.WriteLine("잘못된 입력입니다.");

                    break;

            }

            return;

        }
        static void ShowPlayerStat()
        {
            Console.Clear();

            // 플레이어스탯

            Console.WriteLine("상태보기\n캐릭터의 정보가 표시됩니다.");
            Console.WriteLine($"Lv. {player.Lv}");
            Console.WriteLine($"{player.Name} ( {player.Job} )");
            Console.WriteLine($"공격력 : {player.Atk}");
            Console.WriteLine($"방어력 : {player.Def}");
            Console.WriteLine($"체 력 : {player.Hp}");
            Console.WriteLine($"Gold : {player.Gold}");

            Console.WriteLine("0. 나가기");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            if (Console.ReadLine() == "0")
            {
                gameState = GameScene.StartScene;
                return;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");

            }


        }
        static void ShowInventoryScene()
        {
            Console.Clear();

            // 인벤토리 , 아이템불러오기
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다" + UI.newLine + "[아이템 목록]");

            //인벤토리!!



            Console.WriteLine("1. 장착 관리\n0. 나가기" + UI.newLine + "원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            string input = Console.ReadLine();

            if (input == "0") { }

        }
        static void ShowClothScene()
        {
            Console.Clear();

            // 장착
        }
        static void ShowShopScene()
        {
            Console.Clear();
            Console.WriteLine("상점\n필요한 아이템을 얻을 수 있는 상점입니다." + UI.newLine + "[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine("\n[아이템 목록]");

            foreach (var item in Item.items)
            {
                if (item.ItemAttack == 0)
                {
                    Console.WriteLine($"-{item.ItemName}   | 방어력 +{item.ItemDefense}   | {item.ItemDescription}   | {item.ItemPrice} G");
                }
                else
                {
                    Console.WriteLine($"-{item.ItemName}   | 공격력 +{item.ItemAttack}   | {item.ItemDescription}   | {item.ItemPrice} G");
                }
            }

            Console.WriteLine(UI.newLine + "1. 아이템 구매\n0. 나가기");
            Console.Write(UI.newLine + "원하시는 행동을 입력해주세요.\n>> ");
            string input = Console.ReadLine();
            if (input == "0") { gameState = GameScene.StartScene; }
            if (input == "1") { Purchase.ItemList(player); }

            // 상점
        }


    }

    public class Purchase
    {
        static bool isSoldOut = true;

        public static void ItemList(Player player, Item item)
        {
            Console.Clear();
            Console.WriteLine("상점\n필요한 아이템을 얻을 수 있는 상점입니다." + UI.newLine + "[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine("\n[아이템 목록]");

            bool isSoldOut = player.inventory.Contains(item);


            foreach (var shopItem in Item.items)
            {
               

                if (item.ItemAttack == 0)
                {
                    if (isSoldOut)
                        Console.WriteLine($"-{item.ItemName}   | 방어력 +{item.ItemDefense}   | {item.ItemDescription}   | 판매 완료 G");
                    else Console.WriteLine($"-{item.ItemName}   | 방어력 +{item.ItemDefense}   | {item.ItemDescription}   | {item.ItemPrice} G");
                }
                else
                {
                    if (isSoldOut)
                        Console.WriteLine($"-{item.ItemName}   | 공격력 +{item.ItemAttack}   | {item.ItemDescription}   | 판매 완료 G");
                    else Console.WriteLine($"-{item.ItemName}   | 공격력 +{item.ItemAttack}   | {item.ItemDescription}   | {item.ItemPrice} G");
                }
            }

            if (int.TryParse(Console.ReadLine(), out int input))
            {
                if (input == 0) return; // 스타트로나가기


                if (input > 0 && input <= Item.items.Count)
                {
                    PurchaseItem();

                }
                else { Console.WriteLine("잘못된 입력입니다."); }
            }

        }
        public static void PurchaseItem(Player player, Item item)
        {
            if (isSoldOut)
                Console.WriteLine("이미 구매했습니다.");
            
            else if (player.Gold > item.ItemPrice)
                {
                    Console.WriteLine("구매");
                    player.Gold -= item.ItemPrice;
                    player.inventory.Add(item);
                }
            else
                {
                    Console.WriteLine("돈이 부족합니다.");

                }
            }
        } 
    }

        public class UI
        {
            public static string newLine = Environment.NewLine + Environment.NewLine;
        }

        public class Player
        {
            public int Lv { get; set; }
            public string Name { get; set; }
            public string Job { get; set; }
            public int Atk { get; set; }
            public int Def { get; set; }
            public int Hp { get; set; }
            public double Gold { get; set; }

            public List<Item> inventory;

            public Player(int lv, string name, String job, int atk, int def, int hp, double gold)
            {
                Lv = lv;
                Job = job;
                Name = name;
                Atk = atk;
                Def = def;
                Hp = hp;
                Gold = gold;

                inventory = new List<Item>();
            }

            public void PlayerInfo()
            {
                Console.WriteLine($"Lv. {Lv}");
                Console.WriteLine($"{Name} ( {Job} )");
                Console.WriteLine($"공격력 : {Atk}");
                Console.WriteLine($"방어력 : {Def}");
                Console.WriteLine($"체 력 : {Hp}");
                Console.WriteLine($"Gold : {Gold}");
            }
        }
        public class Item
        {
            public string ItemName { get; set; }
            public int ItemAttack { get; set; }
            public int ItemDefense { get; set; }
            public string ItemDescription { get; set; }
            public double ItemPrice { get; set; }

            public bool isSoldOut { get; set; }

            public Item(String itemName, int itemAttack, int itemDefense, string itemDescription, double itemPrice)
            {
                ItemName = itemName;
                ItemAttack = itemAttack;
                ItemDefense = itemDefense;
                ItemDescription = itemDescription;
                ItemPrice = itemPrice;
                isSoldOut = false;
            }

            public static List<Item> items = new List<Item>
        {
                new Item("수련자 갑옷", 0, 5, "수련에 도움을 주는 갑옷입니다.", 1000),
                new Item("무쇠갑옷", 0,9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2500 ),
                new Item("스파르타의 갑옷", 0,15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500),
                new Item("낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검 입니다.", 600),
                new Item("청동 도끼", 5, 0, "어디선가 사용됐던거 같은 도끼입니다.", 1500 ),
                new Item("스파르타의 창", 7, 0, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3000)
        };
        }
    }
}
