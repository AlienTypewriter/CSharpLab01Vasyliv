using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfApp1
{
    public class Model: INotifyPropertyChanged
    {
        int[] day= new int[12]
        {
            21,21,21,21,21,22,23,24,23,24,23,22
        };
        String[] ch = new string[12]
        {
            "Rat", "Ox" , "Tiger", "Rabbit", "Dragon", "Snake",
        "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig"
        };
        String[] eu = new string[12]
        {
            "Capricorn","Aquerius", "Pisces", "Aries", "Taurus" , "Gemini", "Cancer",
            "Leo", "Virgo","Libra", "Scoprio", "Sagittarius"
        };
        private DateTime selectedDate = DateTime.Parse("01/01/2000");
        private int age;
        private String euZodiac;
        private String chZodiac;

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public DateTime SelectedDate { get => selectedDate; set => selectedDate = value; }

        public Model()
        {
            CalcButtonCommand = new CalculateButtonCommand(this);
        }

        class CalculateButtonCommand : ICommand
        {
            Model parent;

            public CalculateButtonCommand(Model parent)
            {
                this.parent = parent;
            }

            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter) { return parent.SelectedDate != null; }

            public void Execute(object parameter)
            {
                DateTime comp = DateTime.Now;
                if (parent.SelectedDate >= comp)
                {
                    MessageBox.Show("Well, hello there, time traveler. Think you're a bit late to the party (psst, I don't believe you, enter a valid date of birth)");
                    return;
                }
                if (comp.Year - parent.SelectedDate.Year > 135)
                {
                    MessageBox.Show("You shouldn't be around computers right now, grandpa. Better go lie down (psst, I don't believe you, enter a valid date of birth)");
                    parent.Age = 0;
                    return;
                } else if (comp.Day == parent.SelectedDate.Day)
                {
                    MessageBox.Show("Happy birthday!");
                }
                parent.Age = comp.Year - parent.SelectedDate.Year - ((parent.SelectedDate.Day <= comp.Day) ? 0 : 1);
                int inEuropian = 0;
                for (int i=0;i<12;i++)
                {
                    if (parent.SelectedDate.Month>i+1||(parent.selectedDate.Month==i+1&&parent.SelectedDate.Day>=parent.day[i]))
                    {
                        inEuropian = i+1;
                    }
                }
                inEuropian %= 12;
                int inChinese = (parent.selectedDate.Year - 4 - ((parent.selectedDate < new DateTime(parent.SelectedDate.Year, 2, 5)) ? 1 : 0)) % 12;
                parent.ChZodiac = parent.ch[inChinese];
                parent.EuZodiac = parent.eu[inEuropian];
            }
        }

        public ICommand CalcButtonCommand { get; private set; }
        public int Age { get => age;
            set
            {
                age = value;
                NotifyPropertyChanged("Age");
            }
        }

        public string EuZodiac { get => euZodiac; set
            {
                euZodiac = value;
                NotifyPropertyChanged("EuZodiac");
            }
        }
        public string ChZodiac { get => chZodiac; set
            {
                chZodiac = value;
                NotifyPropertyChanged("ChZodiac");
            }
        }
    }
}
