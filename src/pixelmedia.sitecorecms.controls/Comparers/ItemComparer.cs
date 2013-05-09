using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;

namespace PixelMEDIA.SitecoreCMS.Controls.Compareres
{
    public class ItemComparer : IEqualityComparer<Item>
    {
        // Products are equal if their GUIDs are equal. 
        public bool Equals(Item x, Item y)
        {
            //Check whether any of the compared objects is null. 
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the compared objects reference the same data. 
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether the products' ID's are equal. 
            return x.ID == y.ID;
        }

        public int GetHashCode(Item item)
        {
            //Check whether the object is null 
            if (Object.ReferenceEquals(item, null)) return 0;

            //Get hash code for the ID field. 
            int hashItemID = item.ID.GetHashCode();

            //Get hash code for the Name field. 
            int hashItemName = item.Name == null ? 0 : item.Name.GetHashCode();

            //Calculate the hash code for the product. 
            return hashItemID ^ hashItemName;
        }

    }
}
