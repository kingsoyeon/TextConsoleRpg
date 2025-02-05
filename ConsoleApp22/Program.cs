using Microsoft.VisualBasic;
using System;
using System.ComponentModel.Design;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using static ConsoleApp22.Program;

namespace ConsoleApp22
{
    public enum GameScene
    {
        StartScene,
        PlayerStatScene,
        InventoryScene,
        ClothScene,
        ShopScene,
        PurchaseScene,
        RelaxScene

    }

    internal class Program
    {
        static GameScene gameState = GameScene.StartScene;
        static Player player = new Player(1, "소연", "전사", 17, 10, 100, 1500);
        static List<Item> equipItems = new List<Item>();



        static void Main(string[] args)
        {   
            // 화면전환
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
                    case GameScene.RelaxScene:
                        ShowRelaxScene();
                        break;
                        
                }
            }
        }

        // 시작화면
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
            int input = int.Parse(Console.ReadLine());

            // 화면 전환
            switch (input)
            {
                case 1: gameState = GameScene.PlayerStatScene; break; // 스타트
                case 2: gameState = GameScene.InventoryScene; break; // 인벤토리
                case 3: gameState = GameScene.ShopScene; break; // 상점
                // case 4: gameState = GameScene.PlayerStatScene; break; // 던전(미구현)
                case 5: gameState = GameScene.RelaxScene; break; // 휴식
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadKey();

                    break;
            }

            return;

        }
        // 플레이어 스탯

        static void ShowPlayerStat()
        {
            int addAtk = 0;
            int addDef = 0;


            foreach (var item in equipItems)
            {
                addAtk += item.ItemAttack;
                addDef += item.ItemDefense;
            }

            Console.Clear();

            
            Console.WriteLine("상태보기\n캐릭터의 정보가 표시됩니다.");
            Console.WriteLine($"Lv. {player.Lv}");
            Console.WriteLine($"{player.Name} ( {player.Job} )");

            Console.WriteLine($"공격력 : {player.Atk} (+{addAtk})");
            Console.WriteLine($"방어력 : {player.Def} (+{addDef})");

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
                Console.ReadKey();
            }


        }
        // 인벤토리

        static void ShowInventoryScene()
        {
            Console.Clear();
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다" + UI.newLine + "[아이템 목록]");



            foreach (var item in Item.items)
            {
                string equip = item.IsEquip ? "[E]" : "";
                if (item.isSoldOut)
                {

                    if (item.ItemAttack == 0) { Console.WriteLine($"- {equip}{item.ItemName}   | 방어력 +{item.ItemDefense}   | {item.ItemDescription}   | {item.ItemPrice} G"); }
                    else { Console.WriteLine($"-{equip}{item.ItemName}   | 공격력 +{item.ItemAttack}   | {item.ItemDescription}   | {item.ItemPrice} G"); }
                }
                else
                {
                    Console.WriteLine("\n");
                }
            }

            Console.WriteLine("1. 장착 관리\n0. 나가기" + UI.newLine + "원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            int input = int.Parse(Console.ReadLine());


            if (input == 0) { gameState = GameScene.StartScene; }
            if (input == 1)
            {
                ShowClothScene();
            }
        }

        // 장착 관리
        static void ShowClothScene()
        {
            Console.Clear();

            // 보유 중인 아이템 리스트
            // 장착하고 있는 것은 [E]로 표시
            foreach (var item in Item.items)
            {
                string equip = item.IsEquip ? "[E]" : "";
                // int addAtk = item.IsEquip ? "(+{Item.ItemAttack})" : "";
                if (item.isSoldOut)
                {

                    if (item.ItemAttack == 0) { Console.WriteLine($"- {equip}{item.ItemName}   | 방어력 +{item.ItemDefense}   | {item.ItemDescription}   | {item.ItemPrice} G"); }
                    else { Console.WriteLine($"-{equip}{item.ItemName}   | 공격력 +{item.ItemAttack}   | {item.ItemDescription}   | {item.ItemPrice} G"); }
                }
                else
                {
                    Console.WriteLine("\n");
                }
            }
            Console.WriteLine("\n0: 나가기\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            int input = int.Parse(Console.ReadLine());

            // 나가기 하는 경우
            if (input == 0) { gameState = GameScene.StartScene; }

            // 아이템 선택하는 경우
            if (input >= 1 && input <= Item.items.Count)
            {
                Item selectedItem = player.inventory[input - 1];

                // 아이템 장착,해제
                selectedItem.IsEquip = !selectedItem.IsEquip;


                // 장착한 아이템 목록에서 추가, 제외
                if (selectedItem.IsEquip)
                {
                    equipItems.Add(selectedItem);
                }
                else equipItems.Remove(selectedItem);
                   
            }
            else 
            { 
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadKey();
            }

           
        }

        // 상점
        static void ShowShopScene()
        {
            Console.Clear();
            Console.WriteLine("상점\n필요한 아이템을 얻을 수 있는 상점입니다." + UI.newLine + "[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine("\n[아이템 목록]");

            for (int i = 0; i < Item.items.Count; i++)
            {
                var stock = Item.items[i];

                if (stock.ItemAttack == 0)
                {
                    if (stock.isSoldOut) Console.WriteLine($"- {i + 1} {stock.ItemName}   | 방어력 +{stock.ItemDefense}   | {stock.ItemDescription}   | 판매 완료 G");
                    else Console.WriteLine($"- {i + 1} {stock.ItemName}   | 방어력 +{stock.ItemDefense}   | {stock.ItemDescription}   | {stock.ItemPrice} G");

                }
                else
                {
                    if (stock.isSoldOut)
                        Console.WriteLine($"- {i + 1}  {stock.ItemName}   | 공격력 +{stock.ItemAttack}   | {stock.ItemDescription}   | 판매 완료 G");
                    else Console.WriteLine($"- {i + 1} {stock.ItemName}   | 공격력 +{stock.ItemAttack}   | {stock.ItemDescription}   | {stock.ItemPrice} G");
                }
            }

            Console.WriteLine(UI.newLine + "1. 아이템 구매\n0. 나가기");
            Console.Write(UI.newLine + "원하시는 행동을 입력해주세요.\n>> ");
            int input = int.Parse(Console.ReadLine());

            if (input == 0) { gameState = GameScene.StartScene; }
            if (input == 1)
            {
                Console.Clear();
                Console.WriteLine("상점-아이템 구매\n필요한 아이템을 얻을 수 있는 상점입니다." + UI.newLine + "[보유 골드]");
                Console.WriteLine($"{player.Gold} g");
                Console.WriteLine("\n[아이템 목록]");


                for (int i = 0; i < Item.items.Count; i++)
                {
                    var stock = Item.items[i];

                    if (stock.ItemAttack == 0)
                    {
                        if (stock.isSoldOut) Console.WriteLine($"- {i + 1} {stock.ItemName}   | 방어력 +{stock.ItemDefense}   | {stock.ItemDescription}   | 판매 완료 G");
                        else Console.WriteLine($"- {i + 1} {stock.ItemName}   | 방어력 +{stock.ItemDefense}   | {stock.ItemDescription}   | {stock.ItemPrice} G");

                    }
                    else
                    {
                        if (stock.isSoldOut)
                            Console.WriteLine($"- {i + 1}  {stock.ItemName}   | 공격력 +{stock.ItemAttack}   | {stock.ItemDescription}   | 판매 완료 G");
                        else Console.WriteLine($"- {i + 1} {stock.ItemName}   | 공격력 +{stock.ItemAttack}   | {stock.ItemDescription}   | {stock.ItemPrice} G");
                    }
                }

                Console.WriteLine("\n0.나가기");

                Console.WriteLine("원하시는 행동을 입력해주세요.\n>> ");

                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out int inputNum))
                    {   //나가기
                        if (inputNum == 0) { break; }
                        // 아이템 선택 
                        if (inputNum > 0 && inputNum <= Item.items.Count)
                        {
                            Item selectedItem = Item.items[inputNum - 1];
                            // 아이템 품절    
                            if (selectedItem.isSoldOut)
                            { Console.WriteLine("이미 구매한 아이템입니다. 다시 입력해주세요."); }
                            // 돈 충분하면
                            else if (player.Gold >= selectedItem.ItemPrice)
                            {
                                Console.WriteLine("구매를 완료했습니다.");
                                selectedItem.isSoldOut = true;
                                player.Gold -= selectedItem.ItemPrice;

                                player.inventory.Add(selectedItem); // 인벤토리에 추가
                                return;

                            }
                            // 돈부족하면
                            else if (player.Gold < selectedItem.ItemPrice)
                            {
                                Console.WriteLine("돈이 부족합니다.");

                            }
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다. 다시 입력하세요.");
                            Console.ReadKey();
                        }

                    }
                }
            }

        }
        // 도전기능: 휴식하기
        static void ShowRelaxScene()
        {
            Console.Clear();
            Console.WriteLine($"휴식하기\n500 G 를 내면 체력을 회복할 수 있습니다. ( 보유 골드 : {player.Gold} ) ");

            Console.WriteLine("\n1. 휴식하기\n0. 나가기" + UI.newLine + "원하시는 행동을 선택해주세요");
            Console.Write(">> ");
            string input = Console.ReadLine();
            if (input == "0")
            {
                gameState = GameScene.StartScene; 
            }
            else if (input == "1")
            {
                if(player.Gold >= 500) 
                {
                    Console.WriteLine("충분히 휴식했습니다!!!");
                    player.Gold -= 500;
                    player.Hp = 100;
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Gold 가 부족합니다.");
                    Console.ReadKey();
                }
            }

            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadKey();
            }


        }

    }
    // UI
    public class UI
    {
        public static string newLine = Environment.NewLine + Environment.NewLine;
    }

    // Player 관련 정보
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

    // item 관련 정보
    public class Item
    {
        public string ItemName { get; set; }
        public int ItemAttack { get; set; }
        public int ItemDefense { get; set; }
        public string ItemDescription { get; set; }
        public double ItemPrice { get; set; }

        public bool isSoldOut { get; set; }
        public bool IsEquip { get; set; }

        public Item(String itemName, int itemAttack, int itemDefense, string itemDescription, double itemPrice)
        {
            ItemName = itemName;
            ItemAttack = itemAttack;
            ItemDefense = itemDefense;
            ItemDescription = itemDescription;
            ItemPrice = itemPrice;

            isSoldOut = false;
            IsEquip = false;

        }
        // 아이템 리스트 

        public static List<Item> items = new List<Item>
        {
                new Item("수련자 갑옷", 0, 5, "수련에 도움을 주는 갑옷입니다.", 1000),
                new Item("무쇠갑옷", 0,9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2500 ),
                new Item("스파르타의 갑옷", 0,15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500),
                new Item("낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검 입니다.", 600),
                new Item("청동 도끼", 5, 0, "어디선가 사용됐던거 같은 도끼입니다.", 1500 ),
                new Item("스파르타의 창", 7, 0, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3000),
                new Item("샌즈의 눈물", 20, 0, "샌즈가 흘린 눈물입니다.", 99999)
        };
    }
}

  


