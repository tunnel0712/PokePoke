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
            public bool IsFullFilled = false;
            public virtual bool IsFullfill() { throw new NotImplementedException(); }
            public void Update() 
            {
                this.IsFullFilled = this.IsFullfill();
                if (this.IsFullFilled) 
                { 
                    this.Fullfill++; 
                } 
            }
            public void Update(bool fullfill) 
            {
                this.IsFullFilled = fullfill;
                if (this.IsFullFilled) 
                { 
                    this.Fullfill++; 
                } 
            }
            public virtual void Reset() { throw new NotImplementedException(); }
            public double GetResult() { return Math.Round((this.Fullfill / this.Count) * 100, 2); } 
            public virtual string ToName() { throw new NotImplementedException(); }
        }

        public static bool HaveOne(this List<Card> hands, CardType type)
        {
            return hands.Any(h => h.Type == type);
        }
        public static bool HaveTwo(this List<Card> hands, CardType type)
        {
            return hands.Count(h => h.Type == type) == 2;
        }
        #endregion

        #region <デッキごとに変える>
        /// <summary>
        /// カード種別
        /// </summary>
        public enum CardType
        {
            Celebi,
            //
            Tsutaja,
            Jaropi,
            Jaroda,
            //
            Tamatama,
            Nassy,
            //
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
                // セレビィ
                new Card { Type = CardType.Celebi, IsTane = true },
                new Card { Type = CardType.Celebi, IsTane = true },
                // ナッシー
                new Card { Type = CardType.Tamatama, IsTane = true },
                new Card { Type = CardType.Tamatama, IsTane = true },
                new Card { Type = CardType.Nassy },
                new Card { Type = CardType.Nassy },
                // ジャローダ
                // new Card { Type = CardType.Tsutaja, IsTane = true },
                new Card { Type = CardType.Tsutaja, IsTane = true },
                new Card { Type = CardType.Jaropi, },
                new Card { Type = CardType.Jaropi, },
                new Card { Type = CardType.Jaroda, },
                new Card { Type = CardType.Jaroda, },
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
        public class FastestJaroda : CheckPoint
        {
            public FastestJaroda(int count) : base(count) { }
            public bool FirstTimeTsutaja { get; set; }
            public bool SecondTimeJaropi { get; set; }
            public bool ThirdTimeJaroda { get; set; }
            public override bool IsFullfill()
            {
                return this.FirstTimeTsutaja && this.SecondTimeJaropi && this.ThirdTimeJaroda;
            }
            public override void Reset()
            {
                this.FirstTimeTsutaja = this.SecondTimeJaropi = this.ThirdTimeJaroda = false;
            }
            public override string ToName()
            {
                return "ジャローダが最速で完成する";
            }
        }
        #endregion

        static void Main(string[] args)
        {
            // 試行回数
            int count = 100000;

            #region <チェック項目のリスト>
            FastestJaroda fastestJaroda = new FastestJaroda(count);
            #endregion

            for (int i = 0; i < count; i++)
            {
                #region <初期リセット>
                Deck = CreateDeck(); // 山札をリセット
                List<Card> hands = FirstDraw();
                #endregion

                #region <チェック項目のリセット>
                fastestJaroda.Reset();
                #endregion

                // 1ターン目
                hands = DrawOne(hands);
                #region <チェック項目の更新>
                fastestJaroda.FirstTimeTsutaja = hands.HaveOne(CardType.Tsutaja);
                #endregion

                // 2ターン目
                hands = DrawOne(hands);
                #region <チェック項目の更新>
                fastestJaroda.SecondTimeJaropi = hands.HaveOne(CardType.Jaropi);
                #endregion

                // 3ターン目
                hands = DrawOne(hands);
                #region <チェック項目の更新>
                fastestJaroda.ThirdTimeJaroda = hands.HaveOne(CardType.Jaroda);
                #endregion

                // 4ターン目
                hands = DrawOne(hands);

                #region <チェック項目の更新>
                fastestJaroda.Update();
                #endregion
            }

            #region <結果の出力>
            Console.WriteLine(fastestJaroda.ToName());
            Console.WriteLine(fastestJaroda.GetResult());
            #endregion

            Console.ReadKey();
        }
    }
}
