namespace PokePoke
{
    internal static class Program
    {
        #region <共通メソッド>
        public static List<Card> Deck;

        /// <summary>
        /// カードの定義
        /// </summary>
        public class Card
        {
            public CardType Type;
            public bool IsTane = false;

            public override string ToString()
            {
                return this.Type.ToString();
            }
        }

        /// <summary>
        /// 初期ドロー
        /// </summary>
        /// <returns></returns>
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
            if (hand.Any(h => h.IsTane))
            {
                return hand;
            }
            else
            {
                return FirstDraw();
            }
        }

        /// <summary>
        /// 一枚引く
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
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

        /// <summary>
        /// オーキド博士の使用
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
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

        /// <summary>
        /// モンスターボールの使用
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        public static List<Card> MonsterBall(List<Card> hand)
        {
            Random random = new Random();

            List<Card> result = new List<Card>();

            // ドロー
            foreach (Card card in hand)
            {
                if (card.Type == CardType.PokeBall && Deck.Any(card => card.IsTane))
                {
                    while (true)
                    {
                        int index = random.Next(Deck.Count); // 山札からランダムなインデックスを取得
                        if (Deck[index].IsTane)
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

        /// <summary>
        /// 判定条件
        /// </summary>
        public class CheckPoint
        {
            public CheckPoint(int count) { this.Count = count; }
            public int Count { get; set; }
            public double Fullfill = 0;
            public virtual bool IsFullfill() { throw new NotImplementedException(); }
            public void Update() { if (this.IsFullfill()) { this.Fullfill++; } }
            public virtual void Reset() { throw new NotImplementedException(); }
            public double GetResult() { return this.Fullfill / this.Count * 100; } 
            public virtual string ToName() { throw new NotImplementedException(); }
        }
        #endregion

        #region <デッキごとに変える>
        /// <summary>
        /// カード種別
        /// </summary>
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
        /// <summary>
        /// デッキの作成
        /// </summary>
        /// <returns></returns>
        public static List<Card> CreateDeck()
        {
            List<Card> Deck = new List<Card>
            {
                new Card { Type = CardType.Fire, IsTane = true  },
                new Card { Type = CardType.Fire, IsTane = true },
                //
                new Card { Type = CardType.Hitokage, IsTane = true },
                new Card { Type = CardType.Hitokage, IsTane = true },
                new Card { Type = CardType.Rizard },
                new Card { Type = CardType.Rizard },
                new Card { Type = CardType.Rizardon },
                new Card { Type = CardType.Rizardon },
                ////
                new Card { Type = CardType.Gardy, IsTane = true },
                new Card { Type = CardType.Gardy, IsTane = true },
                new Card { Type = CardType.Windy },
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
        #endregion

        #region <条件ごとに変える>
        public class FastestRezardon : CheckPoint
        {
            public FastestRezardon(int count) : base(count) { }
            public bool FirstTimeHitokage { get; set; }
            public bool SecondTimeRizard { get; set; }
            public bool ThirdTimeRizardon { get; set; }
            public override bool IsFullfill()
            {
                return this.FirstTimeHitokage && this.SecondTimeRizard && this.ThirdTimeRizardon;
            }
            public override void Reset()
            {
                this.FirstTimeHitokage = this.SecondTimeRizard = this.ThirdTimeRizardon = false;
            }
            public override string ToName()
            {
                return "リザードンが最速で完成する確率";
            }
        }
        #endregion

        static void Main(string[] args)
        {
            // 試行回数
            int count = 100000;

            #region <チェック項目のリスト>
            FastestRezardon fastestRezardon = new FastestRezardon(count);
            #endregion

            for (int i = 0; i < count; i++)
            {
                #region <初期リセット>
                Deck = CreateDeck(); // 山札をリセット
                List<Card> hands = FirstDraw();
                #endregion

                #region <チェック項目のリセット>
                fastestRezardon.Reset();
                #endregion

                // 1ターン目
                hands = DrawOne(hands);
                #region <チェック項目の更新>
                fastestRezardon.FirstTimeHitokage = hands.Any(h => h.Type == CardType.Hitokage);
                #endregion

                // 2ターン目
                hands = DrawOne(hands);
                #region <チェック項目の更新>
                fastestRezardon.SecondTimeRizard = hands.Any(h => h.Type == CardType.Rizard);
                #endregion

                // 3ターン目
                hands = DrawOne(hands);
                #region <チェック項目の更新>
                fastestRezardon.ThirdTimeRizardon = hands.Any(h => h.Type == CardType.Rizardon);
                #endregion

                // 4ターン目
                hands = DrawOne(hands);

                #region <チェック項目の更新>
                fastestRezardon.Update();
                #endregion
            }

            #region <結果の出力>
            Console.WriteLine(fastestRezardon.ToName());
            Console.WriteLine(fastestRezardon.GetResult());
            #endregion

            Console.ReadKey();
        }
    }
}
