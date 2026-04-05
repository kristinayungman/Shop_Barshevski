using System.Collections.Generic;

namespace UserApp
{
    public static class OrderStatuses
    {
        public const int Placed = 0;
        public const int Shipped = 1;
        public const int Delivered = 2;

        public static int NormalizeCode(int code)
        {
            if (code < 0 || code > 2)
                return Placed;
            return code;
        }

        public static string Label(int code)
        {
            switch (NormalizeCode(code))
            {
                case Shipped:
                    return "Отправлен";
                case Delivered:
                    return "Доставлен";
                default:
                    return "Оформлен";
            }
        }

        /// <summary>Список для ComboBox: Key — код в БД, Value — подпись.</summary>
        public static List<KeyValuePair<int, string>> ChoiceList { get; } = new List<KeyValuePair<int, string>>
        {
            new KeyValuePair<int, string>(Placed, "Оформлен"),
            new KeyValuePair<int, string>(Shipped, "Отправлен"),
            new KeyValuePair<int, string>(Delivered, "Доставлен")
        };
    }
}
