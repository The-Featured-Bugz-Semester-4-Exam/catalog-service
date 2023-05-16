﻿using System;


namespace catalogServiceAPI.Models
{
    public class Item
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }

        public int ItemStartPrice { get; set; }

        public int ItemSellerID { get; set; }
        public DateTime ItemStartDate { get; set; }
        public DateTime ItemEndDate { get; set; }


        //Nice to have, billeder.
        //public int ItemPictureID { get; set; }

        /*
        public Item(int taxabookingID, DateTime bookingTime, string kundeNavn, string kundeTlf, string adresseStart, string adressedDestination, DateTime startTidspunkt)
        {
            this.TaxabookingID = taxabookingID;
            this.BookingTime = bookingTime;
            this.KundeNavn = kundeNavn;
            this.KundeTlf = kundeTlf;
            this.AdresseStart = adresseStart;
            this.AdresseDestination = adressedDestination;
            this.StartTidspunkt = startTidspunkt;
        }
        */
    }
}
