using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAOT.Model
{
    /// <summary>
    /// Representation of a storage space in a virtual warehouse. This only defines the properties of
    /// the sotrage location and does not actually store anything itself.
    /// </summary>
    public class Location
    {
        public readonly string LocationId;

        public Location(string locationId)
        {
            Assert.IsFalse(string.IsNullOrEmpty(locationId));
            LocationId = locationId;
        }

        
    }


    

}
