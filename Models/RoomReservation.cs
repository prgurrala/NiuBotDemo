using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiDialogsBot.Models
{
    public enum BedSizeOptions
    {
        King, Queen, Single, Double
    }

    public enum AmenitiesOptions
    {
        Kitchen, 
        ExtraTowels,
        Gym
    }
    [Serializable]
    public class RoomReservation
    {
        public BedSizeOptions? BedSize;
        public int? NumberOfOccupants;
        public DateTime? CheckInDate;
        public int? NumberOfDaysToStay;
        public IList<AmenitiesOptions> Amenities;

        public static IForm<RoomReservation> BuildForm()
        {
            return new FormBuilder<RoomReservation>()
                .Message("Welcome to Hotel Bot")
                .Build();
        }
    }
}