using Repository;

namespace Solutions
{
    public class Day7 : AdventSolutionBase
    {
        private const string Filename = "day7.txt";

        public Day7(IDataRetriever dataRetriever) : base(dataRetriever)
        {
        }

        public override int FirstQuestion()
        {
            return FirstQuestion(Filename);
        }

        public override int FirstQuestion(string filename)
        {
            return GeneralSolution(filename, QuestionNumber.QuestionOne);
        }

        public int GeneralSolution(string filename, QuestionNumber question)
        {
            var allLines = GetAllLines(filename);
            var sortedCamelCardEntries = new SortedSet<CamelCardEntry>(new ByStrength());

            foreach (var line in allLines)
            {
                var handBetList = line.Split(" ");
                if (question == QuestionNumber.QuestionOne)
                {
                    sortedCamelCardEntries.Add(new CamelCardEntry(handBetList.First(), int.Parse(handBetList.Last())));
                }
                else
                {
                    sortedCamelCardEntries.Add(new CamelCardEntryPart2(handBetList.First(), int.Parse(handBetList.Last())));
                }
            }

            var rankCounter = 1;
            var totalWinnings = 0;
            foreach (var s in sortedCamelCardEntries)
            {
                /* Console.WriteLine($"\t{s.Hand} : {s.Bid}"); */
                totalWinnings += s.Bid * rankCounter;
                rankCounter++;
            }
            return totalWinnings;
        }

        public override int SecondQuestion()
        {
            return SecondQuestion(Filename);
        }

        public enum QuestionNumber
        {
            QuestionOne = 1, QuestionTwo
        }

        public override int SecondQuestion(string filename)
        {
            return GeneralSolution(filename, QuestionNumber.QuestionTwo);
        }
    }
    
    public class CamelCardEntry
    {
        public CamelCardEntry(string hand, int bid)
        {
            Hand = hand;
            Bid = bid;
        }

        public string Hand { get; set; }
        public int Bid { get; set; }

        public virtual HandType GetHandType()
        {
            var cardCounts = new Dictionary<char, int>();
            foreach (var card in Hand)
            {
                if (cardCounts.ContainsKey(card))
                {
                    cardCounts[card]++;
                    continue;
                }
                cardCounts.Add(card, 1);
            }

            return MapToHandType(cardCounts);
        }

        protected static HandType MapToHandType(Dictionary<char, int> cardCounts)
        {
            if (cardCounts.ContainsValue(5)) return HandType.FiveOfAKind;
            if (cardCounts.ContainsValue(4)) return HandType.FourOfAKind;
            if (cardCounts.ContainsValue(3))
            {
                if (cardCounts.ContainsValue(2)) return HandType.FullHouse;
                return HandType.ThreeOfAKind;
            }
            if (cardCounts.Keys.Count() == 3) return HandType.TwoPairs; //Exactly three distinct cards types and none of them have three in count => two pairs
            if (cardCounts.ContainsValue(2)) return HandType.OnePair;
            return HandType.HighCard;
        }

        public enum HandType
        {
            HighCard,
            OnePair,
            TwoPairs,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind
        }

        public virtual int GetCardValue(char card)
        {
            return card switch
            {
                'A' => 14,
                'K' => 13,
                'Q' => 12,
                'J' => 11,
                'T' => 10,
                _ => (int)char.GetNumericValue(card),
            };
        }
    }

    public class ByStrength : IComparer<CamelCardEntry>
    {
        public int Compare(CamelCardEntry? x, CamelCardEntry? y)
        {
            var result = x.GetHandType().CompareTo(y.GetHandType());
            return result != 0 ? result : CompareHandOrdering(x, y);
        }

        private int CompareHandOrdering(CamelCardEntry? x, CamelCardEntry? y)
        {
            for (int i = 0; i < x.Hand.Length; i++)
            {
                if (x.Hand[i] != y.Hand[i])
                {
                    return x.GetCardValue(x.Hand[i]).CompareTo(y.GetCardValue(y.Hand[i]));
                }
            }
            throw new Exception("No card in the hand was higher than the other. Error!");
        }
    }

    public class CamelCardEntryPart2 : CamelCardEntry
    {
        public CamelCardEntryPart2(string hand, int bid) : base(hand, bid)
        {
        }

        public override int GetCardValue(char card)
        {
            var value = base.GetCardValue(card);
            return value != 11 ? value : 1;
        }

        public override HandType GetHandType()
        {
            var cardCounts = new Dictionary<char, int>();
            var JCounts = 0;
            foreach (var card in Hand)
            {
                if (card == 'J')
                {
                    JCounts++;
                    continue;
                }
                if (cardCounts.ContainsKey(card))
                {
                    cardCounts[card]++;
                    continue;
                }
                cardCounts.Add(card, 1);
            }

            if (JCounts == Hand.Length) cardCounts.Add('J', JCounts);
            else if (JCounts > 0)
            {
                var keyMax = cardCounts.MaxBy(x => x.Value).Key;
                cardCounts[keyMax] += JCounts;
            }

            return MapToHandType(cardCounts);
        }
    }
}
