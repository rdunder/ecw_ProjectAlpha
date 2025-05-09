﻿using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;
public class UserAddressRepository(AppDbContext context) :
    BaseRepository<UserAddressEntity>(context), IUserAddressRepository
{
}
