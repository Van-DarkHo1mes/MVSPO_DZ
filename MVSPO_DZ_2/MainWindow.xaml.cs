using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVSPO_DZ_2
{
    public partial class MainWindow : Window
    {
        private int result;
        private int iMass = 0;
        private bool mod = false;
        private string errorStr = "";

        private string[] massWord;
        private List<Numbers> numArray = new List<Numbers>();

        enum NumberTypes
        {
            Unit,
            Tens,
            Eleven_Nineteen,
            Hundrert,
            Und
        }



        class Numbers
        {
            public NumberTypes type;
            public int value;

            public Numbers(NumberTypes type, int value) 
            {
                this.type = type;
                this.value = value;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            massWord = SplitStringIntoWords(AddStringTextBox.Text);

            if (massWord.Length == 1 && massWord[0] == "null") AfterStringBlock.Text = "Ваше число: " + result;
            else if (CheckForUnit(massWord[iMass]))
            {
                foreach(var num in numArray)
                {
                    if (num.type != NumberTypes.Hundrert) result += num.value;
                    else result = result * 100;
                }
                if (CheckError2())
                {
                    AfterStringBlock.Text = "Ваше число: " + result;
                }
                else
                {
                    AfterStringBlock.Text = errorStr;
                }
                     
            }
            else
            {
                if (CheckError2())
                {
                    AfterStringBlock.Text = "Ошибка! " + CheckError();
                }  
            }
            RerunProgramm();
        }

        private string[] SplitStringIntoWords(string inputString)
        {
            return massWord = inputString.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries); ;
        }

        private bool CheckForUnit(string myNumber)
        {
            foreach (var number in GermanNumbers.unit)
            {
                if (myNumber == number.Key)
                {
                    Numbers num = new Numbers(NumberTypes.Unit, number.Value);
                    numArray.Add(num);

                    if (!ArrayIsOver())
                    {
                        iMass++;
                        return CheckForHelpWord(massWord[iMass]);
                    }
                    else return true;
                }
            }
            return (CheckForElevenNineteen(massWord[iMass]) || CheckForTens(massWord[iMass]));
        }

        private bool CheckForHelpWord(string myNumber)
        {
            if (myNumber == "hundert" && !mod)
            {
                Numbers num = new Numbers(NumberTypes.Hundrert, 100);
                numArray.Add(num);

                if (!ArrayIsOver())
                {
                    iMass++;
                    mod = true;
                    return ((CheckForUnit(massWord[iMass])) || (CheckForTens(massWord[iMass])) || (CheckForElevenNineteen(massWord[iMass]) && (massWord[iMass - 1] != "und")));
                }
                else return true;
            }
            else if (myNumber == "und")
            {
                Numbers num = new Numbers(NumberTypes.Und, 0);
                numArray.Add(num);

                if (!ArrayIsOver())
                {
                    iMass++;
                    return CheckForTens(massWord[iMass]);
                }
                else return false;
            }
            else return false;
        }

        
        private bool CheckForTens(string myNumber)
        {
            foreach (var number in GermanNumbers.tens)
            {
                if (myNumber == number.Key)
                {
                    Numbers num = new Numbers(NumberTypes.Tens, number.Value);
                    numArray.Add(num);

                    return ArrayIsOver();
                }
            }
            return false;
        }

        private bool CheckForElevenNineteen(string myNumber)
        {
            foreach (var number in GermanNumbers.eleven_nineteen)
            {
                if (myNumber == number.Key)
                {
                    Numbers num = new Numbers(NumberTypes.Eleven_Nineteen, number.Value);
                    numArray.Add(num);

                    return ArrayIsOver();
                }
            }
            return false;
        }

        private bool ArrayIsOver()
        {
            if (iMass == massWord.Length - 1) return true;
            else
            {
                return false;
            }
        }
        private bool CheckError2()
        {
            try
            {
                if (numArray[0].type == NumberTypes.Eleven_Nineteen || numArray[0].type == NumberTypes.Tens)
                {
                    foreach (var number in GermanNumbers.tens)
                    {
                        if (massWord[1] == number.Key)
                        {
                            Numbers num = new Numbers(NumberTypes.Tens, number.Value);
                            numArray.Add(num);
                        }
                    }
                    foreach (var number in GermanNumbers.eleven_nineteen)
                    {
                        if (massWord[1] == number.Key)
                        {
                            Numbers num = new Numbers(NumberTypes.Eleven_Nineteen, number.Value);
                            numArray.Add(num);
                        }
                    }

                    AfterStringBlock.Text = "Ошибка! " + Translator(numArray[1].type) + " не может идти после " + Translator(numArray[0].type) + "!";
                    return false;
                }
                else if (numArray[0].type == NumberTypes.Unit && numArray[1].type == NumberTypes.Hundrert && (numArray[2].type == NumberTypes.Eleven_Nineteen || numArray[2].type == NumberTypes.Tens) && massWord.Length == 4)
                {
                    AfterStringBlock.Text = "Ошибка! После " + Translator(numArray[2].type) + " не может ничего идти!";
                    return false;
                }
            }
            catch { }
            try
            {
                if (massWord[0] == "und" || massWord[0] == "hundert")
                {
                    AfterStringBlock.Text = "Ошибка! " + massWord[0] + " не может идти первым!";
                    return false;
                }
                //else if ()
                else if (numArray[0].type == NumberTypes.Eleven_Nineteen && (massWord[1] == "und" || massWord[1] == "hundert"))
                {
                    AfterStringBlock.Text = "Ошибка! " + massWord[1] + " не может идти после  11-19!";
                    return false;
                }
                else if (numArray[0].type == NumberTypes.Tens && (massWord[1] == "und" || massWord[1] == "hundert"))
                {
                    AfterStringBlock.Text = "Ошибка!" + massWord[1] + " не может идти после  десяток!";
                    return false;
                }

                else if (numArray[0].type == NumberTypes.Unit && numArray[1].type == NumberTypes.Hundrert && massWord[2] == "und")
                {
                    AfterStringBlock.Text = "Ошибка! Und не может идти после  сотен!";
                    return false;
                }
                else if (numArray[0].type == NumberTypes.Unit && numArray[1].type == NumberTypes.Hundrert && numArray[2].type == NumberTypes.Unit && (numArray[3].type == NumberTypes.Unit || numArray[3].type == NumberTypes.Tens || numArray[3].type == NumberTypes.Eleven_Nineteen))
                {
                    errorStr = "Ошибка! " + Translator(numArray[3].type) + " не могут идти после единиц!";
                    return false;
                }
                else return true;
            }
            catch { }
            return true;
        }
        private string CheckError()
        {
            if (massWord[iMass] == "null") return "Ноль не может ипользован!";
            try
            {
                if (massWord[iMass] == "und" && iMass == massWord.Length - 1 && massWord[iMass - 1] != "und") return "После und ничего нет!";
                else if (massWord[iMass] == "und") return "Und не может идти после " + Translator(numArray[numArray.Count - 1].type) + "!";
            }
            catch { }
            try
            {
                if (massWord[iMass] == "hundert") return "Сотни не могут идти после " + Translator(numArray[numArray.Count - 1].type) + "!";
            }
            catch { }
            
            foreach (var number in GermanNumbers.unit)
            {
                if (massWord[iMass] == number.Key)
                {
                    return "Единицы не могут идти после " + Translator(numArray[numArray.Count - 1].type) + "!";
                }
            }
            foreach (var number in GermanNumbers.tens)
            {
                if (massWord[iMass] == number.Key)
                {
                    return "Десятки не могут идти после " + Translator(numArray[numArray.Count - 1].type) + "!";
                }
            }
            foreach (var number in GermanNumbers.eleven_nineteen)
            {
                if (massWord[iMass] == number.Key)
                {
                    return "11-19 не могут идти после " + Translator(numArray[numArray.Count - 1].type) + "!";
                }
            }
            return "Неправильное написание числа " + massWord[iMass];
        }


        private String Translator(NumberTypes numberTypes)
        {
            if (numberTypes == NumberTypes.Eleven_Nineteen) return "11-19";
            if (numberTypes == NumberTypes.Unit) return "Единицы";
            if (numberTypes == NumberTypes.Tens) return "Десятки";
            if (numberTypes == NumberTypes.Hundrert) return "Сотни";
            return "Und";
        }
        private void RerunProgramm()
        {
            iMass = 0;
            result = 0;
            mod = false;
            numArray.Clear();
        }

    }
}
