using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Utf8;

namespace JsonBenchmarks
{
    internal static class DataGenerator
    {
        internal static T Generate<T>()
        {
            if (typeof(T) == typeof(LoginViewModel))
                return (T)(object)CreateLoginViewModel();
            if (typeof(T) == typeof(Location))
                return (T)(object)CreateLocation();
            if (typeof(T) == typeof(IndexViewModel))
                return (T)(object)CreateIndexViewModel();
            if (typeof(T) == typeof(MyEventsListerViewModel))
                return (T)(object)CreateMyEventsListerViewModel();

            throw new NotImplementedException();
        }

        private static LoginViewModel CreateLoginViewModel()
            => new LoginViewModel
            {
                Email = (Utf8String)"name.familyname@not.com",
                Email1 = (Utf8String)"1name.familyname@not.com",
                Email2 = (Utf8String)"2name.familyname@not.com",
                Email3 = (Utf8String)"3name.familyname@not.com",
                Email4 = (Utf8String)"4name.familyname@not.com",
                Email5 = (Utf8String)"5name.familyname@not.com",
                Email6 = (Utf8String)"6name.familyname@not.com",
                Email7 = (Utf8String)"7name.familyname@not.com",
                Email8 = (Utf8String)"8name.familyname@not.com",
                Email9 = (Utf8String)"9name.familyname@not.com",
                Email10 = (Utf8String)"10name.familyname@not.com",
                Email11 = (Utf8String)"11name.familyname@not.com",
                Email12 = (Utf8String)"12name.familyname@not.com",
                Email13 = (Utf8String)"13name.familyname@not.com",
                Email14 = (Utf8String)"14name.familyname@not.com",
                Email15 = (Utf8String)"15name.familyname@not.com",
                Email16 = (Utf8String)"16name.familyname@not.com",
                Email17 = (Utf8String)"17name.familyname@not.com",
                Email18 = (Utf8String)"18name.familyname@not.com",
                Email19 = (Utf8String)"19name.familyname@not.com",
                Email20 = (Utf8String)"20name.familyname@not.com",
                Email21 = (Utf8String)"21name.familyname@not.com",
                Password = "abcdefgh123456!@",
                RememberMe = true
            };

        private static Location CreateLocation()
            => new Location
            {
                Id = 1234,
                Address1 = "The Street Name",
                Address2 = "20/11",
                City = "The City",
                State = "The State",
                PostalCode = "abc-12",
                Name = "Nonexisting",
                PhoneNumber = "+0 11 222 333 44",
                Country = "The Greatest"
            };

        private static IndexViewModel CreateIndexViewModel()
            => new IndexViewModel
            {
                IsNewAccount = false,
                FeaturedCampaign = new CampaignSummaryViewModel
                {
                    Description = "Very nice campaing",
                    Headline = "The Headline",
                    Id = 234235,
                    OrganizationName = "The Company XYZ",
                    ImageUrl = "https://www.dotnetfoundation.org/theme/img/carousel/foundation-diagram-content.png",
                    Title = "Promoting Open Source"
                },
                ActiveOrUpcomingEvents = Enumerable.Repeat(
                    new ActiveOrUpcomingEvent
                    {
                        Id = 10,
                        CampaignManagedOrganizerName = "Name FamiltyName",
                        CampaignName = "The very new campaing",
                        Description = "The .NET Foundation works with Microsoft and the broader industry to increase the exposure of open source projects in the .NET community and the .NET Foundation. The .NET Foundation provides access to these resources to projects and looks to promote the activities of our communities.",
                        EndDate = DateTime.UtcNow.AddYears(1),
                        Name = "Just a name",
                        ImageUrl = "https://www.dotnetfoundation.org/theme/img/carousel/foundation-diagram-content.png",
                        StartDate = DateTime.UtcNow
                    },
                    count: 20).ToList()
            };

        private static MyEventsListerViewModel CreateMyEventsListerViewModel()
            => new MyEventsListerViewModel
            {
                CurrentEvents = Enumerable.Repeat(CreateMyEventsListerItem(), 3).ToList(),
                FutureEvents = Enumerable.Repeat(CreateMyEventsListerItem(), 9).ToList(),
                PastEvents = Enumerable.Repeat(CreateMyEventsListerItem(), 60).ToList() // usually  there is a lot of historical data
            };

        private static MyEventsListerItem CreateMyEventsListerItem()
            => new MyEventsListerItem
            {
                Campaign = "A very nice campaing",
                EndDate = DateTime.UtcNow.AddDays(7),
                EventId = 321,
                EventName = "wonderful name",
                Organization = "Local Animal Shelter",
                StartDate = DateTime.UtcNow.AddDays(-7),
                TimeZone = TimeZoneInfo.Utc.DisplayName,
                VolunteerCount = 15,
                Tasks = Enumerable.Repeat(
                    new MyEventsListerItemTask
                    {
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddDays(1),
                        Name = "A very nice task to have"
                    }, 4).ToList()
            };
    }

    /// <summary>
    /// ZeroFormatter requires all properties to be virtual
    /// they are deserialized for real when they are used for the first time
    /// if we don't touch the properites, they are not being deserialized and the result is skewed
    /// </summary>
    public interface IVerifiable
    {
        long TouchEveryProperty();
    }

    // the view models come from a real world app called "AllReady"
    [Serializable]
    public class LoginViewModel : IVerifiable
    {
        public virtual Utf8String Email { get; set; }
        public virtual Utf8String Email1 { get; set; }
        public virtual Utf8String Email2 { get; set; }
        public virtual Utf8String Email3 { get; set; }
        public virtual Utf8String Email4 { get; set; }
        public virtual Utf8String Email5 { get; set; }
        public virtual Utf8String Email6 { get; set; }
        public virtual Utf8String Email7 { get; set; }
        public virtual Utf8String Email8 { get; set; }
        public virtual Utf8String Email9 { get; set; }
        public virtual Utf8String Email10 { get; set; }
        public virtual Utf8String Email11 { get; set; }
        public virtual Utf8String Email12 { get; set; }
        public virtual Utf8String Email13 { get; set; }
        public virtual Utf8String Email14 { get; set; }
        public virtual Utf8String Email15 { get; set; }
        public virtual Utf8String Email16 { get; set; }
        public virtual Utf8String Email17 { get; set; }
        public virtual Utf8String Email18 { get; set; }
        public virtual Utf8String Email19 { get; set; }
        public virtual Utf8String Email20 { get; set; }
        public virtual Utf8String Email21 { get; set; }

        public virtual string Password { get; set; }
        public virtual bool RememberMe { get; set; }

        public long TouchEveryProperty() => Email.ToString().Length + Password.Length + Convert.ToInt32(RememberMe);
    }

    [Serializable]
    public class Location : IVerifiable
    {
        public virtual int Id { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string Name { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Country { get; set; }

        public long TouchEveryProperty() => Id + Address1.Length + Address2.Length + City.Length + State.Length + PostalCode.Length + Name.Length + PhoneNumber.Length + Country.Length;
    }

    [Serializable]
    public class ActiveOrUpcomingCampaign : IVerifiable
    {
        public virtual int Id { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTimeOffset StartDate { get; set; }
        public virtual DateTimeOffset EndDate { get; set; }

        public long TouchEveryProperty() => Id + ImageUrl.Length + Name.Length + Description.Length + StartDate.Ticks + EndDate.Ticks;
    }

    [Serializable]
    public class ActiveOrUpcomingEvent : IVerifiable
    {
        public virtual int Id { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string Name { get; set; }
        public virtual string CampaignName { get; set; }
        public virtual string CampaignManagedOrganizerName { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTimeOffset StartDate { get; set; }
        public virtual DateTimeOffset EndDate { get; set; }

        public long TouchEveryProperty() => Id + ImageUrl.Length + Name.Length + CampaignName.Length + CampaignManagedOrganizerName.Length + Description.Length + StartDate.Ticks + EndDate.Ticks;
    }

    [Serializable]
    public class CampaignSummaryViewModel : IVerifiable
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string ImageUrl { get; set; }
        public virtual string OrganizationName { get; set; }
        public virtual string Headline { get; set; }

        public long TouchEveryProperty() => Id + Title.Length + Description.Length + ImageUrl.Length + OrganizationName.Length + Headline.Length;
    }

    [Serializable]
    public class IndexViewModel : IVerifiable
    {
        public virtual List<ActiveOrUpcomingEvent> ActiveOrUpcomingEvents { get; set; }
        public virtual CampaignSummaryViewModel FeaturedCampaign { get; set; }
        public virtual bool IsNewAccount { get; set; }
        public bool HasFeaturedCampaign => FeaturedCampaign != null;

        public long TouchEveryProperty()
        {
            long result = FeaturedCampaign.TouchEveryProperty() + Convert.ToInt32(IsNewAccount);

            for (int i = 0; i < ActiveOrUpcomingEvents.Count; i++) // no LINQ here to prevent from skewing allocations results
                result += ActiveOrUpcomingEvents[i].TouchEveryProperty();

            return result;
        }
    }

    [Serializable]
    public class MyEventsListerViewModel : IVerifiable
    {
        // the orginal type defined these fields as IEnumerable,
        // but XmlSerializer failed to serialize them with "cannot serialize member because it is an interface" error
        public virtual List<MyEventsListerItem> CurrentEvents { get; set; } = new List<MyEventsListerItem>();
        public virtual List<MyEventsListerItem> FutureEvents { get; set; } = new List<MyEventsListerItem>();
        public virtual List<MyEventsListerItem> PastEvents { get; set; } = new List<MyEventsListerItem>();

        public long TouchEveryProperty()
        {
            long result = 0;

            // no LINQ here to prevent from skewing allocations results
            for (int i = 0; i < CurrentEvents.Count; i++) result += CurrentEvents[i].TouchEveryProperty();
            for (int i = 0; i < FutureEvents.Count; i++) result += FutureEvents[i].TouchEveryProperty();
            for (int i = 0; i < PastEvents.Count; i++) result += PastEvents[i].TouchEveryProperty();

            return result;
        }
    }

    [Serializable]
    public class MyEventsListerItem : IVerifiable
    {
        public virtual int EventId { get; set; }
        public virtual string EventName { get; set; }
        public virtual DateTimeOffset StartDate { get; set; }
        public virtual DateTimeOffset EndDate { get; set; }
        public virtual string TimeZone { get; set; }
        public virtual string Campaign { get; set; }
        public virtual string Organization { get; set; }
        public virtual int VolunteerCount { get; set; }

        public virtual List<MyEventsListerItemTask> Tasks { get; set; } = new List<MyEventsListerItemTask>();

        public long TouchEveryProperty()
        {
            long result = EventId + EventName.Length + StartDate.Ticks + EndDate.Ticks + TimeZone.Length + Campaign.Length + Organization.Length + VolunteerCount;

            for (int i = 0; i < Tasks.Count; i++) // no LINQ here to prevent from skewing allocations results
                result += Tasks[i].TouchEveryProperty();

            return result;
        }
    }

    [Serializable]
    public class MyEventsListerItemTask : IVerifiable
    {
        public virtual string Name { get; set; }
        public virtual DateTimeOffset? StartDate { get; set; }
        public virtual DateTimeOffset? EndDate { get; set; }

        public string FormattedDate
        {
            get
            {
                if (!StartDate.HasValue || !EndDate.HasValue)
                {
                    return null;
                }

                var startDateString = string.Format("{0:g}", StartDate.Value);
                var endDateString = string.Format("{0:g}", EndDate.Value);

                return string.Format($"From {startDateString} to {endDateString}");
            }
        }

        public long TouchEveryProperty() => Name.Length + StartDate.Value.Ticks + EndDate.Value.Ticks;
    }
}
