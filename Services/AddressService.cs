using Microsoft.Extensions.Logging;
using P21_latest_template.Models;
using P21_latest_template.Repositories;
using System;

namespace P21_latest_template.Services
{
    public class AddressService : IAddressService
    {
        protected readonly IDbConnectionService DbConn;
        protected readonly ILogger Logger;
        protected readonly IAddressRepository AddressRepository;

        public AddressService(IDbConnectionService dbConn,
            ILoggerFactory loggerFactory,
            IAddressRepository addressRepo)
        {
            DbConn = dbConn;
            Logger = loggerFactory.CreateLogger<AddressService>();
            AddressRepository = addressRepo;
        }

        public AddressCreate Update(AddressCreate model)
        {
            var entity = AddressRepository.Get(model.id);

            if (entity == null)
                return null;

            entity.name = model.name;
            entity.mail_address1 = model.mail_address1;
            entity.mail_address2 = model.mail_address2;
            entity.mail_city = model.mail_city;
            entity.mail_state = model.mail_state;
            entity.mail_postal_code = model.mail_postal_code;
            entity.mail_country = model.mail_country;
            entity.phys_address1 = model.phys_address1;
            entity.phys_address2 = model.phys_address2;
            entity.phys_city = model.phys_city;
            entity.phys_state = model.phys_state;
            entity.phys_postal_code = model.phys_postal_code;
            entity.phys_country = model.phys_country;
            entity.central_phone_number = model.central_phone_number;
            entity.date_last_modified = DateTime.Now;
            entity.last_maintained_by = "admin";

            var updated = AddressRepository.Update(entity);

            return updated ? model : null;
        }
    }
}
