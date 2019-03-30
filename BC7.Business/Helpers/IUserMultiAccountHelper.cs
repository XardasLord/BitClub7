using System;

namespace BC7.Business.Helpers
{
    public interface IUserMultiAccountHelper
    {
        string GetNextMultiAccountName(Guid userAccountDataId);
    }
}
