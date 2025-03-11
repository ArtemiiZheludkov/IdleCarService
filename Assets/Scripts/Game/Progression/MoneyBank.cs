using System;

namespace IdleCarService.Progression
{
    public class MoneyBank
    {
        public event Action<int> MoneyChanged;

        public int Money { get; private set; }

        public MoneyBank(int initialMoney = 0)
        {
            Money = initialMoney;
        }
        
        public void AddMoney(int amount)
        {
            if (amount < 0)
                return;

            Money += amount;
            MoneyChanged?.Invoke(Money);
        }
        
        public bool TrySpendMoney(int amount)
        {
            if (amount < 0 || Money < amount)
                return false;

            Money -= amount;
            MoneyChanged?.Invoke(Money);
            return true;
        }
        
        public void LoadData(int newAmount)
        {
            Money = newAmount;
            MoneyChanged?.Invoke(Money);
        }
    }
}