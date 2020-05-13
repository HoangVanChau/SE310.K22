using HRM.Models.Cores;

namespace HRM.Helpers
{
    public static class ValidateUtils
    {
        public static bool ValidateNewContract(Contract newContract)
        {
            if (newContract == null)
            {
                return false;
            }
            if (newContract.StartDate >= newContract.EndDate)
            {
                return false;
            }

            if (newContract.ContractName == null)
            {
                return false;
            }
            
            return true;
        }
    }
}