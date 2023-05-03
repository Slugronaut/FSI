using System;
using static SAOT.Model.PickOrdersDatabase;

namespace SAOT.Model
{
    [Flags]
    public enum OrderTypes : int
    {
        Undefined           = 0,
        PickForVendor       = 1 << 0,
        PickForProduction   = 1 << 1,
        InventoryCount      = 1 << 2,
        VendorNCR           = 1 << 3,
        CustomerNCR         = 1 << 4,
        All = -1,
    }

    public enum PickOrderStatuses : int
    {
        PickPending,
        Picking,
        Picked,
        Shipped,
        Adjusted,
        Closed   = 100,
    }

    public enum PickItemStatuses : int
    {
        PickPending,
        Picked,
        Canceled = 100,
    }
    /*
        pickOrder table
        orderId INTEGER PRIMARY KEY AUTOINCREMENT,
        orderType INTEGER,
        createdBy INTEGER,
        dateCreated TEXT,
        dateModified TEXT,
        destinationId INTEGER,
        inventoryId INTEGER,
        status INTEGER,
        adjustedInSAP INTEGER, (bool value only)
        carrierId INTEGER,
        carrierTracking TEXT
     */

    /// <summary>
    /// Data structure containing info for a single pick order.
    /// </summary>
    public class PickOrder
    {
        public int OrderId;
        public int OrderType;
        public int CreatedById;
        public DateTime DateCreated;
        public DateTime DateModified;
        public int ShipToId;
        public int InventoryAdjustId;
        public PickOrderStatuses Status;
        public bool AdjustedInSAP;
        public int CarrierId;
        public string CarrierTracking;
    }

    /*
        pickItems table
     
        pickItemId INTEGER PRIMARY KEY AUTOINCREMENT,
        orderId INTEGER,
        dateModified TEXT,
        status INTEGER,
        matId TEXT,
        requestedQty REAL,
        pickedQty REAL,
        pickedBy INTEGER,
        notes TEXT
     */
    /// <summary>
    /// Data structure containing info for a list of items to be picked for an order.
    /// </summary>
    public class PickListItem
    {
        public int PickItemId;
        public readonly int OrderId;
        public DateTime DateModified;
        public PickItemStatuses Status;
        public string MaterialId;
        public decimal RequestedQty;
        public decimal PickedQty;
        public int PickedById;
        public string Notes;
        
        public PickListItem(int orderId)
        {
            OrderId = orderId;
            PickedQty = -1;
        }
    }
}
