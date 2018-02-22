using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSwaper
{
    public class Game
    {
        private Dictionary<int, object> Transelate { get; set; }
        public int Bombs { get; }
        public int Size { get; }
        private int[,] Map { get; }
        private DateTime StartTime { get; set; }
        private DateTime FinishTime { get; set; }

        public TimeSpan GameTime => FinishTime - StartTime;

        public List<string> BombXY { get; set; }
        public Game(int size, int bombs)
        {
            Size = size;
            Bombs = bombs;
            BombXY = new List<string>();
            Map = new int[Size, Size];
            CreateMap();
        }

        public void CreateMap()
        {
            var l = Enumerable.Range(0, Size * Size - 1).OrderBy(x => Guid.NewGuid()).ToList();
            for (var i = 0; i <= Bombs; i++)
            {
                    BombXY.Add(l[i] / Size + "," + l[i] % Size);
                    Map[l[i] / Size, l[i] % Size] = CampType.Bomb.GetHashCode();
            }
            for (var x = 0; x < Size; x++)
            {
                for (var y = 0; y < Size; y++)
                {
                    if (Map[x, y] == CampType.Bomb.GetHashCode())
                        continue;

                    var number = 0;

                    if (x - 1 >= 0)
                        number += Map[x -1, y] == CampType.Bomb.GetHashCode() ? 1 : 0;
                    if (x + 1 < Size)
                        number += Map[x +1, y] == CampType.Bomb.GetHashCode() ? 1 : 0;

                    if (y + 1 < Size)
                    {
                        number += Map[x, y + 1] == CampType.Bomb.GetHashCode() ? 1 : 0;
                        if (x - 1 >= 0)
                            number += Map[x - 1, y + 1] == CampType.Bomb.GetHashCode() ? 1 : 0;
                        if (x + 1 < Size)
                            number += Map[x + 1, y + 1] == CampType.Bomb.GetHashCode() ? 1 : 0;
                    }
                    if (y - 1 >= 0)
                    {
                        number += Map[x, y -1] == CampType.Bomb.GetHashCode() ? 1 : 0;
                        if (x - 1 >= 0)
                            number += Map[x - 1, y - 1] == CampType.Bomb.GetHashCode() ? 1 : 0;
                        if (x + 1 < Size)
                            number += Map[x + 1, y - 1] == CampType.Bomb.GetHashCode() ? 1 : 0;
                    }
                    Map[x, y] = number;
                }
            }
        }

        public void Start()
        {
            StartTime = DateTime.Now;
        }

        public void End()
        {
            FinishTime = DateTime.Now;
        }
        public CampType GetCamp(int x, int y)
        {
            return (CampType)Enum.Parse(typeof(CampType), Map[x, y].ToString());
        }
    }

    public class Shuffler
    {
    }
}
