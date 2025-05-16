enum Day
{
    Sunday,
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday
}

class Program{
    static void Main(){
        Day today = Day.Sunday;

        if (today == Day.Monday){
            Console.WriteLine("It's Monday!");
        }
        else if(today == Day.Tuesday){
            Console.WriteLine("It's Tuesday");
        }
        else if(today == Day.Wednesday){
            Console.WriteLine("It's Wednesday");
        }
        else if(today == Day.Thursday){
            Console.WriteLine("It's Thursday");
        }
        else if(today == Day.Friday){
            Console.WriteLine("It's Friday");
        }
        else if(today == Day.Saturday){
            Console.WriteLine("It's Saturday");
        }
        else{
            Console.WriteLine("It's Sunday");
        }
    }
}

