namespace Praetoria_demo.Policies
{
    //note these would be better saved as database entries, but for the sake of this demo, we'll hardcode them here
    //additionally, min monthly income should adjust based on currency
    //centralised here and not added as data annotations in case of potential updates to the policy in the future, and to keep the validation logic in one place
    public static class EligibilityPolicyConstants
    {
        public const decimal MinMonthlyIncomeGBP = 2000;
        public const int MaxIncomeMultiplier = 4;
        public const int MaxTermMonths = 60;
        public const int MinTermMonths = 12;
    }
}
