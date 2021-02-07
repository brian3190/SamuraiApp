using Microsoft.EntityFrameworkCore;

namespace SamuraiApp.Data
{
    public class SamuraiContextNoTracking : SamuraiContext 
    {
        public SamuraiContextNoTracking()
        {
            base.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
        }




    }
}
