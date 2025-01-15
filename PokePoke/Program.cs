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
            Rocon, 
            Kyukon,
            //
            Ponita,
            Gyarop,
            //
            Katsura,
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
                //キュウコン
                new Card { Type = CardType.Rocon, IsTane = true },
                new Card { Type = CardType.Rocon, IsTane = true },
                new Card { Type = CardType.Kyukon },
                new Card { Type = CardType.Kyukon },
                //ギャロップ
                new Card { Type = CardType.Ponita, IsTane = true },
                new Card { Type = CardType.Ponita, IsTane = true },
                new Card { Type = CardType.Gyarop },
                new Card { Type = CardType.Gyarop },
                //カツラ
                new Card { Type = CardType.Katsura },
                new Card { Type = CardType.Katsura },
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
        public class FastestKyucon : CheckPoint
        {
            public FastestKyucon(int count) : base(count) { }
            public bool FirstTimeRocon { get; set; }
            public bool SecondTimeKyucon { get; set; }
            public override bool IsFullfill()
            {
                return this.FirstTimeRocon && this.SecondTimeKyucon;
            }
            public override void Reset()
            {
                this.FirstTimeRocon = this.SecondTimeKyucon = false;
            }
            public override string ToName()
            {
                return "キュウコンが最速で完成する";
            }
        }
        public class FastestGyarop : CheckPoint
        {
            public FastestGyarop(int count) : base(count) { }
            public bool FirstTimePonita { get; set; }
            public bool SecondTimeGyarop { get; set; }
            public override bool IsFullfill()
            {
                return this.FirstTimePonita && this.SecondTimeGyarop;
            }
            public override void Reset()
            {
                this.FirstTimePonita = this.SecondTimeGyarop = false;
            }
            public override string ToName()
            {
                return "ギャロップが最速で完成する";
            }
        }
        public class FastestKatsura : CheckPoint
        {
            public FastestKatsura(int count) : base(count) { }
            public bool SecondTimeKatsura { get; set; }
            public override bool IsFullfill()
            {
                return this.SecondTimeKatsura;
            }
            public override void Reset()
            {
                this.SecondTimeKatsura = false;
            }
            public override string ToName()
            {
                return "カツラが最速で使える";
            }
        }
        public class FullKatsura : CheckPoint
        {
            public FullKatsura(int count) : base(count) { }
            public bool SecondTimeKatsura { get; set; }
            public bool ThirdTimeTwoKatsura { get; set; }
            public override bool IsFullfill()
            {
                return this.SecondTimeKatsura && this.ThirdTimeTwoKatsura;
            }
            public override void Reset()
            {
                this.SecondTimeKatsura = this.ThirdTimeTwoKatsura = false;
            }
            public override string ToName()
            {
                return "カツラが二枚連続で使える確率";
            }
        }
        public class GyaropOrKyucon : CheckPoint
        {
            public GyaropOrKyucon(int count) : base(count) { }
            public override string ToName()
            {
                return "ギャロップかキュウコンが最速で完成する";
            }
        }
        public class KatsuraAndGyaropOrKyucon : CheckPoint
        {
            public KatsuraAndGyaropOrKyucon(int count) : base(count) { }
            public override string ToName()
            {
                return "ギャロップかキュウコンが完成してカツラも手札にある";
            }
        }
        public class FullKatsuraAndGyaropOrKyucon : CheckPoint
        {
            public FullKatsuraAndGyaropOrKyucon(int count) : base(count) { }
            public override string ToName()
            {
                return "ギャロップかキュウコンが完成してカツラが二枚連続で使える";
            }
        }
        #endregion

        static void Main(string[] args)
        {
            // 試行回数
            int count = 100000;

            #region <チェック項目のリスト>
            FastestGyarop fastestGyarop = new FastestGyarop(count);
            FastestKyucon fastestKyucon = new FastestKyucon(count);
            FastestKatsura fastestKatsura = new FastestKatsura(count);
            FullKatsura fullKatsura = new FullKatsura(count);
            GyaropOrKyucon gyaropOrKyucon = new GyaropOrKyucon(count);
            KatsuraAndGyaropOrKyucon katsuraAndGyaropOrKyucon = new KatsuraAndGyaropOrKyucon(count);
            FullKatsuraAndGyaropOrKyucon fullKatsuraAndGyaropOrKyucon = new FullKatsuraAndGyaropOrKyucon(count);
            #endregion

            for (int i = 0; i < count; i++)
            {
                #region <初期リセット>
                Deck = CreateDeck(); // 山札をリセット
                List<Card> hands = FirstDraw();
                #endregion

                #region <チェック項目のリセット>
                fastestGyarop.Reset();
                fastestKyucon.Reset();
                fastestKatsura.Reset();
                fullKatsura.Reset();
                #endregion

                // 1ターン目
                hands = DrawOne(hands);
                #region <チェック項目の更新>
                fastestKyucon.FirstTimeRocon = hands.HaveOne(CardType.Rocon);
                fastestGyarop.FirstTimePonita = hands.HaveOne(CardType.Ponita);
                #endregion

                // 2ターン目
                hands = DrawOne(hands);
                #region <チェック項目の更新>
                fastestKyucon.SecondTimeKyucon = hands.HaveOne(CardType.Kyukon);
                fastestGyarop.SecondTimeGyarop = hands.HaveOne(CardType.Gyarop);
                fastestKatsura.SecondTimeKatsura = hands.HaveOne(CardType.Katsura);
                fullKatsura.SecondTimeKatsura = hands.HaveOne(CardType.Katsura);
                #endregion

                // 3ターン目
                hands = DrawOne(hands);
                #region <チェック項目の更新>
                fullKatsura.ThirdTimeTwoKatsura = hands.HaveTwo(CardType.Katsura);
                #endregion

                // 4ターン目
                hands = DrawOne(hands);

                #region <チェック項目の更新>
                fastestGyarop.Update();
                fastestKyucon.Update();
                fastestKatsura.Update();
                fullKatsura.Update();
                gyaropOrKyucon.Update(fastestGyarop.IsFullfill() || fastestKyucon.IsFullfill());
                katsuraAndGyaropOrKyucon.Update(fastestKatsura.IsFullFilled && gyaropOrKyucon.IsFullFilled);
                fullKatsuraAndGyaropOrKyucon.Update(fullKatsura.IsFullFilled && gyaropOrKyucon.IsFullFilled);
                #endregion
            }

            #region <結果の出力>
            //
            Console.WriteLine(fastestKyucon.ToName());
            Console.WriteLine(fastestKyucon.GetResult());
            //
            Console.WriteLine(fastestGyarop.ToName());
            Console.WriteLine(fastestGyarop.GetResult());
            //
            Console.WriteLine(gyaropOrKyucon.ToName());
            Console.WriteLine(gyaropOrKyucon.GetResult());
            //
            Console.WriteLine(katsuraAndGyaropOrKyucon.ToName());
            Console.WriteLine(katsuraAndGyaropOrKyucon.GetResult());
            //
            Console.WriteLine(fullKatsuraAndGyaropOrKyucon.ToName());
            Console.WriteLine(fullKatsuraAndGyaropOrKyucon.GetResult());
            #endregion

            Console.ReadKey();
        }
    }
}
