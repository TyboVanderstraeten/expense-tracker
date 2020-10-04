using System;

namespace ExpenseTracker.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description => Text switch
        {
            "a" => "ok"
        };
    }
}