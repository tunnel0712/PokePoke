namespace PokePoke
{
    internal static class Program
    {
        public enum CardType
        {
            Fire,
            Hitokage,
            Rizard,
            Rizardon,
            Gardy,
            Windy,
            Okido,
            PokeBall,
            Other,
        }

        public static List<Card> Deck;

        public static List<Card> CreateDeck()
        {
            List<Card> Deck = new List<Card>
            {
                new Card { Type = CardType.Fire },
                new Card { Type = CardType.Fire },
                //
                new Card { Type = CardType.Hitokage },
                new Card { Type = CardType.Hitokage },
                new Card { Type = CardType.Rizard },
                new Card { Type = CardType.Rizard },
                new Card { Type = CardType.Rizardon },
                new Card { Type = CardType.Rizardon },
                ////
                new Card { Type = CardType.Gardy },
                new Card { Type = CardType.Windy },
                new Card { Type = CardType.Gardy },
                new Card { Type = CardType.Windy },

                //
                new Card { Type = CardType.Okido },
                new Card { Type = CardType.Okido },
                new Card { Type = CardType.PokeBall },
                new Card { Type = CardType.PokeBall },
            };
            while (Deck.Count() < 20)
            {
                Deck.Add(new Card { Type = CardType.Other });
            }

            return Deck;
        }

        public class Card
        {
            public CardType Type;

            public override string ToString()
            {
                return this.Type.ToString();
            }
        }

        public static List<Card> FirstDraw()
        {
            // 枚数調整
            Deck = CreateDeck();

            // 初期ドロー
            Random random = new Random();
            List<Card> hand = new List<Card>();
            for (int i = 0; i < 5; i++)
            {
                int index = random.Next(Deck.Count); // 山札からランダムなインデックスを取得
                hand.Add(Deck[index]); // カードを手札に追加
                Deck.RemoveAt(index); // 山札からそのカードを削除
            }

            // マリガン
            if (hand.ContainTane())
            {
                return hand;
            }
            else
            {
                return FirstDraw();
            }
        }

        public static List<Card> DrawOne(List<Card> hand)
        {
            // ドロー
            Random random = new Random();
            int index = random.Next(Deck.Count); // 山札からランダムなインデックスを取得
            hand.Add(Deck[index]); // カードを手札に追加
            Deck.RemoveAt(index); // 山札からそのカードを削除

            // モンスターボール使用
            hand = MonsterBall(hand);

            // オーキド使用
            hand = Okid(hand);

            // モンスターボール使用
            hand = MonsterBall(hand);

            return hand;
        }

        public static List<Card> Okid(List<Card> hand)
        {
            Random random = new Random();

            List<Card> result = new List<Card>();

            bool Okided = false;

            // ドロー
            foreach (Card card in hand)
            {
                if (card.Type == CardType.Okido && Okided == false)
                {
                    int index = random.Next(Deck.Count); // 山札からランダムなインデックスを取得
                    result.Add(Deck[index]); // カードを手札に追加
                    Deck.RemoveAt(index); // 山札からそのカードを削除
                    index = random.Next(Deck.Count);
                    result.Add(Deck[index]); // カードを手札に追加
                    Deck.RemoveAt(index); // 山札からそのカードを削除
                    Okided = true;
                }
                else
                {
                    result.Add(card);
                }
            }

            return result;
        }

        public static List<Card> MonsterBall(List<Card> hand)
        {
            Random random = new Random();

            List<Card> result = new List<Card>();

            // ドロー
            foreach (Card card in hand)
            {
                if (card.Type == CardType.PokeBall && Deck.Any(card => card.IsTane()))
                {
                    while (true)
                    {
                        int index = random.Next(Deck.Count); // 山札からランダムなインデックスを取得
                        if (Deck[index].IsTane())
                        {
                            result.Add(Deck[index]); // カードを手札に追加
                            Deck.RemoveAt(index); // 山札からそのカードを削除
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    result.Add(card);
                }
            }

            return result;
        }

        public static bool ContainTane(this List<Card> hand)
        {
            return hand.Any(card =>
            {
                return card.IsTane();
            });
        }

        public static bool IsTane(this Card card)
        {
            return card.Type == CardType.Fire ||
                   card.Type == CardType.Gardy ||
                   card.Type == CardType.Hitokage;
        }
            

        public class Saisoku
        {
            public bool firstTimeFire = false;
            public bool firstTimeHitokage = false;
            public bool secondTimeRizard = false;
            public bool thirdTimeRizardon = false;

            public void Reset()
            {
                firstTimeFire = false;
                firstTimeHitokage = false;
                secondTimeRizard = false;
                thirdTimeRizardon = false;
            }

            public bool FulfillSaisoku()
            {
                return firstTimeHitokage && secondTimeRizard && thirdTimeRizardon;
            }

            public bool FulfillFireSaisoku()
            {
                return firstTimeFire && firstTimeHitokage && secondTimeRizard && thirdTimeRizardon;
            }
        }

        public class Windy
        {
            public int GardyTime = -1;
            public int WindyTime = -1;

            public void Clear()
            {
                GardyTime = -1;
                WindyTime = -1;
            }

            public void SetGardyTime(int time)
            {
                if(GardyTime == -1) { this.GardyTime = time; }
            }
            public void SetWindyTime(int time)
            {
                if (WindyTime == -1) { this.WindyTime = time; }
            }

            public bool IsOneTurn()
            {
                return (GardyTime != -1 && WindyTime != -1) && GardyTime == 0 && WindyTime <= 1;
            }
            public bool IsTwoTurn()
            {
                return (GardyTime != -1 && WindyTime != -1) && GardyTime <= 1 && WindyTime <= 2;
            }
            public bool IsThreeTurn()
            {
                return (GardyTime != -1 && WindyTime != -1) && GardyTime <= 2 && WindyTime <= 3;
            }
            public bool IsFourTurn()
            {
                return (GardyTime != -1 && WindyTime != -1) && GardyTime <= 3 && WindyTime <= 4;
            }
        }

        static void Main(string[] args)
        {
            //
            double Fire = 0;
            double NoFire = 0;

            //
            double FireFire = 0;
            double NoFireFire = 0;

            //
            Saisoku saisoku = new Saisoku();
            double Saisoku = 0;
            
            //
            double FireSaisoku = 0;
            
            //
            Windy windy = new Windy();
            double oneWindy = 0;
            double twoWindy = 0;
            double threeWindy = 0;
            double fourWindy = 0;
            
            int count = 100000;
            for (int i = 0; i < count; i++)
            {
                Deck = CreateDeck(); // 山札をリセット
                List<Card> hands = FirstDraw();

                // フラグリセット
                saisoku.Reset();
                windy.Clear();

                // 1ターン目
                hands = DrawOne(hands);
                if (hands.Count(hand => hand.Type == CardType.Fire) >= 1)
                {
                    Fire++;
                }
                else
                {
                    NoFire++;
                }
                if (hands.Count(hand => hand.Type == CardType.Fire) == 2)
                {
                    FireFire++;
                }
                else
                {
                    NoFireFire++;
                }
                saisoku.firstTimeHitokage = hands.Any(h => h.Type == CardType.Hitokage);
                saisoku.firstTimeFire = hands.Any(h => h.Type == CardType.Fire);
                if (hands.Any(h => h.Type == CardType.Gardy)) { windy.SetGardyTime(0); }
                if (hands.Any(h => h.Type == CardType.Windy)) { windy.SetWindyTime(0); }

                // 2ターン目
                hands = DrawOne(hands);
                saisoku.secondTimeRizard = hands.Any(h => h.Type == CardType.Rizard);
                if (hands.Any(h => h.Type == CardType.Gardy)) { windy.SetGardyTime(1); }
                if (hands.Any(h => h.Type == CardType.Windy)) { windy.SetWindyTime(1); }

                // 3ターン目
                hands = DrawOne(hands);
                saisoku.thirdTimeRizardon = hands.Any(h => h.Type == CardType.Rizardon);
                if (hands.Any(h => h.Type == CardType.Gardy)) { windy.SetGardyTime(2); }
                if (hands.Any(h => h.Type == CardType.Windy)) { windy.SetWindyTime(2); }

                // 4ターン目
                hands = DrawOne(hands);
                if (hands.Any(h => h.Type == CardType.Gardy)) { windy.SetGardyTime(3); }
                if (hands.Any(h => h.Type == CardType.Windy)) { windy.SetWindyTime(3); }

                // カウント
                if (saisoku.FulfillSaisoku()) { Saisoku++; }
                if (saisoku.FulfillFireSaisoku()) { FireSaisoku++; }
                if (windy.IsOneTurn()) { oneWindy++; }
                if (windy.IsTwoTurn()) { twoWindy++; }
                if (windy.IsThreeTurn()) { threeWindy++; }
                if (windy.IsFourTurn()) { fourWindy++; }
            }

            Console.WriteLine("初手にファイヤーが来る確率");
            Console.WriteLine((Fire / count) * 100);

            // 
            Console.WriteLine("初手にファイヤーが二枚来る確率");
            Console.WriteLine((FireFire/count)*100);

            //
            Console.WriteLine("リザードンが最速で来る確率");
            Console.WriteLine((Saisoku / count) * 100);

            // 初手ファイヤーが来てリザードンが最速で立つ確率
            Console.WriteLine("初手ファイヤーが来てリザードンが最速で立つ確率");
            Console.WriteLine((FireSaisoku / count) * 100);

            //
            Console.WriteLine("ウィンディ1ターン");
            Console.WriteLine((oneWindy / count) * 100);
            Console.WriteLine("ウィンディ2ターン");
            Console.WriteLine((twoWindy / count) * 100);
            Console.WriteLine("ウィンディ3ターン");
            Console.WriteLine((threeWindy / count) * 100);
            Console.WriteLine("ウィンディ4ターン");
            Console.WriteLine((fourWindy / count) * 100);


            while (true) { }
        }
    }
}
