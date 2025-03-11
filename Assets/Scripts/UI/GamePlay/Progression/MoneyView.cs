using IdleCarService.Progression;
using TMPro;
using UnityEngine;

namespace IdleCarService.UI.GamePlay
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _valueText;
        
        private MoneyBank _bank;

        public void Init(MoneyBank bank)
        {
            _bank = bank;
            _valueText.text = _bank.Money.ToString();
        }

        public void Enable()
        {
            _valueText.text = _bank.Money.ToString();
            _bank.MoneyChanged += OnValueChanged;
        }

        public void Disable()
        {
            _bank.MoneyChanged -= OnValueChanged;
        }
        
        private void OnValueChanged(int money)
        {
            _valueText.text = money.ToString();
        }
    }
}