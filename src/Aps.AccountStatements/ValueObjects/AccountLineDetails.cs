using Seterlund.CodeGuard;

namespace Aps.AccountStatements.ValueObjects
{
    public class AccountLineDetails
    {
        public string Item { get; private set; }
        public string Value { get; private set; }

        public AccountLineDetails(string item, string value)
        {
            Guard.That(item).IsNotNullOrEmpty();
            Guard.That(value).IsNotNullOrEmpty();

            Item = item;
            Value = value;
        }
    }
}