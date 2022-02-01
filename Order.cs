using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventHub
{
    public class Order
    {
        private string itemName;
        private int id;
        private int numberOfItems;
        public Order(string itemName, int numberOfItems)
        {
            this.itemName = itemName;
            this.numberOfItems = numberOfItems;
            this.id = new Random().Next(1000);

        }
        public override string ToString()
        {
            return $"Id : {id}, itemName : {itemName}, numberOfItems : {numberOfItems}";
        }
    }
}
