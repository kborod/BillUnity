

namespace Kborod.Model.Implementations
{
    public class AccountModelLocal : AccountModel
    {
        public override void Login(string name)
        {
            Name = name;
            InvokeLoggedIn();
        }
    }
}
