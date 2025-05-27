using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Slot5PRNDemo1.Models;

namespace Slot5PRNDemo1
{
    public static class ODataModelBuilderConfig
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Gadgets>("GadgetsOdata");
            return modelBuilder.GetEdmModel();
        }
    }
}
