// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace System.IO.Pipelines.Samples.Models
{
    public static class BigModels
    {
        public static readonly Model About100Fields = new Model()
        {
            CreatedDate = DateTime.Now,
            UpdateBy = Guid.NewGuid().ToString(),
            UpdatedDate = DateTime.Now,

            Provider = new HealthCareProvider()
            {
                CreatedDate = DateTime.Now,
                Name = Guid.NewGuid().ToString(),
                UpdateBy = Guid.NewGuid().ToString(),
                UpdatedDate = DateTime.Now,

                Services = new List<Service>()
                {
                    new Service()
                    {
                        Code = "abcd",
                        CreatedDate = DateTime.Now,
                        Name = Guid.NewGuid().ToString(),
                        SubName = Guid.NewGuid().ToString(),
                        UpdateBy = Guid.NewGuid().ToString(),
                        UpdatedDate = DateTime.Now,
                    },
                    new Service()
                    {
                        Code = "abcd",
                        CreatedDate = DateTime.Now,
                        Name = Guid.NewGuid().ToString(),
                        SubName = Guid.NewGuid().ToString(),
                        UpdateBy = Guid.NewGuid().ToString(),
                        UpdatedDate = DateTime.Now,
                    },
                    new Service()
                    {
                        Code = "abcd",
                        CreatedDate = DateTime.Now,
                        Name = Guid.NewGuid().ToString(),
                        SubName = Guid.NewGuid().ToString(),
                        UpdateBy = Guid.NewGuid().ToString(),
                        UpdatedDate = DateTime.Now,
                    }
                }
            },
            Enrollment = new Enrollment()
            {
                CreatedDate = DateTime.Now,
                UpdateBy = Guid.NewGuid().ToString(),
                UpdatedDate = DateTime.Now,

                Client = new Client()
                {
                    CreatedDate = DateTime.Now,
                    UpdateBy = Guid.NewGuid().ToString(),
                    UpdatedDate = DateTime.Now,

                    HomeAddress = new Address()
                    {
                        City = "Seattle",
                        State = "WA",
                        Street1 = Guid.NewGuid().ToString(),
                        Zip = 98004,
                    },
                },
                Program = new Program()
                {
                    CreatedDate = DateTime.Now,
                    UpdateBy = Guid.NewGuid().ToString(),
                    UpdatedDate = DateTime.Now,

                    Services = new List<Service>()
                    {
                        new Service()
                        {
                            Code = "abcd",
                            CreatedDate = DateTime.Now,
                            Name = Guid.NewGuid().ToString(),
                            SubName = Guid.NewGuid().ToString(),
                            UpdateBy = Guid.NewGuid().ToString(),
                            UpdatedDate = DateTime.Now,
                        },
                        new Service()
                        {
                            Code = "abcd",
                            CreatedDate = DateTime.Now,
                            Name = Guid.NewGuid().ToString(),
                            SubName = Guid.NewGuid().ToString(),
                            UpdateBy = Guid.NewGuid().ToString(),
                            UpdatedDate = DateTime.Now,
                        },
                        new Service()
                        {
                            Code = "abcd",
                            CreatedDate = DateTime.Now,
                            Name = Guid.NewGuid().ToString(),
                            SubName = Guid.NewGuid().ToString(),
                            UpdateBy = Guid.NewGuid().ToString(),
                            UpdatedDate = DateTime.Now,
                        }
                    }
                },
            },
            Service = new Service()
            {
                Code = "abcd",
                CreatedDate = DateTime.Now,
                Name = Guid.NewGuid().ToString(),
                SubName = Guid.NewGuid().ToString(),
                UpdateBy = Guid.NewGuid().ToString(),
                UpdatedDate = DateTime.Now,
            },
        };
    }

    public abstract class BaseEntity<TId>
    {
        public abstract TId Id { get; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdateBy { get; set; }

        public virtual bool IsTransient()
        {
            return Id.Equals(default(TId));
        }
    }

    public class Model : BaseEntity<int>
    {
        public override int Id => EnrollmentServiceId;
        public int EnrollmentServiceId { get; set; }
        public int EnrollmentId { get; set; }
        public int ServiceId { get; set; }
        public int ProviderId { get; set; }
        public virtual Enrollment Enrollment { get; set; }
        public virtual Service Service { get; set; }
        public virtual HealthCareProvider Provider { get; set; }
    }

    public class Enrollment : BaseEntity<int>
    {
        public override int Id => EnrollmentId;
        public int EnrollmentId { get; set; }
        public int ClientId { get; set; }
        public virtual User PrimaryCaseManager { get; set; }
        public virtual User SecondaryCaseManager { get; set; }
        public int ProgramId { get; set; }
        public virtual Program Program { get; set; }
        public virtual Client Client { get; set; }
        public virtual ICollection<Model> EnrollmentServices { get; set; }
        public virtual ICollection<Doc> Documents { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }

    public class Service : BaseEntity<int>
    {
        public override int Id => ServiceId;
        public int ServiceId { get; set; }
        public int ProgramId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }
        public ICollection<HealthCareProvider> Providers { get; set; }
        public Program Program { get; set; }
    }

    public class HealthCareProvider : BaseEntity<int>
    {
        public override int Id => ProviderId;
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }

    public class Client : BaseEntity<int>
    {
        public override int Id => ClientId;
        public int ClientId { get; set; }
        public virtual Address HomeAddress { get; set; }
        public virtual ICollection<Doc> Docs { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<ClientCareSetting> CareSettings { get; set; }
    }

    public class Program : BaseEntity<int>
    {
        public override int Id => ProgramId;
        public int ProgramId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }

    public class Note : BaseEntity<int>
    {
        public override int Id => NoteId;
        public int NoteId { get; set; }
        public string Content { get; set; }
    }

    public class Doc : BaseEntity<int>
    {
        public override int Id => DocId;
        public int DocId { get; set; }
        public string Filename { get; set; }
    }

    public class Contact : BaseEntity<int>
    {
        public override int Id => ContactId;
        public int ContactId { get; set; }
    }

    public class User : BaseEntity<int>
    {
        public override int Id => UserId;
        public int UserId { get; set; }
    }

    public class ClientCareSetting : BaseEntity<int>
    {
        public override int Id => ClientCareSettingId;
        public int ClientCareSettingId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class Address
    {
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public int Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
