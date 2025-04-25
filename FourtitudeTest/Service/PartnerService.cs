using FourtitudeTest.Model.Dbm;

namespace FourtitudeTest.Service
{
    public class PartnerService
    {
        private readonly DataContext _context;

        public PartnerService(DataContext context)
        {
            _context = context;
        }

        public PartnerDbm GetPartnerByRefNo(string partnerKey)
        {
            return Helper.Reuseable.Partners.FirstOrDefault(p => string.Equals(p.partnerRefNo, partnerKey, StringComparison.InvariantCulture));
        }
    }
}
