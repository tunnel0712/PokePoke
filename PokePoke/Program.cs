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
            public bool IsBottom = false;
            public bool IsTane = false;
            public bool IsPsychic = false;

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

            // 一枚引く
            while (true)
            {
                int index = random.Next(Deck.Count); // 山札からランダムなインデックスを取得
                if (!Deck[index].IsBottom)
                {
                    hand.Add(Deck[index]); // カードを手札に追加
                    Deck.RemoveAt(index); // 山札からそのカードを削除
                    break;
                }
                else
                {
                    continue;
                }
            }

            // モンスターボール使用
            hand = MonsterBall(hand);
            
            // 幻の石板使用
            hand = MysticalTablet(hand);
            
            // オーキド使用
            hand = Okid(hand);
            
            // モンスターボール使用
            hand = MonsterBall(hand);
            
            // 幻の石板使用
            hand = MysticalTablet(hand);
            
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
                    while (true)
                    {
                        int index = random.Next(Deck.Count); // 山札からランダムなインデックスを取得
                        // 底でなかったら
                        if (!Deck[index].IsBottom)
                        {
                            result.Add(Deck[index]); // カードを手札に追加
                            Deck.RemoveAt(index); // 山札からそのカードを削除
                            index = random.Next(Deck.Count);
                            result.Add(Deck[index]); // カードを手札に追加
                            Deck.RemoveAt(index); // 山札からそのカードを削除
                            Okided = true;
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
            
            // 底フラグの解除
            foreach(Card card in Deck)
            {
                card.IsBottom = false;
            }

            return result;
        }

        /// <summary>
        /// 幻の石板
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        public static List<Card> MysticalTablet(List<Card> hand)
        {
            Random random = new Random();

            List<Card> result = new List<Card>();

            // ドロー
            foreach (Card card in hand)
            {
                if (card.Type == CardType.MysticalTablet)
                {
                    int index = random.Next(Deck.Count); // 山札からランダムなインデックスを取得
                    if (Deck[index].IsPsychic)
                    {
                        result.Add(Deck[index]); // カードを手札に追加
                        Deck.RemoveAt(index); // 山札からそのカードを削除
                    }
                    else
                    {
                        Deck[index].IsBottom = true; // 底フラグを設定
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
            public string ToResultString() { return this.ToName() + "確率 " + this.GetResult() + " %"; }
        }

        /// <summary>
        /// 種ポケモン初手の例
        /// </summary>
        public class FastestOne : CheckPoint
        {
            public FastestOne(int count) : base(count) { }
            public bool FirstTime { get; set; }
            public override bool IsFullfill()
            {
                return this.FirstTime;
            }
            public override void Reset()
            {
                this.FirstTime = false;
            }
            public override string ToName()
            {
                return "種ポケモンが初手で来る";
            }
        }

        /// <summary>
        /// 最速2進化の例
        /// </summary>
        public class FastestStage2 : CheckPoint
        {
            public FastestStage2(int count) : base(count) { }
            public bool FirstTimeTane { get; set; }
            public bool SecondTimeStage1 { get; set; }
            public bool ThirdTimeStage2 { get; set; }
            public override bool IsFullfill()
            {
                return this.FirstTimeTane && this.SecondTimeStage1 && this.ThirdTimeStage2;
            }
            public override void Reset()
            {
                this.FirstTimeTane = this.SecondTimeStage1 = this.ThirdTimeStage2 = false;
            }
            public override string ToName()
            {
                return "2進化ポケモンが最速で完成する";
            }
        }

        /// <summary>
        /// 一枚引く
        /// </summary>
        /// <param name="hands"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HaveOne(this List<Card> hands, CardType type)
        {
            return hands.Any(h => h.Type == type);
        }
        /// <summary>
        /// 二枚引く
        /// </summary>
        /// <param name="hands"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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
            // セレビィ
            Celebi,
            // ジャローダ
            Tsutaja,
            Jaropi,
            Jaroda,
            // ナッシー
            Tamatama,
            Nassy,
            // ミュウ
            Mew,
            // ミュウツー
            Mewtwo,
            // サーナイト
            Rartos,
            Kirlia,
            Sernight,
            //
            Okido,
            PokeBall,
            MysticalTablet,
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
                // ミュウ
                new Card { Type = CardType.Mew, IsPsychic = true, IsTane = true },
                // ミュウツー
                new Card { Type = CardType.Mewtwo, IsPsychic = true, IsTane = true },
                new Card { Type = CardType.Mewtwo, IsPsychic = true, IsTane = true },
                // サーナイト
                new Card { Type = CardType.Rartos, IsPsychic = true, IsTane = true },
                new Card { Type = CardType.Rartos, IsPsychic = true, IsTane = true },
                new Card { Type = CardType.Kirlia, IsPsychic = true },
                new Card { Type = CardType.Kirlia, IsPsychic = true },
                new Card { Type = CardType.Sernight, IsPsychic = true },
                new Card { Type = CardType.Sernight, IsPsychic = true },
                // 石板
                new Card { Type = CardType.MysticalTablet },
                new Card { Type = CardType.MysticalTablet },
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
        public class FastestSernight : FastestStage2 
        {
            public FastestSernight(int count) : base(count) { }
            public override string ToName()
            {
                return "サーナイトが最速で完成する";
            }
        }
        public class FastestMewtow : FastestOne
        {
            public FastestMewtow(int count) : base(count) { }
            public override string ToName() 
            {
                return "ミュウツーが初手で来る";
            }
        }
        public class FastestSernightAndMewtow : CheckPoint
        {
            public FastestSernightAndMewtow(int count) : base(count) { }
            public override string ToName()
            {
                return "ミュウツーが初手で来てサーナイトも最速で完成する";
            }
        }
        #endregion

        static void Main(string[] args)
        {
            // 試行回数
            int count = 100000;

            #region <チェック項目のリスト>
            FastestSernight fastestSernight = new FastestSernight(count);
            FastestMewtow fastestMewtow = new FastestMewtow(count);
            FastestSernightAndMewtow fastestSernightAndMewtow = new FastestSernightAndMewtow(count);
            #endregion

            for (int i = 0; i < count; i++)
            {
                #region <初期リセット>
                Deck = CreateDeck(); // 山札をリセット
                List<Card> hands = FirstDraw();
                #endregion

                #region <チェック項目のリセット>
                fastestSernight.Reset();
                fastestMewtow.Reset();
                #endregion

                // 1ターン目
                hands = DrawOne(hands);
                #region <チェック項目の更新>
                fastestSernight.FirstTimeTane = hands.HaveOne(CardType.Rartos);
                fastestMewtow.FirstTime = hands.HaveOne(CardType.Mewtwo);
                #endregion

                // 2ターン目
                hands = DrawOne(hands);
                #region <チェック項目の更新>
                fastestSernight.SecondTimeStage1 = hands.HaveOne(CardType.Kirlia);
                #endregion

                // 3ターン目
                hands = DrawOne(hands);
                #region <チェック項目の更新>
                fastestSernight.ThirdTimeStage2 = hands.HaveOne(CardType.Sernight);
                #endregion

                // 4ターン目
                hands = DrawOne(hands);

                #region <チェック項目の更新>
                fastestSernight.Update();
                fastestMewtow.Update();
                fastestSernightAndMewtow.Update(fastestSernight.IsFullFilled && fastestMewtow.IsFullFilled);
                #endregion
            }

            #region <結果の出力>
            Console.WriteLine(fastestSernightAndMewtow.ToResultString());
            #endregion
        }
    }
}
