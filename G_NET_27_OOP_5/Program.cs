#region solution:
using System;

namespace G_NET_27_OOP_5
{
    #region Interfaces
    interface IPrintable
    {
        void Print();
    }

    interface IBookable
    {
        bool Book();
        bool Cancel();
    }
    #endregion

    #region Struct
    struct SeatLocation
    {
        public char Row;
        public int Number;

        public SeatLocation(char row, int number)
        {
            Row = row;
            Number = number;
        }

        public override string ToString()
        {
            return $"{Row}{Number}";
        }
    }
    #endregion

    #region Base Class
    class Ticket : IPrintable, IBookable, ICloneable
    {
        private static int counter = 0;
        protected decimal price;

        public int TicketId { get; private set; }
        public string MovieName { get; set; }
        public SeatLocation Seat { get; set; }
        public bool IsBooked { get; private set; }

        public decimal PriceAfterTax => price * 1.14m;

        public Ticket(string movie, SeatLocation seat)
        {
            TicketId = ++counter;
            MovieName = movie;
            Seat = seat;
        }

        public bool Book()
        {
            if (IsBooked) return false;
            IsBooked = true;
            return true;
        }

        public bool Cancel()
        {
            if (!IsBooked) return false;
            IsBooked = false;
            return true;
        }

        public void SetPrice(decimal p)
        {
            price = p;
        }

        public void SetPrice(decimal basePrice, decimal multiplier)
        {
            price = basePrice * multiplier;
        }

        public virtual void Print()
        {
            Console.Write($"[Ticket #{TicketId}] {MovieName} | Price: {price} | After Tax: {PriceAfterTax:0.0} | Booked: {(IsBooked ? "Yes" : "No")}");
        }

        public virtual object Clone()
        {
            return new Ticket(MovieName, new SeatLocation(Seat.Row, Seat.Number))
            {
                price = this.price
            };
        }
    }
    #endregion

    #region Derived Classes
    class StandardTicket : Ticket
    {
        public StandardTicket(string movie, SeatLocation seat) : base(movie, seat) { }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($" | Standard | Seat: {Seat}");
        }

        public override object Clone()
        {
            return new StandardTicket(MovieName, new SeatLocation(Seat.Row, Seat.Number))
            {
                price = this.price
            };
        }
    }

    class VIPTicket : Ticket
    {
        public bool LoungeAccess { get; set; }
        public decimal ServiceFee { get; set; }

        public VIPTicket(string movie, SeatLocation seat, bool lounge, decimal fee)
            : base(movie, seat)
        {
            LoungeAccess = lounge;
            ServiceFee = fee;
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($" | VIP | Lounge: {(LoungeAccess ? "Yes" : "No")} | Fee: {ServiceFee}");
        }

        public override object Clone()
        {
            return new VIPTicket(MovieName, new SeatLocation(Seat.Row, Seat.Number), LoungeAccess, ServiceFee)
            {
                price = this.price
            };
        }
    }

    class IMAXTicket : Ticket
    {
        public bool Is3D { get; set; }

        public IMAXTicket(string movie, SeatLocation seat, bool is3D)
            : base(movie, seat)
        {
            Is3D = is3D;
        }

        public override void Print()
        {
            base.Print();
            Console.WriteLine($" | IMAX | 3D: {(Is3D ? "Yes" : "No")}");
        }

        public override object Clone()
        {
            return new IMAXTicket(MovieName, new SeatLocation(Seat.Row, Seat.Number), Is3D)
            {
                price = this.price
            };
        }
    }
    #endregion

    #region Cinema
    class Cinema
    {
        private Ticket[] tickets = new Ticket[20];

        public void Open()
        {
            Console.WriteLine(" Cinema Opened ");
        }

        public void Close()
        {
            Console.WriteLine("Cinema Closed");
        }

        public bool AddTicket(Ticket t)
        {
            for (int i = 0; i < tickets.Length; i++)
            {
                if (tickets[i] == null)
                {
                    tickets[i] = t;
                    return true;
                }
            }
            return false;
        }

        public void PrintAllTickets()
        {
            Console.WriteLine("\n All Tickets ");
            foreach (var t in tickets)
            {
                if (t != null)
                {
                    ((IPrintable)t).Print();
                }
            }
        }
    }
    #endregion

    #region Helper
    class BookingHelper
    {
        public static void PrintAll(IPrintable[] items)
        {
            Console.WriteLine("\n BookingHelper.PrintAll");
            foreach (var item in items)
            {
                item.Print();
            }
        }
    }
    #endregion

    internal class Program
    {
        static void Main(string[] args)
        {
            Cinema cinema = new Cinema();
            cinema.Open();

            var t1 = new StandardTicket("Inception", new SeatLocation('A', 5));
            var t2 = new VIPTicket("Avengers", new SeatLocation('B', 3), true, 50);
            var t3 = new IMAXTicket("Dune", new SeatLocation('C', 10), true);

            t1.SetPrice(80);
            t2.SetPrice(200);
            t3.SetPrice(130);

            t1.Book();
            t2.Book();
            t3.Book();

            cinema.AddTicket(t1);
            cinema.AddTicket(t2);
            cinema.AddTicket(t3);

            cinema.PrintAllTickets();

            Console.WriteLine("\n Clone Test ");
            var clone = (VIPTicket)t2.Clone();
            clone.MovieName = "Interstellar";

            Console.Write("Original : ");
            t2.Print();

            Console.Write("Clone : ");
            clone.Print();

            Console.WriteLine("\n After Cancellation ");
            t1.Cancel();
            t1.Print();

            BookingHelper.PrintAll(new IPrintable[] { t1, t2, t3 });

            cinema.Close();
        }
    }
}
#endregion