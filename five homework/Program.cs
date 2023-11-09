using Morzyanka;
using System.Diagnostics;
using System.Text;
using System.Threading.Channels;

internal class Program
{
    private static void Main(string[] args)
    {
        //KrestikiNoliki.Game game = new KrestikiNoliki.Game();
        //game.Run();
        //game.Run(true);
        string morz = MorzConvertor.ConvertToMorze("Съешь ещё этих мягких французских булок, да выпей же чаю.");
        Console.WriteLine(morz);
        string text = MorzConvertor.ConvertFromMorze(morz);
        Console.WriteLine(text);
    }
}
namespace KrestikiNoliki
{
    enum Signs { X,O,_};
    class Game
    {
        
        Signs winner = Signs._;
        
        Signs Player;
        Signs Oponent;
        Signs[,] pole = new Signs[3, 3];
        Random random = new Random();
        int countofsteps = 0;
        bool endcheck()
        {

            for (int i = 0; i < 3; i++)
            {
                if (pole[i, 0] == pole[i, 1] && pole[i, 0] == pole[i, 2]&& pole[i, 0]!=Signs._)
                {
                    winner = pole[i, 0];
                    return true;
                }
                if(pole[0, i] == pole[1, i] && pole[0, i] == pole[2, i] && pole[0, i] != Signs._)
                {
                    winner = pole[0, i];
                    return true;
                }
            }
            if (pole[0, 0] != Signs._ && pole[0, 0] == pole[1, 1] && pole[0, 0] == pole[2, 2])
            {
                winner = pole[1,1];
                return true;
            }
            if (pole[0, 2] != Signs._ && pole[0, 2] == pole[1, 1] && pole[0,2] == pole[2, 0])
            {
                winner = pole[1, 1];
                return true;
            }
            return false;
        }
        void ComputerStepRand(Signs sign)
        {
            int x,y;
            while (true)
            {
                x=random.Next(3);
                y=random.Next(3);
                if (pole[y,x] == Signs._)
                {
                    pole[y,x] = sign;
                    break;
                }
            }
        }
        void computerstepclever()
        {
            //надо бы написать код, например это https://habr.com/ru/articles/329058/
        }
        void Playerstep(Signs whoplay)
        {
            Console.WriteLine("сделайте свой шаг используя \n\t789\n\t456\n\t123");
            ConsoleKey num;
            do
            {
            int? X=null, Y = null;
                num = Console.ReadKey().Key;
                Console.WriteLine();
                switch (num)
                {
                    case ConsoleKey.NumPad7:
                        Y = 0; X = 0;
                        break;
                    case ConsoleKey.NumPad8:
                        Y = 0; X = 1;
                        break;
                    case ConsoleKey.NumPad9:
                        Y = 0; X = 2;
                        break;
                    case ConsoleKey.NumPad4:
                        Y = 1; X = 0;
                        break;
                    case ConsoleKey.NumPad5:
                        Y = 1; X = 1;
                        break;
                    case ConsoleKey.NumPad6:
                        Y = 1; X = 2;
                        break;
                    case ConsoleKey.NumPad1:
                        Y = 2; X = 0;
                        break;
                    case ConsoleKey.NumPad2:
                        Y = 2; X = 1;
                        break;
                    case ConsoleKey.NumPad3:
                        Y = 2; X = 2;
                        break;
                    default:
                        Console.WriteLine("неверный ввод, используйте цифры на клавиатуре справа");
                        continue;
                }
                if (pole[Y.Value, X.Value] == Signs._)
                {
                    pole[Y.Value, X.Value] = whoplay;
                    return;
                }
                else
                {
                    Console.WriteLine("это место уже занято");
                    continue;
                }
            } while (true);

        }
        private void showpole()
        {
            for (int i = 0; i < pole.GetLength(0); i++)
            {
                for (int j = 0; j < pole.GetLength(1); j++)
                {
                    Console.Write(" " + pole[i,j]);
                }
                Console.WriteLine();
            }
        }
        delegate void Step(Signs sign);
        /// <summary>
        /// этот метод запускает игру
        /// </summary>
        /// <param name="whoplay"> false для игры с компьютером true для игры м человеком.</param>
        public void Run(bool whoplay=false)
        {
            Step OponentStep;
            if(whoplay) OponentStep = new Step(ComputerStepRand);
            else OponentStep = new Step(Playerstep);
            for (int i = 0; i < pole.GetLength(0); i++)
            {
                for (int j = 0; j < pole.GetLength(1); j++)
                {
                    pole[i, j] = Signs._;
                }
            }
            Console.WriteLine("выберите за крестики или за нолики играть Х - 0");
            char igrok;
            //showpole();
            do
            {
                igrok = Console.ReadLine().ToUpper()[0];
                if (igrok == '0') igrok = Signs.O.ToString()[0];
            }while (igrok!= Signs.O.ToString()[0]&& igrok != Signs.X.ToString()[0]);
            Console.WriteLine("вы выбрали "+ igrok);
            Player = (igrok == 'X') ? Signs.X : Signs.O;
            Oponent = (Player == Signs.X) ? Signs.O : Signs.X;
            if (random.Next(2) == 0)  Console.WriteLine("вы ходите первый");
            else 
            { 
                Console.WriteLine("вы ходите второй");
                OponentStep(Oponent);
                countofsteps++;
            }
            while(true) 
            {
                Console.WriteLine();
                showpole();
                Playerstep(Player);
                if (++countofsteps == 9 || endcheck()) break;
                Console.WriteLine();
                showpole();
                OponentStep(Oponent);
                if (++countofsteps == 9 || endcheck()) break;
            }
            showpole();
            if(winner==Signs._) Console.WriteLine("ничья");
            else Console.WriteLine("победили "+winner.ToString());
        }
        
    }
}

namespace Morzyanka
{
    static class MorzConvertor
    {
        static Dictionary<char, string> morz = new Dictionary<char, string>()
        {
            {'А', ".-"},
            {'Б', "-..."},
            {'В', ".--"},
            {'Г', "--."},
            {'Д', "-.."},
            {'Е', "."},
            {'Ё', "."},
            {'Ж', "...-"},
            {'З', "--.."},
            {'И', ".."},
            {'Й', ".---"},
            {'К', "-.-"},
            {'Л', ".-.."},
            {'М', "--"},
            {'Н', "-."},
            {'О', "---"},
            {'П', ".--."},
            {'Р', ".-."},
            {'С', "..."},
            {'Т', "-"},
            {'У', "..-"},
            {'Ф', "..-."},
            {'Х', "...."},
            {'Ц', "-.-."},
            {'Ч', "---."},
            {'Ш', "----"},
            {'Щ', "--.-"},
            {'Ъ', "--.--"},
            {'Ы', "-.--"},
            {'Ь', "-..-"},
            {'Э', "..-.."},
            {'Ю', "..--"},
            {'Я', ".-.-"},
            {'0', "-----"},
            {'1', ".----"},
            {'2', "..---"},
            {'3', "...--"},
            {'4', "....-"},
            {'5', "....."},
            {'6', "-...."},
            {'7', "--..."},
            {'8', "---.."},
            {'9', "----."},
            {'.', "......"},
            {',',".-.-.-" },
            {' ', " " }
            
        };
        public static string ConvertToMorze(string str)
        {
            str=str.ToUpper();
            StringBuilder toreturn = new StringBuilder();
            foreach (char c in str)
            {
                if (morz.TryGetValue(c, out string toadd))
                {
                    toreturn.Append(toadd);
                    toreturn.Append(" ");
                }
                else
                {
                    Console.WriteLine(c);
                    throw new Exception();
                }
            }
            return toreturn.ToString();
        }
        public static string ConvertFromMorze(string str)
        {
            string[] strings = str.Split(' ');
            StringBuilder toreturn = new StringBuilder();
            foreach (string s in strings)
            {
                if (s == "")
                {
                    toreturn.Append(" ");
                    continue;
                }
                string key = morz.FirstOrDefault(x => x.Value == s).Key.ToString();
                toreturn.Append(key);
            }

            return toreturn.ToString();
        }
    }
}
