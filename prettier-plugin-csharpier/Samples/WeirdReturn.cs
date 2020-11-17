namespace TheNamespace
{
    public class TheClass {
        public void Method(IUnitOfWork unitOfWork, ISiteContext siteContext) {
            if (true == false) {
                return GetNodeIdByType(unitOfWork, siteContext, "ProductListPage");
            }

            return null;
        }
    }
}
