

namespace Kborod.Model.Implementations
{
    public abstract class AccountModelServer : AccountModel
    {
        public override void Login(string name)
        {
            //authorization on backend here

            Name = name;
            InvokeLoggedIn();
        }
    }
}
