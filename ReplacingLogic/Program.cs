using System;
using System.Collections.Generic;
using System.Linq;

namespace IMJunior
{
    class Program
    {
        static void Main(string[] args)
        {
            PaymentSystemsFactory paymentSystemsFactory = new PaymentSystemsFactory();
            List<IPaymentSystem> paymentSystems = paymentSystemsFactory.CreateAllPaymentSystems();

            Shop shop = new Shop(paymentSystems);
            shop.Run();
        }
    }

    class Shop
    {
        private List<IPaymentSystem> _paymentSystems;

        public Shop(List<IPaymentSystem> paymentSystems)
        {
            _paymentSystems = paymentSystems ?? throw new ArgumentNullException(nameof(paymentSystems));
        }

        public void Run()
        {
            var orderForm = new OrderForm(_paymentSystems.Select(system => system.Name).ToList());
            var systemId = orderForm.ShowForm();

            foreach (var paymentSistem in _paymentSystems)
            {
                if (paymentSistem.Name.Equals(systemId, StringComparison.OrdinalIgnoreCase))
                {
                    paymentSistem.AcceptPayment();
                }
            }
        }
    }

    public class OrderForm
    {
        public OrderForm(List<string> supportedPaymentSystems)
        {
            SupportedPaymentSystems = supportedPaymentSystems ?? throw new ArgumentNullException(nameof(supportedPaymentSystems));
        }

        public IReadOnlyList<string> SupportedPaymentSystems { get; }

        public string ShowForm()
        {
            Console.WriteLine($"Мы принимаем: {string.Join(", ", SupportedPaymentSystems)}");
            Console.WriteLine("Какое системой вы хотите совершить оплату?");

            return Console.ReadLine();
        }
    }

    class PaymentSystemsFactory
    {
        public IPaymentSystem CreateQiwi()
        {
            return new Qiwi();
        }

        public IPaymentSystem CreateWebMoney()
        {
            return new WebMoney();
        }

        public IPaymentSystem CreateCard()
        {
            return new Card();
        }

        public List<IPaymentSystem> CreateAllPaymentSystems()
        {
            return new List<IPaymentSystem>()
            {
                CreateQiwi(),
                CreateWebMoney(),
                CreateCard()
            };
        }
    }

    interface IPaymentSystem
    {
        string Name { get; }

        void AcceptPayment();
    }

    class Qiwi : IPaymentSystem
    {
        public Qiwi()
        {
            Name = "QIWI";
        }

        public string Name { get; private set; }

        public void AcceptPayment()
        {
            Console.WriteLine("Перевод на страницу QIWI...");
            Console.WriteLine("Проверка платежа через QIWI...");
            Console.WriteLine("Оплата прошла успешно!");
        }
    }

    class WebMoney : IPaymentSystem
    {
        public WebMoney()
        {
            Name = "WebMoney";
        }

        public string Name { get; private set; }

        public void AcceptPayment()
        {
            Console.WriteLine("Вызов API WebMoney...");
            Console.WriteLine("Проверка платежа через WebMoney...");
            Console.WriteLine("Оплата прошла успешно!");
        }
    }

    class Card : IPaymentSystem
    {
        public Card()
        {
            Name = "Card";
        }

        public string Name { get; private set; }

        public void AcceptPayment()
        {
            Console.WriteLine("Вызов API банка эмитера карты Card...");
            Console.WriteLine("Проверка платежа через Card...");
            Console.WriteLine("Оплата прошла успешно!");
        }
    }
}